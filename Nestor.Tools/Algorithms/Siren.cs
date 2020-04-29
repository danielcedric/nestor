namespace Nestor.Tools.Algorithms
{
    /// <summary>
    /// Vérification d'un numéro Siren.
    /// </summary>
    public static class Siren
    {

        /// <summary>
        /// Vérifie la validité d'un numéro SIREN.
        /// (Attention : cette méthode valide le numéro SIREN
        /// selon l'algorithme de Luhn, mais ne vérifie pas
        /// que le numéro soit attribué, ni à quelle entreprise).
        /// </summary>
        /// <param name="sirenToCheck">Numéro SIREN.</param>
        /// <returns>Vrai si le SIREN est valide, faux sinon.</returns>
        /// <remarks>Système d’Identification du Répertoire des ENtreprises.
        /// http://fr.wikipedia.org/wiki/SIREN
        /// </remarks>
        public static bool Check(string sirenToCheck)
        {
            return Luhn.Check(sirenToCheck, LuhnCheckType.Siren);
        }

    }
}
