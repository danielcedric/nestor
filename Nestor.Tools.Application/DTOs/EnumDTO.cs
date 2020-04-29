using System;

namespace Nestor.Tools.Application.DTOs
{
    public class EnumDTO<TEnum> where TEnum : struct, IConvertible
    {
        #region Properties
        /// <summary>
        /// Affecte ou obtient la clé de l'énumération
        /// </summary>
        public int Key { get; set; }
        /// <summary>
        /// Affecte ou obtient la valeur de l'énumération
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Affecte ou obtient la description de l'énumération
        /// </summary>
        public string Description { get; set; }
        
        #endregion

        #region Constructors
        public EnumDTO()
        {

        }

        /// <summary>
        /// Contructeur du DTO
        /// </summary>
        /// <param name="key">Clé de l'énumération</param>
        /// <param name="value">Valeur de l'énumération</param>
        public EnumDTO(int key, string value) : this(key, value, null)
        {

        }

        /// <summary>
        /// Contructeur du DTO
        /// </summary>
        /// <param name="key">Clé de l'énumération</param>
        /// <param name="value">Valeur de l'énumération</param>
        /// <param name="description">Description de l'énumération</param>
        public EnumDTO(int key, string value, string description)
        {
            this.Key = key;
            this.Value = value;
            this.Description = description;
        }
        #endregion
    }
}
