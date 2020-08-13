using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Nestor.Tools.Infrastructure.SqlKata.QueryParameters;

namespace Nestor.Tools.Infrastructure.SqlKata.Abstractions
{
    public interface IDMLService
    {
        /// <summary>
        ///     Insère un enregistrement
        /// </summary>
        /// <param name="connection">Connexion à la bdd</param>
        /// <param name="transaction">Transaction à utiliser</param>
        /// <param name="obj">Objet à insérer</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="schema">Nom du schéma</param>
        /// <returns>Element ajouté</returns>
        dynamic InsertData(DbConnection connection, dynamic obj, string table, string schema = "dbo",
            bool closeConnection = true);

        /// <summary>
        ///     Insère un enregistrement
        /// </summary>
        /// <param name="connection">Connexion à la base de données</param>
        /// <param name="transaction">Transaction à utiliser</param>
        /// <param name="obj">Objet à insérer</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="schema">Nom du schéma</param>
        /// <returns>Element ajouté</returns>
        dynamic InsertData(DbConnection connection, DbTransaction transaction, dynamic obj, string table,
            string schema = "dbo", bool closeConnection = true);

        /// <summary>
        ///     Insère plusieurs enregistrements
        /// </summary>
        /// <param name="connection">Connexion à la base de données</param>
        /// <param name="obj">Objets à insérer</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="schema">Nom du schéma</param>
        /// <returns></returns>
        IEnumerable<dynamic> InsertData(DbConnection connection, IEnumerable<dynamic> objs, string table,
            string schema = "dbo");

        /// <summary>
        ///     Met à jour un enregistrement
        /// </summary>
        /// <param name="connection">Connexion à la base de données</param>
        /// <param name="obj">Objet à insérer</param>
        /// <param name="columns">Liste des champs à mettre à jour</param>
        /// <param name="values">Liste des valeurs</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="schema">Nom du schéma</param>
        /// <returns></returns>
        Task<int> UpdateDataAsync(DbConnection connection, long id, IEnumerable<string> columns,
            IEnumerable<object> values, string table, string schema = "dbo", DbTransaction transaction = null);

        /// <summary>
        ///     Supprime un enregistrement en base de données
        /// </summary>
        /// <typeparam name="TId">Type de l'identifiant</typeparam>
        /// <param name="connection">Connexion à la base de données</param>
        /// <param name="id">Identifiant</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="schema">Nom du schéma</param>
        /// <param name="transaction">Transaction dans laquelle s'éxecute la transaction</param>
        /// <returns></returns>
        Task<int> DeleteDataAsync(DbConnection connection, long id, string table, string schema = "dbo",
            DbTransaction transaction = null);

        /// <summary>
        ///     Effectue une sélection de données en base de données
        /// </summary>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="schema">Nom du schéma</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="whereQueryParameters">Liste des critères de filtrage</param>
        /// <param name="sortParameters">Liste des critères de tris</param>
        /// <param name="groupByParameters">Liste des critères de regroupement de données</param>
        /// <param name="aggregateQueryParameters">Liste des critères d'aggrégation de données</param>
        /// <param name="skip">Nombre d'éléments à éviter</param>
        /// <param name="take">Nombre d'éléments à prendre</param>
        /// <returns></returns>
        IEnumerable<dynamic> SelectData(DbConnection connection, string table, string schema,
            IEnumerable<WhereQueryParameter> whereQueryParameters, IEnumerable<SortQueryParameter> sortParameters,
            IEnumerable<GroupByQueryParameter> groupByParameters,
            IEnumerable<AggregateQueryParameter> aggregateQueryParameters, int? take, int? skip,
            DbTransaction transaction = null);

        /// <summary>
        ///     Effectue une sélection de données en base de données
        /// </summary>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="schema">Nom du schéma</param>
        /// <param name="table">Nom de la table</param>
        /// <returns></returns>
        IEnumerable<dynamic> SelectData(DbConnection connection, string table, string schema);

        /// <summary>
        ///     Effectue une sélection de données avec jointure
        /// </summary>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="schema">Nom du schéma</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="joinTable">Table à joindre</param>
        /// <param name="joinOnForeignKey">Prédicat de jointure du côté de la FK</param>
        /// <param name="joinOnPrimaryKey">Prédicat de jointure du côté de la PK</param>
        /// <param name="column">Colonne where</param>
        /// <param name="value">Valeur cherchée</param>
        /// <returns></returns>
        IEnumerable<dynamic> SelectData(DbConnection connection, IEnumerable<string> columns, string table,
            string schema, string joinTable, string joinOnForeignKey, string joinOnPrimaryKey, string column,
            object value, DbTransaction transaction = null);

        /// <summary>
        ///     Compte le nombre d'enregistrements présents dans la table
        /// </summary>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="schema">Nom du schéma</param>
        /// <returns></returns>
        Task<long> CountAsync(DbConnection connection, string table, string schema);

        /// <summary>
        ///     Obtient un objet depuis son identifiant
        /// </summary>
        /// <typeparam name="TId">Type de l'identifiant</typeparam>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="id">Identifiant</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="schema">Nom du schéma</param>
        /// <returns></returns>
        Task<dynamic> GetByIdAsync(DbConnection connection, long id, string table, string schema = "dbo",
            DbTransaction transaction = null, bool closeConnection = true);

        /// <summary>
        ///     Obtient le résultat d'une execution de requête SQL
        /// </summary>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="rawSql">requête SQL à exécuter</param>
        /// <param name="parameters">Paramètres d'entrée</param>
        /// <param name="closeConnection">Si vrai, ferme la connexion</param>
        /// <returns></returns>
        IEnumerable<dynamic> GetFromRawSql(DbConnection connection, string rawSql, IEnumerable<object> parameters,
            bool closeConnection = true);

        /// <summary>
        ///     Obtient le résultat d'une execution de requête SQL
        /// </summary>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="rawSql">requête SQL à exécuter</param>
        /// <param name="parameters">Paramètres d'entrée</param>
        /// <param name="closeConnection">Si vrai, ferme la connexion</param>
        /// <typeparam name="T">Type de retour</typeparam>
        /// <returns></returns>
        IEnumerable<T> GetFromRawSql<T>(DbConnection connection, string rawSql, IEnumerable<object> parameters,
            bool closeConnection = true);
    }
}