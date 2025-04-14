using System.Reflection;
using System.Runtime.Loader;

namespace SystemMonitor.Services.PluginLoaderService;

public class PluginLoadContext(string pluginPath) : AssemblyLoadContext
{
    private AssemblyDependencyResolver _resolver = new(pluginPath);

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        string? assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
        if (assemblyPath != null)
        {
            return LoadFromAssemblyPath(assemblyPath);
        }

        return null;
    }


    public Assembly LoadPlugin()
    {
        Console.WriteLine($"Loading commands from: {pluginPath}");
        var assemblyName = Path.GetFileNameWithoutExtension(pluginPath);

        return LoadFromAssemblyName(
            new AssemblyName(assemblyName));
    }
}