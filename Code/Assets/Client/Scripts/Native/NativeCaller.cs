using System;
using System.Collections.Generic;
using UnityEngine;


namespace XZXD
{
	public class NativeCaller
	{
		public static NativeCallInterface nativeCallInterface;

		public static void loadDeviceInfo()
		{
			
			switch (SystemConfig.Instance.platformId) {
			case PlatformId.Editor:
				nativeCallInterface = new EditorNativeCallerImpl ();
				break;
			case PlatformId.Appstore:
				nativeCallInterface = new iPhoneNativeCallerImpl ();
				break;
			case PlatformId.GooglePlay:
				nativeCallInterface = new AndroidNativeCallerImpl ();
				break;
			case PlatformId.AndroidNormal:
				nativeCallInterface = new AndroidNormalNativeCallerImpl ();
				break;
			}
			try{
				nativeCallInterface.loadDeviceInfo ();
			}catch(Exception e){
				Debug.LogError (e.Message);
			}
		}

		public static void initSdk(String gateIp)
		{
			nativeCallInterface.initSdk (gateIp);
		}

		public static void sdkLogin(string loginData)
		{
			nativeCallInterface.sdkLogin (loginData);
		}

		public static void RegisterPush ()
		{
			nativeCallInterface.RegisterPush ();
		}

		static System.Collections.IEnumerator _HideNetMask ()
		{
			yield return new WaitForSeconds (5);
//			XZXD.UI.UIBoxManager.Instance.ClearNetMask ();
		}

		public static void sdkPay(Dictionary<string, string> payInfo, string pluginId)
		{
//			XZXD.UI.UIBoxManager.Instance.CreateNetMask ();
//			RunCoroutine.Run (_HideNetMask ());
			nativeCallInterface.sdkPay (payInfo, pluginId);
		}

		public static void logout()
		{
			nativeCallInterface.logout ();
		}

		public static void accountSwitch()
		{
			nativeCallInterface.accountSwitch ();
		}

		public static void quit()
		{
			nativeCallInterface.quit ();
		}

		public static void requestProducts()
		{
			try{	
			nativeCallInterface.requestProducts ();
		}catch(Exception e){
			Debug.LogError (e.Message);
		}
		}

		public static void showToolBar()
		{
			nativeCallInterface.showToolBar ();
		}

		public static void hideToolBar()
		{
			nativeCallInterface.hideToolBar ();
		}

		public static void submitLoginGameRole(string dataType)
		{
			nativeCallInterface.submitLoginGameRole (dataType);
		}

		public static void sendDataToTalkingData(string methodName, string[] data)
		{
			nativeCallInterface.sendDataToTalkingData (methodName,data);
		}

		public static void sendDataToTalkingDataCpa(string methodName, string[] data)
		{

			nativeCallInterface.sendDataToTalkingDataCpa (methodName,data);
		}

		public static void updateGame(string uri)
		{
			UnityEngine.Application.OpenURL(uri);
			nativeCallInterface.updateGame (uri);
		}


		public static string getChannelId ()
		{
			return nativeCallInterface.getChannelId ();
		}

		public static string GetDeviceId ()
		{
			return "sanxiao_"+SystemInfo.deviceUniqueIdentifier;
		}
	}
}

