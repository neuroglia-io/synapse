/*
 * Copyright © 2022-Present The Synapse Authors
 * <p>
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * <p>
 * http://www.apache.org/licenses/LICENSE-2.0
 * <p>
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

namespace Synapse
{
    /// <summary>
    /// Defines extensions for objects
    /// </summary>
    public static class ObjectExtensions
    {

        /// <summary>
        /// Copies properties to the specified object
        /// </summary>
        /// <typeparam name="T">The type of the object to copy</typeparam>
        /// <param name="source">The object to copy the properties of</param>
        /// <param name="destination">The object to copy the properties to</param>
        public static void CopyTo<T>(this T source, T destination)
        {
            foreach(var property in typeof(T).GetProperties().Where(p => p.CanRead && p.CanWrite))
            {
                var value = property.GetValue(source);
                property.SetValue(destination, value);
            }
        }

    }

}
