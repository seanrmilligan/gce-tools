﻿/* Copyright 2022 Google LLC
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     https://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
namespace Google.Cloud.Storage.Models.Extensions
{
    public static class IEnumerableExtensions
    {
        public static int IndexOf<T>(this IEnumerable<T> enumerable, T obj)
        {
            int i = 0;

            foreach (T element in enumerable)
            {
                if (Equals(element, obj))
                {
                    return i;
                }

                i++;
            }

            return -1;
        }
        
        public static bool IsNullOrEmpty<T>(this IEnumerable<T>? enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
        
        public static bool None<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.Any();
        }
    }
}