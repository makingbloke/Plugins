// Copyright ©2021 Mike King.
// This file is licensed to you under the MIT license.
// See the License.txt file in the solution root for more information.

using System;
using System.Runtime.Serialization;

namespace dotDoc.Plugins.Exceptions
{
    [Serializable]
    public class CannotCreatePluginTypeException : Exception
    {
        public CannotCreatePluginTypeException()
        {
        }

        public CannotCreatePluginTypeException(string message)
            : base(message)
        {
        }

        public CannotCreatePluginTypeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected CannotCreatePluginTypeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
