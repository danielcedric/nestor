using System.Configuration;

namespace Nestor.Tools.Infrastructure.FluentNHibernate.Config
{
    public class FluentConfigurationAssemblyElement : ConfigurationElement
    {
        /// <summary>
        /// Obtient le nom de la clé associée à l'élément
        /// </summary>
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get
            {
                return this["key"] as string;
            }
        }

        /// <summary>
        /// Obtient le nom de l'assembly associé à l'élément
        /// </summary>
        [ConfigurationProperty("assembly", IsRequired = true)]
        public string Assembly
        {
            get
            {
                return this["assembly"] as string;
            }
        }
    }
}
