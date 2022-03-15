using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Nestor.Tools.Infrastructure.EntityFramework.Models;

namespace Nestor.Tools.Infrastructure.EntityFramework.DataDescription
{
    public interface IDDLService
    {
        /// <summary>
        ///     Créé une nouvelle colonne en base de données
        /// </summary>
        /// <param name="context">Contexte de  la base de données</param>
        /// <param name="tableName">Nom de la table à créer</param>
        /// <param name="columnName">Nom de la colonne à modifier</param>
        /// <param name="defaultValue">Valeur par défaut (si applicable)</param>
        /// <param name="isNullable">si vrai, indique que la colonne est nullable</param>
        /// <param name="maxLength">Longueur max du champ (si applicable)</param>
        /// <param name="schema">Schéma auquel il faut rattacher la table</param>
        /// <param name="updateDatabase">Si vrai, met à jour la base de données</param>
        /// <returns></returns>
        Task<IEnumerable<DbOperationResult>> CreateTableAsync(DbContext context, string tableName,
            string schema = "dbo", bool updateDatabase = true);

        /// <summary>
        ///     Créé une nouvelle colonne en base de données
        /// </summary>
        /// <param name="context">Contexte de  la base de données</param>
        /// <param name="tableName">Nom de la table à créer</param>
        /// <param name="columnName">Nom de la colonne à créer</param>
        /// <param name="defaultValue">Valeur par défaut à insérer en base de données</param>
        /// <param name="isNullable">Si vrai la colonne acceptera les valeurs nulles</param>
        /// <param name="maxLength">Longueur max du champ si applicable</param>
        /// <typeparam name="TColumn">Type CLR du champ à créer</typeparam>
        /// <param name="schema">Schéma auquel il faut rattacher la table</param>
        /// <param name="updateDatabase">Si vrai, met à jour la base de données</param>
        /// <returns></returns>
        Task<IEnumerable<DbOperationResult>> AddColumnAsync<TColumn>(DbContext context, string tableName,
            string columnName, bool isNullable, object defaultValue, int? maxLength, string schema = "dbo",
            bool updateDatabase = true);

        /// <summary>
        ///     Renomme la colonne
        /// </summary>
        /// <param name="context">Contexte de  la base de données</param>
        /// <param name="tableName">Nom de la table</param>
        /// <param name="oldName">Ancien nom de la colonne</param>
        /// <param name="newName">Nouveau nom de la colonne</param>
        /// <param name="schema">Schéma</param>
        /// <param name="updateDatabase">Si vrai, met à jour la base de données</param>
        /// <returns></returns>
        Task<IEnumerable<DbOperationResult>> RenameColumnAsync(DbContext context, string tableName, string oldName,
            string newName, string schema = "dbo", bool updateDatabase = true);

        /// <summary>
        ///     Met à jour une colonne en base de données
        /// </summary>
        /// <typeparam name="TColumn">Nouveau type de la colonne</typeparam>
        /// <param name="context">Base de données</param>
        /// <param name="tableName">Nom de la table à modifier</param>
        /// <param name="columnName">Nom de la colonne à modifier</param>
        /// <param name="defaultValue">Valeur par défaut (si applicable)</param>
        /// <param name="isNullable">si vrai, indique que la colonne est nullable</param>
        /// <param name="maxLength">Longueur max du champ (si applicable)</param>
        /// <param name="schema">Schéma auquel il faut rattacher la table</param>
        /// <param name="updateDatabase">Si vrai, met à jour la base de données</param>
        /// <returns></returns>
        Task<IEnumerable<DbOperationResult>> AlterColumnAsync<TColumn>(DbContext context, string tableName,
            string columnName, bool isNullable, object defaultValue, int? maxLength, string schema = "dbo",
            bool updateDatabase = true);

        /// <summary>
        ///     Créé une relation de type 1,n ou 0,n
        /// </summary>
        /// <typeparam name="TId">Type de la clé primaire</typeparam>
        /// <param name="context">Contexte de  la base de données</param>
        /// <param name="principalTable">Table principale</param>
        /// <param name="foreignKeyColumName">Nom du champ qui contiendra la clé étrangère</param>
        /// <param name="foreignKeyConstraintName">Nom de la contrainte de clé étrangère</param>
        /// <param name="foreignTable">Table qui contiendra la relation de clé étrangère (côté du 1)</param>
        /// <param name="schema">Schéma de base de données</param>
        /// <param name="onDelete">Option à appliquer en cas de suppression d'un enregistrement du côté de la clé primaire</param>
        /// <param name="onUpdate">Option à appliquer en cas de mise à jour de la clé primaire</param>
        /// <param name="isNullable">Si vrai, la clé étrangère autorisera les valeurs nulles</param>
        /// <param name="defaultValue">Valeur par défaut</param>
        /// <param name="updateDatabase">Si vrai, met à jour la base de données</param>
        /// <returns>Liste des resultats des operations réalisées</returns>
        Task<IEnumerable<DbOperationResult>> AddOneToManyRelationAsync(DbContext context, string principalTable,
            string foreignKeyColumnName, string foreignKeyConstraintName, string foreignTable, string schema,
            ReferentialAction onDelete, ReferentialAction onUpdate, bool isNullable, object defaultValue,
            bool updateDatabase = true);

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
        Task<IEnumerable<DbOperationResult>> AddOneToManyRelationAsync(DbContext context, string principalTable,
            string foreignKeyColumName, string foreignKeyConstraintName, string foreignTable,
            string principalTableSchema, string foreignTableSchema, ReferentialAction onDelete,
            ReferentialAction onUpdate, bool isNullable, object defaultValue, bool updateDatabase = true);

        /// <summary>
        ///     Supprime une contrainte de clé étrangère
        /// </summary>
        /// <param name="context">Contexte de  la base de données</param>
        /// <param name="fkConstraintName">Nom de la contrainte de clé étrangère</param>
        /// <param name="columnName">Nom de la colonne à supprimer (si applicable)</param>
        /// <param name="schema">Nom du schéma</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="dropColumn">Si vrai, supprime la colonne support de la contrainte</param>
        /// <param name="updateDatabase">Si vrai, met à jour la base de données</param>
        /// <returns></returns>
        Task<IEnumerable<DbOperationResult>> DropForeignKey(DbContext context, string fkConstraintName,
            string columnName, string table, string schema, bool dropColumn = false, bool updateDatabase = true);

        /// <summary>
        ///     Supprime une colonne en base de données
        /// </summary>
        /// <param name="context">Contexte de  la base de données</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="column">Nom de la colonne</param>
        /// <param name="schema">Nom du schéma</param>
        /// <param name="updateDatabase">Si vrai, met à jour la base de données</param>
        /// <returns></returns>
        Task<IEnumerable<DbOperationResult>> DropColumn(DbContext context, string table, string column,
            string schema = "dbo", bool updateDatabase = true);

        /// <summary>
        ///     Teste si la colonne existe déjà dans la table
        /// </summary>
        /// <param name="context">Contexte de  la base de données</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="column">Nom de la colone</param>
        /// <param name="schema">Schéma</param>
        /// <returns></returns>
        Task<bool> CheckIfColumnExistsInTable(DbContext context, string table, string column, string schema = "dbo",
            bool closeConnection = true);

        /// <summary>
        ///     Teste si la table existe
        /// </summary>
        /// <param name="context">Contexte de  la base de données</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="schema">Schéma</param>
        /// <returns></returns>
        Task<bool> CheckIfTableExists(DbContext context, string table, string schema = "dbo",
            bool closeConnection = true);

        /// <summary>
        ///     Obtient la liste des colonnes d'une table
        /// </summary>
        /// <param name="context">Contexte de  la base de données</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="schema">Nomm du schéma</param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetColumnsAsync(DbContext context, string table, string schema = "dbo",
            bool closeConnection = true);

        /// <summary>
        ///     Obtient la description complète d'une clé étrangère
        /// </summary>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="constraintName">Nom de la contrainte</param>
        /// <param name="closeConnection">Si vrai, ferme la connexion</param>
        /// <returns></returns>
        Task<ForeignKeyDescription> GetForeignKeyDescription(DbContext context, string constraintName,
            bool closeConnection = true);

        /// <summary>
        ///     Créé un index de performance
        /// </summary>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="columns">Liste des champs</param>
        /// <param name="schema">Nom du schéma</param>
        /// <param name="updateDatabase">Si vrai, met à jour la base de données</param>
        /// <returns></returns>
        Task<string> CreateIndexAsync(DbContext context, string table, IEnumerable<string> columns,
            string schema = "dbo", bool updateDatabase = true);

        /// <summary>
        ///     Créé un index de performance
        /// </summary>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="indexName">Nom de l'index</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="columns">Liste des champs</param>
        /// <param name="schema">Nom du schéma</param>
        /// <param name="updateDatabase">Si vrai, met à jour la base de données</param>
        /// <returns></returns>
        Task<string> CreateIndexAsync(DbContext context, string table, IEnumerable<string> columns, string indexName,
            string schema = "dbo", bool updateDatabase = true);

        /// <summary>
        ///     Créé un index unique
        /// </summary>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="columns">Liste des champs</param>
        /// <param name="schema">Nom du schéma</param>
        /// <param name="updateDatabase">Si vrai, met à jour la base de données</param>
        /// <returns></returns>
        Task<string> CreateUniqueIndexAsync(DbContext context, string table, IEnumerable<string> columns,
            string schema = "dbo", bool updateDatabase = true);

        /// <summary>
        ///     Créé un index unique
        /// </summary>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="indexName">Nom de l'index</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="columns">Liste des champs</param>
        /// <param name="schema">Nom du schéma</param>
        /// <param name="updateDatabase">Si vrai, met à jour la base de données</param>
        /// <returns></returns>
        Task<string> CreateUniqueIndexAsync(DbContext context, string table, IEnumerable<string> columns,
            string indexName, string schema = "dbo", bool updateDatabase = true);

        /// <summary>
        ///     Supprime l'index
        /// </summary>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="indexName">Nom de l'index à supprimer</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="schema">Nom du schéma</param>
        /// <param name="closeConnection">Si vrai met à jour la base de données</param>
        /// <returns></returns>
        Task<IEnumerable<DbOperationResult>> DropIndex(DbContext context, string indexName, string table,
            string schema = "dbo", bool updateDatabase = true);

        /// <summary>
        ///     retourne vrai si l'index existe dans la table indiquée
        /// </summary>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="indexName">Nom de l'index</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="schema">Nom du schéma</param>
        /// <returns></returns>
        Task<bool> CheckIfIndexExistsAsync(DbContext context, DbTransaction transaction, string indexName, string table,
            string schema = "dbo", bool closeConnection = true);
    }
}