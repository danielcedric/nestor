namespace Nestor.Tools.Application.Abstractions.DTOs
{
    /// <summary>
    /// Classe représentant la base d'un DTO
    /// Utilisé pour représenter les informations que tous les DTOs doivent avoir
    /// </summary>
    public abstract class BaseDTO
    {
        /// <summary>
        /// URL de l'objet dans l'application
        /// </summary>
        public abstract string Url { get; }
    }
}
