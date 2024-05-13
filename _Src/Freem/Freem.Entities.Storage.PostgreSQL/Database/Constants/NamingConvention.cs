using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freem.Entities.Storage.PostgreSQL.Database.Constants;

// Constraint naming convention from https://til.cybertec-postgresql.com/post/2019-09-02-Postgres-Constraint-Naming-Convention/
internal static class NamingConvention
{
    public const string PrimaryKeySuffix = "pk";
    public const string ForeignKeySuffix = "fk";
    public const string IndexSuffix = "idx";
    public const string UniqueSuffix = "unique";
    public const string CheckSuffix = "check";
}
