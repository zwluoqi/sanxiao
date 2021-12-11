using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using GCGame.Table;

using Random = UnityEngine.Random;

public class Map:MonoBehaviour
{
	//Instance of this class, so we can access map anywhere by Map.Instance
	public static Map Instance;

	//Max column of map
	public static int maxCol 						= 9;//最大列;
	public static int maxRow 						= 9;//最大行;
	private int[] usedMaxRow ;//记录每列中最高层(0-8);
	public static int MAP_START_Y 					= -(int)(UISquareView.BACK_WIDTH*4.2f);
	public static int MAP_START_X					= -(int)(UISquareView.BACK_WIDTH*((maxRow-1)/2.0f));		
	
	public  List<UISquareView> squares = new List<UISquareView>();
    public List<GameObject> lastRows = new List<GameObject>();
	private int producedBomNum = 0;
	public int eliminatingCount = 0;//正在消除的元素数量,当数量为零时,可确定所有需要消除元素已经消除;
	List<CopyElement> canProduceEleList;//能初始化的元素ID及其相关信息;
    Dictionary<UISquareView, int> savedItemTabId = new Dictionary<UISquareView,int>();//重置前保存元素信息;

		public List<UISquareView> tafangPath = null;//塔防路径;
	public List<int> allEleIDList;//地图中存在的所有元素的ID,排序;
	public List<int> allSquIDList;//地图中存在的所有方块ID,排序;
	public List<int> removedSquareIDList;//每次消除时消除的方块ID;
    public List<int> removedELeIDList;//每次消除时消除的元素ID; 
    public Dictionary<int,int> removedMissionIDList;//每轮消除时消除的任务ID及数量;
    public int removedMissionCombineTimes = 0;
	
	public delegate void OnCombinedCallback();//消除元素回调函数;



	// Use this for initialization
	void Awake () 
	{
		Instance = this;
	}
	public void LoadMap()
	{
		//统计关卡表中能产生的元素;
		canProduceEleList = new List<CopyElement>();
		for(int i=0;i<10;i++){
            int id = LevelData.currenCopyDetail.GetCanProduceIDbyIndex(i);
			if(id != -1){
				CopyElement ele = new CopyElement();
				ele.id = id;
                ele.existMax = LevelData.currenCopyDetail.GetCanExistNumbyIndex(i);
                ele.produceMax = LevelData.currenCopyDetail.GetCanProduceMaxbyIndex(i);
				canProduceEleList.Add(ele);
			}
		}		
		StartCoroutine( GenSquaresAndEles());
	}

    public void MapOver()
    {
        StopAllCoroutines();
        RemoveMap();
    }

		private List<UISquareView> CalculatePath()
    {
				List<UISquareView> path = new List<UISquareView>();
        if (LevelData.tfData == null)
        {
            return path;
        }
        List<UISquareView> yxs = GetSquares(LevelData.tfData.tfyxID);
        if (yxs.Count > 0)
        {
            UISquareView yx = yxs[0];
            UISquareView nextPos = null;
            do{
				nextPos = null;
                if (yx.m_col > 0)
                {
					UISquareView maybe = (GetSquare(yx.m_row, yx.m_col - 1));
                    if ((LevelData.tfData.tfGeZiIDs.Contains(maybe.tab_id) || maybe.tab_id == LevelData.tfData.tfendID) && !path.Contains(maybe))
                    {
						nextPos = maybe;
                    }
                }

                if (yx.m_col < 8)
                {
					UISquareView maybe = GetSquare(yx.m_row, yx.m_col + 1);
                    if ((LevelData.tfData.tfGeZiIDs.Contains(maybe.tab_id) || maybe.tab_id == LevelData.tfData.tfendID) && !path.Contains(maybe))
					{
						nextPos = maybe;
					}
                }

                if (yx.m_row > 0)
                {
					UISquareView maybe = GetSquare(yx.m_row -1 , yx.m_col);
                    if ((LevelData.tfData.tfGeZiIDs.Contains(maybe.tab_id) || maybe.tab_id == LevelData.tfData.tfendID) && !path.Contains(maybe))
					{
						nextPos = maybe;
					}
                }

                if (yx.m_row < 8)
                {
					UISquareView maybe = GetSquare(yx.m_row +1, yx.m_col);
                    if ((LevelData.tfData.tfGeZiIDs.Contains(maybe.tab_id) || maybe.tab_id == LevelData.tfData.tfendID) && !path.Contains(maybe))
					{
						nextPos = maybe;
					}
                }

                if (nextPos != null)
                {
										path.Add(nextPos);
                    yx = nextPos;
                }
            } while (nextPos != null);
        }
        return path;
    }

    IEnumerator GenSquaresAndEles()
    {
        yield return StartCoroutine(GenSquares());
        tafangPath = CalculatePath();
				foreach (UISquareView square in squares)
				{
						square.UpdateDir();
				}
        if (LevelData.needGuild)
        {
            StartCoroutine(GenGuildItems());
        }
        else
        {
            StartCoroutine(GenItems(GenerolItemType.New));
        }
    }

	/// <summary>
	/// Generate Map
	/// </summary>
	/// <returns>
	/// The map.
	/// </returns>
	IEnumerator GenSquares()
	{
		SystemConfig.LogWarning("GenSquares");
        bool bFlg = false;
		usedMaxRow = new int[maxCol];
		for(int row = 0; row< maxRow; row++)
		{
            bool start = bFlg;
			for(int col=0; col<maxCol; col++)
			{
                bFlg = !bFlg;
				GenSquare(row,col,bFlg);
			}
			bFlg = !start;
		}
        yield return null;
        for (int row = 0; row < maxRow; row++)
        {
            for (int col = 0; col < maxCol; col++)
            {
                GetSquare(row, col).UpdateSquareAndItemState();
            }
        }
        yield return null;
        RefreshLashRow();
    }

    public void RefreshLashRow()
    {
        ClearLastRow();
        if (LevelData.mode == CopyMode.ElE && LevelData.lastRowShow)
        {
            for (int i = 0; i < Map.maxCol; i++)
            {
                UISquareView square = Map.Instance.GetSquare(0, i);
                if (square.tab_square.IfwaiKuang != 0)
                {
                    GameObject go = WidgetBufferManager.Instance.loadWidget("Game/LastRow");
                    go.transform.parent = EleUIController.Instance.squareSpace;
                    go.transform.localPosition = square.DownSquarePosition;
                    go.transform.localScale = Vector3.one;
                    lastRows.Add(go);
                }
            }
        }
    }

    private void ClearLastRow()
    {
        foreach (GameObject go in lastRows)
        {
            WidgetBufferManager.Instance.DestroyWidgetObj("Game/LastRow", go);
        }
        lastRows.Clear();
    }

	/// <summary>
	/// Generate the square at [row,column]
	/// </summary>
	/// <param name='row'>
	/// Row.
	/// </param>
	/// <param name='col'>
	/// Col.
	/// </param>
	void GenSquare(int row, int col, bool bFlg)
	{
		UISquareView square = null;
		int mapValue = LevelData.mapData[row*maxCol + col];
		int squareID = mapValue;
		square = UISquareView.LoadSquare(row,col,squareID,bFlg,
			EleUIController.Instance.squareSpace);
		//add square to squares list
		squares.Add(square);
		
		if(square.tab_square.Updown != -1 || square.tab_square.SquareAtDown != -1){
			if(usedMaxRow[col] < row){
				usedMaxRow[col] = row;
			}
		}
	}

	public List<UISquareView> GetSquares(int tab_id){
		List<UISquareView> rets = new List<UISquareView>();
		for(int i=0;i<maxRow;i++){
			for(int j=0;j<maxCol;j++){
				UISquareView tmp = GetSquare(i,j);
				if(tmp.tab_id == tab_id){
					rets.Add(tmp);
				}
			}
		}
		return rets;
	}
	
	//统计地图中元素与方块;
	public void CalculateCurrentEleAndSqu(){
		allSquIDList = new List<int>();
		allEleIDList = new List<int>();
		for(int row = 0; row< Map.maxRow; row++)
		{
			for(int col=0; col<Map.maxCol; col++)
			{
				UISquareView square = GetSquare(row,col);
				if(!allSquIDList.Contains(square.tab_id)){
					allSquIDList.Add(square.tab_id);
					//SystemConfig.MyLogWarning("map tab_square id:"+square.tab_id);
				}
				if(square.item != null)
				{
					if(!allEleIDList.Contains(square.item.tab_id)){
						allEleIDList.Add(square.item.tab_id);
						//SystemConfig.MyLogWarning("map tab_ele id:"+square.item.tab_id);
					}
				}				
			}
		}
        allSquIDList.Sort(delegate(int tabid1, int tabid2) { return tabid1.CompareTo(tabid2); });
        allEleIDList.Sort(delegate(int tabid1, int tabid2) { if (TableManager.GetElementByID(tabid1).AffectType == (int)AffectType.WANNENG) return -1;
            return tabid1.CompareTo(tabid2); });
	}		
	
	//增加同类Square;
	//如果消除的方块ID中不包含地图中存在的ID,
	//并且该方块有递增效果,改变地图中某个位置为此ID;	
	public void AddSquareWithSameId(){

        foreach(int squ_id in allSquIDList){
			if(!removedSquareIDList.Contains(squ_id) && TableManager.GetSquareByID(squ_id).Addifnotdisappear == 1){
				List<UISquareView> rets = GetSquares(squ_id);
				if(rets.Count == 0)
					continue;
				for(int i=0;i<rets.Count;i++){
					UISquareView square = rets[i];
					List<UISquareView> canMovedToEle = new List<UISquareView>();
					if(square.m_row > 0){
						if (GetSquare(square.m_row-1,square.m_col).tab_square.CanCoverByOther == 1)
							canMovedToEle.Add(GetSquare(square.m_row-1,square.m_col));
					}
					
					if(square.m_row < maxRow -1){
						if (GetSquare(square.m_row+1,square.m_col).tab_square.CanCoverByOther == 1)
							canMovedToEle.Add(GetSquare(square.m_row+1,square.m_col));
					}
					
					if(square.m_col > 0){
						if (GetSquare(square.m_row,square.m_col - 1).tab_square.CanCoverByOther == 1)
							canMovedToEle.Add(GetSquare(square.m_row,square.m_col - 1));
					}
					
					if(square.m_col < maxCol -1){
						if (GetSquare(square.m_row,square.m_col + 1).tab_square.CanCoverByOther == 1)
							canMovedToEle.Add(GetSquare(square.m_row,square.m_col + 1));
					}
					
					UISquareView target = null;
					if(canMovedToEle.Count != 0){
						int index = Random.Range(0,canMovedToEle.Count);
						target = canMovedToEle[index];
					}

					if(target != null){
						SystemConfig.Log(target.name+" Addifnotdisappear :"+squ_id);
                        target.AddAtTop(squ_id, square);
						break;//同一ID，有一个能新增即可;
						//todo
					}
				}
			}
		}
	}
	
	//移动Square;
	//如果方块每次消除后都能移动,那么随机选择一个能被动态取代的square;
	public void MoveSquareToOtherPos(){

        foreach (int squ_id in allSquIDList)
        {
            if (!removedSquareIDList.Contains(squ_id) && TableManager.GetSquareByID(squ_id).Moveifnotdisappear == 1)
            {
                List<UISquareView> rets = GetSquares(squ_id);
                if (rets.Count == 0)
                    continue;

                for (int i = 0; i < rets.Count; i++)
                {
                    UISquareView square = rets[i];
                    List<UISquareView> canMovedToEle = new List<UISquareView>();
                    if (square.m_row > 0)
                    {
                        if (GetSquare(square.m_row - 1, square.m_col).tab_square.CanCoverByOther == 1)
                            canMovedToEle.Add(GetSquare(square.m_row - 1, square.m_col));
                    }

                    if (square.m_row < maxRow - 1)
                    {
                        if (GetSquare(square.m_row + 1, square.m_col).tab_square.CanCoverByOther == 1)
                            canMovedToEle.Add(GetSquare(square.m_row + 1, square.m_col));
                    }

                    if (square.m_col > 0)
                    {
                        if (GetSquare(square.m_row, square.m_col - 1).tab_square.CanCoverByOther == 1)
                            canMovedToEle.Add(GetSquare(square.m_row, square.m_col - 1));
                    }

                    if (square.m_col < maxCol - 1)
                    {
                        if (GetSquare(square.m_row, square.m_col + 1).tab_square.CanCoverByOther == 1)
                            canMovedToEle.Add(GetSquare(square.m_row, square.m_col + 1));
                    }

                    UISquareView target = null;
                    if (canMovedToEle.Count != 0)
                    {
                        int index = Random.Range(0, canMovedToEle.Count);
                        target = canMovedToEle[index];
                    }

                    if (target != null)
                    {
                        SystemConfig.Log("Moveifnotdisappear source:" + square.name);
                        SystemConfig.Log("Moveifnotdisappear target:" + target.name);
                        int sourceID = square.tab_id;
                        square.RemoveAtTop();
                        target.AddAtTop(sourceID, square);
                    }
                }

            }

        }


        //for(int row = 0; row< Map.maxRow; row++)
        //{
        //    for(int col=0; col<Map.maxCol; col++)
        //    {
        //        UISquareView square = GetSquare(row,col);
        //        if (!removedSquareIDList.Contains(square.tab_id) && square.tab_square.Moveifnotdisappear == 1)
        //        {
        //            List<UISquareView> canMovedToEle = new List<UISquareView>();
        //            if(square.m_row > 0){
        //                if (GetSquare(square.m_row-1,square.m_col).tab_square.CanCoverByOther == 1)
        //                    canMovedToEle.Add(GetSquare(square.m_row-1,square.m_col));
        //            }

        //            if(square.m_row < maxRow -1){
        //                if (GetSquare(square.m_row+1,square.m_col).tab_square.CanCoverByOther == 1)
        //                    canMovedToEle.Add(GetSquare(square.m_row+1,square.m_col));
        //            }

        //            if(square.m_col > 0){
        //                if (GetSquare(square.m_row,square.m_col - 1).tab_square.CanCoverByOther == 1)
        //                    canMovedToEle.Add(GetSquare(square.m_row,square.m_col - 1));
        //            }

        //            if(square.m_col < maxCol -1){
        //                if (GetSquare(square.m_row,square.m_col + 1).tab_square.CanCoverByOther == 1)
        //                    canMovedToEle.Add(GetSquare(square.m_row,square.m_col + 1));
        //            }

        //            UISquareView target = null;
        //            if(canMovedToEle.Count != 0){
        //                int index = Random.Range(0,canMovedToEle.Count);
        //                target = canMovedToEle[index];
        //            }

        //            if(target != null){
        //                SystemConfig.MyLog("Moveifnotdisappear source:"+square.name);
        //                SystemConfig.MyLog("Moveifnotdisappear target:"+target.name);
        //                int sourceID = square.tab_id;
        //                square.RemoveAtTop();
        //                target.AddAtTop(sourceID, square);
        //            }
					
        //        }
        //    }
        //}
		
	}

    public int TFYXCanMove()
    {
        int eliMissionCount = MissionManager.Instance.CompletedMissionCount(removedMissionIDList);
        if (eliMissionCount > 0)
        {
            return Mathf.Clamp(eliMissionCount / 3, 1, eliMissionCount);
        }
        else
        {
            return 0;
        }
    }

    public UISquareView GetTFYX()
    {
        List<UISquareView> yxs = GetSquares(LevelData.tfData.tfyxID);
        if (yxs.Count > 0)
        {
            return yxs[0];
        }
        return null;
    }

    public bool TFYXMove()
    {
        if (LevelData.mode == CopyMode.TAFANG)
        {
            SystemConfig.LogWarning("tfyxmove");
            UISquareView yx = GetTFYX();
            if (yx != null)
            {
								UISquareView mayNext = tafangPath[0];
                if (mayNext.tab_square.CanMove != 1 && mayNext.tab_id != LevelData.tfData.tfendID)
                {
                    return false;
                }
								UISquareView nextPos = tafangPath[0];
								tafangPath.RemoveAt(0);
                
                nextPos.AddAtTop(LevelData.tfData.tfyxID, yx);
                yx.RemoveAtTop();
				if(nextPos.item != null){
					nextPos.item.MoveIn(yx);
					nextPos.item = null;
				}

                if (tafangPath.Count == 0)
                {
                    return true;
                }
                else
                {
										//換面向

										//UISquareView dirNext = tafangPath[tafangPath.Count - 1];
										//yx.UpdateDirection(dirNext);
                    return false;
                }
            }
        }
        return false;
    }

    public bool TFDRmove(out List<AttackModel> attacked)
    {
        bool tfdrCanMove = false;
        attacked = new List<AttackModel>();
        if (LevelData.mode == CopyMode.TAFANG)
        {
            SystemConfig.LogWarning("tfdrmove");
            List<UISquareView> drs = new List<UISquareView>();
            foreach (int drID in LevelData.tfData.tfDiRenIDs)
            {
                List<UISquareView> drIDs = GetSquares(drID);
                drs.AddRange(drIDs);
            }
            UISquareView yx = GetTFYX();
            if (drs.Count > 0 && yx != null)
            {
                foreach (UISquareView drSquare in drs)
                {
                    UISquareView source = drSquare;
                    //是否已经在英雄周围;
                    bool near = Math.Abs(source.m_row - yx.m_row) + Math.Abs(source.m_col - yx.m_col) == 1;
                    if (near)
                    {
                        attacked.Add(new AttackModel(source,yx));
                    }

                    List<UISquareView> canMovedToSquare = new List<UISquareView>();
                    if (drSquare.m_col < yx.m_col && GetSquare(drSquare.m_row, drSquare.m_col + 1).tab_square.CanMove == 1)
                    {
                        canMovedToSquare.Add(GetSquare(drSquare.m_row, drSquare.m_col + 1));
                    }
                    if (drSquare.m_col > yx.m_col && GetSquare(drSquare.m_row, drSquare.m_col -1).tab_square.CanMove == 1)
                    {
                        canMovedToSquare.Add(GetSquare(drSquare.m_row, drSquare.m_col - 1));
                    }
                    if (drSquare.m_row < yx.m_row && GetSquare(drSquare.m_row + 1, drSquare.m_col).tab_square.CanMove == 1)
                    {
                        canMovedToSquare.Add(GetSquare(drSquare.m_row + 1, drSquare.m_col));
                    }
                    if (drSquare.m_row > yx.m_row && GetSquare(drSquare.m_row - 1, drSquare.m_col).tab_square.CanMove == 1)
                    {
                        canMovedToSquare.Add(GetSquare(drSquare.m_row - 1, drSquare.m_col));
                    }
                    UISquareView target = null;
                    if (canMovedToSquare.Count != 0)
                    {
                        int index = Random.Range(0, canMovedToSquare.Count);
                        target = canMovedToSquare[index];
                    }

                    if (target != null)
                    {
                        SystemConfig.Log("direnMove source:" + drSquare.name);
                        SystemConfig.Log("direnMove target:" + target.name);
                        int sourceID = drSquare.tab_id;
                        
                        target.AddAtTop(sourceID, drSquare);
                        drSquare.RemoveAtTop();
                        if (target.item != null)
                        {
                            target.item.MoveIn(drSquare);
                            target.item = null;
                        }
                        source = target;
                        tfdrCanMove = true;
                    }

                    /*
                    //毛毛虫移动到周围了,攻击!
                    bool near = Math.Abs( source.m_row - yx.m_row) + Math.Abs( source.m_col - yx.m_col) ==1;
                    if (near)
                    {
                        attacked.Add(source);
                    }
                     * */
                }
            }
        }
        return tfdrCanMove;
    }

    IEnumerator GenGuildItems()
    {
        //清除已产生的item
        foreach (CopyElement ele in canProduceEleList)
        {
            ele.producedNum = 0;
        }
        for (int m = 0; m < maxRow; m++)
        {
            for (int n = 0; n < maxCol; n++)
            {
                UISquareView squareGet = GetSquare(m, n);
                int tabid = LevelData.GetGuildEleId(m,n);
                if (squareGet.CanStayItemIn() && tabid != -1)
                {
                    UIEliminateItemView item = GenGuildItem(squareGet,tabid);
                    item.transform.localScale = Vector3.zero;
                }
            }
        }
        for (int m = 0; m < maxRow; m++)
        {
            for (int n = 0; n < maxCol; n++)
            {
                UISquareView squareGet = GetSquare(m, n);
                if (squareGet.item != null)
                {
                    iTweenHandler.PlayToScale(squareGet.item.gameObject, Vector3.zero, Vector3.one, EliminateLogic.Instance.createDeltaTime, null);
                }
            }
        }
        yield return new WaitForSeconds(EliminateLogic.Instance.createDeltaTime);
        EliminateLogic.Instance.GetEliminatePlayer().EndGeneMap();
    }

    private UIEliminateItemView GenGuildItem(UISquareView square,int tabid)
    {
        UIEliminateItemView item = UIEliminateItemView.LoadGameObject(square, tabid, EleUIController.Instance.squareSpace);
        CopyElement copyEle = canProduceEleList.Find((obj) => obj.id == tabid);
        if (copyEle != null)
        {
            copyEle.producedNum++;
        }
        return item;
    }


	IEnumerator GenItems(GenerolItemType gType)
	{	
		SystemConfig.LogWarning("GenItems");
		//Gen square at row i, column j

		//清除已产生的item
		foreach(CopyElement ele in canProduceEleList){
			ele.producedNum = 0;
		}

		//重置前,获取当前的item信息
        if (gType == GenerolItemType.Reset)
        {
            savedItemTabId = new Dictionary<UISquareView, int>();
			for(int i = 0; i< maxRow; i++)
			{

				for(int j=0; j<maxCol; j++)
				{
					UISquareView square = GetSquare(i,j);
                    if (square.item != null && !square.item.baseEle)
					{
                        savedItemTabId.Add(square, square.item.tab_id);	
					}
                    
				}
			}
		}

		//产生item
        SwipeDirection diction;
		do{
			for(int i = 0; i< maxRow; i++)
			{

				for(int j=0; j<maxCol; j++)
				{
					UISquareView square = GetSquare(i,j);
					if(square.item != null)
					{
						square.item.staySquare = null; 
						square.item.DestroyObj();
						square.item = null;		
					}
				}
			}

            int count = 0;
			for(int m = 0; m< maxRow; m++)
			{
				for(int n=0; n<maxCol; n++)
				{
					UISquareView squareGet = GetSquare(m,n);
					if(squareGet.CanStayItemIn())
					{
                        UIEliminateItemView item = GenItem(squareGet, gType);
                        item.transform.localScale = Vector3.zero;
                        if (++count%10 == 0)
                        {
                            yield return 1;
                        }
					}
				}
			}
            yield return null;
        } while (FindNextMove(out diction).Count == 0);
        savedItemTabId.Clear();

        for (int m = 0; m < maxRow; m++)
        {
            for (int n = 0; n < maxCol; n++)
            {
                UISquareView squareGet = GetSquare(m, n);
                if (squareGet.item != null)
                {
                    iTweenHandler.PlayToScale(squareGet.item.gameObject, Vector3.zero, Vector3.one, EliminateLogic.Instance.createDeltaTime, null);
                }
            }
        }



		//临时道具生成;
        //if(LocalDataBase.Instance().GetEquipTmpNum(EquipTypeTmp.Miracle) > 0){
        //    bool changed = false;
        //    do{
        //        int row = Random.Range(0,maxRow);
        //        int col = Random.Range(0,maxCol);
        //        UISquareView square = GetSquare(row,col);
        //        if (square.CanEliminateItemByCheck() 
        //            &&square.item.tab_ele.AffectType < 0){
        //            int ret = square.item.tab_ele.FiveEle;
        //            if(ret != -1){
        //                square.item.SetInitID(ret);
        //                LocalDataBase.Instance().SetEquipTmpNum(EquipTypeTmp.Miracle,0);
        //                changed = true;
        //            }
        //        }
        //    }while(!changed);
        //}
        //if(LocalDataBase.Instance().GetEquipTmpNum(EquipTypeTmp.Specital) > 0){
        //    bool changed = false;
        //    do{
        //        int row = Random.Range(0,maxRow);
        //        int col = Random.Range(0,maxCol);
        //        UISquareView square = GetSquare(row,col);
        //        if (square.CanEliminateItemByCheck() 
        //            &&square.item.tab_ele.AffectType < 0){
        //            int ret = square.item.tab_ele.FourElecol;
        //            if(ret != -1){
        //                square.item.SetInitID(ret);
        //                LocalDataBase.Instance().SetEquipTmpNum(EquipTypeTmp.Specital,0);
        //                changed = true;
        //            }
        //        }
        //    }while(!changed);
        //}

        yield return new WaitForSeconds(EliminateLogic.Instance.createDeltaTime);
        EliminateLogic.Instance.GetEliminatePlayer().EndGeneMap();

	}
	public void ReGenItems()
	{
		StartCoroutine(GenItems(GenerolItemType.Reset));
	}
	
	/// <summary>
	/// Generate item for square.
	/// </summary>
	/// <param name='square'>
	/// Square.
	/// </param>
    UIEliminateItemView GenItem(UISquareView square, GenerolItemType gType)
	{
        int iType = -1;
        if (savedItemTabId.ContainsKey(square))
        {
            iType = savedItemTabId[square];
        }
        else
        {


            //List<int> remainTypes = new List<int>();//可供选择的元素ID;
            List<int> specitalRemainTypes = new List<int>();
            //当remainTypes为空时,应该随机选择一个可以生成的元素;
            //而不用考虑最大可生成值与最大可存在数量;

            //重新生成底图的逻辑
            //bool useReset = false;
            //Dictionary<int,int> remainedToSaved = new Dictionary<int, int>();//生成元素到保存元素的映射
            //if (gType == GenerolItemType.Reset)
            //{
            //    int savedIndex = 0;
            //    foreach(int tabID in savedItemTabId){
            //        int ele_id = tabID;
            //        //方块在上方,但是元素不能被覆盖;则不能在该位置生成该元素;
            //        if(square.tab_square.Updown == 1 && TableManager.GetElementByID(ele_id).CanCoverWhenInit != 1){

            //            //方块不在最上行,并且元素可以在最下面一行继续下落,则不能在该位置生成该元素;
            //        }else if (square.m_row < usedMaxRow[square.m_col] && TableManager.GetElementByID(ele_id).CanDropatdown == 1){

            //        }else{
            //            bool canGen = CanGenItemByType(square,ele_id);
            //            if(canGen ){
            //                remainTypes.Add(ele_id);
            //                remainedToSaved[remainTypes.Count-1] = savedIndex;
            //            }
            //        }
            //        savedIndex++;
            //    }
            //}
            //if(remainTypes.Count == 0){
            //计算目前能新生成的元素;
            foreach (CopyElement ele in canProduceEleList)
            {
                int ele_id = ele.id;

                //方块在上方,但是元素不能被覆盖;则不能在该位置生成该元素;
                if (square.tab_square.Updown == 1 && TableManager.GetElementByID(ele_id).CanCoverWhenInit != 1)
                {

                    //方块不在最上行,并且元素可以在最下面一行继续下落,则不能在该位置生成该元素;
                }
                else if (square.m_row < usedMaxRow[square.m_col] && TableManager.GetElementByID(ele_id).CanDropatdown == 1)
                {

                }
                else
                {
                    bool canGen = CanGenItemByType(square, ele_id);
                    if (canGen)
                    {
                        int typeCount = GetItemCountByType(ele_id);
                        if ((ele.producedNum < ele.produceMax || ele.produceMax == -1)
                            && (typeCount < ele.existMax || ele.existMax == -1))
                        {
                            //remainTypes.Add(ele_id);
                            specitalRemainTypes.Add(ele_id);
                        }
                    }

                }
            }
            //}
            //else if (gType == GenerolItemType.Reset)
            //{
            //    useReset = true;
            //}
            //else
            //{

            //}

            //Random generate GetItemType() from remain types we can 
            int index = 0;
            //if(remainTypes.Count >0){
            //    index = UnityEngine.Random.Range(0,remainTypes.Count);
            //    iType = remainTypes[index];
            //    //重置生成的元素,删除重置列表对应的项;
            //    if(useReset){
            //        int toSaved = remainedToSaved[index];
            //        savedItemTabId.RemoveAt(toSaved);
            //    }
            //}else{
            index = UnityEngine.Random.Range(0, specitalRemainTypes.Count);
            iType = specitalRemainTypes[index];
            //}
        }


		//load game object by GetItemType()
		UIEliminateItemView item = UIEliminateItemView.LoadGameObject(square,iType,EleUIController.Instance.squareSpace);
		CopyElement copyEle = canProduceEleList.Find((obj) => obj.id == iType);
		if(copyEle != null){
			copyEle.producedNum++; 
		}
		
		
		//如果关卡中存在炸药,则炸药随机附在可移动对象身上;
        if (GetBomCount() < LevelData.currenCopyDetail.BomExistMax && producedBomNum < LevelData.currenCopyDetail.BomProduceMAx && square.CanMoveItemOut())
        {
			bool canAddBom = false;
			int u = Random.Range(0,100);
            if (u < LevelData.currenCopyDetail.BomPro || LevelData.currenCopyDetail.BomPro == -1)
            {
				canAddBom = true;
			}

			if(canAddBom){
				int i = Random.Range(0,2);
				if(i == 0){
					item.AddBomObj();
					producedBomNum++;
				}else{
					;
				}	
			}
		}

		return item;
	}
	
	
	public bool CanGenItemByType(UISquareView ui_square, int i_type){
		int row = ui_square.m_row;
		int col = ui_square.m_col;
		bool canGen = true;
		Tab_Element tab_ele = TableManager.GetElementByID(i_type);
		//Rule to generate items: We won't generate item that immediately make combination
		//with available items.
		//Example: call the square is "X", if we have a item GetItemType() "A", and now map is like this
		//A-A-X 
		//or 
		//X-A-A, 
		//or
		/*X A
		  A A
		  A X*/
		//we won't gen "A" GetItemType() for this square		
		if(col >= 1)
		{
			UISquareView neighbor = GetSquare(row,col-1);
			if(neighbor.CanEliminateItemByCheck() && neighbor.item.tab_ele.EleColor == tab_ele.EleColor)
				canGen = false;
		}
		if(col < maxCol-1)
		{
			UISquareView neighbor = GetSquare(row,col+1);
			if(neighbor.CanEliminateItemByCheck() && neighbor.item.tab_ele.EleColor == tab_ele.EleColor)
				canGen = false;
		}
		if(row >= 1)
		{
			UISquareView neighbor = GetSquare(row-1,col);
			if(neighbor.CanEliminateItemByCheck() && neighbor.item.tab_ele.EleColor == tab_ele.EleColor)
				canGen = false;
		}
		if(row < maxRow - 1){
			UISquareView neighbor = GetSquare(row+1,col);
			if(neighbor.CanEliminateItemByCheck() && neighbor.item.tab_ele.EleColor == tab_ele.EleColor)
				canGen = false;
		}
		return canGen;
	}
	
	
	public void PressOnItem(UIEliminateItemView pressedItem){

		//使用道具逻辑;
		if(EleUIController.Instance.selectedEquipView != null){
			//选择项是一般项能被消除才能使用道具;
			if(!pressedItem.staySquare.CanEliminateByEquipAffect()){
				return ;
			}
			bool clearEquipSelect = true;
			if(EleUIController.Instance.selectedEquipView.effectType == EquipEffectType.Bomb_SameCor){
				EquipManager.Instance.UseBomb_SameCor(pressedItem);
			}else if(EleUIController.Instance.selectedEquipView.effectType == EquipEffectType.Hammer){
				EquipManager.Instance.UseHammer(pressedItem);
			}else if(EleUIController.Instance.selectedEquipView.effectType == EquipEffectType.BombRow){
				EquipManager.Instance.UseBombRow(pressedItem);
				EleUIController.Instance.equipController.UsedCompleted();
			}else if(EleUIController.Instance.selectedEquipView.effectType == EquipEffectType.BombCol){
				EleUIController.Instance.equipController.UsedCompleted();
				EquipManager.Instance.UseBombCol(pressedItem);
			}else if(EleUIController.Instance.selectedEquipView.effectType == EquipEffectType.Exchange){
				//EquipManager.Instance.UseExchangeEle(pressedItem);
				//todo
				clearEquipSelect = false;
            }
            else if (EleUIController.Instance.selectedEquipView.effectType == EquipEffectType.BomEffect) {
                EquipManager.Instance.UseBomEffect(pressedItem);
            }
            else
            {
                Debug.LogError("meifa shiyong daoju");
            }
			if(clearEquipSelect){
				EleUIController.Instance.selectedEquipView = null;
                EleUIController.Instance.UpdateEquipNumUI();
				return ;
			}
		}		
		
		//选择项能移动即可被选择移动;
        if (pressedItem.staySquare.CanMoveItemOut())
		{
            if (EleUIController.Instance.selectedItem == null)
            {
                EleUIController.Instance.selectedItem = pressedItem;
            } 
            else
            {  
                //已选项周边方块
                List<UISquareView> squares = EleUIController.Instance.selectedItem.staySquare.GetNeighbors();
               if (squares.Contains(pressedItem.staySquare))
               {
					if(EleUIController.Instance.selectedEquipView != null
					   && EleUIController.Instance.selectedEquipView.effectType == EquipEffectType.Exchange){
						EquipManager.Instance.UseExchangeEle(EleUIController.Instance.selectedItem,pressedItem);
						EleUIController.Instance.selectedEquipView.UpdateNumberLabel();				
						EleUIController.Instance.selectedEquipView = null;
					}
					else{
						EleUIController.Instance.selectedItem.targetSquare = pressedItem.staySquare;
						EleUIController.Instance.targetItem = pressedItem;
					}
               } 
               else
               {
                   EleUIController.Instance.selectedItem = pressedItem;
               }
            }
		}
	}
	
	public void DragOnItem(Vector2 deltaMove){
		
		SwipeDirection dir = FingerController.JudgeDragDir(deltaMove.x, deltaMove.y);
		if (dir != SwipeDirection.None)
		{
			UISquareView neighbor = FingerController.GetNearDirSquare(
				EleUIController.Instance.selectedItem.staySquare, 
				FingerController.GetMoveDirectionByDir(dir));
			
			if(neighbor != null&&neighbor.CanMoveItemOut()){
				if(EleUIController.Instance.selectedEquipView != null
				   && EleUIController.Instance.selectedEquipView.effectType == EquipEffectType.Exchange){
					EquipManager.Instance.UseExchangeEle(EleUIController.Instance.selectedItem,neighbor.item);
					EleUIController.Instance.selectedEquipView.UpdateNumberLabel();				
					EleUIController.Instance.selectedEquipView = null;
				}else{
					EleUIController.Instance.selectedItem.targetSquare = neighbor;
					EleUIController.Instance.targetItem = neighbor.item;
				}
			}
		}
		else
		{
		   Debug.LogError("dri err");
		}
		
	}
	
	public UISquareView GetSquare(int row, int col)
	{
		return squares[row * maxCol + col];
	}
	
	public int GetItemCountByType(int iType){
		int count = 0;
		for(int i=0;i<maxRow;i++){
			for(int j=0;j<maxCol;j++){
				
				UISquareView squareView = GetSquare(i,j);
				if(squareView.item != null){
					if(squareView.item.tab_id == iType)
						count++;
				}
			}
		}
		return count;
	}
	
	public int GetBomCount(){
		int count = 0;
		for(int i=0;i<maxRow;i++){
			for(int j=0;j<maxCol;j++){
				
				UISquareView squareView = GetSquare(i,j);
				if(squareView.item != null){
					if(squareView.item.GetBomObj() != null )
						count++;
				}
			}
		}
		return count;
	}
	
	
	
	/*
	/// <summary>
	/// 根据得到的三消组合,得到一个消除单元;
	/// </summary>
	/// <returns>
	/// The one combine.
	/// </returns>
	/// <param name='subCombines'>
	/// 子对象组.
	/// </param>
	/// <param name='combineRow'>
	/// row = true表示横排;
	/// </param>
	private Combine GetOneCombine(List<UIEliminateItemView> subCombines,bool combineRow){
		//生成一个消除单元;
		Combine combine = new Combine();
		combine.items.AddRange(subCombines);
		if(combineRow){
			combine.combinType = Combine.CombineType.ROW;
		}else{
			combine.combinType = Combine.CombineType.COL;
		}
		
		//元素消除对周围的影响（-1，不影响，0周围爆炸影响。1横排影响。2纵排影响）
		//如果被消除的元素能影响横排或列排,则将整列或整行元素放到预备元素里面;
		for(int i=0;i<subCombines.Count;i++){
			if(subCombines[i].tab_ele.AffectType == 1){
				combine.tmpothers.AddRange(GetAffectRowEle(subCombines[i]));			
			}else if(subCombines[i].tab_ele.AffectType == 2){
				combine.tmpothers.AddRange(GetAffectColEle(subCombines[i]));			
			}else if(subCombines[i].tab_ele.AffectType == 0){
				combine.tmpothers.AddRange(GetAffectArroundEle(subCombines[i]));		
			}else {
				
			}
		}
		
		return combine;
	}
	*/
	
	//得到横排影响的集合;
	public List<UIEliminateItemView> GetAffectRowEle(UISquareView subSquView){
		List<UIEliminateItemView> attectList = new List<UIEliminateItemView>();
		for(int j=0;j<maxCol;j++){
			UISquareView squareTmp = GetSquare(subSquView.m_row,j);

			if(squareTmp.CanEliminateByRowSpecitalAffect()){
				attectList.Add(squareTmp.item);
			}
		}	
		return attectList;
	}
	
	//得到纵列影响的集合;
    public List<UIEliminateItemView> GetAffectColEle(UISquareView subSquView)
    {
		List<UIEliminateItemView> attectList = new List<UIEliminateItemView>();
		for(int j=0;j<maxRow;j++){
            UISquareView squareTmp = GetSquare(j, subSquView.m_col);

			if(squareTmp.CanEliminateByColSpecitalAffect()){
				attectList.Add(squareTmp.item);
			}
		}		
		return attectList;
	}
	
	//得到周围1平方影响的集合;
    public List<UIEliminateItemView> GetAffectArroundEle(UISquareView subSquView, int power = 1)
    {
		List<UIEliminateItemView> attectList = new List<UIEliminateItemView>();
		for(int row=0; row<maxRow; row++)
		{
			for(int col=0; col< maxCol; col++)
			{
				if(Mathf.Abs(subSquView.m_row - row) +
                    Mathf.Abs(subSquView.m_col - col) <= (Mathf.Pow(power,2) + 1)
                    && Mathf.Abs(subSquView.m_row - row) <= power + 1
                    && Mathf.Abs(subSquView.m_col - col) <= power + 1)
                {
					UISquareView squareTmp = GetSquare(row,col);
					
					if(squareTmp.CanEliminateByBomSpecitalAffect()){
						attectList.Add(squareTmp.item);
					}
				}
			}
			
		}	
		return attectList;
	}

    public bool CanRemoveEleListBySameColor(List<UIEliminateItemView> subCombines)
    {
        //检测是否是同一ID元素
        int sub_ele_id = subCombines[0].tab_id;
        bool sameID = true;
        foreach (UIEliminateItemView view in subCombines)
        {
            if (view.tab_id != sub_ele_id)
                sameID = false;
        }
        //如果是同一ID，并且同一ID不能被一起消除,则不处理了;
        if (sameID && TableManager.GetElementByID(sub_ele_id).CanEleBySame != 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


	//得到所有要消除的对象体;
	public List<Combine> GetCheckedCombines(){
        CalculateCurrentEleAndSqu();

		List<Combine> combines = new List<Combine>();
		
		//根据颜色检测消除;
		List<int> colorDic = new List<int>();	
		foreach(int ele_id in allEleIDList){
			Tab_Element tab_ele = TableManager.GetElementByID(ele_id);
			if(tab_ele == null)
				continue;
			if(tab_ele.ElimilateByCheck != 1)
				continue;
			if(colorDic.Contains(tab_ele.EleColor))
				continue;
			colorDic.Add(tab_ele.EleColor);
			//横排检测;
			for(int row=0; row<maxRow; row++)
			{
				List<UIEliminateItemView> subCombines = new List<UIEliminateItemView>();
				for(int col=0; col< maxCol; col++)
				{
					//Get current square
					UISquareView square = GetSquare(row,col);
					
					//该位置元素不能被检测消除,则统计以前的信息;
					if(!square.CanEliminateItemByCheck())
					{
						if(subCombines.Count >= 3)
						{
                            if (CanRemoveEleListBySameColor(subCombines))
                            {
                                Combine combine = new Combine();
                                combine.items.AddRange(subCombines);
                                combine.combinType = Combine.CombineType.ROW;
                                combines.Add(combine);
                                foreach (UIEliminateItemView view in subCombines)
                                {
                                    SystemConfig.Log(view.name);
                                }
                            }
						}
						subCombines.Clear();
						continue;
					}
					
					//类型相同,添加到消除单元内;
					if(square.item.tab_ele.EleColor == tab_ele.EleColor)
					{
						//如果下方能继续掉落,提前结束统计						
						if(row > 1){
							//下+方;
							bool removeList = false;
							for(int down_row = row - 1;down_row  >= 0;down_row--){
                                if (GetSquare(down_row, col).CanStayItemIn() && GetSquare(down_row + 1, col).CanMoveItemOut())
                                {
									removeList = true;
									break;
								}else if(GetSquare(down_row,col).CanMoveItemOut()){
									continue;
								}else{
									removeList = false;
									break;
								}
							}
							
							if(removeList){
								subCombines.Clear();
								continue;
							}
						}						
						subCombines.Add(square.item);
					}
					
					//如果当前格子已经是最后一个或者
					//当前格子的动物与正在统计的动物类型不一样
					//则清除数据或产生数据;
					if(square.item.tab_ele.EleColor != tab_ele.EleColor || col == maxCol-1)
					{
						if(subCombines.Count >= 3)
						{
                            if (CanRemoveEleListBySameColor(subCombines))
                            {
                                Combine combine = new Combine();
                                combine.items.AddRange(subCombines);
                                combine.combinType = Combine.CombineType.ROW;
                                combines.Add(combine);
                                foreach (UIEliminateItemView view in subCombines)
                                {
                                    SystemConfig.Log(view.name);
                                }
                            }
							
						}
						subCombines.Clear();
						continue;
					}
				}
				
			}
			//纵排检测;
			for(int col=0; col< maxCol; col++)
			{
				List<UIEliminateItemView> subCombines = new List<UIEliminateItemView>();
				for(int row=0; row < maxRow; row++)
				{
					UISquareView square = GetSquare(row,col);
					
					//该位置元素不能被检测消除,则统计以前的信息;
					if(!square.CanEliminateItemByCheck())
					{
						if(subCombines.Count >= 3)
						{

                            if (CanRemoveEleListBySameColor(subCombines))
                            {
                                Combine combine = new Combine();
                                combine.items.AddRange(subCombines);
                                combine.combinType = Combine.CombineType.COL;
                                combines.Add(combine);
                                foreach (UIEliminateItemView view in subCombines)
                                {
                                    SystemConfig.Log(view.name);
                                }
                            }				
						}
						subCombines.Clear();
						continue;
					}
					if(square.item.tab_ele.EleColor == tab_ele.EleColor)
					{
						//如果下方能继续掉落,提前结束统计
						if(row > 1){
							//下+方;
							bool removeList = false;
							for(int down_row = row - 1;down_row  >= 0;down_row--){
                                if (GetSquare(down_row, col).CanStayItemIn() && GetSquare(down_row + 1, col).CanMoveItemOut())
                                {
									removeList = true;
									break;
								}else if(GetSquare(down_row,col).CanMoveItemOut()){
									continue;
								}else{
									removeList = false;
									break;
								}
							}
							
							if(removeList){
								subCombines.Clear();
								continue;
							}
						}						
						subCombines.Add(square.item);
					}
					if(square.item.tab_ele.EleColor != tab_ele.EleColor || row == maxRow-1)
					{
						if(subCombines.Count >= 3)
						{
                            if (CanRemoveEleListBySameColor(subCombines))
                            {
                                Combine combine = new Combine();
                                combine.items.AddRange(subCombines);
                                combine.combinType = Combine.CombineType.COL;
                                combines.Add(combine);
                                foreach (UIEliminateItemView view in subCombines)
                                {
                                    SystemConfig.Log(view.name);
                                }
                            }						
							
						}
						subCombines.Clear();
						continue;
					}
				}
			}
			
		}
		
		if(combines.Count == 0)
			return combines;
		
		Combine result = new Combine();
		//找出三消或四消或五消的交叉点;
		for(int i=0;i<combines.Count;i++){
			for(int j=i+1;j<combines.Count;j++){
				for(int m = 0;m<combines[j].items.Count;m++){
					if(combines[i].items.Contains(combines[j].items[m])){
						UIEliminateItemView changedItem = combines[j].items[m];
						if(changedItem.tab_ele.CrossEle != -1 ){
							result.AddChangedToOtherItem(changedItem,changedItem.tab_ele.CrossEle);
						}

                        if (changedItem.tab_ele.AffectType > (int)AffectType.NONE)
                        {
                            result.items.AddRange(GetAffectArroundEle(changedItem.staySquare));
                        }

						if(changedItem.tab_ele.CollectiveMissionid != -1){
							changedItem.CollectiveMissionid = changedItem.tab_ele.CollectiveMissionid;
							changedItem.CollectiveMissionnum = changedItem.tab_ele.CollectiveMissionnum;
						}

						combines[i].combinType = Combine.CombineType.CROSS;//交叉
						combines[j].combinType = Combine.CombineType.CROSS;//交叉
						break;
					}
				}
			}
		}
		
		//找出横排或纵列三消且未交叉的对象;
		for(int i=0;i<combines.Count;i++){
			if(combines[i].items.Count == 3 && combines[i].combinType != Combine.CombineType.CROSS){
				UIEliminateItemView changedItem = combines[i].GetChangedItem();
				if(changedItem.tab_ele.ThreeEle != -1 ){
					result.AddChangedToOtherItem(changedItem,changedItem.tab_ele.ThreeEle);
				}

				if(changedItem.tab_ele.CollectiveMissionid != -1){
					changedItem.CollectiveMissionid = changedItem.tab_ele.CollectiveMissionid;
					changedItem.CollectiveMissionnum = changedItem.tab_ele.CollectiveMissionnum;
				}

			}
		}
		
		//找出横排四消且未交叉的对象;
		for(int i=0;i<combines.Count;i++){
			if(combines[i].items.Count ==4 && combines[i].combinType == Combine.CombineType.ROW){
				UIEliminateItemView changedItem = combines[i].GetChangedItem();
                if (changedItem.tab_ele.AffectType <= (int)AffectType.NONE)
                {
					result.AddChangedToOtherItem(changedItem,changedItem.tab_ele.FourElerow);
                }
                if ( changedItem.tab_ele.AffectType > (int)AffectType.NONE)
                {
                    result.items.AddRange(GetAffectRowEle(changedItem.staySquare));
                }
				
				if(changedItem.tab_ele.CollectiveMissionid != -1){
					changedItem.CollectiveMissionid = changedItem.tab_ele.CollectiveMissionid;
					changedItem.CollectiveMissionnum = changedItem.tab_ele.CollectiveMissionnum;
				}
			}
		}
		
		
		//找出纵列四消未交叉的对象;
		for(int i=0;i<combines.Count;i++){
			if(combines[i].items.Count ==4 && combines[i].combinType == Combine.CombineType.COL){
				UIEliminateItemView changedItem = combines[i].GetChangedItem();
                if (changedItem.tab_ele.FourElecol != -1 )
                {
					result.AddChangedToOtherItem(changedItem,changedItem.tab_ele.FourElecol);
				}
                if ( changedItem.tab_ele.AffectType > (int)AffectType.NONE)
                {
                    result.items.AddRange(GetAffectColEle (changedItem.staySquare));
                }
                
                if (changedItem.tab_ele.CollectiveMissionid != -1)
                {
					changedItem.CollectiveMissionid = changedItem.tab_ele.CollectiveMissionid;
					changedItem.CollectiveMissionnum = changedItem.tab_ele.CollectiveMissionnum;
				}
			}
		}
		
		//找出横排或纵列五消且未交叉的对象;
		for(int i=0;i<combines.Count;i++){
			if(combines[i].items.Count >=5 && combines[i].combinType != Combine.CombineType.CROSS){
				UIEliminateItemView changedItem = combines[i].GetChangedItem();
				if(changedItem.tab_ele.FiveEle != -1 ){
					result.AddChangedToOtherItem(changedItem,changedItem.tab_ele.FiveEle);
				}
				
				if(changedItem.tab_ele.CollectiveMissionid != -1){
					changedItem.CollectiveMissionid = changedItem.tab_ele.CollectiveMissionid;
					changedItem.CollectiveMissionnum = changedItem.tab_ele.CollectiveMissionnum;
				}		
			}
		}
		
		
		//消除重复item项;
		for(int i=0;i<combines.Count;i++){
			for(int j= 0 ;j < combines[i].items.Count;j++){
				if(!result.items.Contains(combines[i].items[j])){
					result.items.Add(combines[i].items[j]);
				}

			}
		}
		
		//剔除item与changeditem的重复项;
		result.CullData();
		
		combines.Clear();
		if(result.items.Count > 0){
			combines.Add(result);
		}
		
		return combines;
	}
	
	/// 
	IEnumerator OnEliminateAndDrop()
	{
		//If can found combinations, disappear hint effect, count explosion,
		//and remove all combinations' item

		bool canDrop = false;
		int continueDrops = 1;
		int combineIndex = 0;//消除的次数;
		do
		{
			combineIndex++;
			SystemConfig.LogWarning("OnEliminateAndDrop:!"+combineIndex);
			List<Combine> combines = GetCheckedCombines();

			if(combines.Count > 0){
				continueDrops = 1;
				SystemConfig.LogWarning("RemoveCombines!"+combineIndex);
				yield return StartCoroutine(RemoveCombines(combines));
				SystemConfig.LogWarning("RemoveCombines!"+combineIndex);
				SystemConfig.LogWarning("drop first!"+combineIndex);
				yield return StartCoroutine(DropItemsOneSquare(continueDrops));
				SystemConfig.LogWarning("droped first!"+combineIndex);
				canDrop = true;
			}else{
				if(CanDropItemsOneSquare()){
					continueDrops ++ ;
					SystemConfig.LogWarning("drop second!"+combineIndex);
					yield return StartCoroutine(DropItemsOneSquare(continueDrops));
					SystemConfig.LogWarning("droped second!"+combineIndex);
					canDrop = true;
				}else{
					canDrop = false;
				}
			}
		}while(canDrop);
		SystemConfig.Log("completed EliminateAndDrop");

	}
	
	//Remove all items in combination, then move all item down, 
	//after move all item down, check combinations again
	IEnumerator RemoveCombines(List<Combine> combines)
	{
		eliminatingCount = 0;
		int count = combines.Count;
		while(count > 0)
		{
			count--;
			//destroy combine
			combines[count].CombineItems();
			//remove combine from combinations list
			combines.RemoveAt(count);
		}

		while (eliminatingCount >0){
			yield return 1;
		}
		
		SystemConfig.Log("completed Remove combine");
	}
	
	bool CanDropItemsOneSquare(){
		bool canDrop = false;
		for(int row=0; row<maxRow; row++)
		{
			for(int col=0; col<maxCol; col++)
			{
				UISquareView square = GetSquare(row,col);	
				if(square.CanMoveItemOut()){
					UISquareView targetSquare = null;	
					//最下一行不用检测;
					if(row >= 1)
					{
						//正下方;
						UISquareView neighbor = GetSquare(row-1,col);
						if(neighbor.CanStayItemIn())
							targetSquare = neighbor;
						
						//下+方;
						int downCount = 1;
						while(neighbor.CanMoveItemThrough()
							&& row >= 1 + downCount
							&& targetSquare == null){
							UISquareView neighbor_down = GetSquare(row-(1 + downCount),col);
							if(neighbor_down.CanStayItemIn()){
								targetSquare = neighbor_down;
							}
							downCount++;
						}
						
						//左下方;
						if(col >= 1  && targetSquare == null)
						{
							neighbor = GetSquare(row-1,col-1);
							if(neighbor.CanStayItemIn()){
								bool canMove= true;

								for(int i = row;i<maxRow;i++){
									if(GetSquare(i,col-1).CanMoveItemOut()){
										canMove = true;
										break;
									}else if(GetSquare(i,col-1).CanMoveItemThrough()){
										continue;
									}else{
										canMove = false;
										break;
									}
								}

								if(!canMove){
									targetSquare = neighbor;
								}
							}
						}
						//右下方;
						if(col < maxCol-1 && targetSquare == null)
						{
							neighbor = GetSquare(row-1,col+1);
							if(neighbor.CanStayItemIn()){								
								bool canMove= true;

								for(int i = row;i<maxRow;i++){
									if(GetSquare(i,col+1).CanMoveItemOut()){
										canMove = true;
										break;
									}else if(GetSquare(i,col+1).CanMoveItemThrough()){
										continue;
									}else{
										canMove = false;
										break;
									}
								}

								if(!canMove){
									targetSquare = neighbor;
								}
							}
						}							
					}else{
						//如果是最下一行,检测如果是香蕉,则下落,下落完成后消除;
						if(square.item.tab_ele.CanDropatdown ==1 ){
							canDrop = true;
						}
					}

					if(targetSquare !=null){
						canDrop = true;
					}
				}else if(square.CanStayItemIn()){
					//顶层无对象,则上面产生新的item;
					if(row == usedMaxRow[col] &&
					   (row == 8 || GetSquare(row+1,col).CanMoveItemThrough())){
						canDrop = true;
					}
				}
				
				if(canDrop){
					return canDrop;
				}
			}
		}
		
		return false;
	}
	
	
	IEnumerator DropItemsOneSquare(int continueDrops){
		List<UIEliminateItemView> droppingItems = new List<UIEliminateItemView>();
		for(int row=0; row<maxRow; row++)
		{
			for(int col=0; col<maxCol; col++)
			{
				UISquareView square = GetSquare(row,col);	
				if(square.CanMoveItemOut()){
					UISquareView targetSquare = null;	
					//最下一行不用检测;
					if(row >= 1)
					{
						//正下方;
						UISquareView neighbor = GetSquare(row-1,col);
						if(neighbor.CanStayItemIn())
							targetSquare = neighbor;
						
						//下+方;
						int downCount = 1;
						while(neighbor.CanMoveItemThrough()
							&& row >= 1 + downCount
							&& targetSquare == null){
							UISquareView neighbor_down = GetSquare(row-(1 + downCount),col);
							if(neighbor_down.CanStayItemIn()){
								targetSquare = neighbor_down;
							}
							downCount++;
						}
						
						//左下方;
						if(col >= 1  && targetSquare == null)
						{
							neighbor = GetSquare(row-1,col-1);
							if(neighbor.CanStayItemIn()){
								bool canMove= true;

								for(int i = row;i<maxRow;i++){
									if(GetSquare(i,col-1).CanMoveItemOut()){
										canMove = true;
										break;
									}else if(GetSquare(i,col-1).CanMoveItemThrough()){
										continue;
									}else{
										canMove = false;
										break;
									}
								}

								if(!canMove){
									targetSquare = neighbor;
								}
							}
						}
						//右下方;
						if(col < maxCol-1 && targetSquare == null)
						{
							neighbor = GetSquare(row-1,col+1);
							if(neighbor.CanStayItemIn()){								
								bool canMove= true;

								for(int i = row;i<maxRow;i++){
									if(GetSquare(i,col+1).CanMoveItemOut()){
										canMove = true;
										break;
									}else if(GetSquare(i,col+1).CanMoveItemThrough()){
										continue;
									}else{
										canMove = false;
										break;
									}
								}

								if(!canMove){
									targetSquare = neighbor;
								}
							}
						}							
					}else{
						//如果是最下一行,检测如果是香蕉,则下落,下落完成后消除;
						if(square.item.tab_ele.CanDropatdown ==1 ){
                            square.item.DropLastDownSquare(EliminateLogic.Instance.dropDeltaTime / continueDrops);
							square.item = null;
						}
					}

					if(targetSquare !=null){
						droppingItems.Add(square.item);
                        square.item.DropIn(targetSquare, EliminateLogic.Instance.dropDeltaTime / continueDrops);
						square.item = null;
						
						//顶层移动,则上面产生新的item;
						//或其上层可穿透,则上面产生新的item;;
						if(row == usedMaxRow[col] &&
						   (row == 8 || GetSquare(row+1,col).CanMoveItemThrough())
						   ){
                               UIEliminateItemView item = GenItem(square, GenerolItemType.Produce);
                               droppingItems.Add(item);
                               item.SimulateDropIn(EliminateLogic.Instance.dropDeltaTime / continueDrops);
                               SystemConfig.Log(item.transform.localScale);
                               SystemConfig.Log(1);
						}
					}
				}else if(square.CanStayItemIn()){
					//顶层无对象,则上面产生新的item;
					//或其上层可穿透,则上面产生新的item;;
					if(row == usedMaxRow[col] &&
					   (row == 8 || GetSquare(row+1,col).CanMoveItemThrough())
					   ){
                           UIEliminateItemView item = GenItem(square, GenerolItemType.Produce);
                           droppingItems.Add(item);
                           item.SimulateDropIn(EliminateLogic.Instance.dropDeltaTime / continueDrops);
                           SystemConfig.Log(item.transform.localScale);
                           SystemConfig.Log(3);
					}
				}
			}
		}
		
		//他们应播放掉落动画;
		for(int i=0;i<droppingItems.Count;i++){
			UISquareView square = droppingItems[i].staySquare;
			if(square.m_row > 1 && GetSquare(square.m_row - 1,square.m_col).CanMoveItemThrough() ){
				;
			}else if(square.m_row == 0 && droppingItems[i].tab_ele.CanDropatdown == 1){
				;
			}else{
				droppingItems[i].DropAnimation();
			}
		}

        while (!IsAllItemIdleOrNone())
        {
            yield return null;
        }
		
		SystemConfig.Log("completed drop");
	}


    public bool IsAllItemIdleOrNone()
    {
        foreach (UISquareView square in squares)
        {
            if (square.item != null && square.item.state != UIEliminateItemState.Idle && square.item.state != UIEliminateItemState.None)
            {
                return false;
            }
        }
        return true;
    }
	
	/// <summary>
	/// Automaticly find all items can make one combination to hint player
	/// </summary>
	/// <returns>
	/// The next move.
	/// </returns>
	public List<UIEliminateItemView> FindNextMove(out SwipeDirection diction)
	{
        CalculateCurrentEleAndSqu();
        diction = SwipeDirection.Left;
		List<UIEliminateItemView> nextMoveItems = new List<UIEliminateItemView>();
		
		//根据颜色检测可能消除的情况,并缓存;
		List<int> colorDic = new List<int>();	
		foreach(int ele_id in allEleIDList){
			int itemType = ele_id;
			Tab_Element tab_ele = TableManager.GetElementByID(ele_id);
			if(tab_ele == null)
				continue;
			if(tab_ele.ElimilateByCheck != 1)
				continue;
			if(colorDic.Contains(tab_ele.EleColor))
				continue;
			//Check squares around current square (called "x") 
			// "o" is GetItemType() of item, if squares make a shape suitable, choose them
			for(int row=0; row<maxRow; row++)
			{
				for(int col=0; col< maxCol; col++)
				{
					UISquareView square = GetSquare(row,col);
					if(!square.CanMoveItemOut())
					{
						nextMoveItems.Clear();
						continue;
					}

                    //万能元素首选
                    if (square.item.tab_ele.AffectType == (int)AffectType.WANNENG)
                    {
                        nextMoveItems.Add(square.item);
                        UIEliminateItemView firstItem = null;

                        if (row > 0 && GetSquare(row - 1, col).CanEliminateItemByCheck() && square.CanMoveItemOut())
                        {
                            diction = SwipeDirection.Down;
                            firstItem = GetSquare(row - 1, col).item;
                            if (firstItem.tab_ele.AffectType != (int)AffectType.NONE)
                            {
                                nextMoveItems.Add(firstItem);
                                return nextMoveItems;
                            }
                        }
                        if (row < maxRow - 1 && GetSquare(row + 1, col).CanEliminateItemByCheck() && square.CanMoveItemOut() )
                        {
                            diction = SwipeDirection.Up;
                            firstItem = (GetSquare(row + 1, col).item);
                            if (firstItem.tab_ele.AffectType != (int)AffectType.NONE)
                            {
                                nextMoveItems.Add(firstItem);
                                return nextMoveItems;
                            }
                        }
                        if (col > 0 && GetSquare(row, col - 1).CanEliminateItemByCheck() && square.CanMoveItemOut() )
                        {
                            diction = SwipeDirection.Left;
                            firstItem = (GetSquare(row, col - 1).item);
                            if (firstItem.tab_ele.AffectType != (int)AffectType.NONE)
                            {
                                nextMoveItems.Add(firstItem);
                                return nextMoveItems;
                            }
                        }
                        if (col < maxCol - 1 && GetSquare(row, col + 1).CanEliminateItemByCheck() && square.CanMoveItemOut())
                        {
                            diction = SwipeDirection.Right;
                            firstItem = (GetSquare(row, col + 1).item);
                            if (firstItem.tab_ele.AffectType != (int)AffectType.NONE)
                            {
                                nextMoveItems.Add(firstItem);
                                return nextMoveItems;
                            }
                        }

                        if (firstItem != null)
                        {
                            nextMoveItems.Add(firstItem);
                            return nextMoveItems;
                        }
                    }
                    nextMoveItems.Clear();

                    //高亮及周围次选
                    if (square.item.tab_ele.AffectType != (int)AffectType.NONE)
                    {
                        nextMoveItems.Add(square.item);
                        if (row > 0 && GetSquare(row - 1, col).CanEliminateItemByCheck() && GetSquare(row - 1, col).CanMoveItemOut()
                            && GetSquare(row - 1, col).item.tab_ele.AffectType != (int)AffectType.NONE)
                        {
                            diction = SwipeDirection.Down;
                            nextMoveItems.Add(GetSquare(row - 1, col).item);
                        }
                        else if (row < maxRow - 1 && GetSquare(row + 1, col).CanEliminateItemByCheck() && square.CanMoveItemOut()
                            && GetSquare(row + 1, col).item.tab_ele.AffectType != (int)AffectType.NONE)
                        {
                            diction = SwipeDirection.Up;
                            nextMoveItems.Add(GetSquare(row + 1, col).item);
                        }
                        else if (col > 0 && GetSquare(row, col - 1).CanEliminateItemByCheck() && square.CanMoveItemOut()
                            && GetSquare(row, col - 1).item.tab_ele.AffectType != (int)AffectType.NONE)
                        {
                            diction = SwipeDirection.Left;
                            nextMoveItems.Add(GetSquare(row, col - 1).item);
                        }
                        else if (col < maxCol - 1 && GetSquare(row, col + 1).CanEliminateItemByCheck() && square.CanMoveItemOut()
                            && GetSquare(row, col + 1).item.tab_ele.AffectType != (int)AffectType.NONE)
                        {
                            diction = SwipeDirection.Right;
                            nextMoveItems.Add(GetSquare(row, col + 1).item);
                        }
                    }
                    if (nextMoveItems.Count >= 2)
                        return nextMoveItems;
                    else
                        nextMoveItems.Clear();

					//current square called x
					//o-o-x
					//	  o
					if(col > 1 && row > 0)
					{
						square = GetSquare(row-1,col);
						if(square.CanMoveItemOut() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row,col-1);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row,col-2);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);

                        //多余搜索横向
                        List<UIEliminateItemView> tmp = new List<UIEliminateItemView>();
                        if (col < maxCol - 1 && nextMoveItems.Count >= 3)
                        {
                            square = GetSquare(row, col+1);
                            if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                                tmp.Add(square.item);
                        }
                        if (col < maxCol - 2 && nextMoveItems.Count >= 3 && tmp.Count >= 1)
                        {
                            square = GetSquare(row, col + 2);
                            if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                                tmp.Add(square.item);
                        }
                        if (tmp.Count >= 1)
                        {
                            nextMoveItems.AddRange(tmp);
                        }

                        //多余搜索纵向
                        tmp.Clear();
                        if (row < maxRow - 1 && nextMoveItems.Count >= 3)
                        {
                            square = GetSquare(row+1, col);
                            if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                                tmp.Add(square.item);
                        }
                        if (row < maxRow - 2 && nextMoveItems.Count >= 3 && tmp.Count >= 1)
                        {
                            square = GetSquare(row+2, col);
                            if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                                tmp.Add(square.item);
                        }
                        if (tmp.Count >= 2)
                        {
                            nextMoveItems.AddRange(tmp);
                        }
					}
                    diction = SwipeDirection.Up;
                    if (nextMoveItems.Count >= 3 && CanRemoveEleListBySameColor(nextMoveItems))
						return nextMoveItems;
					else
						nextMoveItems.Clear();
					
					//    o
					//o-o x
					if(col > 1 && row < maxRow-1)
					{
						square = GetSquare(row+1,col);
						if(square.CanMoveItemOut() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row,col-1);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row,col-2);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);

                        //多余搜索
                        if (col < maxCol - 1 && nextMoveItems.Count >= 3)
                        {
                            square = GetSquare(row, col + 1);
                            if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                                nextMoveItems.Add(square.item);
                        }
                        if (col < maxCol - 2 && nextMoveItems.Count >= 4)
                        {
                            square = GetSquare(row, col + 2);
                            if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                                nextMoveItems.Add(square.item);
                        }
					}
                    diction = SwipeDirection.Down;
                    if (nextMoveItems.Count >= 3 && CanRemoveEleListBySameColor(nextMoveItems))
						return nextMoveItems;
					else
						nextMoveItems.Clear();
					
					//x o o
					//o
					if(col < maxCol-2 && row > 0)
					{
						square = GetSquare(row-1,col);
						if(square.CanMoveItemOut() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row,col+1);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row,col+2);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);

                        //多余搜索
                        if (col > 0 && nextMoveItems.Count >= 3)
                        {
                            square = GetSquare(row, col - 1);
                            if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                                nextMoveItems.Add(square.item);
                        }
                        if (col > 1 && nextMoveItems.Count >= 4)
                        {
                            square = GetSquare(row, col - 2);
                            if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                                nextMoveItems.Add(square.item);
                        }
					}
                    diction = SwipeDirection.Up;
                    if (nextMoveItems.Count >= 3 && CanRemoveEleListBySameColor(nextMoveItems))
						return nextMoveItems;
					else
						nextMoveItems.Clear();
					
					//o
					//x o o
					if(col < maxCol-2 && row <maxRow-1)
					{
						square = GetSquare(row+1,col);
						if(square.CanMoveItemOut() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row,col+1);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row,col+2);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);

                        //多余搜索
                        if (col > 0 && nextMoveItems.Count >= 3)
                        {
                            square = GetSquare(row, col - 1);
                            if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                                nextMoveItems.Add(square.item);
                        }
                        if (col > 1 && nextMoveItems.Count >= 4)
                        {
                            square = GetSquare(row, col - 2);
                            if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                                nextMoveItems.Add(square.item);
                        }
					}
                    diction = SwipeDirection.Down;
                    if (nextMoveItems.Count >= 3 && CanRemoveEleListBySameColor(nextMoveItems))
						return nextMoveItems;
					else
						nextMoveItems.Clear();
					
					//o
					//o
					//x o
					if(col < maxCol-1 && row < maxRow-2)
					{
						square = GetSquare(row,col+1);
						if(square.CanMoveItemOut() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row+1,col);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row+2,col);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);

                        //多余搜索
                        if (row > 0 && nextMoveItems.Count >= 3)
                        {
                            square = GetSquare(row - 1, col);
                            if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                                nextMoveItems.Add(square.item);
                        }
                        if (row > 1 && nextMoveItems.Count >= 4)
                        {
                            square = GetSquare(row - 2, col);
                            if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                                nextMoveItems.Add(square.item);
                        }
					}
                    diction = SwipeDirection.Left;
                    if (nextMoveItems.Count >= 3 && CanRemoveEleListBySameColor(nextMoveItems))
						return nextMoveItems;
					else
						nextMoveItems.Clear();
					
					//x o
					//o
					//o
					if(col < maxCol-1 && row >1)
					{
						square = GetSquare(row,col+1);
						if(square.CanMoveItemOut() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row-1,col);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row-2,col);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);

                        //多余搜索
                        if (row < maxRow - 1 && nextMoveItems.Count >= 3)
                        {
                            square = GetSquare(row + 1, col);
                            if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                                nextMoveItems.Add(square.item);
                        }
                        if (row < maxRow - 2 && nextMoveItems.Count >= 4)
                        {
                            square = GetSquare(row + 2, col);
                            if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                                nextMoveItems.Add(square.item);
                        }
					}
                    diction = SwipeDirection.Left;
                    if (nextMoveItems.Count >= 3 && CanRemoveEleListBySameColor(nextMoveItems))
						return nextMoveItems;
					else
						nextMoveItems.Clear();
					
					//	o
					//  o
					//o x
					if(col > 0 && row < maxRow -2)
					{
						square = GetSquare(row,col-1);
						if(square.CanMoveItemOut() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row+1,col);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row+2,col);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);

                        //多余搜索
                        if (row > 0 && nextMoveItems.Count >= 3)
                        {
                            square = GetSquare(row - 1, col);
                            if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                                nextMoveItems.Add(square.item);
                        }
                        if (row > 1 && nextMoveItems.Count >= 4)
                        {
                            square = GetSquare(row - 2, col);
                            if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                                nextMoveItems.Add(square.item);
                        }
					}
                    diction = SwipeDirection.Right;
                    if (nextMoveItems.Count >= 3 && CanRemoveEleListBySameColor(nextMoveItems))
						return nextMoveItems;
					else
						nextMoveItems.Clear();
					
					//o x
					//  o
					//  o
					if(col > 0 && row > 1)
					{
						square = GetSquare(row,col-1);
						if(square.CanMoveItemOut() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row-1,col);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row-2,col);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);

                        //多余搜索
                        if (row < maxRow - 1 && nextMoveItems.Count >= 3)
                        {
                            square = GetSquare(row + 1, col);
                            if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                                nextMoveItems.Add(square.item);
                        }
                        if (row < maxRow - 2 && nextMoveItems.Count >= 4)
                        {
                            square = GetSquare(row + 2, col);
                            if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                                nextMoveItems.Add(square.item);
                        }
					}
                    diction = SwipeDirection.Right;
                    if (nextMoveItems.Count >= 3 && CanRemoveEleListBySameColor(nextMoveItems))
						return nextMoveItems;
					else
						nextMoveItems.Clear();
					
					//o-x-o-o
					if(col < maxCol-2 && col > 0)
					{
						square = GetSquare(row,col-1);
						if(square.CanMoveItemOut() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row,col+1);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row,col+2);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);

                        //多余搜索
                        //if (row > 0 && nextMoveItems.Count >= 3)
                        //{
                        //    square = GetSquare(row - 1, col);
                        //    if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                        //        nextMoveItems.Add(square.item);
                        //}
                        //if (row < maxRow - 1 && nextMoveItems.Count >= 3)
                        //{
                        //    square = GetSquare(row + 1, col);
                        //    if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                        //        nextMoveItems.Add(square.item);
                        //}
					}
                    diction = SwipeDirection.Right;
                    if (nextMoveItems.Count >= 3 && CanRemoveEleListBySameColor(nextMoveItems))
						return nextMoveItems;
					else
						nextMoveItems.Clear();
					
					//o-o-x-o
					if(col < maxCol-1 && col > 1)
					{
						square = GetSquare(row,col+1);
						if(square.CanMoveItemOut() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row,col-1);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row,col-2);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);

                        //多余搜索
                        //if (row > 0 && nextMoveItems.Count >= 3)
                        //{
                        //    square = GetSquare(row - 1, col);
                        //    if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                        //        nextMoveItems.Add(square.item);
                        //}
                        //if (row < maxRow - 1 && nextMoveItems.Count >= 3)
                        //{
                        //    square = GetSquare(row + 1, col);
                        //    if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                        //        nextMoveItems.Add(square.item);
                        //}
					}
                    diction = SwipeDirection.Left;
                    if (nextMoveItems.Count >= 3 && CanRemoveEleListBySameColor(nextMoveItems))
						return nextMoveItems;
					else
						nextMoveItems.Clear();
					//o
					//x
					//o
					//o
					if(row < maxRow - 1 && row > 1)
					{
						square = GetSquare(row+1,col);
						if(square.CanMoveItemOut() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row-1,col);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row-2,col);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);

                        //多余搜索
                        //if (col > 0 && nextMoveItems.Count >= 3)
                        //{
                        //    square = GetSquare(row, col  - 1);
                        //    if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                        //        nextMoveItems.Add(square.item);
                        //}
                        //if (col < maxCol - 1 && nextMoveItems.Count >= 3)
                        //{
                        //    square = GetSquare(row, col + 1);
                        //    if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                        //        nextMoveItems.Add(square.item);
                        //}
					}
                    diction = SwipeDirection.Down;
                    if (nextMoveItems.Count >= 3 && CanRemoveEleListBySameColor(nextMoveItems))
						return nextMoveItems;
					else
						nextMoveItems.Clear();
					
					//o
					//o
					//x
					//o
					if(row < maxRow - 2 && row > 0)
					{
						square = GetSquare(row-1,col);
						if(square.CanMoveItemOut() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row+1,col);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row+2,col);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);

                        //多余搜索
                        //if (col > 0 && nextMoveItems.Count >= 3)
                        //{
                        //    square = GetSquare(row, col - 1);
                        //    if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                        //        nextMoveItems.Add(square.item);
                        //}
                        //if (col < maxCol - 1 && nextMoveItems.Count >= 3)
                        //{
                        //    square = GetSquare(row, col + 1);
                        //    if (square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
                        //        nextMoveItems.Add(square.item);
                        //}
					}
                    diction = SwipeDirection.Up;
                    if (nextMoveItems.Count >= 3 && CanRemoveEleListBySameColor(nextMoveItems))
						return nextMoveItems;
					else
						nextMoveItems.Clear();
					//  o
					//o x o
					if( row < maxRow -1 && col > 0 && col< maxCol -1)
					{
						square = GetSquare(row+1,col);
						if(square.CanMoveItemOut() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row,col-1);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row,col+1);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
					}
                    diction = SwipeDirection.Down;
                    if (nextMoveItems.Count >= 3 && CanRemoveEleListBySameColor(nextMoveItems))
						return nextMoveItems;
					else
						nextMoveItems.Clear();
					//o x o
					//  o
					if( row > 1 && col > 0 && col< maxCol -1)
					{
						square = GetSquare(row-1,col);
						if(square.CanMoveItemOut() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row,col-1);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row,col+1);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
					}
                    diction = SwipeDirection.Up;
                    if (nextMoveItems.Count >= 3 && CanRemoveEleListBySameColor(nextMoveItems))
						return nextMoveItems;
					else
						nextMoveItems.Clear();
					//  o
					//o x 
					//  o
					if( row > 0 && row< maxRow -1 &&  col > 1)
					{
						square = GetSquare(row,col-1);
						if(square.CanMoveItemOut() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row-1,col);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row+1,col);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
					}
                    diction = SwipeDirection.Right;
                    if (nextMoveItems.Count >= 3 && CanRemoveEleListBySameColor(nextMoveItems))
						return nextMoveItems;
					else
						nextMoveItems.Clear();
					//  o
					//  x o
					//  o
					if( row > 0 && row < maxRow -1 && col < maxCol -1)
					{
						square = GetSquare(row,col+1);
						if(square.CanMoveItemOut() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row-1,col);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
						square = GetSquare(row+1,col);
						if(square.CanEliminateItemByCheck() && square.item.tab_ele.EleColor == tab_ele.EleColor)
							nextMoveItems.Add(square.item);
					}
                    diction = SwipeDirection.Left;
                    if (nextMoveItems.Count >= 3 && CanRemoveEleListBySameColor(nextMoveItems))
						return nextMoveItems;
					else
						nextMoveItems.Clear();

				}
			}
			
		}
		return nextMoveItems;
	}
	
	#region ELIMILATE

	//游戏结束前,消除某些特效元素;
	public void RemoveSpec(OnCombinedCallback callback){
		StartCoroutine (OnRemoveSpec(callback));
	}

	private IEnumerator OnRemoveSpec(OnCombinedCallback callback){
		OnCombinedCallback onCombinedCallback = callback;
		List<Combine> combines = new List<Combine>();
		List<UIEliminateItemView> itemList= new List<UIEliminateItemView>();
		
		for (int i = 0; i < maxRow; i++)
		{
			for (int j = 0; j < maxCol; j++)
			{
				UISquareView square = GetSquare(i, j);
				
				if (square.item != null && square.item.tab_ele.AffectType != -1)//对其他产生影响;
				{
					itemList.Add(square.item);
				}
			}
		}
		Combine combine = new Combine();
		combine.items.AddRange(itemList);
		combines.Add(combine);
		yield return StartCoroutine(RemoveCombines(combines));
		yield return StartCoroutine(OnEliminateAndDrop());

		if(onCombinedCallback != null){
			SystemConfig.Log("OnUseStep");
			onCombinedCallback();
		}
	}



	//游戏结束前使用步数进行消除;
	public void UseStep(OnCombinedCallback callback,int stepMax,EffectBase flyIns){
		StartCoroutine (OnUseStep(callback,stepMax,flyIns));
	}
	
	private IEnumerator OnUseStep(OnCombinedCallback callback,int stepMax,EffectBase flyIns){
		OnCombinedCallback onCombinedCallback = callback;

        List<Combine> combines = new List<Combine>();
		List<UIEliminateItemView> changedItem= new List<UIEliminateItemView>();
		
		for(int i=0;i<stepMax;){
			int row = Random.Range(0,maxRow);
			int col = Random.Range(0,maxCol);
			UISquareView square = GetSquare(row,col);
            if (square.CanEliminateItemByCheck() 
				&& square.item.tab_ele.AffectType < 0
                && !changedItem.Contains(square.item))//基本元素;
            {

                if (square.item.tab_ele.FourElecol != -1 || square.item.tab_ele.FourElerow != -1)
                {
					changedItem.Add(square.item);

					EffectBase fly = flyIns.Duplicate();
					fly.StartPos = EleUIController.Instance.flyStartPos.transform;
					fly.DirectionPoint = square.transform.position;
					fly.EndPos = square.transform;
					fly.MoveSpeed = true;
					fly.EffectEnd = delegate(EffectBase effect, GameObject target, float total_time){
						SystemConfig.Log("animation completed");
						EleUIController.Instance.limitAmount--;
						UIEliminateItemView item = target.GetComponent<UIEliminateItemView>();
                        item.ChangeToSpec();
					};
                    SoundEffect.Instance.PlaySound(SoundEffect.stepToEffect);
					fly.Play(square.item.gameObject);
                    yield return new WaitForSeconds(EliminateLogic.Instance.GetEliminatePlayer().flyIns.durationTime * 0.75f);
					i++;
				}
            }
		}

        yield return new WaitForSeconds(EliminateLogic.Instance.GetEliminatePlayer().flyIns.delaydestroyTime);
		
		while(changedItem.Count > 0){
			Combine combine = new Combine();
            while (changedItem.Count > 0 && combine.items.Count < 2)
            {
                if (changedItem[0].tab_ele.AffectType >= 0)
                {
                    combine.items.Add(changedItem[0]);
                }
                changedItem.RemoveAt(0);
            }
			combines.Add(combine);
			yield return StartCoroutine(RemoveCombines(combines));
			if(changedItem.Count > 0){
				yield return StartCoroutine(OnEliminateAndDrop());
			}
			for(int i=0;i<changedItem.Count;){
				if(changedItem[i].staySquare == null){
					changedItem.RemoveAt(i);
				}else{
					i++;
				}
			}
		}		
		
		
		if(onCombinedCallback != null){
			SystemConfig.Log("OnUseStep");
			onCombinedCallback();
		}		
		
	}
	
	
	
	//消除面板内所有元素;
	public void RemoveAllItems(OnCombinedCallback callback){

		StartCoroutine (OnRemoveAllItems(callback));
	}
	
	private IEnumerator OnRemoveAllItems(OnCombinedCallback callback){
		OnCombinedCallback onCombinedCallback = callback;

        List<Combine> combines = new List<Combine>();
		List<UIEliminateItemView> itemList= new List<UIEliminateItemView>();

		for (int i = 0; i < maxRow; i++)
        {
            for (int j = 0; j < maxCol; j++)
            {
               UISquareView square = GetSquare(i, j);
  
                if (square.item != null && square.item.tab_ele.CanElimilated == 1)//只要是元素就能被消除;
                {
                   	itemList.Add(square.item);
                }
            }
        }
        Combine combine = new Combine();
        combine.items.AddRange(itemList);
        combines.Add(combine);
        yield return StartCoroutine(RemoveCombines(combines));
		if(onCombinedCallback != null){
			SystemConfig.Log("OnRemoveAllItems");
			onCombinedCallback();
		}		
		
	}
	
	
	//高亮或爆炸互换;
	public void RemoveItemsBySpecital(OnCombinedCallback callback,UIEliminateItemView source,UIEliminateItemView target){
		StartCoroutine (OnRemoveItemsBySpecital(callback,source,target));
	}
	
	private IEnumerator OnRemoveItemsBySpecital(OnCombinedCallback callback,UIEliminateItemView source,UIEliminateItemView target){
		OnCombinedCallback onCombinedCallback = callback;

		List<Combine> combines = new List<Combine>();
		
		Combine combine = new Combine();
		//两个爆炸;
		if(source.tab_ele.AffectType == 0 && target.tab_ele.AffectType == 0){
			List<UIEliminateItemView> s_arroundList = GetAffectArroundEle(source.staySquare,2);
			//List<UIEliminateItemView> t_arroundList = GetAffectArroundEle(target);
			
			for(int i=0;i<s_arroundList.Count;i++){
				if(!combine.items.Contains(s_arroundList[i])){
					combine.items.Add(s_arroundList[i]);
				}
			}
			
            //for(int i=0;i<t_arroundList.Count;i++){
            //    if(!combine.items.Contains(t_arroundList[i])){
            //        combine.items.Add(t_arroundList[i]);
            //    }
            //}	
		
		//源:爆炸,目标:高亮
		}else if(source.tab_ele.AffectType == 0 && (target.tab_ele.AffectType == 1 || target.tab_ele.AffectType == 2)){
            List<UIEliminateItemView> tmp;
			List<UIEliminateItemView> eleList = new List<UIEliminateItemView>();
			if(target.tab_ele.AffectType == 1){
                tmp = GetAffectRowEle(target.staySquare);
                eleList.AddRange(tmp);
                if (target.staySquare.m_row > 0)
                {
                    tmp = GetAffectRowEle(GetSquare(target.staySquare.m_row - 1, target.staySquare.m_col));
                    eleList.AddRange(tmp);
                }
                if (target.staySquare.m_row < maxRow - 1)
                {
                    tmp = GetAffectRowEle(GetSquare(target.staySquare.m_row + 1, target.staySquare.m_col));
                    eleList.AddRange(tmp);
                }				
			}else if(target.tab_ele.AffectType == 2){
                tmp = GetAffectColEle(target.staySquare);
                eleList.AddRange(tmp);
                if (target.staySquare.m_row > 0)
                {
                    tmp = GetAffectColEle(GetSquare(target.staySquare.m_row, target.staySquare.m_col -1));
                    eleList.AddRange(tmp);
                }
                if (target.staySquare.m_row < maxRow - 1)
                {
                    tmp = GetAffectColEle(GetSquare(target.staySquare.m_row, target.staySquare.m_col + 1));
                    eleList.AddRange(tmp);
                }	
			}
			
			for(int i=0;i<eleList.Count;i++){
				if(!combine.items.Contains(eleList[i])){
					combine.items.Add(eleList[i]);
				}
			}
			
		//源:高亮,目标:爆炸
		}else if(target.tab_ele.AffectType == 0 && (source.tab_ele.AffectType == 1 || source.tab_ele.AffectType == 2)){
            List<UIEliminateItemView> tmp;
            List<UIEliminateItemView> eleList = new List<UIEliminateItemView>();
            if (source.tab_ele.AffectType == 1)
            {
                tmp = GetAffectRowEle(source.staySquare);
                eleList.AddRange(tmp);
                if (source.staySquare.m_row > 0)
                {
                    tmp = GetAffectRowEle(GetSquare(source.staySquare.m_row - 1, source.staySquare.m_col));
                    eleList.AddRange(tmp);
                }
                if (source.staySquare.m_row < maxRow - 1)
                {
                    tmp = GetAffectRowEle(GetSquare(source.staySquare.m_row + 1, source.staySquare.m_col));
                    eleList.AddRange(tmp);
                }
            }
            else if (source.tab_ele.AffectType == 2)
            {
                tmp = GetAffectColEle(source.staySquare);
                eleList.AddRange(tmp);
                if (source.staySquare.m_row > 0)
                {
                    tmp = GetAffectColEle(GetSquare(source.staySquare.m_row, source.staySquare.m_col - 1));
                    eleList.AddRange(tmp);
                }
                if (source.staySquare.m_row < maxRow - 1)
                {
                    tmp = GetAffectColEle(GetSquare(source.staySquare.m_row, source.staySquare.m_col + 1));
                    eleList.AddRange(tmp);
                }
            }

            for (int i = 0; i < eleList.Count; i++)
            {
                if (!combine.items.Contains(eleList[i]))
                {
                    combine.items.Add(eleList[i]);
                }
            }
		
		//两个高亮,
		}else if((source.tab_ele.AffectType == 1 || source.tab_ele.AffectType == 2)&& (target.tab_ele.AffectType == 1 || target.tab_ele.AffectType == 2)){

            List<UIEliminateItemView> rowList = GetAffectRowEle(target.staySquare);
            List<UIEliminateItemView> colList = GetAffectColEle(target.staySquare);
            List<UIEliminateItemView> s_rowList = GetAffectRowEle(source.staySquare);
            List<UIEliminateItemView> s_colList = GetAffectColEle(source.staySquare);

            for (int i = 0; i < rowList.Count; i++)
            {
                if (!combine.items.Contains(rowList[i]))
                {
                    combine.items.Add(rowList[i]);
                }
            }

            for (int i = 0; i < colList.Count; i++)
            {
                if (!combine.items.Contains(colList[i]))
                {
                    combine.items.Add(colList[i]);
                }
            }

            for (int i = 0; i < s_rowList.Count; i++)
            {
                if (!combine.items.Contains(s_rowList[i]))
                {
                    combine.items.Add(s_rowList[i]);
                }
            }

            for (int i = 0; i < s_colList.Count; i++)
            {
                if (!combine.items.Contains(s_colList[i]))
                {
                    combine.items.Add(s_colList[i]);
                }
            }		
			
		}
		if(!combine.items.Contains(source)){
			combine.items.Add(source);
		}
		
		if(!combine.items.Contains(target)){
			combine.items.Add(target);
		}
		
    	combines.Add(combine);
    	yield return StartCoroutine(RemoveCombines(combines));

		if(onCombinedCallback != null){
			SystemConfig.Log("OnRemoveItemsBySpecital");
			onCombinedCallback();
		}		
		
	}
	
	//万能与高亮/爆炸/一般互换;
	public void ChangedToSameTypeAndRemove(OnCombinedCallback callback,UIEliminateItemView miracle,UIEliminateItemView speticle){
		StartCoroutine (OnChangedToSameTypeAndRemove(callback,miracle,speticle));
	}
	
	private IEnumerator OnChangedToSameTypeAndRemove(OnCombinedCallback callback,UIEliminateItemView miracle,UIEliminateItemView speticle){
		OnCombinedCallback onCombinedCallback = callback;

		List<Combine> combines = new List<Combine>();
		List<UIEliminateItemView> changedItems= new List<UIEliminateItemView>();

        EffectBase xuanzhuan = null;
        if (speticle.tab_ele.AffectType < 0)
        {
            xuanzhuan = EliminateLogic.Instance.GetEliminatePlayer().xuanZhuanIns.Duplicate();
            xuanzhuan.StartPos = miracle.transform;
            xuanzhuan.Play(miracle.gameObject);
        }

        int m = 0;
		for (int i = 0; i < maxRow; i++)
        {
            for (int j = 0; j < maxCol; j++)
            {
               	UISquareView square = GetSquare(i, j);
                
                if (square.CanEliminateByMiracleAffect() && square.item.tab_ele.EleColor == speticle.tab_ele.EleColor)//受万能元素影响同色替换;
                {
                   	changedItems.Add(square.item);
                    if (speticle.staySquare.m_row == i && speticle.staySquare.m_col == j
                        || miracle.staySquare.m_row == i && miracle.staySquare.m_col == j)
                    {
                        continue;
                    }
					if(speticle.tab_ele.AffectType < 0){
                        square.item.MoveToTarget(miracle.transform);
					}else{
                        m++;
                        EffectBase fly = EliminateLogic.Instance.GetEliminatePlayer().flyIns.Duplicate();
                        fly.StartPos = miracle.transform;
						fly.DirectionPoint = square.transform.position;
						fly.EndPos = square.transform;
						fly.MoveSpeed = true;
						fly.EffectEnd = delegate(EffectBase effect, GameObject target, float total_time){
							SystemConfig.Log("animation completed");
							UIEliminateItemView item = target.GetComponent<UIEliminateItemView>();
                            if (speticle.tab_ele.AffectType == 0)
                            {
                                item.ChangedInitID(speticle.tab_id);
                            }
                            else
                            {
                                m = Random.Range(0, 2);
                                if (m == 0)
                                {
                                    item.ChangedInitID(item.tab_ele.FourElecol);
                                }
                                else
                                {
                                    item.ChangedInitID(item.tab_ele.FourElerow);
                                }
                            }
						};
						fly.Play(square.item.gameObject);
                        yield return new WaitForSeconds(EliminateLogic.Instance.GetEliminatePlayer().flyIns.durationTime * 0.75f);
					}
                }
            }
        }

        if (speticle.tab_ele.AffectType < 0)
        {
            while (!IsAllItemIdleOrNone())
            {
                yield return null;
            }
            xuanzhuan.SetVisible(false);
        }
        else
        {
            yield return new WaitForSeconds(EliminateLogic.Instance.GetEliminatePlayer().flyIns.delaydestroyTime);
        }
		//Combine tmp = GetOneCombine(changedItem,false);
		
		if(speticle.tab_ele.AffectType < 0){
			Combine combine = new Combine();
			combine.items.Add(miracle);
			combine.items.AddRange(changedItems);
			combines.Add(combine);
			yield return StartCoroutine(RemoveCombines(combines));
		}else{
			while(changedItems.Count > 0){
				Combine combine = new Combine();
                while (changedItems.Count > 0 && combine.items.Count < 2)
                {
                    if (changedItems[0].tab_ele.AffectType >= 0)
                    {
                        combine.items.Add(changedItems[0]);
                    }
                    changedItems.RemoveAt(0);
                }
				combines.Add(combine);
				yield return StartCoroutine(RemoveCombines(combines));
				if(changedItems.Count > 0){
					yield return StartCoroutine(OnEliminateAndDrop());
				}
				
				for(int i=0;i<changedItems.Count;){
					if(changedItems[i].staySquare == null){
						changedItems.RemoveAt(i);
					}else{
						i++;
					}
				}			
			}
		}

		if(onCombinedCallback != null){
			SystemConfig.Log("OnChangedToSameTypeAndRemove");
			onCombinedCallback();
		}
	}
	
	//来自于道具同色元素的消除;
	public void RemoveAllItemsBySameColor(int ele_id,OnCombinedCallback callback){
		StartCoroutine (OnRemoveAllItemsBySameColor(callback,ele_id));
	}
	
	private IEnumerator OnRemoveAllItemsBySameColor(OnCombinedCallback callback,int ele_id)
    {   
        //Gen square at row i, column j
		OnCombinedCallback onCombinedCallback = callback;
      
        List<Combine> combines = new List<Combine>();
		List<UIEliminateItemView> itemList = new List<UIEliminateItemView>();
		Tab_Element tab_ele = TableManager.GetElementByID(ele_id);
        for (int i = 0; i < maxRow; i++)
        {
            for (int j = 0; j < maxCol; j++)
            {
               UISquareView square = GetSquare(i, j);

               if (square.CanEliminateByMiracleAffect())
                {
					if (square.item.tab_ele.EleColor == tab_ele.EleColor)
           			 {
						itemList.Add(square.item);
					}
                }
            }
        }

		//Combine combineTmp = GetOneCombine(itemList,false);

		Combine combine = new Combine();
		combine.items.AddRange(itemList);
		/*
		for(int i=0;i<combineTmp.tmpothers.Count;i++){
			if(!combine.items.Contains(combineTmp.tmpothers[i])){
				combine.items.Add(combineTmp.tmpothers[i]);
			}
		}
		*/
        combines.Add(combine);
		
        yield return StartCoroutine(RemoveCombines(combines));
		if(onCombinedCallback != null){
			SystemConfig.Log("OnRemoveAllItemsBySameColor");
			onCombinedCallback();
		}		
    }
	
	//来自于道具锤子消除
	public void RemoveOneSelectedItem(UIEliminateItemView pitem,OnCombinedCallback callback){
		StartCoroutine (OnRemoveOneSelectedItem(callback,pitem));
	}

	private IEnumerator OnRemoveOneSelectedItem(OnCombinedCallback callback,UIEliminateItemView pitem){
		OnCombinedCallback onCombinedCallback = callback;

        List<UIEliminateItemView> itemlist = new List<UIEliminateItemView>();
        List<Combine> combines = new List<Combine>();
        Combine combine = new Combine();
        itemlist.Add(pitem);
        combine.items.AddRange(itemlist);
        combines.Add(combine);
        yield return StartCoroutine(RemoveCombines(combines));
		if(onCombinedCallback != null){
			SystemConfig.Log("OnRemoveOneSelectedItem");
			onCombinedCallback();
		}	
	}

	
	//来自于道具同行或列消除;
	public void RemoveRowOrColItems(UIEliminateItemView pitem,OnCombinedCallback callback,EquipEffectType rowType){
        StartCoroutine(OnRemoveRowOrColItems(callback, pitem, rowType));
	}

    private IEnumerator OnRemoveRowOrColItems(OnCombinedCallback callback, UIEliminateItemView pitem, EquipEffectType rowType)
    {
		OnCombinedCallback onCombinedCallback = callback;

        List<UIEliminateItemView> itemList= new List<UIEliminateItemView>();
        List<Combine> combines = new List<Combine>();
		
		int itemRow = pitem.staySquare.m_row;
		int itemCol = pitem.staySquare.m_col;
		
		if(rowType == EquipEffectType.BombRow){
            itemList = GetAffectRowEle(pitem.staySquare);
            //for(int i =0 ;i<maxCol;i++){
            //    UISquareView square = GetSquare(itemRow, i);
            //    if (square.CanEliminateByRowSpecitalAffect())
            //    {
            //        itemList.Add(square.item);
            //    }
            //}
        }
        else if (rowType == EquipEffectType.BombCol)
        {
            itemList = GetAffectColEle(pitem.staySquare);
            //for(int i =0 ;i<maxRow;i++){
            //    UISquareView square = GetSquare(i, itemCol);
            //    if (square.CanEliminateByColSpecitalAffect())
            //    {
            //         itemList.Add(square.item);
            //    }
            //}

        }
        else if (rowType == EquipEffectType.BomEffect)
        {
            itemList = GetAffectArroundEle(pitem.staySquare);
        }
		//Combine combineTmp = GetOneCombine(itemList,false);

		Combine combine = new Combine();
		combine.items.AddRange(itemList);
		/*
		for(int i=0;i<combineTmp.tmpothers.Count;i++){
			if(!combine.items.Contains(combineTmp.tmpothers[i])){
				combine.items.Add(combineTmp.tmpothers[i]);
			}
		}
		*/
        combines.Add(combine);
        yield return StartCoroutine(RemoveCombines(combines));
		
		if(onCombinedCallback != null){
			SystemConfig.Log("OnRemoveRowOrColOrEffectItems");
			onCombinedCallback();
		}
	}

	//来源于检测的消除;
	public void EliminateFromCheck(OnCombinedCallback callback){
		StartCoroutine (OnEliminateFromCheck(callback));
	}	
	
	/// <summary>
	/// 消除当前地图中可以消除的元素;
	/// </summary>
	/// <returns>
	/// The combine.
	/// </returns>
    IEnumerator OnEliminateFromCheck(OnCombinedCallback callback, float delayTime = 0)
    {
		OnCombinedCallback onCombinedCallback = callback;
		yield return  StartCoroutine (OnEliminateAndDrop());
		if(onCombinedCallback != null){
			SystemConfig.Log("EliminateFromCheck");
			onCombinedCallback();
		}		
	}

	#endregion
	
	public void RemoveMap()
	{
		//Remove all squares
		for(int i=0; i<squares.Count; i++)
		{
			squares[i].RemoveAllObjects();
            squares[i].Destroy(false);
		}
		squares.Clear();
		producedBomNum = 0;

        ClearLastRow();
	}	
}
