using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    DoubleToStringNum dts = new DoubleToStringNum();

    [Header("-미션 페이지 부모 그리드")]
    public Transform DailyMiss;
    public Transform SpecilMiss;
    public Transform AllMiss;

    public Transform[] dailyBtn;
    public Transform[] specialBtn;
    public Transform[] allBtn;

    /// <summary>
    /// 실행시 한번만 초기화. 게임 스타트?
    /// 1. 버튼 클릭 회색으로 만듦.
    /// 2. 해금 안된거 덮어줌
    /// </summary>
    public void SpecialMissonImgInit()
    {
        for (int i = 1; i < SpecilMiss.childCount; i++)
        {
            // 인덱스가 펀치 인덱스보다 크면?
            if (PlayerPrefsManager.GetInstance().weaponInfo[i].isUnlock)
            {
                // 초기 몽땅 덮는 커버 이미지 다 벗겨줌.
                specialBtn[i].parent.GetChild(4).gameObject.SetActive(false);
            }
            else // 아니면 봉인
            {
                // 초기 몽땅 덮는 커버 이미지 다 씌어줌.
                specialBtn[i].parent.GetChild(4).gameObject.SetActive(true);
                //버튼에 회색 씌워줌.
                specialBtn[i].GetChild(1).gameObject.SetActive(true);
            }

        }
    }

    /// <summary>
    /// 미션 탭 클립하면 초기화 해야한다.
    /// 2. 상단 탭 클릭해도 초기화.
    /// </summary>
    public void MissionInit()
    {
        DailyMissionUpdate();
        SpecMissionUpdate();
        AllMissionUpdate();
    }


    

    /// <summary>
    /// 미션 창 켤때 새로고침 해줌
    /// </summary>
    void DailyMissionUpdate()
    {
        // 레닷 꺼줌.
        RedDotManager.GetInstance().QuestDotOff0();

        // 횟수 부분 할당
        var day = PlayerPrefsManager.GetInstance().questInfo[0];
        var day3 = PlayerPrefsManager.GetInstance().questInfo3[0];

        if (day.daily_Abs >= 1)
        {
            dailyBtn[0].GetChild(1).gameObject.SetActive(false);
            DailyMiss.GetChild(0).GetChild(2).GetComponent<Text>().text = "횟수 : ( 1 / 1 )";
            // 보상 받았으면 닫아줌.
            if(day.daily_Abs >= 52525)
            {
                dailyBtn[0].GetChild(1).gameObject.SetActive(true);
            }
            else //아직 보상 안받음.
            {
                RedDotManager.GetInstance().QuestDotOn0();
            }

        } else DailyMiss.GetChild(0).GetChild(2).GetComponent<Text>().text = "횟수 : ( " + day.daily_Abs + " / 1 )";

        if (day.daily_Atk >= 5)
        {
            dailyBtn[1].GetChild(1).gameObject.SetActive(false);
            DailyMiss.GetChild(1).GetChild(2).GetComponent<Text>().text = "횟수 : ( 5 / 5 )";
            if (day.daily_Atk >= 52525)
            {
                dailyBtn[1].GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                RedDotManager.GetInstance().QuestDotOn0();
            }

        }
        else DailyMiss.GetChild(1).GetChild(2).GetComponent<Text>().text = "횟수 : ( " + day.daily_Atk + " / 5 )";

        if (day.daily_HP >= 5)
        {
            dailyBtn[2].GetChild(1).gameObject.SetActive(false);
            DailyMiss.GetChild(2).GetChild(2).GetComponent<Text>().text = "횟수 : ( 5 / 5 )";
            if (day.daily_HP >= 52525)
            {
                dailyBtn[2].GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                RedDotManager.GetInstance().QuestDotOn0();
            }

        }
        else DailyMiss.GetChild(2).GetChild(2).GetComponent<Text>().text = "횟수 : ( " + day.daily_HP + " / 5 )";


        if (day.daily_Punch >= 5)
        {
            dailyBtn[3].GetChild(1).gameObject.SetActive(false);
            DailyMiss.GetChild(3).GetChild(2).GetComponent<Text>().text = "횟수 : ( 5 / 5 )";
            if (day.daily_Punch >= 52525)
            {
                dailyBtn[3].GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                RedDotManager.GetInstance().QuestDotOn0();
            }

        }
        else DailyMiss.GetChild(3).GetChild(2).GetComponent<Text>().text = "횟수 : ( " + day.daily_Punch + " / 5 )";


        if (day.daily_MiniCombo >= 5)
        {
            dailyBtn[4].GetChild(1).gameObject.SetActive(false);
            DailyMiss.GetChild(4).GetChild(2).GetComponent<Text>().text = "횟수 : ( 5 / 5 )";
            if (day.daily_MiniCombo >= 52525)
            {
                dailyBtn[4].GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                RedDotManager.GetInstance().QuestDotOn0();
            }

        }
        else DailyMiss.GetChild(4).GetChild(2).GetComponent<Text>().text = "횟수 : ( " + day.daily_MiniCombo + " / 5 )";


        if (day.daily_ArtiGatcha >= 1)
        {
            dailyBtn[5].GetChild(1).gameObject.SetActive(false);
            DailyMiss.GetChild(5).GetChild(2).GetComponent<Text>().text = "횟수 : ( 1 / 1 )";
            if (day.daily_ArtiGatcha >= 52525)
            {
                dailyBtn[5].GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                RedDotManager.GetInstance().QuestDotOn0();
            }

        }
        else DailyMiss.GetChild(5).GetChild(2).GetComponent<Text>().text = "횟수 : ( " + day.daily_ArtiGatcha + " / 1 )";



        if (day3.daily_MiniGameCombo >= 30)
        {
            dailyBtn[6].GetChild(1).gameObject.SetActive(false);
            DailyMiss.GetChild(6).GetChild(2).GetComponent<Text>().text = "횟수 : ( 30 / 30 )";
            if (day3.daily_MiniGameCombo >= 52525)
            {
                dailyBtn[6].GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                RedDotManager.GetInstance().QuestDotOn0();
            }

        }
        else DailyMiss.GetChild(6).GetChild(2).GetComponent<Text>().text = "횟수 : ( " + day3.daily_MiniGameCombo + " / 30 )";


        UserWallet.GetInstance().ShowAllMoney();

        //PlayerPrefsManager.GetInstance().SavequestInfo();
        //PlayerPrefsManager.GetInstance().SavequestInfo2();
        //PlayerPrefsManager.GetInstance().SavequestInfo3();
        //PlayerPrefsManager.GetInstance().SavequestInfo4();
        //PlayerPrefsManager.GetInstance().SavequestInfo5();
        //PlayerPrefsManager.GetInstance().SavequestInfo6();

    }

    /// <summary>
    /// 1. 일반 미션 버튼에 달려있는거
    /// 0시에 초기화 되어야 한다.
    /// 달성시 보상과 함께 횟수 증가해야 한다.
    /// </summary>
    /// <param name="_index"></param>
    public void DailyMission(int _index)
    {
        // 클릭했는데 커버 씌워져있으면 빠꾸
        if (dailyBtn[_index].GetChild(1).gameObject.activeSelf) return;
        //보상 획득
        GetDiamond("10");
        PopUpObjectManager.GetInstance().ShowWarnnigProcess("보상으로 10 다이아를 받았습니다.");
        // 다이아 얻으면 커버 씌우기.
        dailyBtn[_index].GetChild(1).gameObject.SetActive(true);

        // 횟수 부분 할당
        var day = PlayerPrefsManager.GetInstance().questInfo[0];
        var day2 = PlayerPrefsManager.GetInstance().questInfo3[0];
        /// 이 상태를 유지 어케함?
        switch (_index)
        {
            case 0:
                day.daily_Abs = 52525;
                break;

            case 1:
                day.daily_Atk = 52525;
                break;

            case 2:
                day.daily_HP = 52525;
                break;

            case 3:
                day.daily_Punch = 52525;
                break;

            case 4:
                day.daily_MiniCombo = 52525;
                break;

            case 5:
                day.daily_ArtiGatcha = 52525;
                break;

            case 6:
                day2.daily_MiniGameCombo = 52525;
                break;

        }


    }


    [Header("- 누적 미션 MAX 커버")]
    public GameObject[] AllMission;
    bool isAllRed;
    /// <summary>
    /// 누적 미션 업데이트
    /// </summary>
    void AllMissionUpdate()
    {
        // 레드닷 초기화.
        isAllRed = false;
        //
        var all = PlayerPrefsManager.GetInstance().questInfo[0];
        var all2 = PlayerPrefsManager.GetInstance().questInfo2[0];
        var all3 = PlayerPrefsManager.GetInstance().questInfo3[0];
        var all4 = PlayerPrefsManager.GetInstance().questInfo4[0];
        //
        var all6 = PlayerPrefsManager.GetInstance().questInfo6[0];

        // 굵은 검은 글씨
        Text txtTop;
        // 아래 파란 글씨
        Text txt;
        // 임시저장 스트링스트림
        string tmpString ="";
        string tmpStringTail ="";


        long[] all_M = new long[]
        {
            all.All_Mattzip,
            all.All_Atk,
            all.All_HP,
            all.All_Punch,
            all.All_MiniGame,
            all.All_Gatcha,
            all.All_Abs,
            all2.All_Mugan,
            all3.All_MiniGame,
                        //
            all4.All_Per_Atk,
            all4.All_Per_HP,
            all4.All_Dia_Atk,
            all4.All_Dia_HP,
            //
            all6.All_PVPGame
        };

        long[] all_UP = new long[]
        {
            all.All_Mattzip_Cnt+1,
            all.All_Atk_Cnt+1,
            all.All_HP_Cnt+1,
            all.All_Punch_Cnt+1,
            all.All_MiniGame_Cnt+1,
            all.All_Gatcha_Cnt+1,
            all.All_Abs_Cnt+1,
            all2.All_Mugan_Cnt+1,
            all3.All_MiniGame_Cnt+1,
                                    //
            all4.All_Per_Atk_Cnt+1,
            all4.All_Per_HP_Cnt+1,
            all4.All_Dia_Atk_Cnt+1,
            all4.All_Dia_HP_Cnt+1,
            //
            all6.All_PVPGame_Cnt+1

        };

        /// 앞에 4개는 50회
        for (int i = 0; i < 4; i++)
        {
            if (i == 0) tmpString = "맷집 ";
            else if (i == 1) tmpString = "공격력 ";
            else if (i == 2) tmpString = "체력 ";
            else if (i == 3) tmpString = "장비 ";
            //
            txtTop = AllMiss.GetChild(i).GetChild(1).GetComponent<Text>();
            txt = AllMiss.GetChild(i).GetChild(2).GetComponent<Text>();
            // 달성해서 다이아 받은 지점에서 50 곱해서 맥스값
            txtTop.text = tmpString + (all_UP[i] * 50) + "회 강화";
            txt.text = "횟수 : ( " + all_M[i] + " / "+ (all_UP[i] * 50) +" )";
            // 달성했다면 커버 이미지 제거
            if (all_M[i] >= (all_UP[i] * 50))
            {
                /// 1,000회 제한.
                if (all_UP[i] >= 20)
                {
                    allBtn[i].GetChild(3).gameObject.SetActive(true);
                    txt.text = "횟수 : ( 1000 / 1000 )";
                    continue;
                }
                allBtn[i].GetChild(1).gameObject.SetActive(false);
                isAllRed = true;
            }
            else
            {
                allBtn[i].GetChild(1).gameObject.SetActive(true);
            }
        }

        /// 뒤에 4개는 10회
        for (int i = 4; i <9; i++)
        {
            if (i == 4)
            {
                tmpString = "무한 버티기 ";
                tmpStringTail = " 회 하기";
            }
            else if (i == 5)
            {
                tmpString = "유물 ";
                tmpStringTail = " 회 뽑기";

            }
            else if (i == 6)
            {
                tmpString = "광고 ";
                tmpStringTail = " 회 보기";
            }
            else if (i == 7)
            {
                tmpString = "무한의 탑 ";
                tmpStringTail = " 회 하기";
            }
            else if (i == 8)
            {
                tmpString = "미니 게임 ";
                tmpStringTail = " 회 하기";
            }
            txtTop = AllMiss.GetChild(i).GetChild(1).GetComponent<Text>();
            txt = AllMiss.GetChild(i).GetChild(2).GetComponent<Text>();
            // 달성해서 다이아 받은 지점에서 10 곱해서 맥스값
            txtTop.text = tmpString + (all_UP[i] * 10) + tmpStringTail;
            txt.text = "횟수 : ( " + all_M[i] + " / " + (all_UP[i] * 10) + " )";
            // 달성했다면 커버 이미지 제거
            if (all_M[i] >= (all_UP[i] * 10))
            {
                /// 1,000회 제한.
                if (all_UP[i] >= 100)
                {
                    allBtn[i].GetChild(3).gameObject.SetActive(true);
                    txt.text = "횟수 : ( 1000 / 1000 )";
                    continue;
                }

                allBtn[i].GetChild(1).gameObject.SetActive(false);
                isAllRed = true;
            }
            else
            {
                allBtn[i].GetChild(1).gameObject.SetActive(true);
            }

        }


        /// 맨뒤에 4개는 50회
        for (int i = 9; i < AllMiss.childCount; i++)
        {
            tmpStringTail = "회 강화";

            if (i == 9) tmpString = "공/퍼 ";
            else if (i == 10) tmpString = "체/퍼 ";
            else if (i == 11) tmpString = "공/특 ";
            else if (i == 12) tmpString = "체/특 ";
            else if (i == 13)
            {
                tmpString = "PVP ";
                tmpStringTail = "회 하기";
            }
            //
            txtTop = AllMiss.GetChild(i).GetChild(1).GetComponent<Text>();
            txt = AllMiss.GetChild(i).GetChild(2).GetComponent<Text>();
            // 달성해서 다이아 받은 지점에서 50 곱해서 맥스값
            txtTop.text = tmpString + (all_UP[i] * 50) + tmpStringTail; //"회 강화";
            txt.text = "횟수 : ( " + all_M[i] + " / " + (all_UP[i] * 50) + " )";
            // 달성했다면 커버 이미지 제거
            if (all_M[i] >= (all_UP[i] * 50))
            {
                /// 1,000회 제한.
                if (all_UP[i] >= 20)
                {
                    allBtn[i].GetChild(3).gameObject.SetActive(true);
                    txt.text = "횟수 : ( 1000 / 1000 )";
                    continue;
                }
                allBtn[i].GetChild(1).gameObject.SetActive(false);
                isAllRed = true;
            }
            else
            {
                allBtn[i].GetChild(1).gameObject.SetActive(true);
            }
        }






        if (isAllRed)
        {
            RedDotManager.GetInstance().QuestDotOn1();

        }
        else
        {
            RedDotManager.GetInstance().QuestDotOff1();
        }

    }

    /// <summary>
    /// 누적 미션 보상 버튼에 할당
    /// </summary>
    /// <param name="_index"></param>
    public void GetRewordAllMission(int _index)
    {
        // 클릭했는데 커버 씌워져있으면 빠꾸
        if (allBtn[_index].GetChild(1).gameObject.activeSelf) return;



        var all = PlayerPrefsManager.GetInstance().questInfo[0];
        var all2 = PlayerPrefsManager.GetInstance().questInfo2[0];
        var all3 = PlayerPrefsManager.GetInstance().questInfo3[0];
        var all4 = PlayerPrefsManager.GetInstance().questInfo4[0];

        long[] all_M = new long[]
{
            all.All_Mattzip,
            all.All_Atk,
            all.All_HP,
            all.All_Punch,
            all.All_MiniGame,
            all.All_Gatcha,
            all.All_Abs,
            all2.All_Mugan,
            all3.All_MiniGame,
            //
            all4.All_Per_Atk,
            all4.All_Per_HP,
            all4.All_Dia_Atk,
            all4.All_Dia_HP
};

        long[] all_UP = new long[]
        {
            all.All_Mattzip_Cnt+1,
            all.All_Atk_Cnt+1,
            all.All_HP_Cnt+1,
            all.All_Punch_Cnt+1,
            all.All_MiniGame_Cnt+1,
            all.All_Gatcha_Cnt+1,
            all.All_Abs_Cnt+1,
            all2.All_Mugan_Cnt+1,
            all3.All_MiniGame_Cnt+1,
                        //
            all4.All_Per_Atk_Cnt+1,
            all4.All_Per_HP_Cnt+1,
            all4.All_Dia_Atk_Cnt+1,
            all4.All_Dia_HP_Cnt+1

        };


        //보상 획득
        GetDiamond("20");
        PopUpObjectManager.GetInstance().ShowWarnnigProcess("보상으로 20 다이아를 받았습니다.");
        // 다이아 얻으면 커버 씌우기.
        allBtn[_index].GetChild(1).gameObject.SetActive(true);
        // 십의 자리 하나 올려주기
        switch (_index)
        {
            case 0: PlayerPrefsManager.GetInstance().questInfo[0].All_Mattzip_Cnt ++; break;
            case 1: PlayerPrefsManager.GetInstance().questInfo[0].All_Atk_Cnt++; break;
            case 2: PlayerPrefsManager.GetInstance().questInfo[0].All_HP_Cnt++; break;
            case 3: PlayerPrefsManager.GetInstance().questInfo[0].All_Punch_Cnt++; break;
            case 4: PlayerPrefsManager.GetInstance().questInfo[0].All_MiniGame_Cnt++; break;
            case 5: PlayerPrefsManager.GetInstance().questInfo[0].All_Gatcha_Cnt++; break;
            case 6: PlayerPrefsManager.GetInstance().questInfo[0].All_Abs_Cnt++; break;
            case 7: PlayerPrefsManager.GetInstance().questInfo2[0].All_Mugan_Cnt++; break;
            case 8: PlayerPrefsManager.GetInstance().questInfo3[0].All_MiniGame_Cnt++; break;
            //
            case 9: PlayerPrefsManager.GetInstance().questInfo4[0].All_Per_Atk_Cnt++; break;
            case 10: PlayerPrefsManager.GetInstance().questInfo4[0].All_Per_HP_Cnt++; break;
            case 11: PlayerPrefsManager.GetInstance().questInfo4[0].All_Dia_Atk_Cnt++; break;
            case 12: PlayerPrefsManager.GetInstance().questInfo4[0].All_Dia_HP_Cnt++; break;
        }
        // 보상 더 있나 새로고침
        AllMissionUpdate();
        //
    }

    bool isSpecRed;
    [Header("- 특별 미션 MAX 커버")]
    public GameObject[] SpecMission;
    /// <summary>
    /// 특별 미션 업데이트
    /// </summary>
    void SpecMissionUpdate()
    {
        // 커버 이미지 닫혀있나 초기화.
        isSpecRed = false;
        // 퀘스트 인포 긁기
        var spec = PlayerPrefsManager.GetInstance().questInfo[0];
        var spec2 = PlayerPrefsManager.GetInstance().questInfo2[0];
        var spec3 = PlayerPrefsManager.GetInstance().questInfo3[0];
        var spec4 = PlayerPrefsManager.GetInstance().questInfo4[0];
        //
        var spec5 = PlayerPrefsManager.GetInstance().questInfo5[0];
        //
        Text txt;
        Text txtTop;

        long[] Spec_M = new long[]
        {
            spec.Pun_01,
            spec.Pun_02,
            spec.Pun_03,
            spec.Pun_04,
            spec.Pun_05,
            spec.Pun_06,
            spec.Pun_07,
            spec.Pun_08,
            spec.Pun_09,
            spec.Pun_10,
            spec.Pun_11,
            spec.Pun_12,
            spec.Pun_13,
            spec.Pun_14,
            spec.Pun_15,
            spec.Pun_16,
            spec.Pun_17,
            spec.Pun_18,
            spec.Pun_19,
            spec.Pun_20,
            spec.Pun_21,
            spec.Pun_22,
            spec.Pun_23,
            spec.Pun_24,
            spec.Pun_25,
            spec.Pun_26,
            spec.Pun_27,
            spec.Pun_28,
            spec.Pun_29,
            spec.Pun_30,
            spec.Pun_31,
            spec.Pun_32,
            spec.Pun_33,
            spec.Pun_34,
            spec.Pun_35,
            spec.Pun_36,
            spec.Pun_37,
            spec.Pun_38,
            spec.Pun_39,
            spec.Pun_40,
            spec.Pun_41,
            spec.Pun_42,
            spec.Pun_43,
            spec.Pun_44,
            spec.Pun_45,
            spec.Pun_46,
            spec.Pun_47,
            spec.Pun_48,
            spec.Pun_49,
            spec.Pun_50,
            spec2.Pun_51,
            spec2.Pun_52,
            spec2.Pun_53,
            spec2.Pun_54,
            spec2.Pun_55,
            spec2.Pun_56,
            spec2.Pun_57,
            spec2.Pun_58,
            spec2.Pun_59,
            spec2.Pun_60,
            spec2.Pun_61,
            spec2.Pun_62,
            spec2.Pun_63,
            spec2.Pun_64,
            spec2.Pun_65,
            spec2.Pun_66,
            spec2.Pun_67,
            spec2.Pun_68,
            spec2.Pun_69,
            spec2.Pun_70,
            spec3.Pun_71,
            spec3.Pun_72,
            spec3.Pun_73,
            spec3.Pun_74,
            spec3.Pun_75,
            spec3.Pun_76,
            spec3.Pun_77,
            spec3.Pun_78,
            spec3.Pun_79,
            spec3.Pun_80,
            spec4.Pun_81,
            spec4.Pun_82,
            spec4.Pun_83,
            spec4.Pun_84,
            spec4.Pun_85,
            spec4.Pun_86,
            spec4.Pun_87,
            spec4.Pun_88,
            spec4.Pun_89,
            spec4.Pun_90,
            spec5.Pun_91,
            spec5.Pun_92,
            spec5.Pun_93,
            spec5.Pun_94,
            spec5.Pun_95,
            spec5.Pun_96,
            spec5.Pun_97,
            spec5.Pun_98,
            spec5.Pun_99,
            spec5.Pun_100

        };

        long[] Spec_UP = new long[]
        {
            spec.Pun_01_Cnt+1,
            spec.Pun_02_Cnt+1,
            spec.Pun_03_Cnt+1,
            spec.Pun_04_Cnt+1,
            spec.Pun_05_Cnt+1,
            spec.Pun_06_Cnt+1,
            spec.Pun_07_Cnt+1,
            spec.Pun_08_Cnt+1,
            spec.Pun_09_Cnt+1,
            spec.Pun_10_Cnt+1,
            spec.Pun_11_Cnt+1,
            spec.Pun_12_Cnt+1,
            spec.Pun_13_Cnt+1,
            spec.Pun_14_Cnt+1,
            spec.Pun_15_Cnt+1,
            spec.Pun_16_Cnt+1,
            spec.Pun_17_Cnt+1,
            spec.Pun_18_Cnt+1,
            spec.Pun_19_Cnt+1,
            spec.Pun_20_Cnt+1,
            spec.Pun_21_Cnt+1,
            spec.Pun_22_Cnt+1,
            spec.Pun_23_Cnt+1,
            spec.Pun_24_Cnt+1,
            spec.Pun_25_Cnt+1,
            spec.Pun_26_Cnt+1,
            spec.Pun_27_Cnt+1,
            spec.Pun_28_Cnt+1,
            spec.Pun_29_Cnt+1,
            spec.Pun_30_Cnt+1,
            spec.Pun_31_Cnt+1,
            spec.Pun_32_Cnt+1,
            spec.Pun_33_Cnt+1,
            spec.Pun_34_Cnt+1,
            spec.Pun_35_Cnt+1,
            spec.Pun_36_Cnt+1,
            spec.Pun_37_Cnt+1,
            spec.Pun_38_Cnt+1,
            spec.Pun_39_Cnt+1,
            spec.Pun_40_Cnt+1,
            spec.Pun_41_Cnt+1,
            spec.Pun_42_Cnt+1,
            spec.Pun_43_Cnt+1,
            spec.Pun_44_Cnt+1,
            spec.Pun_45_Cnt+1,
            spec.Pun_46_Cnt+1,
            spec.Pun_47_Cnt+1,
            spec.Pun_48_Cnt+1,
            spec.Pun_49_Cnt+1,
            spec.Pun_50_Cnt+1,
            spec2.Pun_51_Cnt+1,
            spec2.Pun_52_Cnt+1,
            spec2.Pun_53_Cnt+1,
            spec2.Pun_54_Cnt+1,
            spec2.Pun_55_Cnt+1,
            spec2.Pun_56_Cnt+1,
            spec2.Pun_57_Cnt+1,
            spec2.Pun_58_Cnt+1,
            spec2.Pun_59_Cnt+1,
            spec2.Pun_60_Cnt+1,
            spec2.Pun_61_Cnt+1,
            spec2.Pun_62_Cnt+1,
            spec2.Pun_63_Cnt+1,
            spec2.Pun_64_Cnt+1,
            spec2.Pun_65_Cnt+1,
            spec2.Pun_66_Cnt+1,
            spec2.Pun_67_Cnt+1,
            spec2.Pun_68_Cnt+1,
            spec2.Pun_69_Cnt+1,
            spec2.Pun_70_Cnt+1,
            spec3.Pun_71_Cnt+1,
            spec3.Pun_72_Cnt+1,
            spec3.Pun_73_Cnt+1,
            spec3.Pun_74_Cnt+1,
            spec3.Pun_75_Cnt+1,
            spec3.Pun_76_Cnt+1,
            spec3.Pun_77_Cnt+1,
            spec3.Pun_78_Cnt+1,
            spec3.Pun_79_Cnt+1,
            spec3.Pun_80_Cnt+1,
            spec4.Pun_81_Cnt+1,
            spec4.Pun_82_Cnt+1,
            spec4.Pun_83_Cnt+1,
            spec4.Pun_84_Cnt+1,
            spec4.Pun_85_Cnt+1,
            spec4.Pun_86_Cnt+1,
            spec4.Pun_87_Cnt+1,
            spec4.Pun_88_Cnt+1,
            spec4.Pun_89_Cnt+1,
            spec4.Pun_90_Cnt+1,
            spec5.Pun_91_Cnt+1,
            spec5.Pun_92_Cnt+1,
            spec5.Pun_93_Cnt+1,
            spec5.Pun_94_Cnt+1,
            spec5.Pun_95_Cnt+1,
            spec5.Pun_96_Cnt+1,
            spec5.Pun_97_Cnt+1,
            spec5.Pun_98_Cnt+1,
            spec5.Pun_99_Cnt+1,
            spec5.Pun_100_Cnt+1
        };

        for (int i = 0; i < SpecilMiss.childCount; i++)
        {
            // i 는 다음에 해금할 무기
            if (PlayerPrefsManager.GetInstance().weaponInfo[i].isUnlock)
            {
                // 커버이미지 벗기기
                specialBtn[i].parent.GetChild(4).gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < SpecilMiss.childCount -2; i++)
        {
            txtTop = SpecilMiss.GetChild(i).GetChild(1).GetComponent<Text>();
            txtTop.text = (Spec_UP[i] * 500) + "대 훈련";

            txt = SpecilMiss.GetChild(i).GetChild(2).GetComponent<Text>();
            // 달성해서 다이아 받은 지점에서 10 곱해서 맥스값
            txt.text = "횟수 : ( " + Spec_M[i] + " / " + (Spec_UP[i] * 500) + " )";
            // 달성했다면 커버 이미지 제거
            if (Spec_M[i] >= (Spec_UP[i] * 500))
            {
                /// 10,000회 제한.
                if (Spec_UP[i] >= 20)
                {
                    SpecMission[i].SetActive(true);
                }
                else
                {
                    specialBtn[i].GetChild(1).gameObject.SetActive(false);
                    // 하나라도 커버 이미지 벗겨진게 있으면
                    isSpecRed = true;
                }
 
            }
            else
            {
                // 달성 아니면 커버 다시 씌우기.
                specialBtn[i].GetChild(1).gameObject.SetActive(true);
            }

        }
        // 커버 이미지 여부에 따라 레드닷 끄고 켜고
        if (isSpecRed)
        {
            RedDotManager.GetInstance().QuestDotOn2();
        }
        else
        {
            RedDotManager.GetInstance().QuestDotOff2();
        }

        

    }

    /// <summary>
    /// 특별 미션 버튼에 할당
    /// </summary>
    /// <param name="_index"></param>
    public void GetRewordSpecialMission(int _index)
    {
        // 클릭했는데 커버 씌워져있으면 빠꾸 || 10,000회 넘으면 빠꾸
        if (specialBtn[_index].GetChild(1).gameObject.activeSelf) return;



        //보상 획득
        GetDiamond("10");
        PopUpObjectManager.GetInstance().ShowWarnnigProcess("보상으로 10 다이아를 받았습니다.");
        // 다이아 얻으면 커버 씌우기.
        specialBtn[_index].GetChild(1).gameObject.SetActive(true);
        // 십의 자리 하나 올려주기
        switch (_index)
        {
            case 0: PlayerPrefsManager.GetInstance().questInfo[0].Pun_01_Cnt++; break;
            case 1: PlayerPrefsManager.GetInstance().questInfo[0].Pun_02_Cnt++; break;
            case 2: PlayerPrefsManager.GetInstance().questInfo[0].Pun_03_Cnt++; break;
            case 3: PlayerPrefsManager.GetInstance().questInfo[0].Pun_04_Cnt++; break;
            case 4: PlayerPrefsManager.GetInstance().questInfo[0].Pun_05_Cnt++; break;
            case 5: PlayerPrefsManager.GetInstance().questInfo[0].Pun_06_Cnt++; break;
            case 6: PlayerPrefsManager.GetInstance().questInfo[0].Pun_07_Cnt++; break;
            case 7: PlayerPrefsManager.GetInstance().questInfo[0].Pun_08_Cnt++; break;
            case 8: PlayerPrefsManager.GetInstance().questInfo[0].Pun_09_Cnt++; break;
            case 9: PlayerPrefsManager.GetInstance().questInfo[0].Pun_10_Cnt++; break;
            case 10: PlayerPrefsManager.GetInstance().questInfo[0].Pun_11_Cnt++; break;
            case 11: PlayerPrefsManager.GetInstance().questInfo[0].Pun_12_Cnt++; break;
            case 12: PlayerPrefsManager.GetInstance().questInfo[0].Pun_13_Cnt++; break;
            case 13: PlayerPrefsManager.GetInstance().questInfo[0].Pun_14_Cnt++; break;
            case 14: PlayerPrefsManager.GetInstance().questInfo[0].Pun_15_Cnt++; break;
            case 15: PlayerPrefsManager.GetInstance().questInfo[0].Pun_16_Cnt++; break;
            case 16: PlayerPrefsManager.GetInstance().questInfo[0].Pun_17_Cnt++; break;
            case 17: PlayerPrefsManager.GetInstance().questInfo[0].Pun_18_Cnt++; break;
            case 18: PlayerPrefsManager.GetInstance().questInfo[0].Pun_19_Cnt++; break;
            case 19: PlayerPrefsManager.GetInstance().questInfo[0].Pun_20_Cnt++; break;
            case 20: PlayerPrefsManager.GetInstance().questInfo[0].Pun_21_Cnt++; break;
            case 21: PlayerPrefsManager.GetInstance().questInfo[0].Pun_22_Cnt++; break;
            case 22: PlayerPrefsManager.GetInstance().questInfo[0].Pun_23_Cnt++; break;
            case 23: PlayerPrefsManager.GetInstance().questInfo[0].Pun_24_Cnt++; break;
            case 24: PlayerPrefsManager.GetInstance().questInfo[0].Pun_25_Cnt++; break;
            case 25: PlayerPrefsManager.GetInstance().questInfo[0].Pun_26_Cnt++; break;
            case 26: PlayerPrefsManager.GetInstance().questInfo[0].Pun_27_Cnt++; break;
            case 27: PlayerPrefsManager.GetInstance().questInfo[0].Pun_28_Cnt++; break;
            case 28: PlayerPrefsManager.GetInstance().questInfo[0].Pun_29_Cnt++; break;
            case 29: PlayerPrefsManager.GetInstance().questInfo[0].Pun_30_Cnt++; break;
            case 30: PlayerPrefsManager.GetInstance().questInfo[0].Pun_31_Cnt++; break;
            case 31: PlayerPrefsManager.GetInstance().questInfo[0].Pun_32_Cnt++; break;
            case 32: PlayerPrefsManager.GetInstance().questInfo[0].Pun_33_Cnt++; break;
            case 33: PlayerPrefsManager.GetInstance().questInfo[0].Pun_34_Cnt++; break;
            case 34: PlayerPrefsManager.GetInstance().questInfo[0].Pun_35_Cnt++; break;
            case 35: PlayerPrefsManager.GetInstance().questInfo[0].Pun_36_Cnt++; break;
            case 36: PlayerPrefsManager.GetInstance().questInfo[0].Pun_37_Cnt++; break;
            case 37: PlayerPrefsManager.GetInstance().questInfo[0].Pun_38_Cnt++; break;
            case 38: PlayerPrefsManager.GetInstance().questInfo[0].Pun_39_Cnt++; break;
            case 39: PlayerPrefsManager.GetInstance().questInfo[0].Pun_40_Cnt++; break;
            case 40: PlayerPrefsManager.GetInstance().questInfo[0].Pun_41_Cnt++; break;
            case 41: PlayerPrefsManager.GetInstance().questInfo[0].Pun_42_Cnt++; break;
            case 42: PlayerPrefsManager.GetInstance().questInfo[0].Pun_43_Cnt++; break;
            case 43: PlayerPrefsManager.GetInstance().questInfo[0].Pun_44_Cnt++; break;
            case 44: PlayerPrefsManager.GetInstance().questInfo[0].Pun_45_Cnt++; break;
            case 45: PlayerPrefsManager.GetInstance().questInfo[0].Pun_46_Cnt++; break;
            case 46: PlayerPrefsManager.GetInstance().questInfo[0].Pun_47_Cnt++; break;
            case 47: PlayerPrefsManager.GetInstance().questInfo[0].Pun_48_Cnt++; break;
            case 48: PlayerPrefsManager.GetInstance().questInfo[0].Pun_49_Cnt++; break;
            case 49: PlayerPrefsManager.GetInstance().questInfo[0].Pun_50_Cnt++; break;
            case 50: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_51_Cnt++; break;
            case 51: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_52_Cnt++; break;
            case 52: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_53_Cnt++; break;
            case 53: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_54_Cnt++; break;
            case 54: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_55_Cnt++; break;
            case 55: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_56_Cnt++; break;
            case 56: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_57_Cnt++; break;
            case 57: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_58_Cnt++; break;
            case 58: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_59_Cnt++; break;
            case 59: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_60_Cnt++; break;
            case 60: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_61_Cnt++; break;
            case 61: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_62_Cnt++; break;
            case 62: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_63_Cnt++; break;
            case 63: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_64_Cnt++; break;
            case 64: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_65_Cnt++; break;
            case 65: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_66_Cnt++; break;
            case 66: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_67_Cnt++; break;
            case 67: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_68_Cnt++; break;
            case 68: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_69_Cnt++; break;
            case 69: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_70_Cnt++; break;
            //
            case 70: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_71_Cnt++; break;
            case 71: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_72_Cnt++; break;
            case 72: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_73_Cnt++; break;
            case 73: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_74_Cnt++; break;
            case 74: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_75_Cnt++; break;
            case 75: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_76_Cnt++; break;
            case 76: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_77_Cnt++; break;
            case 77: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_78_Cnt++; break;
            case 78: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_79_Cnt++; break;
            case 79: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_80_Cnt++; break;
            //
            case 80: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_81_Cnt++; break;
            case 81: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_82_Cnt++; break;
            case 82: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_83_Cnt++; break;
            case 83: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_84_Cnt++; break;
            case 84: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_85_Cnt++; break;
            case 85: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_86_Cnt++; break;
            case 86: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_87_Cnt++; break;
            case 87: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_88_Cnt++; break;
            case 88: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_89_Cnt++; break;
            case 89: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_90_Cnt++; break;
            // 0622
            case 90: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_91_Cnt++; break;
            case 91: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_92_Cnt++; break;
            case 92: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_93_Cnt++; break;
            case 93: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_94_Cnt++; break;
            case 94: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_95_Cnt++; break;
            case 95: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_96_Cnt++; break;
            case 96: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_97_Cnt++; break;
            case 97: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_98_Cnt++; break;
            case 98: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_99_Cnt++; break;
            case 99: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_100_Cnt++; break;

        }
        //PlayerPrefsManager.GetInstance().SavequestInfo();
        //PlayerPrefsManager.GetInstance().SavequestInfo2();
        //PlayerPrefsManager.GetInstance().SavequestInfo3();
        //PlayerPrefsManager.GetInstance().SavequestInfo4();
        //PlayerPrefsManager.GetInstance().SavequestInfo5();
        //PlayerPrefsManager.GetInstance().SavequestInfo6();

        // 보상 더 있나 새로고침
        SpecMissionUpdate();

    }

    /// <summary>
    /// 다이아 지급
    /// </summary>
    /// <param name="_Dia">스트링으로 작성</param>
    void GetDiamond(string _Dia)
    {
        //var dia = PlayerPrefsManager.GetInstance().diamond;
        //var result = dts.AddStringDouble(dia, _Dia);

        //PlayerPrefsManager.GetInstance().diamond = result;

        PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + float.Parse(_Dia));


        UserWallet.GetInstance().ShowAllMoney();
        //PlayerPrefsManager.GetInstance().SavequestInfo();
        //PlayerPrefsManager.GetInstance().SavequestInfo2();
        //PlayerPrefsManager.GetInstance().SavequestInfo3();
        //PlayerPrefsManager.GetInstance().SavequestInfo4();
        //PlayerPrefsManager.GetInstance().SavequestInfo5();
        //PlayerPrefsManager.GetInstance().SavequestInfo6();
    }


}
