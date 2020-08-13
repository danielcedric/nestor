namespace Nestor.Tools.Infrastructure.EntityFramework.Models
{
    public class DbOperationResult
    {
        #region Enum

        public enum DbOperationStatus
        {
            Success,
            Error,
            NotExecuted
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Affecte ou obtient la commande SQL
        /// </summary>
        public string SqlCommand { get; set; }

        /// <summary>
        ///     Affecte ou obtient l'état de la requête
        /// </summary>
        public DbOperationStatus Status { get; set; }

        /// <summary>
        ///     Affecte ou obtient le message
        /// </summary>
        public string Message { get; set; }

        #endregion

        #region Constructors

        public DbOperationResult() : this(null, DbOperationStatus.NotExecuted)
        {
        }

        public DbOperationResult(string cmd) : this(cmd, DbOperationStatus.NotExecuted)
        {
        }

        public DbOperationResult(string sqlCommand, DbOperationStatus status)
        {
            SqlCommand = sqlCommand;
            Status = status;
        }

        #endregion
    }
}