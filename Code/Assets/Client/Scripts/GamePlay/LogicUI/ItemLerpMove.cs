using UnityEngine;
using System.Collections;

public class ItemLerpMove : MonoBehaviour {

    public float horitalSpeed = 800;
    public float defaultVicSpeed = 120;
    public float viticalSpeed = 120;
    public float defaultM = 720*100;
    public float viticalAcclerationSpeed = 120;
    public float viticalAccAccSpeed = 120;
    public Vector3 from = Vector3.zero;
    public Vector3 to = new Vector3(0,0,360);
    public float rotationOnceDuration = 0.3f;
    public Transform centerTarget;
    public bool hasArrivedTarget = true;

    public delegate void OnTimeFinishedCallback();
    public OnTimeFinishedCallback onFinish = null;

    private float factor = 0;
    private float rotationTime = 0;
    void Update()
    {

        if (!hasArrivedTarget && centerTarget != null)
        {
            rotationTime += Time.deltaTime;
            if (rotationTime > rotationOnceDuration)
            {
                rotationTime -= rotationOnceDuration;
            }
            factor = rotationTime / rotationOnceDuration;
            Quaternion currentQuaternion = Quaternion.Euler(new Vector3(
                Mathf.Lerp(from.x, to.x, factor),
                Mathf.Lerp(from.y, to.y, factor),
                Mathf.Lerp(from.z, to.z, factor)));

            viticalAcclerationSpeed += viticalAccAccSpeed * Time.deltaTime;
            viticalSpeed += viticalAcclerationSpeed * Time.deltaTime;
            Vector3 viticalDic = centerTarget.localPosition - this.transform.localPosition;
            Vector3 horitalDic = Quaternion.Euler(0, 0, 90) * viticalDic;
            float viticalDis = (viticalDic.normalized * viticalSpeed * Time.deltaTime).magnitude;
            if (viticalDis > viticalDic.magnitude)
            {
                enabled = false;
                hasArrivedTarget = true;
                this.transform.localPosition = centerTarget.localPosition;
                this.transform.localRotation = Quaternion.identity;
                if (onFinish != null)
                {
                    OnTimeFinishedCallback tmp = onFinish;
                    onFinish = null;
                    tmp();
                }
            }
            else
            {
                Vector3 moveDetal = (viticalDic.normalized * viticalSpeed + horitalDic.normalized * horitalSpeed) * Time.deltaTime;
                this.transform.localPosition += moveDetal;
                this.transform.localRotation = currentQuaternion;
            }

        }
    }

    public void MoveToTarget(Transform target)
    {
        viticalSpeed = defaultVicSpeed;
        viticalAcclerationSpeed = defaultM/(target.localPosition - this.transform.localPosition).magnitude;
        centerTarget = target;
        hasArrivedTarget = false;
        enabled = true;
        rotationTime = 0;
        this.transform.localRotation = Quaternion.identity;
    }
}
