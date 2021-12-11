using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TipWord
{
    public string word;
    public Vector3 pos;

    public TipWord(string _word, Vector3 _pos)
    {
        word = _word;
        pos = _pos;
    }
}

public class TipGuide : MonoBehaviour {
    public Queue<TipWord> tipWords;
	public UILabel tipLabel;

    public delegate void LastWordTipOnClick();

    public LastWordTipOnClick lastWordTipOnClick;

    public void ShowWords(Queue<TipWord> words)
    {
        tipWords = words;
        ShowNextWord();
    }

    public void OnClick()
    {
        if (!ShowNextWord())
        {
            #region guild
            if (LocalDataBase.equipGuild)
            {
                LocalDataBase.equipWaitInput++;
            }
            #endregion
            GuideManager.Instance.HideGuide();

            if (lastWordTipOnClick != null)
            {
                lastWordTipOnClick();
                lastWordTipOnClick = null;
            }
        }
    }

    public bool ShowNextWord()
    {
        if (tipWords.Count > 0)
        {
            TipWord currentTip = tipWords.Dequeue();
            this.transform.localPosition = currentTip.pos;
            tipLabel.text = currentTip.word;
            return true;
        }
        else
        {
            return false;
        }
    }
}
