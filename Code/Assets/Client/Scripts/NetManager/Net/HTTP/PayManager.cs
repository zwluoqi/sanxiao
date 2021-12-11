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
// * Filename:PayWebRequestUtil.cs
// * Created:2018/4/26
// * Author:  lucy.yijian
// * Purpose:
// * ==============================================================================
// */
//
using System;
using NetWork.Layer;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;
using System.Text;

public class PayManager
{


	public void Send(string url,byte[] bytes){
		PayWebRequestUtil webUtil = new PayWebRequestUtil ();
		webUtil.Init (url, this);
		webUtil.Send (bytes);
	}

	public void SessionCompleted (bool b, byte[] data)
	{
		//TODO
		string ret ="";
		if (data != null) {
			ret = System.Text.Encoding.UTF8.GetString (data);
		}
		UnityEngine.Debug.LogError ("SessionCompleted:" + b + " ret:" + ret);
	}


	public void Tick(){
		//TODO
	}


}

