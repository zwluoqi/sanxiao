using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GCGame.Table;

using Random = UnityEngine.Random;
using System;

//Item class
public class UIEliminateItemView :MonoBehaviour 
{
    public static string prefabStr = "Game/Item";
    public static string bomSpriteName = "Game/zhadan";
    public static string borderSpriteName = "Game/texiaoyuansutexiao1";
    public static string lineSpriteName = "Game/texiaoyuansu2";

	//Data
	public UISquareView 	staySquare = null;//该项当前的位置;
	public UISquareView		targetSquare = null;//该项移动目标位置;
    //public bool droping = false;
	private Vector2 draggedDelta;
	private EliminateItem itemObject;
	public Tab_Element tab_ele{get {return itemObject.tab_ele;}}
	public int tab_id{get{return itemObject.type;}set{itemObject.type = value;}}
    public bool baseEle { get { return itemObject.type >= 0 && itemObject.type <= 4; } }
    public int CollectiveMissionid = -1;
    public int CollectiveMissionnum = 0;

    public UIEliminateItemState state = UIEliminateItemState.None;
    
    public delegate void OnSwapCallback();
	private OnSwapCallback onSwapCallBack = null;//交换位置后的回调函数;



	//UI
	public  GameObject ContainerObj;
	public  UISprite sprite;
	public UISprite light;
	private GameObject bom;	//附带的炸弹元素;
    private GameObject border;//特效元素附带的边框;
    private GameObject line;//特效元素附带的方向;

    EventDelayManger.EventDelay bomEventDelay;

	void Awake(){
		itemObject = new EliminateItem();
        UIEventListener.Get(gameObject).onPress += OnPressEvent;
        UIEventListener.Get(gameObject).onDrag += OnDragEvent;
	}
    
	
    void OnPressEvent(GameObject go,bool isPressed)
	{
		if(!isPressed){
			draggedDelta = Vector2.zero;
			return;
		}
		
		if(EliminateLogic.Instance.GetEliminatePlayer().GetEliminateProcedureManager()
			.GetActiveProcedure().GetProcedureType() != EliminateProcedureType.PROCEDURE_WAITING_INPUT)
			return ;
		draggedDelta = Vector2.zero;
		Map.Instance.PressOnItem(this);
	}	
	
	void OnDragEvent(GameObject go,Vector2 dic){
        if (EleUIController.Instance.selectedItem == null )
        {
            return;
        }		
		
		if(EliminateLogic.Instance.GetEliminatePlayer().GetEliminateProcedureManager()
			.GetActiveProcedure().GetProcedureType() != EliminateProcedureType.PROCEDURE_WAITING_INPUT)
			return ;		
		
		draggedDelta += dic;
		SystemConfig.Log(draggedDelta);
		if(draggedDelta.magnitude > sprite.width/4){
			Map.Instance.DragOnItem(draggedDelta);
		}
	}
	
	private  void UpdateItemSprite(){
		if(tab_ele != null){
            sprite.spriteName = tab_ele.NormalSprite;
            sprite.MakePixelPerfect();
            UpdateEleDepth();
            DeleteSpecitalParticleObj();
            AddSpecitalParticleObj();
		}else{

		}
		name  = string.Format("Item row{0},col{1},type{2}", staySquare.m_row,staySquare.m_col,tab_id);
	}


    public void ChangedInitID(int newid)
    {
        SetCurrentTabID(newid);

        if (itemObject.produceParticle != null)
        {
            EffectBase effect = itemObject.produceParticle;
            effect.StartPos = ContainerObj.transform;
            effect.MoveSpeed = false;
            effect.Play(null);
        }
    }

    public void LoadInit(int newid, UISquareView square)
    {
		staySquare = square;
		square.item = this;
		gameObject.transform.localPosition = square.SquarePosition + new Vector3(0,0,-2f);

        SetCurrentTabID(newid);
        state = UIEliminateItemState.Idle;
    }
	
	//根据类型设置item的属性;	
	public void SetCurrentTabID(int newid){
		tab_id = newid;

        CollectiveMissionid = -1;
        CollectiveMissionnum = 0;
		UpdateItemSprite();
        itemObject.particles.Clear();

        EffectBase particle;
        for (int i = 0; i < 6; i++)
        {
            int tab_parID = tab_ele.GetParticlebyIndex(i);
            if (tab_parID != -1)
            {
                //Tab_ParticleAnimation tab_par = TableManager.GetParticleAnimationByID(tab_parID);
                EffectManager.Instance.m_EffectCacheList.TryGetValue(tab_parID, out particle);
                if (particle != null)
                {
                    particle = particle.Duplicate();
                    particle.LoadResource();
                    itemObject.particles.Add(particle);
                }
            }
        }
        if (tab_ele.ProduceParticle != -1)
        {
            EffectManager.Instance.m_EffectCacheList.TryGetValue(tab_ele.ProduceParticle, out particle);
            if (particle != null)
            {
                itemObject.produceParticle = particle.Duplicate();
                itemObject.produceParticle.LoadResource(); ;
            }
        }
        else
        {
            itemObject.produceParticle = null;
        }
	}



	/// <summary>
	/// Move item into square
	/// </summary>
	/// <param name='square'>
	/// Square.
	/// </param>
	public void MoveIn(UISquareView square)
	{
        state = UIEliminateItemState.Move;

		square.item = this;
		staySquare = square;				
		name  = string.Format("Item row{0},col{1},type{2}", square.m_row,square.m_col,tab_id);
        iTweenHandler.PlayToPos(gameObject, transform.localPosition, square.SquarePosition + new Vector3(0, 0, -2f), EliminateLogic.Instance.moveinDeltaTime, false);
        EventDelayManger.Instance.CreateEvent(MoveInCompleted, EliminateLogic.Instance.moveinDeltaTime);

        SoundEffect.Instance.PlaySound(SoundEffect.move);
        MoveAnimation();
	}

    private void MoveInCompleted()
    {
        state = UIEliminateItemState.Idle;
    }
	
	//往一个位置下落,不管是左下方右下方还是正下方;
	public void DropIn(UISquareView square,float duration)
	{
        state = UIEliminateItemState.Drop;

		square.item = this;
		staySquare = square;				
		name  = string.Format("Item row{0},col{1},type{2}", square.m_row,square.m_col,tab_id);
		iTweenHandler.PlayToPos(gameObject,transform.localPosition,square.SquarePosition+new Vector3(0,0,-2f),duration,false);
        EventDelayManger.Instance.CreateEvent(DropCompleted, duration);
	}

	//模拟下落,对图片作处理,模拟图片下落的过程,实际不是正在的下落;
	public void SimulateDropIn(float duration){
        state = UIEliminateItemState.Drop;

		UIDrawRegionController drawRegion = sprite.gameObject.AddMissingComponent<UIDrawRegionController>();
		drawRegion.showType = UIDrawRegionController.ShowType.TopToBottom;
		drawRegion.duration = duration;
		drawRegion.Play();
        EventDelayManger.Instance.CreateEvent(DropCompleted, duration);
	}
	
	
	//最下一行往下落;
	public void DropLastDownSquare(float duration){
        state = UIEliminateItemState.Drop;

		name  = string.Format("Item row{0},col{1}", staySquare.m_row-1,staySquare.m_col);
		iTweenHandler.PlayToPos(gameObject,transform.localPosition,
			staySquare.DownSquarePosition,
			duration,
			false);
        EventDelayManger.Instance.CreateEvent(DropLastSquareCompleted, duration);
		staySquare = null;
	}

    public void MoveToTarget(Transform target)
    {
        state = UIEliminateItemState.FlyToTarget;
        GetComponent<ItemLerpMove>().onFinish = FlyCompleted;
        GetComponent<ItemLerpMove>().MoveToTarget(target);

    }

    private void FlyCompleted()
    {
        state = UIEliminateItemState.Idle;
    }
	
	
	//下落结束回调函数;
	private void DropCompleted(){
        state = UIEliminateItemState.Idle;
	}

    private void DropLastSquareCompleted()
    {
        state = UIEliminateItemState.None;
        FireElimilateData();
        DestroyObj();
    }
	
	
	public GameObject GetBomObj(){
		return bom;
	}
	
	public void AddBomObj(){
        bom = WidgetBufferManager.Instance.loadWidget(bomSpriteName, ContainerObj.transform);
        UISprite bomsprite = bom.GetComponent<UISprite>();
        bomsprite.depth = sprite.depth + 1;
        bomsprite.spriteName = bomSpriteName;
        bomsprite.MakePixelPerfect();

        bomEventDelay = EventDelayManger.Instance.CreateEvent(BomedSelf, 10);
	}

    private void DeleteBomObj()
    {
        if (bom != null)
        {
            WidgetBufferManager.Instance.DestroyWidgetObj(bomSpriteName, bom);
            bom = null;
        }
        else
        {

        }
    }

    private void BomedSelf()
    {
        Debug.LogWarning("BomSelf");
        EleUIController.Instance.limitAmount--;
        DeleteBomObj();
    }

    private void AddSpecitalParticleObj()
    {
        if (tab_ele.AffectType >= 0)
        {
            sprite.depth = UISquareView.square_item_high_depth;

            border = WidgetBufferManager.Instance.loadWidget(borderSpriteName, ContainerObj.transform);
            UISprite spriteBorder = border.GetComponent<UISprite>();
            //spriteBorder.spriteName = borderSpriteName;
            spriteBorder.depth = sprite.depth - 2;
            spriteBorder.MakePixelPerfect();
        }

        if (tab_ele.AffectType == (int)AffectType.ROW || tab_ele.AffectType == (int)AffectType.COL)
        {
            line = WidgetBufferManager.Instance.loadWidget(lineSpriteName, ContainerObj.transform);
            UISprite spriteLine = line.GetComponent<UISprite>();
            //spriteLine.spriteName = lineSpriteName;
            spriteLine.depth = sprite.depth - 1;
            spriteLine.MakePixelPerfect();
        }

        if (tab_ele.AffectType == (int)AffectType.ROW)
        {
            line.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
        else if (tab_ele.AffectType == (int)AffectType.COL)
        {
            line.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    /// <summary>
    /// 方块在元素下方
    /// </summary>
    /// <param name="down"></param>
    public void UpdateEleDepth( )
    {
        bool down = staySquare.tab_square.Updown == 0;
        if (down && tab_ele.AffectType >= 0)
        {
            sprite.depth = UISquareView.square_item_high_depth;
        }
        else
        {
            sprite.depth = UISquareView.square_item_depth;
        }
        if (border != null)
        {
            border.GetComponent<UISprite>().depth = sprite.depth - 2;
        }
        if (line != null)
        {
            line.GetComponent<UISprite>().depth = sprite.depth - 1;
        }
        if (bom != null)
        {
            bom.GetComponent<UISprite>().depth = sprite.depth + 1;
        }
    }

    private void DeleteSpecitalParticleObj()
    {
        if (border)
        {
            WidgetBufferManager.Instance.DestroyWidgetObj(borderSpriteName, border);
            border = null;
        }
        else
        {

        }
        if (line)
        {
            WidgetBufferManager.Instance.DestroyWidgetObj(lineSpriteName, line);
            line = null;
        }
        else
        {

        }
    }
	
	
	//与其他元素交换位置;
    public void SwapItem(UIEliminateItemView neighbor,OnSwapCallback callBack)
    {
		onSwapCallBack = callBack;
        //Do the swap with neirghbor item
        StartCoroutine(OnSwap(neighbor));
    }
	IEnumerator OnSwap(UIEliminateItemView neighbor)
	{
		OnSwapCallback tmpOnSwapCallBack = onSwapCallBack;
		//target square will be neighbor item's square
		targetSquare = neighbor.staySquare;
		//Move neighbor item to this item square
		neighbor.MoveIn(staySquare);
		//Move this item to neighbor item square in 0.5s
	    MoveIn(targetSquare);
        yield return new WaitForSeconds(EliminateLogic.Instance.moveinDeltaTime);
		
		
		if(tmpOnSwapCallBack != null){
			tmpOnSwapCallBack();
		}
	}
	

	public void HintAnimation()
	{
		ContainerObj.GetComponent<Animation>().Play("ItemHint");
	}
	public void IdleAnimation()
	{
        ContainerObj.GetComponent<Animation>().Play("ItemIdle");
	}
	public void DropAnimation(){
        ContainerObj.GetComponent<Animation>().Play("ItemDrop");
	}

    public void MoveAnimation()
    {
        ContainerObj.GetComponent<Animation>().Play("ItemMove");
    }
	
	/// <summary>
	/// Destroy this instance.
	/// </summary>
	public virtual void Destroy()
	{
		//because one item can add into more than one combine,
		//so we need to check if this item already destroyed, 
		//dont destroy it any more
		if(staySquare == null)
			return;
        SoundEffect.Instance.PlaySound(tab_ele.EliminateSound);
		//改变周边位置的属性;
		foreach(UISquareView square in staySquare.GetNeighbors())
		{
			if(square.tab_square.DisappearByOther == 1)
			{
				square.RemoveBlock();
			}
		}		
		
        //消除身上的爆炸物;
		if(bom != null){
            EventDelayManger.Instance.Delete(bomEventDelay);
            DeleteBomObj();
		}
		
		//元素消除能使方块消除,则消除方块;
		Tab_Square currentTabSquare = TableManager.GetSquareByID(staySquare.tab_id);
		if(staySquare.tab_square.DisappearBySelf == 1){
			staySquare.RemoveBlock();
		}
		
		//如果方块消除前元素不能消除,则直接返回;
		if(currentTabSquare.EliminateBeforeRemoved != 1 ){
			return ;
		}

		//消除数据相关
		FireElimilateData();


        //获取影响对象;
        List<UIEliminateItemView> attectList = new List<UIEliminateItemView>();
        if (tab_ele.AffectType == 1)
        {
            attectList = Map.Instance.GetAffectRowEle(this.staySquare);
        }
        else if (tab_ele.AffectType == 2)
        {
            attectList = Map.Instance.GetAffectColEle(this.staySquare);
        }
        else if (tab_ele.AffectType == 0)
        {
            attectList = Map.Instance.GetAffectArroundEle(this.staySquare);
        }
        else
        {
            ;
        }

		//消除特效;
		Map.Instance.eliminatingCount++ ;
        EliminateLogic.Instance.GetEliminatePlayer().eliminatingCount++;
        if (itemObject.particles.Count != 0)
        {
            foreach (EffectBase effect in itemObject.particles)
            {
                effect.StartPos = transform;
                effect.MoveSpeed = false;
                effect.DirectionPoint = transform.position;
                effect.EffectEnd = OnEffectEndEvent;
                effect.Play(null);
            }
        }
        else
        {
            OnTexiaoEffectEndEvent(null,null,0);
        }

        //消除方块与自身位置信息;
        gameObject.SetActive(false);
        if (staySquare != null)
        {
            staySquare.item = null;
            staySquare = null;
        }
        //显示消除文字
        EleUIController.Instance.ShowTextEffect(EliminateLogic.Instance.GetEliminatePlayer().eliminatingCount);

		//消除周边;
		foreach(UIEliminateItemView item in attectList){
			item.Destroy();
		}
	}

	public void FireElimilateData(){
		//分数;
		if(tab_ele.BaseScore > 0){
			MissionManager.Instance.AddScore(tab_ele.BaseScore);
            EleUIController.Instance.ShowOneScore(transform.localPosition,tab_ele.ScoreColor+ "+" + tab_ele.BaseScore);
		}
		
		//如果元素消除能完成任务;
        if (tab_ele.ProduceMissionid != -1 && MissionManager.Instance.HasMission(tab_ele.ProduceMissionid))
        {
			MissionManager.Instance.AddFri(tab_ele.ProduceMissionid,tab_ele.ProduceMissionnum);
			Vector3 pos = EleUIController.Instance.GetMissionIconWolrdPos(tab_ele.ProduceMissionid);
			EleUIController.Instance.ProcessMissionFlyItem(transform.position, pos,TableManager.GetMissionByID( tab_ele.ProduceMissionid).SpriteName);
		}

        //如果元素下落能完成任务;
        if (tab_ele.DropMissionid != -1 && MissionManager.Instance.HasMission(tab_ele.DropMissionid))
        {
            MissionManager.Instance.AddFri(tab_ele.DropMissionid, tab_ele.DropMissionNum);
            Vector3 pos = EleUIController.Instance.GetMissionIconWolrdPos(tab_ele.DropMissionid);
            EleUIController.Instance.ProcessMissionFlyItem(transform.position, pos, TableManager.GetMissionByID(tab_ele.DropMissionid).SpriteName);
        }
		
		//如果元素消除能得到掉落包;
		if(tab_ele.ProduceDropbagid != -1){
			GiftModel gift = GetGiftEquip(tab_ele.ProduceDropbagid);
			if(gift != null){
				if(gift.giftType == 1){
                    try
                    {
                        Tab_Equip tab_equip = TableManager.GetEquipByID(gift.giftID);
                        if (tab_equip == null)
                        {
                            Debug.LogError("tab_equip ID:" + gift.giftID);
                        }
                        GameObject equip = EleUIController.Instance.GetTheEquipObject((EquipEnumID)tab_equip.EnumID);
                        Vector3 pos = equip.transform.position;
                        LocalDataBase.Instance().AddEquipTmpNum((EquipEnumID)tab_equip.EnumID, gift.giftNum);
                        UIEquipView view = equip.GetComponent<UIEquipView>();
                        view.UpdateNumberLabel();
                        EleUIController.Instance.ProcessEquipFlyItem(transform.position, pos, tab_equip.SpriteName);
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message);
                    }
				}
				
			}			
		}
		
		//如果元素消除能得到奖励;
		if(tab_ele.ProduceDataid != -1){
			
			if(tab_ele.ProduceDataid == (int)DataType.jinbi){
				Vector3 pos = EleUIController.Instance.goldNumber.transform.position;
                EleUIController.Instance.ProcessDataFlyItem(transform.position, pos, SystemConfig.goldSpriteName);				
				EliminateLogic.Instance.GetEliminatePlayer().getGoldNum += 1;
			}else if(tab_ele.ProduceDataid == (int)DataType.zhuanshi){//todo
				Vector3 pos = EleUIController.Instance.goldNumber.transform.position;
                EleUIController.Instance.ProcessDataFlyItem(transform.position, pos, SystemConfig.zhuanshiSpriteName);				
				EliminateLogic.Instance.GetEliminatePlayer().getZhuanshiNum += 1;
			}else if(tab_ele.ProduceDataid == (int)DataType.power){//todo
				Vector3 pos = EleUIController.Instance.goldNumber.transform.position;
                EleUIController.Instance.ProcessDataFlyItem(transform.position, pos, SystemConfig.powerSpriteName);				
				EliminateLogic.Instance.GetEliminatePlayer().getPower += 1;
			}
		}
		
		//集体消除完成的任务;
        if (CollectiveMissionid != -1 && MissionManager.Instance.HasMission(CollectiveMissionid))
        {
			MissionManager.Instance.AddFri(CollectiveMissionid,CollectiveMissionnum);
			Vector3 pos = EleUIController.Instance.GetMissionIconWolrdPos(CollectiveMissionid);
			EleUIController.Instance.ProcessMissionFlyItem(transform.position, pos,TableManager.GetMissionByID(CollectiveMissionid).SpriteName);			
		}

        //增加步数
        if (tab_ele.RewardIdInCopy == 0 && tab_ele.RewardNumInCopy > 0)
        {
            EleUIController.Instance.limitAmount += tab_ele.RewardNumInCopy;
            EleUIController.Instance.eleTextEffect.ShowTextEffect(LanguageManger.GetMe().GetWords("L_1070") + tab_ele.RewardNumInCopy);
        }
	}

	public void OnEffectEndEvent(EffectBase effect, GameObject target, float total_time){

        foreach (EffectBase effectBase in itemObject.particles)
        {
            if (effectBase.state != EffectState.End)
            {
                return;
            }
        }
		OnTexiaoEffectEndEvent(effect,target,total_time);
		
	}

	public void OnTexiaoEffectEndEvent(EffectBase effect, GameObject target, float total_time){
		Map.Instance.eliminatingCount--;
		DestroyObj();
	}
	
	public void DestroyObj(){
        state = UIEliminateItemState.None;
        if (staySquare != null)
        {
            staySquare.item = null;
            staySquare = null;
        }
		WidgetBufferManager.Instance.DestroyWidgetObj(prefabStr,gameObject);
        DeleteSpecitalParticleObj();
	}
	
	//获取正常礼包数据;
	//0为正常礼包,1为暗箱礼包;
	private GiftModel GetGiftEquip(int dropid){
		GiftModel dropModel = null;
		Tab_Drop drop = TableManager.GetDropByID(dropid);
		if(drop != null){
			int u = Random.Range(0,100);
			int v = 0;
			for(int i=0;i<4;i++){
				int drapType = drop.GetDropTypebyIndex(i);
				if(drapType == -1){
					break;
				}
				v += drop.GetProbyIndex(i);
				if(u < v){
					dropModel = new GiftModel(drop.GetDropTypebyIndex(i),drop.GetValbyIndex(i),1);
					break;
				}
			}
		}
		
		return dropModel;
	}

	
	public void OnSelected(bool selected){
		light.gameObject.SetActive(selected);
	}

    public void ChangeToSpec()
    {
        List<int> changed = new List<int>();
        if (tab_ele.FourElecol != -1)
        {
            changed.Add(tab_ele.FourElecol);
        }
        if (tab_ele.FourElerow != -1)
        {
            changed.Add(tab_ele.FourElerow);
        }
        int count = changed.Count;
        int index = Random.Range(0, count);
        int newType = changed[index];
        ChangedInitID(newType);
    }


	
	//产生一个item
	public static UIEliminateItemView LoadGameObject(UISquareView square,int itemType, Transform parent)
	{
		

		GameObject obj = WidgetBufferManager.Instance.loadWidget(prefabStr,parent);
		UIEliminateItemView item = obj.GetComponent<UIEliminateItemView>();
        item.LoadInit(itemType, square);
		return item;
	}	
	
}
