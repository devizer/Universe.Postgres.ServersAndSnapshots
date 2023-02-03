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

mkdir -p /Artifacts/Debug
cat /etc/*release > /Artifacts/Debug/OS-RELEASE.txt

Say "Creating '[Before] /usr snapshot' artifact ...."
time cp -a /usr "/Artifacts/[Before] usr"

Say "Bootstrap docker container [$(hostname)]"
try-and-retry apt-get update -y -qq
try-and-retry apt-get install curl ca-certificates gnupg lsb-release tree locales sudo procps -y -qq | grep Unpack

printf "en_US.UTF-8 UTF-8\nde_DE.UTF8 UTF-8\n" | tee /etc/locale.gen > /dev/null; DEBIAN_FRONTEND=noninteractive dpkg-reconfigure locales

Say "Configure postgres apt repo [$(hostname)]"
echo "deb http://apt.postgresql.org/pub/repos/apt $(lsb_release -cs)-pgdg main" | tee /etc/apt/sources.list.d/pgdg.list
apt-get update -qq
apt-cache search postgres | grep -E '^postgres' | sort | tee /Artifacts/Debug/apt-postgres-packages.txt
apt-cache policy postgresql-14

Say "Checking postgres versions"
for v in 9.6 10 11 12 13 14 15 16; do
  f="/Artifacts/Debug/POSTGRES VERSIONS.txt"
  echo "Try Version [$v]" | tee -a "$f"
  apt-cache policy "postgresql-$v" 2>&1 | tee -a "$f"
  echo "" | tee -a "$f"
done


Say "Installing postgresql-14"
err=0
time apt-get install -y -qq postgresql-14 postgresql-server-dev-14 postgresql-pltcl-14 postgresql-14-cron postgresql-14-orafce postgresql-14-pg-stat-kcache |& tee /Artifacts/Debug/postgres-14-install-log.txt || err=1
time apt-get install -y -qq postgresql-15 postgresql-server-dev-15 postgresql-pltcl-15 postgresql-15-cron postgresql-15-orafce postgresql-15-pg-stat-kcache |& tee /Artifacts/Debug/postgres-15-install-log.txt || err=2
time apt-get install -y -qq postgresql-13 postgresql-server-dev-13 postgresql-pltcl-13 postgresql-13-cron postgresql-13-orafce postgresql-13-pg-stat-kcache |& tee /Artifacts/Debug/postgres-13-install-log.txt || err=3
Say "ERROR = [$err]"


Say "Starting v15"
mkdir -p /var/pg-15
sudo chown -R postgres /var/pg-15
sudo -u postgres /usr/lib/postgresql/15/bin/initdb -D /var/pg-15 || true
sudo chown -R postgres /var/pg-15
pushd /var/pg-15
sudo -u postgres /usr/lib/postgresql/15/bin/pg_ctl -w -D /var/pg-15 start || true
popd

Say "Starting v14"
mkdir -p /var/pg-14
sudo chown -R postgres /var/pg-14
sudo -u postgres /usr/lib/postgresql/14/bin/initdb -D /var/pg-14 || true
echo "
port = 5433" >> /var/pg-14/postgresql.conf
sudo chown -R postgres /var/pg-14
pushd /var/pg-14
sudo -u postgres /usr/lib/postgresql/14/bin/pg_ctl -w -D /var/pg-14 start || true

Say "Starting v13"
mkdir -p /var/pg-13
sudo chown -R postgres /var/pg-13
sudo -u postgres /usr/lib/postgresql/13/bin/initdb -D /var/pg-13 || true
echo "
port = 5434" >> /var/pg-13/postgresql.conf
sudo chown -R postgres /var/pg-13
pushd /var/pg-13
sudo -u postgres /usr/lib/postgresql/13/bin/pg_ctl -w -D /var/pg-13 start || true


ps aux |& tee "/Artifacts/Debug/Process after install of postres.txt"

mkdir -p /Artifacts/PostgreSQL
cp -a /usr/lib/postgresql /Artifacts/PostgreSQL

Say "Creating '[After] /usr snapshot' artifact ...."
time cp -a /usr "/Artifacts/[After] usr"

Say "Creating '[After] /var snapshot' artifact ...."
time cp -a /var "/Artifacts/[After] var"

Say "Grab users"
cat /etc/passwd | tee /Artifacts/Debug/passwd.txt
