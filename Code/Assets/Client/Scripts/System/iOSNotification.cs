using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class iOSNotification
{
	public int type;
	public string sTitle;
	public string sText;
	public string sTime;
	public string sDay;
	
	public string sAlertTitle 
	{ 
		get{
			return sTitle;
		}
	}
	
	public string sAlertBody
	{ 
		get{
			return sText;
		}
	}
	
	public List<System.DateTime> kTime
	{
		get{
			List<System.DateTime> lsTime = new List<System.DateTime>();
			lsTime.Clear();
			if ( type != 1 )
			{
				int y = int.Parse(sTime.Substring(0, 4));
				int M = int.Parse(sTime.Substring(4, 2));
				int d = int.Parse(sTime.Substring(6, 2));
				int h = int.Parse(sTime.Substring(8, 2));
				int m = int.Parse(sTime.Substring(10, 2));
				int s = int.Parse(sTime.Substring(12, 2));
				System.DateTime time = new System.DateTime(y, M, d, h, m, s);
				lsTime.Add(time);
			} else {
				int h = int.Parse(sTime.Substring(0, 2));
				int m = int.Parse(sTime.Substring(2, 2));
				int s = int.Parse(sTime.Substring(4, 2));
				if (bLoopDay)
				{
					System.DateTime _now = System.DateTime.Now;
					System.DateTime _today = new DateTime(_now.Year, _now.Month, _now.Day, h, m, s);
					if (_today <= _now)
					{
						_today.AddDays(1);
					}
					lsTime.Add(_today);
				} else {
					string[] day = sDay.Split(new char[]{','}, System.StringSplitOptions.RemoveEmptyEntries);
					foreach (string sWeek in day)
					{
						System.DateTime t = Now2Week(int.Parse(sWeek), h, m, s);
						lsTime.Add(t);
					}
				}
			}
			return lsTime;
		}
	}
	
	public bool bLoopDay
	{
		get{
			string[] day = sDay.Split(new char[]{','}, System.StringSplitOptions.RemoveEmptyEntries);
			return day.Length >= 7;
		}
	}
	public bool bLoopWeek
	{
		get{
			return (type == 1);
		}
	}
	
	private DateTime Now2Week(int week, int hour, int min, int sec){
		DateTime _today = DateTime.Now;
		int _hour = hour;
		int _min = min;
		int _sec = sec;
		int _week = 1;
		
		if (_today.DayOfWeek == DayOfWeek.Monday) {
			_week = 1;
		} else if (_today.DayOfWeek == DayOfWeek.Tuesday) {
			_week = 2;
		} else if (_today.DayOfWeek == DayOfWeek.Wednesday) {
			_week = 3;
		} else if (_today.DayOfWeek == DayOfWeek.Thursday) {
			_week = 4;
		} else if (_today.DayOfWeek == DayOfWeek.Friday) {
			_week = 5;
		} else if (_today.DayOfWeek == DayOfWeek.Saturday) {
			_week = 6;
		} else if (_today.DayOfWeek == DayOfWeek.Sunday) {
			_week = 7;
		}
		int tempDay;
		
		// alert time is tody
		if (week == _week &&
		    ((hour > _today.Hour) ||
		     (hour == _today.Hour && min > _today.Minute) ||
		 	 (hour == _today.Hour && min == _today.Minute && sec > _today.Second)))
		{
			tempDay = 0;
		} else {
			tempDay = (week > _week)?(week-_week):(week-_week+7);
		}
		_today = _today.AddDays(tempDay);
		
		DateTime _resTime = new DateTime(_today.Year, _today.Month, _today.Day, _hour, _min, _sec);
		return _resTime;
	}
}
