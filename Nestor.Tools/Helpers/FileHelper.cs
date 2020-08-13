using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Nestor.Tools.Exceptions;

namespace Nestor.Tools.Helpers
{
    public static class FileHelper
    {
        /// <summary>
        ///     Méthode qui détecte l'encodage du flux fourni en paramètre.
        /// </summary>
        /// <param name="fileStream">Fichier sous forme de flux</param>
        /// <returns>Encodage détecté</returns>
        public static Encoding DetectEncoding(FileStream fileStream)
        {
            using (var reader = new StreamReader(fileStream.Name))
            {
                reader.Peek();
                return reader.CurrentEncoding;
            }
        }

        /// <summary>
        ///     Méthode qui détecte l'encodage du flux fourni en paramètre.
        /// </summary>
        /// <param name="fileStream">Chemin du fichier</param>
        /// <returns>Encodage détecté</returns>
        public static Encoding DetectEncoding(string filePath)
        {
            if (!File.Exists(filePath))
                throw new NestorException("Détection de l'encodage impossible, le fichier spécifié n'existe pas.");

            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return DetectEncoding(fileStream);
            }
        }

        /// <summary>
        ///     retourne un objet stream du fichier
        /// </summary>
        /// <param name="fileFullPath">chemin complet du fichier</param>
        /// <returns></returns>
        public static Stream GetFileStream(string fileFullPath)
        {
            if (File.Exists(fileFullPath))
                return File.Open(fileFullPath, FileMode.Open);

            return null;
        }

        public static byte[] StreamFile(this FileInfo file)
        {
            return StreamFile(file.FullName);
        }

        public static byte[] StreamFile(this string filename)
        {
            var fs = new FileStream(filename, FileMode.Open, FileAccess.Read);

            // Create a byte array of file stream length
            var ImageData = new byte[fs.Length];

            //Read block of bytes from stream into the byte array
            fs.Read(ImageData, 0, Convert.ToInt32(fs.Length));

            //Close the File Stream
            fs.Close();
            return ImageData; //return the byte data
        }

        /// <summary>
        ///     Supprime un fichier, renvoi false si non réussi
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <returns></returns>
        public static bool DeleteFile(string fileFullPath)
        {
            try
            {
                File.Delete(fileFullPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Retourne un nom de fichier non existant dans le dossier en cours
        /// </summary>
        /// <param name="newName">nom du fichier à vérifier</param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static string GetNewFileName(string newName, DirectoryInfo dir)
        {
            var existanceIndex = 0; //si la ressource existe on préfixe

            if (dir.Exists)
                if (dir.GetFiles().Any(f => f.Name.Replace(f.Extension, "") == newName))
                {
                    //si oui on cherche si elle existe en préfixant avec un nombre, que l'on incrémente au besoin
                    while (dir.GetFiles().Any(f =>
                        f.Name.Replace(f.Extension, "") == string.Format("{0}_{1}", newName, existanceIndex.ToString()))
                    ) existanceIndex++;
                    //quand on a determiner pour quel valeur le nom est utilisable on le met
                    newName += string.Format("_{0}", existanceIndex.ToString());
                }

            //on retourne le nouveau nom 
            return newName;
        }

        public static string HachFile(string filePath)
        {
            FileStream fileToHash;
            byte[] byHash;

            try
            {
                // Vars initialization            
                fileToHash = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);

                // Hash generation
                MD5 md5 = new MD5CryptoServiceProvider();
                byHash = md5.ComputeHash(fileToHash);

                // Hash byte[] convertion to hex string
                var sHashHexString = "";

                for (var _i = 0; _i < byHash.Length; _i++)
                    sHashHexString += byHash[_i].ToString("X2");

                fileToHash.Close();
                return sHashHexString.ToLower();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}