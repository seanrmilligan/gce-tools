﻿// Copyright (c) Microsoft Corporation. All rights reserved.

namespace Google.Cloud.Storage.Windows.winioctl
{
    /// <summary>
    /// The STORAGE_ASSOCIATION_TYPE enumeration indicates whether a storage
    /// descriptor identifies a device or a port.
    /// </summary>
    public enum STORAGE_ASSOCIATION_TYPE
    {
        /// <summary>
        /// Indicates that the descriptor identifies a device.
        /// </summary>
        StorageIdAssocDevice = 0,
        
        /// <summary>
        /// Indicates that the descriptor identifies a port.
        /// </summary>
        StorageIdAssocPort = 1,
        
        /// <summary>
        /// Indicates that the descriptor identifies a target.
        /// </summary>
        StorageIdAssocTarget = 2
    }
}