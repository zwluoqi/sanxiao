using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GCGame.Table;


public class EliminateItem  {
	public int 	type = 0;	//该项的类型ID;
	//public ItemSubType	subType = ItemSubType.normal;//该项目的子类型,一般类型或者高亮类型;
	public int 			score = 10; //消除该项目获得的分数;
	//public EffectBase sparkParticle = null;
	//public EffectBase texiaoParticle = null;

    public List<EffectBase> particles = new List<EffectBase>();
    public EffectBase produceParticle;

	public Tab_Element tab_ele{get{
			return TableManager.GetElementByID(type);
		}
		}
}

/*
public class int{
	public int 	type = int.green;	//该项的类型;
	public ItemSubType	subType = ItemSubType.normal;//该项目的子类型,一般类型或者高亮类型;
	
	public int(int itype,ItemSubType stype){
		type = itype;
		subType = stype;
	}
}
*/