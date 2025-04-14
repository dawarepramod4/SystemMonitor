using System.Reflection;
using SystemMonitor.Core;

namespace SystemMonitor.Services.PluginLoaderService;

public class PluginLoaderService()
{
    public List<IMonitorPlugin> GetAllPluginsFromAssembly()
    {
        try
        {
            //get the current directory which will be /bin from generated ILCode
            var binDir =Path.Combine(AppContext.BaseDirectory,"Plugins");
            
            //need to read all the dlls for assembly
            var files = Directory.GetFiles(binDir, "*.dll").ToList();
            
            //Host(current) and Core Dll will not contain any plugins
            files.Remove(typeof(Program).Assembly.Location);
            files.Remove(Path.Combine(binDir, "SystemMonitor.Core.dll"));
            
            var plugins = files.SelectMany(pluginPath =>
            {
                var pluginLoadContext = new PluginLoadContext(pluginPath);
                var pluginAssembly = pluginLoadContext.LoadPlugin();
                return CreateIMonitorPluginsFromAssembly(pluginAssembly);
            }).ToList();
            return plugins;
        }
        catch (IOException ioex)
        {
            Console.WriteLine(ioex.Message);
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
    
    /// <summary>
    /// Creates Commands from the provided Assembly from the classes using IMonitorPlugin
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    static IEnumerable<IMonitorPlugin> CreateIMonitorPluginsFromAssembly(Assembly assembly)
    {
        var count = 0;
        foreach (var type in assembly.GetTypes())
        {
            //name of the type should contain IMonitorPlugin implying it implements IMonitorPlugin
            if (!type.GetInterfaces().Any(intf => intf.FullName?.Contains(nameof(IMonitorPlugin)) ?? false)) continue;
            
            //cast the type to IMonitorPlugin
            if (Activator.CreateInstance(type) is not IMonitorPlugin result) continue;
            count++; 
            yield return result;
        }

        if (count != 0) yield break;
    }
}