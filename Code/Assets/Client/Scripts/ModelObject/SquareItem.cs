using UnityEngine;
using System.Collections;
using GCGame.Table;
using System.Collections.Generic;

/*
//Our map is one grid, so square 
public enum SquareType
{
	empty = 0,
	normal,
	block,
	block_stone,
	block_glass,
	lock_one,
	lock_two,
	lock_three,
	empty_spetical,	
}
*/
 
public class SquareItem {
	public int tab_id					= 1; //方块类型;
	public int row  						= 0;//行;
	public int col							= 0;//列;	

    public List<EffectBase> particles = new List<EffectBase>();
    public EffectBase produceParticle;
	public Tab_Square tab_square{
		get{
			return TableManager.GetSquareByID((int)tab_id);
		}
	}
}
