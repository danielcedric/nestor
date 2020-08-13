using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Nestor.Tools.Exceptions;
using Nestor.Tools.Infrastructure.EntityFramework.Models;

namespace Nestor.Tools.Infrastructure.EntityFramework.Services
{
    public class SqlServerMigrationService : MigrationService
    {
        public SqlServerMigrationService() : base("Microsoft.EntityFrameworkCore.SqlServer")
        {
        }


        #region Methods

        /// <summary>
        ///     Teste si la table existe
        /// </summary>
        /// <param name="database">Base de données</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="schema">Schéma</param>
        /// <returns></returns>
        public override async Task<bool> CheckIfTableExists(DbContext context, string table, string schema = "dbo",
            bool closeConnection = true)
        {
            if (context.Database.IsSqlServer())
                using (var connection = context.Database.GetDbConnection())
                {
                    using (var cmd = connection.CreateCommand())
                    {
                        var hasRows = false;

                        cmd.CommandText = new StringBuilder()
                            .Append("SELECT *")
                            .Append("FROM INFORMATION_SCHEMA.TABLES ")
                            .Append($"WHERE TABLE_SCHEMA = '{schema}' ")
                            .Append($"AND TABLE_NAME = '{table}'").ToString();

                        await context.Database.OpenConnectionAsync();
                        using (var result = await cmd.ExecuteReaderAsync())
                        {
                            hasRows = result.HasRows;
                        }

                        if (connection.State == ConnectionState.Open && closeConnection)
                            context.Database.CloseConnection();

                        return hasRows;
                    }
                }

            throw new NestorException("The database is not a SQL Server Database");
        }

        /// <summary>
        ///     Créé une nouvelle colonne en base de données
        /// </summary>
        /// <param name="database">Base de données</param>
        /// <param name="tableName">Nom de la table à créer</param>
        /// <param name="schema">Schéma auquel il faut rattacher la table</param>
        /// <param name="updateDatabase">Si vrai, met à jour la base de données</param>
        /// <returns></returns>
        public override async Task<IEnumerable<DbOperationResult>> CreateTableAsync(DbContext context, string tableName,
            string schema = "dbo", bool updateDatabase = true)
        {
            if (context.Database.IsSqlServer())
            {
                if (!string.IsNullOrEmpty(schema))
                    MigrationBuilder.EnsureSchema(schema);

                // Création de la table 
                var newTable = new CreateTableOperation {Name = tableName, Schema = schema};

                // Ajout d'une clé primaire
                var idColumn = new AddColumnOperation
                    {Name = "Id", ClrType = typeof(long), IsNullable = false, Table = tableName, Schema = schema};
                idColumn.AddAnnotation("SqlServer:ValueGenerationStrategy",
                    SqlServerValueGenerationStrategy.IdentityColumn);
                newTable.Columns.Add(idColumn);

                newTable.PrimaryKey = new AddPrimaryKeyOperation {Columns = new[] {"Id"}, Name = $"PK_{tableName}"};
                return await ComputeResultAsync(context, newTable, updateDatabase);
            }

            throw new NestorException("The database is not a SQL Server Database");
        }

        /// <summary>
        ///     Obtient les colonnes présentes dans la table
        /// </summary>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="schema">Nom du schéma</param>
        /// <returns></returns>
        public override async Task<IEnumerable<string>> GetColumnsAsync(DbContext context, string table,
            string schema = "dbo", bool closeConnection = true)
        {
            using (var connection = context.Database.GetDbConnection())
            {
                using (var cmd = connection.CreateCommand())
                {
                    IList<string> columns = new List<string>();

                    cmd.CommandText = new StringBuilder()
                        .Append("SELECT Name ")
                        .Append("FROM sys.columns ")
                        .Append($"WHERE Object_ID = Object_ID('[{schema}].[{table}]')").ToString();

                    if (connection.State == ConnectionState.Closed)
                        await connection.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                            while (await reader.ReadAsync())
                                columns.Add(reader.GetString(0));
                    }

                    if (connection.State == ConnectionState.Open && closeConnection)
                        connection.Close();

                    return columns;
                }
            }
        }

        /// <summary>
        ///     Teste si la colonne existe déjà dans la table
        /// </summary>
        /// <param name="database">Base de données</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="column">Nom de la colone</param>
        /// <param name="schema">Schéma</param>
        /// <returns></returns>
        public override async Task<bool> CheckIfColumnExistsInTable(DbContext context, string table, string column,
            string schema = "dbo", bool closeConnection = true)
        {
            if (context.Database.IsSqlServer())
                using (var connection = context.Database.GetDbConnection())
                {
                    using (var cmd = connection.CreateCommand())
                    {
                        var hasRows = false;

                        cmd.CommandText = new StringBuilder()
                            .Append("SELECT Name ")
                            .Append("FROM sys.columns ")
                            .Append($"WHERE Name = '{column}' ")
                            .Append($"AND Object_ID = Object_ID('[{schema}].{table}')").ToString();

                        if (connection.State == ConnectionState.Closed)
                            await context.Database.OpenConnectionAsync();

                        using (var result = await cmd.ExecuteReaderAsync())
                        {
                            hasRows = result.HasRows;
                        }

                        if (connection.State == ConnectionState.Open && closeConnection)
                            context.Database.CloseConnection();
                        return hasRows;
                    }
                }

            throw new NestorException("The database is not a SQL Server Database");
        }

        /// <summary>
        ///     Teste si la colonne existe déjà dans la table
        /// </summary>
        /// <param name="database">Base de données</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="column">Nom de la colone</param>
        /// <param name="schema">Schéma</param>
        /// <returns></returns>
        public override async Task<ForeignKeyDescription> GetForeignKeyDescription(DbContext context,
            string constraintName, bool closeConnection = true)
        {
            if (context.Database.IsSqlServer())
            {
                var connection = context.Database.GetDbConnection();

                using (var cmd = connection.CreateCommand())
                {
                    var fkConstraintDesc = new ForeignKeyDescription();

                    cmd.CommandText = new StringBuilder()
                        .Append("SELECT f.name AS foreign_key_name")
                        .Append(", OBJECT_NAME(f.parent_object_id) AS table_name")
                        .Append(", COL_NAME(fc.parent_object_id, fc.parent_column_id) AS constraint_column_name")
                        .Append(", OBJECT_NAME (f.referenced_object_id) AS referenced_table")
                        .Append(
                            ", COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS referenced_column_name")
                        .Append(", is_disabled")
                        .Append(", delete_referential_action_desc")
                        .Append(", update_referential_action_desc")
                        .Append(" FROM sys.foreign_keys AS f")
                        .Append(" INNER JOIN sys.foreign_key_columns AS fc")
                        .Append(" ON f.object_id = fc.constraint_object_id")
                        .Append($" WHERE f.name = '{constraintName}'").ToString();

                    if (connection.State == ConnectionState.Closed)
                        await context.Database.OpenConnectionAsync();


                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                            while (await reader.ReadAsync())
                            {
                                fkConstraintDesc.ConstraintName = reader.GetString(0);
                                fkConstraintDesc.TableName = reader.GetString(1);
                                fkConstraintDesc.ConstraintColumnName = reader.GetString(2);
                                fkConstraintDesc.ReferencedTable = reader.GetString(3);
                                fkConstraintDesc.ReferencedColumnName = reader.GetString(4);
                                fkConstraintDesc.IsDisabled = reader.GetBoolean(5);
                                fkConstraintDesc.OnDeleteAction = ConvertToReferentialAction(reader.GetString(6));
                                fkConstraintDesc.OnUpdateAction = ConvertToReferentialAction(reader.GetString(7));
                                break;
                            }
                    }

                    if (connection.State == ConnectionState.Open && closeConnection)
                        context.Database.CloseConnection();
                    return fkConstraintDesc;
                }
            }

            throw new NestorException("The database is not a SQL Server Database");
        }

        /// <summary>
        ///     Converti une chaine de caractères en <see cref="ReferentialAction" />
        /// </summary>
        /// <param name="action">Valeur à convertir</param>
        /// <returns></returns>
        private ReferentialAction ConvertToReferentialAction(string action)
        {
            var referentialAction = ReferentialAction.NoAction;
            switch (action)
            {
                case "CASCADE":
                    referentialAction = ReferentialAction.Cascade;
                    break;
                case "RESTRICT":
                    referentialAction = ReferentialAction.Restrict;
                    break;
                case "SET_NULL":
                    referentialAction = ReferentialAction.SetNull;
                    break;
                case "SET_DEFAULT":
                    referentialAction = ReferentialAction.SetNull;
                    break;
                case "NO_ACTION":
                default:
                    referentialAction = ReferentialAction.NoAction;
                    break;
            }

            return referentialAction;
        }

        /// <summary>
        ///     retourne vrai si l'index existe dans la table indiquée
        /// </summary>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="indexName">Nom de l'index</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="schema">Nom du schéma</param>
        /// <returns></returns>
        public override async Task<bool> CheckIfIndexExistsAsync(DbContext context, DbTransaction transaction,
            string indexName, string table, string schema = "dbo", bool closeConnection = true)
        {
            if (context.Database.IsSqlServer())
            {
                var connection = context.Database.GetDbConnection();

                using (var cmd = connection.CreateCommand())
                {
                    var hasRows = false;
                    cmd.Transaction = transaction;
                    cmd.CommandText = new StringBuilder()
                        .Append("SELECT si.Name ")
                        .Append("FROM sys.indexes AS si ")
                        .Append("INNER JOIN sys.objects AS so ON si.object_id=so.object_id ")
                        .Append("INNER JOIN sys.schemas AS sc ON so.schema_id=sc.schema_id ")
                        .Append($"WHERE sc.Name = '{schema}' ")
                        .Append($"AND so.Name = '{table}' ")
                        .Append($"AND si.Name = '{indexName}'").ToString();

                    if (connection.State == ConnectionState.Closed)
                        await context.Database.OpenConnectionAsync();

                    using (var result = await cmd.ExecuteReaderAsync())
                    {
                        hasRows = result.HasRows;
                    }

                    if (connection.State == ConnectionState.Open && closeConnection)
                        context.Database.CloseConnection();
                    return hasRows;
                }
            }

            throw new NestorException("The database is not a SQL Server Database");
        }

        #endregion
    }
}