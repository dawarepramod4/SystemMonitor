using System.Reflection;
using System.Runtime.Loader;

namespace SystemMonitor.Services.PluginLoaderService;

public class PluginLoadContextService(string pluginPath) : AssemblyLoadContext
{
    private AssemblyDependencyResolver _resolver = new(pluginPath);
    
    public Assembly LoadPlugin(string assemblyPath)
    {
        Console.WriteLine($"Loading commands from: {assemblyPath}");
        return LoadFromAssemblyName(
            new AssemblyName(Path.GetFileNameWithoutExtension(assemblyPath)));
    }
}