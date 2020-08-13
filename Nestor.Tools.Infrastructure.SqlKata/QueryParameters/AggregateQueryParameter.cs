using System.Collections.Generic;
using Nestor.Tools.Infrastructure.SqlKata.QueryParameters.Common;

namespace Nestor.Tools.Infrastructure.SqlKata.QueryParameters
{
    public class AggregateQueryParameter
    {
        #region Properties

        /// <summary>
        ///     Affecte ou obtient le nom du champ
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        ///     Affecte ou obtient le type d'aggregation
        /// </summary>
        public IEnumerable<Aggregate> Aggregates { get; set; }

        #endregion
    }
}