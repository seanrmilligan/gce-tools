// https://docs.microsoft.com/en-us/windows/win32/api/nvme/ne-nvme-nvme_identify_cns_codes
// Copyright (c) Microsoft Corporation. All rights reserved.

namespace Microsoft.nvme.h
{
    public enum NVME_IDENTIFY_CNS_CODES
    {
        /// <summary>
        /// Information for a specific namespace will be returned.
        ///
        /// The Identify Namespace NVME_IDENTIFY_NAMESPACE_DATA structure is
        /// returned to the host for the namespace specified in the Namespace
        /// Identifier (NSID) member of the NVME_COMMAND structure, if the
        /// namespace is attached to this controller.
        ///
        /// If the specified namespace is an inactive namespace ID, then the
        /// controller returns a zero filled data structure.
        ///
        /// If the controller supports Namespace Management and NSID is set to
        /// FFFFFFFFh, the controller returns an NVME_IDENTIFY_NAMESPACE_DATA
        /// that specifies capabilities that are common across namespaces.
        /// </summary>
        NVME_IDENTIFY_CNS_SPECIFIC_NAMESPACE,
        
        /// <summary>
        /// Information for a controller will be returned to the host in an
        /// Identify Controller NVME_IDENTIFY_CONTROLLER_DATA data structure.
        /// </summary>
        NVME_IDENTIFY_CNS_CONTROLLER,
        
        /// <summary>
        /// A list of active namespaces will be returned
        ///
        /// A list of up to 1024 active namespace IDs is returned to the host
        /// containing active namespaces with a namespace identifier greater
        /// than the value specified in the NSID member of the NVME_COMMAND
        /// structure.
        /// </summary>
        NVME_IDENTIFY_CNS_ACTIVE_NAMESPACES,
        
        /// <summary>
        /// Information for a descriptor namespace will be returned.
        /// </summary>
        NVME_IDENTIFY_CNS_DESCRIPTOR_NAMESPACE,
        
        /// <summary>
        /// An NVM_SET_LIST will be returned.
        /// </summary>
        NVME_IDENTIFY_CNS_NVM_SET,
        NVME_IDENTIFY_CNS_SPECIFIC_NAMESPACE_IO_COMMAND_SET,
        NVME_IDENTIFY_CNS_SPECIFIC_CONTROLLER_IO_COMMAND_SET,
        NVME_IDENTIFY_CNS_ACTIVE_NAMESPACE_LIST_IO_COMMAND_SET,
        NVME_IDENTIFY_CNS_ALLOCATED_NAMESPACE_LIST,
        NVME_IDENTIFY_CNS_ALLOCATED_NAMESPACE,
        NVME_IDENTIFY_CNS_CONTROLLER_LIST_OF_NSID,
        NVME_IDENTIFY_CNS_CONTROLLER_LIST_OF_NVM_SUBSYSTEM,
        NVME_IDENTIFY_CNS_PRIMARY_CONTROLLER_CAPABILITIES,
        NVME_IDENTIFY_CNS_SECONDARY_CONTROLLER_LIST,
        NVME_IDENTIFY_CNS_NAMESPACE_GRANULARITY_LIST,
        NVME_IDENTIFY_CNS_UUID_LIST,
        NVME_IDENTIFY_CNS_DOMAIN_LIST,
        NVME_IDENTIFY_CNS_ENDURANCE_GROUP_LIST,
        NVME_IDENTIFY_CNS_ALLOCATED_NAMSPACE_LIST_IO_COMMAND_SET,
        NVME_IDENTIFY_CNS_ALLOCATED_NAMESPACE_IO_COMMAND_SET,
        NVME_IDENTIFY_CNS_IO_COMMAND_SET
        
    }
}