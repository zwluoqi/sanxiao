using UnityEngine;
using System.Collections;
using NetWork.Layer;
//using XZXD.UI;

public class NetManager
{
	private static NetManager instance;

	public static NetManager Instance {
		get {
			if (instance == null) {
				instance = new NetManager ();

			}
			return instance;
		}
	}

	public HTTPManager httpManager;

	public static void Release ()
	{
		instance = null;		
	}

	private bool m_bLastLockScreen;

	public NetManager ()
	{
		httpManager = new HTTPManager (false);
		httpManager.netErrorHandle = OnNetErrorHandle;
	}

	void OnNetErrorHandle (string arg1, string arg2)
	{
		NetErrorHandle.ErrorMessage (arg1, arg2);
	}

	public void InitHttp (string sPath, string relativePath)
	{
		httpManager.SetServerUrl (sPath, relativePath);
	}



	public void NetTick (float fDelta, int nScenesID)
	{
		HttpTick ();
	}


	private void HttpTick ()
	{
		if (httpManager != null) {
			httpManager.Tick ();
		}
	}



	public bool SendHttp (int nOpcode, object kMsg, HttpHandler dShow, bool bLockScreen = true)
	{
        
		bool success = httpManager.Send (nOpcode, kMsg, !bLockScreen, dShow);
		if (success) {
			if (bLockScreen) {
				m_bLastLockScreen = bLockScreen;
				UnityEngine.Debug.LogWarning ("BoxManager.CreateNetMask()");
//				UIBoxManager.Instance.CreateNetMask ();
			}
		}
		return success;
	}

  
	public void CheckErrorPacket (Packet kErrorMsg)
	{
		UnityEngine.Debug.LogError ("Error opcode: " + kErrorMsg.nOpCode);
		//异常处理
	}

	public static void ResetZoneUrl(string url){
//		VersionTool.zoneUrl = url;
		string serverURL = url+ "entry/";
		NetManager.Instance.InitHttp (serverURL,"");
//		SDKOrderTick.SetHttp(serverURL,"");
//		ChatOrderTick.SetHttp (serverURL, "");
	}

}
