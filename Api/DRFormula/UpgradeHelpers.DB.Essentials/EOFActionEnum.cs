// <copyright file="EOFActionEnum.cs" company="Mobilize.Net">
//       Copyright (c) 2022 Mobilize, Inc. All Rights Reserved,
//       All classes are provided for customer use only,
//       all other use is prohibited without prior written consent from Mobilize.Net,
//       no warranty express or implied,
//       use at own risk.
// </copyright>

namespace UpgradeHelpers.DB
{
    /// <summary>
    /// EOF action values.
    /// </summary>
    public enum EOFActionEnum
    {
        /// <summary>
        /// Move Last
        /// </summary>
        MoveLast,
        /// <summary>
        /// End of file
        /// </summary>
        EOF,
        /// <summary>
        /// Add Record state
        /// </summary>
        Add
    }
}
