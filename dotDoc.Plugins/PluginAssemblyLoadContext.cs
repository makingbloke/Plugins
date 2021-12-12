// Copyright ©2021 Mike King.
// This file is licensed to you under the MIT license.
// See the License.txt file in the solution root for more information.

using System.Reflection;
using System.Runtime.Loader;

namespace dotDoc.Plugins
{
    /// <summary>
    /// Runtime scope for simple plugin library.
    /// </summary>
    internal class PluginAssemblyLoadContext : AssemblyLoadContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginAssemblyLoadContext"/> class.
        /// </summary>
        /// <remarks>See <see cref="AssemblyLoadContext"/> for further details.</remarks>
        public PluginAssemblyLoadContext()
            : base(isCollectible: true)     // isCollectible = true means any plugin loaded with this can be unloaded.
        {
        }

        /// <inheritdoc/>
        protected override Assembly Load(AssemblyName name)
        {
            return null;
        }
    }
}
