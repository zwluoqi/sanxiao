using UnityEngine;
using System.Collections;

public class PopupTweenTextBox : MonoBehaviour
{

	public GameObject label;

	public void Init(string txt){
		label.GetComponent<UILabel>().text = txt;
	}

	public void OnTweenAlphaDone(){
		Destroy (gameObject);
	}
}
