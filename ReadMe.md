# Plugins

Copyright ©2021 Mike King.  
Licensed using the MIT licence. See the License.txt file in the solution root for more information.  

## Overview

Plugins is a simple library for dynamically loading, using and unloading plugin assemblies.

## Pre-Requisites

This solution uses .Net 6.0.

## Usage

To create a plugin first create a two standard library projects, one having the plugin interface and the other the plugin code itself. (See the dotDoc.Plugins.Test.CalculatorInterface / dotDoc.Plugins.Test.Calculator projects used by the unit tests for an example.)

Next create an instance of the ```Plugin``` class. Pass in the path / filename of the plugin. There is a static helper method in ```Plugin``` called ```GetPluginFileName``` which when passed a plugin name with no extension adds any prefix / suffix needed for the current platform e.g., a .dll suffix for windows or a lib prefix and .so suffix for Linux.

Plugin is a disposable class when the instance is disposed then the plugin assemblies are unloaded.

To create an instance of a class held within the plugin call ```CreateInstance``` passing either the full name of the class or the type of the class and optionally any constructor arguments used by the class. The type can be obtained by calling ```GetPluginType``` with the full name of the class or using an entry in the type collection returned by ```PluginTypes```. If the class created is disposable, then remember to dispose it before disposing of the plugin as the assembly will be unloaded.

### Example

```
string pluginFileName = Path.Combine(@"c:\temp\pluginassemblies", Plugin.GetPluginFileName("Calculator"));
using Plugin plugin = new (pluginFileName);

ICalculator addInstance = plugin.CreateInstance<ICalculator>("Calculator.Add");

int result = addInstance.Calculate(1, 1);
```
