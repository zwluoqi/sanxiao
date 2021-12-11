using UnityEngine;
using System.Collections;
using GCGame.Table;

public class StartGiftPage : Page {

    public void GetBtn()
    {
        //第一次进入游戏,产生用户道具信息;
        LocalDataBase.Instance().SetDataNum(DataType.power, LocalDataBase.maxPower+3);
        this.Close();
		ADTouTiao.Instance.RequestRewardVideo(OnPlayDone);
    }

	void OnPlayDone ()
	{
		//TODO
        BoxManager.Instance.ShowPopupMessage(string.Format(LanguageManger.GetMe().GetWords("L_S008")));
        LocalDataBase.Instance().AddDataNum(DataType.power, 3);
        LocalDataBase.Instance().SetDataNum(DataType.zhuanshi, 200);
        Hashtable hashTable = TableManager.GetEquip();
        foreach (DictionaryEntry dic in hashTable)
        {
            Tab_Equip tabEquip = (Tab_Equip)dic.Value;
            if (tabEquip.EnumID == (int)EquipEnumID.Hammer)
            {
                LocalDataBase.SetEquipNum((EquipEnumID)tabEquip.EnumID, 2);
            }
            if (tabEquip.EnumID == (int)EquipEnumID.ResetItem)
            {
                LocalDataBase.SetEquipNum((EquipEnumID)tabEquip.EnumID, 1);
            }
            if (tabEquip.EnumID == (int)EquipEnumID.Exchange)
            {
                LocalDataBase.SetEquipNum((EquipEnumID)tabEquip.EnumID, 2);
            }
            if (tabEquip.EnumID == (int)EquipEnumID.BomEffect)
            {
                LocalDataBase.SetEquipNum((EquipEnumID)tabEquip.EnumID, 1);
            }
            if (tabEquip.EnumID == (int)EquipEnumID.RowColEliminate)
            {
                LocalDataBase.SetEquipNum((EquipEnumID)tabEquip.EnumID, 1);
            }
        }
	}

    public void CancleBtn()
    {
        LocalDataBase.Instance().SetDataNum(DataType.power, LocalDataBase.maxPower);
        this.Close();
    }


    protected override void DoClose()
    {
        MapController m_mapController = SceneManager.Instance.UI.GetComponent<UIController>().map;
        m_mapController.FirstGuide();
    }
}
