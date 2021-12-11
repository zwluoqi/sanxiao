using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Com.Communication;

namespace NetWork.Layer
{
    public class SocketController : MonoBehaviour
    {
        private static Dictionary<string, SocketController> socketDict = new Dictionary<string, SocketController>();
        private bool autoHeartBeat = false;
        private float autoBeatTime = 0;
        private float lastSendTime = 0;
        public static SocketController GetSocketController(string channelName)
        {
            if(socketDict.ContainsKey(channelName))
            {
                return socketDict[channelName];
            }
            else
            {
                return null;
            }
        }

        public static SocketController AddSocketChannel(string channelName)
        {
            if(!socketDict.ContainsKey(channelName))
            {
                GameObject gobj = new GameObject(channelName);
                SocketController sc = gobj.AddComponent<SocketController>();
                return sc;
            }
            else
            {
                return socketDict[channelName];
            }

        }
        public void Awake()
        {
            if(!socketDict.ContainsKey(name))
            {
                socketDict.Add(name, this);
                hasInit = false;
                DontDestroyOnLoad(gameObject);
            }
        }

        private bool hasInit = false;

        public bool HasInit
        {
            get
            {
                return hasInit;
            }
        }

        private SocketTool socketTool;

        public void InitSocket(string address, int port, float autoBeatTime = 0,int timeout = 15000 )
        {
            if(socketTool != null)
            {
                socketTool.Clear();
            }
            this.autoHeartBeat = autoBeatTime != 0;
            this.autoBeatTime = autoBeatTime;
            socketTool = new SocketTool(name, address, port, timeout);
            socketTool.Connect();
            lastSendTime = Time.time;
        }

        public bool IsReady
        {
            get
            {
                return socketTool != null && socketTool.State == SocketState.WORKING;
            }
        }

        public void Reconnect()
        {
            if(socketTool != null)
            {
                socketTool.Reconnect();
                lastSendTime = Time.time;
            }
        }

        public void Close()
        {
            if(socketTool != null)
            {
                socketTool.Close();
                receiveHandlers.Clear();
                //Close()可能被委托调用,不能在此处SocketTool置空,报错！
                //socketTool = null;
                //hasInit = false;
            }
        }

        public SocketState State
        {
            get
            {
                if(socketTool != null)
                {
                    return socketTool.State;
                }
                else
                {
                    return SocketState.NONE;
                }
            }
        }

        public bool Send(Packet packet)
        {
            if(socketTool != null)
            {
                lastSendTime = Time.time;
                return socketTool.SendMessage(packet);
            }
            else
            {
                return false;
            }
        }

        public bool Send(int protoId,object msg)
        {
            if (socketTool != null)
            {
                lastSendTime = Time.time;
                return socketTool.SendMessage(protoId,msg);
            }
            else
            {
                return false;
            }
        }

        public void OnDestroy()
        {
            if(socketDict.ContainsKey(name))
            {
                socketDict.Remove(name);
            }
        }


        public void Update()
        {
            if(socketTool != null)
            {
                Packet packet = socketTool.CheckReceivePacket();
                while(packet != null)
                {
#if UNITY_EDITOR
//                    XZXDDebug.LogWarning("socket nOpCode:" + packet.nOpCode);
#endif
                    if(receiveHandlers.ContainsKey(packet.nOpCode))
                    {
                        receiveHandlers[packet.nOpCode](packet.nOpCode,packet.kBody);
                    }
                    else
                    {
                        DefaultHandler(packet.nOpCode, packet.kBody);
                    }
                    packet = socketTool.CheckReceivePacket();
                }
                if(autoHeartBeat && Time.time - lastSendTime > autoBeatTime && socketTool.State == SocketState.WORKING)
                {
//                    Send(OpDefine.CsHeartBeat, new CsHeartBeat());
                }
            }
        }

        public void DefaultHandler(int protoId,object msg)
        {
            //Debug.Log("Channel " + name + " receive message id: " + protoId + " with no handler");
        }

        private Dictionary<int, ReceivePacketHandler> receiveHandlers = new Dictionary<int, ReceivePacketHandler>();
        public delegate void ReceivePacketHandler(int protoId, object msg);
        public void RegistReceiveHandler(int protoId,ReceivePacketHandler receiveHandler)
        {
            if(receiveHandlers.ContainsKey(protoId))
            {
                receiveHandlers[protoId] += receiveHandler;
            }
            else
            {
                receiveHandlers[protoId] = receiveHandler;
            }
        }

        public void UnregistReceiveHandler(int protoId, ReceivePacketHandler receiveHandler)
        {
            if(receiveHandlers.ContainsKey(protoId))
            {
                receiveHandlers[protoId] -= receiveHandler;
                if(receiveHandlers[protoId] == null)
                {
                    receiveHandlers.Remove(protoId);
                }
            }
        }

        public void OnApplicationQuit()
        {
            foreach(var s in socketDict)
            {
                s.Value.Close();
            }
            socketDict.Clear();
        }
    }
}