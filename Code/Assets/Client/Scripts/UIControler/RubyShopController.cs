using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GCGame.Table;
//using xjgame.message;
//using sanxiao.net;

public class RubyShopController : MonoBehaviour
{

	public UIGrid grid;
	public UILabel rubyNum;
    private string ItemName = "RubyShopItem";
	private List<GameObject> itemObjList = new List<GameObject>();

    public static int current_tab_rubyID;

	void OnEnable(){
		rubyNum.text = LocalDataBase.Instance().GetDataNum(DataType.zhuanshi).ToString();

		Hashtable table = TableManager.GetRubyshop();
		foreach(DictionaryEntry dic in table){
			Tab_Rubyshop powerItem = (Tab_Rubyshop)dic.Value;
			GameObject go = ResourcesManager.Instance.loadWidget(ItemName,grid.transform);
			go.name = dic.Key.ToString();
            RubyShopItemView view = go.GetComponent<RubyShopItemView>();
            view.icon.spriteName = powerItem.SpriteName;

            view.button.name = dic.Key.ToString();
            view.costRMB.text = "￥" + powerItem.CostRMB;
            view.getZhuanshinum.text = powerItem.BaseNum.ToString();
            view.songZhuanshinum.text = powerItem.SongNum.ToString();
            UIEventListener.Get(view.button).onClick = OnBuyItem;
			itemObjList.Add(go);
		}

		grid.repositionNow = true;
	}

	void OnDisable(){

		for(int i=0;i<itemObjList.Count;i++){
			Destroy(itemObjList[i]);
		}
		itemObjList.Clear();
	}

    void Update()
    {
        rubyNum.text = LocalDataBase.Instance().GetDataNum(DataType.zhuanshi).ToString();
    }

    //public void OnCloseBtn(){
    //    PageManager.Instance.CloseCurrentWin();
    //}

	private void OnBuyItem(GameObject go){
        current_tab_rubyID = int.Parse(go.name);

        string productid = RubyShopController.GetProductIDByTabID(current_tab_rubyID);
        if (string.IsNullOrEmpty(productid))
        {
            BoxManager.Instance.ShowMessageTip("本渠道不支持购买砖石");
        }
        else
        {
            Tab_Rubyshop rubyItem = TableManager.GetRubyshopByID(current_tab_rubyID);
            Debug.LogWarning("start buy:" + rubyItem.Detial + " num:" + rubyItem.GetNum);
            SDKObjecty.buyProduct(current_tab_rubyID, RubyShopController.OnBuyRubyLocal);
            Invoke("HideNetWork", 2);
        }
	}

    void HideNetWork()
    {
        SceneManager.Instance.NetWorkBox.SetActive(false);
    }

    public static void OnBuyRubyLocal(int tab_rubyShopID){
        Tab_Rubyshop rubyItem = TableManager.GetRubyshopByID(tab_rubyShopID);
        if (rubyItem != null)
        {
            Debug.LogWarning("buy:" + rubyItem.Detial + " num:" + rubyItem.GetNum);

            LocalDataBase.Instance().AddDataNum(DataType.zhuanshi, rubyItem.GetNum);
            BoxManager.Instance.ShowPopupMessage(string.Format(
                LanguageManger.GetMe().GetWords("SHOP_003")
                , rubyItem.GetNum));


			Tab_ChargeGift data = TableManager.GetChargeGiftByID(0);
			if(data != null && !LocalDataBase.Instance().HadGetChargeGift()){
				for(int i=0;i<10;i++){
					if(data.GetEquipidbyIndex(i) <0){
							break;
					}
					int equipID = data.GetEquipidbyIndex(i);
					Tab_Equip equip = TableManager.GetEquipByID(equipID);
										LocalDataBase.Instance().AddEquipNum((EquipEnumID)equip.EnumID, data.GetGetNumbyIndex(i));
				}
								LocalDataBase.Instance().AddDataNum(DataType.zhuanshi,data.GetzuanshiNUM);
								LocalDataBase.Instance().AddDataNum(DataType.power,data.GetPowerNUM);

			}
			LocalDataBase.Instance().SetChargeNum(rubyItem.GetNum);
			LocalDataBase.Instance().SetHadGetChargeGift();

            SoundEffect.Instance.PlaySound(SoundEffect.buySuccess);
            //Umeng.GA.Pay(rubyItem.CostRMB, (Umeng.GA.PaySource)SystemConfig.Instance.platformID, tab_rubyShopID.ToString(), 1, rubyItem.GetNum);

        }
    }

//    public static void BuyRubyMessage(int tab_rubyShopID,string reciever, UIListener.OnReceive funReceive)
//    {
//        current_tab_rubyID = tab_rubyShopID;
//        CSBuyZhuanshi msg = (CSBuyZhuanshi)PacketDistributed.CreatePacket(MessageID.CSBuyZhuanshi);
//        msg.Tab_rubyshopid = tab_rubyShopID;
//        msg.ZhuanshuNum = LocalDataBase.Instance().GetDataNum(DataType.zhuanshi);
//        msg.PowerNum = LocalDataBase.Instance().GetDataNum(DataType.power);
//        msg.JinbiNum = LocalDataBase.Instance().GetDataNum(DataType.jinbi);
//        NetworkSender.Instance().send(funReceive, msg, true);
//    }
//
//    public static void BuyRubyCallBack(bool success, PacketDistributed packet)
//    {
//        if (success)
//        {
//            SCBuyZhuanshi msg = (SCBuyZhuanshi)packet;
//            LocalDataBase.Instance().SetDataNum(DataType.power, msg.PowerNum);
//            LocalDataBase.Instance().SetDataNum(DataType.zhuanshi, msg.ZhuanshuNum);
//            LocalDataBase.Instance().SetDataNum(DataType.jinbi, msg.JinbiNum);
//
//            Tab_Rubyshop rubyItem = TableManager.GetRubyshopByID(current_tab_rubyID);
//            BoxManager.Instance.ShowMessageTip(string.Format(
//                LanguageManger.GetMe().GetWords("SHOP_003")
//                , rubyItem.GetNum));
//
//            SoundEffect.Instance.PlaySound(SoundEffect.buySuccess);
//        }
//    }

    public static string GetProductIDByTabID(int tab_id)
    {
        string produceID = "";
        foreach (KeyValuePair<int, string> obj in LocalDataBase.idens)
        {
            if (obj.Key == tab_id)
            {
                produceID = obj.Value;
                break;
            }
        }
        return produceID;
    }

    public static int GetTabIDByProductID(string produceid)
    {

        int tab_rubyID = -1;
        foreach (KeyValuePair<int, string> obj in LocalDataBase.idens)
        {
            if (obj.Value == produceid)
            {
                tab_rubyID = obj.Key;
                break;
            }
        }
        return tab_rubyID;

    }


}
