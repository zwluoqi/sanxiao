using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ByteDance.Union;
using System;
//using XZXD.UI;

public class ADTouTiao : MonoBehaviour
{

	//	private RewardVideoAd rewardAd;
	private FullScreenVideoAd fullScreenVideoAd;
	private AdNative adNative;

	private AdNative AdNative {
		get {
			if (this.adNative == null) {
				this.adNative = SDK.CreateAdNative ();
			}

			return this.adNative;
		}
	}

	public static ADTouTiao Instance {
		get {
			return instance;
		}
	}

	private static ADTouTiao instance;

	void Awake ()
	{
		instance = this;
	}
	//
	//	/// <summary>
	//	/// Load the reward Ad.
	//	/// </summary>
	//	void LoadRewardAd()
	//	{
	//		if (this.rewardAd != null)
	//		{
	//			Debug.LogError("广告已经加载");
	////			this.information.text = "广告已经加载";
	//			return;
	//		}
	//
	//		var adSlot = new AdSlot.Builder()
	//			#if UNITY_IOS
	//			.SetCodeId("900546826")
	//			#else
	//			.SetCodeId("945156772")
	//			#endif
	//			.SetSupportDeepLink(true)
	//			.SetImageAcceptedSize((int)UIManager.Instance.GetUIWidth(), (int)UIManager.Instance.GetUIHeight())
	//			.SetRewardName("灵石") // 奖励的名称
	//			.SetRewardAmount(100) // 奖励的数量
	//			.SetUserID(Grow.GrowFun.Ins.selfZoneid+","+Grow.GrowFun.Ins.selfPid.ToString()) // 用户id,必传参数
	//			.SetMediaExtra("zoneid="+Grow.GrowFun.Ins.selfZoneid) // 附加参数，可选
	//			.SetOrientation(AdOrientation.Vertical) // 必填参数，期望视频的播放方向
	//			.Build();
	//
	//		this.AdNative.LoadRewardVideoAd(
	//			adSlot, new RewardVideoAds.RewardVideoAdListener(this));
	//	}
	//
	//	/// <summary>
	//	/// Show the reward Ad.
	//	/// </summary>
	//	void ShowRewardAd()
	//	{
	//		if (this.rewardAd == null)
	//		{
	//			Debug.LogError("请先加载广告");
	////			this.information.text = "请先加载广告";
	//			return;
	//		}
	//		this.rewardAd.ShowRewardVideoAd();
	//		this.rewardAd = null;
	//		onRequestCallBack = null;
	//		LoadRewardAd ();
	//	}

	Action onRequestCallBack;
	//	public void RequestRewardVideo ()
	//	{
	//		if (this.rewardAd == null) {
	//			onRequestCallBack = ShowRewardAd;
	//			LoadRewardAd ();
	//		} else {
	//			onRequestCallBack = null;
	//			ShowRewardAd ();
	//		}
	//	}

	public void OnRequest (RewardVideoAd ad)
	{
//		this.rewardAd = ad;
//		if (onRequestCallBack != null) {
//			onRequestCallBack ();
//		}
	}


	Action playDone = null;
	public void RequestRewardVideo (Action call)
	{
		this.playDone = call;
		if (this.fullScreenVideoAd == null) {
			onRequestCallBack = ShowFullScreenVideoAd;
			LoadFullScreenVideoAd ();
		} else {
			onRequestCallBack = null;
			ShowFullScreenVideoAd ();
		}
	}


	public string[] codeids = new string[] {
		"945157414",
		"945157413",
		"945157412",
		"945157411",
		"945157410",
		"945157408",
		"945157393",
	};
	public int adindex = 0;

	/// <summary>
	/// Loads the full screen video ad.
	/// </summary>
	public  void LoadFullScreenVideoAd ()
	{
		
		var adSlot = new AdSlot.Builder ()
			#if UNITY_IOS
			.SetCodeId(codeids[adindex])
			#else
			.SetCodeId (codeids [adindex])
			#endif
			.SetSupportDeepLink (true)
			.SetImageAcceptedSize (1080, 1920)
			.SetOrientation (AdOrientation.Horizontal)
			.Build ();
		this.AdNative.LoadFullScreenVideoAd (adSlot, new FullScreenAds.FullScreenVideoAdListener (this));

	}

	/// <summary>
	/// Show the reward Ad.
	/// </summary>
	 void ShowFullScreenVideoAd ()
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
		if (this.fullScreenVideoAd == null) {
			Debug.LogError ("请先加载广告");
//			this.information.text = "请先加载广告";
			return;
		}

		this.fullScreenVideoAd.ShowFullScreenVideoAd ();
		this.fullScreenVideoAd = null;
		#endif
	}

	public bool IsReady ()
	{
		return this.fullScreenVideoAd != null;
	}

	public void OnPlayDone(){
		if (playDone != null) {
			var temp = playDone;
			playDone = null;
			temp ();
		}
	}
	public void OnRequestFull (FullScreenVideoAd ad)
	{
		if (adindex >= codeids.Length - 1) {
			adindex = 0;
		} else {
			adindex++;
		}
		this.fullScreenVideoAd = ad;
		if (onRequestCallBack != null) {
			var temp = onRequestCallBack;
			onRequestCallBack = null;
			temp ();
		}
	}

	public sealed class AppDownloadListener : IAppDownloadListener
	{
		private ADTouTiao example;

		public AppDownloadListener (ADTouTiao example)
		{
			this.example = example;
		}

		public void OnIdle ()
		{
		}

		public void OnDownloadActive (
			long totalBytes, long currBytes, string fileName, string appName)
		{
			Debug.Log ("下载中，点击下载区域暂停");
//			this.example.information.text = "下载中，点击下载区域暂停";
		}

		public void OnDownloadPaused (
			long totalBytes, long currBytes, string fileName, string appName)
		{
			Debug.Log ("下载暂停，点击下载区域继续");
//			this.example.information.text = "下载暂停，点击下载区域继续";
		}

		public void OnDownloadFailed (
			long totalBytes, long currBytes, string fileName, string appName)
		{
			Debug.LogError ("下载失败，点击下载区域重新下载");
//			this.example.information.text = "下载失败，点击下载区域重新下载";
		}

		public void OnDownloadFinished (
			long totalBytes, string fileName, string appName)
		{
			Debug.Log ("下载完成，点击下载区域重新下载");
//			this.example.information.text = "下载完成，点击下载区域重新下载";
		}

		public void OnInstalled (string fileName, string appName)
		{
			Debug.Log ("安装完成，点击下载区域打开");
//			this.example.information.text = "安装完成，点击下载区域打开";
		}
	}



}
