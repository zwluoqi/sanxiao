using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine.UI;
/// <summary>
/// 挂载到空物体或者Button上都可以，看个人编程喜好
/// </summary>
public class AdSystem : MonoBehaviour
{
////	游戏广告ID（unityAds官网注册账号，申请游戏得到的游戏广告ID）
//	private string gameId;
//	//是否处于测试模式
//	private bool enableTestMode = false;
//	//测试的button
//	private Button button;
//	void Awake()
//	{
//
//		#if UNITY_IOS
//		gameId = "3577540";
//		#else
//		//初始化游戏ID，官网添加游戏时的广告ID
//		gameId = "3577541";
//		#endif
//
//
////		//获取button组件
////		button = GetComponent<Button>();
//	}
////	void OnEnable()
////	{
////		//绑定button事件
////		button.onClick.RemoveAllListeners();
////
////		button.onClick.AddListener(OnShowUnityAdsButtonClick);
////	}
//
//	IEnumerator Start()
//	{
//		Debug.LogError ("Ien start");
//		//如果广告平台被支持
//		if (Advertisement.isSupported)
//		{
//			//初始化ID
//			Advertisement.Initialize(gameId, enableTestMode);
//		}
//		//等待广告初始化
//		//如果没有完成初始化或者没有准备好
//		while (!Advertisement.isInitialized || !Advertisement.IsReady())
//		{
//			//等待0.5s
//			yield return new WaitForSeconds(0.5f);
//			Debug.LogError ("Ien while");
//		}
//		Debug.LogError ("Ien end");
//		var ads = new UnityAdsListener ();
//		Advertisement.AddListener(ads);
//	}
////	//button回调，显示广告
////	void OnShowUnityAdsButtonClick()
////	{
////		//显示默认的广告
////		Advertisement.Show();
////	}
//
//
//	public class UnityAdsListener : IUnityAdsListener{
//		#region IUnityAdsListener implementation
//
//		public void OnUnityAdsReady (string placementId)
//		{
//			UnityEngine.Debug.LogError ("ads ready");
//		}
//
//		public void OnUnityAdsDidError (string message)
//		{
//			UnityEngine.Debug.LogError ("ads error:" + message);
//		}
//
//		public void OnUnityAdsDidStart (string placementId)
//		{
//			UnityEngine.Debug.LogError ("ads start:" + placementId);
//		}
//
//		public void OnUnityAdsDidFinish (string placementId, ShowResult showResult)
//		{
//			UnityEngine.Debug.LogError ("ads finish:" + placementId+" showResult:"+showResult);
//			if (showResult == ShowResult.Finished) {
//				Debug.LogError ("看完广告：");
//				SendPacket sendpacket = new SendPacket ();
//				sendpacket.Send (Com.Communication.OpDefine.CSFinishAds, new Com.Communication.CSFinishAds (), Grow.GrowFun.GET_GUID, OnReward);
//			} else if (showResult == ShowResult.Skipped) {
//				Debug.LogError ("跳过广告：");
//				SendPacket sendpacket = new SendPacket ();
//				sendpacket.Send (Com.Communication.OpDefine.CSFinishAds, new Com.Communication.CSFinishAds (), Grow.GrowFun.GET_GUID, OnReward);
//			}
//		}
//
//
//		void OnReward ()
//		{
//			//GG
//		}
//		#endregion
//
//
//	}
}