using System;
using System.Collections.Generic;
using System.Data.Common;

namespace UpgradeHelpers.DB.RDO
{
    /// <summary>
    /// This class represents the rdoEngine class.
    /// </summary>
    public class RDOEngineHelper : EngineHelper<RDOEnvironmentHelper>
    {
        /// <summary>
        /// timeout value
        /// </summary>
        private int _timeout = 15;

        private static RDOEngineHelper instance;

        /// <summary>
        /// This returns the default instance of the Engine as exposed by RDO.
        /// </summary>
        /// <param name="factory">This is the Provider factory used internally to create the necesary ADO .Net internal objects.</param>
        /// <returns>A new instance of the rdoEngine object.</returns>
        public static RDOEngineHelper Instance(DbProviderFactory factory)
        {
            if (instance == null)
            {
                instance = new RDOEngineHelper(factory);
            }
            return (RDOEngineHelper)instance;
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="factory">This is the Provider factory used internally to create the necesary ADO .Net internal objects.</param>
        protected RDOEngineHelper(DbProviderFactory factory)
            : base(factory)
        {
        }

        /// <summary>
        /// Gets and sets the default login time out.
        /// </summary>
        public int DefaultLoginTimeOut
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        /// <summary>
        /// Creates a new Environment instance as exposed by RDO.
        /// </summary>
        /// <param name="name">The name of the new environment instance.</param>
        public void CreateEnvironment(String name)
        {
            RDOEnvironmentHelper env = new RDOEnvironmentHelper(factory);
            env.Name = name;
            env.LoginTimeOut = _timeout;
            connectionContainers.Add(env);
        }

        /// <summary>
        /// Gets the list of evironments contained on this engine instance.
        /// </summary>
        public List<RDOEnvironmentHelper> Environments
        {
            get { return connectionContainers; }
        }
    }
}
