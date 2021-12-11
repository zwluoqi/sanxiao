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
// * Filename:RewardVideoAds.cs
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

    public class RewardVideoAds
    {

	public sealed class RewardVideoAdListener : IRewardVideoAdListener
		{
			private ADTouTiao example;

			public RewardVideoAdListener(ADTouTiao example)
			{
				this.example = example;
			}

			public void OnError(int code, string message)
			{
				Debug.LogError("OnRewardError: " + message+"   code:"+code);
				//			this.example.information.text = "OnRewardError: " + message;
			}

			public void OnRewardVideoAdLoad(RewardVideoAd ad)
			{
				Debug.Log("OnRewardVideoAdLoad");
				//			this.example.information.text = "OnRewardVideoAdLoad";

				ad.SetRewardAdInteractionListener(
					new RewardAdInteractionListener(this.example));
				ad.SetDownloadListener(
				new ADTouTiao.AppDownloadListener(this.example));

				this.example.OnRequest (ad);
			}

			public void OnExpressRewardVideoAdLoad(ExpressRewardVideoAd ad)
			{
			}

			public void OnRewardVideoCached()
			{
				Debug.Log("OnRewardVideoCached");
				//			this.example.information.text = "OnRewardVideoCached";
			}
		}


	public sealed class RewardAdInteractionListener : IRewardAdInteractionListener
		{
			private ADTouTiao example;

			public RewardAdInteractionListener(ADTouTiao example)
			{
				this.example = example;
			}

			public void OnAdShow()
			{
				Debug.Log("rewardVideoAd show");
				//			this.example.information.text = "rewardVideoAd show";
			}

			public void OnAdVideoBarClick()
			{
				Debug.Log("rewardVideoAd bar click");
				//			this.example.information.text = "rewardVideoAd bar click";
			}

			public void OnAdClose()
			{
				Debug.Log("rewardVideoAd close");
				//			this.example.information.text = "rewardVideoAd close";
			}

			public void OnVideoComplete()
			{
				Debug.Log("rewardVideoAd complete");
				//			this.example.information.text = "rewardVideoAd complete";
			}

			public void OnVideoError()
			{
				Debug.LogError("rewardVideoAd error");
				//			this.example.information.text = "rewardVideoAd error";
			}

			public void OnRewardVerify(
				bool rewardVerify, int rewardAmount, string rewardName)
			{
				Debug.Log("verify:" + rewardVerify + " amount:" + rewardAmount +
					" name:" + rewardName);
				//			this.example.information.text =
				//				"verify:" + rewardVerify + " amount:" + rewardAmount +
				//				" name:" + rewardName;
			}
		}

    }


