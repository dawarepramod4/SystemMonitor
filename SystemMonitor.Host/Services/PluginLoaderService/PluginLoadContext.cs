using System.Reflection;
using System.Runtime.Loader;

namespace SystemMonitor.Services.PluginLoaderService;

public class PluginLoadContext(string pluginPath) : AssemblyLoadContext
{
    private AssemblyDependencyResolver _resolver = new(pluginPath);
    
    public Assembly LoadPlugin()
    {
        Console.WriteLine($"Loading commands from: {pluginPath}");
        return LoadFromAssemblyName(
            new AssemblyName(Path.GetFileNameWithoutExtension(pluginPath)));
    }
}