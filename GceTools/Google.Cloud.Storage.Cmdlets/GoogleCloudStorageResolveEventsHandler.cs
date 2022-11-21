using System.Management.Automation;
using System.Reflection;
using System.Runtime.Loader;

namespace Google.Cloud.Storage.Cmdlets;

public class GoogleCloudStorageResolveEventsHandler : IModuleAssemblyInitializer, IModuleAssemblyCleanup
{
    // Get the path of the dependency directory.
    // In this case we find it relative to the Google.Cloud.Storage.Cmdlets.dll location
    private static readonly string dependencyDirPath = Path.GetFullPath(
        Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            "Dependencies"));

    private static readonly GoogleCloudStorageAssemblyLoadContext dependencyAlc = new(dependencyDirPath);

    public void OnImport()
    {
        // Add the Resolving event handler here
        AssemblyLoadContext.Default.Resolving += ResolveAlcEngine;
    }

    public void OnRemove(PSModuleInfo psModuleInfo)
    {
        // Remove the Resolving event handler here
        AssemblyLoadContext.Default.Resolving -= ResolveAlcEngine;
    }

    private static Assembly? ResolveAlcEngine(AssemblyLoadContext defaultAlc, AssemblyName assemblyToResolve)
    {
        // We only want to resolve the Alc.Engine.dll assembly here.
        // Because this will be loaded into the custom ALC,
        // all of *its* dependencies will be resolved
        // by the logic we defined for that ALC's implementation.
        //
        // Note that we are safe in our assumption that the name is enough
        // to distinguish our assembly here,
        // since it's unique to our module.
        // There should be no other AlcModule.Engine.dll on the system.
        if (!assemblyToResolve.Name.Equals("Google.Cloud.Storage.Engine"))
        {
            return null;
        }

        // Allow our ALC to handle the directory discovery concept
        //
        // This is where Alc.Engine.dll is loaded into our custom ALC
        // and then passed through into PowerShell's ALC,
        // becoming the bridge between both
        return dependencyAlc.LoadFromAssemblyName(assemblyToResolve);
    }
}