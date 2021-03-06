// Copyright 2017 Google Inc. All rights reserved.
//
// Licensed under the MIT License, you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
//
//     http://www.opensource.org/licenses/mit-license.php
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using UnityEngine;
using UnityEngine.EventSystems;

/// Provides an interface for executing events for _IEventSystemHandler_.
public interface IGvrEventExecutor {
  bool Execute<T>(GameObject target,
    BaseEventData eventData,
    ExecuteEvents.EventFunction<T> functor)
    where T : IEventSystemHandler;

  GameObject ExecuteHierarchy<T>(GameObject root,
    BaseEventData eventData,
    ExecuteEvents.EventFunction<T> callbackFunction)
    where T : IEventSystemHandler;

  GameObject GetEventHandler<T>(GameObject root)
    where T : IEventSystemHandler;
}
