namespace Nestor.Tools.Application.DTOs
{
    public class ComponentInfoDTO
    {
        #region Properties

        /// <summary>
        ///     Affecte ou obtient le nom du composant
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Affecte ou obtient le nom complet du composant
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        ///     Affecte ou obtient la description du composant
        /// </summary>
        public string Description { get; set; }

        #endregion
    }
}