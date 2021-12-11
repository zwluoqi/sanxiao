using UnityEngine;
using System.Collections;
using GCGame.Table;

public class CopyText : MonoBehaviour
{

	public UISprite HeadLeft;
	public UISprite HeadRight;
	public GameObject TextDi;
	public GameObject TextLabel;
	
	public void Show(Tab_Copystory story,bool isAlpha){
        string text = LanguageManger.GetMe().GetWords( story.StoryContent);
        TextLabel.GetComponent<UILabel>().text = text;
        if (story.LeftHeroIcon != "None")
        {
            string leftName = story.LeftHeroIcon;
            
            if (!string.IsNullOrEmpty(leftName))
            {
                HeadLeft.gameObject.SetActive(true);
                HeadLeft.spriteName = leftName;
                TextDi.transform.localRotation = new Quaternion(0, 0, 0, 0);

            }
            else
            {
                HeadLeft.gameObject.SetActive(false);
            }
        }
        else
        {
            HeadLeft.gameObject.SetActive(false);
        }

        if (story.RightHeroIcon != "None")
        {
            string rightName = story.RightHeroIcon;

            if (!string.IsNullOrEmpty(rightName))
            {
                HeadRight.gameObject.SetActive(true);
                HeadRight.spriteName = rightName;
                TextDi.transform.localRotation = new Quaternion(0, 180, 0, 0);

            }
            else
            {
                HeadRight.gameObject.SetActive(false);
            }
        }
        else
        {
            HeadRight.gameObject.SetActive(false);
        }
        if (isAlpha)
        {
            gameObject.GetComponent<TweenAlpha>().ResetToBeginning();
            gameObject.GetComponent<TweenAlpha>().PlayForward();
        }

	}
}

