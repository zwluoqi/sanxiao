//using UnityEngine;
//using System;
//using System.Collections;
//using System.Threading;
//using System.Net;
//using System.Net.Sockets;
//using System.Collections.Generic;
//using NetWork.Packets.Factory;
//
//namespace NetWork.Layer
//{
//    public class SocketUtil
//    {
//        private string m_sIPAddr;
//        private int m_nPort;
//		private Socket socket;
//		private NetState netState = NetState.NotConnect;
//        private Thread receive;
//		private Queue<Packet> sendQueue;
//
//		enum SocketBoxType
//        {
//			none,
//			clean,
//			wait,
//			time_out,
//			net_error,
//			need_socket,
//		}
//		private SocketBoxType socketBoxType = SocketBoxType.none;
//		private string netError = "";
//
//        public SocketUtil()
//        {
//            sendQueue = new Queue<Packet>();
//            this.m_lsWorldPackage = new Queue<Packet>();
//        }
//
//#region Connect
//        public void Connect(string sIP, int nPort)
//		{
//            if (string.IsNullOrEmpty(sIP))
//            {
//                this.m_sIPAddr = StaticData.SERVER_IP;
//                this.m_nPort = StaticData.SERVER_PORT;
//            }
//            else
//            {
//                this.m_sIPAddr = sIP;
//                this.m_nPort = nPort;
//            }
//			CreateConnect();
//        }
//        
//        public void ResetConnection()
//        {
//            Disconnect();
//            sendQueue.Clear();
//            this.m_lsWorldPackage.Clear();
//        }
//
//        private void CreateConnect ()
//        {
//            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(this.m_sIPAddr), this.m_nPort);
//			this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//			string socketException = "";
//			// 开始创建SOCKET链接
//			socketBoxType = SocketBoxType.wait;
//
//			IAsyncResult res = socket.BeginConnect(endPoint, (async) => {
//				try{
//					Socket client = (Socket)async.AsyncState;
//					if (client.Connected) {
//						client.EndConnect(async);
//						netState = NetState.Connected;
//					} else {
//						netState = NetState.Disconnect;
//					}
//					StartReceiveThread();
//				} catch (Exception e) {
//					netState = NetState.Exception;
//					socketException = e.ToString();
//					Debug.Log(e);
//				}
//			}, socket);
//
//			Thread m_hConnectThread = new Thread(()=>{
//				bool success = res.AsyncWaitHandle.WaitOne(5000, true);
//				if (success)
//                {
//					if (netState == NetState.Connected)
//                    {
//                        NetLogModul.Log("Success Connect: " + this.m_sIPAddr + ":" + this.m_nPort);	
//						// 创建SOCKET成功，清空WaitBox
//						socketBoxType = SocketBoxType.clean;
//					}
//                    else if (netState == NetState.Disconnect)
//                    {
//						// 创建SOCKET失败，请检查网络是否连接
//						socketBoxType = SocketBoxType.net_error;
//						netError = "Check Your Net!";
//					}
//                    else if (netState == NetState.Exception)
//                    {
//						// 创建SOCKET出现异常
//						socketBoxType = SocketBoxType.net_error;
//						netError = socketException;
//					}
//                    else
//                    {
//						if (this.socket.Connected)
//                        {
//                            NetLogModul.Log("Success Connect: " + this.m_sIPAddr + ":" + this.m_nPort);	
//							// 创建SOCKET成功，清空WaitBox
//							socketBoxType = SocketBoxType.clean;
//						} else {
//							// 创建SOCKET失败，请检查网络是否连接
//							socketBoxType = SocketBoxType.net_error;
//							netError = "Check Your Net!";
//						}
//					}
//				}
//                else
//                {
//					netState = NetState.TimeOut;
//					// 创建Socket超时
//					socketBoxType = SocketBoxType.time_out;
//				}
//			});
//			m_hConnectThread.IsBackground = true;
//			m_hConnectThread.Start();
//			this.m_lsWorldPackage.Clear();
//			aTempByte = null;
//        }
//#endregion
//
//#region Tick & Receive
//		private void StartReceiveThread ()
//        {
//			receive = new Thread(new ThreadStart(ReceiveTick));
//			receive.IsBackground = true;
//			receive.Start();
//		}
//
//		private void ReceiveTick ()
//        {
//            while (netState != NetState.Disconnect)
//            {
//				if (socket.Connected && IsCanReceive())
//                {
//					try {
//						byte[] bytes = new byte[4096];
//						int length = this.socket.Receive(bytes);
//						if (length > 0)
//						{
//                            //Debug.Log("Receive length is " + length);
//							SplitPackage(bytes, length);
//						}
//					} catch (Exception e) {
//						if (!(e is ThreadAbortException)) {
//							NetLogModul.LogError("ReceiveSocket Exception: " + e.ToString() + ". Thread Id = " + Thread.CurrentThread.ManagedThreadId);
//						}
//					}
//				}
//                Thread.Sleep(50);
//			}
//		}
//		
//		public void Tick ()
//        {
//			switch(socketBoxType)
//            {
//			case SocketBoxType.clean:
//				API.NetAPI.CleanNetBox();
//				socketBoxType = SocketBoxType.none;
//				break;
//			case SocketBoxType.need_socket:
//				API.NetAPI.ShowNeedSocketConnect();
//				socketBoxType = SocketBoxType.none;
//				break;
//			case SocketBoxType.net_error:
//				API.NetAPI.ShowNetErrorBox(netError);
//				socketBoxType = SocketBoxType.none;
//				break;
//			case SocketBoxType.time_out:
//				//API.NetAPI.ShowNetTimeOutBox(); // 不显示弹窗
//				socketBoxType = SocketBoxType.none;
//                Reconnect(); // 默默地重连
//				break;
//			case SocketBoxType.wait:
//				//API.NetAPI.ShowNetWaitBox(); // 默默地重连，不显示联网遮罩
//                //BoxManager.CreatePopupTextBox("DEBUG:" + LanguageTools.GetText(1111));
//				socketBoxType = SocketBoxType.none;
//				break;
//			}
//			
//			if (netState != NetState.Connected)
//            {
//				return;
//			}
//			if (socket == null || !socket.Connected)
//			{
//				netState = NetState.Disconnect;
//				// 检测到SOCKET链接断开，提示需要重连
//				API.NetAPI.ShowNeedSocketConnect();
//				return;
//			}
//			if (sendQueue.Count > 0)
//            {
//				SendMessage();
//			}
//		}
//#endregion
//
//#region Split Packet
//		private Queue<Packet> m_lsWorldPackage;
//		private byte[] aTempByte = null;
//
//        /// <summary>
//        /// 将之前的残余数据与新收到的数据进行组装
//        /// </summary>
//        /// <param name="bytes">接收到的数据</param>
//		/// <param name="msgLen">数据长度</param>
//        private void SplitPackage(byte[] bytes, int msgLen)
//        {
//            byte[] aTempMsg;
//            if (aTempByte != null)
//            {
//                aTempMsg = new byte[msgLen + aTempByte.Length];
//                Array.Copy(aTempByte, 0, aTempMsg, 0, aTempByte.Length);
//                Array.Copy(bytes, 0, aTempMsg, aTempByte.Length, msgLen);
//            }
//            else
//            {
//                aTempMsg = new byte[msgLen];
//                Array.Copy(bytes, 0, aTempMsg, 0, msgLen);
//            }
//            while (true)
//            {
//            	/*string _s = "";
//				foreach (byte b in aTempMsg)
//                {
//					_s += (int)b + " ";
//				}
//				Debug.Log(_s);*/
//            
//                //判断剩余待处理数据长度是否满足包头长度
//				if (aTempMsg.Length >= PacketFactory.ReceivePacketHead)
//                {
//					//获取包头记载数据长度
//					int nLength = PacketFactory.GetMsgLength(aTempMsg);
//                    //存在至少一个完整数据包
//                    //此处为完整数据包处理流程
//					if (nLength + PacketFactory.ReceivePacketHead <= aTempMsg.Length)
//                    {
//                        int nOpcode;
//                        object body;
//						bool s = PacketFactory.Bytes2PB(aTempMsg, out nOpcode, out body);
//                        Enqueue(new Packet(nOpcode, body));
//						//NetLogModul.Log(string.Format("<--- ID:{0} Length:{1}", nOpcode, nLength + PacketFactory.ReceivePacketHead));
//                        //重置剩余消息包
//						byte[] _temp = new byte[aTempMsg.Length - PacketFactory.ReceivePacketHead - nLength];
//						Array.Copy(aTempMsg, nLength + PacketFactory.ReceivePacketHead, _temp, 0, aTempMsg.Length - PacketFactory.ReceivePacketHead - nLength);
//                        aTempMsg = new byte[_temp.Length];
//                        Array.Copy(_temp, aTempMsg, _temp.Length);
//                    }
//                    else
//                    {
//                        if (aTempMsg.Length > 0)
//                        {
//                            aTempByte = new byte[aTempMsg.Length];
//                            Array.Copy(aTempByte, 0, aTempMsg, 0, aTempMsg.Length);
//                        }
//                        break;
//                    }
//                }
//                else
//                {
//                    if (aTempMsg.Length > 0)
//                    {
//                        aTempByte = new byte[aTempMsg.Length];
//                        Array.Copy(aTempByte, 0, aTempMsg, 0, aTempMsg.Length);
//                    }
//                    break;
//                }
//            }
//        }
//#endregion
//
//        private void Enqueue(Packet item)
//        {
//			lock (this.m_lsWorldPackage)
//            {
//                this.m_lsWorldPackage.Enqueue(item);
//            }
//        }
//        public Packet Dequeue()
//        {
//			lock (this.m_lsWorldPackage)
//            {
//                return this.m_lsWorldPackage.Dequeue();
//            }
//        }
//        public int GetPacketCount()
//        {
//			lock (this.m_lsWorldPackage)
//            {
//                return this.m_lsWorldPackage.Count;
//            }
//        }
//
//#region Send
//        //向服务端发送数据包
//        public void SendMessage(int nOpCode, object obj, NetWork.Layer.Define.ServerType type)
//        {
//			lock (sendQueue)
//            {
//				sendQueue.Enqueue(new Packet(nOpCode, obj, type));
//        	}
//        }
//        
//        private void SendMessage ()
//        {
//			if (IsCanSend())
//            {
//				try
//				{
//					while (sendQueue.Count > 0)
//                    {
//						Packet pkg = sendQueue.Dequeue();
//						byte[] newByte = {};
//						bool s = PacketFactory.PB2Bytes(pkg.nOpCode, pkg.kBody, pkg.socketServerType, out newByte);
//						
//                        /*string _s = "";
//    					foreach (var a in newByte)
//                        {
//							_s += a;
//						}
//						Debug.Log("Send:"+_s);*/
//
//						socket.BeginSend(newByte, 0, newByte.Length, SocketFlags.None, (asy) =>
//                        {
//							try
//							{
//								Socket handler = (Socket)asy.AsyncState;
//								int bytesSent = handler.EndSend(asy);
//								//NetLogModul.Log(string.Format("---> ID:{0} Length:{1}", pkg.nOpCode, bytesSent));
//							}
//                            catch (Exception e)
//                            {
//								NetLogModul.LogError("Send Message Failed: " + e.ToString());
//							}
//						}, socket);
//					}
//				}
//				catch (Exception e)
//				{
//					NetLogModul.LogError("Send Message Failed: " + e);
//				}
//			}
//        }
//        
//		// 断开链接
//		public void Disconnect()
//		{
//			try
//			{
//				netState = NetState.NotConnect;
//				if (socket == null) 
//					return;
//				
//				receive.Abort();
//				receive = null;
//				socket.Shutdown(SocketShutdown.Both);
//				socket.Close(1);
//				socket = null;
//			}
//			catch (Exception ex)
//			{
//				NetLogModul.LogError("SocketObject Closed Exception: " + ex.Message);
//			}
//		}
//		
//		// 是否链接成功
//		public bool IsConnectSuccess()
//		{
//			if (netState == NetState.Connected)
//				return true;
//			return false;
//		}
//		
//		// 重新链接
//		public void Reconnect()
//		{
//			Disconnect();
////			
////			mReconnectCount++;
////			if (mReconnectCount > NetworkManager.Instance.mReconnectLimit)
////			{
////				mReconnectCount = 0;
////				if (null != NetworkManager.onDisconnetedAction)
////					NetworkManager.onDisconnetedAction();
////			}
////			else
////			{
////				LogManager.Log("Reconnect To Server. Repeat time = " + mReconnectCount, LogType.Normal);
//            if (string.IsNullOrEmpty(this.m_sIPAddr))
//            {
//                this.m_sIPAddr = StaticData.SERVER_IP;
//                this.m_nPort = StaticData.SERVER_PORT;
//            }
//			CreateConnect();
////			}
//		}
//		
//		public bool IsCanReceive()
//		{
//			bool ret = ((this.socket != null) && this.socket.Poll(0, SelectMode.SelectRead));
//            return ret;
//		}
//		
//		public bool IsCanSend()
//		{
//			return ((this.socket != null) && this.socket.Poll(0, SelectMode.SelectWrite));
//		}
//#endregion
//    }
//    public enum NetState
//    {
//        // 未链接网络
//        NotConnect,
//        // 链接上
//        Connected,
//        // 断开链接
//        Disconnect,
//        // 网络不稳定
//        Unsteadiness,
//        // 网络异常
//        Exception,
//        // 链接超时
//        TimeOut,
//    }
//}