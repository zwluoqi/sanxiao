using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIFlyManager  {

    public float CurrentTime = 0;
    public Queue<UIFlyItem> FlyItems = new Queue<UIFlyItem>();

    public void Update(float deltaTime)
    {
        if (FlyItems.Count > 0)
        {
            CurrentTime += deltaTime;
            if (CurrentTime > EliminateLogic.Instance.FlyDeltaTime)
            {
                CurrentTime -= EliminateLogic.Instance.FlyDeltaTime;
                UIFlyItem currentFlyItem = FlyItems.Dequeue();
                currentFlyItem.Play();
            }

        }
        else
        {
            CurrentTime = 0;
        }
    }

}
