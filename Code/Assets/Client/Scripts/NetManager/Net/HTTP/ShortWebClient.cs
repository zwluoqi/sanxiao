using UnityEngine;
using System.Collections;
using System.Net;
namespace NetWork.Layer
{
    public class ShortWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(System.Uri address)
        {
            HttpWebRequest hwr = (HttpWebRequest)base.GetWebRequest(address);
            hwr.Timeout = 30000;
            hwr.ReadWriteTimeout = 30000;
            return hwr;
        }
    }
}