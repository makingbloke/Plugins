// Copyright ©2021 Mike King.
// This file is licensed to you under the MIT license.
// See the License.txt file in the solution root for more information.

namespace dotDoc.Plugins.Test.CalculatorInterface
{
    /// <summary>
    /// Interface for calculator classes used by the tests.
    /// </summary>
    public interface ICalculator
    {
        /// <summary>
        /// Perform an operation on two integers.
        /// </summary>
        /// <param name="i1">Integer 1.</param>
        /// <param name="i2">Integer 2.</param>
        /// <returns>The result of the operation.</returns>
        public int Calculate(int i1, int i2);
    }
}
