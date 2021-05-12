using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public GameObject charCancas;
    [Space]
    public TutorialMissionManager tmm;
    public GroggyManager groggyManager;
    DoubleToStringNum dts = new DoubleToStringNum();
    [Header("- 캐릭터 창에 보일 이미지")]
    public Image charaBody;

    [Header("- 유니폼 왼쪽 버튼")]
    public GameObject[] equipBtn; // 장착 노란 [버튼]
    public GameObject[] unEquipBtn; // 장착중 그레이 이미지

    [Header("- 유니폼 오른쪽 버튼")]
    public GameObject[] upgradeBtn; // 쌀밥 표기 업그레이드 [버튼] 
    public GameObject[] buyBtn; // 다이아로 구매 노란 텍스트
    public GameObject[] grayBtn; // 쌀밥없음 그레이 이미지
    public GameObject[] maxUpgradeBtn; // 쌀밥 강화 맥스 이미지

    [Header("-유니폼 옷걸이")]
    public SpriteRenderer[] CharaBodySprite;
    [Header("-유니폼 베이스옷")]
    public Sprite[] UniformSprite_0;
    [Header("-유니폼 트레이닝 옷")]
    public Sprite[] UniformSprite_1;
    public Sprite[] UniformSprite_2;
    public Sprite[] UniformSprite_3;
    public Sprite[] UniformSprite_4;
    public Sprite[] UniformSprite_5;
    public Sprite[] UniformSprite_6;
    [Header("-유니폼 장신구")]
    public Sprite[] UniformSprite_Punch;
    public Sprite[] UniformSprite_Boot;
    public Sprite[] UniformSprite_Hell;
    public Sprite[] UniformSprite_Cape;
    public Sprite[] UniformSprite_Something;

    [Header("-유니폼 레벨업 관련 텍스트")]
    public Text[] Uniform_LV;
    public Text[] Uniform_Price;
    public Text[] Uniform_Desc;

    [Header("-강화 텍스트 박스")]
    //public Text POWER_UP_TEXT;
    public Text POWER_UP_LV;
    public Text POWER_UP_Price;

    [Header("-버튼 회색으로 덮기")]
    public GameObject POWER_UP_Gray;

    [Header("-맥스 버튼 덮기")]
    public GameObject POWER_UP_Max;

    /// 캐릭터 정보 레벨업 내부 정보
    int CharaLv;
    string Chara_Attack_UP;
    string Chara_HP_UP;
    string Chara_Recov_UP;


    [Header("-패시브 스킬 배열")]
    public Text[] SKILL_UP_LV;
    public Text[] SKILL_UP_Price;
    public Text[] SKILL_UP_Desc;
    public GameObject[] SKILL_UP_Gray;
    public GameObject[] SKILL_UP_Max;

    [Header("-장신구 배열")]
    public GameObject[] Pet_Equip; // 버튼 그 자체
    public GameObject[] Pet_unEquip; // 왼쪽 그레이 이미지

    public GameObject[] Pyt_buyBtn; // buy text = 다이아 3000개
    public GameObject[] Pyt_upBtn; // 업그레이드 text 국밥 갯수
    public GameObject[] Pet_Gray; // 회색판
    public GameObject[] Pet_Max; // 맥스판
    public Text[] Pet_UP_LV;
    public Text[] Pet_UP_Price;
    public Text[] Pet_UP_Desc;



    /// <summary>
    /// 유니티폼 탭 초기화 -> 게임스타트에서 불러준다
    /// </summary>
    public void UniformInit()
    {
        if (!PlayerPrefs.HasKey("Uniform_Data") || PlayerPrefs.GetString("Uniform_Data") == "")
        {
            PlayerPrefsManager.GetInstance().Uniform_Data = "1+0+0+0+0+0+0";
        }

        string[] sDataList = (PlayerPrefsManager.GetInstance().Uniform_Data).Split('+');

        for (int i = 0; i < sDataList.Length; i++)
        {
            /// 해당 구매 기록이 존재하면
            if (sDataList[i] == "1")
            {
                buyBtn[i].SetActive(false); // 구매 텍스트 숨겨주고
                //
                equipBtn[i].SetActive(true); //  장착가능 노란 [버튼] 활성화.
                /// 번역
                if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                    equipBtn[i].GetComponentInChildren<Text>().text = "장착"; // 장착가능 노란 [버튼] 활성화.
                else
                    equipBtn[i].GetComponentInChildren<Text>().text = "Mount"; // 장착가능 노란 [버튼] 활성화.
                upgradeBtn[i].transform.GetChild(2).gameObject.SetActive(true); // 쌀밥 표기 켜줌

            }
            else /// 구매하지 않았음.
            {
                buyBtn[i].SetActive(true); // 구매 텍스트 활성화.
                grayBtn[i].SetActive(false); // 구매 가능할땐 그레이 이미지 비활성화.
                //
                unEquipBtn[i].SetActive(false); //  장착가능 노란 [버튼] 활성화.
                upgradeBtn[i].transform.GetChild(2).gameObject.SetActive(false); // 쌀밥 표기 꺼줌
                unEquipBtn[i].SetActive(true); // 그 위에 그레이 버튼으로 덮을 것. (미구매 표기용)
                /// 번역
                if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                    equipBtn[i].GetComponentInChildren<Text>().text = "미보유"; // 장착가능 노란 [버튼] 활성화.
                else
                    equipBtn[i].GetComponentInChildren<Text>().text = "None"; // 장착가능 노란 [버튼] 활성화.
            }

        }

        // 마지막에 착용한거로 유니폼 세팅.
        Real_Uniform_Equip(PlayerPrefs.GetInt("Uniform_Curent", 0));

        // 유니폼 수치 적용
        Uniform_Refresh();

        // 캐릭터 레벨업 돈 체크
        Power_UP_Cheak();

        //패시브 스킬 갱신
        Skill_Refresh();

        // 펫 새로 고침
        Pet_Refresh();

    }


    /// <summary>
    /// 유니폼 항목에 달려있는 오른쪽 버튼
    /// </summary>
    public void Uinform_RightClicked(int _index)
    {
        // 구매 버튼이 활성화 상태라면?
        if (buyBtn[_index].activeSelf)
        {
            /// 1. 구매 안했으면 다이아 표기해주고 구매.
            Uniform_Purchase(_index);
        }
        else /// 구매 버튼 비활성화 = 쌀밥 업그레이드 대기 중.
        {
            Uniform_Upgrade(_index);
        }
    }



    string dia_pass = "";

    bool dia_Cheack(int _index)
    {
        // 구매 여부 데이터 배열
        string[] sDataList = (PlayerPrefsManager.GetInstance().Uniform_Data).Split('+');

        /// 구매내역이 없어야 한다
        if (sDataList[_index] == "0")
        {
            // 구매가격
            int _price = 0;

            switch (_index)
            {
                case 0: break;
                case 1: _price = 1000; break;
                case 2: _price = 1000; break;
                case 3: _price = 1000; break;
                case 4: _price = 2000; break;
                case 5: _price = 2000; break;
                case 6: _price = 3000; break;
            }
            // 체크 통과했으면 
            dia_pass = dts.SubStringDouble(PlayerPrefs.GetFloat("dDiamond").ToString("f0"), _price);


            // 다이아 없으면 false
            if (dia_pass == "-1")
            {
                grayBtn[_index].SetActive(true);

                return false;
            }
            else // 구매 가능하면 트루
            {
                grayBtn[_index].SetActive(false);
                return true;
            }
        }
        else // 구매내역 있으면?
        {
            return false;
        }

    }

    /// <summary>
    /// 유니폼 구매 버튼에 할당해서 호출
    /// 1. 구매 안했으면 다이아 표기해주고 구매.
    /// </summary>
    void Uniform_Purchase(int _index)
    {
        // 구매 여부 데이터 배열
        string[] sDataList = (PlayerPrefsManager.GetInstance().Uniform_Data).Split('+');

        if (dia_Cheack(_index))
        {
            // 구매한적 없다면 다이아 소모
            PlayerPrefs.SetFloat("dDiamond", float.Parse(dia_pass));
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("성공적으로 구입하셨습니다.");
            else
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("Successfully purchased.");            // 데이터에 구매 완료 표기
            sDataList[_index] = "1";
        }
        else // 돈 없으면 꺼져
        {
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("보유 다이아가 부족합니다.");
            else
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("Not enough diamonds.");
            
            return;
        }

        UserWallet.GetInstance().ShowAllMoney();

        // 버튼 처리 해줌.
        buyBtn[_index].SetActive(false); // 구매 텍스트 숨겨주고

        unEquipBtn[_index].SetActive(false); //  장착가능 그레이 버튼 비활성화.
        /// 번역
        if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
            equipBtn[_index].GetComponentInChildren<Text>().text = "장착"; //  장착 가능!
        else
            equipBtn[_index].GetComponentInChildren<Text>().text = "Mount"; //  장착 가능!

        upgradeBtn[_index].transform.GetChild(2).gameObject.SetActive(true); // 쌀밥 표기 켜줌

        // 데이터 새로고침
        string result = "";

        result += sDataList[0] + "+";
        result += sDataList[1] + "+";
        result += sDataList[2] + "+";
        result += sDataList[3] + "+";
        result += sDataList[4] + "+";
        result += sDataList[5] + "+";
        result += sDataList[6];

        PlayerPrefsManager.GetInstance().Uniform_Data = result;
        PlayerPrefs.Save();

        // 데이터 리프레시
        Uniform_Refresh();
    }
    

    /// <summary>
    /// 구매된 거 다시 한번 확인
    /// </summary>
    void Uniform_PurchaceRefresh()
    {
        // 구매 여부 데이터 배열
        string[] sDataList = (PlayerPrefsManager.GetInstance().Uniform_Data).Split('+');

        for (int _index = 0; _index < buyBtn.Length; _index++)
        {
            if(sDataList[_index] == "1")
            {
                // 버튼 처리 해줌.
                buyBtn[_index].SetActive(false); // 구매 텍스트 숨겨주고

                // 얘는 지금 끼고 있는 애다?
                if (PlayerPrefs.GetInt("Uniform_Curent", 0) == _index)
                {
                    unEquipBtn[_index].SetActive(true); //  그레이 버튼 활성화
                    /// 번역
                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        equipBtn[_index].GetComponentInChildren<Text>().text = "장착중"; // 장착가능 노란 [버튼] 활성화.
                    else
                        equipBtn[_index].GetComponentInChildren<Text>().text = "Mounting"; // 장착가능 노란 [버튼] 활성화.
                }
                else
                {
                    unEquipBtn[_index].SetActive(false); //  그레이 버튼 비활성화
                    /// 번역
                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        equipBtn[_index].GetComponentInChildren<Text>().text = "장착"; // 장착가능 노란 [버튼] 활성화.
                    else
                        equipBtn[_index].GetComponentInChildren<Text>().text = "Mount"; // 장착가능 노란 [버튼] 활성화.
                }
                upgradeBtn[_index].transform.GetChild(2).gameObject.SetActive(true); // 쌀밥 표기 켜줌
            }
        }
    }





    /// <summary>
    /// 장신구 다이아 체크
    /// </summary>
    bool Pet_diaCheack()
    {
        //Debug.LogError("장신구 다이아 체크");
        // 체크 통과했으면 
        if (Mathf.FloorToInt(PlayerPrefs.GetFloat("dDiamond")) - 3000 < 0)
        {
            dia_pass = "-1";
        }
        else
        {
            dia_pass = (Mathf.FloorToInt(PlayerPrefs.GetFloat("dDiamond")) - 3000).ToString("f0");
        }

        // 다이아 없으면 false
        if (dia_pass == "-1")
        {
            for (int i = 0; i < Pet_Gray.Length; i++)
            {
                Pet_Gray[i].SetActive(true);
            }

            return false;
        }
        else // 구매 가능하면 트루
        {
            //Debug.LogError("구매가능 트루");
            for (int i = 0; i < Pet_Gray.Length; i++)
            {
                Pet_Gray[i].SetActive(false);
            }
            return true;
        }
    }

    /// <summary>
    /// 장신구 구매 가격 3000 고정
    /// </summary>
    /// <param name="_index"></param>
    public void Pet_PurchaseUpgrade(int _index)
    {
        // 구입버튼 꺼져있음 = 이미 구매한 상태 = 국밥으로 강화하는 중
        if (!Pyt_buyBtn[_index].activeSelf)
        {
            Pet_Upgrade(_index);
            return;
        }

        // 돈 없으면 꺼져
        if (!Pet_diaCheack())
        {
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("보유 다이아가 부족합니다.");
            else
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("Not enough diamonds.");
            
            return;
        }

        // 구매 여부 데이터 배열
        int sDataList = PlayerPrefs.GetInt("Pet_BuyData", 000);
        /// 0717 추가 황금망토 망토
        //int isBuyCape = PlayerPrefs.GetInt("Pet_BuyData_Cape", 0);

        // 구매한적 없다면 다이아 소모
        PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 3000);
        if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("성공적으로 구입하셨습니다.");
        else
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("Successfully purchased.");
        // 데이터에 구매 완료 표기
        if (_index == 0)            /// 글러브
        {
            if (sDataList == 000)
            {
                PlayerPrefs.SetInt("Pet_BuyData", 100);
            }
            else if (sDataList == 010)
            {
                PlayerPrefs.SetInt("Pet_BuyData", 110);
            }
            else if (sDataList == 001)
            {
                PlayerPrefs.SetInt("Pet_BuyData", 101);
            }
            else if (sDataList == 011)
            {
                PlayerPrefs.SetInt("Pet_BuyData", 111);
            }
        }
        else if (_index == 1)       /// 부츠
        {
            if (sDataList == 000)
            {
                PlayerPrefs.SetInt("Pet_BuyData", 010);
            }
            else if (sDataList == 100)
            {
                PlayerPrefs.SetInt("Pet_BuyData", 110);
            }
            else if (sDataList == 001)
            {
                PlayerPrefs.SetInt("Pet_BuyData", 011);
            }
            else if (sDataList == 101)
            {
                PlayerPrefs.SetInt("Pet_BuyData", 111);
            }
        }
        else if (_index == 2)       /// 헬멧
        {
            if (sDataList == 000)
            {
                PlayerPrefs.SetInt("Pet_BuyData", 001);
            }
            else if (sDataList == 100)
            {
                PlayerPrefs.SetInt("Pet_BuyData", 101);
            }
            else if (sDataList == 010)
            {
                PlayerPrefs.SetInt("Pet_BuyData", 011);
            }
            else if (sDataList == 110)
            {
                PlayerPrefs.SetInt("Pet_BuyData", 111);
            }
        }
        else if (_index == 3)   /// 망토
        {
            PlayerPrefs.SetInt("Pet_BuyData_Cape", 1);
        }
        else if (_index == 4)   /// 신규 장신구
        {
            PlayerPrefs.SetInt("Pet_BuyData_Something", 1);
        }

        PlayerPrefs.Save();

        UserWallet.GetInstance().ShowAllMoney();

        // 버튼 처리 해줌.
        Pyt_buyBtn[_index].SetActive(false); // 다이아  텍스트 숨겨주고

        Pet_unEquip[_index].SetActive(false); //  장착가능 그레이 버튼 비활성화.
        /// 번역
        if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
            Pet_Equip[_index].GetComponentInChildren<Text>().text = "장착"; //  장착가능 그레이 버튼 비활성화.
        else
            Pet_Equip[_index].GetComponentInChildren<Text>().text = "Mount"; //  장착가능 그레이 버튼 비활성화.
        Pyt_upBtn[_index].SetActive(true); // 쌀밥 표기 켜줌

        // 새로고침
        Pet_Refresh();
    }





    /// <summary>
    /// 장신구 장착
    /// 구매 했으면 장착
    /// </summary>
    public void Pet_Epuip(int _index)
    {
        /// 구입 텍스트가 표시 중이면 장착 못함
        if (Pyt_buyBtn[_index].activeSelf) return;


        /// 이미 장착중인데 누르면?
        if (Pet_unEquip[_index].activeSelf)
        {
            Pet_unEquip[_index].SetActive(false); // 회색버튼 벗는다.
            /// 번역
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                Pet_Equip[_index].GetComponentInChildren<Text>().text = "장착"; // (장창중 표기용)
            else
                Pet_Equip[_index].GetComponentInChildren<Text>().text = "Mount"; // (장창중 표기용)

            // 펫 장착 인덱스 설정 0은 아무것도 안 입은거
            PlayerPrefs.SetInt("Pet_Curent", 0);
            // 데이터 저장
            PlayerPrefs.Save();
            // 유니폼 실제 이미지 장착해줌
            ChageUniMiniPet(PlayerPrefs.GetInt("Uniform_Curent", 0), 0);

        }
        else /// 장착중 아닐때 누르면
        {
            for (int i = 0; i < Pet_unEquip.Length; i++)
            {
                // 구입한 항목 장착중 그레이 버튼 초기화.
                if (!Pyt_buyBtn[i].activeSelf)
                {
                    /// 번역
                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        Pet_Equip[i].GetComponentInChildren<Text>().text = "장착"; // (장창중 표기용)
                    else
                        Pet_Equip[i].GetComponentInChildren<Text>().text = "Mount"; // (장창중 표기용)
                    Pet_unEquip[i].SetActive(false);
                }
            }

            Pet_unEquip[_index].SetActive(true); // 해당 위치 그레이 버튼으로 덮을 것. (장창중 표기용)
            /// 번역
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                Pet_Equip[_index].GetComponentInChildren<Text>().text = "장착중"; // (장창중 표기용)
            else
                Pet_Equip[_index].GetComponentInChildren<Text>().text = "Mounting"; // (장창중 표기용)

            // 펫 장착 인덱스 설정
            PlayerPrefs.SetInt("Pet_Curent", _index + 1);
            // 데이터 저장
            PlayerPrefs.Save();
            // 유니폼 실제 이미지 장착해줌
            ChageUniMiniPet(PlayerPrefs.GetInt("Uniform_Curent", 0), _index + 1);
        }


    }

    /// <summary>
    /// 장신구 정보 새로고침
    /// 1. 구입여부?
    /// 2. 장착 / 미장착?
    /// 3. 국밥 업글 가능?
    /// </summary>
    public void Pet_Refresh()
    {
        // 다이아몬드 체크 한번 돌려주기
        Pet_diaCheack();

        int index = PlayerPrefs.GetInt("Pet_BuyData", 000);
        //int index_Cape = PlayerPrefs.GetInt("Pet_BuyData_Cape", 0);
        //int index_Something = PlayerPrefs.GetInt("Pet_BuyData_Something", 0);

        // 구매 여부 판단
        if (PlayerPrefs.GetInt("Pet_BuyData_Cape", 0) == 1)
        {
            // 버튼 처리 해줌.
            Pet_unEquip[3].SetActive(false); // 국밥 표기 켜짐
            Pyt_buyBtn[3].SetActive(false); // 다이아 꺼짐
            /// 번역
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                Pet_Equip[3].GetComponentInChildren<Text>().text = "장착";
            else
                Pet_Equip[3].GetComponentInChildren<Text>().text = "Mount";
            Pyt_upBtn[3].SetActive(true); // 쌀밥 표기 켜줌
        }
        else
        {
            Pet_unEquip[3].SetActive(true); //  장착불가 회색등 활성화
            /// 번역
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                Pet_Equip[3].GetComponentInChildren<Text>().text = "미보유";
            else
                Pet_Equip[3].GetComponentInChildren<Text>().text = "None";
        }

        // 구매 여부 판단
        if (PlayerPrefs.GetInt("Pet_BuyData_Something", 0) == 1)
        {
            // 버튼 처리 해줌.
            Pet_unEquip[4].SetActive(false); // 국밥 표기 켜짐
            Pyt_buyBtn[4].SetActive(false); // 다이아 꺼짐
            /// 번역
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                Pet_Equip[4].GetComponentInChildren<Text>().text = "장착";
            else
                Pet_Equip[4].GetComponentInChildren<Text>().text = "Mount";
            Pyt_upBtn[4].SetActive(true); // 쌀밥 표기 켜줌
        }
        else
        {
            Pet_unEquip[4].SetActive(true); //  장착불가 회색등 활성화
            /// 번역
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                Pet_Equip[4].GetComponentInChildren<Text>().text = "미보유";
            else
                Pet_Equip[4].GetComponentInChildren<Text>().text = "None";
        }



        // 구매 여부 판단
        if (index == 100)
        {
            // 버튼 처리 해줌.
            Pet_unEquip[0].SetActive(false); // 국밥 표기 켜짐
            Pyt_buyBtn[0].SetActive(false); // 다이아 꺼짐
            Pyt_upBtn[0].SetActive(true); // 쌀밥 표기 켜줌
            Pet_unEquip[1].SetActive(true); //  장착불가 회색등 활성화
            Pet_unEquip[2].SetActive(true); //  장착불가 회색등 활성화

            //
            /// 번역
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
            {
                Pet_Equip[0].GetComponentInChildren<Text>().text = "장착";
                Pet_Equip[1].GetComponentInChildren<Text>().text = "미보유";
                Pet_Equip[2].GetComponentInChildren<Text>().text = "미보유";
            }
            else
            {
                Pet_Equip[0].GetComponentInChildren<Text>().text = "Mount";
                Pet_Equip[1].GetComponentInChildren<Text>().text = "None";
                Pet_Equip[2].GetComponentInChildren<Text>().text = "None";
            }


        }
        else if (index == 010)
        {
            // 버튼 처리 해줌.
            Pet_unEquip[1].SetActive(false); // 국밥 표기 켜짐
            Pyt_buyBtn[1].SetActive(false); // 다이아 꺼짐
            Pyt_upBtn[1].SetActive(true); // 쌀밥 표기 켜줌

            Pet_unEquip[0].SetActive(true); //  장착불가 회색등 활성화
            Pet_unEquip[2].SetActive(true); //  장착불가 회색등 활성화
                                            //
            /// 번역
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
            {
                Pet_Equip[1].GetComponentInChildren<Text>().text = "장착";
                Pet_Equip[0].GetComponentInChildren<Text>().text = "미보유";
                Pet_Equip[2].GetComponentInChildren<Text>().text = "미보유";
            }
            else
            {
                Pet_Equip[1].GetComponentInChildren<Text>().text = "Mount";
                Pet_Equip[0].GetComponentInChildren<Text>().text = "None";
                Pet_Equip[2].GetComponentInChildren<Text>().text = "None";
            }

        }
        else if (index == 001)
        {
            // 버튼 처리 해줌.
            Pet_unEquip[2].SetActive(false); // 국밥 표기 켜짐
            Pyt_buyBtn[2].SetActive(false); // 다이아 꺼짐
            Pyt_upBtn[2].SetActive(true); // 쌀밥 표기 켜줌

            //
            Pet_unEquip[1].SetActive(true); //  장착불가 회색등 활성화
            Pet_unEquip[0].SetActive(true); //  장착불가 회색등 활성화

            /// 번역
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
            {
                Pet_Equip[1].GetComponentInChildren<Text>().text = "미보유";
                Pet_Equip[0].GetComponentInChildren<Text>().text = "미보유";
                Pet_Equip[2].GetComponentInChildren<Text>().text = "장착";
            }
            else
            {
                Pet_Equip[1].GetComponentInChildren<Text>().text = "None";
                Pet_Equip[0].GetComponentInChildren<Text>().text = "None";
                Pet_Equip[2].GetComponentInChildren<Text>().text = "Mount";
            }

        }
        else if (index == 110)
        {
            // 버튼 처리 해줌.
            Pet_unEquip[0].SetActive(false); // 국밥 표기 켜짐
            Pyt_buyBtn[0].SetActive(false); // 다이아 꺼짐
            Pet_unEquip[1].SetActive(false); // 국밥 표기 켜짐
            Pyt_buyBtn[1].SetActive(false); // 다이아 꺼짐
            Pyt_upBtn[0].SetActive(true); // 쌀밥 표기 켜줌
            Pyt_upBtn[1].SetActive(true); // 쌀밥 표기 켜줌

            //
            Pet_unEquip[2].SetActive(true); //  장착불가 회색등 활성화
            /// 번역
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
            {
                Pet_Equip[1].GetComponentInChildren<Text>().text = "장착";
                Pet_Equip[2].GetComponentInChildren<Text>().text = "미보유";
                Pet_Equip[0].GetComponentInChildren<Text>().text = "장착";
            }
            else
            {
                Pet_Equip[1].GetComponentInChildren<Text>().text = "Mount";
                Pet_Equip[2].GetComponentInChildren<Text>().text = "None";
                Pet_Equip[0].GetComponentInChildren<Text>().text = "Mount";
            }


        }
        else if (index == 101)
        {
            // 버튼 처리 해줌.
            Pet_unEquip[0].SetActive(false); // 국밥 표기 켜짐
            Pyt_buyBtn[0].SetActive(false); // 다이아 꺼짐
            Pet_unEquip[2].SetActive(false); // 국밥 표기 켜짐
            Pyt_buyBtn[2].SetActive(false); // 다이아 꺼짐
            Pyt_upBtn[0].SetActive(true); // 쌀밥 표기 켜줌
            Pyt_upBtn[2].SetActive(true); // 쌀밥 표기 켜줌

            //
            Pet_unEquip[1].SetActive(true); //  장착불가 회색등 활성화
            /// 번역
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
            {
                Pet_Equip[2].GetComponentInChildren<Text>().text = "장착";
                Pet_Equip[0].GetComponentInChildren<Text>().text = "장착";
                Pet_Equip[1].GetComponentInChildren<Text>().text = "미보유";
            }
            else
            {
                Pet_Equip[2].GetComponentInChildren<Text>().text = "Mount";
                Pet_Equip[0].GetComponentInChildren<Text>().text = "Mount";
                Pet_Equip[1].GetComponentInChildren<Text>().text = "None";
            }

        }
        else if (index == 011)
        {
            // 버튼 처리 해줌.
            Pet_unEquip[1].SetActive(false); // 국밥 표기 켜짐
            Pyt_buyBtn[1].SetActive(false); // 다이아 꺼짐
            Pet_unEquip[2].SetActive(false); // 국밥 표기 켜짐
            Pyt_buyBtn[2].SetActive(false); // 다이아 꺼짐
            Pyt_upBtn[1].SetActive(true); // 쌀밥 표기 켜줌
            Pyt_upBtn[2].SetActive(true); // 쌀밥 표기 켜줌

            //
            Pet_unEquip[0].SetActive(true); //  장착불가 회색등 활성화
            /// 번역
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
            {
                Pet_Equip[2].GetComponentInChildren<Text>().text = "장착";
                Pet_Equip[0].GetComponentInChildren<Text>().text = "미보유";
                Pet_Equip[1].GetComponentInChildren<Text>().text = "장착";
            }
            else
            {
                Pet_Equip[2].GetComponentInChildren<Text>().text = "Mount";
                Pet_Equip[0].GetComponentInChildren<Text>().text = "None";
                Pet_Equip[1].GetComponentInChildren<Text>().text = "Mount";
            }

        }
        else if (index == 111)
        {
            // 버튼 처리 해줌.
            Pet_unEquip[0].SetActive(false); // 국밥 표기 켜짐
            Pyt_buyBtn[0].SetActive(false); // 다이아 꺼짐
            Pet_unEquip[1].SetActive(false); // 국밥 표기 켜짐
            Pyt_buyBtn[1].SetActive(false); // 다이아 꺼짐
            Pet_unEquip[2].SetActive(false); // 국밥 표기 켜짐
            Pyt_buyBtn[2].SetActive(false); // 다이아 꺼짐
            Pyt_upBtn[0].SetActive(true); // 쌀밥 표기 켜줌
            Pyt_upBtn[1].SetActive(true); // 쌀밥 표기 켜줌
            Pyt_upBtn[2].SetActive(true); // 쌀밥 표기 켜줌
            /// 번역
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
            {
                Pet_Equip[1].GetComponentInChildren<Text>().text = "장착";
                Pet_Equip[2].GetComponentInChildren<Text>().text = "장착";
                Pet_Equip[0].GetComponentInChildren<Text>().text = "장착";
            }
            else
            {
                Pet_Equip[1].GetComponentInChildren<Text>().text = "Mount";
                Pet_Equip[2].GetComponentInChildren<Text>().text = "Mount";
                Pet_Equip[0].GetComponentInChildren<Text>().text = "Mount";
            }


        }
        else
        {
            Pet_unEquip[0].SetActive(true); //  장착불가 회색등 활성화
            Pet_unEquip[1].SetActive(true); //  장착불가 회색등 활성화
            Pet_unEquip[2].SetActive(true); //  장착불가 회색등 활성화

            /// 번역
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
            {
                Pet_Equip[1].GetComponentInChildren<Text>().text = "미보유";
                Pet_Equip[0].GetComponentInChildren<Text>().text = "미보유";
                Pet_Equip[2].GetComponentInChildren<Text>().text = "미보유";
            }
            else
            {
                Pet_Equip[1].GetComponentInChildren<Text>().text = "None";
                Pet_Equip[0].GetComponentInChildren<Text>().text = "None";
                Pet_Equip[2].GetComponentInChildren<Text>().text = "None";
            }

        }

        string result = "";

        for (int i = 0; i < Pet_UP_LV.Length; i++)
        {
            switch (i)
            {
                case 0:
                    /// 번역
                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        result = "맷집 게이지 " + (0.05f * PlayerPrefsManager.GetInstance().Pet_Touch_Lv).ToString("f2") + "% 감소";
                    else
                        result = "<color=#e53935>▼</color> Mattzip gauge " + (0.05f * PlayerPrefsManager.GetInstance().Pet_Touch_Lv).ToString("f2") + "%";
                    Pet_UP_LV[i].text = "Lv." + PlayerPrefsManager.GetInstance().Pet_Touch_Lv;
                    break;

                case 1:
                    /// 번역
                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        result = "국밥 버프 요구 터치 횟수 " + PlayerPrefsManager.GetInstance().Pet_Buff_Lv + "회 차감";
                    else
                        result = "<color=#e53935>▼</color> Number of touches required for soup buff " + PlayerPrefsManager.GetInstance().Pet_Buff_Lv;
                    Pet_UP_LV[i].text = "Lv." + PlayerPrefsManager.GetInstance().Pet_Buff_Lv;
                    break;

                case 2:
                    /// 번역
                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        result = "맷집 증가량 " + (0.005f * PlayerPrefsManager.GetInstance().Pet_Matt_Up_Lv).ToString("f3") + "% 증가";
                    else
                        result = "<color=#aee571>▲</color> Amount of Mattzip " + (0.005f * PlayerPrefsManager.GetInstance().Pet_Matt_Up_Lv).ToString("f3") + "%";
                    Pet_UP_LV[i].text = "Lv." + PlayerPrefsManager.GetInstance().Pet_Matt_Up_Lv;
                    break;

                case 3:
                    /// 번역
                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        result = "PVP 맷집 적용 " + (0.04f * PlayerPrefsManager.GetInstance().Pet_PVP_Matt_Lv).ToString("f2") + "% 증가";
                    else
                        result = "<color=#aee571>▲</color> PvP Mattzip application " + (0.04f * PlayerPrefsManager.GetInstance().Pet_PVP_Matt_Lv).ToString("f2") + "%";
                    Pet_UP_LV[i].text = "Lv." + PlayerPrefsManager.GetInstance().Pet_PVP_Matt_Lv;
                    break;

                case 4:
                    /// 번역
                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        result = "PVP 공격 속도 " + (0.5f * PlayerPrefsManager.GetInstance().Pet_PVP_Speed_Lv).ToString("f1") + "% 증가";
                    else
                        result = "업데이트 전 " + (0.5f * PlayerPrefsManager.GetInstance().Pet_PVP_Speed_Lv).ToString("f1") + "% 증가";
                    Pet_UP_LV[i].text = "Lv." + PlayerPrefsManager.GetInstance().Pet_PVP_Speed_Lv;
                    break;

            }

            // 업글할 만한 국밥 있냐?
            Pet_GookBapCheak(i);

            int thisLv = 0;

            switch (i)
            {
                case 0: thisLv = PlayerPrefsManager.GetInstance().Pet_Touch_Lv; break;
                case 1: thisLv = PlayerPrefsManager.GetInstance().Pet_Buff_Lv; break;
                case 2: thisLv = PlayerPrefsManager.GetInstance().Pet_Matt_Up_Lv; break;
                case 3: thisLv = PlayerPrefsManager.GetInstance().Pet_PVP_Matt_Lv; break;
                case 4: thisLv = PlayerPrefsManager.GetInstance().Pet_PVP_Speed_Lv; break;
            }
            // 레벨 확인해서 MAX 뚜껑 닫아줌
            // 근데 만렙이다?
            if (thisLv >= 1000)
            {
                Pet_Max[i].SetActive(true);
            }
            else if (PlayerPrefsManager.GetInstance().Pet_Buff_Lv >= 500)
            {
                Pet_Max[1].SetActive(true);
            }

            if (PlayerPrefs.GetInt("Pet_Curent", 0) != 0)
            {
                Pet_unEquip[PlayerPrefs.GetInt("Pet_Curent", 0)-1].SetActive(true); // 해당 위치 그레이 버튼으로 덮을 것. (장창중 표기용)
                /// 번역
                if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                    Pet_Equip[PlayerPrefs.GetInt("Pet_Curent", 0)-1].GetComponentInChildren<Text>().text = "장착중"; // (장창중 표기용)
                else
                    Pet_Equip[PlayerPrefs.GetInt("Pet_Curent", 0)-1].GetComponentInChildren<Text>().text = "Mount"; // (장창중 표기용)
            }

            Pet_UP_Price[i].text = UserWallet.GetInstance().SeetheNatural(pet_upgrade_price);
            Pet_UP_Desc[i].text = result;

        }


        //int petEquip = PlayerPrefs.GetInt("Pet_Curent", 0);
        //if (petEquip > 0)
        //{
        //    Pet_Equip[petEquip -1].SetActive(false); // 장착중
        //}
        //else
        //{
        //    Pet_Equip[0].SetActive(true); // 아무도 장착 ㄴ
        //    Pet_Equip[1].SetActive(true); // 아무도 장착 ㄴ
        //    Pet_Equip[2].SetActive(true); // 아무도 장착 ㄴ
        //}


    }

    double pet_upgrade_price = 0;
    /// <summary>
    /// 해당 인덱스의 가격 받아와서 국밥 있나 비교
    /// </summary>
    /// <returns></returns>
    bool Pet_GookBapCheak(int _index)
    {
        switch (_index)
        {
            case 0: pet_upgrade_price = (1 + PlayerPrefsManager.GetInstance().Pet_Touch_Lv) * (1+ PlayerPrefsManager.GetInstance().Pet_Touch_Lv) * 100d; break;
            case 1: pet_upgrade_price = (1 + PlayerPrefsManager.GetInstance().Pet_Buff_Lv) * (1 + PlayerPrefsManager.GetInstance().Pet_Buff_Lv) * 200d; break;
            case 2: pet_upgrade_price = (1 + PlayerPrefsManager.GetInstance().Pet_Matt_Up_Lv) * (1 + PlayerPrefsManager.GetInstance().Pet_Matt_Up_Lv) * 300d; break;
            case 3: pet_upgrade_price = (1 + PlayerPrefsManager.GetInstance().Pet_PVP_Matt_Lv) * (1 + PlayerPrefsManager.GetInstance().Pet_PVP_Matt_Lv) * 500d; break;
            case 4: pet_upgrade_price = (1 + PlayerPrefsManager.GetInstance().Pet_PVP_Speed_Lv) * (1 + PlayerPrefsManager.GetInstance().Pet_PVP_Speed_Lv) * 500d; break;

        }

        // 체크 통과했으면 국밥 체크 해줘라.
        Power_UPgoldPass = dts.SubStringDouble(PlayerPrefsManager.GetInstance().gupbap, pet_upgrade_price);

        // 구입 안했으면 리턴 = 다이아 가격 텍스트가 표시 상태면
        if (!Pyt_buyBtn[_index].activeSelf)
        {
            // 쌀밥 없으면 false
            if (Power_UPgoldPass == "-1")
            {
                //Debug.LogError("국밥없다 그레이");

                Pet_Gray[_index].SetActive(true);
                return false;
            }
            else // 구매 가능하면 트루
            {
                //Debug.LogError("국밥으로 구매가능 트루");

                Pet_Gray[_index].SetActive(false);
                return true;
            }
        }
        else
        {
            return false;
        }

    }

    /// <summary>
    /// 누르면 국밥 소모해서 스킬 업그레이드
    /// </summary>
    /// <param name="_index"></param>
    public void Pet_Upgrade(int _index)
    {
        int thisLv = 0;

        switch (_index)
        {
            case 0: thisLv = PlayerPrefsManager.GetInstance().Pet_Touch_Lv; break;
            case 1: thisLv = PlayerPrefsManager.GetInstance().Pet_Buff_Lv;  break;
            case 2: thisLv = PlayerPrefsManager.GetInstance().Pet_Matt_Up_Lv; break;
            case 3: thisLv = PlayerPrefsManager.GetInstance().Pet_PVP_Matt_Lv; break;
            case 4: thisLv = PlayerPrefsManager.GetInstance().Pet_PVP_Speed_Lv; break;
        }

        // 쌀밥 체크해서 통과 못하면 리턴
        if (!Pet_GookBapCheak(_index)) return;

        // 쌀법 충분하면 레벨업 -> 

        // 근데 만렙이다?
        if (thisLv >= 1000)
        {
            Pet_Max[_index].SetActive(true);
            return;
        }
        else if(_index == 1 && PlayerPrefsManager.GetInstance().Pet_Buff_Lv >= 500)
        {
            Pet_Max[1].SetActive(true);
            return;
        }

        PlayerPrefsManager.GetInstance().gupbap = Power_UPgoldPass;
        UserWallet.GetInstance().ShowUserMilk();

        //레벨업
        switch (_index)
        {
            case 0: PlayerPrefsManager.GetInstance().Pet_Touch_Lv++; break;
            case 1: PlayerPrefsManager.GetInstance().Pet_Buff_Lv++; break;
            case 2: PlayerPrefsManager.GetInstance().Pet_Matt_Up_Lv++; break;
            case 3: PlayerPrefsManager.GetInstance().Pet_PVP_Matt_Lv++; break;
            case 4: PlayerPrefsManager.GetInstance().Pet_PVP_Speed_Lv++; break;
        }

        // 스탯 상승
        Pet_Refresh();

        // 데이터 저장
        PlayerPrefs.Save();
    }



















    /// <summary>
    /// 유니폼 장착
    /// 구매 했으면 장착
    /// </summary>
    public void Chara_Epuip(int _index)
    {
        // 이미 장착 중이면 클릭 안된다.
        if (unEquipBtn[_index].activeSelf) return;

        string[] sDataList = (PlayerPrefsManager.GetInstance().Uniform_Data).Split('+');

        /// 구매내역이 없으면 접근 못함 
        if (sDataList[_index] == "0" ) return;

        for (int i = 0; i < unEquipBtn.Length; i++)
        {
            // 구입한 항목 장착중 그레이 버튼 초기화.]
            if (sDataList[_index] == "1")
            {
                unEquipBtn[i].SetActive(false);
                /// 번역
                if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                    equipBtn[i].GetComponentInChildren<Text>().text = "장착";
                else
                    equipBtn[i].GetComponentInChildren<Text>().text = "Mount";
            }

            if (sDataList[i] == "0")
            {
                buyBtn[i].SetActive(true); // 구매 텍스트 활성화.
                upgradeBtn[i].transform.GetChild(2).gameObject.SetActive(false); // 쌀밥 표기 꺼줌
                unEquipBtn[i].SetActive(true); // 그 위에 그레이 버튼으로 덮을 것. (미구매 표기용)
                /// 번역
                if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                    equipBtn[i].GetComponentInChildren<Text>().text = "미보유";
                else
                    equipBtn[i].GetComponentInChildren<Text>().text = "None";
            }
        }



        // 유니폼 장착 인덱스 설정
        PlayerPrefs.SetInt("Uniform_Curent", _index);

        // 게임판 갈아입기
        Real_Uniform_Equip(_index);
    }


    void Real_Uniform_Equip(int _index)
    {
        //Debug.LogError("리얼 유니폼 입음");
        // 유니폼 실제 이미지 장착해줌
        switch (_index)
        {
            case 0: ChageUniformBase(); break;
            case 1: ChageUniform_01(); break;
            // 총 7개인듯
            case 2: ChageUniform_02(); break;
            case 3: ChageUniform_03(); break;
            case 4: ChageUniform_04(); break;
            case 5: ChageUniform_05(); break;
            case 6: ChageUniform_06(); break;
        }

        equipBtn[_index].SetActive(true); // 이큅 버튼 활성화.
        unEquipBtn[_index].SetActive(true); // 그 위에 그레이 버튼으로 덮을 것. (장창중 표기용)
        /// 번역
        if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
            equipBtn[_index].GetComponentInChildren<Text>().text = "장착중"; // (장창중 표기용)
        else
            equipBtn[_index].GetComponentInChildren<Text>().text = "Mounting"; // (장창중 표기용)

        // 유니폼 실제 이미지 장착해줌
        ChageUniMiniPet(_index, PlayerPrefs.GetInt("Pet_Curent", 0));

        // 데이터 저장
        PlayerPrefs.Save();

        if (PlayerPrefs.GetInt("Pet_Curent", 0) != 0)
        {
            Pet_unEquip[PlayerPrefs.GetInt("Pet_Curent", 0) - 1].SetActive(true); // 그 위에 그레이 버튼으로 덮을 것. (장창중 표기용)
            /// 번역
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                Pet_Equip[PlayerPrefs.GetInt("Pet_Curent", 0) - 1].GetComponentInChildren<Text>().text = "장착중"; // (장창중 표기용)
            else
                Pet_Equip[PlayerPrefs.GetInt("Pet_Curent", 0) - 1].GetComponentInChildren<Text>().text = "Mounting"; // (장창중 표기용)
        }




    }

    /// <summary>
    /// 장신구 머 입었냐?
    /// </summary>
    void ChageUniMiniPet(int _index, int pet_index)
    {
        //Debug.LogError("장신구 교체");

        // 아무것도 안 입음
        if (pet_index == 0)
        {
            // 유니폼 실제 이미지 장착해줌
            switch (_index)
            {
                case 0: ChageUniformBase(); break;
                case 1: ChageUniform_01(); break;
                // 총 7개인듯
                case 2: ChageUniform_02(); break;
                case 3: ChageUniform_03(); break;
                case 4: ChageUniform_04(); break;
                case 5: ChageUniform_05(); break;
                case 6: ChageUniform_06(); break;
            }


            equipBtn[_index].SetActive(true); // 이큅 버튼 활성화.
            unEquipBtn[_index].SetActive(true); // 그 위에 그레이 버튼으로 덮을 것. (장창중 표기용)
            /// 번역
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                equipBtn[_index].GetComponentInChildren<Text>().text = "장착중"; // (장창중 표기용)
            else
                equipBtn[_index].GetComponentInChildren<Text>().text = "Mounting"; // (장창중 표기용)
            return;
        }

        // iDle
        for (int i = 0; i < 5; i++)
        {
            if(pet_index == 1)
            {
                CharaBodySprite[i].sprite = UniformSprite_Punch[0 + (3 * _index)];
                charaBody.sprite = UniformSprite_Punch[0 + (3 * _index)];
            }
            if(pet_index == 2)
            {
                CharaBodySprite[i].sprite = UniformSprite_Boot[0 + (3 * _index)];
                charaBody.sprite = UniformSprite_Boot[0 + (3 * _index)];
            }
            if (pet_index == 3)
            {
                CharaBodySprite[i].sprite = UniformSprite_Hell[0 + (3 * _index)];
                charaBody.sprite = UniformSprite_Hell[0 + (3 * _index)];
            }
            if (pet_index == 4)
            {
                CharaBodySprite[i].sprite = UniformSprite_Cape[0 + (3 * _index)];
                charaBody.sprite = UniformSprite_Cape[0 + (3 * _index)];
            }
            if (pet_index == 5)
            {
                CharaBodySprite[i].sprite = UniformSprite_Something[0 + (3 * _index)];
                charaBody.sprite = UniformSprite_Something[0 + (3 * _index)];
            }
        }
        // grrogy
        for (int i = 4; i < 10; i++)
        {
            if (pet_index == 1)
            {
                CharaBodySprite[i].sprite = UniformSprite_Punch[1 + (3 * _index)];
            }
            if (pet_index == 2)
            {
                CharaBodySprite[i].sprite = UniformSprite_Boot[1 + (3 * _index)];
            }
            if (pet_index == 3)
            {
                CharaBodySprite[i].sprite = UniformSprite_Hell[1 + (3 * _index)];
            }
            if (pet_index == 4)
            {
                CharaBodySprite[i].sprite = UniformSprite_Cape[1 + (3 * _index)];
            }
            if (pet_index == 5)
            {
                CharaBodySprite[i].sprite = UniformSprite_Something[1 + (3 * _index)];
            }
        }
        // heal
        for (int i = 10; i < 12; i++)
        {
            if (pet_index == 1)
            {
                CharaBodySprite[i].sprite = UniformSprite_Punch[2 + (3 * _index)];
            }
            if (pet_index == 2)
            {
                CharaBodySprite[i].sprite = UniformSprite_Boot[2 + (3 * _index)];
            }
            if (pet_index == 3)
            {
                CharaBodySprite[i].sprite = UniformSprite_Hell[2 + (3 * _index)];
            }
            if (pet_index == 4)
            {
                CharaBodySprite[i].sprite = UniformSprite_Cape[2 + (3 * _index)];
            }
            if (pet_index == 5)
            {
                CharaBodySprite[i].sprite = UniformSprite_Something[2 + (3 * _index)];
            }
        }

        equipBtn[_index].SetActive(true); // 이큅 버튼 활성화.
        unEquipBtn[_index].SetActive(true); // 그 위에 그레이 버튼으로 덮을 것. (장창중 표기용)
        /// 번역
        if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
            equipBtn[_index].GetComponentInChildren<Text>().text = "장착중"; // (장창중 표기용)
        else
            equipBtn[_index].GetComponentInChildren<Text>().text = "Mounting"; // (장창중 표기용)

    }



    void ChageUniformBase()
    {
        // iDle
        for (int i = 0; i < 4; i++)
        {
            CharaBodySprite[i].sprite = UniformSprite_0[0];
        }
        // grrogy
        for (int i = 4; i < 10; i++)
        {
            CharaBodySprite[i].sprite = UniformSprite_0[1];
        }
        // heal
        for (int i = 10; i < 12; i++)
        {
            CharaBodySprite[i].sprite = UniformSprite_0[2];
        }
        // mini
        CharaBodySprite[12].sprite = UniformSprite_0[3];
        CharaBodySprite[13].sprite = UniformSprite_0[4];

        // 캐릭터 정보 옷 바꿔
        charaBody.sprite = UniformSprite_0[0];

    }
    void ChageUniform_01()
    {
        // iDle
        for (int i = 0; i < 4; i++)
        {
            CharaBodySprite[i].sprite = UniformSprite_1[0];
        }
        // grrogy
        for (int i = 4; i < 10; i++)
        {
            CharaBodySprite[i].sprite = UniformSprite_1[1];
        }
        // heal
        for (int i = 10; i < 12; i++)
        {
            CharaBodySprite[i].sprite = UniformSprite_1[2];
        }
        // mini
        CharaBodySprite[12].sprite = UniformSprite_1[3];
        CharaBodySprite[13].sprite = UniformSprite_1[4];

        // 캐릭터 정보 옷 바꿔
        charaBody.sprite = UniformSprite_1[0];
    }
    void ChageUniform_02()
    {
        // iDle
        for (int i = 0; i < 4; i++)
        {
            CharaBodySprite[i].sprite = UniformSprite_2[0];
        }
        // grrogy
        for (int i = 4; i < 10; i++)
        {
            CharaBodySprite[i].sprite = UniformSprite_2[1];
        }
        // heal
        for (int i = 10; i < 12; i++)
        {
            CharaBodySprite[i].sprite = UniformSprite_2[2];
        }
        // mini
        CharaBodySprite[12].sprite = UniformSprite_2[3];
        CharaBodySprite[13].sprite = UniformSprite_2[4];

        // 캐릭터 정보 옷 바꿔
        charaBody.sprite = UniformSprite_2[0];
    }
    void ChageUniform_03()
    {
        // iDle
        for (int i = 0; i < 4; i++)
        {
            CharaBodySprite[i].sprite = UniformSprite_3[0];
        }
        // grrogy
        for (int i = 4; i < 10; i++)
        {
            CharaBodySprite[i].sprite = UniformSprite_3[1];
        }
        // heal
        for (int i = 10; i < 12; i++)
        {
            CharaBodySprite[i].sprite = UniformSprite_3[2];
        }
        // mini
        CharaBodySprite[12].sprite = UniformSprite_3[3];
        CharaBodySprite[13].sprite = UniformSprite_3[4];

        // 캐릭터 정보 옷 바꿔
        charaBody.sprite = UniformSprite_3[0];
    }
    void ChageUniform_04()
    {
        // iDle
        for (int i = 0; i < 4; i++)
        {
            CharaBodySprite[i].sprite = UniformSprite_4[0];
        }
        // grrogy
        for (int i = 4; i < 10; i++)
        {
            CharaBodySprite[i].sprite = UniformSprite_4[1];
        }
        // heal
        for (int i = 10; i < 12; i++)
        {
            CharaBodySprite[i].sprite = UniformSprite_4[2];
        }
        // mini
        CharaBodySprite[12].sprite = UniformSprite_4[3];
        CharaBodySprite[13].sprite = UniformSprite_4[4];

        // 캐릭터 정보 옷 바꿔
        charaBody.sprite = UniformSprite_4[0];
    }
    void ChageUniform_05()
    {
        // iDle
        for (int i = 0; i < 4; i++)
        {
            CharaBodySprite[i].sprite = UniformSprite_5[0];
        }
        // grrogy
        for (int i = 4; i < 10; i++)
        {
            CharaBodySprite[i].sprite = UniformSprite_5[1];
        }
        // heal
        for (int i = 10; i < 12; i++)
        {
            CharaBodySprite[i].sprite = UniformSprite_5[2];
        }
        // mini
        CharaBodySprite[12].sprite = UniformSprite_5[3];
        CharaBodySprite[13].sprite = UniformSprite_5[4];

        // 캐릭터 정보 옷 바꿔
        charaBody.sprite = UniformSprite_5[0];
    }
    void ChageUniform_06()
    {
        // iDle
        for (int i = 0; i < 4; i++)
        {
            CharaBodySprite[i].sprite = UniformSprite_6[0];
        }
        // grrogy
        for (int i = 4; i < 10; i++)
        {
            CharaBodySprite[i].sprite = UniformSprite_6[1];
        }
        // heal
        for (int i = 10; i < 12; i++)
        {
            CharaBodySprite[i].sprite = UniformSprite_6[2];
        }
        // mini
        CharaBodySprite[12].sprite = UniformSprite_6[3];
        CharaBodySprite[13].sprite = UniformSprite_6[4];

        // 캐릭터 정보 옷 바꿔
        charaBody.sprite = UniformSprite_6[0];
    }



    // 0 = idle
    // 1 = groggy1
    // 2 = groggy2
    [Header(" - pvp 용 스프라이트들")]
    public SpriteRenderer[] pvpCharaBodySprite;
    /// <summary>
    /// 외부 호출용 -> PVP 캐릭터 옷 바꿔줌
    /// </summary>
    /// <param name="_index">순수 유니폼 </param>
    /// <param name="pet_index">악세서리 입었나 여부</param>
    /// <param name="inputThree">적이라면 3을 적으세요 </param>
    public void Chage_My_Pvp(int _index, int pet_index, int inputThree)
    {
        //Debug.LogError("장신구 교체");

        // 아무것도 안 입음
        if (pet_index == 0)
        {
            // 유니폼 실제 이미지 장착해줌
            switch (_index)
            {
                case 0:
                    pvpCharaBodySprite[0 + inputThree].sprite = UniformSprite_0[0];
                    pvpCharaBodySprite[1 + inputThree].sprite = UniformSprite_0[1];
                    pvpCharaBodySprite[2 + inputThree].sprite = UniformSprite_0[1];
                    break;
                case 1:
                    pvpCharaBodySprite[0 + inputThree].sprite = UniformSprite_1[0];
                    pvpCharaBodySprite[1 + inputThree].sprite = UniformSprite_1[1];
                    pvpCharaBodySprite[2 + inputThree].sprite = UniformSprite_1[1];
                    break;
                case 2:
                    pvpCharaBodySprite[0 + inputThree].sprite = UniformSprite_2[0];
                    pvpCharaBodySprite[1 + inputThree].sprite = UniformSprite_2[1];
                    pvpCharaBodySprite[2 + inputThree].sprite = UniformSprite_2[1];
                    break;
                case 3:
                    pvpCharaBodySprite[0 + inputThree].sprite = UniformSprite_3[0];
                    pvpCharaBodySprite[1 + inputThree].sprite = UniformSprite_3[1];
                    pvpCharaBodySprite[2 + inputThree].sprite = UniformSprite_3[1];
                    break;
                case 4:
                    pvpCharaBodySprite[0 + inputThree].sprite = UniformSprite_4[0];
                    pvpCharaBodySprite[1 + inputThree].sprite = UniformSprite_4[1];
                    pvpCharaBodySprite[2 + inputThree].sprite = UniformSprite_4[1];
                    break;
                case 5:
                    pvpCharaBodySprite[0 + inputThree].sprite = UniformSprite_5[0];
                    pvpCharaBodySprite[1 + inputThree].sprite = UniformSprite_5[1];
                    pvpCharaBodySprite[2 + inputThree].sprite = UniformSprite_5[1];
                    break;
                case 6:
                    pvpCharaBodySprite[0 + inputThree].sprite = UniformSprite_6[0];
                    pvpCharaBodySprite[1 + inputThree].sprite = UniformSprite_6[1];
                    pvpCharaBodySprite[2 + inputThree].sprite = UniformSprite_6[1];
                    break;
            }


            return;
        }

        else if (pet_index == 1)
        {
            pvpCharaBodySprite[0 + inputThree].sprite = UniformSprite_Punch[0 + (3 * _index)];
            pvpCharaBodySprite[1 + inputThree].sprite = UniformSprite_Punch[1 + (3 * _index)];
            pvpCharaBodySprite[2 + inputThree].sprite = UniformSprite_Punch[1 + (3 * _index)];
        }
        else if (pet_index == 2)
        {
            pvpCharaBodySprite[0 + inputThree].sprite = UniformSprite_Boot[0 + (3 * _index)];
            pvpCharaBodySprite[1 + inputThree].sprite = UniformSprite_Boot[1 + (3 * _index)];
            pvpCharaBodySprite[2 + inputThree].sprite = UniformSprite_Boot[1 + (3 * _index)];
        }
        else if (pet_index == 3)
        {
            pvpCharaBodySprite[0 + inputThree].sprite = UniformSprite_Hell[0 + (3 * _index)];
            pvpCharaBodySprite[1 + inputThree].sprite = UniformSprite_Hell[1 + (3 * _index)];
            pvpCharaBodySprite[2 + inputThree].sprite = UniformSprite_Hell[1 + (3 * _index)];
        }
        else if (pet_index == 4)
        {
            pvpCharaBodySprite[0 + inputThree].sprite = UniformSprite_Cape[0 + (3 * _index)];
            pvpCharaBodySprite[1 + inputThree].sprite = UniformSprite_Cape[1 + (3 * _index)];
            pvpCharaBodySprite[2 + inputThree].sprite = UniformSprite_Cape[1 + (3 * _index)];
        }
        else if (pet_index == 5)
        {
            pvpCharaBodySprite[0 + inputThree].sprite = UniformSprite_Something[0 + (3 * _index)];
            pvpCharaBodySprite[1 + inputThree].sprite = UniformSprite_Something[1 + (3 * _index)];
            pvpCharaBodySprite[2 + inputThree].sprite = UniformSprite_Something[1 + (3 * _index)];
        }
    }





    /// <summary>
    /// 해당 인덱스의 가격 받아와서 쌀밥 있나 비교
    /// </summary>
    /// <returns></returns>
    bool Uniform_SSalCheak(int _index)
    {
        // 클릭한 놈 레벨?
        upgrade_LV = PlayerPrefsManager.GetInstance().uniformInfo[_index].Uniform_LV + 1;

        switch (_index)
        {
            /// 기본 의상일 때는 구매 비활성화.
            case 1: upgrade_price = upgrade_LV * upgrade_LV * 50d; break;
            case 2: upgrade_price = upgrade_LV * upgrade_LV * 50d; break;
            case 3: upgrade_price = upgrade_LV * upgrade_LV * 100d; break;
            case 4: upgrade_price = upgrade_LV * upgrade_LV * 100d; break;
            case 5: upgrade_price = upgrade_LV * upgrade_LV * 200d; break;
            case 6: upgrade_price = upgrade_LV * upgrade_LV * 200d; break;
        }

        PlayerPrefsManager.GetInstance().uniformInfo[_index].Uniform_Price = upgrade_price;

        // 구매 텍스트 비활성화 일때만 쌀밥 체크
        if (!buyBtn[_index].activeSelf)
        {
            // 체크 통과했으면 쌀밥 체크 해줘라.
            Power_UPgoldPass = dts.SubStringDouble(PlayerPrefsManager.GetInstance().ssalbap, upgrade_price);

            // 쌀밥 없으면 false
            if (Power_UPgoldPass == "-1")
            {
                grayBtn[_index].SetActive(true);
                return false;
            }
            else // 구매 가능하면 트루
            {
                grayBtn[_index].SetActive(false);
                return true;
            }
        }
        else
        {
            return false;

        }

    }


    double upgrade_price = 0;
    int upgrade_LV = 0;
    /// <summary>
    /// 누르면 쌀밥 소모해서 유니폼 업그레이드
    /// </summary>
    /// <param name="_index"></param>
    void Uniform_Upgrade(int _index)
    {
        int thisLv = PlayerPrefsManager.GetInstance().uniformInfo[_index].Uniform_LV;
        // 쌀밥 체크해서 통과 못하면 리턴
        if (!Uniform_SSalCheak(_index)) return;

        // 쌀법 충분하면 레벨업 -> 
        // 근데 만렙이다?
        if (thisLv >= 9999)
        {
            maxUpgradeBtn[_index].SetActive(true);
            return;
        }

        PlayerPrefsManager.GetInstance().ssalbap = Power_UPgoldPass;
        UserWallet.GetInstance().ShowUserSSalbap();

        //레벨업
        PlayerPrefsManager.GetInstance().uniformInfo[_index].Uniform_LV++;
        Uniform_SSalCheak(_index);
        // 스탯 상승
        SetUniform_LV(_index, thisLv +1);
        Setform_Price(_index, upgrade_price);
        // 데이터 리프레시
        Uniform_Refresh();

        // 유니폼 데이터 저장
        PlayerPrefsManager.GetInstance().SaveuniformData();
    }







    double skill_upgrade_price = 0;
    int skill_upgrade_LV = 0;
    /// <summary>
    /// 해당 인덱스의 가격 받아와서 국밥 있나 비교
    /// </summary>
    /// <returns></returns>
    bool Skill_SSalCheak(int _index)
    {
        // 클릭한 놈 레벨?
        skill_upgrade_LV = PlayerPrefsManager.GetInstance().uniformInfo[_index].Skill_LV + 1;

        switch (_index)
        {
            case 0: skill_upgrade_price = skill_upgrade_LV * skill_upgrade_LV * 10d; break;
            case 1: skill_upgrade_price = skill_upgrade_LV * skill_upgrade_LV * 50d; break;
            case 2: skill_upgrade_price = skill_upgrade_LV * skill_upgrade_LV * 7d; break;
            case 3: skill_upgrade_price = skill_upgrade_LV * skill_upgrade_LV * 500d; break;
            case 4: skill_upgrade_price = skill_upgrade_LV * skill_upgrade_LV * 1000d; break;
            case 5: skill_upgrade_price = skill_upgrade_LV * skill_upgrade_LV * 1000d; break;
            case 6: skill_upgrade_price = skill_upgrade_LV * skill_upgrade_LV * 2000d; break;
        }

        PlayerPrefsManager.GetInstance().uniformInfo[_index].Skill_Price = skill_upgrade_price;


        // 체크 통과했으면 쌀밥 체크 해줘라.
        Power_UPgoldPass = dts.SubStringDouble(PlayerPrefsManager.GetInstance().gupbap, skill_upgrade_price);

        // 쌀밥 없으면 false
        if (Power_UPgoldPass == "-1")
        {
            SKILL_UP_Gray[_index].SetActive(true);
            return false;
        }
        else // 구매 가능하면 트루
        {
            SKILL_UP_Gray[_index].SetActive(false);
            return true;
        }
    }

    /// <summary>
    /// 누르면 국밥 소모해서 스킬 업그레이드
    /// </summary>
    /// <param name="_index"></param>
    public void Skill_Upgrade(int _index)
    {
        int thisLv = PlayerPrefsManager.GetInstance().uniformInfo[_index].Skill_LV;
        // 쌀밥 체크해서 통과 못하면 리턴
        if (!Skill_SSalCheak(_index)) return;

        // 쌀법 충분하면 레벨업 -> 
        // 근데 만렙이다?
        if (thisLv >= 10000)
        {
            SKILL_UP_Max[_index].SetActive(true);
            return;
        }

        PlayerPrefsManager.GetInstance().gupbap = Power_UPgoldPass;
        UserWallet.GetInstance().ShowUserMilk();

        //레벨업
        PlayerPrefsManager.GetInstance().uniformInfo[_index].Skill_LV++;
        tmm.ExUpdateMission(14); /// 미션 업데이트
        tmm.ExUpdateMission(16); /// 미션 업데이트

        Uniform_SSalCheak(_index);
        // 스탯 상승
        SkillSetUniform_LV(_index, thisLv + 1);
        SkillSetform_Price(_index, skill_upgrade_price);
        // 데이터 리프레시
        Skill_Refresh();

        // 데이터 저장
        PlayerPrefsManager.GetInstance().SaveuniformData();
    }


    /// <summary>
    /// 스킬 보유 효과 수정하려면
    /// </summary>
    /// <param name="_index"></param>
    /// <param name="_contents"></param>
    void SkillSetUniform_LV(int _index, int _contents)
    {
        PlayerPrefsManager.GetInstance().uniformInfo[_index].Skill_LV = _contents;
    }

    void SkillSetform_Price(int _index, double _contents)
    {
        PlayerPrefsManager.GetInstance().uniformInfo[_index].Skill_Price = _contents;
    }











    /// <summary>
    /// 유니폼 보유 효과 수정하려면
    /// </summary>
    /// <param name="_index"></param>
    /// <param name="_contents"></param>
    void SetUniform_LV(int _index, int _contents)
    {
        PlayerPrefsManager.GetInstance().uniformInfo[_index].Uniform_LV = _contents;
    }

    void Setform_Price(int _index, double _contents)
    {
        PlayerPrefsManager.GetInstance().uniformInfo[_index].Uniform_Price = _contents;
    }

    /// <summary>
    /// 유니폼 능력치 갱신
    /// </summary>
    public void Uniform_Refresh()
    {
        string[] sDataList = (PlayerPrefsManager.GetInstance().Uniform_Data).Split('+');

        string result = "";
        float _contents = 0;

        for (int i = 1; i < 7; i++)
        {
            // 다이아몬드 체크
            dia_Cheack(i);

            _contents = PlayerPrefsManager.GetInstance().uniformInfo[i].Uniform_LV;

            /// 번역
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
            {
                switch (i)
                {
                    case 1: result = "골드 획득량 " + Mathf.FloorToInt(_contents * 1f) + "% 증가"; break;
                    case 2: result = "골드 획득량 " + Mathf.FloorToInt(_contents * 1f) + "% 증가"; break;
                    case 3: result = "국밥 획득량 " + Mathf.FloorToInt(_contents * 1f) + "% 증가"; break;
                    case 4: result = "국밥 획득량 " + Mathf.FloorToInt(_contents * 1f) + "% 증가"; break;
                    case 5: result = "쌀밥 획득량 " + Mathf.FloorToInt(_contents * 1f) + "% 증가"; break;
                    case 6: result = "쌀밥 획득량 " + Mathf.FloorToInt(_contents * 1f) + "% 증가"; break;
                }
            }
            else
            {
                switch (i)
                {
                    case 1: result = "<color=#aee571>▲</color> Acquisition gold " + Mathf.FloorToInt(_contents * 1f) + "%"; break;
                    case 2: result = "<color=#aee571>▲</color> Acquisition gold " + Mathf.FloorToInt(_contents * 1f) + "%"; break;
                    case 3: result = "<color=#aee571>▲</color> Acquisition Korean soup " + Mathf.FloorToInt(_contents * 1f) + "%"; break;
                    case 4: result = "<color=#aee571>▲</color> Acquisition Korean soup " + Mathf.FloorToInt(_contents * 1f) + "%"; break;
                    case 5: result = "<color=#aee571>▲</color> Acquisition rice " + Mathf.FloorToInt(_contents * 1f) + "%"; break;
                    case 6: result = "<color=#aee571>▲</color> Acquisition rice " + Mathf.FloorToInt(_contents * 1f) + "%"; break;
                }
            }



            Uniform_LV[i].text = "Lv. "+ PlayerPrefsManager.GetInstance().uniformInfo[i].Uniform_LV.ToString();
            Uniform_Price[i].text = UserWallet.GetInstance().SeetheNatural(PlayerPrefsManager.GetInstance().uniformInfo[i].Uniform_Price);
            Uniform_Desc[i].text = result;

            // 쌀밥 체크후 회색 여부 판단
            Uniform_SSalCheak(i);

            /// 해당 구매 기록이 존재하면 왼쪽 버튼 처리
            if (sDataList[i] == "1")
            {
                buyBtn[i].SetActive(false); // 구매 텍스트 숨겨주고
                //
                unEquipBtn[i].SetActive(false); //  장착가능 노란 [버튼] 활성화.
                /// 번역
                if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                    equipBtn[i].GetComponentInChildren<Text>().text = "장착"; // 장착가능 노란 [버튼] 활성화.
                else
                    equipBtn[i].GetComponentInChildren<Text>().text = "Mount"; // 장착가능 노란 [버튼] 활성화.
                upgradeBtn[i].transform.GetChild(2).gameObject.SetActive(true); // 쌀밥 표기 켜줌

            }


            

            // 레벨 확인해서 MAX 뚜껑 닫아줌
            if (PlayerPrefsManager.GetInstance().uniformInfo[i].Uniform_LV >= 9999)
            {
                maxUpgradeBtn[i].SetActive(true);
            }

        }

        // 구매된거 확인
        Uniform_PurchaceRefresh();

    }


    /// <summary>
    /// 펭수 능력치 갱신
    /// </summary>
    public void Skill_Refresh()
    {
        string result = "";
        float _contents = 0;

        for (int i = 0; i < 7; i++)
        {
            _contents = PlayerPrefsManager.GetInstance().uniformInfo[i].Skill_LV;

            /// 번역
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
            {
                switch (i)
                {
                    case 0: result = "공격력 " + Mathf.FloorToInt(_contents * 1f) + "% 증가"; break;
                    case 1: result = "체력 " + Mathf.FloorToInt(_contents * 1f) + "% 증가"; break;
                    case 2: result = "체력 회복 " + Mathf.FloorToInt(_contents * 1f) + "% 증가"; break;
                    case 3: result = "골드 획득량 " + (_contents * 0.5d).ToString("f1") + "% 증가"; break;
                    case 4: result = "오프라인 보상 " + Mathf.FloorToInt(_contents * 1f) + "% 증가"; break;
                    case 5: result = "국밥 획득량 " + (_contents * 0.5d).ToString("f1") + "% 증가"; break;
                    case 6: result = "쌀밥 획득량 " + (_contents * 0.5d).ToString("f1") + "% 증가"; break;

                }
            }
            else
            {
                switch (i)
                {
                    case 0: result = "<color=#aee571>▲</color> Attack power " + Mathf.FloorToInt(_contents * 1f) + "%"; break;
                    case 1: result = "<color=#aee571>▲</color> Health " + Mathf.FloorToInt(_contents * 1f) + "%"; break;
                    case 2: result = "<color=#aee571>▲</color> Health resilience " + Mathf.FloorToInt(_contents * 1f) + "%"; break;
                    case 3: result = "<color=#aee571>▲</color> Acquisition gold " + (_contents * 0.5d).ToString("f1") + "%"; break;
                    case 4: result = "<color=#aee571>▲</color> Offline rewards " + Mathf.FloorToInt(_contents * 1f) + "%"; break;
                    case 5: result = "<color=#aee571>▲</color> Acquisition Korean soup " + (_contents * 0.5d).ToString("f1") + "%"; break;
                    case 6: result = "<color=#aee571>▲</color> Acquisition rice " + (_contents * 0.5d).ToString("f1") + "%"; break;

                }
            }



            // 쌀밥 체크후 회색 여부 판단
            Skill_SSalCheak(i);

            SKILL_UP_LV[i].text = "Lv." + PlayerPrefsManager.GetInstance().uniformInfo[i].Skill_LV.ToString();
            SKILL_UP_Price[i].text = UserWallet.GetInstance().SeetheNatural(PlayerPrefsManager.GetInstance().uniformInfo[i].Skill_Price);
            SKILL_UP_Desc[i].text = result;


            // 레벨 확인해서 MAX 뚜껑 닫아줌
            if (PlayerPrefsManager.GetInstance().uniformInfo[i].Skill_LV >= 10000)
            {
                SKILL_UP_Max[i].SetActive(true);
            }
        }

    }







    /// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    /// FixedUpdate()
    /// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

    /// <summary>
    /// 계속 누르면 강화.
    /// </summary>
    void FixedUpdate()
    {
        if (isBtnDown1) Charactoer_Lv_UP();
        //
        if (isUniformDown1) Uniform_Upgrade(1);
        if (isUniformDown2) Uniform_Upgrade(2);
        if (isUniformDown3) Uniform_Upgrade(3);
        if (isUniformDown4) Uniform_Upgrade(4);
        if (isUniformDown5) Uniform_Upgrade(5);
        if (isUniformDown6) Uniform_Upgrade(6);
        //
        if (isSkillDown1) Skill_Upgrade(0);
        if (isSkillDown2) Skill_Upgrade(1);
        if (isSkillDown3) Skill_Upgrade(2);
        if (isSkillDown4) Skill_Upgrade(3);
        if (isSkillDown5) Skill_Upgrade(4);
        if (isSkillDown6) Skill_Upgrade(5);
        if (isSkillDown7) Skill_Upgrade(6);
        //
        if (isAccDown1) Pet_Upgrade(0);
        if (isAccDown2) Pet_Upgrade(1);
        if (isAccDown3) Pet_Upgrade(2);
        if (isAccDown4) Pet_Upgrade(3);
        if (isAccDown5) Pet_Upgrade(4);

    }


    /// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    /// 액세사리 꾹 눌러 업그레이드 구역 
    /// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

    #region 스킬 꾹 눌러 업그레이드 구역 

    bool isAccDown1;
    public void AccDown1()
    {
        if (Pyt_buyBtn[0].activeSelf) return;
        Invoke("InvoAcc1", 0.3f);
    }
    void InvoAcc1()
    {
        isAccDown1 = true;
    }
    public void AccUP1()
    {
        CancelInvoke("InvoAcc1");
        isAccDown1 = false;
    }
    bool isAccDown2;
    public void AccDown2()
    {
        if (Pyt_buyBtn[1].activeSelf) return;
        Invoke("InvoAcc2", 0.3f);
    }
    void InvoAcc2()
    {
        isAccDown2 = true;
    }
    public void AccUP2()
    {
        CancelInvoke("InvoAcc2");
        isAccDown2 = false;
    }
    bool isAccDown3;
    public void AccDown3()
    {
        if (Pyt_buyBtn[2].activeSelf) return;
        Invoke("InvoAcc3", 0.3f);
    }
    void InvoAcc3()
    {
        isAccDown3 = true;
    }
    public void AccUP3()
    {
        CancelInvoke("InvoAcc3");
        isAccDown3 = false;
    }
    bool isAccDown4;
    public void AccDown4()
    {
        if (Pyt_buyBtn[3].activeSelf) return;
        Invoke("InvoAcc4", 0.3f);
    }
    void InvoAcc4()
    {
        isAccDown4 = true;
    }
    public void AccUP4()
    {
        CancelInvoke("InvoAcc4");
        isAccDown4 = false;
    }

    bool isAccDown5;
    public void AccDown5()
    {
        if (Pyt_buyBtn[4].activeSelf) return;
        Invoke("InvoAcc5", 0.3f);
    }
    void InvoAcc5()
    {
        isAccDown5 = true;
    }
    public void AccUP5()
    {
        CancelInvoke("InvoAcc5");
        isAccDown5 = false;
    }


    #endregion

    /// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    /// 스킬 꾹 눌러 업그레이드 구역 
    /// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

    #region 스킬 꾹 눌러 업그레이드 구역 

    bool isSkillDown1;
    public void SkillDown1()
    {
        Invoke("InvoSkill1", 0.3f);
    }
    void InvoSkill1()
    {
        isSkillDown1 = true;
    }
    public void SkillUP1()
    {
        CancelInvoke("InvoSkill1");
        isSkillDown1 = false;
    }
    bool isSkillDown2;
    public void SkillDown2()
    {
        Invoke("InvoSkill2", 0.3f);
    }
    void InvoSkill2()
    {
        isSkillDown2 = true;
    }
    public void SkillUP2()
    {
        CancelInvoke("InvoSkill2");
        isSkillDown2 = false;
    }
    bool isSkillDown3;
    public void SkillDown3()
    {
        Invoke("InvoSkill3", 0.3f);
    }
    void InvoSkill3()
    {
        isSkillDown3 = true;
    }
    public void SkillUP3()
    {
        CancelInvoke("InvoSkill3");
        isSkillDown3 = false;
    }
    bool isSkillDown4;
    public void SkillDown4()
    {
        Invoke("InvoSkill4", 0.3f);
    }
    void InvoSkill4()
    {
        isSkillDown4 = true;
    }
    public void SkillUP4()
    {
        CancelInvoke("InvoSkill4");
        isSkillDown4 = false;
    }
    bool isSkillDown5;
    public void SkillDown5()
    {
        Invoke("InvoSkill5", 0.3f);
    }
    void InvoSkill5()
    {
        isSkillDown5 = true;
    }
    public void SkillUP5()
    {
        CancelInvoke("InvoSkill5");
        isSkillDown5 = false;
    }
    bool isSkillDown6;
    public void SkillDown6()
    {
        Invoke("InvoSkill6", 0.3f);
    }
    void InvoSkill6()
    {
        isSkillDown6 = true;
    }
    public void SkillUP6()
    {
        CancelInvoke("InvoSkill6");
        isSkillDown6 = false;
    }
    bool isSkillDown7;
    public void SkillDown7()
    {
        Invoke("InvoSkill7", 0.3f);
    }
    void InvoSkill7()
    {
        isSkillDown7 = true;
    }
    public void SkillUP7()
    {
        CancelInvoke("InvoSkill7");
        isSkillDown7 = false;
    }

    #endregion


    /// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    /// 유니폼 꾹 눌러 업그레이드 구역 Uniform_Upgrade(_index);
    /// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

    #region 유니폼 꾹 눌러 업그레이드 구역 Uniform_Upgrade(_index);

    bool isUniformDown1;
    public void UniformDown1()
    {
        Invoke("InvoUniform1", 0.3f);
    }
    void InvoUniform1()
    {
        isUniformDown1 = true;
    }
    public void UniformUP1()
    {
        CancelInvoke("InvoUniform1");
        isUniformDown1 = false;
    }

    bool isUniformDown2;
    public void UniformDown2()
    {
        Invoke("InvoUniform2", 0.3f);
    }
    void InvoUniform2()
    {
        isUniformDown2 = true;
    }
    public void UniformUP2()
    {
        CancelInvoke("InvoUniform2");
        isUniformDown2 = false;
    }
    bool isUniformDown3;
    public void UniformDown3()
    {
        Invoke("InvoUniform3", 0.3f);
    }
    void InvoUniform3()
    {
        isUniformDown3 = true;
    }
    public void UniformUP3()
    {
        CancelInvoke("InvoUniform3");
        isUniformDown3 = false;
    }
    bool isUniformDown4;
    public void UniformDown4()
    {
        Invoke("InvoUniform4", 0.3f);
    }
    void InvoUniform4()
    {
        isUniformDown4 = true;
    }
    public void UniformUP4()
    {
        CancelInvoke("InvoUniform4");
        isUniformDown4 = false;
    }
    bool isUniformDown5;
    public void UniformDown5()
    {
        Invoke("InvoUniform5", 0.3f);
    }
    void InvoUniform5()
    {
        isUniformDown5 = true;
    }
    public void UniformUP5()
    {
        CancelInvoke("InvoUniform5");
        isUniformDown5 = false;
    }
    bool isUniformDown6;
    public void UniformDown6()
    {
        Invoke("InvoUniform6", 0.3f);
    }
    void InvoUniform6()
    {
        isUniformDown6 = true;
    }
    public void UniformUP6()
    {
        CancelInvoke("InvoUniform6");
        isUniformDown6 = false;
    }

    #endregion

    /// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    /// 캐릭터 정보 구역 Uniform_Upgrade(_index);
    /// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>


    bool isBtnDown1;
    public void BtnDown1()
    {
        Invoke("InvoDown1", 0.3f);
    }
    void InvoDown1()
    {
        isBtnDown1 = true;
    }
    public void BtnUP1()
    {
        CancelInvoke("InvoDown1");
        isBtnDown1 = false;
    }






    double doublePrice;
    /// <summary>
    /// 캐릭터 / 스킬 / 
    /// </summary>
    public void Characther_UP_Update()
    {
        CharaLv = PlayerPrefsManager.GetInstance().Chara_Lv;

        if (CharaLv >= 10000) CharaLv = 10000;

        POWER_UP_LV.text = "Lv. " + CharaLv;

        /// 골드 업그레이드 비용 감소.
        //doublePrice = CharaLv * (CharaLv + 1) * 100d;
        doublePrice = CharaLv > 1 ? 100d * Math.Pow(1.07d, CharaLv) : 100d;
        doublePrice = (doublePrice * (1.0d - PlayerPrefsManager.GetInstance().Arti_GoldUpgrade * 0.001d));

        POWER_UP_Price.text = UserWallet.GetInstance().SeetheNatural(doublePrice);



        //-------------------------------------------------------------------------------------------------------//
        //

        if(CharaLv > 1)
        {
            Chara_Attack_UP = ((CharaLv - 1) * 5f).ToString("f0");
            Chara_HP_UP = ((CharaLv - 1) * 50f).ToString("f0");
            Chara_Recov_UP = ((CharaLv - 1) * 25f).ToString("f0");
            //

            /// 캐릭터 레벨 올리면 세개가 올라감
            PlayerPrefs.SetString("Chara_Attack_UP", Chara_Attack_UP);
            PlayerPrefs.SetString("Chara_HP_UP", Chara_HP_UP);
            PlayerPrefs.SetString("Chara_Recov_UP", Chara_Recov_UP);
            /// 캐릭터 방어력 스탯 값
            PlayerPrefs.SetFloat("Chara_Defence_UP", (CharaLv - 1) * 5f);
            PlayerPrefs.Save();
        }


        //-------------------------------------------------------------------------------------------------------//
        //


        Power_UP_Cheak();
        groggyManager.PowerUP_Init();

        if (coru == null)
            coru = StartCoroutine(Tsukuyomi());
    }

    Coroutine coru;
    WaitForSeconds GC_05_SEC = new WaitForSeconds(0.5f);

    /// <summary>
    /// 회색으로 덮기 판단 0.5초 마다
    /// </summary>
    /// <returns></returns>
    IEnumerator Tsukuyomi()
    {
        yield return null;

        while (true)
        {
            yield return GC_05_SEC;

            if (charCancas.activeSelf)
            {
                Power_UP_Cheak();
            }
        }
    }

    string Power_UPgoldPass;
    /// <summary>
    /// 돈이 안되면 회색으로.
    /// </summary>
    bool Power_UP_Cheak()
    {
        // 현재 공격력 레벨
        if (CharaLv >= 10000)
        {
            POWER_UP_Max.SetActive(true);
            return false;
        }
        // 체크 통과했으면 골드 체크 해줘라.
        Power_UPgoldPass = dts.SubStringDouble(PlayerPrefsManager.GetInstance().gold, doublePrice);

        // 골드 없으면 false
        if (Power_UPgoldPass == "-1")
        {
            POWER_UP_Gray.SetActive(true);
            return false;
        }
        else // 구매 가능하면 트루
        {
            POWER_UP_Gray.SetActive(false);
            return true;
        }

    }
    public void Charactoer_Lv_UP()
    {
        // 돈 없으면 꺼져
        if (!Power_UP_Cheak()) return;

        //골드 감소 처리
        PlayerPrefsManager.GetInstance().gold = Power_UPgoldPass;
        UserWallet.GetInstance().ShowUserGold();
        // 레벨 상승
        PlayerPrefsManager.GetInstance().Chara_Lv++;
        tmm.ExUpdateMission(6); /// 미션 업데이트
        tmm.ExUpdateMission(9); /// 미션 업데이트
        tmm.ExUpdateMission(25); /// 미션 업데이트
        tmm.ExUpdateMission(36); /// 미션 업데이트
        tmm.ExUpdateMission(46); /// 미션 업데이트
        tmm.ExUpdateMission(58); /// 미션 업데이트
        tmm.ExUpdateMission(67); /// 미션 업데이트
        tmm.ExUpdateMission(77); /// 미션 업데이트
        tmm.ExUpdateMission(85); /// 미션 업데이트

        // 새로고침
        Characther_UP_Update();

    }


}
