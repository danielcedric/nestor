namespace Nestor.Tools.Infrastructure.Models
{
    public class SuperAdminSettings
    {
        /// <summary>
        ///     Affecte ou obtient le prénom de l'utilisateur
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     Affecte ou obtient le nom de famille de l'utilisateur
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        ///     Affecte ou obtient l'email de l'utilisateur
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Affecte ou obtient le mot de passe de l'utilisateur
        /// </summary>
        public string Password { get; set; }
    }
}