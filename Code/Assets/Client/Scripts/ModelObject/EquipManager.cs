using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EquipManager{

    private static EquipManager instance;

    public static EquipManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EquipManager();
            }
            return instance;
        }
    }



	public void UseStepEquip(){
		if(LocalDataBase.Instance().DecreaseEquipNum(EquipEnumID.AddStep,1)){
			EventManager.Instance.Fire(EventDefine.Step);
		}else{
			
		}
	}
	
	public void UsedesStep(){
		EventManager.Instance.Fire(EventDefine.desStep);
	}
	
	public void Useadd_time_30s(){
		if(LocalDataBase.Instance().DecreaseEquipNum(EquipEnumID.AddTime,1)){
			EventManager.Instance.Fire(EventDefine.add_time_30s);
		}else{
			
		}
	}	
	
	public void UseBomb_SameCor(UIEliminateItemView item){
		if(LocalDataBase.Instance().DecreaseEquipNum(EquipEnumID.Bomb_SameCor,1)){
			EventManager.Instance.Fire(EventDefine.Bomb_SameCor,item);
		}else{
			
		}
	}
	
	public void UseResetItem(){
		if(LocalDataBase.Instance().DecreaseEquipNum(EquipEnumID.ResetItem,1)){
			EventManager.Instance.Fire(EventDefine.ResetItem);
		}else{
			
		}
	}
	
	public void UseHammer(UIEliminateItemView item){
		if(LocalDataBase.Instance().DecreaseEquipNum(EquipEnumID.Hammer,1)){
			EventManager.Instance.Fire(EventDefine.Hammer,item);
		}else{
			
		}
	}
	
	public void UseBombRow(UIEliminateItemView item){
		if(LocalDataBase.Instance().DecreaseEquipNum(EquipEnumID.RowColEliminate,1)){
			EventManager.Instance.Fire(EventDefine.BombRow,item);
		}else{
			
		}
	}
	
	public void UseBombCol(UIEliminateItemView item){
		if(LocalDataBase.Instance().DecreaseEquipNum(EquipEnumID.RowColEliminate,1)){
			EventManager.Instance.Fire(EventDefine.BombCol,item);
		}else{
			
		}
	}	

	public void UseExchangeEle(UIEliminateItemView one,UIEliminateItemView other){
		if(LocalDataBase.Instance().DecreaseEquipNum(EquipEnumID.Exchange,1)){
			EventManager.Instance.Fire(EventDefine.Exchange,one,other);
		}else{
			
		}
	}

    public void UseBomEffect(UIEliminateItemView item)
    {
        if (LocalDataBase.Instance().DecreaseEquipNum(EquipEnumID.BomEffect, 1))
        {
            EventManager.Instance.Fire(EventDefine.BombEffect, item);
        }
        else
        {

        }
    }

    public float PlayAnimation(EquipEffectType effectType,Transform target)
    {
        GameObject obj = null;
        if (effectType == EquipEffectType.Hammer)
        {
            obj = WidgetBufferManager.Instance.loadWidget("Game/SaoZiAnimationEquip", target.parent);
            obj.transform.localPosition = target.localPosition;
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<EquipUseEffect>().prefabName = "Game/SaoZiAnimationEquip";
            obj.GetComponent<EquipUseEffect>().PlayEffect();
        }
        else if (effectType == EquipEffectType.BombCol)
        {
            obj = WidgetBufferManager.Instance.loadWidget("Game/HuoJianShuAnimationEquip", target.parent);
            obj.transform.localPosition = target.localPosition;
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<EquipUseEffect>().prefabName = "Game/HuoJianShuAnimationEquip";
            obj.GetComponent<EquipUseEffect>().PlayEffect();
        }
        else if (effectType == EquipEffectType.BombRow)
        {
            obj = WidgetBufferManager.Instance.loadWidget("Game/HuoJianHengAnimationEquip", target.parent);
            obj.transform.localPosition = target.localPosition;
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<EquipUseEffect>().prefabName = "Game/HuoJianHengAnimationEquip";
            obj.GetComponent<EquipUseEffect>().PlayEffect();
        }
        else if (effectType == EquipEffectType.BomEffect)
        {
            obj = WidgetBufferManager.Instance.loadWidget("Game/BomAnimationEquip", target.parent);
            obj.transform.localPosition = target.localPosition;
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<EquipUseEffect>().prefabName = "Game/BomAnimationEquip";
            obj.GetComponent<EquipUseEffect>().PlayEffect();
        }

        return obj.GetComponent<EquipUseEffect>().effectDuration;

    }
}
