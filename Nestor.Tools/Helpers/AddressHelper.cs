using System.Collections.Generic;
using System.Net.Http;

namespace Nestor.Tools.Helpers
{
    public class AddressHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address">Adresse à parser</param>
        /// <param name="country">pays de desination</param>
        public static GisgraphyResult ParseUsingGisgraphy(string address, string country = "fr")
        {
            using (var client = new HttpClient())
            {
                string cleanAddress = RegexHelper.Replace(address, RegexHelper.RegexType.MultipleSpaces, " ");
                string uri = $"http://services.gisgraphy.com/addressparser/?address={address}&country={country}&indent=false&format=json";

                var response = client.GetAsync(uri).Result.Content.ReadAsStringAsync().Result;
                return Newtonsoft.Json.JsonConvert.DeserializeObject<GisgraphyResult>(response);
            }
        }

        #region Nested class
        public class AddressParseResult
        {
            /// <summary>
            /// Affecte ou obtient l'indicateur de confiance du parser envers la réponse retournée
            /// </summary>
            public string Confidence { get; set; }
            /// <summary>
            /// Affecte ou obtient le nom du lieu, il est nul dans la case d'adresse mais rempli si lieu commun. Le nom est différent du nom du destinataire.
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Affecte ou obtient le nom du destinataire
            /// </summary>
            public string RecipientName { get; set; }
            /// <summary>
            /// Affecte ou obtient le numéro de l'adresse
            /// </summary>
            public string HouseNumber { get; set; }
            /// <summary>
            /// Affecte ou obtient une information complémentaire sur le numéro (bis, ter, ..)
            /// </summary>
            public string HouseNumberInfo { get; set; }
            /// <summary>
            /// Affecte ou obtient le nom de la rue
            /// </summary>
            public string StreetName { get; set; }
            /// <summary>
            /// Affecte ou obtient le type de la rue
            /// </summary>
            public string StreetType { get; set; }
            /// <summary>
            /// Affecte ou obtient le code postal
            /// </summary>
            public string ZipCode { get; set; }
            /// <summary>
            /// Affecte ou obtient le nom de la ville
            /// </summary>
            public string City { get; set; }
            /// <summary>
            /// Affecte ou obtient le nom de la sous-localité si celle-ci est rattachée à une plus grande ville
            /// </summary>
            public string DependantLocality { get; set; }
            /// <summary>
            /// Affecte ou obtient l'arrondissement
            /// </summary>
            public string Quarter { get; set; }
            /// <summary>
            /// Affecte ou obtient des informations complémentaires
            /// </summary>
            public string ExtraInfo { get; set; }
            /// <summary>
            /// Affecte ou obtient une information sur le type d'appartement
            /// </summary>
            public string SuiteType { get; set; }
            /// <summary>
            /// Affecte ou obtient le numéro de l'appartement (dans une résidence par ex.)
            /// </summary>
            public string SuiteNumber { get; set; }
            /// <summary>
            /// Affecte ou obtient la boite postale
            /// </summary>
            public string POBox { get; set; }
            /// <summary>
            /// Affecte ou obtient des informations complémentaires sur la boite postale (CEDEX, ...)
            /// </summary>
            public string POBoxInfo { get; set; }
            /// <summary>
            /// Agence ou est située la boite postale
            /// </summary>
            public string POBoxAgency { get; set; }
            /// <summary>
            /// Affecte ou obtient le pays
            /// </summary>
            public string Country { get; set; }
            /// <summary>
            /// Affecte ou obtient le code du pays
            /// </summary>
            public string CountryCode { get; set; }
            /// <summary>
            /// Affecte ou obtient la précision de la recherche
            /// </summary>
            public string GeocodingLevel { get; set; }


        }

        public class GisgraphyResult
        {
            public int NumFound { get; set; }
            public int QTime { get; set; }
            public string Attributions { get; set; }
            public List<AddressParseResult> Result { get; set; }
        }
        #endregion
    }
}
