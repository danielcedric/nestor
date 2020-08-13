using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Nestor.Tools.Helpers
{
    public class OvhServicesHelper
    {
        /// <summary>
        ///     Constructeur complet
        /// </summary>
        /// <param name="ApplicationKey">Clé d'application</param>
        /// <param name="applicationSecret">Clé secrète d'application</param>
        /// <param name="consumerKey">Clé du script qui consomme l'api</param>
        /// <param name="apiQuery">url de base de l'api</param>
        public OvhServicesHelper(string applicationKey, string applicationSecret, string consumerKey)
        {
            ApplicationKey = applicationKey;
            ApplicationSecret = applicationSecret;
            ConsumerKey = consumerKey;
        }

        /// <summary>
        ///     Obtient le nom du compte SMS connecté
        /// </summary>
        /// <returns>sms-XX0000-1</returns>
        public string GetServiceName()
        {
            var method = "GET";
            var query = "https://eu.api.ovh.com/1.0/sms/";
            var body = string.Empty;
            var unixTimestamp = ((int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString();

            // Création de la signature
            var signature = string.Format("{0}+{1}+{2}+{3}+{4}+{5}", ApplicationSecret, ConsumerKey, method, query,
                body, unixTimestamp);

            // Création de la requête
            var req = (HttpWebRequest) WebRequest.Create(query);
            req.Method = method;
            req.ContentType = "application/json";
            req.Headers.Add("X-Ovh-Application:" + ApplicationKey);
            req.Headers.Add("X-Ovh-Consumer:" + ConsumerKey);
            req.Headers.Add("X-Ovh-Signature:" + string.Concat("$1$", SHA1Helper.Hash(signature)));
            req.Headers.Add("X-Ovh-Timestamp:" + unixTimestamp);

            try
            {
                var result = string.Empty;
                using (var response = (HttpWebResponse) req.GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            // La chaine est du type ["sms-XX0000-1"], on cherche à récupérer la valeur entre double quote.
                            var regex = new Regex(@"(?<=\["")(.*?)(?=""\])");
                            result = regex.Match(reader.ReadToEnd().Trim()).Value;
                            reader.Close();
                        }

                        responseStream.Close();
                    }

                    response.Close();
                }

                return result;
            }
            catch (WebException e)
            {
                var error = e.Message;
                using (var response = e.Response)
                {
                    using (var data = ((HttpWebResponse) response).GetResponseStream())
                    {
                        using (var reader = new StreamReader(data))
                        {
                            error = reader.ReadToEnd();
                            reader.Close();
                        }

                        data.Close();
                    }

                    response.Close();
                }

                return error;
            }
        }

        /// <summary>
        ///     Envoi un SMS au numéro de téléphone passé en paramètre
        /// </summary>
        /// <param name="internationalReceiverPhone">Numéro de téléphone au format international +33660000000</param>
        /// <param name="message">Contenu du message</param>
        /// <param name="sender">Nom de l'expéditeur qui sera affiché sur le SMS</param>
        /// <returns>{"totalCreditsRemoved":1,"invalidReceivers":[],"ids":[27814656],"validReceivers":["+33600000000"]}</returns>
        public string SendSMS(string internationalReceiverPhone, string message, string sender)
        {
            ServiceName = GetServiceName();
            if (string.IsNullOrEmpty(ServiceName))
                throw new KeyNotFoundException(
                    "Aucune clé de service OVH n'a été retournée par l'API. Le SMS n'a pas été envoyé");
            return SendSMS(ServiceName, internationalReceiverPhone, message, sender);
        }

        /// <summary>
        ///     Envoi un SMS au numéro de téléphone passé en paramètre
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="internationalReceiverPhone">Numéro de téléphone au format international +33660000000</param>
        /// <param name="message">Contenu du message</param>
        /// <param name="sender">Nom de l'expéditeur qui sera affiché</param>
        /// <returns>{"totalCreditsRemoved":1,"invalidReceivers":[],"ids":[27814656],"validReceivers":["+33600000000"]}</returns>
        public string SendSMS(string serviceName, string internationalReceiverPhone, string message, string sender)
        {
            var method = "POST";
            var query = string.Format("https://eu.api.ovh.com/1.0/sms/{0}/jobs/", serviceName);
            var body = string.Format(
                @"{{""charset"": ""UTF-8"", ""receivers"": [ ""{0}"" ], ""message"": ""{1}"", ""priority"": ""high"", ""noStopClause"": true, ""senderForResponse"": false, ""sender"": ""{2}""}}",
                internationalReceiverPhone, message, sender);
            var unixTimestamp = ((int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString();


            // Création de la signature
            var signature = string.Format("{0}+{1}+{2}+{3}+{4}+{5}", ApplicationSecret, ConsumerKey, method, query,
                body, unixTimestamp);

            // Création de la requête
            var req = (HttpWebRequest) WebRequest.Create(query);
            req.Method = method;
            req.ContentType = "application/json";
            req.Headers.Add("X-Ovh-Application:" + ApplicationKey);
            req.Headers.Add("X-Ovh-Consumer:" + ConsumerKey);
            req.Headers.Add("X-Ovh-Signature:" + string.Concat("$1$", SHA1Helper.Hash(signature)));
            req.Headers.Add("X-Ovh-Timestamp:" + unixTimestamp);

            using (var s = req.GetRequestStream())
            {
                using (var sw = new StreamWriter(s))
                {
                    sw.Write(body);
                }
            }

            try
            {
                var result = string.Empty;
                using (var response = (HttpWebResponse) req.GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            result = reader.ReadToEnd();
                            reader.Close();
                        }

                        responseStream.Close();
                    }

                    response.Close();
                }

                return result;
            }
            catch (WebException e)
            {
                var error = e.Message;
                using (var response = e.Response)
                {
                    using (var data = ((HttpWebResponse) response).GetResponseStream())
                    {
                        using (var reader = new StreamReader(data))
                        {
                            error = reader.ReadToEnd();
                            reader.Close();
                        }

                        data.Close();
                    }

                    response.Close();
                }

                return error;
            }
        }

        #region Properties

        /// <summary>
        ///     Affecte ou obtient la clé de l'application
        /// </summary>
        public string ApplicationKey { get; }

        /// <summary>
        ///     Affecte ou obtient la clé secrète de l'application
        /// </summary>
        public string ApplicationSecret { get; set; }

        /// <summary>
        ///     Affecte ou obtient la clé de l'utilisateur qui consomme l'API
        /// </summary>
        public string ConsumerKey { get; }

        /// <summary>
        ///     Affecte ou obtient le nom du service associé au compte OVH
        /// </summary>
        public string ServiceName { get; private set; }

        #endregion
    }
}