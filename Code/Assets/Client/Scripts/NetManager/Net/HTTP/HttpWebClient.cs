using System;
using System.Net;

namespace NetWork.Layer
{
    public class HttpWebClient : WebClient
    {

        private int _timeOut = 10;

        public int Timeout
        {
            get
            {
                return _timeOut;
            }
            set
            {
                if (value <= 0)
                    _timeOut = 10;
                _timeOut = value;
            }
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            request.Timeout = 1000 * Timeout;
            request.ReadWriteTimeout = 1000 * Timeout;
            return request;
        }
    }
}