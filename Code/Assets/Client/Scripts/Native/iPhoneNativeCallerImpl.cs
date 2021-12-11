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
// * Filename:iPhoneNativeCallerImpl.cs
// * Created:2019/3/5
// * Author:  zhouwei
// * Alert:
// * 代码千万行
// * 注释第一行
// * 命名不规范
// * 同事两行泪
// * Purpose:
// * ==============================================================================
// */

using System;

namespace XZXD
{
	
	public class iPhoneNativeCallerImpl:NativeCallInterface
    {
		#region NativeCallInterface implementation
		public void loadDeviceInfo ()
		{
			UnityEngine.Debug.Log ("iPhoneNativeCallerImpl");
			#if UNITY_IOS
			IOSNativeCallerImpl.loadDeviceInfo();
			#endif
		}
		public void initSdk (string gateIp)
		{
			#if UNITY_IOS
			IOSNativeCallerImpl.initSdk(gateIp);
			#endif
		}
		public void sdkLogin (string loginData)
		{
			#if UNITY_IOS
			IOSNativeCallerImpl.login(loginData);
			#endif
		}
		public void RegisterPush ()
		{
			#if UNITY_IOS
			UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert | UnityEngine.iOS.NotificationType.Badge | UnityEngine.iOS.NotificationType.Sound);
			#endif
		}
		public void sdkPay (System.Collections.Generic.Dictionary<string, string> payInfo, string pluginId)
		{
			string[] payData = new string[payInfo.Count];
			int count = 0;
			foreach(string key in payInfo.Keys)
			{
				string data = payInfo[key];
				payData[count++] = key + "=" + data;
			}
			#if UNITY_IOS
			IOSNativeCallerImpl.pay (payData, payData.Length, pluginId);
			#endif
		}
		public void logout ()
		{
			#if UNITY_IOS
			IOSNativeCallerImpl.logout ();
			#endif
		}
		public void accountSwitch ()
		{
			#if UNITY_IOS
			IOSNativeCallerImpl.accountSwitch ();
			#endif
		}
		public void quit ()
		{
			#if UNITY_IOS
			IOSNativeCallerImpl.quit ();
			#endif
		}
		public void requestProducts ()
		{
			#if UNITY_IOS
			IOSNativeCallerImpl.requestProducts ();
			#endif
		}
		public void showToolBar ()
		{
			#if UNITY_IOS
			IOSNativeCallerImpl.showToolBar ();
			#endif
		}
		public void hideToolBar ()
		{
			#if UNITY_IOS
			IOSNativeCallerImpl.hideToolBar ();
			#endif
		}
		public void submitLoginGameRole (string dataType)
		{
		
		}
		public void sendDataToTalkingData (string methodName, string[] data)
		{
		
		}
		public void sendDataToTalkingDataCpa (string methodName, string[] data)
		{
		
		}
		public void updateGame (string uri)
		{

		}

		public string getChannelId ()
		{
			return "sanxiao_ios";
		}
		#endregion
        
    }

}

