using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CopyAllStarPage : Page
{
    public UIGrid grid;
    public UICopyItemView copyItemPref;

    private List<GameObject> starList = new List<GameObject>();
    protected override void DoOpen()
    {
        ClearObjs();
        foreach (CopyDataModel model in LocalDataBase.copyModels)
        {
            if (model.star > 0 && model.star < 3)
            {
                UICopyItemView itemView = GameObject.Instantiate(copyItemPref) as UICopyItemView;
                itemView.Init(model);
                itemView.transform.parent = grid.transform;
                itemView.transform.localPosition = Vector3.zero;
                itemView.transform.localScale = Vector3.one;
                UIEventListener.Get(itemView.gameObject).onClick = OnClickCopyButton;
                starList.Add(itemView.gameObject);
            }
        }
        grid.repositionNow = true;
    }


    public override void OnUnactive()
    {
        ClearObjs();
    }

    private void ClearObjs()
    {

        foreach (GameObject go in starList)
        {
            Destroy(go);
        }

        starList.Clear();
    }

    void OnClickCopyButton(GameObject go)
    {
        UICopyItemView item = go.GetComponent<UICopyItemView>();

        if (!item.locked)
        {
            LocalDataBase.Instance().SetSelectCopyLevel(item.copyData.copyID);
            PageManager.Instance.OpenPage("CopyBeforeController", "");
        }
    }

}
