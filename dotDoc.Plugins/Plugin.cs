// Copyright ©2021-2022 Mike King.
// This file is licensed to you under the MIT license.
// See the License.txt file in the solution root for more information.

using DotDoc.Plugins.Exceptions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;

namespace DotDoc.Plugins;

/// <summary>
/// Simple plugin library.
/// </summary>
public class Plugin : IDisposable
{
    private readonly AssemblyLoadContext _assemblyLoadContext;
    private readonly Assembly _assembly;
    private bool _isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="Plugin"/> class.
    /// </summary>
    /// <param name="fileName">Path and filename of plugin.</param>
    /// <param name="assemblyLoadContext">Optional instance of custom AssemblyLoadContext or <c>null</c> for default.</param>
    public Plugin(string fileName, AssemblyLoadContext assemblyLoadContext = null)
    {
        this._assemblyLoadContext = assemblyLoadContext ?? new PluginAssemblyLoadContext();
        this._assembly = this._assemblyLoadContext.LoadFromAssemblyPath(Path.GetFullPath(fileName));
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="Plugin"/> class.
    /// </summary>
    ~Plugin()
    {
        this.Dispose(false);
    }

    /// <summary>
    /// Converts a plugin name into a shared library name by adding a platform specific prefix and suffix.
    /// </summary>
    /// <remarks>This method is based on code in the .Net framework source.</remarks>
    /// <param name="pluginName">The plugin name.</param>
    /// <returns>A string containing the plugin name and a platform specific prefix and suffix.</returns>
    public static string GetPluginFileName(string pluginName)
    {
        string prefix;
        string suffix;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            prefix = string.Empty;
            suffix = ".dll";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            prefix = "lib";
            suffix = ".dylib";
        }
        else
        {
            prefix = "lib";
            suffix = ".so";
        }

        return prefix + pluginName + suffix;
    }

    /// <summary>
    /// Releases all resources used by <see cref="Plugin"/>.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Creates an instance of a class contained within the plugin.
    /// </summary>
    /// <typeparam name="T">The type return. For example, an interface or base class the plugin is derived from.</typeparam>
    /// <param name="fullName">The fully qualified name of the type to create.</param>
    /// <param name="args">Arguments for the plugin constructor.</param>
    /// <returns>An instance of the class.</returns>
    public T CreateInstance<T>(string fullName, params object[] args)
        where T : class
    {
        return this.GetPluginType(fullName) is Type type && Activator.CreateInstance(type, args) is T instance
            ? instance
            : throw new CannotCreatePluginTypeException();
    }

    /// <summary>
    /// Creates an instance of a class contained within the plugin.
    /// </summary>
    /// <typeparam name="T">The type return. For example, an interface or base class the plugin is derived from.</typeparam>
    /// <param name="type">The type to create.</param>
    /// <param name="args">Arguments for the plugin constructor.</param>
    /// <returns>An instance of the class.</returns>
    public T CreateInstance<T>(Type type, params object[] args)
        where T : class
    {
        return this.PluginTypes().Contains(type) && Activator.CreateInstance(type, args) is T instance
            ? instance
            : throw new CannotCreatePluginTypeException();
    }

    /// <summary>
    /// Gets the type of a class contained within the plugin.
    /// </summary>
    /// <param name="fullName">The fully qualified name of the type.</param>
    /// <returns>The plugin <see cref="Type"/> or <c>null</c> if the type cannot be found.</returns>
    public Type GetPluginType(string fullName)
    {
        return this._assembly.GetType(fullName, false, false);
    }

    /// <summary>
    /// Gets a collection of the public types that are held within the plugin.
    /// </summary>
    /// <returns>A collection of the public types that are held within the plugin.</returns>
    public IEnumerable<Type> PluginTypes()
    {
        return this._assembly.ExportedTypes;
    }

    /// <summary>
    /// Releases all resources used by <see cref="Plugin"/>.
    /// </summary>
    /// <param name="disposing"><c>true</c> dispose of managed objects.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!this._isDisposed)
        {
            this._isDisposed = true;
            this._assemblyLoadContext.Unload();
        }
    }
}
