using UnityEngine;
using System.Collections;
using System;

public class PowerShare : MonoBehaviour {
    public UILabel remainedTime;


    InfoController infoController;

    void OnEnable()
    {
        infoController = SceneManager.Instance.UI.GetComponent<UIController>().info;
    }

    void Update()
    {
        if (infoController.remainedTimeSpan > TimeSpan.Zero)
        {
            TimeSpan tSpan = new TimeSpan(0,0,(int)(infoController.remainedTimeSpan.TotalSeconds*3));
            remainedTime.text = string.Format(LanguageManger.GetMe().GetWords("L_S003"), tSpan.Minutes, tSpan.Seconds);
        }
    }
}
