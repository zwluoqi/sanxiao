using System;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;
using ProtoBuf.Meta;
using Com.Communication;
namespace Com.Communication{
   public class PBConvert{
       public static Object toObject(Int32 opCode, byte[] objectBytes) {
              return toObject(opCode, objectBytes, 0, objectBytes.Length);
       }
       public static Object toObject(Int32 opCode, byte[] objectBytes,int start,int count) {
            Object result = null;
            using (MemoryStream m = new MemoryStream(objectBytes,start,count)){
                switch (opCode) { 
#region 错误信息 10000
                    case OpDefine.ErrorMessage:
                        result = ProtoBuf.Serializer.Deserialize<ErrorMessage>(m);
                        break;
#endregion
#region 登陆信息 20000
                    case OpDefine.CSLoginTcp:
                        result = ProtoBuf.Serializer.Deserialize<CSLoginTcp>(m);
                        break;
                    case OpDefine.SCLoginTcp:
                        result = ProtoBuf.Serializer.Deserialize<SCLoginTcp>(m);
                        break;
                    case OpDefine.GSRegisterTcp:
                        result = ProtoBuf.Serializer.Deserialize<GSRegisterTcp>(m);
                        break;
                    case OpDefine.SGRegisterTcp:
                        result = ProtoBuf.Serializer.Deserialize<SGRegisterTcp>(m);
                        break;
                    case OpDefine.CSCheckVersion:
                        result = ProtoBuf.Serializer.Deserialize<CSCheckVersion>(m);
                        break;
                    case OpDefine.SCCheckVersion:
                        result = ProtoBuf.Serializer.Deserialize<SCCheckVersion>(m);
                        break;
                    case OpDefine.CSLogin:
                        result = ProtoBuf.Serializer.Deserialize<CSLogin>(m);
                        break;
                    case OpDefine.SCLogin:
                        result = ProtoBuf.Serializer.Deserialize<SCLogin>(m);
                        break;
                    case OpDefine.CSGenerateName:
                        result = ProtoBuf.Serializer.Deserialize<CSGenerateName>(m);
                        break;
                    case OpDefine.SCGenerateName:
                        result = ProtoBuf.Serializer.Deserialize<SCGenerateName>(m);
                        break;
                    case OpDefine.CSBeginGame:
                        result = ProtoBuf.Serializer.Deserialize<CSBeginGame>(m);
                        break;
                    case OpDefine.SCBeginGame:
                        result = ProtoBuf.Serializer.Deserialize<SCBeginGame>(m);
                        break;
                    case OpDefine.CSSyncInstData:
                        result = ProtoBuf.Serializer.Deserialize<CSSyncInstData>(m);
                        break;
                    case OpDefine.SCSyncInstData:
                        result = ProtoBuf.Serializer.Deserialize<SCSyncInstData>(m);
                        break;
                    case OpDefine.CSLastInteration:
                        result = ProtoBuf.Serializer.Deserialize<CSLastInteration>(m);
                        break;
                    case OpDefine.SCLastInteration:
                        result = ProtoBuf.Serializer.Deserialize<SCLastInteration>(m);
                        break;
                    case OpDefine.CSUpdateEveryDayData:
                        result = ProtoBuf.Serializer.Deserialize<CSUpdateEveryDayData>(m);
                        break;
                    case OpDefine.SCUpdateEveryDayData:
                        result = ProtoBuf.Serializer.Deserialize<SCUpdateEveryDayData>(m);
                        break;
                    case OpDefine.CSUpdateTickData:
                        result = ProtoBuf.Serializer.Deserialize<CSUpdateTickData>(m);
                        break;
                    case OpDefine.SCUpdateTickData:
                        result = ProtoBuf.Serializer.Deserialize<SCUpdateTickData>(m);
                        break;
                    case OpDefine.CSGetTopLevel:
                        result = ProtoBuf.Serializer.Deserialize<CSGetTopLevel>(m);
                        break;
                    case OpDefine.SCGetTopLevel:
                        result = ProtoBuf.Serializer.Deserialize<SCGetTopLevel>(m);
                        break;
                    case OpDefine.CSGetPayItemData:
                        result = ProtoBuf.Serializer.Deserialize<CSGetPayItemData>(m);
                        break;
                    case OpDefine.SCGetPayItemData:
                        result = ProtoBuf.Serializer.Deserialize<SCGetPayItemData>(m);
                        break;
                    case OpDefine.CSGetOtherPlayerData:
                        result = ProtoBuf.Serializer.Deserialize<CSGetOtherPlayerData>(m);
                        break;
                    case OpDefine.SCGetOtherPlayerData:
                        result = ProtoBuf.Serializer.Deserialize<SCGetOtherPlayerData>(m);
                        break;
                    case OpDefine.CSGetCodeMail:
                        result = ProtoBuf.Serializer.Deserialize<CSGetCodeMail>(m);
                        break;
                    case OpDefine.SCGetCodeMail:
                        result = ProtoBuf.Serializer.Deserialize<SCGetCodeMail>(m);
                        break;
                    case OpDefine.CSGuideUnitDone:
                        result = ProtoBuf.Serializer.Deserialize<CSGuideUnitDone>(m);
                        break;
                    case OpDefine.SCGuideUnitDone:
                        result = ProtoBuf.Serializer.Deserialize<SCGuideUnitDone>(m);
                        break;
                    case OpDefine.CSGuideStepChange:
                        result = ProtoBuf.Serializer.Deserialize<CSGuideStepChange>(m);
                        break;
                    case OpDefine.SCGuideStepChange:
                        result = ProtoBuf.Serializer.Deserialize<SCGuideStepChange>(m);
                        break;
                    case OpDefine.CSCheckPay:
                        result = ProtoBuf.Serializer.Deserialize<CSCheckPay>(m);
                        break;
                    case OpDefine.SCCheckPay:
                        result = ProtoBuf.Serializer.Deserialize<SCCheckPay>(m);
                        break;
                    case OpDefine.CSGetPvpRank:
                        result = ProtoBuf.Serializer.Deserialize<CSGetPvpRank>(m);
                        break;
                    case OpDefine.SCGetPvpRank:
                        result = ProtoBuf.Serializer.Deserialize<SCGetPvpRank>(m);
                        break;
                    case OpDefine.CSPvpStartBattle:
                        result = ProtoBuf.Serializer.Deserialize<CSPvpStartBattle>(m);
                        break;
                    case OpDefine.SCPvpStartBattle:
                        result = ProtoBuf.Serializer.Deserialize<SCPvpStartBattle>(m);
                        break;
                    case OpDefine.CSPvpEndBattle:
                        result = ProtoBuf.Serializer.Deserialize<CSPvpEndBattle>(m);
                        break;
                    case OpDefine.SCPvpEndBattle:
                        result = ProtoBuf.Serializer.Deserialize<SCPvpEndBattle>(m);
                        break;
                    case OpDefine.CSOneKeyEquipEquip:
                        result = ProtoBuf.Serializer.Deserialize<CSOneKeyEquipEquip>(m);
                        break;
                    case OpDefine.CSChatGet:
                        result = ProtoBuf.Serializer.Deserialize<CSChatGet>(m);
                        break;
                    case OpDefine.CSChatSend:
                        result = ProtoBuf.Serializer.Deserialize<CSChatSend>(m);
                        break;
                    case OpDefine.SCChatGet:
                        result = ProtoBuf.Serializer.Deserialize<SCChatGet>(m);
                        break;
                    case OpDefine.CSMenpaiBossGet:
                        result = ProtoBuf.Serializer.Deserialize<CSMenpaiBossGet>(m);
                        break;
                    case OpDefine.CSMenpaiBossRankStartBattle:
                        result = ProtoBuf.Serializer.Deserialize<CSMenpaiBossRankStartBattle>(m);
                        break;
                    case OpDefine.SCMenpaiBossGet:
                        result = ProtoBuf.Serializer.Deserialize<SCMenpaiBossGet>(m);
                        break;
                    case OpDefine.CSMenpaiBossRankGet:
                        result = ProtoBuf.Serializer.Deserialize<CSMenpaiBossRankGet>(m);
                        break;
                    case OpDefine.SCMenpaiBossRankGet:
                        result = ProtoBuf.Serializer.Deserialize<SCMenpaiBossRankGet>(m);
                        break;
                    case OpDefine.CSResetMenpaiBossCoolDown:
                        result = ProtoBuf.Serializer.Deserialize<CSResetMenpaiBossCoolDown>(m);
                        break;
                    case OpDefine.SCResetMenpaiBossCoolDown:
                        result = ProtoBuf.Serializer.Deserialize<SCResetMenpaiBossCoolDown>(m);
                        break;
                    case OpDefine.CSGetMenpaiDamageReward:
                        result = ProtoBuf.Serializer.Deserialize<CSGetMenpaiDamageReward>(m);
                        break;
                    case OpDefine.SCGetMenpaiDamageReward:
                        result = ProtoBuf.Serializer.Deserialize<SCGetMenpaiDamageReward>(m);
                        break;
                    case OpDefine.CSBuyMenpaiBossCount:
                        result = ProtoBuf.Serializer.Deserialize<CSBuyMenpaiBossCount>(m);
                        break;
                    case OpDefine.SCBuyMenpaiBossCount:
                        result = ProtoBuf.Serializer.Deserialize<SCBuyMenpaiBossCount>(m);
                        break;
                    case OpDefine.CSStartBattle:
                        result = ProtoBuf.Serializer.Deserialize<CSStartBattle>(m);
                        break;
                    case OpDefine.SCStartBattle:
                        result = ProtoBuf.Serializer.Deserialize<SCStartBattle>(m);
                        break;
                    case OpDefine.CSKaKa:
                        result = ProtoBuf.Serializer.Deserialize<CSKaKa>(m);
                        break;
                    case OpDefine.CSPayReward:
                        result = ProtoBuf.Serializer.Deserialize<CSPayReward>(m);
                        break;
                    case OpDefine.CSYaMieDie:
                        result = ProtoBuf.Serializer.Deserialize<CSYaMieDie>(m);
                        break;
                    case OpDefine.CSJubao:
                        result = ProtoBuf.Serializer.Deserialize<CSJubao>(m);
                        break;
                    case OpDefine.CSKuafuBossGet:
                        result = ProtoBuf.Serializer.Deserialize<CSKuafuBossGet>(m);
                        break;
                    case OpDefine.CSKuafuBossRankStartBattle:
                        result = ProtoBuf.Serializer.Deserialize<CSKuafuBossRankStartBattle>(m);
                        break;
                    case OpDefine.SCKuafuBossGet:
                        result = ProtoBuf.Serializer.Deserialize<SCKuafuBossGet>(m);
                        break;
                    case OpDefine.CSKuafuBossRankGet:
                        result = ProtoBuf.Serializer.Deserialize<CSKuafuBossRankGet>(m);
                        break;
                    case OpDefine.SCKuafuBossRankGet:
                        result = ProtoBuf.Serializer.Deserialize<SCKuafuBossRankGet>(m);
                        break;
                    case OpDefine.CSResetKuafuBossCoolDown:
                        result = ProtoBuf.Serializer.Deserialize<CSResetKuafuBossCoolDown>(m);
                        break;
                    case OpDefine.SCResetKuafuBossCoolDown:
                        result = ProtoBuf.Serializer.Deserialize<SCResetKuafuBossCoolDown>(m);
                        break;
                    case OpDefine.CSGetKuafuBossDamageReward:
                        result = ProtoBuf.Serializer.Deserialize<CSGetKuafuBossDamageReward>(m);
                        break;
                    case OpDefine.SCGetKuafuBossDamageReward:
                        result = ProtoBuf.Serializer.Deserialize<SCGetKuafuBossDamageReward>(m);
                        break;
                    case OpDefine.CSBuyKuafuBossCount:
                        result = ProtoBuf.Serializer.Deserialize<CSBuyKuafuBossCount>(m);
                        break;
                    case OpDefine.SCBuyKuafuBossCount:
                        result = ProtoBuf.Serializer.Deserialize<SCBuyKuafuBossCount>(m);
                        break;
                    case OpDefine.CSRandomTask:
                        result = ProtoBuf.Serializer.Deserialize<CSRandomTask>(m);
                        break;
                    case OpDefine.SCRandomTask:
                        result = ProtoBuf.Serializer.Deserialize<SCRandomTask>(m);
                        break;
                    case OpDefine.CSActivityBossStartBattle:
                        result = ProtoBuf.Serializer.Deserialize<CSActivityBossStartBattle>(m);
                        break;
                    case OpDefine.SCActivityBossGet:
                        result = ProtoBuf.Serializer.Deserialize<SCActivityBossGet>(m);
                        break;
                    case OpDefine.CSActivityBossGet:
                        result = ProtoBuf.Serializer.Deserialize<CSActivityBossGet>(m);
                        break;
                    case OpDefine.CSPvpNewStartBattle:
                        result = ProtoBuf.Serializer.Deserialize<CSPvpNewStartBattle>(m);
                        break;
                    case OpDefine.SCPvpNewStartBattle:
                        result = ProtoBuf.Serializer.Deserialize<SCPvpNewStartBattle>(m);
                        break;
                    case OpDefine.CSGetPvpNewRank:
                        result = ProtoBuf.Serializer.Deserialize<CSGetPvpNewRank>(m);
                        break;
                    case OpDefine.SCGetPvpNewRank:
                        result = ProtoBuf.Serializer.Deserialize<SCGetPvpNewRank>(m);
                        break;
                    case OpDefine.CSGetPlayerLiveRecord:
                        result = ProtoBuf.Serializer.Deserialize<CSGetPlayerLiveRecord>(m);
                        break;
                    case OpDefine.SCGetPlayerLiveRecord:
                        result = ProtoBuf.Serializer.Deserialize<SCGetPlayerLiveRecord>(m);
                        break;
                    case OpDefine.CSGetAllTeamInfo:
                        result = ProtoBuf.Serializer.Deserialize<CSGetAllTeamInfo>(m);
                        break;
                    case OpDefine.SCGetAllTeamInfo:
                        result = ProtoBuf.Serializer.Deserialize<SCGetAllTeamInfo>(m);
                        break;
                    case OpDefine.CSGetTeamInfo:
                        result = ProtoBuf.Serializer.Deserialize<CSGetTeamInfo>(m);
                        break;
                    case OpDefine.SCGetTeamInfo:
                        result = ProtoBuf.Serializer.Deserialize<SCGetTeamInfo>(m);
                        break;
                    case OpDefine.CSCreateTeam:
                        result = ProtoBuf.Serializer.Deserialize<CSCreateTeam>(m);
                        break;
                    case OpDefine.CSChangeTeam:
                        result = ProtoBuf.Serializer.Deserialize<CSChangeTeam>(m);
                        break;
                    case OpDefine.CSEnterTeam:
                        result = ProtoBuf.Serializer.Deserialize<CSEnterTeam>(m);
                        break;
                    case OpDefine.CSExitTeam:
                        result = ProtoBuf.Serializer.Deserialize<CSExitTeam>(m);
                        break;
                    case OpDefine.CSTeamReadyBattle:
                        result = ProtoBuf.Serializer.Deserialize<CSTeamReadyBattle>(m);
                        break;
                    case OpDefine.CSStartTeamBattle:
                        result = ProtoBuf.Serializer.Deserialize<CSStartTeamBattle>(m);
                        break;
                    case OpDefine.SCStartTeamBattle:
                        result = ProtoBuf.Serializer.Deserialize<SCStartTeamBattle>(m);
                        break;
                    case OpDefine.CSChangeTeamPos:
                        result = ProtoBuf.Serializer.Deserialize<CSChangeTeamPos>(m);
                        break;
                    case OpDefine.CSGetSearchTeamInfo:
                        result = ProtoBuf.Serializer.Deserialize<CSGetSearchTeamInfo>(m);
                        break;
                    case OpDefine.CSGetFriendInfo:
                        result = ProtoBuf.Serializer.Deserialize<CSGetFriendInfo>(m);
                        break;
                    case OpDefine.SCGetFriendInfo:
                        result = ProtoBuf.Serializer.Deserialize<SCGetFriendInfo>(m);
                        break;
                    case OpDefine.CSGetPushPlayerInfo:
                        result = ProtoBuf.Serializer.Deserialize<CSGetPushPlayerInfo>(m);
                        break;
                    case OpDefine.SCGetPushPlayerInfo:
                        result = ProtoBuf.Serializer.Deserialize<SCGetPushPlayerInfo>(m);
                        break;
                    case OpDefine.CSGetSearchFriendInfo:
                        result = ProtoBuf.Serializer.Deserialize<CSGetSearchFriendInfo>(m);
                        break;
                    case OpDefine.CSShenqingFriend:
                        result = ProtoBuf.Serializer.Deserialize<CSShenqingFriend>(m);
                        break;
                    case OpDefine.CSApplyFriend:
                        result = ProtoBuf.Serializer.Deserialize<CSApplyFriend>(m);
                        break;
                    case OpDefine.CSSendGiftFriend:
                        result = ProtoBuf.Serializer.Deserialize<CSSendGiftFriend>(m);
                        break;
                    case OpDefine.CSChangeFriendGift:
                        result = ProtoBuf.Serializer.Deserialize<CSChangeFriendGift>(m);
                        break;
                    case OpDefine.SCChangeFriendGift:
                        result = ProtoBuf.Serializer.Deserialize<SCChangeFriendGift>(m);
                        break;
                    case OpDefine.CSGetZuzhiInfo:
                        result = ProtoBuf.Serializer.Deserialize<CSGetZuzhiInfo>(m);
                        break;
                    case OpDefine.SCGetZuzhiInfo:
                        result = ProtoBuf.Serializer.Deserialize<SCGetZuzhiInfo>(m);
                        break;
                    case OpDefine.CSCreateZuzhi:
                        result = ProtoBuf.Serializer.Deserialize<CSCreateZuzhi>(m);
                        break;
                    case OpDefine.CSChangeZuzhi:
                        result = ProtoBuf.Serializer.Deserialize<CSChangeZuzhi>(m);
                        break;
                    case OpDefine.CSEnterZuzhi:
                        result = ProtoBuf.Serializer.Deserialize<CSEnterZuzhi>(m);
                        break;
                    case OpDefine.CSExitZuzhi:
                        result = ProtoBuf.Serializer.Deserialize<CSExitZuzhi>(m);
                        break;
                    case OpDefine.CSFinishAds:
                        result = ProtoBuf.Serializer.Deserialize<CSFinishAds>(m);
                        break;
                    case OpDefine.SCFinishAds:
                        result = ProtoBuf.Serializer.Deserialize<SCFinishAds>(m);
                        break;
#endregion
                    default:
                        break;
                }
            }
            return result;
        }
        public static byte[] toBytes(Int32 opCode,Object obj){
            byte[] result = null;
            using (MemoryStream m = new MemoryStream()){
                ProtoBuf.Serializer.Serialize(m, obj);
                m.Position = 0;
                int length = (int)m.Length;
                result = new byte[length];
                m.Read(result, 0, length);
            }
            return result;
        }
   }
}
