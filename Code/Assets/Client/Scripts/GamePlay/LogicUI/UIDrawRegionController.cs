using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIDrawRegionController : MonoBehaviour {
	
	public enum ShowType{
		LeftToRight,
		RightToLeft,
		BottomToTop,
		TopToBottom,
	};
	public ShowType showType = ShowType.TopToBottom;
	public float duration;
	private UISprite sprite;
	
	
	[HideInInspector]
	public List<EventDelegate> onFinished = new List<EventDelegate>();	
	List<EventDelegate> mTemp = null;
	private bool mStart = false;
	private float mStartTime = 0;
	// Use this for initialization
	void Awake () {
		sprite = GetComponent<UISprite>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!mStart){
			return;
		}
		mStartTime += Time.deltaTime;	
		float fillValue = Mathf.Lerp(0,1,mStartTime/duration);
		sprite.drawRegion = new Vector4(0f,1-fillValue, 1f, 1f);
		if(fillValue >= 1){
			if (onFinished != null)
			{
				mTemp = onFinished;
				onFinished = new List<EventDelegate>();

				// Notify the listener delegates
				EventDelegate.Execute(mTemp);

				// Re-add the previous persistent delegates
				for (int i = 0; i < mTemp.Count; ++i)
				{
					EventDelegate ed = mTemp[i];
					if (ed != null) EventDelegate.Add(onFinished, ed, ed.oneShot);
				}
				mTemp = null;
			}			
			mStart = false;
			enabled = true;
		}
		
	}
	
	public void Play(){
		mStart = true;
		enabled = true;
		mStartTime = 0;
		Update();
	}
	
	/// <summary>
	/// Convenience function -- set a new OnFinished event delegate (here for to be consistent with RemoveOnFinished).
	/// </summary>
	
	public void SetOnFinished (EventDelegate.Callback del) { EventDelegate.Set(onFinished, del); }

	/// <summary>
	/// Convenience function -- set a new OnFinished event delegate (here for to be consistent with RemoveOnFinished).
	/// </summary>

	public void SetOnFinished (EventDelegate del) { EventDelegate.Set(onFinished, del); }

	/// <summary>
	/// Convenience function -- add a new OnFinished event delegate (here for to be consistent with RemoveOnFinished).
	/// </summary>

	public void AddOnFinished (EventDelegate.Callback del) { EventDelegate.Add(onFinished, del); }

	/// <summary>
	/// Convenience function -- add a new OnFinished event delegate (here for to be consistent with RemoveOnFinished).
	/// </summary>

	public void AddOnFinished (EventDelegate del) { EventDelegate.Add(onFinished, del); }

	/// <summary>
	/// Remove an OnFinished delegate. Will work even while iterating through the list when the tweener has finished its operation.
	/// </summary>

	public void RemoveOnFinished (EventDelegate del)
	{
		if (onFinished != null) onFinished.Remove(del);
		if (mTemp != null) mTemp.Remove(del);
	}	
}
