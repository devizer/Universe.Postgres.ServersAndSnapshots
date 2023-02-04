set -e; set -u; set -o pipefail

SYSTEM_ARTIFACTSDIRECTORY="${SYSTEM_ARTIFACTSDIRECTORY:-/transient-builds}"
export LC_ALL=en_US.utf8

script=https://raw.githubusercontent.com/devizer/test-and-build/master/install-build-tools-bundle.sh; (wget -q -nv --no-check-certificate -O - $script 2>/dev/null || curl -ksSL $script) | TARGET_DIR=/usr/local/bin bash > /dev/null
Say --Reset-Stopwatch

cpu="$(cat /proc/cpuinfo | grep -E '^(model name|Hardware)' | awk -F':' 'NR==1 {print $2}')"; cpu="$(echo -e "${cpu:-}" | sed -e 's/^[[:space:]]*//')"
Say "CPU: ${cpu:-}, $(nproc) Cores"

smart-apt-install rsync pv sshpass jq qemu-user-static -y -qq >/dev/null

Say "Registering binary formats for qemu-user-static"
try-and-retry docker pull -q multiarch/qemu-user-static:register
try-and-retry docker run --rm --privileged multiarch/qemu-user-static:register --reset >/dev/null
# docker buildx imagetools inspect --raw "$image" | jq

function Build-Image() 
{
    Say "Start container [$IMAGE] for [$KEY]"
    tmp=/tmp/proot-$KEY
    mkdir -p $tmp; rm -rf $tmp/*
    docker run -d --privileged --hostname "container-$KEY" --name "container-$KEY" -v /usr/bin/qemu-arm-static:/usr/bin/qemu-arm-static -v /usr/bin/qemu-aarch64-static:/usr/bin/qemu-aarch64-static "$IMAGE" bash -c 'while [ 1 -eq 1 ] ; do echo ...; sleep 1; done'
    for cmd in Say try-and-retry; do
      docker cp /usr/local/bin/$cmd "container-$KEY":/usr/local/bin/$cmd
    done
    docker cp $(pwd)/Repack-Postgres-on-Linux-in-Container.sh "container-$KEY":/tmp/Repack-Postgres-on-Linux-in-Container.sh
    err=0
    docker exec -t "container-$KEY" bash /tmp/Repack-Postgres-on-Linux-in-Container.sh || err=1
    suffix="ok"; if [[ $err != 0 ]]; then Say "ERRRRRRRRRRRRRRRROR"; suffix="with-errors"; fi
    mkdir -p /tmp/$KEY-plain
    docker cp "container-$KEY":/Artifacts/ /tmp/$KEY-plain
    pushd /tmp/$KEY-plain/Artifacts
      for d in *; do if [ -d "$d" ]; then
        Say "[$KEY] Pack '$d'"
        pushd "$d" >/dev/null
        if [[ "$d" == *"PostgreSQL"* ]] && [[ "$suffix" == "ok" ]]; then
          tar cf - . | xz -9 -e > "$SYSTEM_ARTIFACTSDIRECTORY/$KEY-$d.tar.xz"
        else
          7z a -mmt=$(nproc) -mx=1 -ms=on -mqs=on "$SYSTEM_ARTIFACTSDIRECTORY/$KEY-$suffix $d.7z" . | { grep "Archive\|Everything" || true; }
        fi
        popd >/dev/null
      fi; done
    popd
    Say "Clean up $KEY"
    rm -rf /tmp/$KEY-plain
    docker rm -f "container-$KEY"
}
# arm32v7 IS NOT SUPPORTED
IMAGE="arm64v8/debian:11" KEY=debian-11-aarch64  Build-Image
IMAGE="ubuntu:20.04"      KEY=ubuntu-2004-x86_64 Build-Image
exit 0;
IMAGE="i386/debian:10"    KEY=debian-10-i386     Build-Image
IMAGE="arm32v7/debian:11" KEY=debian-11-arm32v7  Build-Image

# IMAGE="debian:testing"    KEY=debian-12-x86_64   Build-Image
# IMAGE="debian:10"    KEY=debian-10-x86_64   Build-Image
IMAGE="debian:11"    KEY=debian-11-x86_64   Build-Image
# IMAGE="ubuntu:18.04" KEY=ubuntu-1804-x86_64 Build-Image
# IMAGE="ubuntu:22.04" KEY=ubuntu-2204-x86_64 Build-Image
# IMAGE="ubuntu:22.10" KEY=ubuntu-2210-x86_64 Build-Image
