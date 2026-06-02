using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class ApplyDbProgrammability : Migration
    {
        private static readonly (string RelativePath, string ObjectType)[] SqlObjects =
        {
            ("StoredProcedures/Interaction.sql", "PROCEDURE"),
            ("StoredProcedures/Lead.sql", "PROCEDURE"),
            ("StoredProcedures/Opportunity.sql", "PROCEDURE"),
            ("StoredProcedures/OpportunityActionPlan.sql", "PROCEDURE"),
            ("StoredProcedures/PasswordRecovery.sql", "PROCEDURE"),
            ("StoredProcedures/Product.sql", "PROCEDURE"),
            ("StoredProcedures/Prompt.sql", "PROCEDURE"),
            ("StoredProcedures/User.sql", "PROCEDURE"),
            ("View/Dashboard.sql", "VIEW"),
            ("Function/Dashboard.sql", "FUNCTION")
        };

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            foreach (var sqlObject in SqlObjects)
            {
                foreach (var statement in ReadStatements(sqlObject.RelativePath))
                {
                    migrationBuilder.Sql($"DROP {sqlObject.ObjectType} IF EXISTS {GetQualifiedName(statement)};");
                    migrationBuilder.Sql(statement);
                }
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            foreach (var sqlObject in SqlObjects.Reverse())
            {
                foreach (var statement in ReadStatements(sqlObject.RelativePath).Reverse())
                {
                    migrationBuilder.Sql($"DROP {sqlObject.ObjectType} IF EXISTS {GetQualifiedName(statement)};");
                }
            }
        }

        private static string[] ReadStatements(string relativePath)
        {
            var filePath = ResolveSqlPath(relativePath);
            var sql = File.ReadAllText(filePath);

            return Regex
                .Split(sql, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase)
                .Select(statement => statement.Trim())
                .Where(statement => !string.IsNullOrWhiteSpace(statement))
                .ToArray();
        }

        private static string ResolveSqlPath(string relativePath)
        {
            var normalizedPath = relativePath.Replace('/', Path.DirectorySeparatorChar);
            var candidates = EnumerateCandidateRoots()
                .SelectMany(root => new[]
                {
                    Path.Combine(root, "Db", normalizedPath),
                    Path.Combine(root, "Repository", "Db", normalizedPath)
                })
                .Distinct()
                .ToArray();

            var resolvedPath = candidates.FirstOrDefault(File.Exists);

            if (resolvedPath is null)
            {
                throw new FileNotFoundException($"SQL file not found for migration: {relativePath}");
            }

            return resolvedPath;
        }

        private static string[] EnumerateCandidateRoots()
        {
            var roots = new[]
            {
                AppContext.BaseDirectory,
                Directory.GetCurrentDirectory()
            };

            return roots
                .Where(path => !string.IsNullOrWhiteSpace(path))
                .SelectMany(EnumerateSelfAndParents)
                .Distinct()
                .ToArray();
        }

        private static string[] EnumerateSelfAndParents(string path)
        {
            var directories = new System.Collections.Generic.List<string>();
            var current = new DirectoryInfo(path);

            while (current is not null)
            {
                directories.Add(current.FullName);
                current = current.Parent;
            }

            return directories.ToArray();
        }

        private static string GetQualifiedName(string statement)
        {
            var match = Regex.Match(
                statement,
                @"CREATE\s+(?:OR\s+ALTER\s+)?(?:PROCEDURE|VIEW|FUNCTION)\s+(?:(?:\[(?<schemaBracket>[^\]]+)\]|(?<schemaWord>\w+))\s*\.\s*)?(?:\[(?<nameBracket>[^\]]+)\]|(?<nameWord>\w+))",
                RegexOptions.IgnoreCase | RegexOptions.Multiline);

            if (!match.Success)
            {
                throw new InvalidOperationException("Unable to determine SQL object name from statement.");
            }

            var schema = match.Groups["schemaBracket"].Value;

            if (string.IsNullOrWhiteSpace(schema))
            {
                schema = match.Groups["schemaWord"].Value;
            }

            var name = match.Groups["nameBracket"].Value;

            if (string.IsNullOrWhiteSpace(name))
            {
                name = match.Groups["nameWord"].Value;
            }

            if (string.IsNullOrWhiteSpace(schema))
            {
                schema = "dbo";
            }

            return $"[{schema}].[{name}]";
        }
    }
}
