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
    public sealed class ExpressAd : IDisposable
    {
        private static int loadContextID = 0;
        private static Dictionary<int, IExpressAdListener> loadListeners =
            new Dictionary<int, IExpressAdListener>();

        private static int interactionContextID = 0;
        private static Dictionary<int, IExpressAdInteractionListener> interactionListeners =
            new Dictionary<int, IExpressAdInteractionListener>();

        private static int dislikeContextID = 0;
        private static Dictionary<int, IDislikeInteractionListener> dislikeListeners =
            new Dictionary<int, IDislikeInteractionListener>();

        private delegate void ExpressAd_OnLoad(IntPtr expressAd, int context);
        private delegate void ExpressAd_OnLoadError(int code, string message, int context);

        private delegate void ExpressAd_OnAdViewRenderSucc(int index, float width, float height, int context);
        private delegate void ExpressAd_OnAdViewRenderError(int index, int code, string message, int context);
        private delegate void ExpressAd_WillVisible(int index, int context);
        private delegate void ExpressAd_DidClick(int index, int context);
        private delegate void ExpressAd_OnAdDislike(int index, int dislikeID, string dislikeReason, int context);


        private IntPtr expressAd;
        private IntPtr expressAdView;
        static List<ExpressAd> expressAds = new List<ExpressAd>();
        private bool disposed;
        public int index;

        internal ExpressAd(IntPtr expressAd, int flag)
        {
            if (flag == 0)
            {
                this.expressAd = expressAd;
            }
            else
            {
                this.expressAdView = expressAdView;
            }

        }

        internal ExpressAd(int index)
        {
            this.index = index;
        }

        ~ExpressAd()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Sets the slide interval time.
        /// </summary>
        /// <param name="intervalTime">Interval time.</param>
        public void SetSlideIntervalTime(int intervalTime){}

        public void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            UnionPlatform_ExpressAd_Dispose(this.expressAd);
            UnionPlatform_ExpressAdView_Dispose(this.expressAdView);
            this.disposed = true;
        }

        public void ShowExpressAd(float originX, float originY)
        {
            UnionPlatform_ExpressAd_Show(this.index, originX, originY);
        }

        internal static void LoadExpressAdAd(
            AdSlot slot, IExpressAdListener listener)
        {
            var context = loadContextID++;
            loadListeners.Add(context, listener);

            for (int i = 0; i < slot.adCount; i++)
            {
                expressAds.Add(new ExpressAd(i));
            }

            UnionPlatform_ExpressAd_Load(
                slot.CodeId,
                slot.width,
                slot.height,
                slot.supportDeepLink,
                slot.adCount,
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
            Debug.Log("chaors SetDislikeCallback");
            var context = dislikeContextID++;
            dislikeListeners.Add(context, listener);

            UnionPlatform_ExpressAd_SetDislikeListener(
                ExpressAd_OnAdDislikeMethod,
                context);
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
            UnionPlatform_ExpressAd_SetInteractionListener(
                this.expressAd,
                ExpressAd_OnAdViewRenderSuccMethod,
                ExpressAd_OnAdViewRenderErrorMethod,
                ExpressAd_WillVisibleMethod,
                ExpressAd_DidClickMethod,
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
        private static extern void UnionPlatform_ExpressAd_Load(
            string slotID,
            float width,
            float height,
            bool isSupportDeepLink,
            int adCount,
            ExpressAd_OnLoad onLoad,
            ExpressAd_OnLoadError onLoadError,
            int context);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_ExpressAd_Show(
            int index,
            float originX,
            float originY);


        [DllImport("__Internal")]
        private static extern void UnionPlatform_ExpressAd_SetInteractionListener(
            IntPtr rewardVideoAd,
            ExpressAd_OnAdViewRenderSucc viewRenderSucc,
            ExpressAd_OnAdViewRenderError ViewRenderError,
            ExpressAd_WillVisible willVisible,
            ExpressAd_DidClick didClick,
            int context);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_ExpressAd_SetDislikeListener(
                ExpressAd_OnAdDislike ExpressAd_OnAdDislikeMethod,
                int context);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_ExpressAd_Dispose(
            IntPtr expressAdPtr);

        [DllImport("__Internal")]
        private static extern void UnionPlatform_ExpressAdView_Dispose(
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
                    listener.OnExpressAdLoad(expressAds);
                    loadListeners.Remove(context);
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

        [AOT.MonoPInvokeCallback(typeof(ExpressAd_OnAdViewRenderSucc))]
        private static void ExpressAd_OnAdViewRenderSuccMethod(int index, float width, float height, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                IExpressAdInteractionListener listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    ExpressAd expressAd = expressAds[index];
                    listener.OnAdViewRenderSucc(expressAd, width, height);
                }
                else
                {
                    Debug.LogError(
                        "The ExpressAd_OnAdViewRenderSucc can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(ExpressAd_OnAdViewRenderError))]
        private static void ExpressAd_OnAdViewRenderErrorMethod(int index, int code, string message, int context)
        {
            UnityDispatcher.PostTask(() =>
            {
                IExpressAdInteractionListener listener;
                if (interactionListeners.TryGetValue(context, out listener))
                {
                    ExpressAd expressAd = expressAds[index];
                    listener.OnAdViewRenderError(expressAd, code, message);
                }
                else
                {
                    Debug.LogError(
                        "The ExpressAd_OnAdViewRenderError can not find the context.");
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
                    ExpressAd expressAd = expressAds[index];
                    listener.OnAdShow(expressAd);
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
                    ExpressAd expressAd = expressAds[index];
                    listener.OnAdClicked(expressAd);
                }
                else
                {
                    Debug.LogError(
                        "The ExpressAd_DidClick can not find the context.");
                }
            });
        }

        [AOT.MonoPInvokeCallback(typeof(ExpressAd_OnAdDislike))]
        private static void ExpressAd_OnAdDislikeMethod(int index, int dislikeID, string dislikeReason, int context)
        {
            //Debug.Log("chaors ExpressAd_OnAdDislikeMethod")
            UnityDispatcher.PostTask(() =>
            {
                IDislikeInteractionListener listener;
                if (dislikeListeners.TryGetValue(context, out listener))
                {
                    ExpressAd expressAd = expressAds[index];
                    listener.OnSelected(dislikeID, dislikeReason);
                }
                else
                {
                    Debug.LogError(
                        "The ExpressAd_OnAdDislike can not find the context.");
                }
            });
        }
    }
#endif
}
