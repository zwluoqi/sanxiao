using UnityEngine;
using System.Collections;
using GCGame.Table;

public class UIZhuanpanItemView : MonoBehaviour {
    public UISprite icon;
    public UILabel num;

    public void Init(Tab_Zhuanpan zhuanpan){
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
            
        }
        else if (zhuanpan.Classid == (int)ClassID.Equip)
        {
            Tab_Equip equip = TableManager.GetEquipByID(zhuanpan.Objid);
            icon.spriteName = equip.SpriteName;
        }

        num.text = zhuanpan.Num.ToString();

    }
}
