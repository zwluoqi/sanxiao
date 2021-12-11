using UnityEngine;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;
using Com.Communication;
using System.IO;

namespace NetWork.Layer
{
    public enum SocketState : int
    {
        NONE,
        CREATE,
        CONNECTING,
        WORKING,
        CLOSE,
        ERROR,
    }
    public class SocketTool
    {
        private string name;
        public String Name
        {
            get
            {
                return name;
            }
        }

        public bool Connected
        {
            get
            {
                if (socket == null)
                {
                    return false;
                }
                else
                {
                    return socket.Connected;
                }
            }
        }
        private Queue<Packet> receivePackage;
        private Socket socket;
        private SocketState state;
        private byte[] receiveBuffer;
        private string address;
        private int port;
        private int sendTimeout;
        private int receiveTimeout;
        public SocketState State
        {
            get
            {
                return state;
            }
        }


        public SocketTool(string name, string address, int port,int timeout = 15000)
        {
            this.name = name;
            this.address = address;
            this.port = port;
            this.sendTimeout = timeout;
            this.receiveTimeout = timeout;
            receivePackage = new Queue<Packet>();
            state = SocketState.CREATE;
            receiveBuffer = new byte[8092];
            Array.Clear(receiveBuffer, 0, receiveBuffer.Length);
        }
        public void Connect()
        {
            Clear();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.SendTimeout = sendTimeout;
            socket.ReceiveTimeout = receiveTimeout;
            try
            {
                state = SocketState.CONNECTING;
                socket.BeginConnect(address, port, ConnectHandler, socket);
            }
            catch (SocketException e)
            {
                state = SocketState.ERROR;
                Debug.LogError(e);
            }
        }

        private void ConnectHandler(IAsyncResult async)
        {
            try
            {
                Socket s = (Socket)async.AsyncState;
                if (s.Connected)
                {
                    s.EndConnect(async);
                    state = SocketState.WORKING;
                    ReceiveHelper ah = new ReceiveHelper();
                    ah.socket = s;
                    ah.isHeader = true;
                    ah.receivedLength = 0;
                    ah.requiredLength = 4;
                    s.BeginReceive(receiveBuffer, 0, 4, SocketFlags.None, ReceiveHandler, ah);
                }
                else
                {
                    state = SocketState.ERROR;
                    Debug.LogError("Socket connect Error");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Connect error,Exception " + e);
                state = SocketState.ERROR;
            }
        }

        private void ReceiveHandler(IAsyncResult async)
        {
            ReceiveHelper ah = (ReceiveHelper)async.AsyncState;
            Socket s = ah.socket;
            if(!s.Connected)
            {
                return;
            }
            try
            {
                int read = s.EndReceive(async);
                if (read > 0)
                {
                    ah.receivedLength += read;
                    if (ah.receivedLength > ah.requiredLength)
                    {
                        Debug.LogError("something error in socket!");
                        s.Close();
                        state = SocketState.ERROR;
                        return;
                    }
                    else if (ah.receivedLength == ah.requiredLength)//接收完一次合理的数据
                    {
                        if (ah.isHeader)
                        {
                            int length = BitConverter.ToInt32(receiveBuffer, 0);
                            length = IPAddress.NetworkToHostOrder(length) + 4;
                            ah.isHeader = false;
                            ah.receivedLength = 0;
                            ah.requiredLength = length;
                            s.BeginReceive(receiveBuffer, 0, length, SocketFlags.None, ReceiveHandler, ah);
                        }
                        else
                        {
                            int protoId = BitConverter.ToInt32(receiveBuffer, 0);
                            protoId = IPAddress.NetworkToHostOrder(protoId);
                            System.Object msg = PBConvert.toObject(protoId, receiveBuffer, 4, ah.requiredLength - 4);
                            Packet package = new Packet(protoId, msg);
                            ReceiveNewMessage(package);
                            ah.isHeader = true;
                            ah.receivedLength = 0;
                            ah.requiredLength = 4;
                            s.BeginReceive(receiveBuffer, 0, 4, SocketFlags.None, ReceiveHandler, ah);
                        }
                    }
                    else
                    {
                        s.BeginReceive(receiveBuffer, ah.receivedLength, ah.requiredLength - ah.receivedLength, SocketFlags.None, ReceiveHandler, ah);
                    }
                }
                else
                {
                    s.Close();
                    Debug.Log("remote close the socket");
                    state = SocketState.CLOSE;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception happens in socket receive data. Exception " + e);
                s.Close();
                state = SocketState.ERROR;
            }

        }

        public void Clear()
        {
            if (socket != null)
            {
                if (socket.Connected)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                socket = null;
            }
            if (receiveBuffer != null)
            {
                Array.Clear(receiveBuffer, 0, receiveBuffer.Length);
            }
            if (receivePackage != null)
            {
                receivePackage.Clear();
            }
            state = SocketState.CREATE;
        }

        public void Reconnect()
        {
            Connect();
        }



        private void ReceiveNewMessage(Packet message)
        {
            lock (receivePackage)
            {
                receivePackage.Enqueue(message);
            }
        }

        public Packet CheckReceivePacket()
        {
            if (receivePackage == null)
            {
                return null;
            }
            else
            {
                lock (receivePackage)
                {
                    if (receivePackage.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return receivePackage.Dequeue();
                    }
                }
            }
        }

        public bool SendMessage(int msgId,object msg)
        {
            if (state != SocketState.WORKING)
            {
                return false;
            }
            else
            {
                byte[] data;
                using (MemoryStream ms = new MemoryStream())
                {
                    int protoId = msgId;
                    protoId = IPAddress.HostToNetworkOrder(protoId);
                    byte[] head = BitConverter.GetBytes(protoId);
                    ms.Write(head, 0, 4);
                    ms.Write(head, 0, 4);
                    ProtoBuf.Serializer.Serialize(ms, msg);
                    int length = (int)ms.Position - 8;
                    length = IPAddress.HostToNetworkOrder(length);
                    byte[] lb = BitConverter.GetBytes(length);
                    ms.Position = 0;
                    ms.Write(lb, 0, 4);
                    data = ms.ToArray();
                }
                SendHelper sh = new SendHelper();
                sh.socket = socket;
                sh.data = data;
                sh.sentNumber = 0;
                if (socket != null)
                {
                    socket.BeginSend(data, 0, data.Length, SocketFlags.None, SendHandler, sh);
                }
                else
                {
                    Debug.LogError("SocketTool SendMessage, socket is null");
                }
                return true;
            }
        }

        public bool SendMessage(Packet packet)
        {
            return SendMessage(packet.nOpCode, packet.kBody);
        }

        private void SendHandler(IAsyncResult async)
        {
            SendHelper sh = (SendHelper)async.AsyncState;
            if(!sh.socket.Connected)
            {
                return;
            }
            try
            {
                int sendCount = sh.socket.EndSend(async);
                sh.sentNumber += sendCount;
                if(sh.sentNumber < sh.data.Length)
                {
                    socket.BeginSend(sh.data, sh.sentNumber, sh.data.Length - sh.sentNumber, 
                        SocketFlags.None, SendHandler, sh);
                }
            }
            catch(Exception e)
            {
                Debug.LogError("End send error " + e);
                socket.Close();
                state = SocketState.ERROR;
            }
        }

        public void Close()
        {
            Clear();
        }

        internal class SendHelper
        {
            public Socket socket;
            public int sentNumber;
            public byte[] data;
        }

        internal class ReceiveHelper
        {
            public Socket socket;
            public int receivedLength;
            public int requiredLength;
            public bool isHeader;
        }
    }
}