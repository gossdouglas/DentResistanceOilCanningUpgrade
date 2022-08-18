// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace UpgradeHelpers.DB
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.Common;
	using System.Data.Odbc;
	using System.Globalization;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.InteropServices;
#if NET_CORE_APP || NET_STANDARD_APP
	using SqlClient = Microsoft.Data.SqlClient;
#else
	using SqlClient = System.Data.SqlClient;
#endif
	/// <summary>
	/// This class is added mostly for support with .NET Standard
	/// </summary>
	public static partial class DbProviderFactories
	{
		public const string SYSTEM_WINDOWS_FORMS_ASSEMBLY_NAME = @", System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
		public static readonly string BINDING_SOURCE_TYPE_NAME = string.Format("System.Windows.Forms.BindingSource{0}", SYSTEM_WINDOWS_FORMS_ASSEMBLY_NAME);
		static DbProviderFactories()
		{
			DbProviderFactories.RegisterFactory("System.Data.SqlClient", typeof(SqlClient.SqlClientFactory));
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				DbProviderFactories.RegisterFactory("System.Data.OleDb", typeof(System.Data.OleDb.OleDbFactory));
			}
			DbProviderFactories.RegisterFactory("System.Data.Odbc", typeof(System.Data.Odbc.OdbcFactory));
		}

		private struct ProviderRegistration
		{
			internal ProviderRegistration(Type factoryType, DbProviderFactory factoryInstance)
			{
				this.Factorytype = factoryType;
				this.FactoryInstance = factoryInstance;
			}

			internal Type Factorytype { get;  }
			/// <summary>
			/// The cached instance of the type in <see cref="FactoryTypeAssemblyQualifiedName"/>. If null, this registation is seen as a deferred registration
			/// and <see cref="FactoryTypeAssemblyQualifiedName"/> is checked the first time when this registration is requested through GetFactory().
			/// </summary>
			internal DbProviderFactory FactoryInstance { get; }
		}

		private static ConcurrentDictionary<string, ProviderRegistration> _registeredFactories = new ConcurrentDictionary<string, ProviderRegistration>();
		private const string AssemblyQualifiedNameColumnName = "AssemblyQualifiedName";
		private const string InvariantNameColumnName = "InvariantName";
		private const string NameColumnName = "Name";
		private const string DescriptionColumnName = "Description";
		private const string ProviderGroupColumnName = "DbProviderFactories";
		private const string InstanceFieldName = "Instance";

		public static bool TryGetFactory(string providerInvariantName, out DbProviderFactory factory)
		{
			factory = GetFactory(providerInvariantName, throwOnError: false);
			return factory != null;
		}

		public static DbProviderFactory GetFactory(string providerInvariantName)
		{
			return GetFactory(providerInvariantName, throwOnError: true);
		}

		public static DbProviderFactory GetFactory(DataRow providerRow)
		{
			if (providerRow == null)
			{
				throw new ArgumentException(nameof(providerRow));
			}

			DataColumn assemblyQualifiedNameColumn = providerRow.Table.Columns[AssemblyQualifiedNameColumnName];
			if (null == assemblyQualifiedNameColumn)
			{
				throw new ArgumentException(nameof(providerRow), "DbProviderFactories_NoAssemblyQualifiedName");
			}

			string assemblyQualifiedName = providerRow[assemblyQualifiedNameColumn] as string;
			if (string.IsNullOrWhiteSpace(assemblyQualifiedName))
			{
				throw new ArgumentException(nameof(providerRow), "DbProviderFactories_NoAssemblyQualifiedName");
			}

			return GetFactoryInstance(GetProviderTypeFromTypeName(assemblyQualifiedName));
		}


		public static DbProviderFactory GetFactory(DbConnection connection)
		{
			if (connection == null)
			{
				throw new ArgumentNullException(nameof(connection));
			}
			return connection.GetType().GetProperty("DbProviderFactory", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(connection) as DbProviderFactory;
		}

		public static DataTable GetFactoryClasses()
		{
			DataColumn nameColumn = new DataColumn(NameColumnName, typeof(string)) { ReadOnly = true };
			DataColumn descriptionColumn = new DataColumn(DescriptionColumnName, typeof(string)) { ReadOnly = true };
			DataColumn invariantNameColumn = new DataColumn(InvariantNameColumnName, typeof(string)) { ReadOnly = true };
			DataColumn assemblyQualifiedNameColumn = new DataColumn(AssemblyQualifiedNameColumnName, typeof(string)) { ReadOnly = true };

			DataTable toReturn = new DataTable(ProviderGroupColumnName) { Locale = CultureInfo.InvariantCulture };
			toReturn.Columns.AddRange(new[] { nameColumn, descriptionColumn, invariantNameColumn, assemblyQualifiedNameColumn });
			toReturn.PrimaryKey = new[] { invariantNameColumn };
			foreach (var kvp in _registeredFactories)
			{
				DataRow newRow = toReturn.NewRow();
				newRow[InvariantNameColumnName] = kvp.Key;
				newRow[AssemblyQualifiedNameColumnName] = kvp.Value.Factorytype.AssemblyQualifiedName;
				newRow[NameColumnName] = string.Empty;
				newRow[DescriptionColumnName] = string.Empty;
				toReturn.Rows.Add(newRow);
			}
			return toReturn;
		}

		public static IEnumerable<string> GetProviderInvariantNames()
		{
			return _registeredFactories.Keys.ToList();
		}

		public static void RegisterFactory(string providerInvariantName, Type factoryType)
		{
			if (string.IsNullOrWhiteSpace(providerInvariantName))
			{
				throw new ArgumentException(nameof(providerInvariantName));
			}
			if (factoryType == null)
			{
				throw new ArgumentException(nameof(factoryType));
			}

			// this method performs a deferred registration: the type name specified is checked when the factory is requested for the first time.
			RegisterFactory(providerInvariantName, GetFactoryInstance(factoryType));
		}

		public static void RegisterFactory(string providerInvariantName, DbProviderFactory factory)
		{
			if (string.IsNullOrWhiteSpace(providerInvariantName))
			{
				throw new ArgumentException(nameof(providerInvariantName));
			}
			if (factory == null)
			{
				throw new ArgumentNullException(nameof(factory));
			}

			_registeredFactories[providerInvariantName] = new ProviderRegistration(factory.GetType(), factory);
		}

		public static bool UnregisterFactory(string providerInvariantName)
		{
			ProviderRegistration providerRegistration;
			return !string.IsNullOrWhiteSpace(providerInvariantName) && _registeredFactories.TryRemove(providerInvariantName, out providerRegistration);
		}

		private static DbProviderFactory GetFactory(string providerInvariantName, bool throwOnError)
		{
			if (throwOnError)
			{
				if (string.IsNullOrWhiteSpace(providerInvariantName))
				{
					throw new ArgumentException(nameof(providerInvariantName));
				}
			}
			else
			{
				if (string.IsNullOrWhiteSpace(providerInvariantName))
				{
					return null;
				}
			}
			ProviderRegistration registration;
			bool wasRegistered = _registeredFactories.TryGetValue(providerInvariantName, out registration);
			if (!wasRegistered)
			{
				if (throwOnError)
				{
					throw new ArgumentException(string.Format("DbProviderFactories_InvariantNameNotFound {0}", providerInvariantName));
				}
				else
				{
					return null;
				}
			}
			DbProviderFactory toReturn = registration.FactoryInstance;
			if (toReturn == null)
			{
				// Deferred registration, do checks now on the type specified and register instance in storage.
				// Even in the case of throwOnError being false, this will throw when an exception occurs checking the registered type as the user has to be notified the 
				// registration is invalid, even though the registration is there.
				toReturn = GetFactoryInstance(GetProviderTypeFromTypeName(registration.Factorytype.AssemblyQualifiedName));
				RegisterFactory(providerInvariantName, toReturn);
			}
			return toReturn;
		}

		private static DbProviderFactory GetFactoryInstance(Type providerFactoryClass)
		{

			if (providerFactoryClass == null)
			{
				throw new ArgumentNullException(nameof(providerFactoryClass));
			}
			if (!providerFactoryClass.IsSubclassOf(typeof(DbProviderFactory)))
			{
				throw new ArgumentException(string.Format("DbProviderFactories_NotAFactoryType {0}", providerFactoryClass.FullName));
			}

			FieldInfo providerInstance = providerFactoryClass.GetField(InstanceFieldName, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static);
			if (null == providerInstance)
			{
				throw new InvalidOperationException("DbProviderFactories_NoInstance");
			}
			if (!providerInstance.FieldType.IsSubclassOf(typeof(DbProviderFactory)))
			{
				throw new InvalidOperationException("DbProviderFactories_NoInstance");
			}
			object factory = providerInstance.GetValue(null);
			if (null == factory)
			{
				throw new InvalidOperationException("DbProviderFactories_NoInstance");
			}
			return (DbProviderFactory)factory;
		}


		private static Type GetProviderTypeFromTypeName(string assemblyQualifiedName)
		{
			Type providerType = Type.GetType(assemblyQualifiedName);
			if (null == providerType)
			{
				throw new ArgumentException(string.Format("DbProviderFactories_FactoryNotLoadable {0}", assemblyQualifiedName));
			}
			return providerType;
		}
	}
}
