namespace Nestor.Tools.Infrastructure.Models
{
    public class SetUp
    {
        #region Enums

        public enum ServerModeEnum
        {
            Cloud = 0,
            Server = 1
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Affecte ou obtient le mode de fonctionnement
        /// </summary>
        public ServerModeEnum ServerMode { get; set; }

        /// <summary>
        ///     Affecte ou obtient les options de la base de données
        /// </summary>
        public DatabaseSettings DatabaseSettings { get; set; } = new DatabaseSettings();

        /// <summary>
        ///     Affecte ou obtient les options d'initialisation de la société
        /// </summary>
        public CompanySettings CompanySettings { get; set; } = new CompanySettings();

        /// <summary>
        ///     Affecte ou obtient les options d'initialisation du compte admin
        /// </summary>
        public SuperAdminSettings SuperAdminSettings { get; set; } = new SuperAdminSettings();

        #endregion
    }
}