using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GCGame.Table;
//using xjgame.message;
//using sanxiao.net;

public class TiliShopController : MonoBehaviour {

	public UIGrid grid;
	public UILabel rubyNum;

	private string ItemName = "TiliShopItem";
	private List<GameObject> itemObjList = new List<GameObject>();

    private int current_tab_powerID;

	void OnEnable(){
		rubyNum.text = LocalDataBase.Instance().GetDataNum(DataType.zhuanshi).ToString();

		Hashtable table = TableManager.GetPowershop();
		foreach(DictionaryEntry dic in table){
			Tab_Powershop powerItem = (Tab_Powershop)dic.Value;
			GameObject go = ResourcesManager.Instance.loadWidget(ItemName,grid.transform);
			go.name = dic.Key.ToString();
			go.transform.Find("icon").GetComponent<UISprite>().spriteName = powerItem.SpriteName;
			GameObject button = go.transform.Find("button").gameObject;
			button.name = dic.Key.ToString();
			button.transform.Find("num").GetComponent<UILabel>().text = powerItem.CostRuby.ToString();
            UIEventListener.Get(go).onClick += OnBuyItem;
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

    //public void OnCloseBtn(){
    //    PageManager.Instance.CloseCurrentWin();
    //}

	private void OnBuyItem(GameObject go){
        current_tab_powerID = int.Parse(go.name);
        Tab_Powershop powerItem = TableManager.GetPowershopByID(current_tab_powerID);
        //Debug.LogWarning("buy:"+powerItem.Detial+" num:"+powerItem.GetNum);
		if(LocalDataBase.Instance().GetDataNum(DataType.zhuanshi) < powerItem.CostRuby){
			BoxManager.Instance.ShowMessage(LanguageManger.GetMe().GetWords("L_1004"));
            UIEventListener.Get(BoxManager.Instance.buttonOk).onClick += OnZhuanshiShopCall;
			return ;
		}

        OnBuyItemLocal(current_tab_powerID);

        //BuyPowerMessage(current_tab_powerID, BuyPowerCallBack);

	}

    private void OnZhuanshiShopCall(GameObject go)
    {
        OnBuyZhuanshiBtn();
    }
    private void OnBuyZhuanshiBtn()
    {
//        PageManager.Instance.OpenPage("ShopController", "shopType="+(int)ShopType.Zhuanshi);
    }

    private void OnBuyItemLocal(int tab_id)
    {
        Tab_Powershop powerItem = TableManager.GetPowershopByID(tab_id);
        Debug.LogWarning("buy:" + powerItem.Detial + " num:" + powerItem.GetNum);

        LocalDataBase.Instance().DecreaseDataNum(DataType.zhuanshi, powerItem.CostRuby);
        LocalDataBase.Instance().AddDataNum(DataType.power, powerItem.GetNum);
        BoxManager.Instance.ShowPopupMessage(string.Format(
            LanguageManger.GetMe().GetWords("SHOP_001")
            , powerItem.GetNum));

        rubyNum.text = LocalDataBase.Instance().GetDataNum(DataType.zhuanshi).ToString();
        SoundEffect.Instance.PlaySound(SoundEffect.buySuccess);

        //Umeng.GA.Buy(powerItem.Detial, 1, powerItem.CostRuby);
    }


//    private void BuyPowerMessage(int tab_powerShopID, UIListener.OnReceive funReceive)
//    {
//        CSBuyPower msg = (CSBuyPower)PacketDistributed.CreatePacket(MessageID.CSBuyPower);
//        msg.Tab_powershopid = tab_powerShopID;
//        msg.ZhuanshuNum = LocalDataBase.Instance().GetDataNum(DataType.zhuanshi);
//        msg.PowerNum = LocalDataBase.Instance().GetDataNum(DataType.power);
//        msg.JinbiNum = LocalDataBase.Instance().GetDataNum(DataType.jinbi);
//        NetworkSender.Instance().send(funReceive, msg, true);
//    }
//
//    private void BuyPowerCallBack(bool success, PacketDistributed packet)
//    {
//        if (success)
//        {
//            SCBuyPower msg = (SCBuyPower)packet;
//            LocalDataBase.Instance().SetDataNum(DataType.power, msg.PowerNum);
//            LocalDataBase.Instance().SetDataNum(DataType.zhuanshi, msg.ZhuanshuNum);
//            LocalDataBase.Instance().SetDataNum(DataType.jinbi, msg.JinbiNum);
//
//            Tab_Powershop powerItem = TableManager.GetPowershopByID(current_tab_powerID);
//            BoxManager.Instance.ShowMessageTip(string.Format(
//                LanguageManger.GetMe().GetWords("SHOP_001")
//                , powerItem.GetNum));
//
//            rubyNum.text = LocalDataBase.Instance().GetDataNum(DataType.zhuanshi).ToString();
//            SoundEffect.Instance.PlaySound(SoundEffect.buySuccess);
//        }
//    }

}
