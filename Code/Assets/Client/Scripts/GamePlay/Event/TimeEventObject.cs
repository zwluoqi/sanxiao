using UnityEngine;
using System.Collections;

public class TimeEventObject : MonoBehaviour {
	public delegate void OnTimeFinishedCallback(GameObject go);
	OnTimeFinishedCallback callback = null;

	public float duration = 1; 
	public void SetOnTimeFinished(OnTimeFinishedCallback callBack,float deltaTime){
		callback = callBack;
		duration = deltaTime;
		CancelInvoke("Callback");
        Invoke("Callback", duration);
	}
    public void SetOnTimeFinished(OnTimeFinishedCallback callBack)
    {
        callback = callBack;
        CancelInvoke("Callback");
        Invoke("Callback", duration);
    }
	
	private void Callback(){
		if(callback != null){
            OnTimeFinishedCallback tmp = callback;
            callback = null;
            tmp(gameObject);
		}
	}
}
