using System.Net;
using System.Net.Mail;

namespace Nestor.Tools.Helpers
{
    public class EmailHelper
    {
        #region Properties
        ///// <summary>
        ///// Affecte ou obtient l'hôte SMTP
        ///// </summary>
        //public static string SmtpHost { get; set; }
        ///// <summary>
        ///// Affecte ou obtient le port SMTP à utiliser
        ///// </summary>
        //public static int SmtpPort { get; set; }
        ///// <summary>
        ///// Affecte ou obtient le nom d'utilisateur à utiliser pour se connecter au SMTP
        ///// </summary>
        //public static string SmtpUsername { get; set; }
        ///// <summary>
        ///// Affecte ou obtient le mot de passe à utiliser pour se connecter au SMTP
        ///// </summary>
        //public static string SmtpPassword { get; set; }
        #endregion

        /// <summary>
        /// Envoi un mail avec les informations passées en paramètre
        /// </summary>
        /// <param name="smtpHost">Hote du serveur mail</param>
        /// <param name="smtpPort">Port d'envoi du serveur mail</param>
        /// <param name="smtpUsername">Nom de connexion du serveur mail</param>
        /// <param name="smtpPassword">Mot de passe du serveur mail</param>
        /// <param name="header">Champ d'entête du mail</param>
        /// <param name="body">Corps du mail</param>
        /// <param name="toAddress">List des destinataires</param>
        //public static void SendMail(string smtpHost, int smtpPort, string smtpUsername, string smtpPassword, string header, string body, string[] toAddress)
        //{
        //    //EmailHelper.SmtpHost = smtpHost;
        //    //EmailHelper.SmtpPort = SmtpPort;
        //    //EmailHelper.SmtpUsername = smtpUsername;
        //    //EmailHelper.SmtpPassword = smtpPassword;

        //    SendMail(header, body, toAddress); // Envoi du mail
        //}



        /// <summary>
        /// Méthode pour envoyer un mail
        /// </summary>
        /// <param name="host">Hôte du serveur de mail</param>
        /// <param name="port">Port SMTP du serveur de mail</param>
        /// <param name="smtpUserName">Nom de connexion du serveur mail</param>
        /// <param name="smtpPassword">Mot de passe de connexion du serveur mail</param>
        /// <param name="fromAddress">Adresse depuis laquelle sera envoyée le mail</param>
        /// <param name="toAddress">Adresse à laquelle envoyer le mail</param>
        /// <param name="header">Entête du mail</param>
        /// <param name="body">Corps du message</param>
        public static void SendMail(string host, int port, string smtpUserName, string smtpPassword, string fromAddress, string toAddress, string header, string body)
        {
            // Obtention de la configuration SMTP
            //var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            //if (smtpSection == null)
            //    throw new NullReferenceException("La section 'system.net/mailSettings/smtp' n'est pas définie dans le fichier de configuration de l'application");

            MailMessage message = new MailMessage(!string.IsNullOrEmpty(fromAddress) ? fromAddress : fromAddress, toAddress);
            message.IsBodyHtml = true;
            message.Subject = header;
            message.Body = body;

            // Envoi du mail
            SmtpClient client = new SmtpClient(host, port);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(smtpUserName, smtpPassword);
            client.EnableSsl = false;

            client.Timeout = 3000000;
            client.ServicePoint.MaxIdleTime = System.Threading.Timeout.Infinite;

            client.Send(message);
        }
    }
}
