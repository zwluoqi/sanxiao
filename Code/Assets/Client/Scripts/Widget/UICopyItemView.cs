using UnityEngine;
using System.Collections;

public class UICopyItemView : MonoBehaviour {
	
	public CopyDataModel copyData;

	public bool locked{get{return copyData == null?true:copyData.star == -1;}}
	
	public UISprite icon;
    //public UILabel star;
	public UILabel copyid;
    //public GameObject localTip;
    public GameObject[] stars;
	
	/// <summary>
	/// Updates the state.初始化用户界面时,
	///更新副本的状态;
	/// </summary>
    public void Init(CopyDataModel model)
    {
        copyData = model;
		SetIcon(copyData.star);
		copyid.text = copyData.copyID.ToString();
        if (model.tab_copy.OpenedLimited > LocalDataBase.GetAllStars())
        {
            icon.spriteName = "levebutonsuo";
        }
        else
        {
            icon.spriteName = "levebuton2";
        }
        icon.MakePixelPerfect();
	}
	
	private void SetIcon(int star){

        foreach (GameObject go in stars)
        {
            go.SetActive(false);
        }
        for (int i = 0; i < star; i++)
        {
            stars[i].SetActive(true);
        }
	}
	
}
