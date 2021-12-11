using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;

namespace NetWork.Layer
{
	public class Packet
    {
        public int nOpCode;
        public object kBody;
		public bool background;

       
        public Packet()
        {
            nOpCode = 0;
            kBody = null;
        }

        public Packet(int code, object buff)
        {
            nOpCode		= code;
            kBody		= buff;
        }
        
//		public Define.ServerType socketServerType;
//		public Packet(int code, object buff, Define.ServerType type)
//		{
//			nOpCode		= code;
//			kBody		= buff;
//			socketServerType = type;
//		}
    }

    // 解决
//    public class ExtraPacket : Packet
//    {
//        public Packet kExtraPacket;
//
//        public void DoPacket(Packet packet)
//        {
//            nOpCode = packet.nOpCode;
//            kBody = packet.kBody;
//            socketServerType = packet.socketServerType;
//        }
//
//        public void DoExtraPacket(Packet extraPacket)
//        {
//            kExtraPacket = extraPacket;
//        }
//    }
}

