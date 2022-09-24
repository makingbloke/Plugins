// Copyright ©2021-2022 Mike King.
// This file is licensed to you under the MIT license.
// See the License.txt file in the solution root for more information.

using DotDoc.Plugins;
using DotDoc.Plugins.Exceptions;
using DotDoc.Plugins.Test.CalculatorInterface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace docDoc.Plugins.Tests;

/// <summary>
/// Simple plugin library tests.
/// </summary>
[TestClass]
public class PluginTest
{
    // Fullname of the add class in the plugin used for testing.
    private const string AddClassFullName = "DotDoc.Plugins.Test.Calculator.Add";

    // Path and filename of the calculator plugin used for testing.
    private readonly string _pluginFileName =
        Path.Combine(@"..\..\..\..\DotDoc.Plugins.Test.Calculator\Bin\Debug\net6.0", Plugin.GetPluginFileName("DotDoc.Plugins.Test.Calculator"));

    /// <summary>
    /// Test that a plugin can be loaded successfully.
    /// </summary>
    [TestMethod]
    public void TestCreatePlugin()
    {
        using Plugin plugin = new (this._pluginFileName);
    }

    /// <summary>
    /// Test that an exception is raised if a plugin is missing.
    /// </summary>
    [TestMethod]
    public void TestCreatePluginFail()
    {
        Assert.ThrowsException<FileNotFoundException>(() =>
        {
            using Plugin plugin = new ("NonExistentFileName.dll");
        });
    }

    /// <summary>
    /// Test getting the type of a class contained in the plugin by name.
    /// </summary>
    [TestMethod]
    public void TestGetPluginType()
    {
        using Plugin plugin = new (this._pluginFileName);
        Type pluginType = plugin.GetPluginType(AddClassFullName);

        Assert.AreEqual(AddClassFullName, pluginType?.FullName, "Incorrect type returned.");
    }

    /// <summary>
    /// Test getting a collection of types contained within the plugin.
    /// </summary>
    [TestMethod]
    public void TestPluginTypes()
    {
        using Plugin plugin = new (this._pluginFileName);
        IEnumerable<Type> pluginTypes = plugin.PluginTypes();

        Assert.AreEqual(1, pluginTypes.Count(), "Invalid number of types in collection.");
        Assert.AreEqual(AddClassFullName, pluginTypes.First().FullName, "Incorrect type returned.");
    }

    /// <summary>
    /// Test getting an instance of a class contained in the plugin by name.
    /// </summary>
    [TestMethod]
    public void TestCreateInstanceUsingFullName()
    {
        using Plugin plugin = new (this._pluginFileName);
        ICalculator instance = plugin.CreateInstance<ICalculator>(AddClassFullName);

        Assert.AreEqual(2, instance.Calculate(1, 1), "Invalid result from plugin.");
    }

    /// <summary>
    /// Test getting an instance of a class contained in the plugin by name and using constructor arguments.
    /// </summary>
    [TestMethod]
    public void TestCreateInstanceUsingFullNameAndConstructorArgs()
    {
        using Plugin plugin = new (this._pluginFileName);
        ICalculator instance = plugin.CreateInstance<ICalculator>(AddClassFullName, 100);

        Assert.AreEqual(102, instance.Calculate(1, 1), "Invalid result from plugin.");
    }

    /// <summary>
    /// Test getting an instance of a class fails if the plugin name is invalid.
    /// </summary>
    [TestMethod]
    public void TestCreateInstanceUsingFullNameFail()
    {
        using Plugin plugin = new (this._pluginFileName);

        Assert.ThrowsException<CannotCreatePluginTypeException>(() =>
        {
            plugin.CreateInstance<ICalculator>(typeof(object).FullName);
        });
    }

    /// <summary>
    /// Test getting an instance of a class contained in the plugin by type.
    /// </summary>
    [TestMethod]
    public void TestCreateInstanceUsingType()
    {
        using Plugin plugin = new (this._pluginFileName);
        Type pluginType = plugin.GetPluginType(AddClassFullName);
        ICalculator instance = plugin.CreateInstance<ICalculator>(pluginType);

        Assert.AreEqual(2, instance.Calculate(1, 1), "Invalid result from plugin.");
    }

    /// <summary>
    /// Test getting an instance of a class contained in the plugin by type and using constructor arguments..
    /// </summary>
    [TestMethod]
    public void TestCreateInstanceUsingTypeAndConstructorArgs()
    {
        using Plugin plugin = new (this._pluginFileName);
        Type pluginType = plugin.GetPluginType(AddClassFullName);
        ICalculator instance = plugin.CreateInstance<ICalculator>(pluginType, 100);

        Assert.AreEqual(102, instance.Calculate(1, 1), "Invalid result from plugin.");
    }

    /// <summary>
    /// Test getting an instance of a class fails if the plugin type is invalid.
    /// </summary>
    [TestMethod]
    public void TestCreateInstanceUsingTypeFail()
    {
        using Plugin plugin = new (this._pluginFileName);

        Assert.ThrowsException<CannotCreatePluginTypeException>(() =>
        {
            plugin.CreateInstance<ICalculator>(typeof(object));
        });
    }
}