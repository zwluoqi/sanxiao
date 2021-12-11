using System;
using System.Collections.Generic;
using UnityEngine;


namespace XZXD
{
	public class EditorNativeCallerImpl:NativeCallInterface
	{
		public  void loadDeviceInfo()
		{
			UnityEngine.Debug.Log ("EditorNativeCallerImpl");

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

		public void initSdk (string gateIp)
		{
			
		}

		public void sdkLogin (string loginData)
		{
			
		}

		public void RegisterPush ()
		{
			
		}

		public void sdkPay (Dictionary<string, string> payData, string pluginId)
		{
			string jsonRes = "";

			payData["payType"] = "Editor";
			foreach(var kv in payData){
				jsonRes+=kv.Key+"="+kv.Value+"&";
			}
			NativeCallback.Instance.onPaySuccessed(jsonRes);
		}

		public void showToolBar ()
		{
			
		}

		public void hideToolBar ()
		{
			
		}

		public void submitLoginGameRole (string dataType)
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
			return "sanxiao_editor";
		}
	}
}

