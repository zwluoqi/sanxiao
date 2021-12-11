// /*
//                #########
//               ############
//               #############
//              ##  ###########
//             ###  ###### #####
//             ### #######   ####
//            ###  ########## ####
//           ####  ########### ####
//          ####   ###########  #####
//         #####   ### ########   #####
//        #####   ###   ########   ######
//       ######   ###  ###########   ######
//      ######   #### ##############  ######
//     #######  #####################  ######
//     #######  ######################  ######
//    #######  ###### #################  ######
//    #######  ###### ###### #########   ######
//    #######    ##  ######   ######     ######
//    #######        ######    #####     #####
//     ######        #####     #####     ####
//      #####        ####      #####     ###
//       #####       ###        ###      #
//         ###       ###        ###
//          ##       ###        ###
// __________#_______####_______####______________
//
//                 我们的未来没有BUG
// * ==============================================================================
// * Filename:NativeCallInterface.cs
// * Created:2019/3/5
// * Author:  zhouwei
// * Alert:
// * 代码千万行
// * 注释第一行
// * 命名不规范
// * 同事两行泪
// * Purpose:
// * ==============================================================================
// */
//
using System;
using UnityEngine;
using System.Collections.Generic;

namespace XZXD
{
	public interface NativeCallInterface
	{

		void loadDeviceInfo ();

		void initSdk (String gateIp);

		void sdkLogin (string loginData);

		void RegisterPush ();

		void sdkPay (Dictionary<string, string> payInfo, string pluginId);

		void logout ();

		void accountSwitch ();

		void quit ();

		void requestProducts ();

		void showToolBar ();

		void hideToolBar ();

		void submitLoginGameRole (string dataType);

		void sendDataToTalkingData (string methodName, string[] data);

		void sendDataToTalkingDataCpa (string methodName, string[] data);

		void updateGame (string uri);

		string getChannelId ();
	}
}

