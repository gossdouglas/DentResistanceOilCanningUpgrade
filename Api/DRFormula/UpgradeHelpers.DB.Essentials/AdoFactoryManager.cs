// Author: mrojas
// Project: UpgradeHelpers.DB
// Path: UpgradeHelpers\VB6\DB
// Creation date: 8/6/2009 2:29 PM
// Last modified: 8/21/2009 10:50 AM

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.CompilerServices;

#if NET_CORE_APP
[assembly: InternalsVisibleTo("UpgradeHelpers.DB.Essentials.NetCore.Test")]
#else
[assembly: InternalsVisibleTo("UpgradeHelpers.DB.Essentials.Test")]
#endif

namespace UpgradeHelpers.DB
{
    /// <summary>
    /// This class provides a set of methods and internal classes to read the provider information from the xml configuration file.
    /// </summary>
    public class AdoFactoryManager
    {

        private static System.Threading.ReaderWriterLockSlim lockFactorySection = new System.Threading.ReaderWriterLockSlim();
        private static bool _factorySectionAlreadyInitialized;
        private static Dictionary<string, FactoryConfigurationElement> _factorySection;

        internal static string GetFactoryNameFromProviderType(Type providerType)
        {
            InitializeFactorySection();
            try
            {
                lockFactorySection.EnterReadLock();
                foreach (FactoryConfigurationElement factory in _factorySection.Values)
                {
                    if (factory.FactoryType == providerType.Namespace)
                        return factory.Name;
                }
                return String.Empty;
            }
            finally
            {
                lockFactorySection.ExitReadLock();
            }
        }

        /// <summary>
        /// Gets the factory section instance.
        /// </summary>
        internal static void InitializeFactorySection()
        {
            try
            {
                lockFactorySection.EnterUpgradeableReadLock();
                if (_factorySectionAlreadyInitialized)
                {
                    return;
                }

                if (ConfigurationManager.GetSection(AdoFactoriesConfigurationSection.SECTION_NAME) != null)
                {
                    AdoFactoriesConfigurationSection configFileSection = ConfigurationManager.GetSection(AdoFactoriesConfigurationSection.SECTION_NAME) as AdoFactoriesConfigurationSection;
                    if (configFileSection != null)
                    {
                        try
                        {
                            lockFactorySection.EnterWriteLock();
                            _factorySection = new Dictionary<string, FactoryConfigurationElement>();
                            foreach (FactoryConfigurationElement elem in configFileSection.Factories)
                            {
                                if (!_factorySection.ContainsKey(elem.Name))
                                    _factorySection.Add(elem.Name, elem);
                            }
                        }
                        finally
                        {
                            lockFactorySection.ExitWriteLock();
                        }
                    }
                }
                try
                {
                    lockFactorySection.EnterWriteLock();
                    if (_factorySection == null)
                    {
                        _factorySection = new Dictionary<string, FactoryConfigurationElement>();
                        LoadDefaultFactorySettings(_factorySection);
                    }
                }
                finally
                {
                    lockFactorySection.ExitWriteLock();
                }
                if (_factorySection == null)
                    throw new ArgumentException("There was an error getting the configuration file information. Please check the configuration file.");
            }
            finally
            {
                _factorySectionAlreadyInitialized = true;
                lockFactorySection.ExitUpgradeableReadLock();
            }
        }

        private static void LoadDefaultFactorySettings(Dictionary<string, FactoryConfigurationElement> factorySection)
        {
            factorySection.Add("Access", new FactoryConfigurationElement("Access", "System.Data.OleDb", DatabaseType.Access, true));
            factorySection.Add("SQLServer", new FactoryConfigurationElement("SQLServer", "System.Data.SqlClient", DatabaseType.SQLServer, false));
            //New Changes
            //factorySection.Add("Oracle", new FactoryConfigurationElement("Oracle", "Oracle.DataAccess.Client", DatabaseType.Oracle, false));
            factorySection.Add("Oracle", new FactoryConfigurationElement("Oracle", "System.Data.OracleClient", DatabaseType.Oracle, false));
            factorySection.Add("ODBC", new FactoryConfigurationElement("ODBC", "System.Data.Odbc", DatabaseType.Access, false));
        }

        /// <summary>
        /// Gets the default factory.
        /// </summary>
        /// <returns>The default DBProviderFactory specified on the xml configuration file.</returns>
        public static DbProviderFactory GetFactory()
        {
            return GetFactory(string.Empty);
        }

        internal static FactoryConfigurationElement Default
        {
            get
            {
                InitializeFactorySection();
                try
                {
                    lockFactorySection.EnterReadLock();
                    foreach (FactoryConfigurationElement factory in _factorySection.Values)
                        if (factory.IsDefault)
                            return factory;
                }
                finally
                {
                    lockFactorySection.ExitReadLock();
                }
                return null;

            }
        }

		/// <summary>
		/// Gets the factory according to the factory name.
		/// Note for OleDB, connectionString might differ based on x86/x64 environments.
		///	ConnectionStringx86 = "Provider=Microsoft.Jet.OLEDB.4.0;...";
		///	ConnectionStringx64 = "Provider=Microsoft.ACE.OLEDB.12.0;...";
		/// </summary>
		/// <param name="factoryName">Name of the desire factory.</param>
		/// <returns>Returns the DBProviderFactory that represents the parameter factoryName.</returns>
		public static DbProviderFactory GetFactory(String factoryName)
        {
            if (String.IsNullOrEmpty(factoryName))
                return DbProviderFactories.GetFactory(DefaultFactoryType());
            InitializeFactorySection();
            FactoryConfigurationElement configurationElement;
            bool found;
            try
            {
                lockFactorySection.EnterReadLock();
                found = _factorySection.TryGetValue(factoryName, out configurationElement);
            }
            finally
            {
                lockFactorySection.ExitReadLock();
            }
            if (!found || configurationElement == null || string.IsNullOrEmpty(configurationElement.FactoryType))
                throw new ArgumentException("The factory " + factoryName + " is not registered on the configuration file. Check your App.config file. More information on: https://www.mobilize.net/vbtonet/app.config-and-database-access/");
            return DbProviderFactories.GetFactory(configurationElement.FactoryType);
        }


        /// <summary>
        /// Gets the design mode flag.
        /// </summary>
        protected static bool InDesignMode
        {
            get
            {
                return Process.GetCurrentProcess().ProcessName == "devenv";
            }
        }

        /// <summary>
        /// Just to avoid several repeated error messages
        /// </summary>
        static String LastFactoryNotFound = String.Empty;

        /// <summary>
        /// Gets the factory database type according to the factory name.
        /// </summary>
        /// <param name="factoryName">Name of the desire factory.</param>
        /// <returns>Returns the DatabaseType that represents the parameter factoryName.</returns>
        public static DatabaseType GetFactoryDbType(String factoryName)
        {

            InitializeFactorySection();



            bool hasKey;
            FactoryConfigurationElement configurationElement;
            try
            {
                lockFactorySection.EnterReadLock();
                hasKey = _factorySection.TryGetValue(factoryName, out configurationElement);
                hasKey = hasKey && configurationElement != null;
            }
            finally
            {
                lockFactorySection.ExitReadLock();
            }

            if (!hasKey)
            {
                if (InDesignMode && LastFactoryNotFound.Equals(factoryName))
                {
                    LastFactoryNotFound = factoryName;
                }
                return DatabaseType.Undefined;
            }

            return configurationElement.DatabaseType;
        }

        private static String DefaultFactoryType()
        {
            InitializeFactorySection();
            FactoryConfigurationElement factory = null;
            try
            {
                lockFactorySection.EnterReadLock();
                foreach (FactoryConfigurationElement e in _factorySection.Values)
                {
                    factory = e;
                    if (factory.IsDefault)
                        break;
                }
            }
            finally
            {
                lockFactorySection.ExitReadLock();
            }
            if (factory == null) return null;
            return factory.FactoryType;
        }

    }

    /// <summary>
    /// Class to handle the custom configuration section.
    /// </summary>
    internal class AdoFactoriesConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// Name of the section.
        /// </summary>
        public const string SECTION_NAME = "AdoFactories";

        /// <summary>
        /// Gets and sets the factories collection.
        /// </summary>
        [ConfigurationProperty("Factories", IsRequired = true)]
        public FactoryConfigurationElementsCollection Factories
        {
            get { return (FactoryConfigurationElementsCollection)base["Factories"]; }
            set { base["Factories"] = value; }
        }

    }

	/// <summary>
	/// Represents the configuration file factory element.
	/// </summary>
	internal class FactoryConfigurationElement : ConfigurationElement
    {
        /// <summary>
        /// Creates a new element instance.
        /// </summary>
        public FactoryConfigurationElement()
        {
        }

        /// <summary>
        /// Creates a new element instance.
        /// </summary>
        /// <param name="factoryName">The factory name which represents this element.</param>
        public FactoryConfigurationElement(String factoryName)
        {
            Name = factoryName;
        }

        /// <summary>
        /// Creates a new element instance.
        /// </summary>
        /// <param name="factoryName">The factory name which represents this element.</param>
        /// <param name="factoryType">The name of the factory type.</param>
        /// <param name="isDefault">Marks if the factory is the default.</param>
        /// <param name="databasetype">The database type.</param>
        public FactoryConfigurationElement(String factoryName, String factoryType, DatabaseType databasetype, bool isDefault)
        {
            Name = factoryName;
            FactoryType = factoryType;
            IsDefault = isDefault;
            DatabaseType = databasetype;
        }

        /// <summary>
        /// Gets and sets the factory name.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public String Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Gets and sets the factory type.
        /// </summary>
        [ConfigurationProperty("factorytype", IsRequired = true)]
        public String FactoryType
        {
            get { return (string)this["factorytype"]; }
            set { this["factorytype"] = value; }
        }

        /// <summary>
        /// Gets and sets the database type.
        /// </summary>
        [ConfigurationProperty("databasetype", IsRequired = true)]
        public DatabaseType DatabaseType
        {
            get { return (DatabaseType)this["databasetype"]; }
            set { this["databasetype"] = value; }
        }

        /// <summary>
        /// Gets and sets the default flag.
        /// </summary>
        [ConfigurationProperty("isdefault", IsRequired = true, DefaultValue = false)]
        public bool IsDefault
        {
            get { return (bool)this["isdefault"]; }
            set { this["isdefault"] = value; }
        }
    }

    /// <summary>
    /// The collection of factory elements.
    /// </summary>
    internal class FactoryConfigurationElementsCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Creates a new element.
        /// </summary>
        /// <param name="elementName">The name of the new element.</param>
        /// <returns>The new element.</returns>
        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new FactoryConfigurationElement(elementName);
        }

        /// <summary>
        /// Creates a new element.
        /// </summary>
        /// <returns>The new element.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new FactoryConfigurationElement();
        }

        /// <summary>
        /// Gets the key of the element in the collection.
        /// </summary>
        /// <param name="element">The element to get the key from.</param>
        /// <returns>The element key.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FactoryConfigurationElement)element).Name;
        }

        /// <summary>
        /// Indexer to retrieve an specific element.
        /// </summary>
        /// <param name="name">The element key.</param>
        /// <returns>The element corresponding to the key.</returns>
        public new FactoryConfigurationElement this[String name]
        {
            get
            {
                return (FactoryConfigurationElement)BaseGet(name);
            }
        }

        /// <summary>
        /// Indexer to retrieve an specific element.
        /// </summary>
        /// <param name="index">The index key.</param>
        /// <returns>The element corresponding to the index.</returns>
        public FactoryConfigurationElement this[int index]
        {
            get
            {
                return (FactoryConfigurationElement)BaseGet(index);
            }
        }

        /// <summary>
        /// Gets the default factory type.
        /// </summary>
        /// <returns>The type name of the element.</returns>
        public String DefaultFactoryType()
        {
            FactoryConfigurationElement factory = null;
            foreach (ConfigurationElement e in this)
            {
                factory = e as FactoryConfigurationElement;
                if (factory.IsDefault)
                    break;
            }
            return factory.FactoryType;
        }

        /// <summary>
        /// Gets the factory dbtype according to the parameter.
        /// </summary>
        /// <param name="factoryname">The factory name to get the information from.</param>
        /// <returns></returns>
        public DatabaseType FactoryDbType(String factoryname)
        {
            FactoryConfigurationElement factory = null;
            if (String.IsNullOrEmpty(factoryname))
            {
                foreach (ConfigurationElement e in this)
                {
                    factory = e as FactoryConfigurationElement;
                    if (factory.IsDefault)
                        break;
                }
            }
            else
                factory = this[factoryname];
            return factory.DatabaseType;
        }

        /// <summary>
        /// Adds a new element to the collection.
        /// </summary>
        /// <param name="factory">The factory to be added.</param>
        public void Add(FactoryConfigurationElement factory)
        {
            BaseAdd(factory);
        }

        /// <summary>
        /// Adds a new element to the collection.
        /// </summary>
        /// <param name="element">The element to be added.</param>
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

    }
}
