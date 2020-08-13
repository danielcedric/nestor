using System.Collections.Generic;

namespace Nestor.Tools.Infrastructure.Settings
{
    public class AppSettings
    {
        /// <summary>
        ///     Affecte ou obtient le nom de la chaine de connexion à utiliser
        /// </summary>
        public string UseConnectionString { get; set; }

        /// <summary>
        ///     Affecte ou obtient un booléen qui indique si le tracking des entités est activé ou non
        /// </summary>
        public bool TrackEntities { get; set; } = false;

        /// <summary>
        ///     Affecte ou obtient la liste des chaine de connexions
        /// </summary>
        public List<ConnectionStringSettings> ConnectionStrings { get; set; } = new List<ConnectionStringSettings>();
    }

    public class ConnectionStringSettings
    {
        /// <summary>
        ///     Affecte ou obtient le nom de la chaine de connexion
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Affecte ou obtient la chaine de connexion à la base de données
        /// </summary>
        public string ConnectionString { get; set; }
    }
}