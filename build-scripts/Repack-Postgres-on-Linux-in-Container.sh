set -e; set -u; set -o pipefail

echo '
Acquire::AllowReleaseInfoChange::Suite "true";
Acquire::Check-Valid-Until "0";
APT::Get::Assume-Yes "true";
APT::Get::AllowUnauthenticated "true";
Acquire::AllowInsecureRepositories "1";
Acquire::AllowDowngradeToInsecureRepositories "1";
' | tee /etc/apt/apt.conf.d/98_Z_Custom

mkdir -p /Artifacts
Say "Bootstrap docker container"
try-and-retry apt-get update -y -qq
try-and-retry apt-get install curl ca-certificates gnupg lsb-release tree -y -qq

echo "deb http://apt.postgresql.org/pub/repos/apt $(lsb_release -cs)-pgdg main" | tee /etc/apt/sources.list.d/pgdg.list
apt-get update -qq
apt-cache search postgres | grep -E '^postgres' | sort | tee /Artifacts/apt-postgres-packages.txt
apt-cache policy postgresql-14
err=0
time apt-get install -y -qq postgresql-14 |& tee /Artifacts/postgres-14-install-log.txt || err=1





