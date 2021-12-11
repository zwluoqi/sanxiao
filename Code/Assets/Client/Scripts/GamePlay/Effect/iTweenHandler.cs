using UnityEngine;
using System.Collections;

public class iTweenHandler : MonoBehaviour {

    public delegate void ProjectileFlyComplete(GameObject go);

    public ProjectileFlyComplete ProjectileFlyCompleteHandler;

	// Use this for initialization
	void Start () {
	
	}
	

    public void OnProjectileFlyComplete()
    {
        if (ProjectileFlyCompleteHandler != null)
        {
            ProjectileFlyCompleteHandler(gameObject);
        }

        // EventManager.Instance.Fire(EventDefine.SKILL_PROJECTILE_FLY_COMPLETE, gameObject);
    }
	
	
	public static void PlayToPos(GameObject obj,Vector3 fromPos,Vector3 toPos,float duration,bool worldSpace,EventDelegate.Callback callBack = null){
		TweenPosition newTweenPos = obj.AddMissingComponent<TweenPosition>();
		newTweenPos.ignoreTimeScale = false;
		newTweenPos.worldSpace = worldSpace;
		newTweenPos.from = fromPos;
		newTweenPos.to = toPos;
		newTweenPos.duration = duration;
		newTweenPos.SetOnFinished(callBack);
		newTweenPos.ResetToBeginning();
		newTweenPos.Play(true);
	}
	
	
	public static void PlayToScale(GameObject obj,Vector3 fromPos,Vector3 toPos,float duration,EventDelegate.Callback callBack){
		TweenScale newTweenScale = obj.AddMissingComponent<TweenScale>();
		newTweenScale.ignoreTimeScale = false;
		newTweenScale.from = fromPos;
		newTweenScale.to = toPos;
		newTweenScale.duration = duration;
		newTweenScale.SetOnFinished(callBack);
		newTweenScale.ResetToBeginning();
		newTweenScale.Play(true);
	}	
}
