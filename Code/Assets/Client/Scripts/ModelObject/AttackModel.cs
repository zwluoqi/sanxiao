using UnityEngine;
using System.Collections;

public class AttackModel  {

    public UISquareView sourceAttack;
    public UISquareView targetAttack;
    public int dmg = 1;

    public AttackModel(UISquareView source, UISquareView target)
    {
        this.sourceAttack = source;
        this.targetAttack = target;
    }

    public void Cast0(){
        sourceAttack.AttackOtherSquare(targetAttack);
        EventDelayManger.GetInstance().CreateEvent(ExportAttackDmg, EliminateLogic.Instance.moveinDeltaTime);
    }

    public void ExportAttackDmg()
    {
        sourceAttack.RemoveBlock(true);
        targetAttack.RemoveBlock();
        EleUIController.Instance.ShowOneScore(targetAttack.transform.localPosition, "[ff0000]-1");
    }
}
