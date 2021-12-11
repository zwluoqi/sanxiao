using UnityEngine;
using System.Collections;
using System;


public class BoxManager  {

    private static BoxManager instance;

    public static BoxManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BoxManager();
            }
            return instance;
        }
    }

	public GameObject buttonOk;
	public GameObject buttonCancle;
	public GameObject buttonClose;
	public GameObject topFrame;

	
	//单确定对话框
	public void ShowMessageTip(string message){
		RemoveMessageboxDirectly();
        CreateMessageBox("MessageTipBox");
		MessageBox messageBox = topFrame.GetComponent<MessageBox>();
		messageBox.textInfo.text = message;
	}

	//确定取消对话框
	public void ShowMessage(string message){
        RemoveMessageboxDirectly();
        CreateMessageBox("MessageBox");
        MessageBox messageBox = topFrame.GetComponent<MessageBox>();
        messageBox.textInfo.text = message;
	}

    public void ShowLibaoShareMessage(string message)
    {
        RemoveMessageboxDirectly();
        CreateMessageBox("LibaoShareBox");
        MessageBox messageBox = topFrame.GetComponent<MessageBox>();
        messageBox.textInfo.text = message;
    }

    public void ShowPopupMessage(string message)
    {
        RemoveMessageboxDirectly();
        CreateMessageBox("PopupTweenTextBox");
        PopupTweenTextBox messageBox = topFrame.GetComponent<PopupTweenTextBox>();
        messageBox.Init(message);
    }

    public void ShowBuyPowerMessage()
    {
        RemoveMessageboxDirectly();
        CreateMessageBox("Tilibuzugoumai");
    }

    public void ShowPowerShareMessage()
    {
        RemoveMessageboxDirectly();
        CreateMessageBox("Tilibuzufenxiang");
    }



	public void ShowStepShareMessage()
	{
		RemoveMessageboxDirectly();
		CreateMessageBox("GameStepbuzufenxiang");
	}

	public void ShowTimeShareMessage()
	{
		RemoveMessageboxDirectly();
		CreateMessageBox("GameTimebuzufenxiang");
	}

	public void ShowGuanggaoMessage()
	{
		RemoveMessageboxDirectly();
		CreateMessageBox("Gameguanggao");
	}
	
	public void RemoveMessageBox(GameObject go){
		if(topFrame != null){
			TweenPosition twPos = topFrame.GetComponent<TweenPosition>();
			twPos.from = new Vector3(0,0,0);
			twPos.to = new Vector3(-500,0,0);
			twPos.AddOnFinished(OnPlayFinished);
			twPos.ResetToBeginning();
			twPos.Play(true);
		}
		buttonOk = null;
		buttonCancle = null;
		buttonClose = null;
	}
	
	private void OnPlayFinished(){
		GameObject.Destroy(topFrame);
		topFrame = null;
	}
	
	private void RemoveMessageboxDirectly(){
		if(topFrame != null){
			OnPlayFinished();
		}
		buttonOk = null;
		buttonCancle = null;
		buttonClose = null;
	}
	
	private void CreateMessageBox(string boxName){
		topFrame = ResourcesManager.Instance.LoadBoxGameObject(boxName);
	
		MessageBox messageBox = topFrame.GetComponent<MessageBox>();
        if (messageBox != null)
        {
            buttonOk = messageBox.okButton;
            buttonCancle = messageBox.cancleButton;
            buttonClose = messageBox.closeButton;

            if (buttonCancle != null)
            {
                UIEventListener.Get(buttonCancle).onClick += RemoveMessageBox;
            }
            if (buttonOk != null)
            {
                UIEventListener.Get(buttonOk).onClick += RemoveMessageBox;
            }
            if (buttonClose != null)
            {
                UIEventListener.Get(buttonClose).onClick += RemoveMessageBox;
            }
        }
	}

	public void ClearNetMask ()
	{

	}

	public void CreatOneButtonBox (string ok, string content, UIEventListener.VoidDelegate par)
	{
		RemoveMessageboxDirectly();
		CreateMessageBox ("onebuttonbox");
		MessageBox messageBox = topFrame.GetComponent<MessageBox>();
		messageBox.textInfo.text = content;
		UIEventListener.Get(BoxManager.Instance.buttonOk).onClick += par;
	}
}
