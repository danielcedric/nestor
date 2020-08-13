using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Nestor.Tools.Assemblies
{
    public class AssemblyInfo
    {
        #region Attributes

        private readonly Assembly _assembly;

        #endregion

        #region Constructors

        public AssemblyInfo(Assembly assembly)
        {
            _assembly = assembly;
        }

        #endregion

        #region Methods

        //public string GetNetworkDeployedVersion()
        //{
        //    if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
        //        return System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
        //    else
        //        return string.Empty;
        //}

        public override string ToString()
        {
            return string.Format("{0}/{1} : {2}", Company, Product, Version);
        }

        #endregion

        #region Properties

        private string _title = string.Empty;

        /// <summary>
        ///     Obtient le titre du produit à partir de l'assembly
        /// </summary>
        public string Title
        {
            get
            {
                if (_assembly != null)
                {
                    var customAttributes = _assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                    if (customAttributes != null && customAttributes.Length > 0)
                        _title = ((AssemblyTitleAttribute) customAttributes[0]).Title;
                    if (string.IsNullOrEmpty(_title))
                        _title = string.Empty;
                }

                return _title;
            }
        }

        private string _company = string.Empty;

        /// <summary>
        ///     Obtient le nom de l'entreprise éditrice du produit à partir de l'assembly
        /// </summary>
        public string Company
        {
            get
            {
                if (_assembly != null)
                {
                    var customAttributes = _assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                    if (customAttributes != null && customAttributes.Length > 0)
                        _company = ((AssemblyCompanyAttribute) customAttributes[0]).Company;
                    if (string.IsNullOrEmpty(_company))
                        _company = string.Empty;
                }

                return _company;
            }
        }

        private Version _version;

        /// <summary>
        ///     Obtient la version du produit à partir de l'assembly
        /// </summary>
        public Version Version
        {
            get
            {
                if (_assembly != null)
                    _version = new Version(_assembly.GetName().Version.ToString());

                return _version;
            }
        }

        private Version _fileVersion;

        /// <summary>
        ///     Obtient la version de fichier à partir de l'assembly
        /// </summary>
        public Version FileVersion
        {
            get
            {
                if (_assembly != null)
                    _fileVersion = new Version(FileVersionInfo.GetVersionInfo(_assembly.Location).FileVersion);

                return _fileVersion;
            }
        }

        private string _Product = string.Empty;

        /// <summary>
        ///     Obtient le nom du produit à partir de l'assembly
        /// </summary>
        public string Product
        {
            get
            {
                if (_assembly != null)
                {
                    var customAttributes = _assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                    if (customAttributes != null && customAttributes.Length > 0)
                        _Product = ((AssemblyProductAttribute) customAttributes[0]).Product;
                    if (string.IsNullOrEmpty(_Product))
                        _Product = string.Empty;
                }

                return _Product;
            }
        }

        private string _Copyrights = string.Empty;

        /// <summary>
        ///     Obtient le détail des copyrights à partir de l'assembly
        /// </summary>
        public string Copyrights
        {
            get
            {
                if (_assembly != null)
                {
                    var customAttributes = _assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                    if (customAttributes != null && customAttributes.Length > 0)
                        _Copyrights = ((AssemblyCopyrightAttribute) customAttributes[0]).Copyright;
                    if (string.IsNullOrEmpty(_Copyrights))
                        _Copyrights = string.Empty;
                }

                return _Copyrights;
            }
        }

        private string _Description = string.Empty;

        /// <summary>
        ///     Obtient le détail du produit à partir de l'assembly
        /// </summary>
        public string Description
        {
            get
            {
                if (_assembly != null)
                {
                    var customAttributes = _assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                    if (customAttributes != null && customAttributes.Length > 0)
                        _Description = ((AssemblyDescriptionAttribute) customAttributes[0]).Description;
                    if (string.IsNullOrEmpty(_Description))
                        _Description = string.Empty;
                }

                return _Description;
            }
        }

        private string _Path = string.Empty;

        /// <summary>
        ///     Obtient le chemin à partir de l'assembly
        /// </summary>
        public string Path
        {
            get
            {
                if (_assembly != null)
                {
                    var AssemblyFileInfo = new FileInfo(_assembly.Location);
                    if (!AssemblyFileInfo.Exists) return string.Empty;
                    return AssemblyFileInfo.DirectoryName;
                }

                return string.Empty;
            }
        }

        #endregion
    }
}