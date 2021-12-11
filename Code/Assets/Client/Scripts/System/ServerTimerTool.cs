using UnityEngine;
using System.Collections;
using System;

public class ServerTimerTool {

    private static long distance = 0;

    const long begin = 621355968000000000;//1970,1,1到公元元年的时间差
    //const long dif = 288000000000;

    public static void CorrectTime(long serverTime)
    {
        distance = serverTime * 10000 + begin - DateTime.UtcNow.Ticks;
    }
    public static DateTime CurrentTime
    {
        get
        {
            return new DateTime(DateTime.Now.Ticks + distance);
        }
    }

    public static DateTime Java2CSTime(long time)
    {
        return new DateTime(time * 10000 + begin - DateTime.UtcNow.Ticks + DateTime.Now.Ticks);
    }
}
