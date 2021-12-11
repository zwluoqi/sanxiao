//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IOS)
    using UnityEngine;

    /// <summary>
    /// The advertisement native object for dummy.
    /// </summary>
    public sealed class AdNative
    {
        /// <summary>
        /// Load the feed Ad asynchronously and notice on listener.
        /// </summary>
        public void LoadFeedAd(AdSlot adSlot, IFeedAdListener listener)
        {
            listener.OnError(0, "Not Support on this platform");
        }

        /// <summary>
        /// Load the draw feed Ad asynchronously and notice on listener.
        /// </summary>
        public void LoadDrawFeedAd(AdSlot adSlot, IDrawFeedAdListener listener)
        {
            listener.OnError(0, "Not Support on this platform");
        }

        /// <summary>
        /// Load the native Ad asynchronously and notice on listener.
        /// </summary>
        public void LoadNativeAd(AdSlot adSlot, INativeAdListener listener)
        {
            listener.OnError(0, "Not Support on this platform");
        }

        /// <summary>
        /// Load the banner Ad asynchronously and notice on listener.
        /// </summary>
        public void LoadBannerAd(AdSlot adSlot, IBannerAdListener listener)
        {
            listener.OnError(0, "Not Support on this platform");
        }

        /// <summary>
        /// Load the interaction Ad asynchronously and notice on listener.
        /// </summary>
        public void LoadInteractionAd(
            AdSlot adSlot, IInteractionAdListener listener)
        {
            listener.OnError(0, "Not Support on this platform");
        }

        /// <summary>
        /// Load the splash Ad asynchronously and notice on listener with
        /// specify timeout.
        /// </summary>
        public void LoadSplashAd(
            AdSlot adSlot, ISplashAdListener listener, int timeOut)
        {
            listener.OnError(0, "Not Support on this platform");
        }

        /// <summary>
        /// Load the splash Ad asynchronously and notice on listener.
        /// </summary>
        public void LoadSplashAd(AdSlot adSlot, ISplashAdListener listener)
        {
            listener.OnError(0, "Not Support on this platform");
        }

        /// <summary>
        /// Load the reward video Ad asynchronously and notice on listener.
        /// </summary>
        public void LoadRewardVideoAd(
            AdSlot adSlot, IRewardVideoAdListener listener)
        {
            listener.OnError(0, "Not Support on this platform");
        }

        /// <summary>
        /// Load the full screen video Ad asynchronously and notice on listener.
        /// </summary>
        public void LoadFullScreenVideoAd(
            AdSlot adSlot, IFullScreenVideoAdListener listener)
        {
            listener.OnError(0, "Not Support on this platform");
        }

        public void LoadExpressRewardAd(
            AdSlot adSlot, IRewardVideoAdListener listener)
        {
            listener.OnError(0, "Not Support on this platform");
        }

        public void LoadExpressFullScreenVideoAd(
           AdSlot adSlot, IFullScreenVideoAdListener listener)
        {
            listener.OnError(0, "Not Support on this platform");
        }

        public void LoadNativeExpressAd(
            AdSlot adSlot, IExpressAdListener listener)
        {
            listener.OnError(0, "Not Support on this platform");
        }

        public void LoadExpressInterstitialAd(
            AdSlot adSlot, IExpressAdListener listener)
        {
            listener.OnError(0, "Not Support on this platform");
        }

        public void LoadExpressBannerAd(
            AdSlot adSlot, IExpressAdListener listener)
        {
            listener.OnError(0, "Not Support on this platform");
        }
    }
#endif
}
