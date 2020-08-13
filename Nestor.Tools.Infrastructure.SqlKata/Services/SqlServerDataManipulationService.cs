using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Nestor.Tools.Infrastructure.Abstractions;
using Nestor.Tools.Infrastructure.SqlKata.Abstractions;
using Nestor.Tools.Infrastructure.SqlKata.QueryParameters;
using Nestor.Tools.Infrastructure.SqlKata.QueryParameters.Common;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Nestor.Tools.Infrastructure.SqlKata.Services
{
    public class SqlServerDataManipulationService : IDMLService
    {
        #region Private attributes

        private IDDLService ddlService;

        #endregion

        #region Constructors

        public SqlServerDataManipulationService(IDDLService ddlService)
        {
            this.ddlService = ddlService;
        }

        #endregion

        /// <summary>
        ///     Insère un enregistrement
        /// </summary>
        /// <param name="context">Contexte d'exécution de la base de données</param>
        /// <param name="transaction">Transaction à utiliser (si déjà ouverte)</param>
        /// <param name="obj">Objet à insérer</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="schema">Nom du schéma</param>
        /// <returns>Element ajouté</returns>
        public dynamic InsertData(DbConnection connection, dynamic obj, string table, string schema = "dbo",
            bool closeConnection = true)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var result = InsertData(connection, transaction, obj, table, schema, false);
                    transaction.Commit();
                    return result;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    if (closeConnection && connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
        }

        /// <summary>
        ///     Insère un enregistrement
        /// </summary>
        /// <param name="context">Contexte d'exécution de la base de données</param>
        /// <param name="transaction">Transaction à utiliser (si déjà ouverte)</param>
        /// <param name="obj">Objet à insérer</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="schema">Nom du schéma</param>
        /// <returns>Element ajouté</returns>
        public dynamic InsertData(DbConnection connection, DbTransaction transaction, dynamic obj, string table,
            string schema = "dbo", bool closeConnection = true)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            //Récupération des données sous forme de dictionnaire
            var dictionnary = obj as IDictionary<string, object>;

            //Création de la requête sql
            ICollection<SqlParameter> parameters = new List<SqlParameter>();

            using (var sqlCommand = connection.CreateCommand())
            {
                sqlCommand.Transaction = transaction;
                sqlCommand.CommandText =
                    $"INSERT INTO [{schema}].[{table}]({string.Join(", ", dictionnary.Keys.Select(col => string.Format($"[{col}]")))}) VALUES ({string.Join(", ", dictionnary.Keys.Select(col => string.Format($"@{col}")))}); SELECT CAST(SCOPE_IDENTITY() AS INT);";

                foreach (var kvpItem in dictionnary)
                    if (kvpItem.Value == null)
                    {
                        sqlCommand.Parameters.Add(new SqlParameter($"@{kvpItem.Key}", DBNull.Value));
                    }
                    else
                    {
                        var sqlParam = $"@{kvpItem.Key}";
                        if (string.Compare(kvpItem.Value.ToString(), bool.FalseString, true) == 0 ||
                            string.Compare(kvpItem.Value.ToString(), bool.TrueString, true) == 0)
                            sqlCommand.Parameters.Add(new SqlParameter(sqlParam,
                                bool.Parse(kvpItem.Value.ToString()))); // Valeur booléenne
                        else if (long.TryParse(kvpItem.Value.ToString(), out var longParseResult))
                            sqlCommand.Parameters.Add(new SqlParameter(sqlParam, longParseResult)); // Valeur longue
                        else if (float.TryParse(kvpItem.Value.ToString(), out var floatParseValue))
                            sqlCommand.Parameters.Add(new SqlParameter(sqlParam, floatParseValue)); // Valeur flotante
                        else if (DateTime.TryParse(kvpItem.Value.ToString(), out var dateTimeParseResult))
                            sqlCommand.Parameters.Add(new SqlParameter(sqlParam,
                                dateTimeParseResult)); // Valeur DateTime
                        else
                            sqlCommand.Parameters.Add(new SqlParameter(sqlParam,
                                kvpItem.Value)); // Valeur chaine de caractère
                    }

                if (connection.State != ConnectionState.Open)
                    connection.Open();

                var newId = (int) sqlCommand.ExecuteScalar();
                var result = GetById(connection, newId, table, schema, transaction);

                if (closeConnection && connection.State == ConnectionState.Open)
                    connection.Close();

                return result;
            }
        }


        /// <summary>
        ///     Insère plusieurs enregistrements
        /// </summary>
        /// <param name="context">Contexte d'exécution de la base de données</param>
        /// <param name="obj">Objets à insérer</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="schema">Nom du schéma</param>
        /// <returns></returns>
        public IEnumerable<dynamic> InsertData(DbConnection connection, IEnumerable<dynamic> objs, string table,
            string schema = "dbo")
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Met à jour un objet dans la table donnée
        /// </summary>
        /// <param name="objectId">Identifiant de l'objet</param>
        /// <param name="columns">Liste des colonnes à mettre à jour</param>
        /// <param name="values">Liste des valeurs</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="schema">Nom du schéma</param>
        /// <returns></returns>
        public async Task<int> UpdateDataAsync(DbConnection connection, long id, IEnumerable<string> columns,
            IEnumerable<object> values, string table, string schema = "dbo", DbTransaction transaction = null)
        {
            if (connection.State == ConnectionState.Closed)
                await connection.OpenAsync();

            //var transaction = context.Database.CurrentTransaction.GetDbTransaction();

            var db = new QueryFactory(connection, new SqlServerCompiler());
            var query = db.Query($"{schema}.{table}").Where("Id", id).AsUpdate(columns, values);

            // Insertion de l'enregistrement
            var affected = db.ExecuteScalar<int>(query, transaction, CommandType.Text);
            //transaction.Commit();

            return affected;
        }

        /// <summary>
        ///     Supprime un enregistrement en base de données
        /// </summary>
        /// <typeparam name="long">Type de l'identifiant</typeparam>
        /// <param name="context">Contexte d'exécution de la base de données</param>
        /// <param name="id">Identifiant</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="schema">Nom du schéma</param>
        /// <returns></returns>
        public async Task<int> DeleteDataAsync(DbConnection connection, long id, string table, string schema = "dbo",
            DbTransaction transaction = null)
        {
            if (connection.State == ConnectionState.Closed)
                await connection.OpenAsync();

            var db = new QueryFactory(connection, new SqlServerCompiler());

            var query = db.Query($"{schema}.{table}").Where("Id", id).AsDelete();

            var affected = db.ExecuteScalar<int>(query, transaction, CommandType.Text);

            return affected;
        }

        /// <summary>
        ///     Effectue une sélection de données en base de données
        /// </summary>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="schema">Nom du schéma</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="sortParameters">Liste des critères de tris</param>
        /// <param name="groupByParameters">Liste des critères d'aggrégation de données</param>
        /// <param name="aggregateQueryParameters">Liste des aggrégations à appliquer</param>
        /// <param name="whereQueryParameters">Liste des critères de filtrage</param>
        /// <param name="skip">Nombre d'éléments à éviter</param>
        /// <param name="take">Nombre d'éléments à prendre</param>
        /// <returns></returns>
        public IEnumerable<dynamic> SelectData(DbConnection connection, string table, string schema,
            IEnumerable<WhereQueryParameter> whereQueryParameters, IEnumerable<SortQueryParameter> sortParameters,
            IEnumerable<GroupByQueryParameter> groupByParameters,
            IEnumerable<AggregateQueryParameter> aggregateQueryParameters, int? take, int? skip,
            DbTransaction transaction = null)
        {
            var query = new Query($"{schema}.{table}");

            // Application des critères de tri
            if (sortParameters != null && sortParameters.Any())
                foreach (var item in sortParameters)
                    switch (item.Direction)
                    {
                        case SortDirection.Asc:
                            query = query.OrderBy(item.Field);
                            break;
                        case SortDirection.Desc:
                            query = query.OrderByDesc(item.Field);
                            break;
                    }

            if (whereQueryParameters != null && whereQueryParameters.Any())
                foreach (var group in whereQueryParameters.SelectMany(criteria => criteria.Filters)
                    .GroupBy(filter => filter.GroupingQueryKey))
                {
                    var subQuery = new Query();

                    foreach (var criteria in group)
                    {
                        if (criteria.Logic == LogicOperator.Or)
                            subQuery = subQuery.Or();

                        switch (criteria.Operator)
                        {
                            case FilterOperator.Eq:
                                if (criteria.Query == null)
                                    subQuery = subQuery.Where(criteria.Field, "=", criteria.Value);
                                else
                                    subQuery = subQuery.Where(criteria.Field, "=", criteria.Query);
                                break;
                            case FilterOperator.StartsWith:
                                subQuery = subQuery.WhereStarts(criteria.Field, criteria.Value);
                                break;
                            case FilterOperator.EndsWith:
                                subQuery = subQuery.WhereEnds(criteria.Field, criteria.Value);
                                break;
                            case FilterOperator.Contains:
                                subQuery = subQuery.WhereContains(criteria.Field, criteria.Value);
                                break;
                            case FilterOperator.In:
                                subQuery = subQuery.WhereIn(criteria.Field, criteria.Query);
                                break;
                        }
                    }

                    query = query.Where(q => subQuery);
                }

            if (take.HasValue)
                query = query.Take(take.Value);
            if (skip.HasValue)
                query = query.Skip(skip.Value);

            var db = new QueryFactory(connection, new SqlServerCompiler());
            var result = db.Get<dynamic>(new[] {query}, transaction);
            return result.Count() > 0 ? result.ElementAt(0) : new List<dynamic>();
        }

        public async Task<long> CountAsync(DbConnection connection, string table, string schema)
        {
            var db = new QueryFactory(connection, new SqlServerCompiler());
            var query = new Query($"{schema}.{table}").AsCount("Id");

            return await db.CountAsync<long>(query);
        }

        /// <summary>
        ///     Obtient un objet depuis son identifiant
        /// </summary>
        /// <typeparam name="long">Type de l'identifiant</typeparam>
        /// <param name="connection">Connexion à la base de données</param>
        /// <param name="id">Identifiant</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="schema">Schéma de base de données</param>
        /// <returns></returns>
        public async Task<dynamic> GetByIdAsync(DbConnection connection, long id, string table, string schema = "dbo",
            DbTransaction transaction = null, bool closeConnection = true)
        {
            if (connection.State == ConnectionState.Closed)
                await connection.OpenAsync();

            var obj = GetById(connection, id, table, schema, transaction);

            //if (connection.State == System.Data.ConnectionState.Open && closeConnection)
            //    connection.Close();

            return obj;
        }

        public IEnumerable<dynamic> SelectData(DbConnection connection, string table, string schema)
        {
            var db = new QueryFactory(connection, new SqlServerCompiler());
            var query = new Query($"{schema}.{table}");
            return db.Get(query);
        }

        /// <summary>
        ///     Obtient le résultat d'une execution de requête SQL
        /// </summary>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="rawSql">requête SQL à exécuter</param>
        /// <param name="parameters">Paramètres d'entrée</param>
        /// <param name="closeConnection">Si vrai, ferme la connexion</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetFromRawSql(DbConnection connection, string rawSql,
            IEnumerable<object> parameters, bool closeConnection = true)
        {
            var db = new QueryFactory(connection, new SqlServerCompiler());
            var query = new Query().FromRaw(rawSql, parameters);

            return db.Get(query);
        }

        /// <summary>
        ///     Obtient le résultat d'une execution de requête SQL
        /// </summary>
        /// <param name="context">Contexte de la base de données</param>
        /// <param name="rawSql">requête SQL à exécuter</param>
        /// <param name="parameters">Paramètres d'entrée</param>
        /// <param name="closeConnection">Si vrai, ferme la connexion</param>
        /// <typeparam name="T">Type de retour</typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetFromRawSql<T>(DbConnection connection, string rawSql, IEnumerable<object> parameters,
            bool closeConnection = true)
        {
            var db = new QueryFactory(connection, new SqlServerCompiler());
            var query = new Query().FromRaw(rawSql, parameters);

            return db.Get<T>(query);
        }

        public IEnumerable<dynamic> SelectData(DbConnection connection, IEnumerable<string> columns, string table,
            string schema, string joinTable, string joinOnForeignKey, string joinOnPrimaryKey, string column,
            object value, DbTransaction transaction = null)
        {
            var query = new Query($"{schema}.{table}");

            if (columns.Any())
                query = query.Select(columns.ToArray());

            if (!string.IsNullOrEmpty(joinTable) && !string.IsNullOrEmpty(joinOnForeignKey) &&
                !string.IsNullOrEmpty(joinOnPrimaryKey))
                query = query.Join(joinTable, joinOnPrimaryKey, joinOnForeignKey);

            query = query.Where(column, "=", value);

            var db = new QueryFactory(connection, new SqlServerCompiler());
            var result = db.Get<dynamic>(new[] {query}, transaction);
            return result.Count() > 0 ? result.ElementAt(0) : new List<dynamic>();
        }


        /// <summary>
        ///     Obtient un objet depuis son identifiant
        /// </summary>
        /// <typeparam name="long">Type de l'identifiant</typeparam>
        /// <param name="connection">Connexion à la base de données</param>
        /// <param name="id">Identifiant</param>
        /// <param name="table">Nom de la table</param>
        /// <param name="schema">Schéma de base de données</param>
        /// <returns></returns>
        private dynamic GetById(DbConnection connection, long id, string table, string schema = "dbo",
            IDbTransaction transaction = null)
        {
            var db = new QueryFactory(connection, new SqlServerCompiler());
            var query = new Query($"{schema}.{table}").Where("Id", id);

            // 2 fois .SingleOrDefault(), car pour permettre d'utiliser une transaction, il faut utiliser un tableau de requêtes. 
            return db.Get<dynamic>(new[] {query}, transaction).SingleOrDefault()?.SingleOrDefault();
        }
    }
}