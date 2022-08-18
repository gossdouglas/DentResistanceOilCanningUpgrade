using System;
using System.Data.Common;

namespace UpgradeHelpers.DB.RDO.Events
{
    /// <summary>
    /// Delegate to handle the RowStatusChangedEvent.
    /// </summary>
    /// <param name="sender">The object whic rises the event</param>
    /// <param name="e">The arguments of the event</param>
    public delegate void RowStatusChangedEventHandler(Object sender, EventArgs e);
    /// <summary>
    /// Delegate to handle the WillUpdateRowsEvent.
    /// </summary>
    /// <param name="sender">The object whic rises the event</param>
    /// <param name="e">The arguments of the event</param>
    public delegate void WillUpdateRowsEventHandler(Object sender, WillUpdateRowsEventArgs e);
    /// <summary>
    /// Delegate to handle the RowCurrencyChangeEvent.
    /// </summary>
    /// <param name="sender">The object which rises the event</param>
    /// <param name="e">The arguments of the event</param>
    public delegate void RowCurrencyChangeEventHandler(Object sender, EventArgs e);
    /// <summary>
    /// Delegate to handle the WillDissociateEvent.
    /// </summary>
    /// <param name="sender">The object whic rises the event</param>
    /// <param name="e">The arguments of the event</param>
    public delegate void WillDissociateEventHandler(Object sender, WillDissociateEventArgs e);
    /// <summary>
    /// Delegate to handle the WillAssociateEvent.
    /// </summary>
    /// <param name="sender">The object whic rises the event</param>
    /// <param name="e">The arguments of the event</param>
    public delegate void WillAssociateEventHandler(Object sender, WillAssociateEventArgs e);
    /// <summary>
    /// Delegate to handle the DissociateEvent.
    /// </summary>
    /// <param name="sender">The object whic rises the event</param>
    /// <param name="e">The arguments of the event</param>
    public delegate void DissociateEventHandler(Object sender, EventArgs e);
    /// <summary>
    /// Delegate to handle the AssociateEvent.
    /// </summary>
    /// <param name="sender">The object whic rises the event</param>
    /// <param name="e">The arguments of the event</param>
    public delegate void AssociateEventHandler(Object sender, EventArgs e);

    /// <summary>
    /// Arguments for the WillDissociateEvent.
    /// </summary>
    public class WillDissociateEventArgs : EventArgs
    {
        private bool _cancel;
        /// <summary>
        /// Creates a new WillDissociateEventArgs instance.
        /// </summary>
        /// <param name="cancel">Determines if the event is cancelled</param>
        public WillDissociateEventArgs(bool cancel)
        {
            _cancel = cancel;
        }
        /// <summary>
        /// Gets and set the cancel flag.
        /// </summary>
        public bool Cancel
        {
            get { return _cancel; }
            set { _cancel = value; }
        }
    }
    /// <summary>
    /// Arguments for the WillAssociateEvent.
    /// </summary>
    public class WillAssociateEventArgs : WillDissociateEventArgs
    {
        private readonly DbConnection connection;
        /// <summary>
        /// Creates a new WillDissociateEventArgs instance.
        /// </summary>
        /// <param name="connection">The connection to be associated</param>
        /// <param name="cancel">Determines if the event is cancelled</param>
        public WillAssociateEventArgs(DbConnection connection, bool cancel)
            : base(cancel)
        {
            this.connection = connection;
        }
        /// <summary>
        /// Gets the connection instance.
        /// </summary>
        public DbConnection Connection
        {
            get { return connection; }
        }
    }
    /// <summary>
    /// Arguments for the WillUpdateRowsEvent.
    /// </summary>
    public class WillUpdateRowsEventArgs : EventArgs
    {
        private readonly int returncode;
        /// <summary>
        /// Gets the return code.
        /// </summary>
        public int Returncode
        {
            get { return returncode; }
        }
        /// <summary>
        /// Creates a new WillUpdateRowsEventArgs instance.
        /// </summary>
        /// <param name="returncode">The return code of the event</param>
        public WillUpdateRowsEventArgs(int returncode)
        {
            this.returncode = returncode;
        }
    }
}
