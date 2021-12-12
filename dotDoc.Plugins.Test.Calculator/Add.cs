// Copyright ©2021 Mike King.
// This file is licensed to you under the MIT license.
// See the License.txt file in the solution root for more information.

using dotDoc.Plugins.Test.CalculatorInterface;

namespace dotDoc.Plugins.Test.Calculator
{
    /// <summary>
    /// Calculator method class used by the tests.
    /// </summary>
    public class Add : ICalculator
    {
        // Number which will be passed in via a constructor and added to the Add result. Used to test constructor parameters.
        private int _i0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Add"/> class.
        /// </summary>
        public Add() => _i0 = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Add"/> class.
        /// </summary>
        /// <param name="i0">Number which will be added to the Add result</param>
        public Add(int i0) => _i0 = i0;

        /// <summary>
        /// Add two integers.
        /// </summary>
        /// <param name="i1">Integer 1.</param>
        /// <param name="i2">Integer 2.</param>
        /// <returns>The sum of the two integers.</returns>
        public int Calculate(int i1, int i2) => _i0 + i1 + i2;
    }
}
