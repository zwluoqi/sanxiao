using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIEquipController : MonoBehaviour {

	public List<GameObject> equips = new List<GameObject>(); 
	public GameObject TwoMenu;
	public GameObject BaseMenu;
	public GameObject left;
	public GameObject right;
	public GameObject desTime;
    public GameObject euipbg;

	void OnEnable(){
		TwoMenu.SetActive(false);
		BaseMenu.SetActive(true);
	}

    public void InitEquipView(CopyType mode)
    {
		foreach(GameObject go in equips){
			UIEquipView view = go.GetComponent<UIEquipView>();
			view.Init(OnUseEquip);
		}
		TwoMenu.SetActive(false);
		UIEventListener.Get(left).onClick = OnClickTwoMenu;
		UIEventListener.Get(right).onClick = OnClickTwoMenu;
		UIEventListener.Get(desTime).onClick = DesStepEquip;
	}

	public void DesStepEquip(GameObject go){
		if (LevelData.type == CopyType.TimeLimit)
			return ;			
		EquipManager.Instance.UsedesStep();
	}

    private void OpenShop(GameObject go)
    {
        PageManager.Instance.OpenPage("ShopController", "shopType=" + ShopType.Equip);
    }

	public void OnUseEquip(GameObject go){
        #region guild
        if (LocalDataBase.equipGuild)
        {
            LocalDataBase.equipWaitInput++;
        }
        #endregion

		UIEquipView view = go.GetComponent<UIEquipView>();
        if (EleUIController.Instance.selectedEquipView != null && EleUIController.Instance.selectedEquipView.enumID == view.enumID)
        {
            CancleEquipSelect();
            return;
        }

		EleUIController.Instance.ResetSelect();
		if(view.showedNum <= 0 ){
            PageManager.Instance.OpenPage("BuyEquipEnsure", "buyEquipId=" + view.tab_equip.Equipshopid+"&type=1");
			Debug.LogWarning("equip not enough");	
			return;
		}
		switch(view.enumID){
		case EquipEnumID.AddStep:
                if (LevelData.type == CopyType.TimeLimit)
				return ;
			EquipManager.Instance.UseStepEquip();
            view.UpdateNumberLabel();
			break;
		case EquipEnumID.AddTime:
            if (LevelData.type == CopyType.MoveLimit)
				return ;					
			EquipManager.Instance.Useadd_time_30s();
            view.UpdateNumberLabel();
			break;
		case EquipEnumID.Bomb_SameCor:
		case EquipEnumID.Hammer:
		case EquipEnumID.Exchange:
        case EquipEnumID.BomEffect:
			if(EliminateLogic.Instance.GetEliminatePlayer()
			   .GetEliminateProcedureManager().GetActiveProcedure().GetProcedureType() != EliminateProcedureType.PROCEDURE_WAITING_INPUT)
				return ;
			EleUIController.Instance.selectedEquipView = view;
			break;
		case EquipEnumID.RowColEliminate:
			TwoMenu.SetActive(true);
			BaseMenu.SetActive(false);
			EleUIController.Instance.selectedEquipView = left.GetComponent<UIEquipView>();//默认选择左边;
			left.transform.Find("high").gameObject.SetActive(true);
			right.transform.Find("high").gameObject.SetActive(false);
			break;
		case EquipEnumID.ResetItem:
			if(EliminateLogic.Instance.GetEliminatePlayer()
			   .GetEliminateProcedureManager().GetActiveProcedure().GetProcedureType() != EliminateProcedureType.PROCEDURE_WAITING_INPUT)
				return ;
			EquipManager.Instance.UseResetItem();
            view.UpdateNumberLabel();
			break;
		case EquipEnumID.desStep:
            if (LevelData.type == CopyType.TimeLimit)
				return ;			
			EquipManager.Instance.UsedesStep();
            view.UpdateNumberLabel();
            break;
		}
	}

	public void UpdateEquipNumUI(){
		
		for(int i=0;i<equips.Count;i++){
			UIEquipView view = equips[i].GetComponent<UIEquipView>();
			view.UpdateNumberLabel();
		}
		
	}

	public GameObject GetTheEquipObject(EquipEnumID eType){
		foreach(GameObject go in equips){
			UIEquipView view  = go.GetComponent<UIEquipView>();
			if(view.enumID == eType){
				return go;
			}
		}
		return null;
	}
	

	public void OnClickTwoMenu(GameObject go)
    {
        #region guild
        if (LocalDataBase.equipGuild)
        {
            LocalDataBase.equipWaitInput++;
        }
        #endregion

        UIEquipView view = go.GetComponent<UIEquipView>();
		if(EliminateLogic.Instance.GetEliminatePlayer()
		   .GetEliminateProcedureManager().GetActiveProcedure().GetProcedureType() != EliminateProcedureType.PROCEDURE_WAITING_INPUT)
			return ;
		EleUIController.Instance.selectedEquipView = view;
		if(view.gameObject == left){
			left.transform.Find("high").gameObject.SetActive(true);
			right.transform.Find("high").gameObject.SetActive(false);		
		}else{
			left.transform.Find("high").gameObject.SetActive(false);
			right.transform.Find("high").gameObject.SetActive(true);			}

	}

	public void UsedCompleted(){
		TwoMenu.SetActive(false);
		BaseMenu.SetActive(true);
	}

	public void CancleEquipSelect(){
		TwoMenu.SetActive(false);
		BaseMenu.SetActive(true);
		EleUIController.Instance.ResetSelect();
	}


    private int counter;

    public void OnClickTestEquip()
    {
        counter++;
        if (counter > 10)
        {
            desTime.SetActive(true);
            this.gameObject.SetActive(true);
        }
    }
}
