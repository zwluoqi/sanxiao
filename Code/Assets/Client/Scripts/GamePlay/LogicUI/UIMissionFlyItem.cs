using UnityEngine;
using System.Collections;

public class UIMissionFlyItem: UIFlyItem  {

    public UIMissionFlyItem(Vector3 objLocalPos, Vector3 targetIconWorldPos, string spriteName, string prefabName) : base(objLocalPos, targetIconWorldPos, spriteName, prefabName) { }

    public override void PlayCompleted0()
    {
        SoundEffect.Instance.PlaySound(SoundEffect.missionEffect);
        EleUIController.Instance.UpdateMissionUI();
    }
}
