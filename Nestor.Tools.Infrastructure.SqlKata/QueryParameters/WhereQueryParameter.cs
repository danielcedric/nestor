using System.Collections.Generic;

namespace Nestor.Tools.Infrastructure.SqlKata.QueryParameters
{
    public class WhereQueryParameter
    {
        #region Properties
        public List<FilterQueryParameterPart> Filters { get; set; } = new List<FilterQueryParameterPart>();
        #endregion

        #region Methods
        ///// <summary>
        ///// Ajoute un critère de filtrage
        ///// </summary>
        ///// <param name="logicOperator">Opérateur logique (and|or)</param>
        ///// <param name="filter">Critère de filtrage</param>
        //public void Add(LogicOperator logicOperator, FilterQueryParameterPart filter)
        //{
        //    if (Filters.Any())
        //        Filters.Add(new KeyValuePair<LogicOperator?, FilterQueryParameterPart>(logicOperator, filter));
        //    else
        //        Add(filter);
        //}

        /// <summary>
        /// Ajoute un critère de filtrage sans opérateur logique (utilisé pour la première insertion)
        /// </summary>
        /// <param name="logicOperator">Opérateur logique (and|or)</param>
        /// <param name="filter">Critère de filtrage</param>
        public void Add(FilterQueryParameterPart filter)
        {
            //if (Filters.Any())
            //    Filters.Add(new KeyValuePair<LogicOperator?, FilterQueryParameterPart>(LogicOperator.And, filter));
            //else
                Filters.Add(filter);
        }
        #endregion
    }
}
