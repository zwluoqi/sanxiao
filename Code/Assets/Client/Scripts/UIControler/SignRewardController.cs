using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using GCGame.Table;

public class SignRewardController :Page {

    public UIGrid grid;
    private string ItemName = "SignRewardItem";
    private List<GameObject> rewards = new List<GameObject>();

    protected override void DoOpen()
    {
        foreach (SignReward signReward in currentMonthSignRewards)
        {
            GameObject go = ResourcesManager.Instance.loadWidget(ItemName, grid.transform);
            go.name = signReward.signTime.ToString();
            go.GetComponent<UISignRewardView>().Init(signReward);
            rewards.Add(go);
   
        }
        grid.repositionNow = true;
    }

    protected override void DoClose()
    {
        ClearGameObjects();
        base.DoClose();
    }

    private void ClearGameObjects()
    {
        foreach (GameObject obj in rewards)
        {
            Destroy(obj);
        }
        rewards.Clear();
    }

    public static List<SignReward> currentMonthSignRewards = new List<SignReward>();

    public static void InitSignRewards(DateTime currentTime)
    {
        currentMonthSignRewards.Clear();
        Dictionary<int, SignReward> currentMonthSignRewardsDict = new Dictionary<int, SignReward>();
        System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
        int weekOfYear = gc.GetWeekOfYear(currentTime, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);

        Hashtable signRewars = TableManager.GetSign();
        foreach (DictionaryEntry dic in signRewars)
        {
            Tab_Sign tab_sign = (Tab_Sign)dic.Value;
            if (tab_sign.Year == currentTime.Year && tab_sign.WeekOfYear == weekOfYear)
            {
                Reward reward = new Reward();
                reward.classID = tab_sign.Classid;
                reward.propID = tab_sign.Propid;
                reward.objID = tab_sign.Objid;
                reward.num = tab_sign.Num;

                if (!currentMonthSignRewardsDict.ContainsKey(tab_sign.DayOfWeek))
                {
                    SignReward signreward = new SignReward();
                    signreward.Init(tab_sign.Year, tab_sign.WeekOfYear, tab_sign.DayOfWeek);
                    currentMonthSignRewardsDict.Add(tab_sign.DayOfWeek, signreward);
                }

                currentMonthSignRewardsDict[tab_sign.DayOfWeek].rewards.Add(reward);
            }
        }
        foreach (SignReward item in currentMonthSignRewardsDict.Values)
        {
            currentMonthSignRewards.Add(item);
        }
        currentMonthSignRewards.Sort(delegate(SignReward one, SignReward two) { return one.m_dayofweek.CompareTo(two.m_dayofweek); });
        if (SignReward.CanGetRewardToday())
        {
            foreach (SignReward reward in currentMonthSignRewards)
            {
                if (reward.state == SignState.NotReward)
                {
                    reward.state = SignState.TipReward;
                    break;
                }
            }
        }
    }
}
