using UnityEngine;
using System.Collections;
using GCGame.Table;

public class ZhuanpanAnimation : MonoBehaviour {

    public GameObject button;
    public GameObject mask;
    public UILabel concratulation;

    public UISprite icon;
    public GameObject anmationParent;

    private ZhuanPanController m_controller;
    public void PlayResult(Transform resultPos,Tab_Zhuanpan zhuanpan,ZhuanPanController m_con)
    {
        gameObject.SetActive(true);
        m_controller = m_con;
        //mask.SetActive(true);
        button.SetActive(false);
        concratulation.gameObject.SetActive(false);
        if (zhuanpan.Classid == (int)ClassID.Player)
        {
            if (zhuanpan.Objid == (int)DataType.jinbi)
            {
                icon.spriteName = "xiaozuanshitubiao";
            }
            else if (zhuanpan.Objid == (int)DataType.power)
            {
                icon.spriteName = "tili";
            }
            else if (zhuanpan.Objid == (int)DataType.zhuanshi)
            {
                icon.spriteName = "xiaozuanshitubiao";
            }
            concratulation.text = string.Format(LanguageManger.GetMe().GetWords("L_zhuanpan001"), zhuanpan.Num);
        }
        else if (zhuanpan.Classid == (int)ClassID.Equip)
        {
            Tab_Equip equip = TableManager.GetEquipByID(zhuanpan.Objid);
            icon.spriteName = equip.SpriteName;
            concratulation.text = string.Format(LanguageManger.GetMe().GetWords("L_zhuanpan002"), equip.Detial, zhuanpan.Num);
        }

        TweenPosition pos = anmationParent.GetComponent<TweenPosition>();
        pos.from = anmationParent.transform.parent.transform.InverseTransformPoint(resultPos.position);
        pos.to = Vector3.zero;
        pos.ResetToBeginning();
        pos.PlayForward();
    }

    public void OnFinishedAnimation()
    {
        //mask.SetActive(false);
        button.SetActive(true);
        concratulation.gameObject.SetActive(true);
    }

    public void OnCloseResult()
    {
        gameObject.SetActive(false);
    }
}
