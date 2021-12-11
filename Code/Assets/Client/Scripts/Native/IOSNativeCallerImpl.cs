
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;


namespace XZXD
{
	#if UNITY_IOS
	public class IOSNativeCallerImpl
	{
		[DllImport ("__Internal")]
		public static extern void loadDeviceInfo();

		[DllImport ("__Internal")]
		public static extern void initSdk(String gateIp);

		[DllImport ("__Internal")]
		public static extern void login(string loginData);

		[DllImport ("__Internal")]
		public static extern void logout();

		[DllImport ("__Internal")]
		public static extern void accountSwitch();

		[DllImport ("__Internal")]
		public static extern void pay(string[] payInfo, int dataCount, string pluginId);

		[DllImport ("__Internal")]
		public static extern void quit();

		[DllImport ("__Internal")]
		public static extern void requestProducts();

		[DllImport ("__Internal")]
		public static extern void showToolBar();

		[DllImport ("__Internal")]
		public static extern void hideToolBar();

		[DllImport ("__Internal")]
		public static extern void submitLoginGameRole(string[] roleData, int dataCount);

		[DllImport ("__Internal")]
		public static extern void sendDataToTalkingData(string methodName, string[] data, int dataCount);

		[DllImport ("__Internal")]
		public static extern void sendDataToTalkingDataCpa(string methodName, string[] data, int dataCount);

		[DllImport ("__Internal")]
		public static extern void updateGame(string uri);
	}
	#endif
}

