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
try-and-retry apt-get update -qq
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
time try-and-retry apt-get install -y -qq postgresql-14 postgresql-server-dev-14 postgresql-pltcl-14 postgresql-14-cron postgresql-14-orafce postgresql-14-pg-stat-kcache |& tee /Artifacts/Debug/postgres-14-install-log.txt || err=1
time try-and-retry apt-get install -y -qq postgresql-15 postgresql-server-dev-15 postgresql-pltcl-15 postgresql-15-cron postgresql-15-orafce postgresql-15-pg-stat-kcache |& tee /Artifacts/Debug/postgres-15-install-log.txt || err=2
time try-and-retry apt-get install -y -qq postgresql-13 postgresql-server-dev-13 postgresql-pltcl-13 postgresql-13-cron postgresql-13-orafce postgresql-13-pg-stat-kcache |& tee /Artifacts/Debug/postgres-13-install-log.txt || err=3
Say "ERROR = [$err]"

port=5433
for v in 9.6 10 11 12 13 14 15; do
  Say "Installing postgresql-$v"
  for package in postgresql-$v postgresql-server-dev-$v postgresql-pltcl-$v postgresql-$v-cron postgresql-$v-orafce postgresql-$v-pg-stat-kcache; do
    err=OK
    try-and-retry apt-get install -y -qq $package |& tee -a /Artifacts/Debug/postgres-$v-install-detailed-log.txt || err=FAIL
    echo "$err: v${v} ${package}" >> "/Artifacts/Debug/POSTGRES INSTALL RESULT.txt"
  done

  Say "Starting v$v"
  mkdir -p /var/pg-$v
  sudo chown -R postgres /var/pg-$v
  sudo -u postgres /usr/lib/postgresql/$v/bin/initdb -D /var/pg-$v || true
  echo "
port = $port" >> /var/pg-$v/postgresql.conf
  sudo chown -R postgres /var/pg-$v
  pushd /var/pg-$v
  start=OK
  sudo -u postgres /usr/lib/postgresql/$v/bin/pg_ctl -w -D /var/pg-$v start || start=FAIL
  echo "$start: Status of start for '$v'" | tee -a "/Artifacts/Debug/POSTGRES INSTALL RESULT.txt"
  port=$((port+1))
do

ps aux |& tee "/Artifacts/Debug/Postgres Processes (after install) of postres.txt"

mkdir -p /Artifacts/PostgreSQL
cp -a /usr/lib/postgresql /Artifacts/PostgreSQL

Say "Creating '[After] /usr snapshot' artifact ...."
time cp -a /usr "/Artifacts/[After] usr"

Say "Creating '[After] /var snapshot' artifact ...."
time cp -a /var "/Artifacts/[After] var"

Say "Grab users"
cat /etc/passwd | tee /Artifacts/Debug/passwd.txt
