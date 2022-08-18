namespace UpgradeHelpers.DB.RDO
{
    /// <summary>
    /// Defines the type of concurrence to be used by the recordset.
    /// </summary>
    public enum LockTypeConstants
    {
        /// <summary>
        /// Read Only
        /// </summary>
        rdConcurReadOnly = 1,
        /// <summary>
        /// Lock
        /// </summary>
        rdConcurLock,
        /// <summary>
        /// By Row
        /// </summary>
        rdConcurRowVer,
        /// <summary>
        /// By Value
        /// </summary>
        rdConcurValues,
        /// <summary>
        /// Batch
        /// </summary>
        rdConcurBatch
    }
}