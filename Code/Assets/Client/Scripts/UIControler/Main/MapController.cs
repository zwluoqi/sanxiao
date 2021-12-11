using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GCGame.Table;
//using sanxiao.net;

public class MapController : MonoBehaviour {

    public UIProgressBar progressBar;
	public UIScrollView scrollView;
    public UICopyItemView copyItemPref;
    public GameObject animationMask;

    public GameObject localTip;
    public GameObject yunTip;
	

    public List<UICopyItemView> copyViewList = new List<UICopyItemView>();
    public List<GameObject> copyParentViewList = new List<GameObject>();
    public List<int> copyParentChildCount = new List<int>();
    private int activityContanerCount = 0;

    public UIAtlas[] atlasOfPage;
    private List<UITexture> texturesOfPage = new List<UITexture>();
    private Dictionary<UITexture, string> texturesOfPageCache = new Dictionary<UITexture, string>();


    public bool isDone { get; set; }
    public float progress { get; set; }


    public IEnumerator GeneroCopyItem2()
    {
        isDone = false;
        progress = 0.1f;

        progress = 0.2f;
        yield return null;

        //获得子对象
        int ContanerCount = scrollView.transform.childCount;
        copyParentViewList = new List<GameObject>();
        copyParentChildCount = new List<int>();
        for (int i = 0; i < ContanerCount; i++)
        {
            Transform childParent = scrollView.transform.GetChild(i);
            childParent.localPosition = new Vector3(0, i * SystemConfig.standard_height, 0);
            copyParentViewList.Add(childParent.gameObject);
            UICopyPosView[] childParentViews = childParent.GetComponentsInChildren<UICopyPosView>(true);
            copyParentChildCount.Add(childParentViews.Length);
        }

        foreach (GameObject parent in copyParentViewList)
        {
            UITexture BG = parent.transform.Find("bg").GetComponent<UITexture>();
            texturesOfPage.Add(BG);
            texturesOfPageCache.Add(BG, BG.mainTexture.name);
        }

        //初始化第一页
        UICopyPosView[] copyPoss = scrollView.GetComponentsInChildren<UICopyPosView>(true);
        List<UICopyPosView> sorttingList = new List<UICopyPosView>();
        sorttingList.AddRange(copyPoss);
        sorttingList.Sort(SortItem);
        int count=0;
        foreach (UICopyPosView pos in sorttingList)
        {
            UICopyItemView itemView = GameObject.Instantiate(copyItemPref) as UICopyItemView;
            itemView.transform.parent = pos.transform;
            itemView.transform.localPosition = Vector3.zero;
            itemView.transform.localScale = Vector3.one;
            copyViewList.Add(itemView);
            progress = 0.2f + (count++ * 0.7f / copyPoss.Length);
            yield return null;
        }


        for (int i = LocalDataBase.copyModels.Count; i < copyViewList.Count; i++)
        {
            copyViewList[i].gameObject.SetActive(false);
        }
        yield return null;

        //更新关卡状态;
        SceneManager.Instance.StartCoroutine(UpdateCopyState());
        isDone = true;
        LocalDataBase.LoadResourceCompleted = true;
        progress = 1;
        yield return 1;

    }

	 public IEnumerator UpdateCopyState(){
         animationMask.SetActive(false);

         //大关卡显示;
         int currentMaxLevel = LocalDataBase.GetCurrentMaxLevel();
         int currentParentIndex = 0;
         int childSumCount = 0;
         for (; currentParentIndex < copyParentChildCount.Count; currentParentIndex++)
         {
             childSumCount += copyParentChildCount[currentParentIndex];
             if (currentMaxLevel < childSumCount)
             {
                 break;
             }
         }
         activityContanerCount = currentParentIndex + 1;
         currentParentIndex += 2;
         if (currentParentIndex-1 < copyParentViewList.Count)
         {
             yunTip.transform.parent = copyParentViewList[currentParentIndex-1].transform;
             yunTip.transform.localPosition = Vector3.zero;
         }
         else
         {
             yunTip.transform.parent = this.transform;
             yunTip.transform.localPosition = new Vector3(5000, 0, 0);
         }
         for (int i = 0; i < copyParentViewList.Count; i++)
         {
             if (i < currentParentIndex)
             {
                 copyParentViewList[i].SetActive(true);
             }
             else
             {
                 copyParentViewList[i].SetActive(false);
             }
         }


         //当前关卡显示
		int currentLevel = LocalDataBase.Instance().GetSelectCopyLevel();
		for(int i=0;i<copyViewList.Count&&i<LocalDataBase.copyModels.Count;i++){
            copyViewList[i].Init(LocalDataBase.copyModels[i]);
            copyViewList[i].name = LocalDataBase.copyModels[i].copyID.ToString();
			if(!copyViewList[i].locked){
				copyViewList[i].gameObject.SetActive(true);
				UIEventListener.Get(copyViewList[i].gameObject).onClick = OnClickCopyButton;
			}else{
				copyViewList[i].gameObject.SetActive(false);
				UIEventListener.Get(copyViewList[i].gameObject).onClick = null;
			}

            Vector3 startLocalPosition = Vector3.zero;
            if (LocalDataBase.copyModels[i].copyID == currentLevel)
            {
                localTip.transform.parent = copyViewList[i].transform;
                localTip.transform.localScale = Vector3.one;
                localTip.transform.localPosition = Vector3.zero;
                if (LevelData.WinGameToUI)
                {
                    startLocalPosition = copyViewList[i].transform.InverseTransformPoint(copyViewList[i - 1].transform.position);
                    localTip.transform.localPosition = startLocalPosition;
                }
            }

		}
        yield return null;
        yield return null;

        Debug.LogWarning("currentLevel:" + currentLevel);
        StartCoroutine(_AdustLocalTip());

	}

     IEnumerator _AdustLocalTip()
     {
         int currentLevel = LocalDataBase.Instance().GetSelectCopyLevel();
         GameObject currentCopyView = copyViewList[currentLevel - 1].gameObject;
         float startTime = 0;
         while (startTime < 2)
         {
             Vector3 screenPoint = SceneManager.Instance.mainCamera.transform.InverseTransformPoint(currentCopyView.transform.position);
             //screenPoint = SceneManager.Instance.mainCamera.ScreenToWorldPoint(screenPoint);
             if(Mathf.Abs( screenPoint .y ) <= 4){
                 break;
             }
             float distance = Mathf.Abs(screenPoint.y);
             float targetY = Mathf.MoveTowards(screenPoint.y, 0, distance/8);
             if (progressBar.value <= 0 && targetY - screenPoint.y < 0)
             {
                 break;
             }
             else if (progressBar.value >= 1 && targetY - screenPoint.y > 0)
             {
                 break;
             }
             else
             {
                 progressBar.value += (targetY - screenPoint.y) / (UIRoot.list[0].manualHeight * activityContanerCount);
                 if (progressBar.value == 0 || progressBar.value == 1)
                 {
                     break;
                 }
             }
             startTime += Time.deltaTime;
             yield return null;
         }

         if (LevelData.WinGameToUI)
         {
             LevelData.WinGameToUI = false;
             PlayLocalTipAnimation();
         }
     }

     public void FirstGuide()
     {
         if (PlayerPrefs.GetInt("MapController", -1) != 1 )
         {
             int currentLevel = LocalDataBase.Instance().GetSelectCopyLevel();
             GameObject currentCopyView = copyViewList[currentLevel - 1].gameObject;
             PlayerPrefs.SetInt("MapController", 1);
             GuideManager.Instance.ShowForceGuide(currentCopyView, false, "", Vector3.zero);
         }
     }

	public void UpdateCopyStateAfterGame(){
		StartCoroutine( UpdateCopyState());
	}

    private void PlayLocalTipAnimation()
    {
        animationMask.SetActive(true);
        iTweenHandler.PlayToPos(localTip, localTip.transform.localPosition, Vector3.zero, EliminateLogic.Instance.localTipMoveDeltaTime, false, AutoOpenNextCopyLevel);
    }

    IEnumerator _AutoOpenNextCopyLevel()
    {
        yield return new WaitForSeconds(0.2f);
        LevelData.AutoOpenNextLevel = false;

        int copyIndex = LocalDataBase.Instance().GetSelectCopyLevel();
        Tab_Copydetail copy = TableManager.GetCopydetailByID(copyIndex);
        if (copy.FreeErnie == 1 && PlayerPrefs.GetInt("CopyFreeErnie:" + copyIndex, 0) == 0)
        {
            PlayerPrefs.SetInt("CopyFreeErnie:" + copyIndex, 1);
            LocalDataBase.Instance().SetFreeChouTimes(1);
            BoxManager.Instance.ShowMessage(LanguageManger.GetMe().GetWords("L_1055"));
            UIEventListener.Get(BoxManager.Instance.buttonOk).onClick += OnFreeErnie;
            UIEventListener.Get(BoxManager.Instance.buttonCancle).onClick += OnCopyBefore;
//			IOSDialog dialog = IOSDialog.Create(LanguageManger.GetMe().GetWords("L_1097"), LanguageManger.GetMe().GetWords("L_1098"));
//			dialog.addEventListener(BaseEvent.COMPLETE, onDialogClose);
        }
        else
        {
            OnCopyBefore(null);
        }
    }

//	private void onDialogClose(CEvent e) {
//		
//		//romoving listner
//		(e.dispatcher as IOSDialog).removeEventListener(BaseEvent.COMPLETE, onDialogClose);
//		
//		//parsing result
//		switch((IOSDialogResult)e.data) {
//		case IOSDialogResult.YES:
//			Application.OpenURL("https://itunes.apple.com/cn/app/dai-meng-xiao-xiao-kan/id959344886?mt=8&from=timeline&isappinstalled=0");
//			Debug.Log ("Yes button pressed");
//			break;
//		case IOSDialogResult.NO:
//			Debug.Log ("No button pressed");
//			break;
//			
//		}
//		
//		string result = e.data.ToString();
//		Debug.Log ("Result:"+result + " button pressed");
//	}

    private void AutoOpenNextCopyLevel()
    {
        animationMask.SetActive(false);
        if (LevelData.AutoOpenNextLevel)
        {
            SceneManager.Instance.StartCoroutine(_AutoOpenNextCopyLevel());
        }
    }
	
	void OnClickCopyButton(GameObject go){
        #region guild
        GuideManager.Instance.HideGuide();
        #endregion

		UICopyItemView item = go.GetComponent<UICopyItemView>();
		
		if(!item.locked){
			
			JionCopy(item.copyData.copyID);
			
		}
	}
	
	//添加开始具体副本的接口
	/// <summary>
	/// Jions the copy.
	/// </summary>
	/// <param name='copyIndex'>
	/// Copy index.副本的ID或副本的名称;
	/// </param>
	void JionCopy(int copyIndex){
        //copyIndex参数为当前等级，需要对应的地图信息，不然会报错哦
        if (copyIndex >0)
        {
            LocalDataBase.Instance().SetSelectCopyLevel(copyIndex);
            SystemConfig.Log("JionCopy:" + copyIndex);

            Tab_Copydetail copy = TableManager.GetCopydetailByID(copyIndex);
            if (copy.FreeErnie == 1 && PlayerPrefs.GetInt("CopyFreeErnie:" + copyIndex, 0) == 0)
            {
                PlayerPrefs.SetInt("CopyFreeErnie:" + copyIndex, 1);
                LocalDataBase.Instance().SetFreeChouTimes(1);
                BoxManager.Instance.ShowMessage(LanguageManger.GetMe().GetWords("L_1055"));
                UIEventListener.Get(BoxManager.Instance.buttonOk).onClick += OnFreeErnie;
                UIEventListener.Get(BoxManager.Instance.buttonCancle).onClick += OnCopyBefore;
            }
            else
            {
                OnCopyBefore(null);
            }

        } 
        else
        {
            SystemConfig.Log("加载等级错误:" + copyIndex);
        }
        
		
		
	}


    private void OnFreeErnie(GameObject go)
    {
        PageManager.Instance.OpenPage("ZhuanPanController", "");
    }

    private void OnCopyBefore(GameObject go)
    {
        PageManager.Instance.OpenPage("CopyBeforeController", "");
    }

	/// <summary>
	/// Sorts the item.
	/// 根据item的transform,y轴由小到大排序,x抽由小到大
	/// </summary>
	/// <param name='item1'>
	/// Item1.
	/// </param>
	/// <param name='item2'>
	/// Item2.
	/// </param>
    int SortItem(UICopyPosView item1, UICopyPosView item2)
    {
		
		return item1.transform.position.y
			.CompareTo(item2.transform.position.y);
		
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

    public void RestoreTexturesAssets()
    {
        foreach (UITexture ut in texturesOfPage)
        {
            ut.mainTexture = Resources.Load("Texture/" + texturesOfPageCache[ut]) as Texture;
        }
    }
	
	#region CopyData
	public void UnlockedCopy(){
		foreach(CopyDataModel model in LocalDataBase.copyModels){
			if(model.star == -1 ){
				model.star = 3 ;
				model.SaveData();
			}
		}
		StartCoroutine( UpdateCopyState());
	}



	public void ResetCopy(){
		foreach(CopyDataModel model in LocalDataBase.copyModels){
			if(model.copyID != 1){
				model.star = -1 ;
				model.SaveData();
			}
		}
		StartCoroutine( UpdateCopyState());
	}


	#endregion		
	
}
