using UnityEngine;
using System.Collections;
using GCGame.Table;

public class UISignRewardView : MonoBehaviour {

    public UISprite sprite;
    public UILabel num;
    public GameObject lightState;
    public GameObject hasGetState;

    public SignReward m_reward { get; set; }

    public void Init(SignReward reward)
    {
        m_reward = reward;
        if (reward.rewards[0].classID == (int)ClassID.Player)
        {
            if (reward.rewards[0].objID == (int)DataType.zhuanshi)
            {
                sprite.spriteName = "xiaozuanshitubiao";
            }
            else if (reward.rewards[0].objID == (int)DataType.jinbi)
            {
                sprite.spriteName = "jinbi";
            }
            else if (reward.rewards[0].objID == (int)DataType.power)
            {
                sprite.spriteName = "tili";
            }
            sprite.MakePixelPerfect();
        }
        else if (reward.rewards[0].classID == (int)ClassID.Equip)
        {
            sprite.spriteName = TableManager.GetEquipByID(reward.rewards[0].objID).SpriteName;
        }

        num.text = "X" + reward.rewards[0].num.ToString();

        if (reward.state == SignState.NotReward)
        {
            lightState.SetActive(false);
            hasGetState.SetActive(false);
        }
        else if(reward.state == SignState.HasReward)
        {
            lightState.SetActive(false);
            hasGetState.SetActive(true);
        }
        else if(reward.state == SignState.TipReward)
        {
            lightState.SetActive(true);
            hasGetState.SetActive(false);
        }

        UIEventListener.Get(gameObject).onClick = OnGetReward;
    }


    private void OnGetReward(GameObject go)
    {
        if (!SignReward.CanGetRewardToday())
        {
            BoxManager.Instance.ShowPopupMessage(LanguageManger.GetMe().GetWords("Sign_001"));
        }
        else if (m_reward.state == SignState.HasReward)
        {
            BoxManager.Instance.ShowPopupMessage(LanguageManger.GetMe().GetWords("Sign_001"));
        }
        else if (m_reward.state == SignState.NotReward)
        {
            BoxManager.Instance.ShowPopupMessage(LanguageManger.GetMe().GetWords("Sign_004"));
        }
        else
        {
            if (m_reward.rewards[0].classID == (int)ClassID.Player)
            {
                LocalDataBase.Instance().AddDataNum((DataType)m_reward.rewards[0].objID, m_reward.rewards[0].num);

            }
            else if (m_reward.rewards[0].classID == (int)ClassID.Equip)
            {
                Tab_Equip equip = TableManager.GetEquipByID(m_reward.rewards[0].objID);
                LocalDataBase.Instance().AddEquipNum((EquipEnumID)equip.EnumID, m_reward.rewards[0].num);
            }
            BoxManager.Instance.ShowPopupMessage(LanguageManger.GetMe().GetWords("Sign_002"));
            m_reward.GetSignRewardToday();
            Init(m_reward);
        }
    }
}
