using UnityEngine;
using System.Collections;


public class GuideWord : MonoBehaviour {

    //public GameObject HeadLeft;
    //public GameObject HeadRight;
	public GameObject TextDi;
	public GameObject TextLabel;
    public GameObject touchGuild;

	public void Show(string text, bool isRotate, Vector3 pos,bool isAlpha){
		TextLabel.GetComponent<UILabel>().text = text;
        transform.localPosition = pos;

		if (isRotate){
			TextDi.transform.localRotation = new Quaternion(0,0,0,0);
		}
		else{
			TextDi.transform.localRotation = new Quaternion(0,180,0,0);
		}
        if (isAlpha)
        {
            gameObject.GetComponent<TweenAlpha>().ResetToBeginning();
            gameObject.GetComponent<TweenAlpha>().PlayForward();
        }
	}
}

