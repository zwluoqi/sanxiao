using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SignReward  {
    public int signTime;
    public List<Reward> rewards = new List<Reward>();
    public SignState state;


    public int m_year;
    public int m_weekofyear;
    public int m_dayofweek;
    public void Init(int year, int weekofyear, int dayofweek)
    {
        m_year = year;
        m_weekofyear = weekofyear;
        m_dayofweek = dayofweek;
        signTime = dayofweek;
        state = (SignState)PlayerPrefs.GetInt("signRewardState|" + m_year + "|" + m_weekofyear + "|" + m_dayofweek + "|", (int)SignState.NotReward);
    }

    public static bool CanGetRewardToday()
    {
        bool getok = false;
        DateTime currentTime = TimeHelper.GetBeijingTime(out getok);
        System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
        int weekOfYear = gc.GetWeekOfYear(currentTime, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);

        int hasGetValue = PlayerPrefs.GetInt("hasSignRewardState|" + currentTime.Year + "|" + weekOfYear + "|" + currentTime.DayOfWeek + "|", 0);
        bool hasGet = hasGetValue != 0;
        string lastSignTime = PlayerPrefs.GetString("hasSignTime");
        if (string.IsNullOrEmpty(lastSignTime))
        {
            return true;
        }
        else
        {
            DateTime lastSignDateTime = new DateTime(long.Parse(lastSignTime));
            TimeSpan span = currentTime - lastSignDateTime;
            if (span.TotalDays >= 1 || !hasGet)
            {
                return true;
            }
        }

        return false;
    }

    public void GetSignRewardToday()
    {
        state = SignState.HasReward;
        bool getok = false;
        DateTime currentTime = TimeHelper.GetBeijingTime(out getok);
        System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
        int weekOfYear = gc.GetWeekOfYear(currentTime, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);

        PlayerPrefs.SetInt("hasSignRewardState|" + currentTime.Year + "|" + weekOfYear + "|" + currentTime.DayOfWeek + "|", 1);
        PlayerPrefs.SetString("hasSignTime", currentTime.Ticks.ToString());
        PlayerPrefs.SetInt("signRewardState|" + m_year + "|" + m_weekofyear + "|" + m_dayofweek + "|", (int)state);
    }
}
