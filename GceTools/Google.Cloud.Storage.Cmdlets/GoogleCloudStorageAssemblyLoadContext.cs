using System.Reflection;
using System.Runtime.Loader;

namespace Google.Cloud.Storage.Cmdlets;

internal class GoogleCloudStorageAssemblyLoadContext : AssemblyLoadContext
{
    private readonly string _dependencyDirPath;

    public GoogleCloudStorageAssemblyLoadContext(string dependencyDirPath)
    {
        _dependencyDirPath = dependencyDirPath;
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        // We do the simple logic here of looking for an assembly of the given name
        // in the configured dependency directory.
        string assemblyPath = Path.Combine(
            _dependencyDirPath,
            $"{assemblyName.Name}.dll");

        if (File.Exists(assemblyPath))
        {
            // The ALC must use inherited methods to load assemblies.
            // Assembly.Load*() won't work here.
            return LoadFromAssemblyPath(assemblyPath);
        }

        // For other assemblies, return null to allow other resolutions to continue.
        return null;
    }
}