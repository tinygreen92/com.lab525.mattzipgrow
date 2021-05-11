using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MyFriend { ATK, TIME, SPEEED, MATT }

public class FriendManager : MonoBehaviour
{
    readonly DoubleToStringNum dts = new DoubleToStringNum();

    [Header("- 동료 캐릭터 스탯 창 4개")]
    public Text[] CharaStats;

    [Header("- 동료 스프라이트 4개")]
    public GameObject[] FriendObjects;

    [Header("- 정보 표기 부분")]
    public Text[] LevelBox;
    public Text[] DescBox; // 효과

    [Header("- 버튼 부분")]
    public GameObject[] EquipButton;  
    public GameObject[] GrayButton;  
    public GameObject[] MaxButton;
    public Text[] UpgradeBox;
    public Image[] somoIcon;
    [Space]
    public Sprite SaalIcon;
    public Sprite DiaIcon;

    [Header("- 동료 4개")]
    public int[] thisLevels;

    /// <summary>
    /// 동료 최대 레벨 고정
    /// </summary>
    readonly int MAX_LEVEL = 1000;


    /// <summary>
    /// [캐릭터 버튼] 클릭시 갱신
    /// </summary>
    public void InitFriend()
    {
        int mLenth = FriendObjects.Length;

        for (int i = 0; i < mLenth; i++)
        {
            UpdateFriend((MyFriend)i);
        }
    }


    string mDefaltATK = string.Empty;
    readonly float mDefaltTime = 14400.0f;
    readonly float mDefaltSpeed = 1.0f;




    /// <summary>
    /// 업데이트
    /// </summary>
    /// <param name="_index"></param>
    public void UpdateFriend(MyFriend _index)
    {
        var ppm = PlayerPrefsManager.GetInstance();

        switch (_index)
        {
            case MyFriend.ATK:
                /// → 기본 오프라인 공격력은 현재 공격력의 1/10 
                mDefaltATK = dts.multipleStringDouble(ppm.PlayerDPS, 0.1d);
                thisLevels[0] = ppm.Friend_01_OfflineAtk_Lv;
                LevelBox[0].text = "LV. " + thisLevels[0];
                /// 번역
                if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                    DescBox[0].text = "오프라인 공격력 증가 " + (thisLevels[0] * 0.1f).ToString("f1") + "%";
                else
                    DescBox[0].text = "<color=#aee571>▲</color> offline attack power " + (thisLevels[0] * 0.1f).ToString("f1") + "%";

                /// 버튼 갱신
                SetSSalBtn(0);
                break;

            case MyFriend.TIME:
                /// → 기본 오프라인 시간은 14,400초 ( 4시간 ) 
                thisLevels[1] = ppm.Friend_02_OffTimeUp_Lv;
                LevelBox[1].text = "LV. " + thisLevels[1];
                /// 번역
                if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                    DescBox[1].text = "오프라인 시간 증가 " + (thisLevels[1] * 0.1f).ToString("f1") + "%";
                else
                    DescBox[1].text = "<color=#aee571>▲</color> offline time " + (thisLevels[1] * 0.1f).ToString("f1") + "%";
                /// 버튼 갱신
                SetSSalBtn(1);
                break;

            case MyFriend.SPEEED:
                /// → 기본 오프라인 공격속도는 1대/s
                thisLevels[2] = ppm.Friend_03_OffSpped_Lv;
                LevelBox[2].text = "LV. " + thisLevels[2];
                /// 번역
                if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                    DescBox[2].text = "오프라인 공격속도 증가 " + (thisLevels[2] * 0.1f).ToString("f1") + "%";
                else
                    DescBox[2].text = "<color=#aee571>▲</color> offline attack speed " + (thisLevels[2] * 0.1f).ToString("f1") + "%";
                /// 버튼 갱신
                SetSSalBtn(2);
                break;

            case MyFriend.MATT:
                /// → 기본 오프라인 맷집 증가율은 최종 공격력의 0.01% + 1.0 
                //mDefaltMatt = dts.multipleStringDouble(ppm.Mat_Mattzip, 0.0001d);
                //mDefaltMatt = dts.AddStringDouble(mDefaltMatt, "1");
                /// 최종 공격력의 0.01% + 1.0 
                mDefaltMatt = ppm.GetDPS_Bae(0.0001d, 1);

                thisLevels[3] = ppm.Friend_04_MattzipPer_Lv;
                LevelBox[3].text = "LV. " + thisLevels[3];
                /// 번역
                if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                    DescBox[3].text = "오프라인 맷집 증가율 증가 " + (thisLevels[3] * 0.1f).ToString("f1") + "%";
                else
                    DescBox[3].text = "<color=#aee571>▲</color> offline Mattzip growth rate " + (thisLevels[3] * 0.1f).ToString("f1") + "%";
                /// 버튼 갱신
                SetSSalBtn(3);
                break;
        }

        if (ppm.Friend_01_OfflineAtk_Lv > 0) FriendObjects[0].SetActive(true);
        if (ppm.Friend_02_OffTimeUp_Lv > 0) FriendObjects[1].SetActive(true);
        if (ppm.Friend_03_OffSpped_Lv > 0) FriendObjects[2].SetActive(true);
        if (ppm.Friend_04_MattzipPer_Lv > 0) FriendObjects[3].SetActive(true);
    }


    /// <summary>
    /// 쌀밥 주고 강화 버튼 클릭
    /// </summary>
    public void ClickedUpgradeBtn(int _index)
    {
        /// 맥스버튼 || 회색 버튼이면 리턴
        if (MaxButton[_index].activeSelf || GrayButton[_index].activeSelf) 
            return;

        var ppm = PlayerPrefsManager.GetInstance();
        /// 쌀밥 구매 판별
        if (thisLevels[_index] == 0)
        {
            /// 해금 팝업 호출
            //ShowDiaPunchOkay(_index);
            var tmp = PlayerPrefs.GetFloat("dDiamond", 0);
            PlayerPrefs.SetFloat("dDiamond", tmp - 1000);
            //return;
        }
        else
        {
            /// 정상적 국밥 소모
            ppm.ssalbap = IsPurchase(_index);
            UserWallet.GetInstance().ShowUserSSalbap();
        }


        /// 레벨 업
        switch (_index)
        {
            case 0:
                ppm.Friend_01_OfflineAtk_Lv++;
                break;

            case 1:
                ppm.Friend_02_OffTimeUp_Lv++;
                break;

            case 2:
                ppm.Friend_03_OffSpped_Lv++;
                break;

            case 3:
                ppm.Friend_04_MattzipPer_Lv++;
                break;

            default:
                break;
        }

        // 업뎃
        for (int i = 0; i < MaxButton.Length; i++)
        {
            UpdateFriend((MyFriend)i);
        }
    }


    /// <summary>
    /// 버튼 정보 새로 고침
    /// </summary>
    /// <param name="_somo"></param>
    /// <returns></returns>
    void SetSSalBtn(int _index)
    {
        /// 다이아 구매 전
        if (thisLevels[_index] == 0)
        {
            ///버튼 아이콘 다이아
            somoIcon[_index].sprite = DiaIcon;
            UpgradeBox[_index].text = "1000";

            if (PlayerPrefs.GetFloat("dDiamond", 0) - 1000 < 0)
            {
                GrayButton[_index].SetActive(true);
            }
            else
            {
                GrayButton[_index].SetActive(false);
            }
        }
        /// 만렙이면 맥스 버튼 활성화 후 리턴
        else if (thisLevels[_index] >= MAX_LEVEL)
        {
            MaxButton[_index].SetActive(true); // 노란 이미지
        }
        ///
        else
        {
            ///버튼 아이콘
            somoIcon[_index].sprite = SaalIcon;
            /// 재화 소모 가능한지
            if (IsPurchase(_index) != "-1")
            {
                GrayButton[_index].SetActive(false);
            }
            /// ㄴㄴ 업글 불가
            else
            {
                GrayButton[_index].SetActive(true);
            }
        }

        /// 캐릭터 창 갱신
        CharaStats[_index].text = GetOffFriendStat((MyFriend)_index);

    }


    /// <summary>
    /// 필요한 쌀밥 량 표기해주고 업글 가능한지 여부 리턴
    /// </summary>
    /// <param name="_index"></param>
    string IsPurchase(int _index)
    {
        double _result;

        switch (_index)
        {
            case 0:
                _result = 100 * Math.Pow(1.1d, thisLevels[_index] - 1);
                UpgradeBox[_index].text = UserWallet.GetInstance().SeetheNatural(_result);
                return dts.SubStringDouble(PlayerPrefsManager.GetInstance().ssalbap, _result);

            case 1:
                _result = 100 * Math.Pow(1.15d, thisLevels[_index] - 1);
                UpgradeBox[_index].text = UserWallet.GetInstance().SeetheNatural(_result);
                return dts.SubStringDouble(PlayerPrefsManager.GetInstance().ssalbap, _result);


            case 2:
                _result = 100 * Math.Pow(1.2d, thisLevels[_index] - 1);
                UpgradeBox[_index].text = UserWallet.GetInstance().SeetheNatural(_result);
                return dts.SubStringDouble(PlayerPrefsManager.GetInstance().ssalbap, _result);


            case 3:
                _result = 100 * Math.Pow(1.3d, thisLevels[_index] - 1);
                
                UpgradeBox[_index].text = UserWallet.GetInstance().SeetheNatural(_result);
                return dts.SubStringDouble(PlayerPrefsManager.GetInstance().ssalbap, _result);

            default:
                return "-1";
        }

    }

    /// <summary>
    /// 맷집 증가율 증가
    /// </summary>
    double mDefaltMatt;

    /// <summary>
    /// 캐릭터 정보창으로 가져오는 각 동료 스탯 수치
    /// </summary>
    /// <returns></returns>
    public string GetOffFriendStat(MyFriend _index)
    {
        string _result = string.Empty;

        switch (_index)
        {
            case MyFriend.ATK:
                return mDefaltATK;

            case MyFriend.TIME:
                var _ZeroTime = PlayerPrefs.GetInt("Friend_02_OffTimeUp_Lv", 0);

                if (_ZeroTime != 0)
                    _result = (mDefaltTime * _ZeroTime * 0.001f).ToString("f1");
                else
                    _result = mDefaltTime.ToString("f1");
                return _result;

            case MyFriend.SPEEED:

                var _ZeroSpeed = PlayerPrefs.GetInt("Friend_03_OffSpped_Lv", 0);

                if (_ZeroSpeed != 0)
                    _result = (mDefaltSpeed * (_ZeroSpeed * 0.001f) * 0.1f).ToString("f1");
                else
                    _result = mDefaltSpeed.ToString("f1");
                return _result;

            case MyFriend.MATT:

                return UserWallet.GetInstance().SeetheNatural(mDefaltMatt);

            default:
                return null;
        }

    } 








}
