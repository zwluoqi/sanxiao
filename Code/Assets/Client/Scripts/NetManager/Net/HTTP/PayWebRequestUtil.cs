// /*
//                #########
//               ############
//               #############
//              ##  ###########
//             ###  ###### #####
//             ### #######   ####
//            ###  ########## ####
//           ####  ########### ####
//          ####   ###########  #####
//         #####   ### ########   #####
//        #####   ###   ########   ######
//       ######   ###  ###########   ######
//      ######   #### ##############  ######
//     #######  #####################  ######
//     #######  ######################  ######
//    #######  ###### #################  ######
//    #######  ###### ###### #########   ######
//    #######    ##  ######   ######     ######
//    #######        ######    #####     #####
//     ######        #####     #####     ####
//      #####        ####      #####     ###
//       #####       ###        ###      #
//         ###       ###        ###
//          ##       ###        ###
// __________#_______####_______####______________
//
//                 我们的未来没有BUG
// * ==============================================================================
// * Filename:PayWebRequestUtil.cs
// * Created:2018/4/26
// * Author:  lucy.yijian
// * Purpose:
// * ==============================================================================
// */
//
using System;
using NetWork.Layer;
using UnityEngine.Networking;
using System.Collections;

public class PayWebRequestUtil
{

	private PayManager _payManager;
	public string url;
	private bool m_bNeedReceive = false;

	public UnityWebRequest request;

	public void Init (string url, PayManager payManager)
	{
		this.url = url;
		this._payManager = payManager;
	}


	public bool Send (byte[] data)
	{
		if (request != null)
			return false;
		RunCoroutine.Run (Post (data));
		return true;
	}

	IEnumerator Post (byte[] postBytes)
	{  
		request = new UnityWebRequest (url, "POST");  

		request.uploadHandler = (UploadHandler)new UploadHandlerRaw (postBytes);  
		request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer ();  

		request.SetRequestHeader ("Content-Type", "application/json");  
		request.SetRequestHeader ("CLEARANCE", "I_AM_ADMIN");  

		m_bNeedReceive = true;
		yield return request.Send ();
		UnityWebRequest retRequest = request;
		request = null;
		UpdataCompleted (retRequest);
	}

	public void UpdataCompleted (UnityWebRequest retRequest)
	{
		if (m_bNeedReceive) {
			m_bNeedReceive = false;
			if (retRequest.responseCode == 200) {  
				_payManager.SessionCompleted (true, retRequest.downloadHandler.data);
			} else {
				UnityEngine.Debug.LogError ("UpdataCompleted error: " + retRequest.responseCode);
				_payManager.SessionCompleted (false, null);
			}
		}
	}
		
}
