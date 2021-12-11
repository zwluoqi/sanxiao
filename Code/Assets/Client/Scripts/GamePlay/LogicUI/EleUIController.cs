using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GCGame.Table;


public class EleUIController :MonoBehaviour
{
    private static EleUIController instance;

    public static EleUIController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = SceneManager.Instance.eleController;
            }
            if (instance == null)
            {
                instance = new GameObject("EleUIController").AddComponent<EleUIController>();
            }
            return instance;
        }

    }

    public UIAtlas[] atlasOfPage;
    public UITexture[] texturesOfPage;



	//UI
	public Transform squareSpace;
	public GameObject nomatch;
    public EleTextEffect eleTextEffect;
    public UISlider scoreSlider;
    public UILabel score;
	public UILabel 	moveNumber;
	public UILabel  goldNumber;
	public UILabel copyType;
	public UILabel copyLevel;
	public List<UIMissionView> m_MisIcon = new List<UIMissionView>();
	public GameObject targetScore;
	public GameObject flyStartPos;
	public UIEquipController equipController;
    public CopyText copyWord;
    public MissionAnimation missionAnimation;
    public PreMission preMissionAniamtion;
    public MieSheepAnimation mieSheepAnimation;
    public LastTip lastTip;
    public Animation pauseBtnAnimation;
    //public GameObject LoadObj;

	//DATA
	//时间或步数;
	public int limitAmount{
		set{MissionManager.Instance.limitAmount = value > 0?value:0;UpdateMoveStepUI();}
		get{return MissionManager.Instance.limitAmount;}
	}

	public delegate void StartCoroutineDelegateFun();

	private UIEliminateItemView selectedItemCache;
	public UIEliminateItemView selectedItem{
		get{return selectedItemCache;}
		set{			
            if(selectedItemCache != null){
				selectedItemCache.OnSelected(false);
			}
			selectedItemCache = value;
			if(selectedItemCache != null){
				selectedItemCache.OnSelected(true);
			}
            SoundEffect.Instance.PlaySound(SoundEffect.touch);
		}
	}
	public UIEliminateItemView targetItem{get;set;}
    private UIEquipView m_selectedEquipView;
    public UIEquipView selectedEquipView { get { return m_selectedEquipView; } set { m_selectedEquipView = value; equipController.euipbg.SetActive(value != null); } }

    private UIFlyManager equipFlyManager;
    private UIFlyManager missionFlyManager;
    private UIFlyManager dataFlyManager;



    void Update()
    {
        equipFlyManager.Update(Time.deltaTime);
        missionFlyManager.Update(Time.deltaTime);
        dataFlyManager.Update(Time.deltaTime);
    }

	public void ResetSelect(){
		selectedEquipView = null;
		targetItem = null;
		selectedItem = null;
	}

    public void EnterGame()
    {
        equipFlyManager = new UIFlyManager();
        missionFlyManager = new UIFlyManager();
        dataFlyManager = new UIFlyManager();
        copyWord.gameObject.SetActive(false);
        ResetSelect();
        missionAnimation.Reset();
        lastTip.Reset();
        EleUIController.Instance.targetScore.SetActive(false);
        for (int i = 0; i < m_MisIcon.Count; i++)
        {
            m_MisIcon[i].Init(-1, 0);
            m_MisIcon[i].gameObject.SetActive(false);
        }
    }

    public void ExitGame()
    {

        missionAnimation.Reset();
        lastTip.Reset();
        EleUIController.Instance.targetScore.SetActive(false);
        for (int i = 0; i < m_MisIcon.Count; i++)
        {
            m_MisIcon[i].Init(-1, 0);
            m_MisIcon[i].gameObject.SetActive(false);
        }
    }

    public void RestoreTexturesAssets()
    {
        if (texturesOfPage != null)
        {
            texturesOfPage[0].mainTexture = Resources.Load("Texture/Background52") as Texture;
        }
    }

    public void UnLoadAssets()
    {
        foreach (UIAtlas at in atlasOfPage)
        {
            Resources.UnloadAsset(at.spriteMaterial.mainTexture);
        }
        foreach (UITexture ut in texturesOfPage)
        {
            Resources.UnloadAsset(ut.mainTexture);
        }
    }


    public void PlayMissionAnimation()
    {
        missionAnimation.PlayMoveTop();
    }

    public void PlayPreMissionAnimation()
    {
        preMissionAniamtion.Init();
        preMissionAniamtion.PlayPreMissionAnimation();
    }

    public void PlayMieSheepAnimation()
    {
        mieSheepAnimation.Init();
        mieSheepAnimation.PlayAnimation();
    }

    public void PlayLastTip()
    {
        lastTip.PlayAnimation(limitAmount);
    }


	public void InitEquipView(CopyType mode){
		equipController.InitEquipView(mode);
	}

	
	public void AnimationNoMatch(float duration){
		TweenPosition tween = nomatch.AddMissingComponent<TweenPosition>();
		
		tween.duration = duration;
		tween.from = new Vector3(1000,0,0);
		tween.to = new Vector3(-1000,0,0);
		
		tween.animationCurve = new AnimationCurve(new Keyframe(0,0),
													new Keyframe(0.3f,0.5f),
													new Keyframe(0.7f,0.5f),
													new Keyframe(1,1));
		tween.ResetToBeginning();
		tween.PlayForward();
	}
	
	//更新分数,任务图标;
	public  void UpdateScoreUI(){
		int completedScore = MissionManager.Instance.completedScore;
		int maxScore = LevelData.maxScore;

		scoreSlider.value = completedScore*1.0f/maxScore;
        score.text = completedScore.ToString();
	}

    public void UpdateMissionUI()
    {
        for (int i = 0; i < m_MisIcon.Count; i++)
        {
            if (m_MisIcon[i].m_ntype == -1)
                continue;
            Mission tmp = MissionManager.Instance.GetMissionByID(m_MisIcon[i].m_ntype);
            if (tmp != null)
            {
                m_MisIcon[i].SetScore(tmp.amount);
            }
            else
            {
                Debug.LogWarning("mission error");
            }
        }
    }
	
	
	//更新步数;
	public void UpdateMoveStepUI(){
		moveNumber.text = MissionManager.Instance.limitAmount.ToString() ;
	}
	
	//更新装备数量
	public void UpdateEquipNumUI(){
		equipController.UpdateEquipNumUI();
	}

    public void UpdateDataUI()
    {
		//更新金币数量;
		goldNumber.text = EliminateLogic.Instance.GetEliminatePlayer().getGoldNum.ToString();
    }




	public GameObject GetTheEquipObject(EquipEnumID eType){
		return equipController.GetTheEquipObject(eType);
	}
	public void OnPause(){
        pauseBtnAnimation.Play("PauseMove");
        if (EliminateLogic.Instance.GetEliminatePlayer()
           .GetEliminateProcedureManager().GetActiveProcedure().GetProcedureType() != EliminateProcedureType.PROCEDURE_WAITING_INPUT)
            return;
		PageManager.Instance.OpenPage("PauseController","");
	}

	public Vector3 GetMissionIconWolrdPos(int missionType){
		Vector3 pos = Vector3.zero;
		List<UIMissionView> m_MisIcon = EleUIController.Instance.m_MisIcon;
		for (int i = 0; i < m_MisIcon.Count; i++)
		{
			if (m_MisIcon[i].m_ntype == (int)missionType)
			{
				pos = m_MisIcon[i].m_sprite.transform.position;
				break;
			}
		}
		return pos;
	}

    public GameObject GetMissionIcon(int missionType)
    {
        List<UIMissionView> m_MisIcon = EleUIController.Instance.m_MisIcon;
        for (int i = 0; i < m_MisIcon.Count; i++)
        {
            if (m_MisIcon[i].m_ntype == (int)missionType)
            {
                return m_MisIcon[i].gameObject;
            }
        }
        return null;
    }

    //任务图标飞行
	public void ProcessMissionFlyItem(Vector3 objPos,Vector3 targetIconWorldPos,string spriteName){
        UIFlyItem addFly = new UIMissionFlyItem(objPos, targetIconWorldPos, spriteName, "Game/FlyMissionItem");

        missionFlyManager.FlyItems.Enqueue(addFly);
	}

    //装备图标飞行
	public void ProcessEquipFlyItem(Vector3 objPos,Vector3 targetIconWorldPos,string spriteName){
        UIFlyItem addFly = new UIEquipFlyItem(objPos, targetIconWorldPos, spriteName, "Game/FlyEquipItem");
        equipFlyManager.FlyItems.Enqueue(addFly);
	}

    //数据图标飞行
    public void ProcessDataFlyItem(Vector3 objPos, Vector3 targetIconWorldPos, string spriteName)
    {
        UIFlyItem addFly = new UIDataFlyItem(objPos, targetIconWorldPos, spriteName, "Game/FlyDataItem");
        dataFlyManager.FlyItems.Enqueue(addFly);
    }
	
	
	public void ShowOneScore(Vector3 objLocalPos,string m_score){
        GameObject scoreObject = WidgetBufferManager.Instance.loadWidget("Game/Score", squareSpace);
		scoreObject.transform.localPosition = objLocalPos;
		UILabel scoreLabel = scoreObject.GetComponentInChildren<UILabel>();
		scoreLabel.depth = UISquareView.fly_depth;
		scoreLabel.text = m_score;
        TweenPosition posTween = scoreObject.GetComponentInChildren<TweenPosition>();
        posTween.ResetToBeginning();
        posTween.PlayForward();
        scoreObject.GetComponent<TimeEventObject>().SetOnTimeFinished(ScorePlayCompleted);
	}

    private void ScorePlayCompleted(GameObject go)
    {
        WidgetBufferManager.Instance.DestroyWidgetObj("Game/Score", go);
    }


    private int currentStoryIndex;
    private List<Tab_Copystory> currentStorys;
    public void ShowStory(List<Tab_Copystory> storys)
    {
        currentStoryIndex = 0;
        currentStorys = storys;
        copyWord.gameObject.SetActive(true);
        copyWord.Show(currentStorys[currentStoryIndex], true);
    }

    public void ShowNextStory()
    {
        currentStoryIndex++;
        if (currentStoryIndex < currentStorys.Count)
        {
            copyWord.Show(currentStorys[currentStoryIndex], true);
        }
        else
        {
            copyWord.gameObject.SetActive(false);
            EliminateLogic.Instance.StartEleCheckHits();
        }
    }


    public void ShowTextEffect(int eleCount)
    {
        string text = "";
        if (eleCount == 9)
        {
            text = "1ge";
        }
        else if (eleCount ==18)
        {
            text = "2ge";
        }
        else if (eleCount ==36)
        {
            text = "3ge";
        }
        else if (eleCount ==54)
        {
            text = "4ge";
        }
        if (!string.IsNullOrEmpty(text))
        {
            eleTextEffect.ShowPopText(text);
        }
    }
}
