using UnityEngine;
using System.Collections;
using GCGame.Table;


public class BeforeDefeatedController : Page {
    public GameObject costRuby;
    public GameObject costRMB;
    public UIGrid grid;

	public GameObject[] buttonEquips;
    //public UILabel level;
	public GameObject mission;
	public GameObject score;
    public UISprite title;
    private string titleTime = "zengjia30";
    private string titleStep = "zengjiawubu";

    private int currentTabRubyID = SystemConfig.stepShangping;
	
	public void OnCalcleBtn(){
        this.Close();
		EliminateLogic.Instance.GetEliminatePlayer().GetEliminateProcedureManager().ChangProcedure
			(EliminateProcedureType.PROCEDURE_DEFEATED);
	}
	
	public void OnOkBtn(){

        string productStr = RubyShopController.GetProductIDByTabID(currentTabRubyID);
        SDKObjecty.buyProduct(SystemConfig.stepShangping, BuyTimesOkCostMoney);
        Invoke("HideNetWork", 2);

	}

    private void BuyTimesOkCostMoney(int tabID)
    {
        this.Close();
        EliminateLogic.Instance.GetEliminatePlayer().buy = true;
        if (LevelData.type == CopyType.MoveLimit)
        {
            EventManager.Instance.Fire(EventDefine.Step);
        }
        else
        {
            EventManager.Instance.Fire(EventDefine.add_time_30s);
        }
        EliminateLogic.Instance.StartTimeTick();
        EliminateLogic.Instance.GetEliminatePlayer().GetEliminateProcedureManager().ChangProcedure
            (EliminateProcedureType.PROCEDURE_CHECK_HITS);
    }

    void HideNetWork()
    {
        SceneManager.Instance.NetWorkBox.SetActive(false);
    }

    public void CostRuby()
    {
        int currentRuny = LocalDataBase.Instance().GetDataNum(DataType.zhuanshi);
        if (currentRuny > 29)
        {
            LocalDataBase.Instance().DecreaseDataNum(DataType.zhuanshi, 29);
            BuyTimesOk(currentTabRubyID);
        }
        else
        {
            BoxManager.Instance.ShowPopupMessage(LanguageManger.GetMe().GetWords("L_1020"));
            int spe_tab_rubyId = 2;
            string productid = RubyShopController.GetProductIDByTabID(spe_tab_rubyId);
            if (string.IsNullOrEmpty(productid))
            {
                OnCalcleBtn();
            }
            else
            {
                PageManager.Instance.OpenPage("SpeRubyPage", "tab_rubyID=" + spe_tab_rubyId);//�ض���ĳ��שʯ��Ʒ(200שʯ)
            }
        }
    }
	
	private void BuyTimesOk(int tabID){
        this.Close();
		EliminateLogic.Instance.GetEliminatePlayer().buy = true;
        if (LevelData.type == CopyType.MoveLimit)
        {
            EventManager.Instance.Fire(EventDefine.Step);
        }
        else
        {
            EventManager.Instance.Fire(EventDefine.add_time_30s);
        }
        EliminateLogic.Instance.StartTimeTick();
		EliminateLogic.Instance.GetEliminatePlayer().GetEliminateProcedureManager().ChangProcedure
			(EliminateProcedureType.PROCEDURE_CHECK_HITS);
	}


    protected override void DoOpen()
    {	
        //level.text = LocalDataBase.Instance().GetSelectCopyLevel().ToString();
        if (LevelData.mode == CopyMode.ElE)
        {
            if (LevelData.requestScore <= 0)
            {
                score.SetActive(false);
                mission.SetActive(true);
                if (MissionManager.Instance.IsGetEnoughMission())
                {
                    mission.transform.Find("close").gameObject.SetActive(false);
                    mission.transform.Find("open").gameObject.SetActive(true);
                    mission.transform.Find("Label").GetComponent<UILabel>().text = LanguageManger.GetMe().GetWords("L_1007");
                }
                else
                {
                    mission.transform.Find("close").gameObject.SetActive(true);
                    mission.transform.Find("open").gameObject.SetActive(false);
                    mission.transform.Find("Label").GetComponent<UILabel>().text = LanguageManger.GetMe().GetWords("L_1007");
                }
            }
            else
            {
                score.SetActive(true);
                mission.SetActive(false);
                if (MissionManager.Instance.completedScore > LevelData.requestScore)
                {
                    score.transform.Find("close").gameObject.SetActive(false);
                    score.transform.Find("open").gameObject.SetActive(true);
                    score.transform.Find("Label").GetComponent<UILabel>().text = string.Format(LanguageManger.GetMe().GetWords("L_1006"), LevelData.requestScore);
                }
                else
                {
                    score.transform.Find("close").gameObject.SetActive(true);
                    score.transform.Find("open").gameObject.SetActive(false);
                    score.transform.Find("Label").GetComponent<UILabel>().text = string.Format(LanguageManger.GetMe().GetWords("L_1006"), LevelData.requestScore);
                }
            }
        }
        else
        {
            score.SetActive(false);
            mission.SetActive(true);
            mission.transform.Find("close").gameObject.SetActive(true);
            mission.transform.Find("open").gameObject.SetActive(false);
            mission.transform.Find("Label").GetComponent<UILabel>().text = LanguageManger.GetMe().GetWords("L_1009");
        }


        if (LevelData.type == CopyType.MoveLimit)
        {
            title.spriteName = titleStep;
            currentTabRubyID = SystemConfig.stepShangping;
        }
        else
        {
            title.spriteName = titleTime;
            currentTabRubyID = SystemConfig.addSecondShangping;
        }
        title.MakePixelPerfect();
        string productStr = RubyShopController.GetProductIDByTabID(currentTabRubyID);
        if (string.IsNullOrEmpty(productStr) || LocalDataBase.Instance().GetDataNum(DataType.zhuanshi) > 29)
        {
            costRMB.SetActive(false);
        }
        grid.Reposition();
        grid.repositionNow = true;

	}

}
