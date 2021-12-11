using UnityEngine;
using System.Collections;

public class GiftModel  {
	public int giftType = 0;//1是道具礼包,2是负面礼包;
	public int giftID;
	public int giftNum;
	
	public GiftModel(int type,int id,int num){
		giftType = type;
		giftID = id;
		giftNum = num;
	}
}
