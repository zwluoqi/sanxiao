using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GCGame.Table;


public class UISquareView:MonoBehaviour
{
	//UI
    public UIEliminateItemView item { get; set; }
    public UIProgressBar hpbar;

	private List<GameObject>  squareBackgrounds		= new List<GameObject> ();
	private List<GameObject> blockObjects 	= new List<GameObject>();

	//DATA
	public bool flag{get;set;}
	private SquareItem squareObject = null;
	public int m_row {get{return squareObject.row;} set{squareObject.row = value;}}//行;
	public int m_col {get{return squareObject.col;} set{squareObject.col = value;}}//列;
	public Tab_Square tab_square{get{return squareObject.tab_square;}}
    public int tab_id { 
        get { return squareObject.tab_id; } 
        set { squareObject.tab_id = value; 
            square_hp = tab_square.DefaultHP; } }

    public int square_hp = 0;

	public static string prefabStr = "Game/SquareItem";
    public static string parentPrefabStr = "Game/Square";
    public static string mapEditorParentPrefabStr = "Game/EditorSquare";
	
	public static int BORDER_WIDTH = 88;//白底快的宽度;
	public static int BACK_WIDTH = 78;//每个格子的宽度;
	public static int SQUARE_WIDTH 			= 78;//方块的大小;
	public static int square_border_depth		= 1;//格子外框图的图层;
	public static int square_back_depth			= 2;//格子内背景图的图层;
	public static int square_item_down_depth		= 10;//格子在元素下方的图层起始;
	public static int square_item_depth			= 20;//元素的图层;
    public static int square_item_high_depth    = 80;//高亮元素的图层;
	public static int square_item_up_depth		= 40;//格子在元素上方图层起始;
	public static int fly_depth					=100;//飞行图标;

#if UNITY_EDITOR

    void Awake(){
        UIEventListener.Get(gameObject).onPress += OnPressEvent;
	}	
	void OnPressEvent(GameObject pressed,bool isPressed)
	{

		if(isPressed && MapEditor.Instance != null)
		{
			MapEditor.Instance.ClickOnSquare(this);
		}

	}	
	#endif	
	//Init square by row, col and square type
	public void InitData(int pRow, int pCol, int initID,bool diFlag)
	{
		squareObject = new SquareItem();
		m_row = pRow;
		m_col = pCol;
        SetInitID(initID);
		blockObjects = new List<GameObject>();
        transform.localPosition = this.SquarePosition;
        LoadObjects(diFlag);
	}

    public void SetInitID(int newid)
    {
        tab_id = newid;
        squareObject.particles.Clear();

        EffectBase particle;
        for (int i = 0; i < 6; i++)
        {
            int tab_parID = tab_square .GetParticlebyIndex(i);
            if (tab_parID != -1)
            {
                //Tab_ParticleAnimation tab_par = TableManager.GetParticleAnimationByID(tab_parID);
                EffectManager.Instance.m_EffectCacheList.TryGetValue(tab_parID, out particle);
                if (particle != null)
                {
                    particle = particle.Duplicate();
                    particle.LoadResource();
                    squareObject.particles.Add(particle);
                }
            }
        }
        if (tab_square.ProduceParticle != -1)
        {
            EffectManager.Instance.m_EffectCacheList.TryGetValue(tab_square.ProduceParticle, out particle);
            squareObject.produceParticle = particle;
        }
        else
        {
            squareObject.produceParticle = null;
        }
    }

	//Remove all object in this square
	public void RemoveAllObjects()
	{
		//Remove background
		for(int i=0; i< squareBackgrounds.Count; i++)
		{
			if(squareBackgrounds[i])
				WidgetBufferManager.Instance.DestroyWidgetObj(prefabStr,squareBackgrounds[i]);
		}
		squareBackgrounds.Clear();
		
		//Remove blocks if have
		for(int i=0; i< blockObjects.Count; i++)
		{
			if(blockObjects[i])
				WidgetBufferManager.Instance.DestroyWidgetObj(prefabStr,blockObjects[i]);
		}
		blockObjects.Clear();
		//Remove item if have
		if(item != null)
		{
			item.DestroyObj();
			item = null;
		}
	}

    public void Destroy(bool mapEditor = false)
    {
#if UNITY_EDITOR
        if (mapEditor)
        {
            WidgetBufferManager.Instance.DestroyWidgetObj(mapEditorParentPrefabStr, gameObject);
        }
        else
        {
            WidgetBufferManager.Instance.DestroyWidgetObj(parentPrefabStr, gameObject);
        }
#else
        WidgetBufferManager.Instance.DestroyWidgetObj(parentPrefabStr,gameObject);
#endif

    }
	
	//Load all game objects in this square
	public void LoadObjects(bool bFlg)
	{
		flag = bFlg;
		GameObject obj = null;

		if(tab_square.IfwaiKuang == 1){
			obj = CreateSquareObject(1);
            obj.GetComponentInChildren<UISprite>().width = BORDER_WIDTH;
            obj.GetComponentInChildren<UISprite>().height = BORDER_WIDTH;
            obj.GetComponentInChildren<UISprite>().spriteName = "baisedi";
			obj.name = "baikuang";
			AddBackground(obj);
			
			obj = CreateSquareObject(1);
            obj.GetComponentInChildren<UISprite>().spriteName = bFlg ? "beijing1" : "beijing2";
			AddBackground(obj);
			
			int currentState = (int)tab_id;
			int nextState = tab_square.SquareAtDown;
			Stack<GameObject> tmpGmj = new Stack<GameObject>();
			while(currentState > 1){

				obj = CreateSquareObject(currentState);				
				tmpGmj.Push(obj);
				
				currentState = nextState;
				if(nextState != -1){
					nextState = TableManager.GetSquareByID(currentState).SquareAtDown;
				}
			}
			
			while(tmpGmj.Count >0){
				AddObject(tmpGmj.Pop());
			}
		}else{
			obj = CreateSquareObject(tab_id);
			AddBackground(obj);
		}
        this.name = string.Format("Square row{0},col{1},type{2}", m_row, m_col, tab_id.ToString());
    }
	
	/// <summary>
	/// Adds the background.
	/// </summary>
	/// <param name='obj'>
	/// Object.
	/// </param>
	private void AddBackground(GameObject obj)
	{
		if(squareBackgrounds.Count == 0)
		{
            obj.GetComponentInChildren<UISprite>().depth = square_border_depth;
		}
		else
		{
            obj.GetComponentInChildren<UISprite>().depth = square_back_depth;
		}
		squareBackgrounds.Add( obj);
	}

	public GameObject CreateSquareObject(int type){
		Tab_Square tab_sqa = TableManager.GetSquareByID(type);
        GameObject obj = WidgetBufferManager.Instance.loadWidget(prefabStr, transform);
        obj.GetComponentInChildren<UISprite>().spriteName = tab_sqa.SpriteName;
        obj.GetComponentInChildren<UISprite>().width = SQUARE_WIDTH;
        obj.GetComponentInChildren<UISprite>().height = SQUARE_WIDTH;
		obj.name = type.ToString();
		return obj;
	}

    public GameObject GetTopSquareObject()
    {
		int topLayer = blockObjects.Count-1;
        if (topLayer >= 0)
        {
            return blockObjects[topLayer];
        }
        else
        {
            return null;
        }
    }
	
	/// <summary>
	/// Adds block object into this square.
	/// </summary>
	/// <param name='obj'>
	/// Object.
	/// </param>
	private void AddObject(GameObject obj)
	{
		int squ_id = int.Parse(obj.name);
		if(TableManager.GetSquareByID(squ_id).Updown == 0){
			obj.GetComponentInChildren<UISprite>().depth = square_item_down_depth + blockObjects.Count;
		}else{
            obj.GetComponentInChildren<UISprite>().depth = square_item_up_depth + blockObjects.Count;
		}
		blockObjects.Add(obj);
        this.name = string.Format("Square row{0},col{1},type{2}", m_row, m_col, tab_id.ToString());
	}


	public void RemoveBlock(bool attacked = false)
	{
		int topLayer = blockObjects.Count-1;
		if(topLayer >=0)
		{
            square_hp--;
            //进度条血量实时显示
            if (LevelData.mode == CopyMode.TAFANG)
            {
                if (LevelData.tfData.tfDiRenIDs.Contains(tab_id) || (LevelData.tfData.tfyxID == tab_id && LevelData.tfData.tfDiRenIDs.Count > 0))
                {
                    hpbar.backgroundWidget.alpha = 1;
                    hpbar.value = square_hp * 1.0f / tab_square.DefaultHP;
                }
                else
                {
                    hpbar.backgroundWidget.alpha = 0;
                }
            }

            if (square_hp > 0 && !attacked)
            {
                return;
            }

            SoundEffect.Instance.PlaySound(tab_square.EliminateSound);

            //数据
            FireElimilateData(blockObjects[topLayer]);


            //攻击他人自身消除特效;
            if (squareObject.particles.Count != 0 && attacked)
            {
                foreach (EffectBase effect in squareObject.particles)
                {
                    effect.StartPos = blockObjects[topLayer].transform;
                    effect.MoveSpeed = false;
                    effect.DirectionPoint = blockObjects[topLayer].transform.position;
                    effect.EffectEnd = null;
                    effect.Play(null);
                }
            }

			//删除顶层;
			blockObjects.RemoveAt(topLayer);
			Map.Instance.removedSquareIDList.Add(tab_id);
			//转换到下一层类型;
			if(blockObjects.Count >= 1){

                SetInitID(int.Parse(blockObjects[blockObjects.Count - 1].name));
			}else{

                SetInitID(1);
			}
			this.name = string.Format("Square row{0},col{1},type{2}",m_row,m_col,tab_id.ToString());
		}
        UpdateSquareAndItemState();

	}


    private void FireElimilateData(GameObject topDestroy)
    {
        //分数;
        if (tab_square.BaseScore > 0)
        {
            MissionManager.Instance.AddScore(tab_square.BaseScore);
            EleUIController.Instance.ShowOneScore(transform.localPosition, tab_square.ScoreColor + "+" + tab_square.BaseScore);
        }

        //任务;
        GameObject destroyObj = topDestroy;
        if (tab_square.MissionID != -1 && MissionManager.Instance.HasMission(tab_square.MissionID))
        {
            //有任务是飞向任务栏;
            MissionManager.Instance.AddFri(tab_square.MissionID, tab_square.MissionNum);
            Vector3 pos = EleUIController.Instance.GetMissionIconWolrdPos(tab_square.MissionID);
            EleUIController.Instance.ProcessMissionFlyItem(transform.position, pos, TableManager.GetMissionByID(tab_square.MissionID).SpriteName);
            WidgetBufferManager.Instance.DestroyWidgetObj(prefabStr, destroyObj);
        }
        else
        {
            //无任务时产生动画;
            destroyObj.GetComponentInChildren<Animation>().Play("BrickRotate");
            destroyObj.AddComponent<Rigidbody>();
            destroyObj.GetComponent<Rigidbody>().useGravity = true;
            destroyObj.GetComponent<Rigidbody>().AddRelativeForce(Random.insideUnitCircle.x * Random.Range(30, 40), Random.Range(100, 150), 0);
            TimeEventObject timeEvent = destroyObj.AddMissingComponent<TimeEventObject>();
            timeEvent.SetOnTimeFinished(delegate(GameObject go)
            {
                GameObject.Destroy(go.GetComponent<Rigidbody>());
                WidgetBufferManager.Instance.DestroyWidgetObj(prefabStr, go);
            }, EliminateLogic.Instance.dropedSquareDeltaTime);
        }

        //数据;
        if (tab_square.DataID != -1)
        {
            if (tab_square.DataID == (int)DataType.jinbi)
            {
                EliminateLogic.Instance.GetEliminatePlayer().getGoldNum += 1;
            }
            else if (tab_square.DataID == (int)DataType.zhuanshi)
            {
                EliminateLogic.Instance.GetEliminatePlayer().getZhuanshiNum += 1;
            }
            else if (tab_square.DataID == (int)DataType.power)
            {
                EliminateLogic.Instance.GetEliminatePlayer().getPower += 1;
            }
        }

        //道具;
        if (tab_square.EquipID != -1)
        {
            LocalDataBase.Instance().AddEquipTmpNum((EquipEnumID)tab_square.EquipID, tab_square.EquipNum);
        }
    }

	public void ChangeToOtherType(int newType){
		RemoveAllObjects();
        SetInitID(newType);
		LoadObjects(flag);
	}
	
    /// <summary>
    /// 添加新方块类型,属性继承
    /// </summary>
    /// <param name="newType"></param>
    /// <param name="startSqu"></param>
	public void AddAtTop(int newType,UISquareView startSqu = null){

        SetInitID(newType);
		GameObject topObject = CreateSquareObject(newType);
		if(startSqu != null){
			iTweenHandler.PlayToPos(topObject,startSqu.gameObject.transform.position,
			                        topObject.gameObject.transform.position,
                                    EliminateLogic.Instance.moveinDeltaTime, true);
            topObject.GetComponentInChildren<Animation>().Play("ItemMove");
            square_hp = startSqu.square_hp;
		}
		AddObject(topObject);
        UpdateSquareAndItemState();
	}



    /// <summary>
    /// 移除最上层方块,属性改变;
    /// </summary>
    public void RemoveAtTop()
    {
		int topLayer = blockObjects.Count-1;
		if(topLayer >=0)
		{
			GameObject destroyObj = blockObjects[topLayer];
			blockObjects.RemoveAt(topLayer);
			WidgetBufferManager.Instance.DestroyWidgetObj(prefabStr,destroyObj);		
			//转换到下一层类型;
			if(blockObjects.Count >= 1){

                SetInitID(int.Parse(blockObjects[blockObjects.Count - 1].name));
			}else{

                SetInitID(1);
			}
		}
        UpdateSquareAndItemState();
	}

		public void UpdateDirection(UISquareView dir){
				GameObject obj = GetTopSquareObject();
				UISprite sprite = obj.GetComponentInChildren<UISprite>();
				if(sprite != null){
						if(this.m_col < dir.m_col){
								sprite.transform.localScale = new Vector3(-1,1,1);
						}else{
								sprite.transform.localScale = new Vector3(1,1,1);
						}

				}

		}

		public void UpdateDir(){
				if (LevelData.mode == CopyMode.TAFANG)
				{
						GameObject topObject = GetTopSquareObject();

						if (LevelData.tfData.tfDiRenIDs.Contains(tab_id))
						{
								List<UISquareView> yxs = Map.Instance.GetSquares(LevelData.tfData.tfyxID);

								if (yxs.Count > 0 && topObject != null)
								{
										UISquareView yx = yxs[0];

										if (yx.m_col < this.m_col)
										{
												topObject.transform.localScale = new Vector3(-1, 1, 1);
										}else{
												topObject.transform.localScale = new Vector3(1, 1, 1);
										}
								}
						}

						if(LevelData.tfData.tfyxID == this.tab_id && Map.Instance.tafangPath.Count > 0 && topObject != null){

								UISquareView dirNext = Map.Instance.tafangPath[Map.Instance.tafangPath.Count - 1];
								if(this.m_col < dirNext.m_col){
										topObject.transform.localScale = new Vector3(-1, 1, 1);

								}else{
										topObject.transform.localScale = new Vector3(1, 1, 1);
								}
						}
				}
		}

    public void UpdateSquareAndItemState()
    {
        if(item != null){
            //元素在下方;
            item.UpdateEleDepth();
        }
				UpdateDir();

				if (LevelData.mode == CopyMode.TAFANG)
				{

			if (LevelData.tfData.tfDiRenIDs.Contains(tab_id))
            {
                hpbar.backgroundWidget.alpha = 1;
                UISprite hpBarSprite = hpbar.foregroundWidget as UISprite;
                hpbar.value = square_hp * 1.0f / tab_square.DefaultHP;
                hpBarSprite.spriteName = "guaidexue2";
                hpbar.foregroundWidget.MakePixelPerfect();
                ((UISprite)hpbar.backgroundWidget).spriteName = "guaidexue1";
                hpbar.backgroundWidget.MakePixelPerfect();
			}else if(LevelData.tfData.tfyxID == tab_id&&LevelData.tfData.tfDiRenIDs.Count>0){
                hpbar.backgroundWidget.alpha = 1;
                UISprite hpBarSprite = hpbar.foregroundWidget as UISprite;
                hpbar.value = square_hp * 1.0f / tab_square.DefaultHP;
                hpBarSprite.spriteName = "xiaoxuecao2";
                hpbar.foregroundWidget.MakePixelPerfect();
                ((UISprite)hpbar.backgroundWidget).spriteName = "xiaoxuecao1";
                hpbar.backgroundWidget.MakePixelPerfect();
            }
            else
            {
                hpbar.backgroundWidget.alpha = 0;
            }
        }
        else
        {
            hpbar.backgroundWidget.alpha = 0;
        }

    }

    /// <summary>
    /// 顶层方块移动至目标方块上方;
    /// </summary>
    /// <param name="target"></param>
    public void AttackOtherSquare(UISquareView target)
    {
        if (target != null)
        {
            GameObject topObject = GetTopSquareObject();
            if (topObject != null)
            {
                iTweenHandler.PlayToPos(topObject, this.gameObject.transform.position,
                                        target.gameObject.transform.position,
                                        EliminateLogic.Instance.moveinDeltaTime, true);
            }
        }
    }
	
	/// <summary>
	/// Gets the neighbors of this square.
	/// </summary>
	/// <returns>
	/// The neighbors.
	/// </returns>
	public List<UISquareView> GetNeighbors()
	{
		List<UISquareView> squares = new List<UISquareView>();
		if(m_row < Map.maxRow-1){
			squares.Add(Map.Instance.GetSquare(m_row+1,m_col));
		}
		if(m_row > 0){
			squares.Add(Map.Instance.GetSquare(m_row-1,m_col));
		}
		if(m_col > 0){
			squares.Add(Map.Instance.GetSquare(m_row,m_col-1));
		}
		if(m_col < Map.maxCol-1){
			squares.Add(Map.Instance.GetSquare(m_row,m_col+1));
		}
		return squares;
	}
	
	//用于横排四消连带;
	public bool CanEliminateByRowSpecitalAffect(){
		if(item != null && item.tab_ele.ElimilateByrow == 1&& tab_square.ElimilateBySpecial == 1)
			return true;
		return false;
	}
	
	//用于纵列四消连带;
	public bool CanEliminateByColSpecitalAffect(){
		if(item != null && item.tab_ele.ElimilateBycol == 1&& tab_square.ElimilateBySpecial == 1)
			return true;
		return false;
	}
	
	//用于爆炸四消连带;
	public bool CanEliminateByBomSpecitalAffect(){
		if(item != null && item.tab_ele.ElimilateBybom == 1&& tab_square.ElimilateBySpecial == 1)
			return true;
		return false;
	}
	
	//万能元素交换连带;
	public bool CanEliminateByMiracleAffect(){
		if(item != null && item.tab_ele.ElimilateByMiracle == 1&& tab_square.ElimilateByMiracle == 1)
			return true;
		return false;
	}		
	
	//道具选择连带;
	public bool CanEliminateByEquipAffect(){
		if(item != null && item.tab_ele.ElimilateByEquip == 1&& tab_square.ElimilateByEquip == 1 )
			return true;
		return false;
	}
	
	//能够通过检测消除的对象(不包括道具,四消连带,万能元素交换连带,仅用于单纯检测);
	public bool CanEliminateItemByCheck(){
		if(item != null && item.tab_ele.ElimilateByCheck == 1 && tab_square.ElimilateByCheck ==1)
			return true;
		return false;
	}
	
	
	/// <summary>
	/// 判断该位置是否能存放元素,即
	/// 按照规则,当位置不存在item且位置属性不为tmpty与block_stone时能存放元素
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance can move item in; otherwise, <c>false</c>.
	/// </returns>
	public bool CanStayItemIn()
	{
		//If there is item, cannot move other item in
		if(item == null){
			return tab_square.Updown != -1;
		}
		return false;
	}
	
	/// <summary>
	/// 判断该位置元素能否移动;
	/// 按照规则，当存在元素item且位置类型为normal或block该位置能移动
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance can move item out; otherwise, <c>false</c>.
	/// </returns>
	public bool CanMoveItemOut()
	{
		if(item != null){
			return tab_square.CanMove == 1;
		}
		
		return false;
	}
	

	
	/// <summary>
	/// 判断上方元素能否通过此位置往下掉落;
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance can move item through; otherwise, <c>false</c>.
	/// </returns>
	public bool CanMoveItemThrough()
	{
		//If square is stone, cannot move throught
		return tab_square.CanThrough == 1 && item == null;
	}

  
	#region "POSITION FUNCTIONS"
	public int PositionX
	{
		get
		{
			return Map.MAP_START_X + m_col * BACK_WIDTH;
		}
	}
	public int PositionY
	{
		get
		{
			return Map.MAP_START_Y + m_row * BACK_WIDTH;
		}
	}
	public int PositionZ
	{
		get
		{
			return 0;
		}
	}
	public Vector3 SquarePosition
	{
		get
		{
			return new Vector3(PositionX, PositionY, PositionZ);
		}
	}
	
    //当前位置下方坐标
	public Vector3 DownSquarePosition{
		get{
			return new Vector3(PositionX,PositionY - BACK_WIDTH,PositionZ-2);
		}
	}
	#endregion
	
	//产生一个Square
	public static UISquareView LoadSquare(int pRow, int pCol, int tab_squareID,bool flag,Transform parent,bool mapEditor = false)
	{
        GameObject obj = null;
#if UNITY_EDITOR
        if (mapEditor)
        {
            obj = WidgetBufferManager.Instance.loadWidget(mapEditorParentPrefabStr, parent);
        }
        else
        {
            obj = WidgetBufferManager.Instance.loadWidget(parentPrefabStr, parent);
        }
#else
        obj = WidgetBufferManager.Instance.loadWidget(parentPrefabStr, parent);
#endif
        UISquareView square = obj.GetComponent<UISquareView>();
        square.InitData(pRow, pCol, tab_squareID,flag);

		
		return square;
	}		
}
