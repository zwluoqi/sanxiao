using UnityEngine;
using System.Collections;



public class FlyItemAnmationCurve : MonoBehaviour {

    public FlyItemAnimationCurveData[] curves;

    private FlyItemAnimationCurveData currentData;
    private float timer = 0;
    private Vector3 initPosition;
    private Vector3 distance;
    private Vector3 lastMoveDelta;

    public void Play(AnimationCurveType type,Vector3 targetPos)
    {
        currentData = curves[(int)type];
        timer = 0;
        initPosition = this.transform.localPosition;
        distance = (targetPos - initPosition);
        lastMoveDelta = Vector3.zero;
        enabled = true;
    }

    public void Stop()
    {
        currentData = null;
    }

    void Update()
    {
        if (currentData == null)
            return;
        timer += Time.deltaTime;
        float x = 0f, y = 0f, z = 0f;
        float t;

        if (timer >= currentData.duration)
        {
            t = 1f;
        }
        else
        {
            t = timer / currentData.duration;
        }

        x = currentData.x.Evaluate(t);
        y = currentData.y.Evaluate(t);

        Vector3 deltaMove = new Vector3(x * distance.x, y * distance.y, 0);

        Vector3 p = transform.localPosition + (deltaMove - lastMoveDelta);
        lastMoveDelta = deltaMove;
        transform.localPosition = p;

        if (timer >= currentData.duration)
        {
            enabled = false;
        }
    }
}
