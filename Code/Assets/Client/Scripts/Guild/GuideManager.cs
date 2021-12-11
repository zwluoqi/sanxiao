using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class GuideManager :MonoBehaviour
{
    private static GuideManager instance;

    public static GuideManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GuideManager>();
            }
            if (instance == null)
            {
                instance = new GameObject("GuideManager").AddComponent<GuideManager>();
            }
            return instance;
        }

    }




    public GuidePanel m_GuildCoverPanel;
    public GuideTargets m_GuildTarget;
    public GuideEleTip m_GuildEleTip;
    public GameObject Guildtip;
    public GameObject GuideSayWord;

	public void ShowForceGuide(GameObject colliderTarget, bool isTransformToParent, string word, Vector3 wordPos, bool isRotate = true){
		float x,y,w,h;
		x = colliderTarget.GetComponent<BoxCollider>().center.x - colliderTarget.GetComponent<BoxCollider>().size.x / 2;
		y = colliderTarget.GetComponent<BoxCollider>().center.y + colliderTarget.GetComponent<BoxCollider>().size.y / 2;
		w = colliderTarget.GetComponent<BoxCollider>().size.x;
		h = colliderTarget.GetComponent<BoxCollider>().size.y;
		m_GuildCoverPanel.ShowForceWithHand(colliderTarget.transform,isTransformToParent,new Rect( x,y,w,h),word,wordPos, isRotate);
	}

    public void ShowGuideTarget(UIWidget widget, bool isTransformToParent, string word, Vector3 wordPos, bool isRotate = true)
    {
        float x, y, w, h;
        x = - widget.width / 2;
        y = widget.height / 2;
        w = widget.width;
        h = widget.height;
        m_GuildTarget.ShowForceWithoutHand(widget.transform,isTransformToParent, new Rect(x, y, w, h), word, wordPos, isRotate);
    }

    public void ShowMutipleGuide(UIWidget widget, string word, Vector3 wordPos, List<UIWidget> maskWidgets, Transform target, SwipeDirection dic,bool isRotate = true)
    {
        float x, y, w, h;
        x = -widget.width / 2;
        y = widget.height / 2;
        w = widget.width;
        h = widget.height;
        m_GuildEleTip.ShowForceEleTip(widget.transform, new Rect(x, y, w, h), word, wordPos,maskWidgets,target, isRotate ,dic);
    }
		

    public void ShowGuildTip(Queue<TipWord> words){
        Guildtip.SetActive(true);
        Guildtip.GetComponent<TipGuide>().ShowWords(words);
    }
		
	public void HideGuide(){
        if (m_GuildCoverPanel != null)
        {
            m_GuildCoverPanel.Hide();
        }
        if (m_GuildTarget != null)
        {
            m_GuildTarget.Hide();
        }
        if (m_GuildEleTip != null)
        {
            m_GuildEleTip.Hide();
        }
        if (Guildtip != null)
        {
            Guildtip.SetActive(false);
        }
        if (GuideSayWord != null)
        {
            GuideSayWord.SetActive(false);
        }
	}

}
