﻿/*
 * MIT License
 *
 * Copyright (c) 2022 bd_
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

#if UNITY_EDITOR // workaround issues with docfx

#region

using System;
using System.Linq;
using nadena.dev.ndmf.config;
using nadena.dev.ndmf.runtime;
using UnityEditor;
using UnityEngine;
using VRC.SDKBase.Editor.BuildPipeline;

#endregion

namespace nadena.dev.ndmf
{
    using UnityObject = UnityEngine.Object;
    
    [InitializeOnLoad]
    internal static class ApplyOnPlay
    {
        /**
         * We need to process avatars before lyuma's av3 emulator wakes up and processes avatars; it does this in Awake,
         * so we have to do our processing in Awake as well. This seems to work fine when first entering play mode, but
         * if you subsequently enable an initially-disabled avatar, processing from within Awake causes an editor crash.
         *
         * To workaround this, we initially process in awake; then, after OnPlayModeStateChanged is invoked (ie, after
         * all initially-enabled components have Awake called), we switch to processing from Start instead.
         */
        private static ApplyOnPlayGlobalActivator.OnDemandSource armedSource =
            ApplyOnPlayGlobalActivator.OnDemandSource.Awake;

        static ApplyOnPlay()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            ApplyOnPlayGlobalActivator.OnDemandProcessAvatar = MaybeProcessAvatar;
        }

        private static void MaybeProcessAvatar(ApplyOnPlayGlobalActivator.OnDemandSource source,
            MonoBehaviour component)
        {
            if (Config.ApplyOnPlay && source == armedSource && component != null)
            {
                var avatar = RuntimeUtil.FindAvatarInParents(component.transform);
                if (avatar == null) return;

                if (HookDedup.HasAvatar(avatar.gameObject)) return;
                
                VRCBuildPipelineCallbacks.OnPreprocessAvatar(avatar.gameObject);
                
                RecreateAnimators(avatar);
            }
        }

        private static void RecreateAnimators(Transform avatar)
        {
            // Recreate all animators. This avoids issues where old outfit animators might try
            // animating bones which have since moved.
            //
            // It's important to actually recreate animators here - if we try, for example, calling Rebind,
            // it can still start moving around stale bone references.
            var tmpObject = new GameObject();
            foreach (var animator in avatar.GetComponentsInChildren<Animator>(true))
            {
                // We need to store animator configuration somewhere while we recreate it.
                // Since we can't add two animators to the same object, we'll just stash it on a
                // temporary object.
                var obj = animator.gameObject;
                    
                var tmpAnimator = tmpObject.AddComponent<Animator>();
                bool enabled = animator.enabled;
                    
                EditorUtility.CopySerialized(animator, tmpAnimator);
                UnityObject.DestroyImmediate(animator);
                var newAnimator = obj.AddComponent<Animator>();
                newAnimator.enabled = false;
                EditorUtility.CopySerialized(tmpAnimator, newAnimator);
                newAnimator.enabled = enabled;
                    
                UnityObject.DestroyImmediate(tmpAnimator);
            }
            UnityObject.DestroyImmediate(tmpObject);
        }
        
        private static void OnPlayModeStateChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.EnteredPlayMode)
            {
                armedSource = ApplyOnPlayGlobalActivator.OnDemandSource.Start;
            }
            else if (obj == PlayModeStateChange.EnteredEditMode)
            {
                armedSource = ApplyOnPlayGlobalActivator.OnDemandSource.Awake;
                AvatarProcessor.CleanTemporaryAssets();
            }
        }
    }
}
#endif