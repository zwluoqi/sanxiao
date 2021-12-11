using UnityEngine;
using System.Collections;
using GCGame.Table;


public class RandomGiftPage : Page {

    public int equipShopid = 6;

    public void GetBtn()
    {
        Tab_Equipshop equipShop = TableManager.GetEquipshopByID(equipShopid);
        if (equipShop != null)
        {
            if (LocalDataBase.Instance().GetDataNum(DataType.zhuanshi) >= equipShop.CostRuby)
            {
                LocalDataBase.Instance().DecreaseDataNum(DataType.zhuanshi, equipShop.CostRuby);
                for (int i = 0; i < 5; i++)
                {
                    int equipid = equipShop.GetEquipidbyIndex(i);
                    if (equipid != -1)
                    {
                        int equipNum = equipShop.GetGetNumbyIndex(i);
                        Tab_Equip equip = TableManager.GetEquipByID(equipid);
                        LocalDataBase.Instance().AddEquipNum((EquipEnumID)equip.EnumID, equipNum);
                    }
                }
                BoxManager.Instance.ShowPopupMessage(LanguageManger.GetMe().GetWords("L_S014"));
                //Umeng.GA.Buy("random" + equipShopid, 1, equipShop.CostRuby);
                this.Close();
                return;
            }
            else
            {
                BoxManager.Instance.ShowMessage(LanguageManger.GetMe().GetWords("L_1004"));
//                UIEventListener.Get(BoxManager.Instance.buttonOk).onClick += delegate(GameObject go)
//                {
//                    PageManager.Instance.OpenPage("ShopController", "shopType=" + (int)ShopType.Zhuanshi);
//                };
            }

        }

        //this.Close();
    }

    public void CancleBtn()
    {
        this.Close();
    }

}
