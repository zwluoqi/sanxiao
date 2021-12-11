using UnityEngine;
using System.Collections;
using GCGame.Table;
//using xjgame.message;
using System.Collections.Generic;
//using sanxiao.net;


public class BuyEquipEnsure :Page {


    public UILabel castRuby;
    public UILabel buysNum;
    public UILabel itemDetail;
    public UILabel itemName;
    public UISprite itemIcon;
    public GameObject buyBtn;
    
    private Tab_Equipshop currentBuy;



    private int buyNum;
    private int currentBuyID;
    private int openType;
    protected override void DoOpen()
    {
        currentBuyID = int.Parse(options["buyEquipId"]);
        openType = int.Parse(options["type"]);
        currentBuy = TableManager.GetEquipshopByID(currentBuyID);
        Debug.LogWarning("buy:" + currentBuy.Detial);
        ShowBuyBox();

        #region guild
        if (LocalDataBase.equipGuild && LocalDataBase.equipbuy == 3)
        {
            LocalDataBase.equipbuy++;
            GuideManager.Instance.ShowForceGuide(buyBtn,
                true, LanguageManger.GetMe().GetWords("L_daoju_tishi4"), new Vector3(0,-400,0));
        }
        #endregion
    }

    protected override void DoClose()
    {
        base.DoClose();
    }

    private void ShowBuyBox()
    {
        buysNum.text = "1";
        castRuby.text = currentBuy.CostRuby.ToString();
        itemDetail.text = currentBuy.Detial;
        itemIcon.spriteName = currentBuy.SpriteName;
        itemName.text = currentBuy.GiftName;
    }

    public void OnBuyBoxItems()
    {
        buyNum = int.Parse(buysNum.text);
        int cost = buyNum * currentBuy.CostRuby;
        if (LocalDataBase.Instance().GetDataNum(DataType.zhuanshi) < cost)
        {
//            if (openType == 0)
//            {
//                BoxManager.Instance.ShowMessage(LanguageManger.GetMe().GetWords("L_1004"));
//                UIEventListener.Get(BoxManager.Instance.buttonOk).onClick += OnZhuanshiShopCall;
//
//            }
//            else if (openType == 1)
//            {
//                BoxManager.Instance.ShowPopupMessage(LanguageManger.GetMe().GetWords("L_1020"));
//                int spe_tab_rubyId = 2;
//                string productid = RubyShopController.GetProductIDByTabID(spe_tab_rubyId);
//                if (string.IsNullOrEmpty(productid))
//                {
//                    this.Close();
//                }
//                else
//                {
//                    PageManager.Instance.OpenPage("SpeRubyPage", "tab_rubyID=" + spe_tab_rubyId);//特定的某个砖石商品(200砖石)
//                }
//            }
						BoxManager.Instance.ShowPopupMessage("钻石不足");
            return;
        }

        BuyEquipLocal();
        //BuyEquipMessage(currentBuyID, buyNum, BuyEquipCallBack);
    }

    public void AddItem()
    {
        int num = int.Parse(buysNum.text);
        num++;
        buysNum.text = num.ToString();
        castRuby.text = (currentBuy.CostRuby * num).ToString();
    }

    public void DesItem()
    {

        int num = int.Parse(buysNum.text);
        if (num <= 1)
            return;
        num--;
        buysNum.text = num.ToString();
        castRuby.text = (currentBuy.CostRuby * num).ToString();
    }

    private void BuyEquipLocal()
    {
        LocalDataBase.Instance().DecreaseDataNum(DataType.zhuanshi, currentBuy.CostRuby * buyNum);
        for (int i = 0; i < 10; i++)
        {
            int id = currentBuy.GetEquipidbyIndex(i);
            int num = currentBuy.GetGetNumbyIndex(i);
            if (id == -1)
            {
                break;
            }
            else
            {
                Tab_Equip tab_equip = TableManager.GetEquipByID(id);
                Debug.LogWarning("buy:" + tab_equip.Detial + " num:" + num);
                LocalDataBase.Instance().AddEquipNum((EquipEnumID)tab_equip.EnumID, num * buyNum);
                //GA.Buy(((EquipEnumID)tab_equip.EnumID).ToString(), num , currentBuy.CostRuby);
            }
        }

        BoxManager.Instance.ShowPopupMessage(string.Format(
            LanguageManger.GetMe().GetWords("SHOP_002")
            , buyNum, currentBuy.Detial));
        SoundEffect.Instance.PlaySound(SoundEffect.buySuccess);

        #region guild
        if (LocalDataBase.equipGuild && LocalDataBase.equipbuy == 4)
        {
            LocalDataBase.equipbuy++;
            GuideManager.Instance.HideGuide();
            PageManager.Instance.CloseAllPage();
        }
        #endregion
    }

//    private void BuyEquipMessage(int tab_equipShopID, int num, UIListener.OnReceive funReceive)
//    {
//        CSBuyShopEquip msg = (CSBuyShopEquip)PacketDistributed.CreatePacket(MessageID.CSBuyShopEquip);
//        List<EquipData> equips = LocalDataBase.Instance().GetLocalEquipData();
//        foreach (EquipData equip in equips)
//        {
//            msg.AddEquips(equip);
//        }
//        msg.Tab_equipshopid = tab_equipShopID;
//        msg.BuyNum = num;
//        msg.ZhuanshuNum = LocalDataBase.Instance().GetDataNum(DataType.zhuanshi);
//        NetworkSender.Instance().send(funReceive, msg, true);
//    }

//    private void BuyEquipCallBack(bool success, PacketDistributed packet)
//    {
//        if (success)
//        {
//            SCBuyShopEquip msg = (SCBuyShopEquip)packet;
//            List<EquipData> equips = new List<EquipData>();
//            foreach (EquipData equip in msg.equipsList)
//            {
//                equips.Add(equip);
//            }
//            LocalDataBase.InitEquipData(equips);
//            LocalDataBase.Instance().SetDataNum(DataType.zhuanshi, msg.ZhuanshuNum);
//            //int cost = buyNum * currentBuy.CostRuby;
//            //LocalDataBase.Instance().DecreaseDataNum(DataType.zhuanshi, cost);
//            BoxManager.Instance.ShowMessageTip(string.Format(
//                LanguageManger.GetMe().GetWords("SHOP_002")
//                , buyNum, currentBuy.Detial));
//
//            SoundEffect.Instance.PlaySound(SoundEffect.buySuccess);
//        }
//    }

    private void OnBuyZhuanshiBtn()
    {
        //ShopController.tabIndex = 1;
//        PageManager.Instance.OpenPage("ShopController", "shopType="+(int)ShopType.Zhuanshi);
    }


    private void OnZhuanshiShopCall(GameObject go)
    {
//        OnBuyZhuanshiBtn();
    }
}
