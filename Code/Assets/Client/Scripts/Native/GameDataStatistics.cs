using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace XZXD
{
	public class GameDataStatistics
	{
		public static string EVENTID_CLICK_BUTTON = "click_button";
		public static string EVENTID_PUSH_UI = "push_ui";
		public static string EVENTID_OPEN_DUNGEON = "open_dungeon";
		public static string EVENTID_GAIN_HERO = "gain_hero";
		public static string EVENTID_CREATE_BUILDING = "create_building";


	

		public static void cpa_firstTimeExperience(string action){

			NativeCaller.sendDataToTalkingData("firstTimeExperience", new string[]{action});

		}

		public static void cpa_retention(string action){
			
			NativeCaller.sendDataToTalkingData("retention", new string[]{action});
			
		}
			

		public static void setAccountId(string id)
		{
			NativeCaller.sendDataToTalkingData("setAccountId", new string[]{id});
		}
		
		public static void setAccountName(string name)
		{
			NativeCaller.sendDataToTalkingData("setAccountName", new string[]{name});
		}
		
		public static void setGameServer(string server)
		{
			NativeCaller.sendDataToTalkingData("setGameServer", new string[]{server});
		}
		
		public static void setLevel(int level)
		{
//			NativeCaller.sendDataToTalkingData("setLevel", new string[]{level.ToString()});
		}
		
		public static void onBegin(string missionId)
		{
//			NativeCaller.sendDataToTalkingData("onBegin", new string[]{missionId});
		}
		
		public static void onCompleted(string missionId)
		{
//			NativeCaller.sendDataToTalkingData("onCompleted", new string[]{missionId});
		}
		
		public static void onFailed(string missionId, string cause)
		{
//			NativeCaller.sendDataToTalkingData("onFailed", new string[]{missionId, cause});
		}
		
		public static void onUse(string item, int itemNumber)
		{
//			NativeCaller.sendDataToTalkingData("onUse", new string[]{item, itemNumber.ToString()});
		}

		public static void onChargeRequest(string orderId, string itemId, double moneyAmount, string moneyType, double virtualCurrencyAmount, string paymentType)
		{
			NativeCaller.sendDataToTalkingData("onChargeRequest", new string[]{orderId, itemId, moneyAmount.ToString(), moneyType.ToString(), virtualCurrencyAmount.ToString(), paymentType});
		}

		public static void onChargeSuccess(string orderId)
		{
			NativeCaller.sendDataToTalkingData("onChargeSuccess", new string[]{orderId});
		}
		public static void openCafeHome()
		{
			NativeCaller.sendDataToTalkingData("openCafeHome", new string[]{});
		}

		public static void onEvent(string eventId, Dictionary<string, object> eventData)
		{

		}
	}
}

