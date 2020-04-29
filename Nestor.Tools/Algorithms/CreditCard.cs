namespace Nestor.Tools.Algorithms
{
    /// <summary>
    /// Vérification d'un numéro de carte bancaire.
    /// </summary>
    public static class CreditCard
    {

        /// <summary>
        /// Vérifie la validité d'un numéro de carte bancaire.
        /// </summary>
        /// <param name="number">Numéro de carte bancaire.</param>
        /// <returns>Vrai si le numéro de carte bancaire est valide, faux sinon.</returns>
        public static bool Check(string number)
        {
            return Luhn.Check(number, LuhnCheckType.CreditCard);
        }

    }
}
