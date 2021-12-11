#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;

namespace XZXD
{
	public class AndroidNativeCallerImpl:NativeCallInterface
	{
		private string JAVA_CLASS_NAME = "com.totem9.KingAndDungeons.PluginManager";

		public  void loadDeviceInfo()
		{
#if UNITY_ANDROID
			using(AndroidJavaClass jc = new AndroidJavaClass(JAVA_CLASS_NAME))
			{
				jc.CallStatic("loadDeviceInfo");
			}// using
#endif
		}

		public  void sendDataToTalkingData(string methodName, string[] data)
		{
#if UNITY_ANDROID
			using(AndroidJavaClass jc = new AndroidJavaClass(JAVA_CLASS_NAME))
			{
				jc.CallStatic("sendDataToTalkingData", new object[]{methodName, data});
			}// using
#endif
		}

		public  void initSdk(String gateIp)
		{
#if UNITY_ANDROID
			using(AndroidJavaClass jc = new AndroidJavaClass(JAVA_CLASS_NAME))
			{
				jc.CallStatic("initSdk", new object[]{gateIp});
			}// using
#endif
		}
			

		public  void logout()
		{
#if UNITY_ANDROID
			using(AndroidJavaClass jc = new AndroidJavaClass(JAVA_CLASS_NAME))
			{
				jc.CallStatic("logout");
			}// using
#endif
		}

		public  void accountSwitch()
		{
#if UNITY_ANDROID
			using(AndroidJavaClass jc = new AndroidJavaClass(JAVA_CLASS_NAME))
			{
				jc.CallStatic("accountSwitch");
			}// using
#endif
		}

		public  void quit()
		{
#if UNITY_ANDROID
			using(AndroidJavaClass jc = new AndroidJavaClass(JAVA_CLASS_NAME))
			{
				jc.CallStatic("quit");
			}// using
#endif
		}

		public  void showToolBar()
		{
#if UNITY_ANDROID
			using(AndroidJavaClass jc = new AndroidJavaClass(JAVA_CLASS_NAME))
			{
				jc.CallStatic("showToolBar");
			}// using
#endif
		}

		public  void hideToolBar()
		{
#if UNITY_ANDROID
			using(AndroidJavaClass jc = new AndroidJavaClass(JAVA_CLASS_NAME))
			{
				jc.CallStatic("hideToolBar");
			}// using
#endif
		}

		public  void submitLoginGameRole(string[] roleData)
		{
#if UNITY_ANDROID
			using(AndroidJavaClass jc = new AndroidJavaClass(JAVA_CLASS_NAME))
			{
				jc.CallStatic("submitLoginGameRole", new object[]{roleData});
			}// using
#endif
		}

		public  void updateGame(string uri)
		{
#if UNITY_ANDROID
			using(AndroidJavaClass jc = new AndroidJavaClass(JAVA_CLASS_NAME))
			{
				jc.CallStatic("updateGame", new object[]{uri});
			}// using
#endif
		}

		public  void requestProducts(){
#if UNITY_ANDROID
			using(AndroidJavaClass jc = new AndroidJavaClass(JAVA_CLASS_NAME))
			{
				jc.CallStatic("requestProducts");
			}// using
#endif
		}

		public void sdkLogin (string loginData)
		{
#if UNITY_ANDROID
			using(AndroidJavaClass jc = new AndroidJavaClass(JAVA_CLASS_NAME))
			{
				jc.CallStatic("login",new object[]{loginData});
			}// using
#endif
		}

		public void RegisterPush ()
		{
//			throw new NotImplementedException ();
		}

		public void sdkPay (Dictionary<string, string> payInfo, string pluginId)
		{
			string[] payData = new string[payInfo.Count];
			int count = 0;
			foreach(string key in payInfo.Keys)
			{
				string data = payInfo[key];
				payData[count++] = key + "=" + data;
			}
#if UNITY_ANDROID
			using(AndroidJavaClass jc = new AndroidJavaClass(JAVA_CLASS_NAME))
			{
				jc.CallStatic("pay", new object[]{payData});
			}// using
#endif
		}

		public void submitLoginGameRole (string dataType)
		{

		}

		public void sendDataToTalkingDataCpa (string methodName, string[] data)
		{

		}

		public string getChannelId ()
		{
			return "android";
		}
	}
}
#endif
