//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
#if !UNITY_EDITOR && UNITY_IOS
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;
    /// <summary>
    /// The native Ad.
    /// </summary>
    public sealed class NativeAd : IDisposable
    {
        private static int loadContextID = 0;
        private static Dictionary<int, INativeAdListener> loadListeners =
            new Dictionary<int, INativeAdListener>();

        private static int interactionContextID = 0;
        private static Dictionary<int, IInteractionAdInteractionListener> interactionListeners =
            new Dictionary<int, IInteractionAdInteractionListener>();

        private delegate void NativeAd_OnError(int code, string message, int context);
        private delegate void NativeAd_OnNativeAdLoad(IntPtr nativeAd, int context);

        private delegate void NativeAd_OnAdShow(int context);
        private delegate void NativeAd_OnAdDidClick(int context);
        private delegate void NativeAd_OnAdClose(int context);

        private IntPtr nativeAd;
        private bool disposed;

        internal NativeAd(IntPtr nativeAd)
        {
            this.nativeAd = nativeAd;
        }

        ~NativeAd()
        {
            this.Dispose(false);
        }

        public void ShowNativeAd () 
        {
             UnionPlatform_NativeAd_ShowNativeAd(this.nativeAd);
        }

        internal static void LoadNativeAd(
            AdSlot adSlot, INativeAdListener listener)
        {
            var context = loadContextID++;
            loadListeners.Add(context, listener);
            Debug.Log(adSlot.CodeId);

            UnionPlatform_NativeAd_Load(
            adSlot.CodeId,
            adSlot.adCount,
            adSlot.type,
            adSlot.width, 
            adSlot.height,
            adSlot.supportDeepLink,
            NativeAd_OnErrorMethod,
            NativeAd_OnNativeAdLoadMethod,
            context);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            UnionPlatform_NativeAd_Dispose(this.nativeAd);
            this.disposed = true;
        }

        public void SetNativeAdInteractionListener(
        IInteractionAdInteractionListener listener)
        {
            var context = interactionContextID++;
            interactionListeners.Add(context, listener);
            UnionPlatform_NativeAd_SetInteractionListener(
                this.nativeAd,
                NativeAd_OnAdShowMethod,
                NativeAd_OnAdDidClickMethed,
                NativeAd_OnAdCloseMethod,
                context);
        }


        [DllImport("__Internal")]
        private static extern void UnionPlatform_NativeAd_Dispose(
            IntPtr nativeAd);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_NativeAd_Load(
            string slotID,
            int adCount,
            AdSlotType nativeAdType,
            int width, 
            int height,
            bool supportDeepLink,
            NativeAd_OnError onError,
            NativeAd_OnNativeAdLoad onNativeAdLoad,
            int context);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_NativeAd_ShowNativeAd(IntPtr nativeAd);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_NativeAd_SetInteractionListener(
            IntPtr nativeAd,
            NativeAd_OnAdShow onAdShow,
            NativeAd_OnAdDidClick onAdNativeClick,
            NativeAd_OnAdClose onAdClose,
            int context);

        [AOT.MonoPInvokeCallback(typeof(NativeAd_OnError))]
        private static void NativeAd_OnErrorMethod(int code, string message, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                INativeAdListener listener;
                if (loadListeners.TryGetValue(context, out listener))
                {
                    loadListeners.Remove(context);
                    listener.OnError(code, message);
                }
                else
                {
                    Debug.LogError(
                        "The OnError can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(NativeAd_OnNativeAdLoad))]
        private static void NativeAd_OnNativeAdLoadMethod(IntPtr nativeAd, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                INativeAdListener listener;
                if (loadListeners.TryGetValue(context, out listener))
                {
                    loadListeners.Remove(context);
                    listener.OnNativeAdLoad(null,new NativeAd(nativeAd));
                } else
                {
                    Debug.LogError(
                        "The NativeAd_OnNativeAdLoad can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(NativeAd_OnAdShow))]
        private static void NativeAd_OnAdShowMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                IInteractionAdInteractionListener listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdShow();
                }
                else
                {
                    Debug.LogError(
                        "The OnAdShow can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(NativeAd_OnAdDidClick))]
        private static void NativeAd_OnAdDidClickMethed(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                IInteractionAdInteractionListener listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdClicked();
                }
                else
                {
                    Debug.LogError(
                        "The OnAdVideoBarClick can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(NativeAd_OnAdClose))]
        private static void NativeAd_OnAdCloseMethod(int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                IInteractionAdInteractionListener listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdDismiss();
                }
                else
                {
                    Debug.LogError(
                        "The OnAdClose can not find the context.");
                }
            });
        }
    }
#endif
}
