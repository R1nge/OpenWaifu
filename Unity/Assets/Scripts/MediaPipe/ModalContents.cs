// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;

namespace Mediapipe.Unity.UI
{
    public class ModalContents : MonoBehaviour
    {
        private Modal _modal;

        private void Awake() => _modal = transform.parent.GetComponent<Modal>();
        
        protected Modal GetModal() => _modal;

        public virtual void Exit() => GetModal().Close();
    }
}