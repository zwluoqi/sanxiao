//
//using System;
//using NetWork.Layer;
//using System.Collections;
//using UnityEngine;
//using Com.Communication;
//using XZXD;
//
//
//
//public class SendPacket
//{
//	int nOpCode;
//	object kBody;
//	long guid;
//
//	HTTPManager httpManager;
//
//
//	Action<Packet> onCallBack;
//	public void Send(int _nOpcode,object _kbody,long _guid,Action<Packet> oncall){
//        this.nOpCode = _nOpcode;
//        this.kBody = _kbody;
//        this.guid = _guid;
//		this.onCallBack = oncall;
//
//		string serverURL =VersionTool.zoneUrl+ "entry/";
//		this.httpManager = new HTTPManager (false);
//		this.httpManager.SetServerUrl (serverURL,"");
//		_Tick ();
//	}
//
//	void Send0(){
//		XZXDDebug.Log ("SendPacket:"+(OpDefineEnum)this.nOpCode);
//		sending = httpManager.Send(this.nOpCode, this.kBody,false,OnReceive);
//		successRecieve = false;
//	}
//
//	void OnReceive (Packet receiveData, bool bSuccess)
//	{
//		if (bSuccess) {
//			TimeEventManager.Delete(ref tick);
//			successRecieve = true;
//			XZXDDebug.Log ("SendPacket RecieveSuccess:"+(OpDefineEnum)this.nOpCode);
//			if (onCallBack != null) {
//				onCallBack (receiveData);
//			}
//		}
//		sending = false;
//	}
//
//	TimeEventHandler  tick;
//	private float duration = 0.5f;
//	private bool successRecieve = false;
//	private bool sending = false;
//	void _Tick(){
//
//		httpManager.Tick ();
//		if (successRecieve) {
//			return;
//		}
//		if (!sending) {
//			Send0 ();
//		}
//		duration += duration;
//		duration = Mathf.Clamp (duration, 0, 5);
//		tick = StageMgr.Inst ().timeEventManager.CreateEvent (_Tick, duration);
//	}
//}