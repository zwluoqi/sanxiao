using UnityEngine;
using System.Collections.Generic;
using System.Collections;

using GCGame.Table;

public class ZhuanPanController : Page {
    //public Transform rewardParent;
    public List<Transform> zhuanpanItemPoss = new List<Transform>();
    public UIZhuanpanItemView zhuanpanItemPrefab;
    public TweenRotation hitrow;
    public UILabel price;
    public UILabel myRuby;

    private float startTime;
    private bool gaming;
    private int getIndex;

    //private string free = "mianfeizhuan";
    //private string cost = "huafei10";

    private const int costRuby = 10;
    public float duration = 4;
    public AnimationCurve speedCurve;

    public float defaultDuration = 0.01f;
    private float lightDuration = 0.01f;
    private float startLightTime = 0;
    public GameObject normalLight;
    public GameObject highLight;
    public ZhuanpanAnimation resultAnimation;
    public GameObject returnBtn;
    public GameObject startBtn;


    private List<UIZhuanpanItemView> zhuanpanItems = new List<UIZhuanpanItemView>();

    void Awake()
    {
        //Transform[] rewardPoss = rewardParent.GetComponentsInChildren<Transform>();
        //zhuanpanItemPoss.AddRange(rewardPoss);
        //zhuanpanItemPoss.Remove(rewardParent);
        for (int i = 0; i < 8;i++ )
        {
            Tab_Zhuanpan zhuanpan = TableManager.GetZhuanpanByID(i + 1);
            UIZhuanpanItemView zhuanpanItem = GameObject.Instantiate(zhuanpanItemPrefab) as UIZhuanpanItemView;
            zhuanpanItem.transform.parent = zhuanpanItemPoss[i];
            zhuanpanItem.transform.localPosition = Vector3.zero;
            zhuanpanItem.transform.localScale = Vector3.one;
            zhuanpanItem.Init(zhuanpan);
            zhuanpanItems.Add(zhuanpanItem);
        }
    }

    protected override void DoOpen()
    {
        FreshUI();
        hitrow.enabled = false;
        startBtn.GetComponent<Animation>().Play("ZhuanpanTween");
    }

    public void OnStartGame()
    {
        if (gaming)
        {
            return;
        }
        int freeTimes = LocalDataBase.Instance().GetFreeChouTimes();
        int currentRuby = LocalDataBase.Instance().GetDataNum(DataType.zhuanshi);
        bool freeB =  freeTimes > 0;
        if (freeB)
        {
            LocalDataBase.Instance().SetFreeChouTimes(freeTimes - 1);
        }
        else
        {
            if (currentRuby >= costRuby)
            {
                LocalDataBase.Instance().DecreaseDataNum(DataType.zhuanshi, costRuby);
            }
            else
            {
                BoxManager.Instance.ShowMessage(LanguageManger.GetMe().GetWords("L_1004"));
//                UIEventListener.Get(BoxManager.Instance.buttonOk).onClick += delegate(GameObject go)
//                {
//                    PageManager.Instance.OpenPage("ShopController", "shopType=" + (int)ShopType.Zhuanshi);
//                };
                return;
            }
        }
        getIndex = getWinedZhuanpanID();
        if (getIndex >= 0 && getIndex < 8)
        {
            startTime = 0;
            startLightTime = 0;
            gaming = true;
            hitrow.duration = 1;
            hitrow.enabled = true;
            returnBtn.SetActive(false);
            startBtn.GetComponent<Animation>().Play("ZhuanpanIdle");
            FreshUI();
            //Umeng.GA.Buy("zhuanpan", 1, 10);
        }
        else
        {
            BoxManager.Instance.ShowPopupMessage(LanguageManger.GetMe().GetWords("L_1051"));
        }
    }

    private int getWinedZhuanpanID()
    {
        Hashtable zhuanpans = TableManager.GetZhuanpan();
        int sum = 0;
        foreach (DictionaryEntry dic in zhuanpans)
        {
            sum += ((Tab_Zhuanpan)(dic.Value)).Rate;
        }
        int currentWined = 0;
        int currentRate = Random.Range(0, sum);
        int rangeLeft = 0;
        foreach (DictionaryEntry dic in zhuanpans)
        {
            Tab_Zhuanpan zhuanpan = ((Tab_Zhuanpan)(dic.Value));
            if (currentRate >= rangeLeft && currentRate < rangeLeft+zhuanpan.Rate)
            {
                break;
            }
            currentWined++;
            rangeLeft += zhuanpan.Rate;
        }
        return currentWined;
    }

    void Update()
    {
        if (gaming)
        {
            startTime += Time.deltaTime;
            float t =  Mathf.Min(startTime / duration,1);
            float speed = speedCurve.Evaluate(t) * 360;
            hitrow.duration = 360 / speed;
            if (startTime > duration)
            {
                int thisdeg = (((int)(hitrow.transform.localEulerAngles.z)+360)%360);
                int tmpIndex = thisdeg / 45;
                int remaineddeg = thisdeg % 45;
                int regionIndex = -1;
                if (remaineddeg <= 15)
                {
                    regionIndex = tmpIndex;
                }
                else if (remaineddeg >= 30)
                {
                    regionIndex = tmpIndex + 1;
                }

                if (regionIndex == getIndex)
                {
                    startTime = 0;
                    gaming = false;
                    hitrow.enabled = false;
                    returnBtn.SetActive(true);
                    ShowResult();
                }
            }

            lightDuration = hitrow.duration * defaultDuration;
            startLightTime += Time.deltaTime;
            if (startLightTime > lightDuration)
            {
                startLightTime = 0;
                normalLight.SetActive(!normalLight.activeSelf);
                highLight.SetActive(!highLight.activeSelf);
            }
        }
    }


    public void OnClickClose()
    {
        if (gaming)
        {
            return;
        }
        this.Close();
    }

    private void ShowResult()
    {
        SoundEffect.Instance.PlaySound(SoundEffect.tiliHip);
        startBtn.GetComponent<Animation>().Play("ZhuanpanTween");

        Tab_Zhuanpan result = TableManager.GetZhuanpanByID(getIndex + 1);
        if (result.Classid == (int)ClassID.Player)
        {
            LocalDataBase.Instance().AddDataNum((DataType)result.Objid, result.Num);
            resultAnimation.PlayResult(zhuanpanItemPoss[getIndex], result, this);

            if (result.Objid == (int)(DataType.zhuanshi))
            {
                //Umeng.GA.Bonus(result.Num, Umeng.GA.BonusSource.Source2);
            }
            //BoxManager.Instance.ShowMessageTip(string.Format( LanguageManger.GetMe().GetWords("L_zhuanpan001"),result.Num));
        }
        else if (result.Classid == (int)ClassID.Equip)
        {
            Tab_Equip equip = TableManager.GetEquipByID(result.Objid);
            if (equip != null)
            {
                LocalDataBase.Instance().AddEquipNum((EquipEnumID)equip.EnumID, result.Num);
                resultAnimation.PlayResult(zhuanpanItemPoss[getIndex], result, this);
                //BoxManager.Instance.ShowMessageTip(string.Format(LanguageManger.GetMe().GetWords("L_zhuanpan002"), equip.Detial,result.Num));
            }
        }
        FreshUI();
    }

    void FreshUI()
    {
        myRuby.text = LocalDataBase.Instance().GetDataNum(DataType.zhuanshi).ToString();
        bool freeB = LocalDataBase.Instance().GetFreeChouTimes() > 0;
        if (freeB)
        {
            price.text = LanguageManger.GetMe().GetWords("ZZ_001");
        }
        else
        {
            price.text = string.Format(LanguageManger.GetMe().GetWords("ZZ_002"), costRuby);
        }
        price.MakePixelPerfect();
    }
}
