using System;
using System.Net;

namespace NetWork.Layer
{
	public class HttpUtil :IHTTPUtil
	{
		private Uri uri = null;
//		private string serverUrl = null;
//		private string relativeUrl = null;
		private WebClient webClient = new ShortWebClient();
		//用以排除延迟导致的重复收包问题，即发一包只能收一包，多余收到的包无效，丢弃
		private bool m_bNeedReceive = false;
		private HTTPManager _http;

		
		public void Init(string serverUrl,HTTPManager kHTTP)
        {
//			ServerUrl = serverUrl;
			uri = new Uri(serverUrl);
			_http = kHTTP;
            webClient.UploadDataCompleted += UpdataCompleted;
        }
		
		public bool Send(byte[] data)
        {
            if(webClient.IsBusy)
            {
                UnityEngine.Debug.LogError("webClient.IsBusy！！！！！！！！ ");
                return false;
            }
            try
            {
				//webClient.Headers.Set();
                webClient.UploadDataAsync(uri, data);
				m_bNeedReceive = true;
                return true;
            }
            catch (WebException we)
            {
                UnityEngine.Debug.LogError("Send: " + we);
                return false;
            }
        }
		
		public void UpdataCompleted(Object sender, UploadDataCompletedEventArgs args)
        {
            if (args.Error == null)
            {
                if (args.Result != null && args.Result.Length > 0)
                {
					if (m_bNeedReceive)
					{
						m_bNeedReceive = false;
						_http.SessionCompleted(true, args.Result);
					}
					return;
                }
            } else {
				_http.SessionCompleted(false, null);
				UnityEngine.Debug.LogError("UpdataCompleted error: "+args.Error);
            }

			return;
        }
		
	}
}