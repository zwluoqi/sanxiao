using System;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine;

namespace NetWork.Layer
{
	public class UnityWebRequestUtil:IHTTPUtil
	{
		private HTTPManager _http;
		public string url;
		private bool m_bNeedReceive = false;

		public UnityWebRequest request;

		public void Init (string url, HTTPManager hTTPManager)
		{
			this.url = url;
			_http = hTTPManager;
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
					_http.SessionCompleted (true, retRequest.downloadHandler.data);
				} else {
					UnityEngine.Debug.LogError ("UpdataCompleted error: " + retRequest.responseCode);
					_http.SessionCompleted (false, null);
				}
			}
		}
	}
}

