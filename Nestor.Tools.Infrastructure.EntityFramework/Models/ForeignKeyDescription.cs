using Microsoft.EntityFrameworkCore.Migrations;

namespace Nestor.Tools.Infrastructure.EntityFramework.Models
{
    public class ForeignKeyDescription
    {
        #region Properties

        /// <summary>
        ///     Affecte ou obtient le nom de la contrainte
        /// </summary>
        public string ConstraintName { get; set; }

        /// <summary>
        ///     Affecte ou obtient le nom de la table qui contient la contrainte
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        ///     Affecte ou obtient le nom de la colonne sur laquelle la contrainte s'applique
        /// </summary>
        public string ConstraintColumnName { get; set; }

        /// <summary>
        ///     Affecte ou obtient la table référncée
        /// </summary>
        public string ReferencedTable { get; set; }

        /// <summary>
        ///     Affecte ou obtient le nom de la colonne référencée par la contrainte
        /// </summary>
        public string ReferencedColumnName { get; set; }

        /// <summary>
        ///     Affecte ou obtient si la contrainte est supprimée
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        ///     Affecte ou obtient l'action à faire en cas de suppression de la clé primaire
        /// </summary>
        public ReferentialAction OnDeleteAction { get; set; } = ReferentialAction.NoAction;

        /// <summary>
        ///     Affecte ou obtient l'action à faire en cas de mise à jour de la clé primaire
        /// </summary>
        public ReferentialAction OnUpdateAction { get; set; } = ReferentialAction.NoAction;

        #endregion
    }
}