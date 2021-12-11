using UnityEngine;
using System.Collections;
using GCGame.Table;

public class SpeRubyPage : Page {

    public UILabel baseNum;
    public UILabel addNum;
    public UILabel costRMB;

    private int current_tab_rubyID;
    protected override void DoOpen()
    {
        current_tab_rubyID = int.Parse(options["tab_rubyID"]);
        Tab_Rubyshop rubyItem = TableManager.GetRubyshopByID(current_tab_rubyID);
        baseNum.text = rubyItem.BaseNum.ToString();
        addNum.text = rubyItem.SongNum.ToString();
        costRMB.text = rubyItem.CostRMB.ToString();
    }


    public void OnBuyBoxItems(){
        Tab_Rubyshop rubyItem = TableManager.GetRubyshopByID(current_tab_rubyID);
        string productid = RubyShopController.GetProductIDByTabID(current_tab_rubyID);
        if (string.IsNullOrEmpty(productid))
        {
            BoxManager.Instance.ShowMessageTip("本渠道不支持购买砖石");
        }
        else
        {

            Debug.LogWarning("start buy:" + rubyItem.Detial + " num:" + rubyItem.GetNum);
            SDKObjecty.buyProduct(current_tab_rubyID, RubyShopController.OnBuyRubyLocal);
            Invoke("HideNetWork", 2);
        }
    }

    void HideNetWork()
    {
        SceneManager.Instance.NetWorkBox.SetActive(false);
    }

    public override void Close()
    {
        base.Close();
//        PageManager.Instance.OpenPage("ShopController", "shopType=" + (int)ShopType.Zhuanshi);
    }

}
