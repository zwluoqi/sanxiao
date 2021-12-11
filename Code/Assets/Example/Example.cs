//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

using ByteDance.Union;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// The example for the SDK.
/// </summary>
public sealed class Example : MonoBehaviour
{
    [SerializeField]
    private Text information;

    private AdNative adNative;
    private RewardVideoAd rewardAd;
    private FullScreenVideoAd fullScreenVideoAd;
    private NativeAd bannerAd;
    private NativeAd intersititialAd;
    private AndroidJavaObject mBannerAd;
    private AndroidJavaObject mIntersititialAd;
    private AndroidJavaObject activity;
    private AndroidJavaObject mNativeAdManager;
#if UNITY_IOS
    private ExpressRewardVideoAd expressRewardAd; // for iOS
    private ExpressFullScreenVideoAd expressFullScreenVideoAd; // for iOS
    private ExpressBannerAd iExpressBannerAd; // for iOS
    private ExpressInterstitialAd iExpressInterstitialAd; // for iOS
#else

#endif

    private ExpressAd mExpressFeedad;
    private ExpressAd mExpressBannerAd;
    private ExpressAd mExpressInterstitialAd;


    private AdNative AdNative
    {
        get
        {
            if (this.adNative == null)
            {
                this.adNative = SDK.CreateAdNative();
            }

            return this.adNative;
        }
    }

    public void LoadNativeNannerAd()
    {
#if UNITY_IOS
        if (this.bannerAd != null)
        {
            Debug.LogError("广告已经加载");
            this.information.text = "广告已经加载";
            return;
        }
#else
        if (this.mBannerAd != null)
        {
            Debug.LogError("广告已经加载");
            this.information.text = "广告已经加载";
            return;
        }
#endif

        var adSlot = new AdSlot.Builder()
#if UNITY_IOS
            .SetCodeId("900546687")
#else
            .SetCodeId("901121423")
#endif
            .SetSupportDeepLink(true)
            .SetImageAcceptedSize(600, 257)
            .SetNativeAdType(AdSlotType.Banner)
            .SetAdCount(1)
            .Build();
        this.AdNative.LoadNativeAd(adSlot, new NativeAdListener(this));
    }

    public void LoadNativeIntersititialAd()
    {
#if UNITY_IOS
        if (this.intersititialAd != null)
        {
            Debug.LogError("广告已经加载");
            this.information.text = "广告已经加载";
            return;
        }
#else
        if (this.mIntersititialAd != null)
        {
            Debug.LogError("广告已经加载");
            this.information.text = "广告已经加载";
            return;
        }
#endif
        var adSlot = new AdSlot.Builder()
#if UNITY_IOS
            .SetCodeId("900546829")   
#else
            .SetCodeId("901121435")
#endif
            .SetSupportDeepLink(true)
            .SetImageAcceptedSize(600, 257)
            .SetNativeAdType(AdSlotType.InteractionAd)
            .SetAdCount(1)
            .Build();
        this.AdNative.LoadNativeAd(adSlot, new NativeAdListener(this));
    }

    /// <summary>
    /// Load the reward Ad.
    /// </summary>
    public void LoadRewardAd()
    {
        if (this.rewardAd != null)
        {
            Debug.LogError("广告已经加载");
            this.information.text = "广告已经加载";
            return;
        }

        var adSlot = new AdSlot.Builder()
#if UNITY_IOS
            .SetCodeId("900546826")
#else
            .SetCodeId("901121430")
#endif
            .SetSupportDeepLink(true)
            .SetImageAcceptedSize(1080, 1920)
            .SetRewardName("金币") // 奖励的名称
            .SetRewardAmount(3) // 奖励的数量
            .SetUserID("user123") // 用户id,必传参数
            .SetMediaExtra("media_extra") // 附加参数，可选
            .SetOrientation(AdOrientation.Horizontal) // 必填参数，期望视频的播放方向
            .Build();

        this.AdNative.LoadRewardVideoAd(
            adSlot, new RewardVideoAdListener(this));
    }

    /// <summary>
    /// Show the reward Ad.
    /// </summary>
    public void ShowRewardAd()
    {
        if (this.rewardAd == null)
        {
            Debug.LogError("请先加载广告");
            this.information.text = "请先加载广告";
            return;
        }
        this.rewardAd.ShowRewardVideoAd();
    }

    /// <summary>
    /// Load the reward Ad.
    /// </summary>
    public void LoadExpressRewardAd()
    {
#if UNITY_IOS
             if (this.expressRewardAd != null)
#else
        if (this.rewardAd != null)
#endif
        {
            Debug.LogError("广告已经加载");
            this.information.text = "广告已经加载";
            return;
        }

        var adSlot = new AdSlot.Builder()
#if UNITY_IOS
        // @"900546566";//竖屏
        // @"900546606";//横屏
            .SetCodeId("900546566")
#else
            .SetCodeId("901121543")
#endif
            .SetSupportDeepLink(true)
            .SetImageAcceptedSize(1080, 1920)
            .SetRewardName("金币") // 奖励的名称
            .SetRewardAmount(3) // 奖励的数量
            .SetUserID("user123") // 用户id,必传参数
            .SetMediaExtra("media_extra") // 附加参数，可选
            .SetOrientation(AdOrientation.Horizontal) // 必填参数，期望视频的播放方向
            .Build();
#if UNITY_IOS
        this.AdNative.LoadExpressRewardAd(
            adSlot, new ExpressRewardVideoAdListener(this));
#else
        this.AdNative.LoadRewardVideoAd(
            adSlot, new RewardVideoAdListener(this));
#endif
    }

    /// <summary>
    /// Show the reward Ad.
    /// </summary>
    public void ShowExpressRewardAd()
    {
#if UNITY_IOS
        if (this.expressRewardAd == null)
        {
            Debug.LogError("请先加载广告");
            this.information.text = "请先加载广告";
            return;
        }
        this.expressRewardAd.ShowRewardVideoAd();
#else
        if (this.rewardAd == null)
        {
            Debug.LogError("请先加载广告");
            this.information.text = "请先加载广告";
            return;
        }
        this.rewardAd.ShowRewardVideoAd();
#endif
    }

    /// <summary>
    /// Loads the full screen video ad.
    /// </summary>
    public void LoadFullScreenVideoAd()
    {
        var adSlot = new AdSlot.Builder()
#if UNITY_IOS
                             .SetCodeId("900546299")
#else
                             .SetCodeId("901121375")
#endif
                             .SetSupportDeepLink(true)
                             .SetImageAcceptedSize(1080, 1920)
                             .SetOrientation(AdOrientation.Horizontal)
                             .Build();
        this.AdNative.LoadFullScreenVideoAd(adSlot, new FullScreenVideoAdListener(this));

    }

    /// <summary>
    /// Show the reward Ad.
    /// </summary>
    public void ShowFullScreenVideoAd()
    {
#if UNITY_IOS
        if (this.fullScreenVideoAd == null)
        {
            Debug.LogError("请先加载广告");
            this.information.text = "请先加载广告";
            return;
        }
        this.fullScreenVideoAd.ShowFullScreenVideoAd();
#else
        if (this.fullScreenVideoAd == null)
        {
            Debug.LogError("请先加载广告");
            this.information.text = "请先加载广告";
            return;
        }

        this.fullScreenVideoAd.ShowFullScreenVideoAd();
        this.fullScreenVideoAd = null;
#endif
    }

    /// <summary>
    /// Loads the full screen video ad.
    /// </summary>
    public void LoadExpressFullScreenVideoAd()
    {
        var adSlot = new AdSlot.Builder()
#if UNITY_IOS
        // 900546551 竖屏
        // 900546831 横屏
                             .SetCodeId("900546551")
#else
                               .SetCodeId("901121516")
#endif
                             .SetSupportDeepLink(true)
                             .SetImageAcceptedSize(1080, 1920)
                             .SetOrientation(AdOrientation.Horizontal)
                             .Build();
#if UNITY_IOS
        this.AdNative.LoadExpressFullScreenVideoAd(adSlot, new ExpressFullScreenVideoAdListener(this));
#else
        this.AdNative.LoadFullScreenVideoAd(adSlot, new FullScreenVideoAdListener(this));
#endif      
    }

    /// <summary>
    /// Show the reward Ad.
    /// </summary>
    public void ShowExpressFullScreenVideoAd()
    {
#if UNITY_IOS
        if (this.expressFullScreenVideoAd == null)
        {
            Debug.LogError("请先加载广告");
            this.information.text = "请先加载广告";
            return;
        }
        this.expressFullScreenVideoAd.ShowFullScreenVideoAd();
#else
        if (this.fullScreenVideoAd == null)
        {
            Debug.LogError("请先加载广告");
            this.information.text = "请先加载广告";
            return;
        }

        this.fullScreenVideoAd.ShowFullScreenVideoAd();
        this.fullScreenVideoAd = null;
#endif
    }

    public void LoadExpressBannerAd()
    {
        var adSlot = new AdSlot.Builder()
#if UNITY_IOS
                             .SetCodeId("900546269")
#else
                             .SetCodeId("901121246")
                               ////期望模板广告view的size,单位dp，//高度设置为0,则高度会自适应
#endif
                             .SetExpressViewAcceptedSize(400, 80)
                             .SetSupportDeepLink(true)
                             .SetImageAcceptedSize(1080, 1920)
                             .SetAdCount(1)
                             .SetOrientation(AdOrientation.Horizontal)
                             .Build();
        this.AdNative.LoadExpressBannerAd(adSlot, new ExpressAdListener(this,1));

    }

    /// <summary>
    /// Show the reward Ad.
    /// </summary>
    public void ShowExpressBannerAd()
    {
#if UNITY_IOS
        if (this.iExpressBannerAd == null)
        {
            Debug.LogError("请先加载广告");
            this.information.text = "请先加载广告";
            return;
        }
        this.iExpressBannerAd.ShowExpressAd(5, 100);
#else
        if (this.mExpressBannerAd == null)
        {
            Debug.LogError("请先加载广告");
            this.information.text = "请先加载广告";
            return;
        }
        //设置轮播间隔 30s--120s;不设置则不开启轮播
        this.mExpressBannerAd.SetSlideIntervalTime(30*1000);
        ExpressAdInteractionListener expressAdInteractionListener = new ExpressAdInteractionListener(this, 1);
        ExpressAdDislikeCallback dislikeCallback = new ExpressAdDislikeCallback(this,1);
        this.mExpressBannerAd.SetDownloadListener(
            new AppDownloadListener(this));
        NativeAdManager.Instance().ShowExpressBannerAd(GetActivity(), mExpressBannerAd.handle, expressAdInteractionListener, dislikeCallback);
#endif
    }

    public void LoadExpressInterstitialAd()
    {
        var adSlot = new AdSlot.Builder()
#if UNITY_IOS
                             .SetCodeId("900546270")
                             .SetExpressViewAcceptedSize(200, 300)
#else
                             .SetCodeId("901121133")
                             .SetExpressViewAcceptedSize(350, 0)
                               ////期望模板广告view的size,单位dp，//高度设置为0,则高度会自适应
#endif
                             .SetSupportDeepLink(true)
                             .SetAdCount(1)
                             .SetImageAcceptedSize(1080, 1920)
                             .Build();
        this.AdNative.LoadExpressInterstitialAd(adSlot, new ExpressAdListener(this,2));

    }

    /// <summary>
    /// Show the reward Ad.
    /// </summary>
    public void ShowExpressInterstitialAd()
    {
#if UNITY_IOS
        if (this.iExpressInterstitialAd == null)
        {
            Debug.LogError("请先加载广告");
            this.information.text = "请先加载广告";
            return;
        }
        this.iExpressInterstitialAd.ShowExpressAd(0, 0);
#else
        if (this.mExpressInterstitialAd == null)
        {
            Debug.LogError("请先加载广告");
            this.information.text = "请先加载广告";
            return;
        }
        ExpressAdInteractionListener expressAdInteractionListener = new ExpressAdInteractionListener(this, 1);
        this.mExpressInterstitialAd.SetDownloadListener(
            new AppDownloadListener(this));
        NativeAdManager.Instance().ShowExpressInterstitialAd(GetActivity(), mExpressInterstitialAd.handle, expressAdInteractionListener);
#endif
    }

    public void LoadExpressFeedAd()
    {
        var adSlot = new AdSlot.Builder()
#if UNITY_IOS
                             .SetCodeId("900546131")                   
#else
                             .SetCodeId("901121253")
                             ////期望模板广告view的size,单位dp，//高度设置为0,则高度会自适应
                             .SetExpressViewAcceptedSize(350, 0)
#endif
                             .SetSupportDeepLink(true)
                             .SetImageAcceptedSize(1080, 1920)
                             .SetOrientation(AdOrientation.Horizontal)
                             .SetAdCount(1) //请求广告数量为1到3条
                             .Build();
        this.AdNative.LoadNativeExpressAd(adSlot, new ExpressAdListener(this,0));

    }

    /// <summary>
    /// Show the expressFeed Ad.
    /// </summary>
    public void ShowExpressFeedAd()
    {
#if UNITY_IOS
        if (this.mExpressFeedad == null)
        {
            Debug.LogError("请先加载广告");
            this.information.text = "请先加载广告";
            return;
        }
        this.mExpressFeedad.SetExpressInteractionListener(
               new ExpressAdInteractionListener(this, 0));
        this.mExpressFeedad.SetDownloadListener(
            new AppDownloadListener(this));
        this.mExpressFeedad.ShowExpressAd(5,100);
#else
        if (this.mExpressFeedad == null)
        {
            Debug.LogError("请先加载广告");
            this.information.text = "请先加载广告";
            return;
        }
        ExpressAdInteractionListener expressAdInteractionListener = new ExpressAdInteractionListener(this, 0);
        ExpressAdDislikeCallback dislikeCallback = new ExpressAdDislikeCallback(this,0);
        this.mExpressFeedad.SetExpressInteractionListener(
            expressAdInteractionListener);
        this.mExpressFeedad.SetDownloadListener(
            new AppDownloadListener(this));
        NativeAdManager.Instance().ShowExpressFeedAd(GetActivity(),mExpressFeedad.handle,expressAdInteractionListener,dislikeCallback);
#endif
    }



    public void ShowNativeNannerAd()
    {
#if UNITY_IOS
       if (bannerAd == null)
        {
            Debug.LogError("请先加载广告");
            this.information.text = "请先加载广告";
            return;
        }
        this.bannerAd.ShowNativeAd();
#else
        if (mBannerAd == null)
        {
            Debug.LogError("请先加载广告");
            this.information.text = "请先加载广告";
            return;
        }
        if (mNativeAdManager == null)
        {
            mNativeAdManager = GetNativeAdManager();
        }
        mNativeAdManager.Call("showNativeBannerAd", activity, mBannerAd);
#endif
    }

    public void ShowNativeIntersititialAd()
    {
#if UNITY_IOS
        if (intersititialAd == null)
        {
            Debug.LogError("请先加载广告");
            this.information.text = "请先加载广告";
            return;
        }
        this.intersititialAd.ShowNativeAd();
#else
        if (mIntersititialAd == null)
        {
            Debug.LogError("请先加载广告");
            this.information.text = "请先加载广告";
            return;
        }
        if (mNativeAdManager == null)
        {
            mNativeAdManager = GetNativeAdManager();
        }
        mNativeAdManager.Call("showNativeIntersititialAd", activity, mIntersititialAd);
#endif
    }

    public AndroidJavaObject GetNativeAdManager()
    {
        if (mNativeAdManager != null)
        {
            return mNativeAdManager;
        }
        if (activity == null)
        {
            var unityPlayer = new AndroidJavaClass(
            "com.unity3d.player.UnityPlayer");
            activity = unityPlayer.GetStatic<AndroidJavaObject>(
           "currentActivity");
        }
        var jc = new AndroidJavaClass(
                    "com.bytedance.android.NativeAdManager");
        mNativeAdManager = jc.CallStatic<AndroidJavaObject>("getNativeAdManager");
        return mNativeAdManager;
    }

    public AndroidJavaObject GetActivity()
    {
        if (activity == null)
        {
            var unityPlayer = new AndroidJavaClass(
            "com.unity3d.player.UnityPlayer");
            activity = unityPlayer.GetStatic<AndroidJavaObject>(
           "currentActivity");
        }
        return activity;
    }

    /// <summary>
    /// Dispose the reward Ad.
    /// </summary>
    public void DisposeAds()
    {
#if UNITY_IOS
        if (this.rewardAd != null)
        {
            this.rewardAd.Dispose();
            this.rewardAd = null;
        }
        if (this.fullScreenVideoAd != null)
        {
            this.fullScreenVideoAd.Dispose();
            this.fullScreenVideoAd = null;
        }

        if (this.bannerAd != null)
        {
            this.bannerAd.Dispose();
            this.bannerAd = null;
        }
        if (this.intersititialAd != null)
        {
            this.intersititialAd.Dispose();
            this.intersititialAd = null;
        }
        if (this.expressRewardAd != null)
        {
            this.expressRewardAd.Dispose();
            this.expressRewardAd = null;
        }
        if (this.expressFullScreenVideoAd != null)
        {
            this.expressFullScreenVideoAd.Dispose();
            this.expressRewardAd = null;
        }
        if (this.iExpressBannerAd != null)
        {
            this.iExpressBannerAd.Dispose();
            this.iExpressBannerAd = null;
        }
#else
        if (this.rewardAd != null)
        {
            this.rewardAd = null;
        }
        if (this.fullScreenVideoAd != null)
        {
            this.fullScreenVideoAd = null;
        }
        if (this.mBannerAd != null)
        {
            this.mBannerAd = null;
        }
        if (this.mIntersititialAd != null)
        {
            this.mIntersititialAd = null;
        }
        if (this.mExpressFeedad != null)
        {
            NativeAdManager.Instance().DestoryExpressAd(mExpressFeedad.handle);
            mExpressFeedad = null;
        }
        if (this.mExpressBannerAd != null)
        {
            NativeAdManager.Instance().DestoryExpressAd(mExpressBannerAd.handle);
            mExpressBannerAd = null;
        }
        if (this.mExpressInterstitialAd != null)
        {
            NativeAdManager.Instance().DestoryExpressAd(mExpressInterstitialAd.handle);
            mExpressInterstitialAd = null;
        }
#endif
    }

    private sealed class RewardVideoAdListener : IRewardVideoAdListener
    {
        private Example example;

        public RewardVideoAdListener(Example example)
        {
            this.example = example;
        }

        public void OnError(int code, string message)
        {
            Debug.LogError("OnRewardError: " + message);
            this.example.information.text = "OnRewardError: " + message;
        }

        public void OnRewardVideoAdLoad(RewardVideoAd ad)
        {
            Debug.Log("OnRewardVideoAdLoad");
            this.example.information.text = "OnRewardVideoAdLoad";

            ad.SetRewardAdInteractionListener(
                new RewardAdInteractionListener(this.example));
            ad.SetDownloadListener(
                new AppDownloadListener(this.example));

            this.example.rewardAd = ad;
        }

        public void OnExpressRewardVideoAdLoad(ExpressRewardVideoAd ad)
        {
        }

        public void OnRewardVideoCached()
        {
            Debug.Log("OnRewardVideoCached");
            this.example.information.text = "OnRewardVideoCached";
        }
    }

    private sealed class ExpressRewardVideoAdListener : IRewardVideoAdListener
    {
        private Example example;

        public ExpressRewardVideoAdListener(Example example)
        {
            this.example = example;
        }

        public void OnError(int code, string message)
        {
            Debug.LogError("OnRewardError: " + message);
            this.example.information.text = "OnRewardError: " + message;
        }

        public void OnRewardVideoAdLoad(RewardVideoAd ad)
        {
            Debug.Log("OnRewardVideoAdLoad");
            this.example.information.text = "OnRewardVideoAdLoad";

            ad.SetRewardAdInteractionListener(
                new RewardAdInteractionListener(this.example));
            ad.SetDownloadListener(
                new AppDownloadListener(this.example));

            this.example.rewardAd = ad;
        }

        // iOS
        public void OnExpressRewardVideoAdLoad(ExpressRewardVideoAd ad)
        {
#if UNITY_IOS
            Debug.Log("OnRewardExpressVideoAdLoad");
            this.example.information.text = "OnRewardExpressVideoAdLoad";

            ad.SetRewardAdInteractionListener(
                new RewardAdInteractionListener(this.example));
            ad.SetDownloadListener(
                new AppDownloadListener(this.example));

            this.example.expressRewardAd = ad;
#else
#endif
        }

        public void OnRewardVideoCached()
        {
            Debug.Log("OnExpressRewardVideoCached");
            this.example.information.text = "OnExpressRewardVideoCached";
        }
    }
    /// <summary>
    /// Full screen video ad listener.
    /// </summary>
    private sealed class FullScreenVideoAdListener : IFullScreenVideoAdListener
    {
        private Example example;

        public FullScreenVideoAdListener(Example example)
        {
            this.example = example;
        }

        public void OnError(int code, string message)
        {
            Debug.LogError("OnFullScreenError: " + message);
            this.example.information.text = "OnFullScreenError: " + message;
        }

        public void OnFullScreenVideoAdLoad(FullScreenVideoAd ad)
        {
            Debug.Log("OnFullScreenAdLoad");
            this.example.information.text = "OnFullScreenAdLoad";

            ad.SetFullScreenVideoAdInteractionListener(
                new FullScreenAdInteractionListener(this.example));
            ad.SetDownloadListener(
                new AppDownloadListener(this.example));

            this.example.fullScreenVideoAd = ad;
        }

        // iOS
        public void OnExpressFullScreenVideoAdLoad(ExpressFullScreenVideoAd ad)
        {
            // rewrite
        }

        public void OnFullScreenVideoCached()
        {
            Debug.Log("OnFullScreenVideoCached");
            this.example.information.text = "OnFullScreenVideoCached";
        }
    }

    /// <summary>
    /// Full screen video ad listener.
    /// </summary>
    private sealed class ExpressFullScreenVideoAdListener : IFullScreenVideoAdListener
    {
        private Example example;

        public ExpressFullScreenVideoAdListener(Example example)
        {
            this.example = example;
        }

        public void OnError(int code, string message)
        {
            Debug.LogError("OnFullScreenError: " + message);
            this.example.information.text = "OnFullScreenError: " + message;
        }

        public void OnFullScreenVideoAdLoad(FullScreenVideoAd ad)
        {
            Debug.Log("OnFullScreenAdLoad");
            this.example.information.text = "OnFullScreenAdLoad";

            ad.SetFullScreenVideoAdInteractionListener(
                new FullScreenAdInteractionListener(this.example));
            ad.SetDownloadListener(
                new AppDownloadListener(this.example));

            this.example.fullScreenVideoAd = ad;
        }

        // iOS
        public void OnExpressFullScreenVideoAdLoad(ExpressFullScreenVideoAd ad)
        {
#if UNITY_IOS
            Debug.Log("OnExpressFullScreenAdLoad");
            this.example.information.text = "OnExpressFullScreenAdLoad";

            ad.SetFullScreenVideoAdInteractionListener(
                new FullScreenAdInteractionListener(this.example));
            ad.SetDownloadListener(
                new AppDownloadListener(this.example));

            this.example.expressFullScreenVideoAd = ad;
#else
#endif
        }

        public void OnFullScreenVideoCached()
        {
            Debug.Log("OnFullScreenVideoCached");
            this.example.information.text = "OnFullScreenVideoCached";
        }
    }

    private sealed class ExpressAdListener : IExpressAdListener
    {
        private Example example;
        private int type;//0:feed   1:banner  2:interstitial

        public ExpressAdListener(Example example, int type)
        {
            this.example = example;
            this.type = type;
            this.example.information.text = "ExpressAdListener";
        }
        public void OnError(int code, string message)
        {
            Debug.LogError("onExpressAdError: " + message);
            this.example.information.text = "onExpressAdError";
        }

        public void OnExpressAdLoad(List<ExpressAd> ads)
        {
            Debug.LogError("OnExpressAdLoad");
            this.example.information.text = "OnExpressAdLoad";
            IEnumerator<ExpressAd> enumerator = ads.GetEnumerator();
            if(enumerator.MoveNext()) 
            {
                switch (type)
                {
                    case 0:
                        this.example.mExpressFeedad = enumerator.Current;
                        break;
                    case 1:
                        this.example.mExpressBannerAd = enumerator.Current;
                        break;
                    case 2:
                        this.example.mExpressInterstitialAd = enumerator.Current;
                        break;
                }
            }
        }
#if UNITY_IOS

        public void OnExpressBannerAdLoad(ExpressBannerAd ad)
        {
            Debug.Log("OnExpressBannerAdLoad");
            this.example.information.text = "OnExpressBannerAdLoad";
            ad.SetExpressInteractionListener(
                new ExpressAdInteractionListener(this.example,1));
            ad.SetDownloadListener(
                new AppDownloadListener(this.example));
            this.example.iExpressBannerAd = ad;
        }

        public void OnExpressInterstitialAdLoad(ExpressInterstitialAd ad)
        {
            Debug.Log("OnExpressInterstitialAdLoad");
            this.example.information.text = "OnExpressInterstitialAdLoad";
            ad.SetExpressInteractionListener(
                new ExpressAdInteractionListener(this.example, 2));
            ad.SetDownloadListener(
                new AppDownloadListener(this.example));
            this.example.iExpressInterstitialAd = ad;
        }
#else
#endif
    }

    private sealed class ExpressAdInteractionListener : IExpressAdInteractionListener
    {
        private Example example;
        int type;//0:feed   1:banner  2:interstitial

        public ExpressAdInteractionListener(Example example, int type)
        {
            this.example = example;
            this.type = type;
        }
        public void OnAdClicked(ExpressAd ad)
        {
            Debug.LogError("express OnAdClicked,type:" + type);
        }

        public void OnAdShow(ExpressAd ad)
        {
            Debug.LogError("express OnAdShow,type:" + type);
        }

        public void OnAdViewRenderError(ExpressAd ad, int code, string message)
        {
            Debug.LogError("express OnAdViewRenderError,type:" + type);
        }

        public void OnAdViewRenderSucc(ExpressAd ad, float width, float height)
        {
            Debug.LogError("express OnAdViewRenderSucc,type:"+type);
        }
        public void OnAdClose(ExpressAd ad)
        {
            Debug.LogError("express OnAdClose,type:" + type);
        }
    }

    private sealed class ExpressAdDislikeCallback : IDislikeInteractionListener
    {
        private Example example;
        int type;//0:feed   1:banner
        public ExpressAdDislikeCallback(Example example, int type)
        {
            this.example = example;
            this.type = type;
        }
        public void OnCancel()
        {
            Debug.LogError("express dislike OnCancel");
        }

        public void OnSelected(int var1, string var2)
        {
            Debug.LogError("express dislike OnSelected:" + var2);
#if UNITY_IOS
#else
            //释放广告资源
            switch (type)
            {
                case 0:
                    if (this.example.mExpressFeedad != null)
                    {
                        NativeAdManager.Instance().DestoryExpressAd(this.example.mExpressFeedad.handle);
                        this.example.mExpressFeedad = null;
                    }
                    break;
                case 1:
                    if (this.example.mExpressBannerAd != null)
                    {
                        NativeAdManager.Instance().DestoryExpressAd(this.example.mExpressBannerAd.handle);
                        this.example.mExpressBannerAd = null;
                    }
                    break;
            }
#endif
        }
    }

    private sealed class NativeAdInteractionListener : IInteractionAdInteractionListener 
    {
        private Example example;

        public NativeAdInteractionListener(Example example)
        {
            this.example = example;
        }

        public void OnAdShow()
        {
            Debug.Log("NativeAd show");
            this.example.information.text = "NativeAd show";
        }

        public void OnAdClicked()
        {
            Debug.Log("NativeAd click");
            this.example.information.text = "NativeAd click";
        }

        public void OnAdDismiss()
        {
            Debug.Log("NativeAd close");
            this.example.information.text = "NativeAd close";
        }
    }
    private sealed class NativeAdListener : INativeAdListener
    {
        private Example example;

        public NativeAdListener(Example example)
        {
            this.example = example;
        }

        public void OnError(int code, string message)
        {
            Debug.LogError("OnNativeAdError: " + message);
            this.example.information.text = "OnNativeAdError: " + message;
        }

        public void OnNativeAdLoad(AndroidJavaObject list,NativeAd ad)
        {
#if UNITY_IOS
            this.example.bannerAd = ad;
            this.example.intersititialAd = ad;
            ad.SetNativeAdInteractionListener(
                new NativeAdInteractionListener(this.example)
            );
#else
            var size = list.Call<int>("size");
           
            if(size > 0)
            {
                this.example.mBannerAd = list.Call<AndroidJavaObject>("get", 0);
                this.example.mIntersititialAd = list.Call<AndroidJavaObject>("get", 0);
            }
           
#endif
            //if (ads == null && ads.[0])
            //{
            //    return;
            //}
            ////this.example.bannerAd = ads.[0];
            //this.example.bannerAd = ads.[0];
            Debug.Log("OnNativeAdLoad");
            this.example.information.text = "OnNativeAdLoad";


            //bannerAd.;
            //bannerAd.SetDownloadListener(
                //new AppDownloadListener(this.example));

        }
    }


    /// <summary>
    /// Full screen ad interaction listener.
    /// </summary>
    private sealed class FullScreenAdInteractionListener : IFullScreenVideoAdInteractionListener
    {
        private Example example;

        public FullScreenAdInteractionListener(Example example)
        {
            this.example = example;
        }

        public void OnAdShow()
        {
            Debug.Log("fullScreenVideoAd show");
            this.example.information.text = "fullScreenVideoAd show";
        }

        public void OnAdVideoBarClick()
        {
            Debug.Log("fullScreenVideoAd bar click");
            this.example.information.text = "fullScreenVideoAd bar click";
        }

        public void OnAdClose()
        {
            Debug.Log("fullScreenVideoAd close");
            this.example.information.text = "fullScreenVideoAd close";
        }

        public void OnVideoComplete()
        {
            Debug.Log("fullScreenVideoAd complete");
            this.example.information.text = "fullScreenVideoAd complete";
        }

        public void OnVideoError()
        {
            Debug.Log("fullScreenVideoAd OnVideoError");
            this.example.information.text = "fullScreenVideoAd OnVideoError";
        }

        public void OnSkippedVideo()
        {
            Debug.Log("fullScreenVideoAd OnSkippedVideo");
            this.example.information.text = "fullScreenVideoAd skipped";

        }
    }

    private sealed class RewardAdInteractionListener : IRewardAdInteractionListener
    {
        private Example example;

        public RewardAdInteractionListener(Example example)
        {
            this.example = example;
        }

        public void OnAdShow()
        {
            Debug.Log("rewardVideoAd show");
            this.example.information.text = "rewardVideoAd show";
        }

        public void OnAdVideoBarClick()
        {
            Debug.Log("rewardVideoAd bar click");
            this.example.information.text = "rewardVideoAd bar click";
        }

        public void OnAdClose()
        {
            Debug.Log("rewardVideoAd close");
            this.example.information.text = "rewardVideoAd close";
        }

        public void OnVideoComplete()
        {
            Debug.Log("rewardVideoAd complete");
            this.example.information.text = "rewardVideoAd complete";
        }

        public void OnVideoError()
        {
            Debug.LogError("rewardVideoAd error");
            this.example.information.text = "rewardVideoAd error";
        }

        public void OnRewardVerify(
            bool rewardVerify, int rewardAmount, string rewardName)
        {
            Debug.Log("verify:" + rewardVerify + " amount:" + rewardAmount +
                " name:" + rewardName);
            this.example.information.text =
                "verify:" + rewardVerify + " amount:" + rewardAmount +
                " name:" + rewardName;
        }
    }

    private sealed class AppDownloadListener : IAppDownloadListener
    {
        private Example example;

        public AppDownloadListener(Example example)
        {
            this.example = example;
        }

        public void OnIdle()
        {
        }

        public void OnDownloadActive(
            long totalBytes, long currBytes, string fileName, string appName)
        {
            Debug.Log("下载中，点击下载区域暂停");
            this.example.information.text = "下载中，点击下载区域暂停";
        }

        public void OnDownloadPaused(
            long totalBytes, long currBytes, string fileName, string appName)
        {
            Debug.Log("下载暂停，点击下载区域继续");
            this.example.information.text = "下载暂停，点击下载区域继续";
        }

        public void OnDownloadFailed(
            long totalBytes, long currBytes, string fileName, string appName)
        {
            Debug.LogError("下载失败，点击下载区域重新下载");
            this.example.information.text = "下载失败，点击下载区域重新下载";
        }

        public void OnDownloadFinished(
            long totalBytes, string fileName, string appName)
        {
            Debug.Log("下载完成，点击下载区域重新下载");
            this.example.information.text = "下载完成，点击下载区域重新下载";
        }

        public void OnInstalled(string fileName, string appName)
        {
            Debug.Log("安装完成，点击下载区域打开");
            this.example.information.text = "安装完成，点击下载区域打开";
        }
    }
}
