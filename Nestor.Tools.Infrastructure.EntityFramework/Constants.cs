namespace Nestor.Tools.Infrastructure.EntityFramework
{
    public class Constants
    {
        /// <summary>
        ///     Nom de la chaine de connexion à utiliser
        /// </summary>
        public const string DefaultConnection = nameof(DefaultConnection);

        /// <summary>
        ///     Section "Connexion Strings"
        /// </summary>
        public const string ConnectionStrings = nameof(ConnectionStrings);

        /// <summary>
        ///     Nom de la section "Settings"
        /// </summary>
        public const string AppSettings = nameof(AppSettings);

        /// <summary>
        ///     Nom de la section
        /// </summary>
        public const string Database = nameof(Database);

        /// <summary>
        ///     Clé TenantId
        /// </summary>
        public const string TenantId = "tenantid";

        /// <summary>
        ///     Description du champ TenantId pour Swagger
        /// </summary>
        public const string TenantIdSwaggerDescription =
            "Tenant Id, Type: GUID (e.g b0ed668d-7ef2-4a23-a333-94ad278f45d7)";

        /// <summary>
        ///     Parameter location
        /// </summary>
        public const string Header = "header";

        /// <summary>
        ///     Name of the device database tenant 1
        /// </summary>
        public const string DefaultTeanantDatabase = "DeviceDb";

        /// <summary>
        ///     Name of the device database tenant 2
        /// </summary>
        public const string DeviceDbTenant2 = "DeviceDb-ten2";

        /// <summary>
        ///     Guid of the first tenant
        /// </summary>
        public const string DefaultTenantGuid = "b0ed668d-7ef2-4a23-a333-94ad278f45d7";

        /// <summary>
        ///     Guid of the first tenant
        /// </summary>
        public const string Tenant2Guid = "e7e73238-662f-4da2-b3a5-89f4abb87969";

        /// <summary>
        ///     Name of the logging section in the config files
        /// </summary>
        public const string Logging = nameof(Logging);

        /// <summary>
        ///     Name of the ui culture property in the header
        /// </summary>
        public const string UICulture = "ui-culture";

        /// <summary>
        ///     Name of the culture property in the header
        /// </summary>
        public const string Culture = "culture";

        /// <summary>
        ///     Name of the culture property in the header
        /// </summary>
        public const string Query = "query";

        /// <summary>
        ///     Name of the culture property in the header
        /// </summary>
        public const string EnglishCulture = "en-US";

        /// <summary>
        ///     Name of the culture property in the header
        /// </summary>
        public const string StringInText = "string";
    }
}