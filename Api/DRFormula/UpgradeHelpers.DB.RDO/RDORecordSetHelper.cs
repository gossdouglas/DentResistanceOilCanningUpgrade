using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Data.Common;
using System.Text.RegularExpressions;
using UpgradeHelpers.DB.RDO.Events;
using System.Data;
using System.Collections.Generic;
#if NET_CORE_APP || NET_STANDARD_APP
using SqlClient = Microsoft.Data.SqlClient;
#else
using SqlClient = System.Data.SqlClient;
#endif

namespace UpgradeHelpers.DB.RDO
{
    /// <summary>
    /// This class has the same functionality than the recordset exposed by the RDO library.
    /// </summary>
    [Serializable]
    public class RDORecordSetHelper : RecordSetHelper, ISerializable
    {
        #region Events
        /// <summary>
        /// Row Status Changed event
        /// </summary>
        public event RowStatusChangedEventHandler RowStatusChanged;
        /// <summary>
        /// Will Update Rows event
        /// </summary>
        public event WillUpdateRowsEventHandler WillUpdateRows;
        /// <summary>
        /// Row Currency Change event
        /// </summary>
        public event RowCurrencyChangeEventHandler RowCurrencyChange;
        /// <summary>
        /// Will Dissociate event
        /// </summary>
        public event WillDissociateEventHandler WillDissociate;
        /// <summary>
        /// Will Associate event
        /// </summary>
        public event WillAssociateEventHandler WillAssociate;
        /// <summary>
        /// Dissociate event
        /// </summary>
        public event DissociateEventHandler Dissociate;
        /// <summary>
        /// Associate event
        /// </summary>
        public event AssociateEventHandler Associate;

        #endregion

        #region class variable

        private LockTypeConstants _locktype;
        private bool _editingMode;

        /// <summary>
        /// has auto increment columns
        /// </summary>
        protected bool HasAutoincrementCols = false;

        /// <summary>
        /// New Datarow view when adding to a sorted or filtered collection
        /// </summary>
        protected DataRowView DbvRow = null;

        /// <summary>
        /// is first End Of File?
        /// </summary>
        private bool _firstEof = true;

        /// <summary>
        /// auto increment column name
        /// </summary>
        protected string autoIncrementCol = String.Empty;

        #endregion

        #region constructors
        /// <summary>
        /// Constructs a new RDORecordSetHelper instance using the specified factory.
        /// </summary>
        /// <param name="factoryname">The name used to identify the factory to be used to create all the necesary ADO .Net objects.</param>
        public RDORecordSetHelper(string factoryname)
            : this(AdoFactoryManager.GetFactory(factoryname))
        {
            IsDefaultSerializationInProgress = false;
            _index = -1;
            _newRow = false;
            DatabaseType = AdoFactoryManager.GetFactoryDbType(FactoryName);
        }

        /// <summary>
        /// creates a new recordset helper.
        /// </summary>
        public RDORecordSetHelper()
            : this("")
        {
        }
        /// <summary>
        /// Creates a new RecordsetHelper using the provided DBProviderFactory.
        /// </summary>
        /// <param name="factory">DBProviderFactory instance to be used by internal variable.</param>
        public RDORecordSetHelper(DbProviderFactory factory)
        {
            IsDefaultSerializationInProgress = false;
            _providerFactory = factory;
        }



        /// <summary>
        /// 
        /// </summary>
        protected RDORecordSetHelper(SerializationInfo info, StreamingContext context)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [SecurityPermission(SecurityAction.Demand,
            SerializationFormatter = true)]
        public override void GetObjectData(
            SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Return true if the recordsethelper is caching the adapters
        /// </summary>
        public bool IsCachingAdapter
        {
            get { return CachingAdapter; }
        }

        /// <summary>
        /// Gets and Set the connection to be used to interact with the database.
        /// </summary>
        public override DbConnection ActiveConnection
        {
            get
            {
                return _activeConnection;
            }
            set
            {
                bool cancel = false;
                if (value == null)
                    OnWillDissociate(ref cancel);
                else
                    OnWillAssociate(value, ref cancel);
                if (!cancel)
                {
                    //  base.ActiveConnection = value;
                    if (value != null)
                        Validate();
                    if (_activeCommand != null)
                    {
                        _activeCommand.Connection = value;
                    }
                    _activeConnection = value;
                    _connectionString = ActiveConnection != null ? ActiveConnection.ConnectionString : String.Empty;
                    _connectionStateAtEntry = ActiveConnection != null ? ActiveConnection.State : ConnectionState.Closed;


                    if (value == null)
                        OnDissociate();
                    else
                        OnAssociate();
                }
            }
        }
        /// <summary>
        /// Gets and Set the position of the current record on the recordset instance.
        /// </summary>
        public override int AbsolutePosition
        {
            get { return _index == -1 ? _index : _index + 1; }
            set
            {
                OnRowCurrencyChange();
                BasicMove(value - 1);
            }
        }

        /// <summary>
        /// Property used to determine if the data needs to be get from a dataview or the table directly
        /// </summary>
        protected override bool UsingView
        {
            get
            {
                return Filtered;
            }
        }
        /// <summary>
        /// Gets and Set the percentage of the current position of the total of records retrieved.
        /// </summary>
        public float PercentPosition
        {
            get
            {
                float result = -1;
                if (_index != -1)
                {
                    result = ((_index + 1f) * 100f) / RecordCount;
                }
                return result;
            }
            set
            {
                if (_index != -1)
                {
                    OnRowCurrencyChange();
                    //base.PercentPosition = value;
                    if (_index != -1)
                    {
                        BasicMove(Convert.ToInt32(value * RecordCount / 100) - 1);
                    }
                }
            }
        }

        /// <summary>
        /// Gets and Set the lock type to be used by the recordset.
        /// </summary>
        public LockTypeConstants LockType
        {
            get { return _locktype; }
            set { _locktype = value; }
        }
        /// <summary>
        /// Bookmark a Data Row
        /// </summary>
        public DataRow Bookmark
        {
            get
            {
                return UsingView ? CurrentView[_index].Row : Tables[CurrentRecordSet].Rows[_index];
            }
            set
            {
                CancelUpdate();
                // base.Bookmark = value;
                _index = findBookmarkIndex(value);
            }
        }

        private int _currentRecordSet = 0;
        /// <summary>
        /// Pointer to current recordset data table
        /// </summary>
        public override int CurrentRecordSet
        {
            get
            {
                return _currentRecordSet;
            }
            set
            {
                _currentRecordSet = value;
            }
        }

        /// <summary>
        /// Gets a DataRow with the information of the current record on the RecordsetHelper.
        /// </summary>
        public override DataRow CurrentRow
        {
            get
            {
                DataRow theRow = null;
                if (UsingView)
                {
                    DbvRow = CurrentView[_index];
                    theRow = DbvRow.Row;
                }
                else
                {
                    if (_index < Tables[CurrentRecordSet].Rows.Count)
                    {
                        theRow = Tables[CurrentRecordSet].Rows[_index];
                    }
                }
                return theRow;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FactoryName
        {
            get
            {
                if (ProviderFactory == null) return String.Empty;
                return AdoFactoryManager.GetFactoryNameFromProviderType(ProviderFactory.GetType());
            }
        }

        #endregion

        /// <summary>
        /// Updates the data in a Recordset object by re-executing the query on which the object is based.
        /// </summary>
        public override void Requery()
        {
            Open(true);
        }
        /// <summary>
        /// Looks in all records for a field that matches the “criteria”. 
        /// </summary>
        /// <param name="criteria">A String used to locate the record. It is like the WHERE clause in an SQL statement, but without the word WHERE.</param>
        public void Find(String criteria)
        {
            DataView result = Tables[CurrentRecordSet].DefaultView;
            result.RowFilter = criteria;
            if (result.Count > 0)
            {
                object[] values = result[0].Row.ItemArray;
                bool bfound = false;
                MoveFirst();
                while ((!bfound) && !EOF)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        bfound = (CurrentRow.ItemArray[i].Equals(values[i]));
                        if (!bfound)
                        {
                            break;
                        }
                    }
                    if (!bfound)
                    {
                        MoveNext();
                    }
                }
            }
        }

        /// <summary>
        /// Finds the index in the RecordsetHelper for the “value”.
        /// </summary>
        /// <param name="value">The DataRow to look for.</param>
        /// <returns>The index number if is found, otherwise -1.</returns>
        protected int findBookmarkIndex(DataRow value)
        {
            if (!UsingView)
            {
                return Tables[CurrentRecordSet].Rows.IndexOf(value);
            }
            int result = -1;
            for (int i = 0; i < CurrentView.Count; i++)
            {
                if (CurrentView[i].Row == value)
                {
                    result = i;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool CanMovePrevious
        {
            get { return !BOF; }
        }
        /// <summary>
        /// Gets a bool value indicating if the current record is the last one in the RecordsetHelper object.
        /// </summary>
        public override bool BOF
        {
            get
            {
                return _index == 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool CanMoveNext
        {
            get { return !EOF; }
        }
        /// <summary>
        /// Gets Command Source query string
        /// </summary>
        /// <returns>string</returns>
        public String getSource()
        {
            DbCommand command = _source as DbCommand;
            if (command != null)
            {
                return command.CommandText;
            }
            return (String)_source;
        }

        /// <summary>
        /// Gets the first 256 characters of the sql statement used to open the recordset.
        /// </summary>
        public string Name
        {
            get
            {
                String source = getSource();
                return (source.Length > 256) ? source.Substring(0, 256) : source;
            }
        }

        /// <summary>
        /// Returns the ActiveConnection object if it has been initialized otherwise creates a new DBConnection object.
        /// </summary>
        /// <param name="connectionString">The connection string to be used by the connection.</param>
        /// <returns>A DBConnection containing with the connection string set. </returns>
        protected virtual DbConnection GetConnection(String connectionString)
        {
            if (ActiveConnection != null && ActiveConnection.ConnectionString.Equals(connectionString, StringComparison.InvariantCultureIgnoreCase))
            {
                return ActiveConnection;
            }
            //DbConnection connection = providerFactory.CreateConnectionWithTrace();
            DbConnection connection = _providerFactory.CreateConnection();
            if (connection != null)
            {
                connection.ConnectionString = connectionString;
                return connection;
            }
            return null;
        }

        /// <summary>
        /// Sets the primary key to a DataTable object.
        /// </summary>
        /// <param name="dataTable">The DataTable that holds the currently loaded data.</param>
        private void FixAutoincrementColumns(DataTable dataTable)
        {
            if (ActiveConnection is SqlClient.SqlConnection)
            {
                foreach (DataColumn col in dataTable.PrimaryKey)
                {
                    if (col.AutoIncrement)
                    {
                        col.AutoIncrementSeed = 0;
                        col.AutoIncrementStep = -1;
                        col.ReadOnly = false;
                        HasAutoincrementCols = true;
                        // todo check multiple autoincrement cases
                        autoIncrementCol = col.ColumnName;
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Infers the command type from an SQL string getting the schema metadata from the database.
        /// </summary>
        /// <param name="sqlCommand">The sql string to be analyzed.</param>
        /// <param name="parameters">List of DbParameters</param>
        /// <returns>The command type</returns>
        protected override CommandType getCommandType(String sqlCommand, out List<DbParameter> parameters)
        {
            CommandType commandType = CommandType.Text;
            parameters = null;
            sqlCommand = sqlCommand.Trim();
            if (sqlCommand.StartsWith("select", StringComparison.InvariantCultureIgnoreCase) ||
                sqlCommand.StartsWith("insert", StringComparison.InvariantCultureIgnoreCase) ||
                sqlCommand.StartsWith("update", StringComparison.InvariantCultureIgnoreCase) ||
                sqlCommand.StartsWith("delete", StringComparison.InvariantCultureIgnoreCase) ||
                sqlCommand.StartsWith("exec", StringComparison.InvariantCultureIgnoreCase) ||
                sqlCommand.StartsWith("begin", StringComparison.InvariantCultureIgnoreCase) ||
                sqlCommand.StartsWith("set", StringComparison.InvariantCultureIgnoreCase))
            {
                commandType = CommandType.Text;
                return commandType;
            }
            string[] completeCommandParts = sqlCommand.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            sqlCommand = completeCommandParts[0];
            String[] commandParts = sqlCommand.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            String objectQuery = String.Empty;
            DbConnection connection = GetConnection(_connectionString);
            if (connection.State != ConnectionState.Open)
            {
                //connection.OpenWithTrace();
                connection.Open();
            }
            DataRow[] existingObjects;
            DataTable dbObjects = connection.GetSchema("Tables");
            if (dbObjects.Rows.Count > 0)
            {
                //this is an sql server connection
                if (dbObjects.Columns.Contains("table_catalog") && dbObjects.Columns.Contains("table_schema"))
                {
                    if (commandParts.Length == 3)
                    {
                        objectQuery = "table_catalog = \'" + commandParts[0] + "\' AND table_schema = \'" + commandParts[1] + "\' AND table_name = \'" + commandParts[2] + "\'";
                    }
                    else if (commandParts.Length == 2)
                    {
                        objectQuery = "table_schema = \'" + commandParts[0] + "\' AND table_name = \'" + commandParts[1] + "\'";
                    }
                    else
                    {
                        objectQuery = "table_name = \'" + commandParts[0] + "\'";
                    }
                }
                else if (dbObjects.Columns.Contains("OWNER"))
                {
                    if (commandParts.Length == 2)
                    {
                        objectQuery = "OWNER = \'" + commandParts[0] + "\' AND TABLE_NAME = \'" + commandParts[1] + "\'";
                    }
                    else
                    {
                        objectQuery = "TABLE_NAME = \'" + commandParts[0] + "\'";
                    }
                }
                existingObjects = dbObjects.Select(objectQuery);
                if (existingObjects.Length > 0)
                {
                    commandType = CommandType.TableDirect;
                    return commandType;
                }
            }
            dbObjects = connection.GetSchema("Procedures");
            // The query for looking for stored procedures information is version sensitive.
            // The database version can be verified in SQLServer using a query like "Select @@version"
            // That version can be mapped to the specific SQL Server Product Version with a table like the provided here: http://www.sqlsecurity.com/FAQs/SQLServerVersionDatabase/tabid/63/Default.aspx 
            // The following code verifies columns for SQL Server version 2003, other versions might have a different schema and require changes
            if (dbObjects.Columns.Contains("procedure_catalog") && dbObjects.Columns.Contains("procedure_schema"))
            {
                if (commandParts.Length == 3)
                {
                    objectQuery = "procedure_catalog = \'" + commandParts[0] + "\' AND procedure_schema = \'" + commandParts[1] + " AND procedure_name = " + commandParts[2] +
                                  "\'";
                }
                else if (commandParts.Length == 2)
                {
                    objectQuery = "procedure_schema = \'" + commandParts[0] + "\' AND procedure_name = \'" + commandParts[1] + "\'";
                }
                else
                {
                    objectQuery = "procedure_name = \'" + commandParts[0] + "\'";
                }
            }
            else if (dbObjects.Rows.Count > 0)
            {
                //this is an sql server connection
                if (dbObjects.Columns.Contains("specific_catalog") && dbObjects.Columns.Contains("specific_schema"))
                {
                    if (commandParts.Length == 3)
                    {
                        objectQuery = "specific_catalog = \'" + commandParts[0] + "\' AND specific_schema = \'" + commandParts[1] + " AND specific_name = " + commandParts[2] +
                                      "\'";
                    }
                    else if (commandParts.Length == 2)
                    {
                        objectQuery = "specific_schema = \'" + commandParts[0] + "\' AND specific_name = \'" + commandParts[1] + "\'";
                    }
                    else
                    {
                        objectQuery = "specific_name = \'" + commandParts[0] + "\'";
                    }
                }
                else if (dbObjects.Columns.Contains("OWNER"))
                {
                    if (commandParts.Length == 2)
                    {
                        objectQuery = "OWNER = \'" + commandParts[0] + "\' AND OBJECT_NAME = \'" + commandParts[1] + "\'";
                    }
                    else
                    {
                        objectQuery = "OBJECT_NAME = \'" + commandParts[0] + "\'";
                    }
                }
                existingObjects = dbObjects.Select(objectQuery);
                if (existingObjects.Length > 0)
                {
                    commandType = CommandType.StoredProcedure;
                    if (dbObjects.Columns.Contains("specific_catalog") && dbObjects.Columns.Contains("specific_schema"))
                    {
                        DataTable procedureParameters = connection.GetSchema("ProcedureParameters");
                        DataRow[] theparameters =
                            procedureParameters.Select(
                                "specific_catalog = \'" + existingObjects[0]["specific_catalog"] + "\' AND specific_schema = \'" + existingObjects[0]["specific_schema"] +
                                "' AND specific_name = '" + existingObjects[0]["specific_name"] + "\'",
                                "ordinal_position ASC");
                        if (theparameters.Length > 0)
                        {
                            parameters = new List<DbParameter>(theparameters.Length);
                            foreach (DataRow paraminfo in theparameters)
                            {
                                DbParameter theParameter = _providerFactory.CreateParameter();
#if CLR_AT_LEAST_3_5 
                                    theParameter.ParameterName = paraminfo.Field<string>("parameter_name");
                                    theParameter.DbType = MapToDbType(paraminfo.Field<string>("data_type"));
#else
                                theParameter.ParameterName = paraminfo["parameter_name"] as string;
                                theParameter.DbType = MapToDbType(paraminfo["data_type"] as string);
#endif
                                parameters.Add(theParameter);
                            }
                        }
                    }
                }
            }
            return commandType;
        }
        /// <summary>
        /// Verifies if the ADORecordset object have been open.
        /// </summary>
        protected virtual void Validate()
        {
        }


        #region Open methods

        /// <summary>
        /// Opens the recordset.
        /// </summary>
        public override void Open()
        {
            //base.Open();
            try
            {
                Open(false);

                if (Tables.Count > 0)
                {
                    Tables[CurrentRecordSet].RowChanging += RDORecordSetHelper_RowChanging;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Creates a DBCommand object using de provided parameters.
        /// </summary>
        /// <param name="commandText">A string containing the SQL query.</param>
        /// <param name="commandType">The desire type for the command.</param>
        /// <param name="parameters">A list with the parameters to be included on the DBCommand object.</param>
        /// <returns>A new DBCommand object.</returns>
        protected override DbCommand CreateCommand(String commandText, CommandType commandType, List<DbParameter> parameters)
        {
            Debug.Assert(_providerFactory != null, "Providerfactory must not be null");
            DbCommand command = _providerFactory.CreateCommand();
            switch (commandType)
            {
                case CommandType.StoredProcedure:
                    string[] commandParts = commandText.Split(' ', ',');
                    if (command != null)
                    {
                        command.CommandType = commandType;
                        command.CommandText = commandParts[0];
                        if (parameters != null && (parameters.Count == commandParts.Length - 1))
                        {
                            for (int i = 1; i < commandParts.Length; i++)
                            {
                                DbParameter parameter = parameters[i - 1];
                                object value = commandParts[i];
                                //conversions might be needed for various types
                                //currently there is only a convertion for Guid types. New convertions will be added as needed
                                if (parameter.DbType == DbType.Guid)
                                {
                                    //Remove single quotes if present to avoid exception in Guid constructor
                                    string strValue = commandParts[i].Replace("'", "");
                                    value = new Guid(strValue);
                                }
                                parameter.Value = value;
                            }
                            command.Parameters.AddRange(parameters.ToArray());
                        }
                    }
                    break;
                case CommandType.TableDirect:
                    //ODBC and SQL Client providers do not support table direct commands
                    string providerType = _providerFactory.GetType().ToString();
                    if (providerType.StartsWith("System.Data.Odbc") ||
#if NET_CORE_APP || NET_STANDARD_APP
						providerType.StartsWith("Microsoft.Data.SqlClient"))
#else
						providerType.StartsWith("System.Data.SqlClient"))
#endif
                    {
                        if (command != null)
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText = "Select * from " + commandText;
                        }
                    }
                    else
                    {
                        goto default;
                    }
                    break;
                default:
                    if (command != null)
                    {
                        command.CommandType = commandType;
                        command.CommandText = commandText;
                    }
                    break;
            }
            return command;
        }

        /// <summary>
        /// Creates the update command for the database update operations of the recordset
        /// </summary>
        /// <param name="adapter">The data adapter that will contain the update command</param>
        /// <param name="cmdBuilder">The command builder to get the update command from.</param>
        protected virtual void CreateUpdateCommand(DbDataAdapter adapter, DbCommandBuilder cmdBuilder)
        {
            try
            {
                adapter.UpdateCommand = (string.IsNullOrEmpty(_sqlUpdateQuery)) ? cmdBuilder.GetUpdateCommand(true) : CreateCommand(_sqlUpdateQuery, CommandType.Text, null);
            }
            catch (Exception)
            {
                adapter.UpdateCommand = CreateUpdateCommandFromMetaData();
            }
        }
        /// <summary>
        /// Creates a delete command using the information contained in the RecordsetHelper.    
        /// </summary>
        /// <returns>A DBCommand object containing a delete command.</returns>
        protected DbCommand CreateDeleteCommandFromMetaData()
        {
            DbCommand result = null;
            String tableName = getTableName(_activeCommand.CommandText);
            int j = 0;
            try
            {
                if (!string.IsNullOrEmpty(tableName))
                {
                    string wherePart = "";
                    List<DbParameter> listGeneral = new List<DbParameter>();

                    foreach (DataColumn dColumn in Tables[CurrentRecordSet].Columns)
                    {
                        if (wherePart.Length > 0)
                        {
                            wherePart += " AND ";
                        }

                        DbParameter pInfo;
                        if (dColumn.AllowDBNull)
                        {
                            wherePart += "((? = 1 AND " + dColumn.ColumnName + " IS NULL) OR (" + dColumn.ColumnName + " = ?))";

                            pInfo = CreateParameterFromColumn("p" + (++j), dColumn);
                            pInfo.DbType = DbType.Int32;
                            pInfo.SourceVersion = DataRowVersion.Original;
                            pInfo.SourceColumnNullMapping = true;
                            pInfo.Value = 1;
                            listGeneral.Add(pInfo);

                            pInfo = CreateParameterFromColumn("p" + (++j), dColumn);
                            pInfo.SourceVersion = DataRowVersion.Original;
                            listGeneral.Add(pInfo);
                        }
                        else
                        {
                            wherePart += "(" + dColumn.ColumnName + " = ?)";
                            pInfo = CreateParameterFromColumn("q" + (++j), dColumn);
                            pInfo.SourceVersion = DataRowVersion.Original;
                            listGeneral.Add(pInfo);
                        }
                    }
                    string sql = "DELETE FROM " + tableName + " WHERE (" + wherePart + ")";
                    result = ProviderFactory.CreateCommand();
                    if (result != null)
                    {
                        result.CommandText = sql;
                        listGeneral.ForEach(delegate (DbParameter p) { result.Parameters.Add(p); });
                        result.Connection = _activeCommand.Connection;
                    }
                }
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// Creates an update command using the information contained in the RecordsetHelper.
        /// </summary>
        /// <returns>A DBCommand object containing an update command.</returns>
        protected DbCommand CreateUpdateCommandFromMetaData()
        {
            int i = 0, j = 0;
            DbCommand result = null;
            String tableName = getTableName(_activeCommand.CommandText);
            try
            {
                if (!string.IsNullOrEmpty(tableName))
                {
                    string updatePart = "";
                    string wherePart = "";
                    string sql = "";
                    List<DbParameter> listGeneral = new List<DbParameter>();
                    List<DbParameter> listWhere = new List<DbParameter>();

                    foreach (System.Data.DataColumn dColumn in Tables[CurrentRecordSet].Columns)
                    {
                        if (Tables[CurrentRecordSet].PrimaryKey != null && !(Array.Exists(
                            Tables[CurrentRecordSet].PrimaryKey,
                            delegate (DataColumn col) { return col.ColumnName.Equals(dColumn.ColumnName, StringComparison.InvariantCultureIgnoreCase); })
                              || dColumn.ReadOnly))
                        {
                            if (updatePart.Length > 0)
                            {
                                updatePart += " , ";
                            }

                            updatePart += dColumn.ColumnName + " = ?";
                            listGeneral.Add(CreateParameterFromColumn("p" + (++i), dColumn));
                        }
                        if (wherePart.Length > 0)
                        {
                            wherePart += " AND ";
                        }
                        DbParameter param;
                        if (dColumn.AllowDBNull)
                        {
                            wherePart += "((? = 1 AND " + dColumn.ColumnName + " IS NULL) OR (" + dColumn.ColumnName + " = ?))";
                            param = CreateParameterFromColumn("q" + (++j), dColumn);
                            param.DbType = DbType.Int32;
                            param.SourceVersion = DataRowVersion.Original;
                            param.SourceColumnNullMapping = true;
                            param.Value = 1;
                            listWhere.Add(param);
                            param = CreateParameterFromColumn("q" + (++j), dColumn);
                            param.SourceVersion = DataRowVersion.Original;
                            listWhere.Add(param);
                        }
                        else
                        {
                            wherePart += "(" + dColumn.ColumnName + " = ?)";
                            param = CreateParameterFromColumn("q" + (++j), dColumn);
                            param.SourceVersion = DataRowVersion.Original;
                            listWhere.Add(param);
                        }
                    }
                    listGeneral.AddRange(listWhere);
                    sql = "UPDATE " + tableName + " SET " + updatePart + " WHERE " + wherePart;
                    result = ProviderFactory.CreateCommand();
                    if (result != null)
                    {
                        result.CommandText = sql;
                        listGeneral.ForEach(delegate (DbParameter p) { result.Parameters.Add(p); });
                        result.Connection = _activeCommand.Connection;
                    }
                }
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// Creates an insert command using the information contained in the RecordsetHelper.
        /// </summary>
        /// <returns>A DBCommand object containing an insert command.</returns>
        protected DbCommand CreateInsertCommandFromMetaData()
        {
            DbCommand result = null;
            int i = 0;
            String tableName = getTableName(_activeCommand.CommandText);
            try
            {

                if (!string.IsNullOrEmpty(tableName))
                {
                    List<DbParameter> parameters = new List<DbParameter>();
                    string fieldsPart = "";
                    string valuesPart = "";
                    string sql;
                    foreach (DataColumn dColumn in Tables[CurrentRecordSet].Columns)
                    {
                        if (!dColumn.ReadOnly)
                        {
                            if (fieldsPart.Length > 0)
                            {
                                fieldsPart += ", ";
                                valuesPart += ", ";
                            }

                            fieldsPart += dColumn.ColumnName;
                            valuesPart += "?";
                            parameters.Add(CreateParameterFromColumn("p" + (++i), dColumn));
                        }
                    }
                    sql = "INSERT INTO " + tableName + " (" + fieldsPart + ") VALUES (" + valuesPart + ")";
                    result = ProviderFactory.CreateCommand();
                    if (result != null)
                    {
                        result.CommandText = sql;
                        parameters.ForEach(delegate (DbParameter p) { result.Parameters.Add(p); });
                        result.Connection = _activeCommand.Connection;
                    }
                }
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// Assigns the InsertCommand to the adaptar parameter
        /// </summary>
        /// <param name="adapter">DbDataAdapter</param>
        protected void CompleteInsertCommand(DbDataAdapter adapter)
        {
            String extraCommandText = "";
            String extraCommandText1 = "";
            Dictionary<String, String> identities = IdentityColumnsManager.GetIndentityInformation(getTableName(_activeCommand.CommandText));
            if (identities != null)
            {
                foreach (KeyValuePair<String, String> identityInfo in identities)
                {
                    adapter.InsertCommand.UpdatedRowSource = UpdateRowSource.Both;
                    //outPar.ParameterName = (isOracle ? ":" : "@") + identityInfo.Key;

                    if (DatabaseType == DatabaseType.Oracle)
                    {
                        DbParameter outPar = adapter.InsertCommand.Parameters[":" + identityInfo.Key];
                        //todo: check for null
                        outPar.Direction = ParameterDirection.Output;
                        outPar.DbType = GetDBType(Tables[CurrentRecordSet].Columns[identityInfo.Key].DataType);

                        if (String.IsNullOrEmpty(extraCommandText))
                        {
                            extraCommandText = " RETURNING " + identityInfo.Key;
                            extraCommandText1 = " INTO :" + identityInfo.Key;
                        }
                        else
                        {
                            extraCommandText += ", " + identityInfo.Key;
                            extraCommandText1 += ", :" + identityInfo.Key;
                        }
                    }
                    else if (DatabaseType != DatabaseType.Undefined)
                    {
                        extraCommandText = MsInsertCommandCompletion(adapter, identityInfo.Key, extraCommandText);
                    }
                }
            }
            else
            {
                extraCommandText = MsInsertCommandCompletion(adapter, autoIncrementCol, extraCommandText);
            }
            adapter.InsertCommand.CommandText += extraCommandText + extraCommandText1;
        }

        /// <summary>
        /// SqlServer Identity value for last insert execution.
        /// </summary>
        /// <param name="adapter">DbDataAdapter to set</param>
        /// <param name="identityInfo">Name of Identity field</param>
        /// <param name="extraCommandText">used to set the query to get the identity value</param>
        /// <returns>returns the entire query in the adapter</returns>
        protected string MsInsertCommandCompletion(DbDataAdapter adapter, String identityInfo, string extraCommandText)
        {
            if (!String.IsNullOrEmpty(identityInfo))
            {
                DbParameter outPar = _providerFactory.CreateParameter();
                if (outPar != null)
                {
                    outPar.ParameterName = "@" + identityInfo;
                    outPar.DbType = GetDBType(Tables[CurrentRecordSet].Columns[identityInfo].DataType);
                    outPar.Direction = ParameterDirection.Output;
                    outPar.SourceColumn = identityInfo;
                }
                extraCommandText += " SELECT @" + identityInfo + " = SCOPE_IDENTITY()";
                adapter.InsertCommand.Parameters.Add(outPar);
            }
            return extraCommandText;
        }


        /// <summary>
        /// OleDb Row Updated event
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">Row updated event args</param>
        protected void RecordSetHelper_RowUpdatedOleDb(object sender, System.Data.OleDb.OleDbRowUpdatedEventArgs e)
        {
            //This behavior depends on the database we are interacting with
            if (e.StatementType == StatementType.Insert && e.Status == UpdateStatus.Continue)
            {
                Dictionary<String, String> identities = IdentityColumnsManager.GetIndentityInformation(getTableName(_activeCommand.CommandText));
                if (identities != null)
                {
                    DbCommand oCmd = e.Command.Connection.CreateCommand();
                    oCmd.Transaction = e.Command.Transaction;
                    foreach (KeyValuePair<String, String> identityInfo in identities)
                    {
                        switch (DatabaseType)
                        {
                            case DatabaseType.Oracle:
                                oCmd.CommandText = "Select " + identityInfo.Value + ".Currval from dual";
                                break;
                            case DatabaseType.SQLServer:
                                oCmd.CommandText = "SELECT SCOPE_IDENTITY()";
                                break;
                            case DatabaseType.Access:
                                oCmd.CommandText = "SELECT @@IDENTITY";
                                break;
                        }
                        e.Row[identityInfo.Key] = oCmd.ExecuteScalar();
                    }
                    e.Row.AcceptChanges();
                }
            }
        }
        /// <summary>
        /// Using connection parameter creates a Database Data Adapter
        /// </summary>
        /// <param name="connection">DbConnection parameter</param>
        /// <param name="updating">if updating creates all internal query strings</param>
        /// <returns></returns>
        protected virtual DbDataAdapter CreateAdapter(DbConnection connection, bool updating)
        {
            Debug.Assert(connection != null, "Error during CreateAdapter call. Connection String must never be null");
            DbDataAdapter realAdapter = ProviderFactory.CreateDataAdapter();
            DbDataAdapter adapter = ProviderFactory.CreateDataAdapter();
            bool isOracleProvider = ProviderFactory.GetType().FullName.Equals("Oracle.DataAccess.Client.OracleClientFactory");
            DbCommandBuilder cmdBuilder = null;
            KeyValuePair<DbConnection, string> key = new KeyValuePair<DbConnection, string>();
            try
            {
                cmdBuilder = ProviderFactory.CreateCommandBuilder();

                if (_activeCommand.Connection == null || _activeCommand.Connection.ConnectionString.Equals(""))
                {
                    //What should we use here. ActiveConnection or the connection we are sending as parameter
                    //it seams more valid to use the parameter
                    _activeCommand.Connection = connection;
                }
                if (String.IsNullOrEmpty(_activeCommand.CommandText))
                {
                    _activeCommand.CommandText = _sqlSelectQuery;
                }
                DbTransaction transaction = TransactionManager.GetCurrentTransaction(connection);
                if (transaction != null)
                {
                    _activeCommand.Transaction = transaction;
                }

                if (CachingAdapter)
                {
                    key = new KeyValuePair<DbConnection, string>(_activeCommand.Connection, _activeCommand.CommandText);
                    if (_dataAdaptersCached.ContainsKey(key))
                    {
                        return _dataAdaptersCached[key];
                    }
                }

                if (adapter != null)
                {
                    adapter.SelectCommand = _activeCommand;
                    realAdapter.SelectCommand = adapter.SelectCommand;
                    cmdBuilder.DataAdapter = adapter;
                    if (updating)
                    {
                        if (DatabaseType == DatabaseType.Access || DatabaseType == DatabaseType.SQLServer || getTableName(_activeCommand.CommandText).Contains(" "))
                        {
                            cmdBuilder.QuotePrefix = "[";
                            cmdBuilder.QuoteSuffix = "]";
                        }
                        CreateUpdateCommand(adapter, cmdBuilder);
                        try
                        {
                            adapter.InsertCommand = (string.IsNullOrEmpty(_sqlInsertQuery)) ? cmdBuilder.GetInsertCommand(true) : CreateCommand(_sqlInsertQuery, CommandType.Text, null);
                        }
                        catch (Exception)
                        {
                            adapter.InsertCommand = CreateInsertCommandFromMetaData();
                        }
                        try
                        {
                            adapter.DeleteCommand = (string.IsNullOrEmpty(_sqlDeleteQuery)) ? cmdBuilder.GetDeleteCommand(true) : CreateCommand(_sqlDeleteQuery, CommandType.Text, null);
                        }
                        catch (Exception)
                        {
                            adapter.DeleteCommand = CreateDeleteCommandFromMetaData();
                        }

#if SUPPORT_OBSOLETE_ORACLECLIENT                            
                        if ((ProviderFactory is SqlClient.SqlClientFactory) || isOracleProvider)
#else
                        if ((ProviderFactory is SqlClient.SqlClientFactory))
#endif
                        {
#if SUPPORT_OBSOLETE_ORACLECLIENT                            
			                //EVG20080326: Oracle.DataAccess Version 10.102.2.20 Bug. It returns "::" instead ":" before each parameter name, wich is invalid.
			                if (isOracleProvider)
			                {
			                    adapter.InsertCommand.CommandText = adapter.InsertCommand.CommandText.Replace("::", ":");
			                    adapter.DeleteCommand.CommandText = adapter.DeleteCommand.CommandText.Replace("::", ":");
			                    adapter.UpdateCommand.CommandText = adapter.UpdateCommand.CommandText.Replace("::", ":");
			                }
#endif
                            CompleteInsertCommand(adapter);
                        }
                        else if (ProviderFactory is System.Data.OleDb.OleDbFactory)
                        {
                            ((System.Data.OleDb.OleDbDataAdapter)realAdapter).RowUpdated += RecordSetHelper_RowUpdatedOleDb;
                        }
                        realAdapter.InsertCommand = CloneCommand(adapter.InsertCommand);
                        realAdapter.DeleteCommand = CloneCommand(adapter.DeleteCommand);
                        realAdapter.UpdateCommand = CloneCommand(adapter.UpdateCommand);
                        if (realAdapter.InsertCommand != null)
                        {
                            realAdapter.InsertCommand.Transaction = realAdapter.SelectCommand.Transaction;
                        }
                        if (realAdapter.DeleteCommand != null)
                        {
                            realAdapter.DeleteCommand.Transaction = realAdapter.SelectCommand.Transaction;
                        }
                        if (realAdapter.UpdateCommand != null)
                        {
                            realAdapter.UpdateCommand.Transaction = realAdapter.SelectCommand.Transaction;
                        }
                    }
                }

                if (CachingAdapter)
                {
                    _dataAdaptersCached.Add(key, realAdapter);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (adapter != null) adapter.Dispose();
                if (cmdBuilder != null)
                {
                    cmdBuilder.Dispose();
                }
            }
            return realAdapter;
        }

        /// <summary>
        /// operation finished state
        /// </summary>
        private bool operationFinished;
        /// <summary>
        /// Opens the RecordsetHelper and requeries according to the value of “requery” parameter.
        /// </summary>
        /// <param name="requery">Indicates if a requery most be done.</param>
        protected void OpenRecordset(bool requery)
        {
            FirstEof = true;
            if (ActiveConnection != null && _activeCommand != null)
            {
                DbDataAdapter dbAdapter = null;
                try
                {
                    if (requery && Tables.Count > 0)
                    {
                        Tables.Clear();
                    }
                    dbAdapter = CreateAdapter(ActiveConnection, false);
                    _sqlSelectQuery = _activeCommand.CommandText;
                    OperationFinished = false;
                    if (LoadSchema)
                    {
                        using (DataTable tmpTable = new DataTable())
                        {
                            if ((Tables.Count > 0) && (Tables[CurrentRecordSet].Columns.Count > 0))
                            {
                                dbAdapter.FillSchema(tmpTable, SchemaType.Source);
                            }
                        }
                    }
                    if (!LoadSchemaOnly)
                        dbAdapter.Fill(this);
                    else
                        dbAdapter.FillSchema(this, SchemaType.Source);
                }
                finally
                {
                    if (!IsCachingAdapter)
                        if (dbAdapter != null) dbAdapter.Dispose();
                }
            }
            if (Tables.Count > 0)
            {
                FixAutoincrementColumns(Tables[CurrentRecordSet]);
                OperationFinished = true;
                CurrentView = Tables[CurrentRecordSet].DefaultView;
                CurrentView.AllowDelete = true;
                CurrentView.AllowEdit = true;
                CurrentView.AllowNew = true;
                if (Tables[CurrentRecordSet].Rows.Count == 0)
                {
                    _index = -1;
                    _eof = true;
                }
                else
                {
                    _index = 0;
                    _eof = false;
                }
            }
            else
            {
                _index = -1;
                _eof = true;
            }
            _newRow = false;
            _opened = true;
            OnAfterQuery();
        }
        /// <summary>
        /// Opens the RecordsetHelper and requeries according to the value of “requery” parameter.
        /// </summary>
        /// <param name="requery">Indicates if a requery most be done.</param>
        public void Open(bool requery)
        {
            if (!requery)
            {
                Validate();
            }
            if (_activeCommand == null && (_source is String))
            {
                List<DbParameter> parameters;
                CommandType commandType = getCommandType((string)_source, out parameters);
                _activeCommand = CreateCommand((string)_source, commandType, parameters);
            }
            if (ActiveConnection == null && _activeCommand != null && _activeCommand.Connection != null)
            {
                ActiveConnection = _activeCommand.Connection;
            }

            OpenRecordset(requery);
        }
        /// <summary>
        /// Creates a new RDORecordSetHelper and opens it.
        /// </summary>
        /// <param name="SQLstr">The sql statement used to populate the recordset.</param>
        /// <param name="connection">The connection used to interact with the database.</param>
        /// <param name="locktype">The lock type used by the recordset.</param>
        /// <param name="factoryName">The name used to identify the factory to be used to create all the necesary ADO .Net objects.</param>
        /// <returns>A new opened recordset.</returns>
        public static RDORecordSetHelper Open(String SQLstr, DbConnection connection, LockTypeConstants locktype, String factoryName)
        {
            if (factoryName == "")
                factoryName = AdoFactoryManager.Default.Name;
            RDORecordSetHelper result = new RDORecordSetHelper(factoryName);
            result.Source = SQLstr;
            result.LockType = locktype;
            result.ActiveConnection = connection;
            result.Open();
            return result;
        }

        /// <summary>
        /// Creates a new RDORecordSetHelper and opens it.
        /// </summary>
        /// <param name="SQLstr">The sql statement used to populate the recordset.</param>
        /// <param name="connection">The connection used to interact with the database.</param>
        /// <param name="factoryName">The name used to identify the factory to be used to create all the necesary ADO .Net objects.</param>
        /// <returns>A new opened recordset.</returns>
        public static RDORecordSetHelper Open(String SQLstr, DbConnection connection, String factoryName)
        {
            return RDORecordSetHelper.Open(SQLstr, connection, LockTypeConstants.rdConcurReadOnly, factoryName);
        }

        /// <summary>
        ///  Creates a new RDORecordSetHelper and opens it.
        /// </summary>
        /// <param name="command">The sql statement used to populate the recordset.</param>
        /// <param name="locktype">The lock type used by the recordset.</param>
        /// <param name="factoryName">The name used to identify the factory to be used to create all the necesary ADO .Net objects.</param>
        /// <returns>A new opened recordset.</returns>
        public static RDORecordSetHelper Open(DbCommand command, LockTypeConstants locktype, String factoryName)
        {
            RDORecordSetHelper result = new RDORecordSetHelper(factoryName);
            result.Source = command;
            result.LockType = locktype;
            result.Open();
            return result;
        }

        /// <summary>
        ///  Creates a new RDORecordSetHelper and opens it.
        /// </summary>
        /// <param name="command">The sql statement used to populate the recordset.</param>
        /// <param name="factoryName">The name used to identify the factory to be used to create all the necesary ADO .Net objects.</param>
        /// <returns>A new opened recordset.</returns>
        public static RDORecordSetHelper Open(DbCommand command, String factoryName)
        {
            return Open(command, LockTypeConstants.rdConcurReadOnly, factoryName);
        }

        #endregion

        #region Data Handling methods

        /// <summary>
        /// Executes the atomic addNew Operation creating the new row and setting the newRow flag.
        /// </summary>
        protected void doAddNew()
        {
            if (UsingView)
            {
                DbvRow = CurrentView.AddNew();
            }
            else
            {
                DataRow _dbRow = Tables[CurrentRecordSet].NewRow();
                Tables[CurrentRecordSet].Rows.Add(_dbRow);
                _index = Tables[CurrentRecordSet].Rows.Count - 1;
                _requiresDefaultValues = true;
            }
            _newRow = true;
        }
        /// <summary>
        /// Creates a new record on the recordset.
        /// </summary>
        public override void AddNew()
        {
            OnRowStatusChanged();
            OnRowCurrencyChange();
            //base.AddNew();
            doAddNew();
        }

        /// <summary>
        /// Sets the recordset on edition mode.
        /// </summary>
        public void Edit()
        {
            _editingMode = true;
        }

        /// <summary>
        /// Deletes the current record of the recordset.
        /// </summary>
        public void Delete()
        {
            // base.Delete();
            CurrentRow.Delete();
            if (!isBatchEnabled())
                Update();
        }
        /// <summary>
        /// Gets or sets the connection string being use by this RecordsetHelper object.
        /// </summary>
        public override String ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                _connectionString = value;
                if (_providerFactory != null)
                {
                    try
                    {
                        Validate();
                        DbConnection connection = _providerFactory.CreateConnection();
                        //DbConnection connection =  providerFactory.CreateConnectionWithTrace();
                        connection.ConnectionString = value;
                        ActiveConnection = connection;
                        //ActiveConnection.OpenWithTrace();
                        ActiveConnection.Open();
                    }
                    catch (Exception ex)
                    {
                        String message = string.Format(
                                    "Problem while trying to set the active connection. Please verify ConnectionString {0} and Factory {1} settings. Error details {2}",
                                    _connectionString,
                                    _providerFactory,
                                    ex.Message);
#if WINFORMS
                        if (Process.GetCurrentProcess().ProcessName == "devenv")
						{
							System.Windows.Forms.MessageBox.Show(message,TITLE_DIALOG_RecordSetError);
						} // if
#else
                        Trace.TraceError(message);
#endif
                        if (!Disconnected)
                            throw;
                    } // catch
                }
                _defaultValues = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override RecordSetHelper CreateInstance()
        {
            return new RDORecordSetHelper();
        }

        /// <summary>
        /// operation finished state
        /// </summary>
        public bool OperationFinished
        {
            get { return operationFinished; }
            set { operationFinished = value; }
        }

        /// <summary>
        /// has auto increment columns
        /// </summary>
        public bool IsDefaultSerializationInProgress
        {
            get { return _isDefaultSerializationInProgress; }
            set { _isDefaultSerializationInProgress = value; }
        }

        /// <summary>
        /// is first End Of File?
        /// </summary>
        public bool FirstEof
        {
            get { return _firstEof; }
            set { _firstEof = value; }
        }

        /// <summary>
        /// is filtered?
        /// </summary>
        public bool Filtered
        {
            get { return _filtered; }
            set { _filtered = value; }
        }
        /// <summary>
        /// Get and set of caching adapter
        /// </summary>
        public bool CachingAdapter
        {
            get { return _cachingAdapter; }
            set { _cachingAdapter = value; }
        }

        /// <summary>
        /// Sets default values to a fields to avoid insert null in the DB when the field does not accept it.
        /// </summary>
        private void AssignDefaultValues(DataRow dbRow)
        {
            DbDataAdapter adapter = null;
            try
            {
                _requiresDefaultValues = false;
                if (_defaultValues == null) //no default values loaded for this table
                {
                    DataTable schemaTable;
                    try
                    {
                        adapter = CreateAdapter(GetConnection(ConnectionString), true);
                        schemaTable = ActiveConnection.GetSchema("Columns", (new string[] { ActiveConnection.Database, "dbo", getTableName(adapter.SelectCommand.CommandText, true).Replace("dbo.", string.Empty) }));
                    }
                    catch
                    {
                        return;
                    }

                    //Preloaded with the number  of elements required
                    _defaultValues = new List<KeyValuePair<bool, object>>();
                    for (int i = 0; i < Tables[CurrentRecordSet].Columns.Count; i++)
                    {
                        _defaultValues.Add(new KeyValuePair<bool, object>());
                    }
                    string defaultValue = String.Empty;
                    bool isComputed = false;
                    bool isUnknown = false;
                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        int thisColumnIndex = Tables[CurrentRecordSet].Columns.IndexOf(Convert.ToString(schemaTable.Rows[i]["COLUMN_NAME"]));
                        if (thisColumnIndex < 0) continue;

                        //13 Maximun length
                        if (Tables[CurrentRecordSet].Columns[thisColumnIndex].DataType == typeof(string))
                            Tables[CurrentRecordSet].Columns[thisColumnIndex].MaxLength = (schemaTable.Rows[i]["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value) ? Convert.ToInt32(schemaTable.Rows[i]["CHARACTER_MAXIMUM_LENGTH"]) : 255;

                        object originalValue = dbRow[thisColumnIndex];
                        if (schemaTable.Rows[i]["COLUMN_DEFAULT"] != DBNull.Value) //Has default value
                        {
                            defaultValue = (string)schemaTable.Rows[i]["COLUMN_DEFAULT"]; //8 Default Value
                            if (Tables[CurrentRecordSet].Columns[thisColumnIndex].DataType == typeof(bool))
                            {
                                defaultValue = defaultValue.Trim((new char[] { '(', ')', '\'' }));
                                dbRow[thisColumnIndex] = Convert.ToBoolean(Convert.ToDouble(defaultValue));
                            }
                            else
                            {
                                try
                                {
                                    string partialResult = defaultValue.Trim((new char[] { '(', ')', '\'' }));
                                    if (Tables[CurrentRecordSet].Columns[thisColumnIndex].MaxLength != -1) //is string
                                        dbRow[thisColumnIndex] = partialResult.Length > Tables[CurrentRecordSet].Columns[thisColumnIndex].MaxLength ? partialResult.Substring(0, Tables[CurrentRecordSet].Columns[thisColumnIndex].MaxLength) : partialResult;
                                    else
                                        dbRow[thisColumnIndex] = partialResult;
                                }
                                catch
                                {
                                    try
                                    {
                                        dbRow[thisColumnIndex] = ComputeValue(defaultValue);
                                        isComputed = true;
                                    }
                                    catch
                                    {
                                        isUnknown = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            object isNullable = schemaTable.Rows[i]["IS_NULLABLE"];
                            bool tmpRes;
                            if (isNullable != null
                                && (string.Equals(Convert.ToString(isNullable), "No", StringComparison.InvariantCultureIgnoreCase)
                                || (bool.TryParse(Convert.ToString(isNullable), out tmpRes) && !tmpRes))) //Not Allow Null and has not default value
                            {
                                //Add more if necesary
                                if (Tables[CurrentRecordSet].Columns[thisColumnIndex].DataType == typeof(string))
                                    dbRow[thisColumnIndex] = string.Empty;
                                else if (Tables[CurrentRecordSet].Columns[thisColumnIndex].DataType == typeof(Int16))
                                    dbRow[thisColumnIndex] = default(Int16);
                                else if (Tables[CurrentRecordSet].Columns[thisColumnIndex].DataType == typeof(Int32))
                                    dbRow[thisColumnIndex] = default(Int32);
                                else if (Tables[CurrentRecordSet].Columns[thisColumnIndex].DataType == typeof(bool))
                                    dbRow[thisColumnIndex] = default(bool);
                                else if (Tables[CurrentRecordSet].Columns[thisColumnIndex].DataType == typeof(decimal))
                                    dbRow[thisColumnIndex] = default(decimal);
                                else if (Tables[CurrentRecordSet].Columns[thisColumnIndex].DataType == typeof(byte))
                                    dbRow[thisColumnIndex] = default(byte);
                                else if (Tables[CurrentRecordSet].Columns[thisColumnIndex].DataType == typeof(char))
                                    dbRow[thisColumnIndex] = default(char);
                            }
                            else
                                dbRow[thisColumnIndex] = DBNull.Value;
                        }
                        if (isComputed)
                        {
                            _defaultValues[thisColumnIndex] = new KeyValuePair<bool, object>(true, defaultValue);
                            isComputed = false;
                        }
                        else if (isUnknown)
                        {
                            _defaultValues[thisColumnIndex] = new KeyValuePair<bool, object>(false, DBNull.Value);
                            isUnknown = false;
                        }
                        else
                            _defaultValues[thisColumnIndex] = new KeyValuePair<bool, object>(false, dbRow[thisColumnIndex]);

                        if (originalValue != DBNull.Value)
                            dbRow[thisColumnIndex] = originalValue;
                    }
                }
                else //already _defaultValues has been created
                {
                    try
                    {
                        dbRow.BeginEdit();
                        for (int i = 0; i < _defaultValues.Count; i++)
                        {
                            if (dbRow[i] == DBNull.Value)
                            {
                                if (!_defaultValues[i].Key)
                                    dbRow[i] = _defaultValues[i].Value;
                                else
                                    dbRow[i] = ComputeValue((string)_defaultValues[i].Value);
                            }
                        }
                    }
                    finally
                    {
                        dbRow.EndEdit();
                    }
                }
            }
            finally
            {
                if (!IsCachingAdapter && adapter != null)
                    adapter.Dispose();
            }
        }

        /// <summary>
        /// Saves any changes made to the DataRow recieved as parameter.
        /// </summary>
        /// <param name="theRow">The row to be save on the Database.</param>
        protected void UpdateWithNoEvents(DataRow theRow)
        {
            if (theRow.RowState != DataRowState.Unchanged)
            {
                if (!isBatchEnabled())
                {
                    DbConnection connection = GetConnection(ConnectionString);
                    DbDataAdapter dbAdapter = null;
                    try
                    {
                        dbAdapter = CreateAdapter(connection, true);
                        if (_requiresDefaultValues)
                            AssignDefaultValues(theRow);

                        dbAdapter.Update(new DataRow[] { theRow });
                    }
                    finally
                    {
                        if (!IsCachingAdapter)
                            if (dbAdapter != null) dbAdapter.Dispose();
                    }
                }
            }
        }
        /// <summary>
        /// Saves the changes done to the current record on the recordset.
        /// </summary>
        /// <remarks>If the recordset is not batch enabled this method saves the changes on the database.</remarks>
        public override void Update()
        {
            int returncode = 0;
            OnWillUpdateRows(ref returncode);
            DataRow theRow = CurrentRow;
            if (_newRow)
            {
                _newRow = false;
            }
            if (theRow.RowState != DataRowState.Unchanged)
            {
                if (!isBatchEnabled())
                {
                    UpdateWithNoEvents(theRow);
                    MoveFirst();
                }
            }
        }

        /// <summary>
        /// Saves a batch of changes to the database.
        /// </summary>
        public void BatchUpdate()
        {
            if (isBatchEnabled())
            {
                Update();
                DbConnection connection = GetConnection(_connectionString);
                using (DbDataAdapter dbAdapter = CreateAdapter(connection, true))
                {
                    DataTable changes = Tables[CurrentRecordSet].GetChanges();
                    if (changes != null)
                    {
                        dbAdapter.Update(changes);
                    }
                }
            }
            else
                throw new InvalidOperationException("The current RecordSet is not set for batch processing.");
        }
        /// <summary>
        /// Cancels execution of any pending process.
        /// </summary>
        public void Cancel()
        {
            DoCancelUpdate();
        }

        /// <summary>
        /// Cancels the changes done to the current recordset.
        /// </summary>
        public void CancelUpdate()
        {
            bool wasNewRow = _newRow;
            OnRowStatusChanged();
            if (wasNewRow)
                //base.Cancel();
                DoCancelUpdate();
            else
            {
                _editingMode = false;
                //  base.CancelUpdate();
                DoCancelUpdate();
            }
        }

        /// <summary>
        /// Releses the resources used by the recordset.
        /// </summary>
        public override void Close()
        {
            //CancelBatch();
            Tables[CurrentRecordSet].RejectChanges();
            _newRow = false;

            try
            {
                base.Close();
            }
            catch (Exception) { }

            finally
            {
                Dispose();
            }
        }
        #endregion

        #region Misc Methods

        /// <summary>
        /// Determines if the recordset is batch enabled.
        /// </summary>
        /// <returns></returns>
        protected override bool isBatchEnabled()
        {
            return _locktype == LockTypeConstants.rdConcurBatch;
        }

        /// <summary>
        /// Returns a delimited string for 'n' rows in a result set.
        /// </summary>
        /// <param name="numrows">Number of rows to be retrieved.</param>
        /// <param name="columnDelimiter">Expression used to separate the columns.</param>
        /// <param name="rowDelimiter">Expression used to separate the rows.</param>
        /// <param name="nullExpr">Expression used to replace nulls.</param>
        /// <returns>A delimited string containing a number of rows.</returns>
        public String GetClipString(int numrows, String columnDelimiter, String rowDelimiter, String nullExpr)
        {
            StringBuilder builder = new StringBuilder();
            OnRowCurrencyChange();
            int i = _index;
            for (; !EOF && _index < i + numrows; _index++)
            {
                foreach (Object data in CurrentRow.ItemArray)
                {
                    builder.Append(data == DBNull.Value ? nullExpr : Convert.ToString(data));
                    builder.Append(columnDelimiter);
                }
                builder.Append(rowDelimiter);
                _eof = _index >= Tables[CurrentRecordSet].Rows.Count - 1;
            }
            return builder.ToString();
        }

        /// <summary>
        /// Returns a delimited string for 'n' rows in a result set.
        /// </summary>
        /// <param name="numrows">Number of rows to be retrieved.</param>
        /// <param name="columnDelimiter">Expression used to separate the columns.</param>
        /// <param name="rowDelimiter">Expression used to separate the rows.</param>
        /// <returns>A delimited string containing a number of rows.</returns>
        public String GetClipString(int numrows, String columnDelimiter, String rowDelimiter)
        {
            return GetClipString(numrows, columnDelimiter, rowDelimiter, String.Empty);
        }

        /// <summary>
        /// Returns a delimited string for 'n' rows in a result set.
        /// </summary>
        /// <param name="numrows">Number of rows to be retrieved.</param>
        /// <param name="columnDelimiter">Expression used to separate the columns.</param>
        /// <returns>A delimited string containing a number of rows.</returns>
        public String GetClipString(int numrows, String columnDelimiter)
        {
            return GetClipString(numrows, columnDelimiter, '\n'.ToString());
        }

        /// <summary>
        /// Returns a delimited string for 'n' rows in a result set.
        /// </summary>
        /// <param name="numrows">Number of rows to be retrieved.</param>
        /// <returns>A delimited string containing a number of rows.</returns>
        public String GetClipString(int numrows)
        {
            return GetClipString(numrows, ' '.ToString());
        }

        /// <summary>
        /// Returns a two dimmension array representing 'n' rows in a result set.
        /// </summary>
        /// <param name="numrows">Number of rows to be retrieved.</param>
        /// <returns>A delimited string containing a number of rows.</returns>
        public object[,] GetRows(int numrows)
        {
            object[,] buffer = new object[Tables[CurrentRecordSet].Columns.Count, numrows];
            OnRowCurrencyChange();
            int i = _index, colindex = 0, rowindex = 0;
            for (; !EOF && _index < i + numrows; _index++)
            {
                foreach (Object data in CurrentRow.ItemArray)
                {
                    buffer[colindex, rowindex] = data;
                    colindex++;
                }
                colindex = 0;
                rowindex++;
                _eof = _index >= Tables[CurrentRecordSet].Rows.Count - 1;
            }
            object[,] result = new object[Tables[CurrentRecordSet].Columns.Count, rowindex];
            for (int rindex = 0; rindex < rowindex; rindex++)
                for (int cindex = 0; cindex < Tables[CurrentRecordSet].Columns.Count; cindex++)
                    result[cindex, rindex] = buffer[cindex, rindex];
            return result;
        }

        #endregion

        /// <summary>
        ///     Returns a new recordset according to the compound statement on the current recordset
        /// </summary>
        /// <returns>boolean true if there are more resultsets</returns>
        public bool MoreResults()
        {
            bool result = false;
            if (CurrentRecordSet < Tables.Count - 1)
            {
                CurrentRecordSet++;
                _index = 0;
                _eof = (Tables[CurrentRecordSet].Rows.Count == 0);
                result = true;
            }
            else
            {
                this.Close();
            }
            return result;
        }

        /// <summary>
        /// Performs the move action after setting a filter
        /// </summary>
        protected override void MoveAfterFilter()
        {
            MoveFirst();
        }

        #region Move Methods

        /// <summary>
        /// Used to handle the common move call.
        /// </summary>
        private delegate void MoveAction();


        /// <summary>
        /// Moves the current record pointer 'n' number of records.
        /// </summary>
        /// <param name="records">The number of records to move the record pointer.</param>
        public override void Move(int records)
        {
            OnRowCurrencyChange();
            // base.Move(records);
            BasicMove(_index + records);
        }

        /// <summary>
        /// Moves the record pointer to the first record.
        /// </summary>
        public override void MoveFirst()
        {
            // DoMove(base.MoveFirst);
            OnRowCurrencyChange();
            BasicMove(0);
        }

        /// <summary>
        /// Moves the record pointer to the last record.
        /// </summary>
        public override void MoveLast()
        {
            //DoMove(base.MoveLast);
            OnRowCurrencyChange();
            BasicMove((UsingView ? CurrentView.Count : Tables[CurrentRecordSet].Rows.Count));
        }

        /// <summary>
        /// Moves the record pointer to the next record.
        /// </summary>
        public override void MoveNext()
        {
            //DoMove(base.MoveNext);
            OnRowCurrencyChange();
            BasicMove(_index + 1);
        }

        /// <summary>
        /// Moves the record pointer to the previous record.
        /// </summary>
        public override void MovePrevious()
        {
            //DoMove(base.MovePrevious);
            OnRowCurrencyChange();
            BasicMove(_index - 1);
        }

        /// <summary>
        /// Actually executes the move method.
        /// </summary>
        private void DoMove(MoveAction action)
        {
            OnRowCurrencyChange();
            action();
        }
        /// <summary>
        /// Moves between the rows of the current recordset.
        /// </summary>
        protected void BasicMove(int newIndex)
        {
            _index = newIndex < 0 ? 0 : newIndex;
            _eof = _index > (UsingView ? CurrentView.Count - 1 : Tables[CurrentRecordSet].Rows.Count - 1);
            _index = _eof ? (UsingView ? CurrentView.Count - 1 : Tables[CurrentRecordSet].Rows.Count - 1) : _index;

            //base.BasicMove(newIndex);
            OnAfterMove();
        }
        #endregion

        #region event triggers

        /// <summary>
        /// Method to trigger the row status changed event when an update happens.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        void RDORecordSetHelper_RowChanging(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action != DataRowAction.Change && e.Action != DataRowAction.Nothing && e.Action != DataRowAction.Rollback && e.Action != DataRowAction.Commit)
            {
                OnRowStatusChanged();
            }
        }

        /// <summary>
        /// Fires event RowStatusChanged.
        /// </summary>
        private void OnRowStatusChanged()
        {
            if (RowStatusChanged != null)
                RowStatusChanged(this, new EventArgs());
        }

        /// <summary>
        /// Fires event RowCurrencyChange.
        /// </summary>
        private void OnRowCurrencyChange()
        {
            if (_newRow || _editingMode)
                CancelUpdate();
            if (RowCurrencyChange != null)
                RowCurrencyChange(this, new EventArgs());
        }

        /// <summary>
        /// Fires event WillUpdateRows.
        /// </summary>
        private void OnWillUpdateRows(ref int returncode)
        {
            if (WillUpdateRows != null)
            {
                WillUpdateRowsEventArgs e = new WillUpdateRowsEventArgs(returncode);
                WillUpdateRows(this, e);
                returncode = e.Returncode;
            }
        }

        /// <summary>
        /// Fires event Associate.
        /// </summary>
        private void OnAssociate()
        {
            if (Associate != null)
                Associate(this, new EventArgs());
        }

        /// <summary>
        /// Fires event Dissociate.
        /// </summary>
        private void OnDissociate()
        {
            if (Dissociate != null)
                Dissociate(this, new EventArgs());
        }

        /// <summary>
        /// Fires event WillAssociate.
        /// </summary>
        private void OnWillAssociate(DbConnection connection, ref bool cancel)
        {
            if (WillAssociate != null)
            {
                WillAssociateEventArgs e = new WillAssociateEventArgs(connection, cancel);
                WillAssociate(this, e);
                cancel = e.Cancel;
            }
        }

        /// <summary>
        /// Fires event WillDissociate.
        /// </summary>
        private void OnWillDissociate(ref bool cancel)
        {
            if (WillDissociate != null)
            {
                WillDissociateEventArgs e = new WillDissociateEventArgs(cancel);
                WillDissociate(this, e);
            }
        }

        #endregion
    }
}
