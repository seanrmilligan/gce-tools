/* Copyright 2022 Google LLC
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
namespace Google.Cloud.Storage.Models;

public enum NvmeVersion : uint
{
    Version_1_0   = 0x00010000,
    Version_1_1   = 0x00010100,
    Version_1_2   = 0x00010200,
    Version_1_2_1 = 0x00010201,
    Version_1_3   = 0x00010300
}