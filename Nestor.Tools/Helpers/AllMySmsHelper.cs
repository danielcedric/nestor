using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Nestor.Tools.Helpers
{
    public class AllMySmsHelper
    {
        #region Properties
        /// <summary>
        /// Affecte ou obtient l'url d'accès à l'APi
        /// </summary>
        public Uri ApiUrl { get; private set; }
        /// <summary>
        /// Affecte ou obtient le login de connexion
        /// </summary>
        public string ApiLogin { get; private set; }
        /// <summary>
        /// Affecte ou obtient la clé d'API
        /// </summary>
        public string ApiKey { get; private set; }
        /// <summary>
        /// Affecte ou obtient le nom de l'expéditeur
        /// </summary>
        public string SenderName { get; private set; }
        /// <summary>
        /// Affecte ou obtient le compte maitre (si utilisation de sous-comtpes)
        /// </summary>
        public string MasterAccount { get; set; }
        #endregion

        #region Constructors
        public AllMySmsHelper(string apiUrl, string apiLogin, string apiKey, string senderName, string masterAccount)
        {
            this.ApiUrl = new Uri(apiUrl);
            this.ApiLogin = apiLogin;
            this.ApiKey = apiKey;
            this.SenderName = senderName;
            this.MasterAccount = masterAccount;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Envoi un SMS
        /// </summary>
        /// <param name="internationalReceiverPhone"></param>
        /// <param name="message"></param>
        public string SendSms(string internationalReceiverPhone, string message)
        {
            SmsContainer smsData = new SmsContainer(this.SenderName, this.MasterAccount, message);
            smsData.Data.Receivers.Add(new SmsReceiver(internationalReceiverPhone));

            string data = string.Format("login={0}&apiKey={1}&smsData={2}",
                ApiLogin,
                ApiKey,
                JsonConvert.SerializeObject(smsData));

            byte[] Buffer = System.Text.Encoding.UTF8.GetBytes(data);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(ApiUrl);

            request.Method = WebRequestMethods.Http.Post;
            request.ContentLength = Buffer.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            using (Stream writer = request.GetRequestStream())
            {
                writer.Write(Buffer, 0, Buffer.Length);
                writer.Flush();
                writer.Close();
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string tmp = reader.ReadToEnd();
                response.Close();
                return tmp;
            }
        }
        #endregion

        #region NestedClass
        public class SmsContainer
        {
            /// <summary>
            /// Affecte ou obtient les données du SMS
            /// </summary>
            [JsonProperty("DATA")]
            public SmsData Data { get; set; }

            #region Constructors
            public SmsContainer(string senderName, string masterAccount, string message)
            {
                Data = new SmsData(senderName, masterAccount, message);
            }
            #endregion

            #region NestedClass
            public class SmsData
            {
                #region Properties
                /// <summary>
                /// Affecte ou obtient le contenu du message
                /// </summary>
                [JsonProperty("MESSAGE")]
                public string Message { get; set; }
                /// <summary>
                /// Affecte ou obtient le nom de l'expéditeur
                /// </summary>
                [JsonProperty("TPOA")]
                public string SenderName { get; set; }
                /// <summary>
                /// Affecte ou obtient la liste des destinataires
                /// </summary>
                [JsonProperty("SMS")]
                public List<SmsReceiver> Receivers { get; set; }

                /// <summary>
                /// Affecte ou obtient le nom du compte maitre (utilisé si utilisation de sous-comptes)
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
                /// Constructeur
                /// </summary>
                /// <param name="senderName">Nom de l'expéditeur</param>
                /// <param name="message">Message</param>
                public SmsData(string senderName, string masterAccount, string message) : this()
                {
                    this.SenderName = senderName;
                    this.MasterAccount = masterAccount;
                    this.Message = message;
                }
                #endregion
            }
            #endregion
        }
        
        public class SmsReceiver
        {
            #region Properties
            /// <summary>
            /// Affecte ou obtient le numéro de téléphone
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
                this.MobilePhone = phone;
            }
            #endregion
        }
        #endregion
    }
}
