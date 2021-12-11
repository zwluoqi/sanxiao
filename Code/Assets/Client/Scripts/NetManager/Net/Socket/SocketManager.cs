//using UnityEngine;
//using System.Collections;
//
//namespace NetWork.Layer
//{
//	public class SocketManager {
//
//        private SocketUtil socketUtil;
//		private EventManger eventManager;
//
//        private static SocketManager instance = null;
//		private bool firstConnect;
//
//        public static SocketManager Instance { get { return instance; } }
//
//        public static SocketManager Init(string sIP, int nPort)
//        {
//            if (instance != null)
//            {
//                return instance;
//            }
//            else
//            {
//                instance = new SocketManager();
//                instance.Connect(sIP, nPort);
//                return instance;
//            }
//            
//        }
//
//        public SocketManager()
//        {
//            socketUtil = new SocketUtil();
//            eventManager = new EventManger();
//        }
//
//        public EventManger GetEventManager()
//        {
//            return eventManager;
//        }
//		
//		public void RegisterMsgListener(int nMsgId, bool bBlockScene, DelegateFunc Handler, int sceneId)
//        {
//			eventManager.RegisterMsgListener(nMsgId, bBlockScene, Handler, sceneId);
//		}
//		
//		public void Connect (string sServerIP, int nHost)
//        {
//            socketUtil.Connect(sServerIP, nHost);
//		}
//		
//		public void Send(int nOpCode, object data, NetWork.Layer.Define.ServerType type)
//        {
//            socketUtil.SendMessage(nOpCode, data, type);
//		}
//
//        public void Disconnect()
//        {
//            if (instance != null && instance.socketUtil != null)
//            {
//                instance.socketUtil.Disconnect();
//            }
//            instance = null;
//        }
//
//		public void ReConnect()
//        {
//            socketUtil.Reconnect();
//		}
//		
//		public void Tick ()
//        {
//            socketUtil.Tick();
//            while (socketUtil.GetPacketCount() > 0)
//            {
//                eventManager.AddPacket(socketUtil.Dequeue());
//			}
//			eventManager.Tick();
//		}
//	}
//}