using System;
using System.Collections.Generic;
using System.Linq;

namespace Nestor.Tools.Helpers
{
    public static class AttributeHelper
    {
        /// <summary>
        ///     Obtient un dictionnaire contenant propriété / valeur pour l'objet passé en paramètre
        /// </summary>
        /// <param name="obj">Objet à évaluer</param>
        /// <param name="attributeType">
        ///     Type de l'attribut à rechercher. Seuls les attributs pouvant être assignés à ce type sont
        ///     retournés.
        /// </param>
        /// <returns></returns>
        public static IDictionary<string, object> GetValueAndPropertyNameDictionary(object obj, Type attributeType)
        {
            return GetValueAndPropertyNameDictionary(obj, attributeType, false);
        }

        /// <summary>
        ///     Obtient un dictionnaire contenant propriété / valeur pour l'objet passé en paramètre
        /// </summary>
        /// <param name="obj">Objet à évaluer</param>
        /// <param name="attributeType">
        ///     Type de l'attribut à rechercher. Seuls les attributs pouvant être assignés à ce type sont
        ///     retournés.
        /// </param>
        /// <param name="attributeInherit">
        ///     true pour rechercher les attributs dans la chaîne d'héritage de ce membre ; sinon,
        ///     false.
        /// </param>
        /// <returns></returns>
        public static IDictionary<string, object> GetValueAndPropertyNameDictionary(object obj, Type attributeType,
            bool attributeInherit)
        {
            IDictionary<string, object> result = new Dictionary<string, object>();
            foreach (var property in obj.GetType().GetProperties())
            foreach (var attribute in property.GetCustomAttributes(attributeType, attributeInherit))
                result.Add(property.Name, property.GetValue(obj));

            return result;
        }

        /// <summary>
        ///     Obtient un dictionnaire contenant propriété / valeur pour l'objet passé en paramètre.
        ///     Seules les propriétés n'étant pas "chapeautées" par les types d'attributs passés en paramètre seront évaluée
        /// </summary>
        /// <param name="obj">Objet à évaluer</param>
        /// <param name="exceptAttributeTypes">
        ///     Tableau de types d'attributs à éviter. Seuls les attributs qui ne sont pas des types
        ///     donnés sont retournée
        /// </param>
        /// <param name="attributeInherit">
        ///     true pour rechercher les attributs dans la chaîne d'héritage de ce membre ; sinon,
        ///     false.
        /// </param>
        /// <returns></returns>
        public static IDictionary<string, object> GetValueAndPropertyNameWithAttributeExceptions(object obj,
            Type[] exceptAttributeTypes, bool attributeInherit)
        {
            IDictionary<string, object> result = new Dictionary<string, object>();
            foreach (var property in obj.GetType().GetProperties())
            {
                var customAttributes = property.GetCustomAttributes(attributeInherit);
                var addValue = true;
                foreach (var attributeType in exceptAttributeTypes)
                    // Si le tableau des attributs de la propriété contient l'attribut à éviter, alors on ajoute pas la valeur au dictionnaire.
                    if (customAttributes.Any(att => att.GetType().Equals(attributeType)))
                    {
                        addValue = false;
                        break;
                    }

                if (addValue)
                    result.Add(property.Name, property.GetValue(obj));
            }

            return result;
        }
    }
}