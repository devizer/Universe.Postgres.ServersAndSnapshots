set -e; set -u; set -o pipefail
images="debian:10 debian:11 debian:12 ubuntu:18.04 ubuntu:20.04 ubuntu:22.04 ubuntu:22.10"


SYSTEM_ARTIFACTSDIRECTORY="${SYSTEM_ARTIFACTSDIRECTORY:-/transient-builds}"
export LC_ALL=en_US.utf8

sudo apt-get install rsync pv sshpass jq qemu-user-static -y -qq >/dev/null
script=https://raw.githubusercontent.com/devizer/test-and-build/master/install-build-tools-bundle.sh; (wget -q -nv --no-check-certificate -O - $script 2>/dev/null || curl -ksSL $script) | TARGET_DIR=/usr/local/bin bash > /dev/null
Say --Reset-Stopwatch
smart-apt-install rsync pv sshpass jq qemu-user-static -y -qq >/dev/null

Say "Registering binary formats for qemu-user-static"
docker run --rm --privileged multiarch/qemu-user-static:register --reset >/dev/null
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
    docker exec -t "container-$KEY" bash /tmp/Repack-Postgres-on-Linux-in-Container.sh || Say ERRRRRRRRRRRRRROR
    mkdir -p /tmp/$KEY-plain
    docker cp "container-$KEY":/Artifacts /tmp/$KEY-plain
    7z a $SYSTEM_ARTIFACTSDIRECTORY/$KEY.7z
}
IMAGE="debian:10"    KEY=debian-10-x86_64   Build-Image
IMAGE="debian:11"    KEY=debian-11-x86_64   Build-Image
IMAGE="debian:12"    KEY=debian-12-x86_64   Build-Image
IMAGE="ubuntu:18.04" KEY=ubuntu-1804-x86_64 Build-Image
IMAGE="ubuntu:20.04" KEY=ubuntu-2004-x86_64 Build-Image
IMAGE="ubuntu:22.04" KEY=ubuntu-2204-x86_64 Build-Image
IMAGE="ubuntu:22.10" KEY=ubuntu-2210-x86_64 Build-Image
