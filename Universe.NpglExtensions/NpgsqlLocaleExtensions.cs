using System.Collections.Generic;
using System.Data;
using System.Linq;
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
            public string Encoding { get; set; }

            public override string ToString()
            {
                return $"{nameof(DbName)}: {DbName}, {nameof(Collate)}: {Collate}, {nameof(CType)}: {CType}, {nameof(Encoding)}: {Encoding}";
            }
        }

        public static DbLocale GetCurrentDatabaseLocale(this IDbConnection connection)
        {
            const string sql = "Select datname As DbName, datcollate As Collate, datctype As CType, pg_encoding_to_char(encoding) As Encoding FROM pg_database where datname = current_database();";
            return connection.QueryFirst<DbLocale>(sql);
        }

        public static IEnumerable<DbLocale> GetDatabasesWithLocale(this IDbConnection connection)
        {
            const string sql = "Select datname As DbName, datcollate As Collate, datctype As CType, pg_encoding_to_char(encoding) As Encoding FROM pg_database where datname = current_database();";
            return connection.Query<DbLocale>(sql).ToArray();
        }

    }
}