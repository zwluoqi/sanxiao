using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



namespace XZXD
{
	public class NativeCallback : MonoBehaviour 
	{
		public delegate void BuySuccess(int tabid);
		public static BuySuccess m_BuySuccess;
		public static int tabid;

		static NativeCallback _instance;
		public static NativeCallback Instance {
			get{
				return _instance;
			}
		}

		void Awake(){
			_instance = this;
		}

		public  List<string> iosChannelIdList;

		private GameObject quitDlg;

		private void onResume(string msg)
		{
			BoxManager.Instance.ShowPopupMessage("UnityNativeCallback : application resume on " );

		}

		private void onPause(string msg)
		{
			BoxManager.Instance.ShowPopupMessage("UnityNativeCallback : application pause on " );
		}

		private void onTerminate(string msg)
		{
			BoxManager.Instance.ShowPopupMessage("UnityNativeCallback : application terminate on " );
		}
			
		Dictionary<string,object> loginCallBackData;
		public void onLoginSuccessed(string message)
		{
//			UI.UIBoxManager.Instance.CreatePopMessage("登陆成功");
//			Debug.LogError (message);
//			loginCallBackData = (Dictionary<string,object>)fastJSON.JSON.Instance.ToObject(message);
//			if(((string)loginCallBackData["loginState"]) == ("true")){
//
//
//				if((string)loginCallBackData["loginMode"] == "login"){
//
//				}else{
//					
//				}
//
//			}else{
//				Debug.LogError("facebook login failed");
//			}
		}

		public void onLogout ()
		{
			throw new NotImplementedException ();
		}

		public void onbackLogin ()
		{
			throw new NotImplementedException ();
		}




		private void onLoginFailed(string msg)
		{
			BoxManager.Instance.ShowPopupMessage("登陆失败： " + msg);
		}

		private void onLogoutSuccessed(string msg)
		{
			BoxManager.Instance.ShowPopupMessage("UnityNativeCallback : logout successed, msg is " + msg);

			NativeCaller.submitLoginGameRole("4");
		}

		private void onAccountSwitchSuccessed(string msg)
		{
			//TODO
		}

		private void onLogoutFailed(string msg)
		{
			BoxManager.Instance.ShowPopupMessage("UnityNativeCallback : logout failed, msg is " + msg);
		}

		public void onPaySuccessed(string resStr)
		{
			BoxManager.Instance.ShowPopupMessage("购买成功，等待发货!");
//			UI.UIBoxManager.Instance.ClearNetMask ();
//			SDKOrderTick.AddOrder(resStr);
			if (m_BuySuccess != null) {
				m_BuySuccess (tabid);
			}
		}

		
		public void onPayFailed(string msg)
		{
//			UI.UIBoxManager.Instance.ClearNetMask ();
			BoxManager.Instance.ShowPopupMessage("UnityNativeCallback : pay failed, the msg is " + msg);
		}

		private void onPayCanceled(string msg)
		{
//			UI.UIBoxManager.Instance.ClearNetMask ();
			BoxManager.Instance.ShowPopupMessage("UnityNativeCallback : pay canceled, the msg is " + msg);
		}


		public static Dictionary<string, string> productsInfoDic = new Dictionary<string, string>();

		public void onRequestProductsResult(string productsInfo)
		{
			Debug.LogError("UnityNativeCallback : request products result is " + productsInfo);

			productsInfoDic = new Dictionary<string, string>();
			string[] products = productsInfo.Split('#');
			foreach(string product in products)
			{
				if(product.Contains(":"))
				{
					string[] strs = product.Split(':');
					if(strs.Length == 2)
					{
						string productId = strs[0];
						string productPriceInfo = strs[1];
						productsInfoDic.Add(productId, productPriceInfo);
					}
				}
			}
		}

		public void onQuit()
		{
			BoxManager.Instance.ShowPopupMessage("UnityNativeCallback : quit game ");

		}

		public void onCallBack(string jsonStr){
			Debug.Log("jsonStr:"+jsonStr);
		}
	}
}
	
