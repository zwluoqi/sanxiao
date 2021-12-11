using UnityEngine;
using System.Collections;
//using XZXD.UI;
//using XZXD;
//using View;
using System;

public class NetErrorHandle
{
	public static void ErrorMessage (string sErrorCode, string sErrorText)
	{
		if (sErrorCode.Equals ("update_new_dict_version")) {
//			UIBoxManager.Instance.CreatNetError ("确定", sErrorText, SessionOut);
		} 
		else if (sErrorCode.Equals("Info_StopServer")) {
//			UIBoxManager.Instance.CreatNetError ("确定", sErrorText, SessionOut);
		}
        else if (sErrorCode.Equals("jiance_zuobi_exit"))
        {
//            UIBoxManager.Instance.CreatNetError("确定", sErrorText, OnExitApp);
        }
		else if (string.IsNullOrEmpty (sErrorCode)) {
//			UIBoxManager.Instance.CreatNetError ("确定", sErrorText, SessionOut);
		}
		else {
//			UIBoxManager.Instance.CreatNetError ("确定", sErrorText, null);
		}
	}

    private static void OnExitApp(bool bo)
    {
//		Application.OpenURL(VersionTool.zuobiExitUrl);
		Application.Quit ();
    }

    static void SessionOut (bool ok)
	{
//		StageMgr.Inst().ReturnLogin ();
	}
}
