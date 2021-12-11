using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Combine
{
	public enum CombineType{
		ROW,
		COL,
		CROSS,
	}//0是横排合并,1是纵排合并,2是交叉合并;
	
	public CombineType combinType = CombineType.ROW;
	public List<UIEliminateItemView> items = new List<UIEliminateItemView>();
	//public List<UIEliminateItemView> tmpothers = new List<UIEliminateItemView>();
	private Dictionary<UIEliminateItemView,int> changedToOtherItems = new Dictionary<UIEliminateItemView,int>();

	public void AddChangedToOtherItem(UIEliminateItemView item,int type){
		changedToOtherItems.Add(item,type);
	}
	
	
	//从左向右,从下往上排序;
	public UIEliminateItemView GetChangedItem(){
		if(items.Contains(EleUIController.Instance.selectedItem)){
			return EleUIController.Instance.selectedItem;
		}
		
		if(items.Contains(EleUIController.Instance.targetItem)){
			return EleUIController.Instance.targetItem;
		}
		items.Sort(delegate(UIEliminateItemView x, UIEliminateItemView y) {
			if(x.staySquare.m_row == y.staySquare.m_row){
				return x.staySquare.m_col.CompareTo(y.staySquare.m_col);
				//return -1;
			}else{
				//return 1;
				return x.staySquare.m_row.CompareTo(y.staySquare.m_row);
			}
		});
		
		//如果是五消,选择中间;
		//如果是四消,选择第三个;
		return items[2];
	}
	
	public void CombineItems()
	{
		foreach(UIEliminateItemView view in changedToOtherItems.Keys){
			view.FireElimilateData();
            view.ChangedInitID(changedToOtherItems[view]);
            SoundEffect.Instance.PlaySound(view.tab_ele.ProduceSound);
		}
		changedToOtherItems.Clear();
		
		while(items.Count>0)
		{
			items[items.Count-1].Destroy();
			items.RemoveAt(items.Count-1);
		}		
	}
	
	public void CullData(){
        //剔除替换的item;
		foreach(UIEliminateItemView view in changedToOtherItems.Keys){
			if(items.Contains(view)){
				items.Remove(view);
			}			
		}

        //剔除重复的item;
        List<UIEliminateItemView> newItems = new List<UIEliminateItemView>();
        foreach (UIEliminateItemView view in items)
        {
            if (!newItems.Contains(view))
            {
                newItems.Add(view);
            }
        }
        items = newItems;
	}
	
}
