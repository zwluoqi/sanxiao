//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//
//using System;
//using Com.Communication;
//using XZXD.UI;
//
//
//
//public class SDKOrderTick {
//	
//	
//	
//	public static NetWork.Layer.HTTPManager httpManager ;
//	
//	private const float fDuration = 15f;
//	private static float[] arrDuration = {
//		0f, 1f, 3f,
//		5f, 5f, 5f, 5f,
//		8f,
//		10f,
//		15f,
//		30f,
//		60f,
//		120f,
//	};
//	private static int nDurationCount = 0;
//	
//	static void AddDurationCount () { nDurationCount += 1; }
//	static void ResetDurationCount () { nDurationCount = 0; }
//	static float CurDuration ()
//	{
//		if (nDurationCount < arrDuration.Length && nDurationCount >= 0)
//		{
//			return arrDuration[nDurationCount];
//		} else {
//			return arrDuration[arrDuration.Length-1];
//		}
//	}
//	
//	public static void OrderTick(float deltaTime){
//		if(netTimeoutTime >30){
//			neting = false;
//		}else{
//			netTimeoutTime+=deltaTime;
//		}
//		
//		
//		if(neting){
//			
//			;//tick
//		}else{
//			if(orderedItems.Count > 0){
//				
//				if(lastSendTime < CurDuration()){
//					lastSendTime += deltaTime;
//				}else{
//					lastSendTime = 0;
//					RequestCheckOrder(orderedItems[orderedItems.Count-1]);
//					if (bNewOrder)
//					{
//						ResetDurationCount();
//						bNewOrder = false;
//					}
//					
//				}
//			}
//		}
//		httpManager.Tick ();
//	}
//	
//	public class OrderItem{
//		/// <summary>
//		/// 服务器产生的唯一ID
//		/// 
//		/// </summary>
//		public string orderID;
//		public string jsonData;
//		public Dictionary<string,string> payInfo;
//	}
//	
//	
//	private static List<OrderItem> orderedItems = new List<OrderItem>();
//	private static List<string> tmpOrderData = new List<string>();
//	private static bool bNewOrder = false;
//	private static float lastSendTime = 0;
//
//	public static string sKey{
//		get{
//			return "server:"+CacheData.GetLoginServerIndex () +":id:"+ CacheData.GetLoginId () +":pass:"+ CacheData.GetLoginPassward ();
//		}
//	}
//
//	public static void SetHttp(string url, string relativeUrl){
//		httpManager = new NetWork.Layer.HTTPManager (false);
//		httpManager.SetServerUrl(url,relativeUrl);
//	}
//
//	public static void InitOrderData(){
//		Debug.LogWarning("InitOrderData");
//		lastSendTime = 0;
//
//		orderedItems.Clear();
//		netTimeoutTime = 0;
//		neting = false;
//		ResetDurationCount();
//		
//
//		
//
//		string sHadOrders = PlayerPrefs.GetString(sKey, "");
//		string[] arrOrders = sHadOrders.Split(new string[]{@"&%&"}, StringSplitOptions.RemoveEmptyEntries);
//		List<string> addOrders = new List<string>();
//		addOrders.AddRange(arrOrders);
//		addOrders.AddRange(tmpOrderData);
//		tmpOrderData.Clear();
//		
//		orderedItems.Clear();
//		string sNewHadOrders = "";
//		foreach (string jsonData in addOrders)
//		{
//			if (string.IsNullOrEmpty(jsonData))
//			{
//				continue;
//			}
//			
//			Dictionary<string,string> orderInfo = getPayInfo(jsonData);
//			string orderId = "";
//			orderInfo.TryGetValue("orderId",out orderId);
//			
//			if(!string.IsNullOrEmpty(orderId)){
//				OrderItem oi = orderedItems.Find(obj=>(obj.orderID == orderId));
//				if(oi == null){
//					oi = new OrderItem();
//					oi.orderID = orderId;
//					oi.jsonData = jsonData;
//					oi.payInfo = orderInfo;
//					
//					orderedItems.Add(oi);
//					sNewHadOrders += (@"&%&" + jsonData);
//				}else{
//					Debug.LogError("order had exist");
//				}
//			}else{
//				Debug.LogError("order format error");
//			}
//		}
//		PlayerPrefs.SetString(sKey, sNewHadOrders);
//		
//	}
//	
//	public static void AddOrder(string jsonRes){		
//		bNewOrder = true;
//		Debug.LogWarning("AddOrder:"+jsonRes);
//
//		
//
//		string sHadOrders = PlayerPrefs.GetString(sKey, "");
//		string[] arrOrders = sHadOrders.Split(new string[]{@"&%&"}, StringSplitOptions.RemoveEmptyEntries);
//		List<string> addOrders = new List<string>();
//		addOrders.AddRange(arrOrders);
//		addOrders.Add(jsonRes);
//		
//		orderedItems.Clear();
//		string sNewHadOrders = "";
//		foreach (string jsonData in addOrders)
//		{
//			if (string.IsNullOrEmpty(jsonData))
//			{
//				continue;
//			}
//			Dictionary<string,string> orderInfo = getPayInfo(jsonData);
//			string orderId = "";
//			orderInfo.TryGetValue("orderId",out orderId);
//			
//			if(!string.IsNullOrEmpty(orderId)){
//				OrderItem oi = orderedItems.Find(obj=>(obj.orderID == orderId));
//				if(oi == null){
//					oi = new OrderItem();
//					oi.orderID = orderId;
//					oi.jsonData = jsonData;
//					oi.payInfo = orderInfo;
//					
//					orderedItems.Add(oi);
//					sNewHadOrders += (@"&%&" + jsonData);
//				}else{
//					Debug.LogError("order had exist");
//				}
//			}else{
//				Debug.LogError("order format error");
//			}
//		}
//		
//		PlayerPrefs.SetString(sKey, sNewHadOrders);
//	}
//	
//	
//	public static float netTimeoutTime = 0;
//	public static bool neting = false;
//	public static void RequestCheckOrder(OrderItem oi){
//		Debug.LogWarning("RequestCheckOrder key:"+oi.orderID);
//		System.Text.StringBuilder sb = new System.Text.StringBuilder();
//		foreach(var data in oi.payInfo){
//			sb.AppendLine("key:"+data.Key+" value:"+data.Value);
//		}
//		Debug.LogWarning(sb.ToString());
//		sb = null;
//
//		neting = true;
//		netTimeoutTime = 0;
//		AddDurationCount();
//
//		CSCheckPay msg = new CSCheckPay ();
//		msg.orderid = long.Parse (oi.orderID);
//		msg.keys.AddRange (oi.payInfo.Keys);
//		msg.vals.AddRange (oi.payInfo.Values);
//
//		if (!httpManager.Send (OpDefine.CSCheckPay, msg, false, GameServerCallBack)) {
//			neting = false;
//		}
//	}
//
//	static void GameServerCallBack (NetWork.Layer.Packet receiveData, bool bSuccess)
//	{
//		neting = false;
//		if (!bSuccess) {
//			return;
//		}
//		var msg = receiveData.kBody as SCCheckPay;
//
//		if(msg.checkState =="server_error"){
//			Debug.LogError("服务器验证未完成:"+msg.checkState);
//			return;
//		}
//		
//		ResetDurationCount();
//		
//		string retOrderStrs = msg.orderid.ToString();
//		string[] orderIDs = retOrderStrs.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
//
//		string sNewHadOrders = "";
//		
//		foreach (string orderID in orderIDs)
//		{
//			OrderItem oi = orderedItems.Find(obj=>(obj.orderID == orderID));
//			if(oi != null){
//				orderedItems.Remove(oi);
//			}
//		}
//		
//		foreach(var st in orderedItems){
//			sNewHadOrders += (@"&%&" + st.jsonData);
//		}
//		
//		PlayerPrefs.SetString(sKey, sNewHadOrders);
//		
//		if (msg.checkState == "success") {
//			Grow.GrowMsgMustSuccessCtrl must = new Grow.GrowMsgMustSuccessCtrl ();
//			RechargeSuccessItem res = new RechargeSuccessItem (msg.rechargeItemId);
//			var updateData = res.GetUpdateData ();
//			must.StartSend ( delegate() {
//				UIBoxManager.Instance.ShowRewardPage(Vector3.zero, updateData.ToDropData());		
//			}, res.OnPaySuccess);
//		}
//	}
//
//	
//	
//	public static Dictionary<string,string> getPayInfo(string resStr){
//		var orderInfo = new Dictionary<string, string>();
//		string[] splitStrArr = resStr.Split('&');
//		foreach(string str in splitStrArr)
//		{
//			if(str.Contains("="))
//			{
//				int firstEquipIndex = str.IndexOf('=');
//				string key = str.Substring(0,firstEquipIndex);
//				string value = str.Substring(firstEquipIndex+1);
//				orderInfo[key] =  value;
//			}
//		}
//		return orderInfo;
//	}
//}
//
