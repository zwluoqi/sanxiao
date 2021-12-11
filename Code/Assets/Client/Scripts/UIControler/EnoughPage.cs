using UnityEngine;
using System.Collections;
using GCGame.Table;


public class EnoughPage : Page {

    public UILabel num;
    public UILabel desc;

	int currentLevel = 1;
    protected override void DoOpen()
    {

        currentLevel = LocalDataBase.Instance().GetSelectCopyLevel();
        Tab_Copydetail copy = TableManager.GetCopydetailByID(currentLevel);
        int allstars = LocalDataBase.GetAllStars();
        num.text = allstars + "/" + copy.OpenedLimited;

        desc.text = string.Format(LanguageManger.GetMe().GetWords("L_1066"), copy.OpenedLimited - allstars);

    }

    public void OnGetOtherCopys()
    {
        PageManager.Instance.CloseAllPage();
        PageManager.Instance.OpenPage("CopyAllStarPage", "");
    }

	public void LimitedOpenPage(){

		if(LocalDataBase.copyModels[currentLevel - 1].buyUnLock){
			return ;
		}

		if(LocalDataBase.Instance().GetDataNum(DataType.zhuanshi) < 30){
			BoxManager.Instance.ShowPopupMessage(LanguageManger.GetMe().GetWords("L_1020"));
			PageManager.Instance.OpenPage("ShopController","");
			return ;
		}
		LocalDataBase.Instance().DecreaseDataNum(DataType.zhuanshi,30);
        //GA.Buy("jiesuo", 1, 30);
		LocalDataBase.copyModels[currentLevel - 1].buyUnLock = true;
		LocalDataBase.copyModels[currentLevel - 1].SaveData();
        BoxManager.Instance.ShowPopupMessage(LanguageManger.GetMe().GetWords("L_1100"));
		this.Close();
	}
}
