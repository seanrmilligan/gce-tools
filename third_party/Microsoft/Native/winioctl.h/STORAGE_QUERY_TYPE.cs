// Copyright (c) third_party.Microsoft Corporation. All rights reserved.

// Adapted from winioctl.h
namespace Microsoft.Native.winioctl.h
{
    /// <summary>Types of queries</summary>
    public enum STORAGE_QUERY_TYPE : int
    {
        /// <summary>
        /// Retrieves the descriptor
        /// </summary>
        PropertyStandardQuery = 0,
        
        /// <summary>
        /// Used to test whether the descriptor is supported
        /// </summary>
        PropertyExistsQuery = 1,
        
        /// <summary>
        /// Used to retrieve a mask of writeable fields in the descriptor
        /// /summary>
        PropertyMaskQuery = 2,
        
        /// <summary>
        /// use to validate the value
        /// </summary>
        PropertyQueryMaxDefined = 3,
    }
}