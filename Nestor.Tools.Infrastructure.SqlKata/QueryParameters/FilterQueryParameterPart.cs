using Nestor.Tools.Infrastructure.SqlKata.QueryParameters.Common;
using SqlKata;

namespace Nestor.Tools.Infrastructure.SqlKata.QueryParameters
{
    public class FilterQueryParameterPart
    {
        #region Properties

        /// <summary>
        ///     Affecte ou obtient le champ concerné par le filtrage
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        ///     Affecte ou obtient l'opérateur de comparaison
        /// </summary>
        public FilterOperator Operator { get; set; }

        /// <summary>
        ///     Affecte ou obtient la valeur
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        ///     Sous requête à effectuer (WHERE mon_champ 'op' (Query))
        /// </summary>
        public Query Query { get; set; }

        /// <summary>
        ///     Affecte ou obtient l'opérateur logique avec le prédécesseur
        /// </summary>
        public LogicOperator Logic { get; set; } = LogicOperator.And;

        /// <summary>
        ///     Affecte ou obtient une clé qui permet le regroupement de requêtes entre elles
        /// </summary>
        public string GroupingQueryKey { get; set; }

        #endregion
    }
}