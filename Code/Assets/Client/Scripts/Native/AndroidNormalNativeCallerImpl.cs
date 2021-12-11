using System;
using System.Collections.Generic;
using UnityEngine;


namespace XZXD
{
	public class AndroidNormalNativeCallerImpl:NativeCallInterface
	{
		public  void loadDeviceInfo()
		{
			UnityEngine.Debug.Log ("EmptyNativeCallerImpl");

		}

		public  void sendDataToTalkingData(string methodName, string[] data)
		{

		}

		public  void logout()
		{
			NativeCallback.Instance.onLogout();
		}

		public  void accountSwitch()
		{
			NativeCallback.Instance.onbackLogin ();
		}

		public  void quit()
		{
			NativeCallback.Instance.onQuit ();
		}


		public  void requestProducts(){
			
		}


		public  void initSdk(String gateIp)
		{

		}



		public  void showToolBar()
		{

		}

		public  void hideToolBar()
		{

		}

		public  void submitLoginGameRole(string[] roleData)
		{

		}

		public  void updateGame(string uri)
		{

		}

		public void sdkLogin (string loginData)
		{
			
		}

		public void RegisterPush ()
		{
			
		}

		public void sdkPay (Dictionary<string, string> payInfo, string pluginId)
		{
			BoxManager.Instance.ShowPopupMessage ("暂不支持");
		}

		public void submitLoginGameRole (string dataType)
		{
			
		}

		public void sendDataToTalkingDataCpa (string methodName, string[] data)
		{
			
		}

		public string getChannelId ()
		{
			return "sanxiao_android_normal";
		}
	}
}

