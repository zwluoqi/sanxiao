using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GCGame.Table;

public class UIEquipView : MonoBehaviour {
	
	public UISprite sprite;
	public UILabel numlabel;
	public int showedNum{get;set;}
	public int equipNum{
		get{
			return LocalDataBase.Instance().GetEquipNum(enumID);
		}
	}
	public EquipEnumID enumID;
	public EquipEffectType effectType;
	public Tab_Equip tab_equip;

	public void Init(UIEventListener.VoidDelegate callBack){
		Hashtable hashTable = TableManager.GetEquip();
		foreach(DictionaryEntry dic in hashTable){
			Tab_Equip tmp = (Tab_Equip)dic.Value;
			if(tmp.EnumID == (int)enumID){
				tab_equip = tmp;
				break;
			}
		}
		if(tab_equip != null){
			UIEventListener.Get(gameObject).onClick = callBack;
			sprite.spriteName = tab_equip.SpriteName;
			UpdateNumberLabel();
		}
	}
	

	
	public void UpdateNumberLabel(){
		int equipTmpNum = LocalDataBase.Instance().GetEquipTmpNum(enumID);
		if(equipTmpNum > 0){
            numlabel.color = Color.blue;
			numlabel.text = equipTmpNum.ToString();
			showedNum = equipTmpNum;
		}else{
			int equipNum = LocalDataBase.Instance().GetEquipNum(enumID);
            numlabel.color = Color.white;
			numlabel.text = equipNum.ToString();
			showedNum = equipNum;
		}
	}

}
