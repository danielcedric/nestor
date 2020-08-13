namespace Nestor.Tools.Algorithms
{
    /// <summary>
    ///     Vérification d'un numéro Siret.
    /// </summary>
    public static class Siret
    {
        /// <summary>
        ///     Vérifie la validité d'un numéro SIRET (et de son numéro SIREN associé).
        ///     (Attention : cette méthode valide le numéro SIRET
        ///     selon l'algorithme de Luhn, mais ne vérifie pas
        ///     que le numéro soit attribué ni à quel établissement).
        /// </summary>
        /// <param name="siretToCheck">Numéro SIRET.</param>
        /// <returns>Vrai si le SIRET, et son SIREN associé sont valides, faux sinon.</returns>
        /// <remarks>
        ///     Système d’Identification du Répertoire des ETablissements.
        ///     http://fr.wikipedia.org/wiki/SIRET
        /// </remarks>
        public static bool Check(string siretToCheck)
        {
            // Les numéros Siren et Siret doivent être valides tous les deux :
            return Luhn.Check(siretToCheck, LuhnCheckType.Siret) && Siren.Check(siretToCheck.Substring(0, 9));
        }
    }
}