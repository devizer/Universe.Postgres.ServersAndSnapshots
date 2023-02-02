function get_darwin_version_major() {
  if [[ "$(uname -s)" == Darwin ]]; then
    echo "$(sysctl -n kern.osrelease | awk -F'.' '{print $1}')"
  fi
}

echo "get_darwin_version_major is [$(get_darwin_version_major)]"
v="$(get_darwin_version_major)"
case $v in
  22) macports_pkg="https://github.com/macports/macports-base/releases/download/v2.8.1/MacPorts-2.8.1-13-Ventura.pkg";;
  21) macports_pkg="https://github.com/macports/macports-base/releases/download/v2.8.1/MacPorts-2.8.1-12-Monterey.pkg";;
  20) macports_pkg="https://github.com/macports/macports-base/releases/download/v2.8.1/MacPorts-2.8.1-11-BigSur.pkg";;
  19) macports_pkg="https://github.com/macports/macports-base/releases/download/v2.8.1/MacPorts-2.8.1-10.15-Catalina.pkg";;
  18) macports_pkg="https://github.com/macports/macports-base/releases/download/v2.8.1/MacPorts-2.8.1-10.14-Mojave.pkg";;
  17) macports_pkg="https://github.com/macports/macports-base/releases/download/v2.8.1/MacPorts-2.8.1-10.13-HighSierra.pkg";;
  16) macports_pkg="https://github.com/macports/macports-base/releases/download/v2.8.1/MacPorts-2.8.1-10.12-Sierra.pkg";;
  15) macports_pkg="https://github.com/macports/macports-base/releases/download/v2.8.1/MacPorts-2.8.1-10.11-ElCapitan.pkg";;
  14) macports_pkg="https://github.com/macports/macports-base/releases/download/v2.8.1/MacPorts-2.8.1-10.10-Yosemite.pkg";;
  13) macports_pkg="https://github.com/macports/macports-base/releases/download/v2.8.1/MacPorts-2.8.1-10.9-Mavericks.pkg";;
  12) macports_pkg="https://github.com/macports/macports-base/releases/download/v2.8.1/MacPorts-2.8.1-10.8-MountainLion.pkg";;
  11) macports_pkg="https://github.com/macports/macports-base/releases/download/v2.8.1/MacPorts-2.8.1-10.7-Lion.pkg";;
  10) macports_pkg="https://github.com/macports/macports-base/releases/download/v2.8.1/MacPorts-2.8.1-10.6-SnowLeopard.pkg";;
  *)  macports_pkg="";;
esac
echo "macports_pkg=[$macports_pkg]"

curl -kSL -o /tmp/MacPorts.pkg "$macports_pkg" || curl -kSL -o /tmp/MacPorts.pkg "$macports_pkg"
time sudo installer -pkg /tmp/MacPorts.pkg -target / # -verbose
rm -f /tmp/MacPorts.pkg || true
export PATH="/opt/local/bin:/opt/local/sbin:$PATH"
