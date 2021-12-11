using UnityEngine;
using System.Collections;

public class DestroyParticle : MonoBehaviour {

    private float m_DestroyTime = 5f;
    public float DestroyTime {get{return m_DestroyTime;}  set { m_DestroyTime = value; Destroy(gameObject, m_DestroyTime*0.001f);} } //除以1000

    private float m_OnhitTime = 1f;
    public float OnhitTime { set { m_OnhitTime = value; StartCoroutine("SkillOnhit"); } }

    public delegate void ParticleCompleteDelegate(GameObject go);
    public ParticleCompleteDelegate particleCompleteDelegate;

    public delegate void SkillOnhitDelegate(GameObject go);
    public SkillOnhitDelegate skillOnhitDelegate;

	// Use this for initialization
	void Start () {
	}
	

    IEnumerator SkillOnhit()
    {
        yield return new WaitForSeconds(m_OnhitTime*0.001f);  //除以1000
        if (skillOnhitDelegate != null)
        {
            skillOnhitDelegate(gameObject);
        }
    }

    public static DestroyParticle AddComponent(GameObject go, int onhit_time, int destroy_time)
    {
        DestroyParticle dp = go.AddComponent<DestroyParticle>();
        dp.OnhitTime = onhit_time;
        dp.DestroyTime = destroy_time;
        return dp;
    }

    void OnDestroy()
    {
        if (particleCompleteDelegate != null)
        {
            particleCompleteDelegate(gameObject);
        }
    }
}
