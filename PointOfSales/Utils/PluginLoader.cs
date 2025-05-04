// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Reflection;
// using System.Text;
// using System.Threading.Tasks;
// using Microsoft.Extensions.DependencyInjection;
// using PointOfSales.Core.Commands;
// using PointOfSales.Core.Constants;
// using PointOfSales.Core.Plugin;
// using PointOfSales.Core.Utils;
//
// namespace PointOfSales.Utils;
//
// public interface IPluginLoader
// {
//     public Task<List<Assembly>> LoadPluginsAsync();
//     public Task<List<IPlugin>> InjectPluginsAsync(ServiceCollection collection, List<Assembly> assemblies);
// }
// public class PluginLoader(IApplicationLogger logger): IPluginLoader
// {
//     private const string PluginFile = ".plugins";
//     private readonly string _pluginDirectory = Path.Combine(LocalConfigurations.LocalFolderPath, LocalConfigurations.PluginFolder);
//
//     private async Task<string[]> ReadPluginConfig()
//     {
//         string filePath = Path.Combine(_pluginDirectory, PluginFile);
//         if (File.Exists(filePath))
//         {
//             
//             return await File.ReadAllLinesAsync(filePath);
//         }
//         logger.LogError("Plugin config file could not be read from {0}", filePath);
//         return [];
//     }
//     public async Task<List<Assembly>> LoadPluginsAsync()
//     {
//         var plugins =await ReadPluginConfig();
//         logger.LogInfo("Start loading plugins {0}", plugins.Length);
//         List<Assembly> loadedPlugins = new List<Assembly>();
//
//         foreach (var pluginFolder in plugins)
//         {
//             var pluginPath = Path.Combine(_pluginDirectory, pluginFolder);
//             if (!Directory.Exists(pluginPath))
//             {
//                 logger.LogError("Plugin config file could not be found in {0}", pluginFolder);
//                 continue;
//             }
//             var dllFiles = Directory.GetFiles(pluginPath, "*.dll", SearchOption.TopDirectoryOnly);
//             if (dllFiles.Length == 0)
//             {
//                 logger.LogWarning("No DLLs found in plugin folder: {0}",  pluginFolder);
//                 continue;
//             }
//             foreach (var dll in dllFiles)
//             {
//                 try
//                 {
//                     var assembly = Assembly.LoadFrom(dll);
//                     loadedPlugins.Add(assembly);
//                 }
//                 catch (Exception ex)
//                 {
//                     logger.LogError(ex,"Failed to load plugin '{0}': {1}", pluginFolder, dll);
//                 }
//             }
//         }
//         
//         return loadedPlugins;
//     }
//
//     public async Task<List<IPlugin>> InjectPluginsAsync(ServiceCollection collection, List<Assembly> assemblies)
//     {
//         List<IPluggable> pluggables = new();
//         List<IPlugin> plugins = new();
//         foreach (var assembly in assemblies)
//         {
//             var types = assembly.GetTypes()
//                 .Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
//             foreach (var type in types)
//             {
//                 // if (Activator.CreateInstance(type) is IPlugin instance)
//                 if (ActivatorUtilities.CreateInstance(collection.BuildServiceProvider(), type) is IPlugin instance)
//                 {
//                     plugins.Add(instance);
//                 }
//             }
//         }
//         foreach (var assembly in assemblies)
//         {
//             var types = assembly.GetTypes()
//                 .Where(t => typeof(IPluggable).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
//             foreach (var type in types)
//             {
//                 if (ActivatorUtilities.CreateInstance(collection.BuildServiceProvider(), type) is IPluggable instance)
//                 {
//                     await instance.OnInitAsync(collection);
//                 }
//             }
//         }
//
//         return plugins;
//     }
// }