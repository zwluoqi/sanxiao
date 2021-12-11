using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopController : Page {

    public List<UIToggle> tabs;
    public GameObject rubuShop;
    public GameObject equipShop;
    public GameObject tiliShop;
    public GameObject libaoShop;
    public UILabel rubyNum;
    public GameObject equipBtn;

    //public static int tabIndex = -1;
    void Awake()
    {
        foreach (UIToggle to in tabs)
        {
            UIEventListener.Get(to.gameObject).onClick += OnTabClick;
        }

    }

    protected override void DoOpen()
    {
        int tabIndex = -1;
        if (options.ContainsKey("shopType"))
        {
            tabIndex = int.Parse(options["shopType"]);
        }

        if (tabIndex >= 0)
        {
            OnTabClick(tabs[tabIndex].gameObject);
        }
        else
        {
            OnTabClick(tabs[1].gameObject);
        }

    }

    public override void DoReopen()
    {
        DoOpen();
    }

    public void OnTabClick(GameObject go)
    {
        go.GetComponent<UIToggle>().value = true;
//		if (go.name == "zhuanshi") {
//			equipShop.SetActive (false);
//			rubuShop.SetActive (true);
//			tiliShop.SetActive (false);
//			libaoShop.SetActive (false);
//		} 
		if (go.name == "equip") {
			rubuShop.SetActive (false);
			equipShop.SetActive (true);
			tiliShop.SetActive (false);
			libaoShop.SetActive (false);
			//if (LocalDataBase.copyModels[14].star >= 0)
			//{
			//    rubuShop.SetActive(false);
			//    equipShop.SetActive(true);
			//    tiliShop.SetActive(false);
			//    libaoShop.SetActive(false);
			//}
			//else
			//{
			//    BoxManager.Instance.ShowPopupMessage(LanguageManger.GetMe().GetWords("L_Module_Level_Limited1"));
			//}

		} else if (go.name == "tili") {
			tiliShop.SetActive (true);
			rubuShop.SetActive (false);
			equipShop.SetActive (false);
			libaoShop.SetActive (false);
		} else if (go.name == "libao") {
			tiliShop.SetActive (false);
			rubuShop.SetActive (false);
			equipShop.SetActive (false);
			libaoShop.SetActive (true);

			//if (LocalDataBase.copyModels[14].star >= 0)
			//{
			//    tiliShop.SetActive(false);
			//    rubuShop.SetActive(false);
			//    equipShop.SetActive(false);
			//    libaoShop.SetActive(true);
			//}
			//else
			//{
			//    BoxManager.Instance.ShowPopupMessage(LanguageManger.GetMe().GetWords("L_Module_Level_Limited1"));
			//}
		} else {
			tiliShop.SetActive (false);
			rubuShop.SetActive (false);
			equipShop.SetActive (false);
			libaoShop.SetActive (false);
		}
    }

    public void OnClose()
    {
        this.Close();
    }


    public void ShareBtn()
    {
		ADTouTiao.Instance.RequestRewardVideo(OnPlayDone);
    }

	void OnPlayDone ()
	{
		
	}
}
