using Nestor.Tools.Infrastructure.SqlKata.QueryParameters.Common;

namespace Nestor.Tools.Infrastructure.SqlKata.QueryParameters
{
    public class SortQueryParameter
    {
        #region Properties

        /// <summary>
        ///     Affecte ou obtient le nom du champ
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        ///     Affecte ou obtient l'ordre de tri
        /// </summary>
        public SortDirection Direction { get; set; }

        #endregion
    }
}