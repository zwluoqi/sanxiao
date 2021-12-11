using UnityEngine;
using System.Collections;

public class FingerController : MonoBehaviour {
		
	public static UISquareView GetNearDirSquare(UISquareView staySquare, Vector3 direction){
		if(direction.magnitude == 0)
			return null;
		//Find neighbor item
		int moveToRow = staySquare.m_row + (int)direction.y;
		int moveToCol = staySquare.m_col + (int)direction.x;
		//Cannot find out of map
		if(moveToRow < 0 || moveToRow >= Map.maxRow)
			return null;
		if(moveToCol < 0 || moveToCol >= Map.maxCol)
			return null;
		UISquareView neighborSquare = Map.Instance.GetSquare(moveToRow,moveToCol);
		return neighborSquare;
	}
	
    public static SwipeDirection JudgeDragDir(float vx, float vy)
    {
        if (vx >= 0 && (Mathf.Abs(vx) > Mathf.Abs(vy))) // Ð¡ÓÚ30¶È  
        {
            return SwipeDirection.Right;
        }
        else if (vx <= 0 && (Mathf.Abs(vx) > Mathf.Abs(vy)))
        {
           
            return SwipeDirection.Left;

        }
        else if (vy >= 0 && (Mathf.Abs(vx) > Mathf.Abs(vy)))
        {
          
            return SwipeDirection.Up;

        }
        else if (vy <= 0 && (Mathf.Abs(vx) > Mathf.Abs(vy)))
        {
          
            return SwipeDirection.Down;
        }
        else if (vx>=0 && vy > 0 &&  vx<vy)
        {

            return SwipeDirection.Up;
        }
        else if (vx <= 0 && vy < 0 && (Mathf.Abs(vx) < Mathf.Abs(vy)))
        {

            return SwipeDirection.Down;
        }
        else if (vx <= 0 && vy > 0 && (Mathf.Abs(vx) < Mathf.Abs(vy)))
        {
            return SwipeDirection.Up;
        }
        else if (vx >= 0 && vy < 0 && (Mathf.Abs(vx) < Mathf.Abs(vy)))
        {
            return SwipeDirection.Down;
        }
        return  SwipeDirection.None;
    }

   public static  Vector3 GetMoveDirectionByDir(SwipeDirection dir)
    {
        Vector3 moveDirection = Vector3.zero;
        switch (dir)
        {
            case SwipeDirection.Up:
                {
                    moveDirection.y = 1;
                    moveDirection.x = 0;
                }
                break;

            case SwipeDirection.Down:
                {
                    moveDirection.y = -1;
                    moveDirection.x = 0;
                }
                break;

            case SwipeDirection.Left:
                {
                    moveDirection.y = 0;
                    moveDirection.x = -1;
                }
                break;
            case SwipeDirection.Right:
                {
                    moveDirection.y = 0;
                    moveDirection.x = 1;
                }
                break;
            default:
                {
                    moveDirection = Vector3.zero;
                }
                break;
        }
        return moveDirection;

    }	
}
