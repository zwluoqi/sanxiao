using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RunCoroutine : MonoBehaviour
{
	static GameObject crtGo = null;
	private IEnumerator iEnum = null;
	public static RunCoroutine Run(IEnumerator routine)
	{
		RunCoroutine rcrt = GetInst();
		rcrt.StartCoroutine (rcrt._Run (routine));
		return rcrt;
	}
	//mayby to crash
	public static void Stop(RunCoroutine runCrt)
	{
		if (null == runCrt)
			return;
		runCrt.StopCoroutine(runCrt.iEnum);
		GameObject.Destroy(runCrt);
	}
	IEnumerator _Run (IEnumerator _routine)
	{
		iEnum = _routine;
		yield return StartCoroutine (_routine);
		GameObject.Destroy( this );
	}
	
	public static RunCoroutine GetInst ()
	{
		if (null == crtGo) {
			crtGo = GameObject.Find ("CrtRunner");
			crtGo = new GameObject ("CrtRunner");
			GameObject.DontDestroyOnLoad (crtGo);
		}
		RunCoroutine resu = crtGo.AddComponent<RunCoroutine> ();
		return resu;
	}
	
	public static void Release()
	{
		if(crtGo != null)
		{
			GameObject.DestroyImmediate(crtGo);
		}
		
		crtGo = null;
	}
}