using UnityEngine;
using System.Collections;

public class UIMissionView : MonoBehaviour {
	//UI
	public  UISprite m_sprite;
	public UILabel m_label;
	public UISprite m_doneSprite;	
	
	//Data
	private int m_num;
	private int m_reqNum;

    public int m_ntype { get; set; }
	
	void Awake(){
		m_ntype = -1;
	}

	public void Init(int type, int reqNum)
	{
	    m_ntype = type;
	    m_reqNum = reqNum;
		m_num = 0;
		m_doneSprite.gameObject.SetActive(false);
	}
	public void SetScore(int score)
	{
        m_num = score;
        if (LevelData.mode == CopyMode.TAFANG)
        {
            m_label.text = "";
            m_doneSprite.gameObject.SetActive(false);
        }
        else
        {
            m_label.text = m_num + "/" + m_reqNum;
            if (m_num >= m_reqNum && !m_doneSprite.gameObject.activeSelf)
            {
                m_doneSprite.gameObject.SetActive(true);
                SoundEffect.Instance.PlaySound(SoundEffect.missionCompletedEffect);
            }
        }

	}
	
	public void SetUIForBefore(CopyMode mode){
        if (mode == CopyMode.TAFANG)
        {
            m_label.text = "";
        }
        else
        {
            m_label.text = m_num + "/" + m_reqNum;
        }
        
		m_doneSprite.gameObject.SetActive(false);
	}
}
