using System.Data;
using Dapper;

namespace Universe.NpglExtensions
{
    public static class NpgsqlLocaleExtensions
    {
        public class DbLocale
        {
            public string DbName { get; set; }
            public string Collate { get; set; }
            public string CType { get; set; }

            public override string ToString()
            {
                return $"{nameof(DbName)}='{DbName}', {nameof(Collate)}='{Collate}', {nameof(CType)}='{CType}'";
            }
        }

        public static DbLocale GetCurrentDatabaseLocale(this IDbConnection connection)
        {
            const string sql = "Select datname As DbName, datcollate As Collate, datctype As CType FROM pg_database where datname = current_database();";
            return connection.QueryFirst<DbLocale>(sql);
        }
    }
}