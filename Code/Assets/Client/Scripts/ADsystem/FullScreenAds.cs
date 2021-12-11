// /*
//                #########
//               ############
//               #############
//              ##  ###########
//             ###  ###### #####
//             ### #######   ####
//            ###  ########## ####
//           ####  ########### ####
//          ####   ###########  #####
//         #####   ### ########   #####
//        #####   ###   ########   ######
//       ######   ###  ###########   ######
//      ######   #### ##############  ######
//     #######  #####################  ######
//     #######  ######################  ######
//    #######  ###### #################  ######
//    #######  ###### ###### #########   ######
//    #######    ##  ######   ######     ######
//    #######        ######    #####     #####
//     ######        #####     #####     ####
//      #####        ####      #####     ###
//       #####       ###        ###      #
//         ###       ###        ###
//          ##       ###        ###
// __________#_______####_______####______________
//
//                 我们的未来没有BUG
// * ==============================================================================
// * Filename:FullScreenAds.cs
// * Created:2020/4/28
// * Author:  lucy.yijian
// * Alert:
// * 代码千万行
// * 注释第一行
// * 命名不规范
// * 同事两行泪
// * Purpose:
// * ==============================================================================
// */
//
using System;
using ByteDance.Union;
using UnityEngine;
//using NetWork.Layer;
//using Com.Communication;

public class FullScreenAds
{



	/// <summary>
	/// Full screen video ad listener.
	/// </summary>
	public sealed class FullScreenVideoAdListener : IFullScreenVideoAdListener
	{
		private ADTouTiao example;

		public FullScreenVideoAdListener(ADTouTiao example)
		{
			this.example = example;
		}

		public void OnError(int code, string message)
		{
			Debug.LogError("OnFullScreenError: " + message+"  code:"+code);
			//			this.example.information.text = "OnFullScreenError: " + message;
		}

		public void OnFullScreenVideoAdLoad(FullScreenVideoAd ad)
		{
			Debug.Log("OnFullScreenAdLoad");
			//			this.example.information.text = "OnFullScreenAdLoad";

			ad.SetFullScreenVideoAdInteractionListener(
				new FullScreenAdInteractionListener(this.example));
			ad.SetDownloadListener(
				new ADTouTiao.AppDownloadListener(this.example));

//			this.example.fullScreenVideoAd = ad;
			this.example.OnRequestFull(ad);
		}

		// iOS
		public void OnExpressFullScreenVideoAdLoad(ExpressFullScreenVideoAd ad)
		{
			// rewrite
		}

		public void OnFullScreenVideoCached()
		{
			Debug.Log("OnFullScreenVideoCached");
//			this.example.information.text = "OnFullScreenVideoCached";
		}
	}

	/// <summary>
	/// Full screen ad interaction listener.
	/// </summary>
	public sealed class FullScreenAdInteractionListener : IFullScreenVideoAdInteractionListener
	{
		private ADTouTiao example;

		public FullScreenAdInteractionListener(ADTouTiao example)
		{
			this.example = example;
		}

		public void OnAdShow()
		{
			Debug.Log("fullScreenVideoAd show");
//			this.example.information.text = "fullScreenVideoAd show";
		}

		public void OnAdVideoBarClick()
		{
			Debug.Log("fullScreenVideoAd bar click");
//			this.example.information.text = "fullScreenVideoAd bar click";
		}

		public void OnAdClose()
		{
			Debug.Log("fullScreenVideoAd close");
//			this.example.information.text = "fullScreenVideoAd close";
		}

		public void OnVideoComplete()
		{
			Debug.Log("fullScreenVideoAd complete");
//			this.example.information.text = "fullScreenVideoAd complete";
//			SendPacket sendpacket = new SendPacket ();
//			var csmsg = new Com.Communication.CSFinishAds ();
//			csmsg.state = 0;
//			sendpacket.Send (Com.Communication.OpDefine.CSFinishAds, csmsg, Grow.GrowFun.GET_GUID, OnReward);
//
//			LocalDataBase.Instance().AddDataNum(DataType.power, 3);
//			BoxManager.Instance.ShowPopupMessage("获得三点体力");
//
			this.example.OnPlayDone();
			this.example.LoadFullScreenVideoAd ();

		}

		public void OnVideoError()
		{
			Debug.Log("fullScreenVideoAd OnVideoError");
//			this.example.information.text = "fullScreenVideoAd OnVideoError";
		}

		public void OnSkippedVideo()
		{
			Debug.Log("fullScreenVideoAd OnSkippedVideo");
//			this.example.information.text = "fullScreenVideoAd skipped";
//			SendPacket sendpacket = new SendPacket ();
//			var csmsg = new Com.Communication.CSFinishAds ();
//			csmsg.state = 1;
//			sendpacket.Send (Com.Communication.OpDefine.CSFinishAds, csmsg, Grow.GrowFun.GET_GUID, OnReward);
//
//			LocalDataBase.Instance().AddDataNum(DataType.power, 3);
//			BoxManager.Instance.ShowPopupMessage("获得三点体力");
			this.example.OnPlayDone();
			this.example.LoadFullScreenVideoAd ();
		}

//		void OnReward (Packet packet)
//		{
//			//GG
//			XZXD.UI.UIBoxManager.Instance.CreatePopMessage("前往驿站领取奖励");
//			var scmsg = (packet.kBody as SCFinishAds);
//			Grow.GrowFun.Ins.growFullPlayer.AddOrUpdateInstPlayerProps (scmsg.updateInstPlayerProps);
//
//
//		}
	}

}