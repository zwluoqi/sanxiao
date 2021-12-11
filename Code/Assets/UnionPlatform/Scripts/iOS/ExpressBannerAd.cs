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
    /// The reward video Ad.
    /// </summary>
    public sealed class ExpressBannerAd : IDisposable
    {
        private static int loadContextID = 0;
        private static Dictionary<int, IExpressAdListener> loadListeners =
            new Dictionary<int, IExpressAdListener>();

        private static int interactionContextID = 0;
        private static Dictionary<int, IExpressAdInteractionListener> interactionListeners =
            new Dictionary<int, IExpressAdInteractionListener>();


        private delegate void ExpressAd_OnLoad(IntPtr expressAd, int context);
        private delegate void ExpressAd_OnLoadError(int code, string message, int context);
        private delegate void ExpressAd_WillVisible(int index, int context);
        private delegate void ExpressAd_DidClick(int index, int context);
        private delegate void ExpressAd_DidClose(int context);


        private IntPtr expressAd;
        private bool disposed;
        public int index;

        internal ExpressBannerAd(IntPtr expressAd)
        {
            this.expressAd = expressAd;
        }

        ~ExpressBannerAd()
        {
            this.Dispose(false);
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

            UnionPlatform_ExpressBannerAd_Dispose(this.expressAd);
            this.disposed = true;
        }

        public void ShowExpressAd(float originX, float originY)
        {
            UnionPlatform_ExpressBannersAd_Show(this.expressAd, originX, originY);
        }

        internal static void LoadExpressAd(
            AdSlot slot, IExpressAdListener listener)
        {
            var context = loadContextID++;
            loadListeners.Add(context, listener);

            UnionPlatform_ExpressBannersAd_Load(
                slot.CodeId,
                slot.viewwidth,
                slot.viewheight,
                slot.supportDeepLink,
                ExpressAd_OnLoadMethod,
                ExpressAd_OnLoadErrorMethod,
                context);
        }

        /// <summary>
        /// Sets the interaction listener for this Ad.
        /// </summary>
        public void SetDislikeCallback(
            IDislikeInteractionListener listener)
        {

        }


        /// <summary>
        /// Sets the interaction listener for this Ad.
        /// </summary>
        public void SetExpressInteractionListener(
            IExpressAdInteractionListener listener)
        {
            var context = interactionContextID++;
            interactionListeners.Add(context, listener);

            Debug.Log("chaors unity interactionContextID:" + interactionContextID);
            UnionPlatform_ExpressBannersAd_SetInteractionListener(
                this.expressAd,
                ExpressAd_WillVisibleMethod,
                ExpressAd_DidClickMethod,
                ExpressAd_OnAdDidCloseMethod,
                context);
        }

        /// <summary>
        /// Sets the download listener.
        /// </summary>
        public void SetDownloadListener(IAppDownloadListener listener)
        {
        }

        /// <summary>
        /// Gets the interaction type.
        /// </summary>
        public int GetInteractionType()
        {
            return 0;
        }


        [DllImport("__Internal")]
        private static extern void UnionPlatform_ExpressBannersAd_Load(
            string slotID,
            float width,
            float height,
            bool isSupportDeepLink,
            ExpressAd_OnLoad onLoad,
            ExpressAd_OnLoadError onLoadError,
            int context);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_ExpressBannersAd_Show(
            IntPtr expressAd,
            float originX,
            float originY);


        [DllImport("__Internal")]
        private static extern void UnionPlatform_ExpressBannersAd_SetInteractionListener(
            IntPtr expressAd,
            ExpressAd_WillVisible willVisible,
            ExpressAd_DidClick didClick,
            ExpressAd_DidClose didClose,
            int context);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_ExpressBannerAd_Dispose(
            IntPtr expressAdPtr);

        [AOT.MonoPInvokeCallback(typeof(ExpressAd_OnLoad))]
        private static void ExpressAd_OnLoadMethod(IntPtr expressAd, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                ;
                IExpressAdListener listener;
                if (loadListeners.TryGetValue(context, out listener))
                {
                    loadListeners.Remove(context);
                    listener.OnExpressBannerAdLoad(new ExpressBannerAd(expressAd));
                }
                else
                {
                    Debug.LogError(
                        "The ExpressAd_OnLoad can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(ExpressAd_OnLoadError))]
        private static void ExpressAd_OnLoadErrorMethod(int code, string message, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                IExpressAdListener listener;
                if (loadListeners.TryGetValue(context, out listener))
                {
                    listener.OnError(code, message);
                    loadListeners.Remove(context);
                }
                else
                {
                    Debug.LogError(
                        "The ExpressAd_OnLoadError can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(ExpressAd_WillVisible))]
        private static void ExpressAd_WillVisibleMethod(int index, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                IExpressAdInteractionListener listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdShow(null);
                }
                else
                {
                    Debug.LogError(
                        "The ExpressAd_WillVisible can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(ExpressAd_DidClick))]
        private static void ExpressAd_DidClickMethod(int index, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                IExpressAdInteractionListener listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdClicked(null);
                }
                else
                {
                    Debug.LogError(
                        "The ExpressAd_DidClick can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(ExpressAd_DidClose))]
        private static void ExpressAd_OnAdDidCloseMethod(int context)
        {
            //Debug.Log("chaors ExpressAd_OnAdDislikeMethod")
            UnityDispatcher.PostTask(() =>
            {
                IExpressAdInteractionListener listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    listener.OnAdClose(null);
                }
                else
                {
                    Debug.LogError(
                        "The ExpressAd_OnAdDidCloseMethod can not find the context.");
                }
            });
        }
    }
#endif
}
