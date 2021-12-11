namespace ByteDance.Union {
#if !UNITY_EDITOR && UNITY_IOS
    using System;
    using UnityEngine;

    /// <summary>
    ///manager for native ad and express ad.
    /// </summary>
    public class NativeAdManager {
       
        private static NativeAdManager sNativeAdManager = new NativeAdManager();

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeAd"/> class.
        /// </summary>
        private NativeAdManager()
        {
        }
        public static NativeAdManager Instance()
        {
            return sNativeAdManager;
        }
        /// <summary>
        /// Shows the express ad.
        /// </summary>
        /// <param name="expressAd">Express ad.</param>
        /// <param name="listener">Listener.</param>
        /// <param name="dislikeInteractionListener">Dislike interaction listener.</param>
        public void ShowExpressFeedAd(AndroidJavaObject activity,AndroidJavaObject expressAd,IExpressAdInteractionListener listener,IDislikeInteractionListener dislikeInteractionListener) { }

        /// <summary>
        /// Shows the express banner ad.
        /// </summary>
        /// <param name="expressAd">Express ad.</param>
        /// <param name="listener">Listener.</param>
        /// <param name="dislikeInteractionListener">Dislike interaction listener.</param>
        public void ShowExpressBannerAd(AndroidJavaObject activity,AndroidJavaObject expressAd,IExpressAdInteractionListener listener,IDislikeInteractionListener dislikeInteractionListener) { }

        /// <summary>
       /// Shows the express interstitial ad.
       /// </summary>
       /// <param name="expressAd">Express ad.</param>
       /// <param name="listener">Listener.</param>
        public void ShowExpressInterstitialAd(AndroidJavaObject activity,AndroidJavaObject expressAd, IExpressAdInteractionListener listener) { }

        public void DestoryExpressAd(AndroidJavaObject expressAd) {}
    }
#endif
}