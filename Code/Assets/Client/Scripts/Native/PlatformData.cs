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
// * Filename:PlatformData.cs
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

public enum PlatformId{
	Editor = 0,
	Appstore,
	GooglePlay,
	AndroidNormal,
}

public class PlatformData
{
	public PlatformId platformId = PlatformId.AndroidNormal;
	public static PlatformData[] platformDatas = new PlatformData[] {
		new PlatformData () {
			platformId = PlatformId.Editor,
		},
		new PlatformData () {
			platformId = PlatformId.Appstore,
		},
		new PlatformData () {
			platformId = PlatformId.GooglePlay,
		},
		new PlatformData () {
			platformId = PlatformId.AndroidNormal,
		}
	};
}

