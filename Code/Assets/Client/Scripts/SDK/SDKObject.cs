using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XZXD;

public class SDKObjecty  {

	public static void buyProduct(int tabid,NativeCallback.BuySuccess onBuySuccess)
    {
        string productid = RubyShopController.GetProductIDByTabID(tabid);
		XZXD.NativeCallback.m_BuySuccess = onBuySuccess;
		XZXD.NativeCallback.tabid = tabid;
		Dictionary<string,string> payInfo = new Dictionary<string, string> ();
		payInfo ["rechargedId"] = tabid.ToString();
		payInfo ["pingtai_id"] = productid;
		payInfo ["Product_Count"] = "1";
		payInfo ["orderId"] = System.DateTime.Now.Ticks.ToString();

		XZXD.NativeCaller.sdkPay (payInfo, "");
    }

    public static void Init()
    {
        LocalDataBase.Instance().InitIapIdentifierInfo();
    }


#if UNITY_IOS
    public static void AddNotification(bool addNote)
    {
        try
        {
            if (addNote)
            {
                List<iOSNotification> lsData = DictSystemPushMessageBlo.getIOSPushMsg();
                foreach (iOSNotification data in lsData)
                {
                    List<System.DateTime> lsTime = data.kTime;
                    if (lsTime != null)
                    {
                        if (data.bLoopDay)
                        {
                            foreach (System.DateTime dTi in lsTime)
                            {
                                AddDay(data.sAlertTitle, data.sAlertBody, dTi, data.bLoopDay);
                            }
                        }
                        else
                        {
                            foreach (System.DateTime dTi in lsTime)
                            {
                                Add(data.sAlertTitle, data.sAlertBody, dTi, data.bLoopWeek);
                            }
                        }
                    }
                }
            }
            else
            {
                ClearNotifications();
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("Error: " + e);
        }
    }
    /// <summary>
    /// 清理本地推送信息.
    /// </summary>
    public static void ClearNotifications()
    {
        UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
        UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
    }

    private static bool Add(string alterAction, string alterBody, System.DateTime fireDate, bool loop_week)
    {
        UnityEngine.iOS.LocalNotification notification = new UnityEngine.iOS.LocalNotification();
        notification.alertAction = alterAction;
        notification.alertBody = alterBody;
        notification.fireDate = fireDate;
        notification.soundName = UnityEngine.iOS.LocalNotification.defaultSoundName;
        if (loop_week)
        {
            notification.repeatInterval = UnityEngine.iOS.CalendarUnit.Weekday;
        }
        else
        {
            notification.repeatInterval = UnityEngine.iOS.CalendarUnit.Year;
        }
        UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(notification);
        return true;
    }
    private static bool AddDay(string alterAction, string alterBody, System.DateTime fireDate, bool loop_Day)
    {
        Debug.Log("AddDay+alterAction:" + alterAction + " alterBody:" + fireDate);
        UnityEngine.iOS.LocalNotification notification = new UnityEngine.iOS.LocalNotification();
        notification.alertAction = alterAction;
        notification.alertBody = alterBody;
        notification.fireDate = fireDate;
        notification.soundName = UnityEngine.iOS.LocalNotification.defaultSoundName;
        if (loop_Day)
        {
            notification.repeatInterval = UnityEngine.iOS.CalendarUnit.Day;
        }
        else
        {
            notification.repeatInterval = UnityEngine.iOS.CalendarUnit.Year;
        }
        UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(notification);
        return true;
    }
#endif
}
