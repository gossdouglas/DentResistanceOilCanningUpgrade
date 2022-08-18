// <copyright file="FieldAttributeEnum.cs" company="Mobilize.Net">
//       Copyright (c) 2022 Mobilize, Inc. All Rights Reserved,
//       All classes are provided for customer use only,
//       all other use is prohibited without prior written consent from Mobilize.Net,
//       no warranty express or implied,
//       use at own risk.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpgradeHelpers.DB
{
    /// <summary>
    /// Specifies the attributes of a field object
    /// </summary>
    public enum FieldAttributeEnum
    {
        adFldCacheDeferred = 4096,
        adFldFixed = 16,
        adFldIsChapter = 8192,
        adFldIsCollection = 262144,
        adFldIsDefaultStream = 131072,
        adFldIsNullable = 32,
        adFldIsRowURL = 65536,
        adFldKeyColumn = 32768,
        adFldLong = 128,
        adFldMayBeNull = 64,
        adFldMayDefer = 2,
        adFldNegativeScale = 16384,
        adFldRowID = 256,
        adFldRowVersion = 512,
        adFldUnknownUpdatable = 8,
        adFldUnspecified = -1,
        adFldUpdatable = 4
    }
}
