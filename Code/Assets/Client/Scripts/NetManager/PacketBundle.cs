using UnityEngine;
using System;
using System.Collections;
using Com.Communication;

public class PacketBundle
{
	private static int[] sim = {
		OpDefine.CSLogin,
		OpDefine.CSCheckVersion,
	};
	private static int[] spe = {OpDefine.CSBeginGame};

	private static System.Random _random;
	private static System.Random random{
		get{
			if (_random == null) {
				_random = new System.Random (System.DateTime.Now.Millisecond);
				m_s6LastRandomKey = GetKeyRandom();
			}
			return _random;
		}
	}


	/// <summary>
	/// opcode and data to pb-message
	/// </summary>
	public static bool ToMsg (int nOpCode, object data, out byte[] msg)
	{
		byte[] aOpCode = ByteWriteInt(nOpCode);
		byte[] aData = PBConvert.toBytes(nOpCode, data);
		if (aData == null)
		{
			msg = new byte[0];
			return false;
		}
		byte[] aLength = ByteWriteInt(aData.Length);
		if (IsSim(nOpCode))
		{
			// build the net message to byte array
			msg = new byte[8 + aData.Length];
			Buffer.BlockCopy(aLength, 0, msg, 0, 4);
			Buffer.BlockCopy(aOpCode, 0, msg, 4, 4);
			Buffer.BlockCopy(aData, 0, msg, 8, aData.Length);
		}
		else if (IsSpe(nOpCode))
		{
			byte[] aZoneID = ByteWriteInt(m_nServerID);
			// build the net message to byte array
			msg = new byte[12 + aData.Length];
			Buffer.BlockCopy(aLength, 0, msg, 0, 4);
			Buffer.BlockCopy(aOpCode, 0, msg, 4, 4);
			Buffer.BlockCopy(aZoneID, 0, msg, 8, 4);
			Buffer.BlockCopy(aData, 0, msg, 12, aData.Length);
		}
		else 
		{
			byte[] aZoneID = ByteWriteInt(m_nServerID);
			byte[] aPID = ByteWriteLong(m_lPlayerID);
			byte[] aPlayerKey = ByteWriteString(m_sPlayerKey);
			byte[] aRandomKey = ByteWriteString(m_s6RandomKey);
			byte[] aPlayerLoginKey = ByteWriteString(m_sAccountLoginKey);
			// build the net message to byte array
			msg = new byte[58 + aData.Length];
			Buffer.BlockCopy(aLength, 0, msg, 0, 4);
			Buffer.BlockCopy(aOpCode, 0, msg, 4, 4);
			Buffer.BlockCopy(aZoneID, 0, msg, 8, 4);
			Buffer.BlockCopy(aPID, 0, msg, 12, 8);
            if (aPlayerKey.Length > 0)
            {
                Buffer.BlockCopy(aPlayerKey, 0, msg, 20, 16);
            }
			Buffer.BlockCopy(aRandomKey, 0, msg, 36, 6);
            if (aPlayerLoginKey.Length > 0)
            {
                Buffer.BlockCopy(aPlayerLoginKey, 0, msg, 42, 16);
            }
			Buffer.BlockCopy(aData, 0, msg, 58, aData.Length);
		}
		return true;
	}
	
	/// <summary>
	/// PB Message to opcode and PB object
	/// </summary>
	public static bool ToObject (byte[] msg, out int nOpCode, out object obj)
	{
		if (msg.Length < 8)
		{
			nOpCode = -1;
			obj = null;
			return false;
		}
		//message copy
		byte[] aMsg = new byte[msg.Length];
		Buffer.BlockCopy(msg, 0, aMsg, 0, msg.Length);
		//get message length
		int nLength = ByteGetInt(aMsg, 0);
		//get pb opcode
		nOpCode = ByteGetInt(aMsg, 4);
		//get pb data
		byte[] aData = new byte[nLength];
		Buffer.BlockCopy(aMsg, 8, aData, 0, nLength);
		// translate the net message from byte array to an object
		if (nLength <= msg.Length-8)
		{
			obj = PBConvert.toObject(nOpCode, aData);
			return true;
		}
		obj = null;
		return false;
	}

	/// <summary>
	/// 获取数据长度
	/// </summary>
	/// <returns>数据长度.</returns>
	/// <param name="msg">待发送数据.</param>
	public static int GetMsgLength (byte[] msg)
	{
		byte[] aMsg = new byte[4];
		Buffer.BlockCopy(msg, 0, aMsg, 0, 4);
		Array.Reverse(aMsg, 0, 4);
		return BitConverter.ToInt32(aMsg, 0);
	}

	private static byte[] ByteWriteInt (int n)
	{
		byte[] arrByte = BitConverter.GetBytes(n);
		//java与C#大小端不一，因此需进行反转
		if (BitConverter.IsLittleEndian)
		{
			Array.Reverse(arrByte);
		}
		return arrByte;
	}

	private static byte[] ByteWriteLong (long l)
	{
		byte[] arrByte = BitConverter.GetBytes(l);
		//java与C#大小端不一，因此需进行反转
		if (BitConverter.IsLittleEndian)
		{
			Array.Reverse(arrByte);
		}
		return arrByte;
	}

	private static byte[] ByteWriteString (string s)
	{
		byte[] arrByte = System.Text.Encoding.ASCII.GetBytes(s);
		return arrByte;
	}

	private static int ByteGetInt (byte[] msg, int nBegin)
	{
		Array.Reverse(msg, nBegin, 4);
		int num = BitConverter.ToInt32(msg, nBegin);
		return num;
	}

	private static bool IsSim (int nOpcode)
	{
		for (int i=0; i<sim.Length; i++)
		{
			if (sim[i] == nOpcode)
			{
				return true;
			}
		}
		return false;
	}
	private static bool IsSpe (int nOpcode)
	{
		for (int i=0; i<spe.Length; i++)
		{
			if (spe[i] == nOpcode)
			{
				return true;
			}
		}
		return false;
	}


	#region http数据包头
	[ThreadStatic]
	/// <summary>
	/// 玩家登陆码，用于控制多端登陆，登录游戏时由服务器返回
	/// </summary>
	public static string m_sAccountLoginKey;

	[ThreadStatic]
	/// <summary>
	/// 服务器ID，每次通讯时附带，为当前服务器ID
	/// </summary>
	public static int m_nServerID;

	[ThreadStatic]
	/// <summary>
	/// 玩家ID，登录游戏时由服务器通过SCBeginGame返回
	/// </summary>
	public static long m_lPlayerID;

	[ThreadStatic]
	/// <summary>
	/// 玩家校验MD5码，登录游戏时由服务器通过SCBeginGame返回
	/// </summary>
	public static string m_sPlayerKey;

	[ThreadStatic]
	/// <summary>
	/// 上次通讯随机KEY，每次通讯需使用不同于上一次的随机Key，6位字符串，范围[0-9][A-Z][a-z]
	/// </summary>
	private static string m_s6LastRandomKey = "";

	/// <summary>
	/// 通讯随机KEY，每次通讯需使用不同于上一次的随机Key（只读）
	/// </summary>
	public static string m_s6RandomKey
	{
		get
		{
			string s = GetKeyRandom();
			while (s.Equals(m_s6LastRandomKey))
			{
				s = GetKeyRandom();
			}
			m_s6LastRandomKey = s;
			return s;
		}
	}

	private static string GetKeyRandom()
	{
		string s = "";
		for (int i = 0; i < 6; i++)
		{
			int type = random.Next(0, 3);
			if (type == 0)//num
			{
				s += random.Next(0, 9).ToString();
			}
			else if (type == 1)
			{//A
				char c1 = 'A';
				c1 = (char)(((int)c1) + random.Next(0, 26));
				s += c1;
			}
			else if (type == 2)
			{//a
				char c2 = 'a';
				c2 = (char)(((int)c2) + random.Next(0, 26));
				s += c2;
			}
		}
		return s;
	}
	#endregion
}
