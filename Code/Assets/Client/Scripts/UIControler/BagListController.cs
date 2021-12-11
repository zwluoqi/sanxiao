using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GCGame.Table;

public class BagListController : Page {

	public UIGrid grid;
	public UILabel rubyNum;
	private string ItemName = "BagItem";
	private List<GameObject> itemObjList = new List<GameObject>();


	protected override void DoOpen()
{

		Hashtable table = TableManager.GetEquip();
		foreach(DictionaryEntry dic in table){
			Tab_Equip equipItem = (Tab_Equip)dic.Value;
			if(LocalDataBase.Instance().GetEquipNum((EquipEnumID) equipItem.EnumID) > 0){
				GameObject go = ResourcesManager.Instance.loadWidget(ItemName,grid.transform);
				go.name = dic.Key.ToString();
				go.transform.Find("icon").GetComponent<UISprite>().spriteName = equipItem.SpriteName;
				go.transform.Find("num").GetComponent<UILabel>().text = "X"+LocalDataBase.Instance().GetEquipNum((EquipEnumID) equipItem.EnumID);

				itemObjList.Add(go);
			}
		}
		grid.repositionNow = true;

        #region guild
        if (LocalDataBase.equipGuild && LocalDataBase.equipbuy == 6)
        {
            LocalDataBase.equipbuy++;
            GuideManager.Instance.ShowGuideTarget(itemObjList[0].GetComponent<UIWidget>(),
                false, LanguageManger.GetMe().GetWords("L_daoju_tishi6"), Vector3.zero);
            UIEventListener.Get(GuideManager.Instance.m_GuildTarget.gameObject).onClick = OnEquipGuildCallback;
        }
        #endregion
	}

    private void OnEquipGuildCallback(GameObject go)
    {
        this.Close();
        PageManager.Instance.OpenPage("CopyBeforeController", "");
    }

	protected override void DoClose()
    {

		for(int i=0;i<itemObjList.Count;i++){
			Destroy(itemObjList[i]);
		}
		itemObjList.Clear();
	}

	public void OnCloseBtn(){
        this.Close();
	}
	
}
