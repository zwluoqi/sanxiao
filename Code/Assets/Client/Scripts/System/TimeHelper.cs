using UnityEngine;
using System.Collections;
using System;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using System.IO;

public class TimeHelper  {

    /// <summary> 
    /// 获取标准北京时间，读取http://www.beijing-time.org/time.asp 
    /// </summary> 
    /// <returns>返回网络时间</returns> 
    public static DateTime GetBeijingTime(out bool ok)
    {

        DateTime dt;
        WebRequest wrt = null;
        WebResponse wrp = null;
        try
        {
            wrt = WebRequest.Create("http://www.beijing-time.org/time.asp");
            wrp = wrt.GetResponse();

            string html = string.Empty;
            using (Stream stream = wrp.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                {
                    html = sr.ReadToEnd();
                }
            }

            string[] tempArray = html.Split(';');
            for (int i = 0; i < tempArray.Length; i++)
            {
                tempArray[i] = tempArray[i].Replace("\r\n", "");
            }

            string year = tempArray[1].Split('=')[1];
            string month = tempArray[2].Split('=')[1];
            string day = tempArray[3].Split('=')[1];
            string hour = tempArray[5].Split('=')[1];
            string minite = tempArray[6].Split('=')[1];
            string second = tempArray[7].Split('=')[1];

            dt = DateTime.Parse(year + "-" + month + "-" + day + " " + hour + ":" + minite + ":" + second);
            ok = true;
        }
        catch (WebException)
        {
            ok = false;
            return DateTime.Parse("2011-1-1");
        }
        catch (Exception)
        {
            ok = false;
            return DateTime.Parse("2011-1-1");
        }
        finally
        {
            if (wrp != null)
                wrp.Close();
            if (wrt != null)
                wrt.Abort();
        }
        return dt;

    }

}
