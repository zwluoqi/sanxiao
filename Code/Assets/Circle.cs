using UnityEngine;
using System.Collections;

public class Circle : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	

		public float speed = 5;

	// Update is called once per frame
	void Update () {
	

				transform.localEulerAngles = new Vector3 (0, speed * Time.deltaTime + transform.localEulerAngles.y, 0);
	}
}
