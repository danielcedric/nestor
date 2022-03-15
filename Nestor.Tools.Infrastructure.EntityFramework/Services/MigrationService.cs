using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using Nestor.Tools.Infrastructure.EntityFramework.DataDescription;
using Nestor.Tools.Infrastructure.EntityFramework.Models;

namespace Nestor.Tools.Infrastructure.EntityFramework.Services
{
    public abstract class MigrationService : IDDLService
    {
        #region Constructors

        protected MigrationService(string activeProvider)
        {
            MigrationBuilder = new MigrationBuilder(activeProvider);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Affecte ou obtient le builder de migrations
        /// </summary>
        public MigrationBuilder MigrationBuilder { get; set; }

        #endregion

        #region Methods

        #region Gestion des colonnes

        /// <summary>
        ///     Créé une nouvelle colonne en base de données
        /// </summary>
        /// <param name="database">Base de données</param>
        /// <param name="tableName">Nom de la table à créer</param>
        /// <param name="columnName">Nom de la colonne à modifier</param>
        /// <param name="defaultValue">Valeur par défaut (si applicable)</param>
        /// <param name="isNullable">si vrai, indique que la colonne est nullable</param>
        /// <param name="maxLength">Longueur max du champ (si applicable)</param>
        /// <param name="schema">Schéma auquel il faut rattacher la table</param>
        /// <param name="updateDatabase">Si vrai, met à jour la base de données</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<DbOperationResult>> AddColumnAsync<TColumn>(DbContext context,
            string tableName, string columnName, bool isNullable, object defaultValue, int? maxLength,
            string schema = "dbo", bool updateDatabase = true)
        {
            var newColumn = new AddColumnOperation
            {
                Name = columnName, ClrType = typeof(TColumn), IsNullable = isNullable, DefaultValue = defaultValue,
                IsUnicode = true, MaxLength = maxLength, Table = tableName, Schema = schema
            };
            return await ComputeResultAsync(context, newColumn, updateDatabase);
        }

        /// <summary>
        ///     Renomme la colonne
        /// </summary>
        /// <param name="database">Base de données</param>
        /// <param name="tableName">Nom de la table</param>
        /// <param name="oldName">Ancien nom de la colonne</param>
        /// <param name="newName">Nouveau nom de la colonne</param>
        /// <param name="schema">Schéma</param>
        /// <param name="updateDatabase">Si vrai, met à jour la base de données</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<DbOperationResult>> RenameColumnAsync(DbContext context, string tableName,
            string oldName, string newName, string schema = "dbo", bool updateDatabase = true)
        {
            var op = new RenameColumnOperation {Schema = schema, Table = tableName, Name = oldName, NewName = newName};
            return await ComputeResultAsync(context, op, updateDatabase);
        }


        /// <summary>
        ///     Met à jour une colonne en base de données
        /// </summary>
        /// <typeparam name="TColumn">Nouveau type de la colonne</typeparam>
        /// <param name="database">Base de données</param>
        /// <param name="tableName">Nom de la table à modifier</param>
        /// <param name="columnName">Nom de la colonne à modifier</param>
        /// <param name="defaultValue">Valeur par défaut (si applicable)</param>
        /// <param name="isNullable">si vrai, indique que la colonne est nullable</param>
        /// <param name="maxLength">Longueur max du champ (si applicable)</param>
        /// <param name="schema">Schéma auquel il faut rattacher la table</param>
        /// <param name="updateDatabase">Si vrai, met à jour la base de données</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<DbOperationResult>> AlterColumnAsync<TColumn>(DbContext context,
            string tableName, string columnName, bool isNullable, object defaultValue, int? maxLength,
            string schema = "dbo", bool updateDatabase = true)
        {
            var newColumn = new AlterColumnOperation
            {
                Name = columnName, ClrType = typeof(TColumn), IsNullable = isNullable, DefaultValue = defaultValue,
                IsUnicode = true, MaxLength = maxLength, Table = tableName, Schema = schema
            };
            return await ComputeResultAsync(context, newColumn, updateDatabase);
        }

        #endregion

        /// <summary>
        ///     Créé une relation de type 1,n ou 0,n
        /// </summary>
        /// <param name="database">Base de données</param>
        /// <param name="principalTable">Table principale</param>
        /// <param name="foreignTable">Table qui contiendra la relation de clé étrangère (côté du 1)</param>
        /// <param name="foreignKeyColumnName">Nom de la colonne qui supportera la contrainte de clé étrangère</param>
        /// <param name="foreignKeyConstraintName">Nom de la contrainte de clé étrangère</param>
        /// <param name="schema">Schéma de base de données</param>
        /// <param name="onDelete">Option à appliquer en cas de suppression d'un enregistrement du côté de la clé primaire</param>
        /// <param name="onUpdate">Option à appliquer en cas de mise à jour de la clé primaire</param>
        /// <param name="isNullable">Si vrai, la clé étrangère autorisera les valeurs nulles</param>
        /// <param name="defaultValue">Valeur par défaut</param>
        /// <param name="updateDatabase">Si vrai, met à jour la base de données</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<DbOperationResult>> AddOneToManyRelationAsync(DbContext context,
            string principalTable, string foreignKeyColumnName, string foreignKeyConstraintName, string foreignTable,
            string schema = "dbo", ReferentialAction onDelete = ReferentialAction.Restrict,
            ReferentialAction onUpdate = ReferentialAction.Cascade, bool isNullable = true, object defaultValue = null,
            bool updateDatabase = true)
        {
            return await AddOneToManyRelationAsync(context, principalTable, foreignKeyColumnName,
                foreignKeyConstraintName, foreignTable, schema, schema, onDelete, onUpdate, isNullable, defaultValue,
                updateDatabase);
        }

        /// <summary>
        ///     Créé une relation de type 1,n ou 0,n
        /// </summary>
        /// <param name="context">Contexte de  la base de données</param>
        /// <param name="principalTable">Table principale</param>
        /// <param name="foreignKeyColumName">Nom du champ qui contiendra la clé étrangère</param>
        /// <param name="foreignKeyConstraintName">Nom de la contrainte de clé étrangère</param>
        /// <param name="foreignTable">Table qui contiendra la relation de clé étrangère (côté du 1)</param>
        /// <param name="principalTableSchema">Schéma de base de données de la table principale</param>
        /// <param name="foreignTableSchema">Schéma de base de données de la table de clé étrangère</param>
        /// <param name="onDelete">Option à appliquer en cas de suppression d'un enregistrement du côté de la clé primaire</param>
        /// <param name="onUpdate">Option à appliquer en cas de mise à jour de la clé primaire</param>
        /// <param name="isNullable">Si vrai, la clé étrangère autorisera les valeurs nulles</param>
        /// <param name="defaultValue">Valeur par défaut</param>
        /// <param name="updateDatabase">Si vrai, met à jour la base de données</param>
        /// <returns>Liste des resultats des operations réalisées</returns>
        public virtual async Task<IEnumerable<DbOperationResult>> AddOneToManyRelationAsync(DbContext context,
            string principalTable, string foreignKeyColumnName, string foreignKeyConstraintName, string foreignTable,
            string principalTableSchema, string foreignTableSchema, ReferentialAction onDelete,
            ReferentialAction onUpdate, bool isNullable, object defaultValue, bool updateDatabase = true)
        {
            // Création de la colonne du côté de la table qui contiendra la clé étrangère
            var fkColumn = new AddColumnOperation
            {
                Name = foreignKeyColumnName, ClrType = typeof(long), IsNullable = isNullable, Table = foreignTable,
                Schema = foreignTableSchema
            };

            //Création de la contraite de clé étrangère
            var fkConstraint = new AddForeignKeyOperation
            {
                Name = foreignKeyConstraintName, PrincipalSchema = principalTableSchema,
                PrincipalTable = principalTable, PrincipalColumns = new[] {"Id"}, Columns = new[] {fkColumn.Name},
                Schema = foreignTableSchema, Table = foreignTable, OnDelete = onDelete, OnUpdate = onUpdate
            };

            return await ComputeResultAsync(context, new MigrationOperation[] {fkColumn, fkConstraint}, updateDatabase);
        }

        /// <summary>
        ///     Supprime une contrainte de clé étrangère
        /// </summary>
        /// <param name="database">Base de données</param>
        /// <param name="fkConstraintName">Nom de la contrainte de clé étrangère</param>
        /// <param name="schema">Nom du schéma</param>
        /// <param name="columnName">Nom de la colonne</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="dropColumn">Si vrai, supprime la colonne support de la contrainte</param>
        /// <param name="updateDatabase">Si vrai, met à jour la base de données</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<DbOperationResult>> DropForeignKey(DbContext context,
            string fkConstraintName, string columnName, string table, string schema = "dbo", bool dropColumn = false,
            bool updateDatabase = true)
        {
            var result = new List<DbOperationResult>();

            // Suppression de la contrainte de clé étrangère
            var dropFKOperation = new DropForeignKeyOperation
                {Name = fkConstraintName, Schema = schema, Table = table, IsDestructiveChange = false};
            result.AddRange(await ComputeResultAsync(context, dropFKOperation, updateDatabase));

            if (dropColumn)
                // Suppression de la colonne
                result.AddRange(await DropColumn(context, table, columnName, schema, updateDatabase));

            return result;
        }

        /// <summary>
        ///     Supprime une colonne en base de données
        /// </summary>
        /// <param name="database">Base de données</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="column">Nom de la colonne</param>
        /// <param name="schema">Nom du schéma</param>
        /// <param name="updateDatabase">Si vrai, met à jour la base de données</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<DbOperationResult>> DropColumn(DbContext context, string table,
            string column, string schema = "dbo", bool updateDatabase = true)
        {
            var dropColumnOperation = new DropColumnOperation
                {Name = column, Schema = schema, Table = table, IsDestructiveChange = true};
            return await ComputeResultAsync(context, dropColumnOperation, updateDatabase);
        }

        #region Abstract methods

        /// <summary>
        ///     Teste si la table existe
        /// </summary>
        /// <param name="database">Base de données</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="schema">Schéma</param>
        /// <returns></returns>
        public abstract Task<bool> CheckIfTableExists(DbContext context, string table, string schema = "dbo",
            bool closeConnection = true);

        /// <summary>
        ///     Teste si la colonne existe déjà dans la table
        /// </summary>
        /// <param name="database">Base de données</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="column">Nom de la colone</param>
        /// <param name="schema">Schéma</param>
        /// <returns></returns>
        public abstract Task<bool> CheckIfColumnExistsInTable(DbContext context, string table, string column,
            string schema = "dbo", bool closeConnection = true);

        /// <summary>
        ///     Créé une nouvelle colonne en base de données
        /// </summary>
        /// <param name="database">Base de données</param>
        /// <param name="tableName">Nom de la table à créer</param>
        /// <param name="schema">Schéma auquel il faut rattacher la table</param>
        /// <param name="updateDatabase">Si vrai, met à jour la base de données</param>
        /// <returns></returns>
        public abstract Task<IEnumerable<DbOperationResult>> CreateTableAsync(DbContext context, string tableName,
            string schema = "dbo", bool updateDatabase = true);

        /// <summary>
        ///     Obtient la liste des colonnes d'une table
        /// </summary>
        /// <param name="connection">Contexte de la base de données</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="schema">Nomm du schéma</param>
        /// <returns></returns>
        public abstract Task<IEnumerable<string>> GetColumnsAsync(DbContext context, string table,
            string schema = "dbo", bool closeConnection = true);

        /// <summary>
        ///     Obtient la description complète d'une clé étrangère
        /// </summary>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="constraintName">Nom de la contrainte</param>
        /// <param name="closeConnection">Si vrai, ferme la connexion</param>
        /// <returns></returns>
        public abstract Task<ForeignKeyDescription> GetForeignKeyDescription(DbContext context, string constraintName,
            bool closeConnection = true);

        /// <summary>
        ///     Retourne vrai si l'index existe dans la base de données
        /// </summary>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="indexName">Nom de l'index</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="schema">Nom du schéma</param>
        /// <returns></returns>
        public abstract Task<bool> CheckIfIndexExistsAsync(DbContext context, DbTransaction transaction,
            string indexName, string table, string schema = "dbo", bool closeConnection = true);

        #endregion

        #endregion

        #region Protected methods

        /// <summary>
        ///     Génère le résultat de l'opération sur la bae de données
        /// </summary>
        /// <param name="database">Connexion à la base de données</param>
        /// <param name="ops">Ensemble d'opérations à passer en base de données</param>
        /// <param name="updateDatabase">Si vrai, applique les requêtes en base de données</param>
        /// <returns></returns>
        protected async Task<IEnumerable<DbOperationResult>> ComputeResultAsync(DbContext context,
            IEnumerable<MigrationOperation> ops, bool updateDatabase)
        {
            var result = new List<DbOperationResult>();

            foreach (var op in ops)
                result.AddRange(await ComputeResultAsync(context, op, updateDatabase));

            return result;
        }

        /// <summary>
        ///     Génère le résultat de l'opération sur la bae de données
        /// </summary>
        /// <param name="database">Connexion à la base de données</param>
        /// <param name="op">Opération à passer en base de données</param>
        /// <param name="updateDatabase">Si vrai, applique les requêtes en base de données</param>
        /// <returns></returns>
        protected async Task<IEnumerable<DbOperationResult>> ComputeResultAsync(DbContext context,
            MigrationOperation op, bool updateDatabase)
        {
            var generator = context.Database.GetService<IMigrationsSqlGenerator>();
            var connection = context.Database.GetService<IRelationalConnection>();

            var commands = generator.Generate(new[] {op});

            if (updateDatabase)
            {
                IList<DbOperationResult> result = new List<DbOperationResult>();

                foreach (var cmd in commands)
                    try
                    {
                        var rows = await cmd.ExecuteNonQueryAsync(connection);
                        result.Add(new DbOperationResult(cmd.CommandText, DbOperationResult.DbOperationStatus.Success)
                            {Message = $"{rows} affectée({(rows > 1 ? "s" : string.Empty)})"});
                    }
                    catch (Exception e)
                    {
                        result.Add(new DbOperationResult(cmd.CommandText, DbOperationResult.DbOperationStatus.Error)
                            {Message = e.InnerException?.Message ?? e.Message});
                    }

                return result;
            }

            return commands.Select(cmd => new DbOperationResult(cmd.CommandText));
        }

        /// <summary>
        ///     Créé un index de performance
        /// </summary>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="indexName">Nom de l'index</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="columns">Liste des champs</param>
        /// <param name="schema">Nom du schéma</param>
        /// <returns>retourne le nom de l'index si correctement créé</returns>
        public async Task<string> CreateIndexAsync(DbContext context, string table, IEnumerable<string> columns,
            string schema = "dbo", bool updateDatabase = true)
        {
            var indexName = $"IX_{table}_{string.Join("_", columns)}";
            return await CreateIndexAsync(context, table, columns, indexName, schema, updateDatabase);
        }

        /// <summary>
        ///     Créé un index de performance
        /// </summary>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="indexName">Nom de l'index</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="columns">Liste des champs</param>
        /// <param name="schema">Nom du schéma</param>
        /// <returns>retourne le nom de l'index si correctement créé</returns>
        public async Task<string> CreateIndexAsync(DbContext context, string table, IEnumerable<string> columns,
            string indexName, string schema = "dbo", bool updateDatabase = true)
        {
            var createIndexOperation = new CreateIndexOperation
            {
                Table = table, Schema = schema, Name = indexName, Columns = columns.ToArray(),
                IsDestructiveChange = false, IsUnique = false
            };
            var result = await ComputeResultAsync(context, createIndexOperation, updateDatabase);

            return indexName;
        }

        /// <summary>
        ///     Créé un index unique
        /// </summary>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="indexName">Nom de l'index</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="columns">Liste des champs</param>
        /// <param name="schema">Nom du schéma</param>
        /// <returns>retourne le nom de l'index si correctement créé</returns>
        public async Task<string> CreateUniqueIndexAsync(DbContext context, string table, IEnumerable<string> columns,
            string schema = "dbo", bool updateDatabase = true)
        {
            var indexName = $"UK_{table}_{string.Join("_", columns)}";
            return await CreateUniqueIndexAsync(context, table, columns, indexName, schema, updateDatabase);
        }

        /// <summary>
        ///     Créé un index de performance
        /// </summary>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="indexName">Nom de l'index</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="columns">Liste des champs</param>
        /// <param name="schema">Nom du schéma</param>
        /// <returns>retourne le nom de l'index si correctement créé</returns>
        public async Task<string> CreateUniqueIndexAsync(DbContext context, string table, IEnumerable<string> columns,
            string indexName, string schema = "dbo", bool updateDatabase = true)
        {
            var createIndexOperation = new CreateIndexOperation
            {
                Table = table, Schema = schema, Name = indexName, Columns = columns.ToArray(),
                IsDestructiveChange = false, IsUnique = true
            };
            var result = await ComputeResultAsync(context, createIndexOperation, updateDatabase);

            return indexName;
        }

        /// <summary>
        ///     Supprime l'index
        /// </summary>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="indexName">Nom de l'index à supprimer</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="schema">Nom du schéma</param>
        /// <param name="updateDatabase">Si vrai, met à jour la base de données</param>
        /// <returns></returns>
        public async Task<IEnumerable<DbOperationResult>> DropIndex(DbContext context, string indexName, string table,
            string schema = "dbo", bool updateDatabase = true)
        {
            // Suppression de la contrainte de clé étrangère
            var dropIndexOperation = new DropIndexOperation
                {Name = indexName, Schema = schema, Table = table, IsDestructiveChange = false};
            var result = await ComputeResultAsync(context, dropIndexOperation, updateDatabase);

            return result;
        }

        #endregion
    }
}