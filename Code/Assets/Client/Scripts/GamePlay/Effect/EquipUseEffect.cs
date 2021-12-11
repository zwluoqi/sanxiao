using UnityEngine;
using System.Collections;

public class EquipUseEffect : MonoBehaviour {
    public float effectDuration;
    public float destroyDuration;
    public GameObject particle;
    
    private UITweener[] tweens;

    private GameObject particleInst;
    [HideInInspector]
    public string prefabName = "";
    void Awake()
    {
        tweens = gameObject.GetComponentsInChildren<UITweener>();
    }

    public void PlayEffect()
    {
        if (particle != null)
        {
            GameObject go = GameObject.Instantiate(particle) as GameObject;
            ParticleSystem[] pss = go.GetComponentsInChildren<ParticleSystem>(true);
            foreach (ParticleSystem ps in pss)
            {
                foreach (Material material in ps.GetComponent<Renderer>().sharedMaterials)
                {
                    if (material != null)
                    {
                        material.renderQueue = 3200;
                    }
                }
                ps.Play();
            }
            go.transform.parent = this.transform;
            go.transform.localPosition = Vector3.one;
            go.transform.localScale = Vector3.one;
            go.transform.localEulerAngles = Vector3.zero;
            particleInst = go;
        }

        foreach (UITweener tween in tweens)
        {
            tween.ResetToBeginning();
            tween.PlayForward();
        }

        //动画播放完成;
        EventDelayManger.Instance.CreateEvent(AnimationCompleteHandler, this.effectDuration);

         //删除对象;
        EventDelayManger.Instance.CreateEvent(OnDesrtoyDuration, this.destroyDuration);
    }

    private void AnimationCompleteHandler()
    {

    }

    private void OnDesrtoyDuration()
    {
        if (particleInst != null)
        {
            GameObject.Destroy(particleInst);
        }
        WidgetBufferManager.Instance.DestroyWidgetObj(prefabName, this.gameObject);
    }
}
