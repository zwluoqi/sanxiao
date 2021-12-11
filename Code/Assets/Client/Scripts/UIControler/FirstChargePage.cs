using UnityEngine;
using System.Collections;
using GCGame.Table;


public class FirstChargePage : Page {


    public void GetBtn()
    {
		RubyShopController.current_tab_rubyID = 0;
	    Tab_Rubyshop rubyItem = TableManager.GetRubyshopByID(0);
	    Debug.LogWarning("start buy:" + rubyItem.Detial + " num:" + rubyItem.GetNum);
	    SDKObjecty.buyProduct(0, RubyShopController.OnBuyRubyLocal);
    }

    public void CancleBtn()
    {
        this.Close();
    }

}
