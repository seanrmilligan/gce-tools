// Copyright (c) Microsoft Corporation. All rights reserved.

// Adapted from nvme.h
namespace Google.Cloud.Storage.Windows.nvme
{
    public enum NVME_IDENTIFY_CNS_CODES
    {
        NVME_IDENTIFY_CNS_SPECIFIC_NAMESPACE            = 0,
        NVME_IDENTIFY_CNS_CONTROLLER                    = 1,
        NVME_IDENTIFY_CNS_ACTIVE_NAMESPACES             = 2,       // A list of up to 1024 active namespace IDs is returned to the host containing active namespaces with a namespace identifier greater than the value specified in the Namespace Identifier (CDW1.NSID) field.
    }
}