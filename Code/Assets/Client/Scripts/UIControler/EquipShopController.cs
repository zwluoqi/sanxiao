using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GCGame.Table;
//using xjgame.message;
//using sanxiao.net;

public class EquipShopController : MonoBehaviour
{
    public enum EquipShopType
    {
        Normal =1,
        Libao =2,
    }

    public EquipShopType shopType = EquipShopType.Normal;
	public UIGrid grid;
	private string ItemName = "EquipShopItem";
	private List<GameObject> itemObjList = new List<GameObject>();

	public UILabel rubyNum;



	void OnEnable(){
		rubyNum.text = LocalDataBase.Instance().GetDataNum(DataType.zhuanshi).ToString();
		Hashtable table = TableManager.GetEquipshop();
		foreach(DictionaryEntry dic in table){
			Tab_Equipshop eqiopitem = (Tab_Equipshop)dic.Value;
            if (eqiopitem.ShopType != (int)shopType)
                continue;
			GameObject go = ResourcesManager.Instance.loadWidget(ItemName,grid.transform);
			go.name = dic.Key.ToString();
            EquipShopItemView view = go.GetComponent<EquipShopItemView>();
            view.icon.spriteName = eqiopitem.SpriteName;
            view.button.name = dic.Key.ToString();
            view.costRuby.text = eqiopitem.CostRuby.ToString();
            UIEventListener.Get(go).onClick = OnBuyItem;
			itemObjList.Add(go);
		}
		grid.repositionNow = true;

        #region guild
        if (LocalDataBase.equipGuild && LocalDataBase.equipbuy == 2)
        {

            TipWord tip = new TipWord(LanguageManger.GetMe().GetWords("L_daoju_tishi2"), new Vector3(0, -400, 0));
            Queue<TipWord> tips = new Queue<TipWord>();
            tips.Enqueue(tip);
            GuideManager.Instance.ShowGuildTip(tips);
            GuideManager.Instance.Guildtip.GetComponent<TipGuide>().lastWordTipOnClick = SlowShowGuild;
        }
        #endregion
	}

    void SlowShowGuild()
    {
        LocalDataBase.equipbuy++;
        GuideManager.Instance.ShowForceGuide(itemObjList[0],
            false, LanguageManger.GetMe().GetWords("L_daoju_tishi3"), new Vector3(0,-400,0));
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
        GuideManager.Instance.HideGuide();
        int currentBuyID = int.Parse(go.name);
        PageManager.Instance.OpenPage("BuyEquipEnsure", "buyEquipId="+currentBuyID+"&type=0");

	}

}
