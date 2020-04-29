using System;
using System.Collections.Generic;

namespace Nestor.Tools.Helpers
{
    class EquationSolvingHelper
    {

        /// <summary>
        /// Enumeration permettant d'évaluer l'associativité de la valeur située soit à gauche, soit à droite de l'opérateur
        /// </summary>
        private enum AssocSide { Left = 0, Right = 1 }

        #region Properties
        /// <summary>
        /// Obtient un dictionnaire des opérateurs algébriques de base avec pour valeur 
        /// </summary>
        private static IDictionary<string, int[]> Operators
        {
            get
            {
                IDictionary<string, int[]> operators = new Dictionary<string, int[]>();
                operators.Add("+", new int[] { 0, (int)AssocSide.Left });
                operators.Add("-", new int[] { 0, (int)AssocSide.Left });
                operators.Add("*", new int[] { 5, (int)AssocSide.Left });
                operators.Add("/", new int[] { 5, (int)AssocSide.Left });

                return operators;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Transforme une équation splitée terme par terme en un graphe RPN
        /// </summary>
        /// <param name="inputTokens">equation splitée terme par terme afin de résoudre une équation de type : (False * False) + False * (True * True)</param>
        /// <returns>résultat sous forme d'une équation RPN</returns>
        /// <see cref="http://en.wikipedia.org/wiki/Shunting-yard_algorithm"/>
        /// <see cref="http://fr.wikipedia.org/wiki/Notation_polonaise_inverse"/>
        public static string[] TransformToRPN(string[] inputTokens)
        {
            List<string> outResult = new List<string>();
            Stack<string> stack = new Stack<string>();

            // Pour chacun des termes de l'équation 
            foreach (string token in inputTokens)
            {

                // Si on est sur un opérateur ie +,-,*,...
                if (IsOperator(token))
                {

                    // Tant que notre pile n'est pas vide et que l'élément au top de la pile est un opérateur
                    while (stack.Count > 0 && IsOperator(stack.Peek()))
                    {

                        // Si le jeton est placé à gauche de l'opérateur ou si le jeton est placé à droite de l'opérateur alors on place la valeur dans la liste de sortie
                        if ((IsAssociative(token, AssocSide.Left) && ComparePrecedence(token, stack.Peek()) <= 0) || (IsAssociative(token, AssocSide.Right) && ComparePrecedence(token, stack.Peek()) < 0))
                        {
                            // On dépile la valeur que l'on place dans la liste de sortie
                            outResult.Add(stack.Pop());
                            continue;
                        }
                        break;
                    }

                    // On place l'opérateur trouvé au top de la pile
                    stack.Push(token);
                }
                else if (token.Equals("("))
                {
                    // Si le jeton en cours d'itération est un caractère '(' alors on le place au top de la pile des opérateurs
                    stack.Push(token);
                }
                else if (token.Equals(")"))
                {
                    // Si je jeton en cours d'itération est un caractère ')' alors on 
                    while (stack.Count > 0 && !stack.Peek().Equals("("))
                    {
                        outResult.Add(stack.Pop());
                    }

                    // On dépile
                    stack.Pop();
                }
                else
                {
                    // On place la valeur dans la liste de sortie
                    outResult.Add(token);
                }
            }

            while (stack.Count > 0)
            {
                outResult.Add(stack.Pop());
            }

            return outResult.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double EvaluateToDouble(string input)
        {
            string[] ouput = EquationSolvingHelper.TransformToRPN(input.Split(' '));
            return EquationSolvingHelper.RPNToDouble(ouput);
        }

        /// <summary>
        /// Evalue une équation composés de booléens en utilisant l'algorithme RPN
        /// </summary>
        /// <param name="input">equation</param>
        /// <returns>résultat de l'évaluation</returns>
        public static bool EvaluateToBool(string input)
        {
            if (!string.IsNullOrEmpty(input.Trim()))
            {
                string[] ouput = EquationSolvingHelper.TransformToRPN(input.Split(' '));
                return EquationSolvingHelper.RPNToBool(ouput);
            }

            return false;
        }

        /// <summary>
        /// Résoud le graphe RPN et retourne le résultat sous forme d'un double
        /// </summary>
        /// <param name="tokens">graphe RPN à résoudre</param>
        /// <returns></returns>
        private static double RPNToDouble(string[] tokens)
        {
            Stack<string> stack = new Stack<string>();

            foreach (string token in tokens)
            {
                if (!IsOperator(token))
                {
                    stack.Push(token);
                }
                else
                {
                    double d2 = double.Parse(stack.Pop());
                    double d1 = double.Parse(stack.Pop());

                    double result = token.CompareTo("+") == 0 ? d1 + d2 :
                        token.CompareTo("-") == 0 ? d1 - d2 :
                        token.CompareTo("*") == 0 ? d1 * d2 :
                        d1 / d2;

                    // On place le résultat dans la pile
                    stack.Push(result.ToString());
                }
            }

            return double.Parse(stack.Pop());
        }

        /// <summary>
        /// Evalue le graphe RPN et retourne le résultat sous forme d'un booléen
        /// </summary>
        /// <param name="tokens">tableau d'expression booléeene à évaluer</param>
        /// <returns></returns>
        private static bool RPNToBool(string[] tokens)
        {
            Stack<string> stack = new Stack<string>();

            foreach (var token in tokens)
            {
                if (!IsOperator(token))
                {
                    stack.Push(token);
                }
                else
                {
                    bool b2 = bool.Parse(stack.Pop());
                    bool b1 = bool.Parse(stack.Pop());

                    bool result = token.CompareTo("*") == 0 ? b1 && b2 : b1 || b2;
                    stack.Push(result.ToString());

                }
            }

            return bool.Parse(stack.Pop());
        }

        /// <summary>
        /// Obtient si le jeton passé en paramètre est un opérateur
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static bool IsOperator(string token)
        {
            return Operators.ContainsKey(token);
        }

        /// <summary>
        /// Test si le jeton de l'operateur est associable à une expression
        /// </summary>
        /// <param name="token"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsAssociative(string token, AssocSide side)
        {
            if (!IsOperator(token))
            {
                throw new ArgumentException(string.Format("Jeton invalide: {0}", token));
            }

            if (Operators[token][1] == (int)side)
                return true;

            return false;
        }

        /// <summary>
        /// Compare la précédence de l'opérateur
        /// </summary>
        /// <param name="token1"></param>
        /// <param name="token2"></param>
        /// <returns></returns>
        private static int ComparePrecedence(string token1, string token2)
        {
            if (!IsOperator(token1) || !IsOperator(token2))
            {
                throw new ArgumentException(string.Format("Jeton invalide: {0} {1}", token1, token2));
            }

            return Operators[token1][0] - Operators[token2][0];
        }

        #endregion
    }
}
