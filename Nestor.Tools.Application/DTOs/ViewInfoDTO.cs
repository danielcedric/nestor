using System.Collections.Generic;

namespace Nestor.Tools.Application.DTOs
{
    public class ViewInfoDTO
    {
        #region Properties
        /// <summary>
        /// Affecte ou obtient le de la page
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Affecte ou obtient une description du contenu de la page
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Affecte ou obtient le nom du controller
        /// </summary>
        public string ControllerName { get; set; }
        /// <summary>
        /// Affecte ou obtient le nom de l'action
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// Affecte ou obtient le nom complet de la vue
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Affecte ou obtient la route permettant de charger le composant
        /// </summary>
        public string Route { get; set; }

        public List<string> Parameters { get; set; }
        #endregion
    }
}
