set -e; set -u; set -o pipefail
cat /etc/*release
export DEBIAN_FRONTEND=noninteractive
echo '
Acquire::AllowReleaseInfoChange::Suite "true";
Acquire::Check-Valid-Until "0";
APT::Get::Assume-Yes "true";
APT::Get::AllowUnauthenticated "true";
Acquire::AllowInsecureRepositories "1";
Acquire::AllowDowngradeToInsecureRepositories "1";
' > /etc/apt/apt.conf.d/98_Z_Custom

mkdir -p /Artifacts
Say "[Before] /usr snapshot"
time cp -a /usr "/Artifacts/[Before] usr"

Say "Bootstrap docker container [$(hostname)]"
try-and-retry apt-get update -y -qq
try-and-retry apt-get install curl ca-certificates gnupg lsb-release tree locales sudo -y -qq | grep Unpack

printf "en_US.UTF-8 UTF-8\nde_DE.UTF8 UTF-8\n" | tee /etc/locale.gen > /dev/null; DEBIAN_FRONTEND=noninteractive dpkg-reconfigure locales

Say "Configure postgres apt repo [$(hostname)]"
echo "deb http://apt.postgresql.org/pub/repos/apt $(lsb_release -cs)-pgdg main" | tee /etc/apt/sources.list.d/pgdg.list
apt-get update -qq
apt-cache search postgres | grep -E '^postgres' | sort | tee /Artifacts/apt-postgres-packages.txt
apt-cache policy postgresql-14
Say "Installing postgresql-14"
err=0
time apt-get install -y -qq postgresql-14 postgresql-server-dev-14 postgresql-pltcl-14 postgresql-14-cron postgresql-14-orafce postgresql-14-pg-stat-kcache |& tee /Artifacts/postgres-14-install-log.txt || err=1

ps aux |& tee "/Artifacts/Process after install of postres.txt"

Say "[After] /usr snapshot"
time cp -a /usr "/Artifacts/[After] usr"
