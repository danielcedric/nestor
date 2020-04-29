using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Nestor.Tools.Helpers
{
    public class IOHelper
    {
        #region Gestion des répertoires
        /// <summary>
        /// Effectue une copie d'un répertoire source vers un répertoire destination
        /// </summary>
        /// <param name="sourceDirName">répertoire source à copier</param>
        /// <param name="destDirName">répertoire cible</param>
        /// <param name="copySubDirs">si vrai, copie les répertoires enfants</param>
        public static void Copy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the source directory does not exist, throw an exception.
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(string.Format("Le répertoire source n'a pas été trouvé : {0}", sourceDirName));
            }

            // If the destination directory does not exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the file contents of the directory to copy.
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                // Create the path to the new copy of the file.
                string temppath = Path.Combine(destDirName, file.Name);

                // Copy the file.
                file.CopyTo(temppath, false);
            }

            // If copySubDirs is true, copy the subdirectories.
            if (copySubDirs)
            {

                foreach (DirectoryInfo subdir in dirs)
                {
                    // Create the subdirectory.
                    string temppath = Path.Combine(destDirName, subdir.Name);

                    // Copy the subdirectories.
                    Copy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        /// <summary>
        /// vérifie que le string est correcte pour un nom de dossier (ie. ne contient pas de caractères spéciaux)
        /// </summary>
        /// <param name="name">Nom du dossier désiré</param>
        /// <returns>Retourne true si le nom est conforme</returns>
        public static bool IsAGoodDirectoryName(string name)
        {
            return !new Regex("^[^\\/?*\"><:|]*$").IsMatch(name);
        }
        #endregion

        #region Gestion des fichiers
        /// <summary>
        /// Méthode qui détecte l'encodage du flux fourni en paramètre.
        /// </summary>
        /// <param name="fileStream">Fichier sous forme de flux</param>
        /// <returns>Encodage détecté</returns>
        public static Encoding DetectFileEncoding(FileStream fileStream)
        {
            Encoding encoding = null;
            if (fileStream.CanSeek)
            {
                byte[] bom = new byte[4]; // L'encodage est défini sur les 4 premiers octets.
                fileStream.Read(bom, 0, 4);
                if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) // UTF-8
                {
                    encoding = Encoding.UTF8;
                }
                else if ((bom[0] == 0xff && bom[1] == 0xfe) || // UCS-2le, UCS-4le, and UCS-16le
                    (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff)) // UCS-4
                {
                    encoding = Encoding.Unicode;
                }
                else if (bom[0] == 0xfe && bom[1] == 0xff) // UTF-16 et UCS-2
                {
                    encoding = Encoding.BigEndianUnicode;
                }
                else // ANSI, Default -> ISO-8859-1
                {
                    encoding = Encoding.Default;
                }

                //On repositionne le curseur en début de fichier afin de permettre la lecture de celui-ci
                fileStream.Seek(0, SeekOrigin.Begin);
            }
            else
            {
                //Le flux ne permet pas la recherche à l'interieur de celui-ci (sortie de console par exemple)
                //on met donc l'encodage ISO-8859-1 par défaut.
                encoding = Encoding.Default;
            }
            return encoding;
        }

        /// <summary>
        /// Méthode qui détecte l'encodage du flux fourni en paramètre.
        /// </summary>
        /// <param name="fileStream">Chemin du fichier</param>
        /// <returns>Encodage détecté</returns>
        public static Encoding DetectFileEncoding(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Détection de l'encodage impossible, le fichier spécifié n'existe pas.");

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                return DetectFileEncoding(fileStream);
        }

        /// <summary>
        /// Retourne un nom de fichier non existant dans le dossier en cours
        /// </summary>
        /// <param name="newName">nom du fichier à vérifier</param>
        /// <param name="dir">répertoire où le fichier va être créé</param>
        /// <returns></returns>
        public static string ComposeAvailableFilename(string newName, DirectoryInfo dir)
        {
            int existanceIndex = 0;//si la ressource existe on préfixe

            if (dir.GetFiles().Any(f => f.Name.Replace(f.Extension, "") == newName))
            {
                //si oui on cherche si elle existe en préfixant avec un nombre, que l'on incrémente au besoin
                while (dir.GetFiles().Any(f => f.Name.Replace(f.Extension, "") == String.Format("{0}_{1}", newName, existanceIndex.ToString())))
                {
                    existanceIndex++;
                }
                //quand on a determiner pour quel valeur le nom est utilisable on le met
                newName += String.Format("_{0}", existanceIndex.ToString());
            }

            //on retourne le nouveau nom 
            return newName;
        }

        /// <summary>
        /// Déplace un fichier d'un emplacement source vers une destination. Si celui-ci existe déjà dans le répertoire de destination il est supprimé
        /// </summary>
        /// <param name="from">Emplacement source</param>
        /// <param name="to">Destination</param>
        /// <returns></returns>
        public static bool MoveFile(string from, string to)
        {
            return MoveFile(from, to, true);
        }

        /// <summary>
        /// Déplace un fichier d'un emplacement source vers une destination
        /// </summary>
        /// <param name="from">Emplacement source</param>
        /// <param name="to">Destination</param>
        /// <param name="deleteIfExists">Si vrai, et si le fichier existe déjà celui-ci sera supprimé à l'emplacement de destination</param>
        /// <returns></returns>
        public static bool MoveFile(string from, string to, bool deleteIfExists)
        {
            try
            {
                if (File.Exists(to) && deleteIfExists)
                    File.Delete(to);

                File.Move(from, to);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
