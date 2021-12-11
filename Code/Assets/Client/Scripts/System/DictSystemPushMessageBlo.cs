using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GCGame.Table;
using System;
using LitJson;

	public class DictSystemPushMessageBlo
	{

        public static string getPushMsg()
        {
			try{

                //IList<Com.Hcwy.H1.DbModel.DictSystemPushMessage> systemPushMsgList = DictSystemPushMessageDao.GetList("", "");
                Hashtable systemPushMsgList = TableManager.GetPushMessage();

                string powerOverTime = "";
                try
                {
                    if (LocalDataBase.Instance().GetDataNum(DataType.power) < LocalDataBase.maxPower)
                    {
                        long powerTimes = (LocalDataBase.maxPower - LocalDataBase.Instance().GetDataNum(DataType.power)) * LocalDataBase.coolDownSecond * 1000;
                        powerOverTime = DateTime.Now.AddMilliseconds(powerTimes).ToString("yyyyMMddHHmmss");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("Android push message error: " + ex);
                }
			JsonData msgs = new JsonData();
            int imsg1 = 0;//寰????
            int imsg2 = 10;//瀹????

            foreach (Tab_PushMessage dictSystemPushMessage in systemPushMsgList.Values)
            {
				JsonData msg = new JsonData();
                msg["type"] = (int)dictSystemPushMessage.MsgType;
				msg["title"] = dictSystemPushMessage.MsgName;
				msg["text"] = dictSystemPushMessage.MsgText;
				msg["time"] = dictSystemPushMessage.Time;
				List<int> days = new List<int>();
                string[] daysstr = dictSystemPushMessage.Days.Split(',');
                foreach (string day in daysstr)
                {
                    if (day.Equals("")) continue;
					days.Add(Int32.Parse(day));
                }
				msg["days"] = JsonMapper.ToObject(JsonMapper.ToJson(days));
                if (dictSystemPushMessage.MsgType==2)
                {
                    imsg2 = imsg2 + 1;
					msg["id"] = imsg2;
                    if (dictSystemPushMessage.SPMId == 7 && !powerOverTime.Equals(""))
                    {
						msg["time"] = powerOverTime;
						msgs.Add(msg);
                    }
                }
                else
                {
                    imsg1 = imsg1 + 1;
					msg["id"] = imsg1;
                    msgs.Add(msg);
                }
  
            }
            string jsonData = JsonMapper.ToJson(msgs);
		    //jsonData=jsonData.Replace("}{","},{");
            return jsonData;
			}catch(Exception ex){
				Debug.LogError("Android push message error: " + ex);
				List<JsonData> msgs = new List<JsonData>();
				string jsonData = JsonMapper.ToJson(msgs);
				return jsonData;
			}
        }
        
		public static List<iOSNotification> getIOSPushMsg()
		{
			try{
				//
                Hashtable systemPushMsgList = TableManager.GetPushMessage();
				List<iOSNotification> lsRes = new List<iOSNotification>();
				lsRes.Clear();
				// player power
                string powerOverTime = "";
                try
                {
                    if (LocalDataBase.Instance().GetDataNum(DataType.power) < LocalDataBase.maxPower)
                    {
                        long powerTimes = (LocalDataBase.maxPower - LocalDataBase.Instance().GetDataNum(DataType.power)) * LocalDataBase.coolDownSecond * 1000;
                        powerOverTime = DateTime.Now.AddMilliseconds(powerTimes).ToString("yyyyMMddHHmmss");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("iOS push message error: " + ex);
                }


				// all
                foreach (Tab_PushMessage dictSystemPushMessage in systemPushMsgList.Values)
				{
					iOSNotification msg = new iOSNotification();
					
					if (dictSystemPushMessage.MsgType == 1)
					{
						msg.type = (int)dictSystemPushMessage.MsgType;
						msg.sTitle = dictSystemPushMessage.MsgName;
						msg.sText = dictSystemPushMessage.MsgText;
						msg.sTime = dictSystemPushMessage.Time;
						msg.sDay = dictSystemPushMessage.Days;
						lsRes.Add(msg);
					} else if (dictSystemPushMessage.MsgType==2) {
						if (dictSystemPushMessage.SPMId == 7 && !powerOverTime.Equals(""))
						{
							msg.type = (int)dictSystemPushMessage.MsgType;
							msg.sTitle = dictSystemPushMessage.MsgName;
							msg.sText = dictSystemPushMessage.MsgText;
							msg.sTime = powerOverTime;
							msg.sDay = dictSystemPushMessage.Days;
							lsRes.Add(msg);
						}
					}
				}

                Debug.Log("lsRes Count:" + lsRes.Count);

				return lsRes;
			}catch(Exception ex){
				Debug.LogError("iOS push message error: " + ex);
				return null;
			}
		}
	}


