using System;
namespace NetWork.Layer
{
	public interface IHTTPUtil
	{

		bool Send(byte[] data);

		void Init(string url,HTTPManager httpmanager);
	}

}