using UnityEngine;
using System.Collections;

public class UIEquipFlyItem: UIFlyItem  {


    public UIEquipFlyItem(Vector3 objLocalPos, Vector3 targetIconWorldPos, string spriteName, string prefabName) : base(objLocalPos, targetIconWorldPos, spriteName, prefabName) { }


    public override void PlayCompleted0()
    {
        EleUIController.Instance.UpdateEquipNumUI();
    }
}
