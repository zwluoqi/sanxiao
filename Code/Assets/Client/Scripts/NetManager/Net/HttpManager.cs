using System;
using System.Collections.Generic;
using System.Text;
using Com.Communication;
using System.IO;

//using PBMessage;
//using XZXD.UI;

namespace NetWork.Layer
{
	public class HTTPManager
	{
		private Queue<Packet> sendDatas = new Queue<Packet> ();
		private Queue<Packet> receivedDatas = new Queue<Packet> ();
		private Queue<HttpHandler> handlers = new Queue<HttpHandler> ();
		private Queue<NET_RESULT_TYPE> results = new Queue<NET_RESULT_TYPE> ();

		private IHTTPUtil httpUtil = null;

		public bool sending { private set; get; }

		public int GetResultCount ()
		{
			return results.Count;
		}

		private Packet m_kLastSendPacket = null;
		private byte[] m_arrLastSendPacket = null;
		private HttpHandler m_dLastSendHandler = null;


		public HTTPManager (bool thread)
		{
			if (thread) {
				httpUtil = new HttpUtil ();
			} else {
				httpUtil = new UnityWebRequestUtil ();
			}
		}


		public void SetServerUrl (string url, string relativeUrl)
		{
			httpUtil.Init (url, this);
			sending = false;
		}


		bool Send (Packet pbwh, HttpHandler successHandler)
		{
			if (sending || GetResultCount () > 0) {
				UnityEngine.Debug.LogError ("时机不对 sending:" + (OpDefineEnum)pbwh.nOpCode);
				return false;
			}
			byte[] msg;
			if (PacketBundle.ToMsg (pbwh.nOpCode, pbwh.kBody, out msg) && httpUtil.Send (msg)) {
				sending = true;
				sendDatas.Enqueue (pbwh);
				handlers.Enqueue (successHandler);
				m_kLastSendPacket = pbwh;
				m_dLastSendHandler = successHandler;
				int nLength = msg.Length;
				m_arrLastSendPacket = new byte[nLength];
				Buffer.BlockCopy (msg, 0, m_arrLastSendPacket, 0, nLength);
				#if UNITY_EDITOR
//				if(XZXDDebug.m_showLog){
//					XZXDDebug.Log ("[PACKET] Tomsg :" + (OpDefineEnum)pbwh.nOpCode);
					UnityEngine.Debug.LogWarning ("[PACKET] Tomsg :" + (OpDefineEnum)pbwh.nOpCode + " content:" + Newtonsoft.Json.JsonConvert.SerializeObject (pbwh.kBody));
//				}
				#endif
				return true;
			} else {
				return false;
			}
		}

		public bool Send (int id, Object pbData, bool background, HttpHandler successHandler)
		{
			var packet = new Packet (id, pbData);
			packet.background = background;
			return Send (packet, successHandler);
		}

		public void SessionCompleted (bool success, byte[] data = null)
		{
			sending = false;
			//此函数调用，表示异步收到消息，此时应去除mask
			if (!success) {
				Enqueue<NET_RESULT_TYPE> (results, NET_RESULT_TYPE.NET_ERROR_TIMEOUT);
				UnityEngine.Debug.LogError ("Msg Error: 网络底层错误");
				//此处应仅调用于网络层异步回调
				//应弹出网络错误弹框，并设置需重新发送
			} else {
				try {
					int id;
					object pbData;
					if (PacketBundle.ToObject (data, out id, out pbData)) {
						#if UNITY_EDITOR
//						if(XZXDDebug.m_showLog){
//							XZXDDebug.Log ("Receive Packet is " + ((Com.Communication.OpDefineEnum)id).ToString ());
						UnityEngine.Debug.LogWarning ("Receive Packet is " + ((Com.Communication.OpDefineEnum)id).ToString () + " content:" + Newtonsoft.Json.JsonConvert.SerializeObject (pbData));
//						}
						#endif
						Enqueue<Packet> (receivedDatas, new Packet (id, pbData));
						Enqueue<NET_RESULT_TYPE> (results, NET_RESULT_TYPE.NET_SUCCESS);
					} else {
						Enqueue<NET_RESULT_TYPE> (results, NET_RESULT_TYPE.NET_ERROR_MSGFORMAT);
					}
				} catch (Exception e) {
					Enqueue<NET_RESULT_TYPE> (results, NET_RESULT_TYPE.NET_ERROR_TRANSLATE);
					UnityEngine.Debug.LogError ("Msg Error: 数据转换错误 " + e);
				}
			}
            
		}

		public void Tick ()
		{
			while (results.Count > 0) {


				NET_RESULT_TYPE eType = Dequeue<NET_RESULT_TYPE> (results);

				Packet pbs = Dequeue<Packet> (sendDatas);

				if (this == NetManager.Instance.httpManager && !pbs.background) {
					UnityEngine.Debug.LogWarning ("BoxManager.ClearNetMask()");
					BoxManager.Instance.ClearNetMask ();
				}

				HttpHandler handler = Dequeue<HttpHandler> (handlers);
				if (eType == NET_RESULT_TYPE.NET_SUCCESS) {
					Packet pbw = Dequeue<Packet> (receivedDatas);
					if (pbw != null) {
						if (pbw.nOpCode == OpDefine.ErrorMessage) {//error opcode
							ErrorMessage msg = (ErrorMessage)pbw.kBody;
							UnityEngine.Debug.LogError (msg.code + ":" + msg.msg);
							if (netErrorHandle != null) {
								netErrorHandle (msg.code, msg.msg);
							}
							handler (pbw, false);
						} else {
							handler (pbw, true);
						}
					}
				} else {
					HttpError (pbs, handler, eType);
				}

			}
		}

		public Action<string,string> netErrorHandle;
        
		public void CleanCatch ()
		{
			sendDatas.Clear ();
			receivedDatas.Clear ();
			results.Clear ();
			handlers.Clear ();
		}

		private void Enqueue<T> (Queue<T>queue, T element)
		{
			lock (queue) {
				queue.Enqueue (element);
			}
		}

		private T Dequeue<T> (Queue<T> queue)
		{
			lock (queue) {
				return queue.Dequeue ();
			}
		}

		public void HttpError (Packet pbwh, HttpHandler retryHandler, NET_RESULT_TYPE eErrorType)
		{ 
			//可以写代码由用户控制是否重新发送
			m_kLastSendPacket = pbwh;
			m_dLastSendHandler = retryHandler;
			if (eErrorType == NET_RESULT_TYPE.NET_ERROR_TIMEOUT) {
				UnityEngine.Debug.LogError ("Mask time over");
//				if (!pbwh.background) {
//					//交互式使用选择重发
//					UIBoxManager.Instance.CreatOneButtonBox ("确定", "连接超时,请检查网络环境",null);
//				}
				retryHandler (pbwh, false);
			} else if (eErrorType == NET_RESULT_TYPE.NET_ERROR_MSGFORMAT) {
				UnityEngine.Debug.LogError ("Message Format Error: 底层传输协议格式异常");
				retryHandler (pbwh, false);
			} else if (eErrorType == NET_RESULT_TYPE.NET_ERROR_TRANSLATE) {
				UnityEngine.Debug.LogError ("Message Translate Error: 服务器通信协议异常！请检查通信协议是否为最新");
				retryHandler (pbwh, false);
			}
		}

		public enum NET_RESULT_TYPE
		{
			NET_SUCCESS,
			NET_ERROR_TIMEOUT,
			NET_ERROR_MSGFORMAT,
			NET_ERROR_TRANSLATE,
		}
	}
	
	public delegate void HttpHandler (Packet receiveData,bool bSuccess);
}