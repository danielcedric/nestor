using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Nestor.Tools.Helpers
{
    public class AllMySmsHelper
    {
        #region Constructors

        public AllMySmsHelper(string apiUrl, string apiLogin, string apiKey, string senderName, string masterAccount)
        {
            ApiUrl = new Uri(apiUrl);
            ApiLogin = apiLogin;
            ApiKey = apiKey;
            SenderName = senderName;
            MasterAccount = masterAccount;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Envoi un SMS
        /// </summary>
        /// <param name="internationalReceiverPhone"></param>
        /// <param name="message"></param>
        public string SendSms(string internationalReceiverPhone, string message)
        {
            var smsData = new SmsContainer(SenderName, MasterAccount, message);
            smsData.Data.Receivers.Add(new SmsReceiver(internationalReceiverPhone));

            var data = string.Format("login={0}&apiKey={1}&smsData={2}",
                ApiLogin,
                ApiKey,
                JsonConvert.SerializeObject(smsData));

            var Buffer = Encoding.UTF8.GetBytes(data);

            var request = (HttpWebRequest) WebRequest.Create(ApiUrl);

            request.Method = WebRequestMethods.Http.Post;
            request.ContentLength = Buffer.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            using (var writer = request.GetRequestStream())
            {
                writer.Write(Buffer, 0, Buffer.Length);
                writer.Flush();
                writer.Close();
            }

            var response = (HttpWebResponse) request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var tmp = reader.ReadToEnd();
                response.Close();
                return tmp;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Affecte ou obtient l'url d'accès à l'APi
        /// </summary>
        public Uri ApiUrl { get; }

        /// <summary>
        ///     Affecte ou obtient le login de connexion
        /// </summary>
        public string ApiLogin { get; }

        /// <summary>
        ///     Affecte ou obtient la clé d'API
        /// </summary>
        public string ApiKey { get; }

        /// <summary>
        ///     Affecte ou obtient le nom de l'expéditeur
        /// </summary>
        public string SenderName { get; }

        /// <summary>
        ///     Affecte ou obtient le compte maitre (si utilisation de sous-comtpes)
        /// </summary>
        public string MasterAccount { get; set; }

        #endregion

        #region NestedClass

        public class SmsContainer
        {
            #region Constructors

            public SmsContainer(string senderName, string masterAccount, string message)
            {
                Data = new SmsData(senderName, masterAccount, message);
            }

            #endregion

            /// <summary>
            ///     Affecte ou obtient les données du SMS
            /// </summary>
            [JsonProperty("DATA")]
            public SmsData Data { get; set; }

            #region NestedClass

            public class SmsData
            {
                #region Properties

                /// <summary>
                ///     Affecte ou obtient le contenu du message
                /// </summary>
                [JsonProperty("MESSAGE")]
                public string Message { get; set; }

                /// <summary>
                ///     Affecte ou obtient le nom de l'expéditeur
                /// </summary>
                [JsonProperty("TPOA")]
                public string SenderName { get; set; }

                /// <summary>
                ///     Affecte ou obtient la liste des destinataires
                /// </summary>
                [JsonProperty("SMS")]
                public List<SmsReceiver> Receivers { get; set; }

                /// <summary>
                ///     Affecte ou obtient le nom du compte maitre (utilisé si utilisation de sous-comptes)
                /// </summary>
                [JsonProperty("MASTERACCOUNT")]
                public string MasterAccount { get; set; }

                #endregion

                #region Constructors

                public SmsData()
                {
                    Receivers = new List<SmsReceiver>();
                }

                /// <summary>
                ///     Constructeur
                /// </summary>
                /// <param name="senderName">Nom de l'expéditeur</param>
                /// <param name="message">Message</param>
                public SmsData(string senderName, string masterAccount, string message) : this()
                {
                    SenderName = senderName;
                    MasterAccount = masterAccount;
                    Message = message;
                }

                #endregion
            }

            #endregion
        }

        public class SmsReceiver
        {
            #region Properties

            /// <summary>
            ///     Affecte ou obtient le numéro de téléphone
            /// </summary>
            [JsonProperty("MOBILEPHONE")]
            public string MobilePhone { get; set; }

            #endregion

            #region Constructors

            public SmsReceiver()
            {
            }

            public SmsReceiver(string phone)
            {
                MobilePhone = phone;
            }

            #endregion
        }

        #endregion
    }
}