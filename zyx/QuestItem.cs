using UnityEngine;
using UnityEngine.UI;


public class QuestItem : MonoBehaviour
{
    public QuestManager pm;
    [Header("- 회색 커버 오브젝트")]
    public GameObject GrayImage;

    [Header("- 아이콘")]
    public Image spriteBox;

    [Header("- 정보 표기 부분")]
    public Text NameBox;
    public Text DescBox;

    [Header("- 버튼 부분")]
    public GameObject GrayButton;
    public GameObject MaxButton;

    /// <summary>
    /// 펀치인덱스 0~99 
    /// </summary>
    int thisIndex;

    #region [초기 세팅] + 변동 정보 새로고침

    public void BoxInfoUpdate(int cnt)
    {
        /// 인덱스 설정 -> 이 스크립트 전체
        thisIndex = cnt;
        // 서순 1
        SetUpdateInfo(pm.GetSpec_M()[thisIndex], pm.GetSpec_UP()[thisIndex]);
        // 서순 2
        SetDefaltInfo();
    }

    /// <summary>
    /// 아이콘/이름/회색커버 기본 정보 새로고침
    /// </summary>
    void SetDefaltInfo()
    {
        spriteBox.sprite = pm.punchImgs[thisIndex];
        //NameBox.text = pm.PunchNames[thisIndex];
        /// 회색 커버
        SetAllGrayCover(thisIndex);
    }

    /// <summary>
    /// 방어전 성공시 해금
    /// </summary>
    public void SetAllGrayCover(int p_index)
    {
        if (thisIndex != p_index) return;

        /// isUnlock 이면 전체 [회색 커버]
        if (PlayerPrefsManager.GetInstance().weaponInfo[thisIndex].isUnlock)
        {
            GrayImage.SetActive(false);
        }
        else
        {
            GrayImage.SetActive(true);
        }
    }

    /// <summary>
    /// 변동 정보 출력
    /// </summary>
    public void SetUpdateInfo(long mText, long upText)
    {
        /// 텍스트 세팅

        /// 번역
        if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
        {
            NameBox.text = $"{upText * 1000}대 훈련";
            DescBox.text = $"횟수 : ( {mText} / {upText * 1000} )";
        }
        else
        {
            NameBox.text = $"{upText * 1000}대 번역";
            DescBox.text = $"번역 : ( {mText} / {upText * 1000} )";
        }

        /// 달성했다면 커버 이미지 제거
        /// ///TODO : [노랑/회색] 버튼 갱신
        if (mText >= upText * 1000)
        {
            /// 20,000회 제한. MAX 버튼 활성화.
            if (upText >= 20)
            {
                MaxButton.SetActive(true);
            }
            /// 아니면 계속 업글 가능
            else
            {
                GrayButton.SetActive(false);
            }

        }
        /// 달성 아니면 커버 다시 씌우기.
        else
        {
            GrayButton.SetActive(true);
        }
        
    }


    #endregion

    #region [정보 갱신] 변동 정보 새로고침

    // 훈련도구 가격
    readonly string tmpPrice;


    /// <summary>
    /// 외부에서 클릭하는 다이아몬드 수령 버튼
    /// </summary>
    public void ClickedGetDiaBtn()
    {
        /// 수령 조건이 안되었으면 리턴
        if (GrayButton.activeSelf) return;
        //보상 획득
        PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 10);
        UserWallet.GetInstance().ShowUserDia();
        PopUpObjectManager.GetInstance().ShowWarnnigProcess("보상으로 10 다이아를 받았습니다.");
        pm.tmm.ExUpdateMission(32); /// 미션 업데이트

        /// 다이아 얻으면 회색 커버 씌우기.
        GrayButton.SetActive(true);
        // 십의 자리 하나 올려주기
        switch (thisIndex)
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
        // 보상 더 있나 새로고침
        pm.SpecMissionUpdate();
    }


    #endregion



}
