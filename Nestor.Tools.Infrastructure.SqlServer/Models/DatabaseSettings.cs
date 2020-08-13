using System.Data.SqlClient;

namespace Nestor.Tools.Infrastructure.Models
{
    public class DatabaseSettings
    {
        /// <summary>
        ///     Affecte ou obtient le serveur
        /// </summary>
        public string ServerType { get; set; }

        /// <summary>
        ///     Affecte ou obtient l'adresse du serveur hôte
        /// </summary>
        public string ServerHost { get; set; }

        /// <summary>
        ///     Affecte ou obtient le nom de la base de données
        /// </summary>
        public string DbName { get; set; }

        /// <summary>
        ///     Affecte ou obtient l'utilisateur de la base de données
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     Affecte ou obtient le mot de passe de la base de données
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     Génère la chaine de connection à la base de données selon les paramètres donnés
        /// </summary>
        /// <returns></returns>
        public string ToConnectionString()
        {
            switch (ServerType)
            {
                case "Microsoft SQL Server":
                    var connectionStringBuilder = new SqlConnectionStringBuilder
                        {DataSource = ServerHost, InitialCatalog = DbName, UserID = UserName, Password = Password};
                    return connectionStringBuilder.ConnectionString;
                default:
                    return null;
            }
        }
    }
}