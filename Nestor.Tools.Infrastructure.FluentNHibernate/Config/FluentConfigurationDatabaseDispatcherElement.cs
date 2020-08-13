using System.Configuration;

namespace Nestor.Tools.Infrastructure.FluentNHibernate.Config
{
    public class FluentConfigurationDatabaseDispatcherElement : ConfigurationElement
    {
        /// <summary>
        ///     Obtient le nom de la clé de la chaine de connexion à la base de données
        /// </summary>
        [ConfigurationProperty("connectionStringName", IsRequired = true)]
        public string ConnectionStringName => this["connectionStringName"] as string;

        ///// <summary>
        ///// Obtient le type de base de données
        ///// </summary>
        //[ConfigurationProperty("databaseType", IsRequired = true, DefaultValue = ORMapping.DatabaseType.UNKNOWN)]
        //public ORMapping.DatabaseType DatabaseType
        //{
        //    get
        //    {
        //        try
        //        {
        //            return (ORMapping.DatabaseType)Enum.Parse(typeof(ORMapping.DatabaseType), this["databaseType"].ToString());
        //        }
        //        catch (Exception)
        //        {
        //            return ORMapping.DatabaseType.UNKNOWN;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Obtient le mode de management de la base de données
        ///// </summary>
        //[ConfigurationProperty("mode", IsRequired = true, DefaultValue = ORMapping.SchemaManagementMode.Validate)]
        //public ORMapping.SchemaManagementMode SchemaManagementMode
        //{
        //    get
        //    {
        //        try
        //        {
        //            return (ORMapping.SchemaManagementMode)Enum.Parse(typeof(ORMapping.SchemaManagementMode), this["mode"].ToString());
        //        }
        //        catch (Exception)
        //        {
        //            return ORMapping.SchemaManagementMode.Validate;
        //        }
        //    }
        //}

        /// <summary>
        ///     Retourne les assemblies pour chaque configuration
        /// </summary>
        [ConfigurationProperty("assemblies")]
        [ConfigurationCollection(typeof(FluentConfigurationAssemblyElement), AddItemName = "assembly")]
        public FluentConfigurationAssemblyCollection Assemblies =>
            this["assemblies"] as FluentConfigurationAssemblyCollection;
    }
}