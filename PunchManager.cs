﻿using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PunchManager : MonoBehaviour
{
    DoubleToStringNum dts = new DoubleToStringNum();

    [Header("-만렙 찍으면 다이아로 산다 스프라이트")]
    public Sprite DiaImg;
    public Sprite GookBapImg;




    [Header("-클릭 발싸 펀치 수정할거")]
    public TapToSpawnLimit tapToSpawnLimit;

    [Header("-훈련장비 부모 그리드")]
    public Transform PunchGrid;

    // 무기 배열로 만들어서 관리 혀라
    Text[] POWER_UP_LV;                 // 해당 무기 레벨 
    Text[] POWER_UP_TEXT;               // 공격력 50% 증가
    Text[] POWER_UP_Price;              // 우유 가격

    // 데이터 임시 저장
    int thisWeaponLevel;
    string thisWeaponCost;
    float thisWeaponEffect;
    bool isUnlock;
    string[] thisDiaBuyWeapArray;



    #region 0601 무기 누적 구매 세이브/로드


    public void DiaBuyWeaponListSave(string result)
    {
        PlayerPrefs.SetString("diaBuyWeaponList", result);
        PlayerPrefs.Save();
    }

    string[] DiaBuyWeaponListLoad()
    {
        string _Data = PlayerPrefs.GetString("diaBuyWeaponList", "525*");
        string[] sDataList = (_Data).Split('*');

        /// diaBuyWeaponList 가 제일 처음 생성될때
        if (sDataList[0] == "525")
        {
            sDataList = new string[100];
            for (int i = 0; i < 100; i++)
            {
                sDataList[i] = "0*";
            }
        }

        return sDataList;
    }

    #endregion


    /// <summary>
    /// 게임 시작할때 펀치 세팅 초기화
    /// </summary>
    public void PunchInit()
    {
        string tmpDiaBuyWeap = "";
        string[] tmpDiaBuyWeapArray = DiaBuyWeaponListLoad();

        //
        int ccnt = PunchGrid.childCount;

        POWER_UP_LV = new Text[ccnt];
        POWER_UP_TEXT = new Text[ccnt];
        POWER_UP_Price = new Text[ccnt];

        for (int i = 0; i < ccnt; i++)
        {
            // 모든 장착 가능 회색 커버 꺼줌.
            PunchGrid.GetChild(i).GetChild(4).GetChild(0).GetChild(1).gameObject.SetActive(false);
            // 리스트 긁기
            GetThisWeaponInfo(i);
            // 각 항목 수정
            SetPunchLv(i, thisWeaponLevel);
            SetPunchText(i, thisWeaponEffect);
            SetPunchPrice(i, thisWeaponCost);


            if (thisWeaponLevel == 100)
            {
                ///이미 다이아몬드로 샀는가?
                if (tmpDiaBuyWeapArray[i] == "1")
                {
                    /// 능력치 총합 더 해주고.
                    ///TODO : 만렙 처리 회색 커버
                    PunchGrid.GetChild(i).GetChild(4).GetChild(1).GetChild(1).gameObject.SetActive(true); // Cover_Btn
                    PunchGrid.GetChild(i).GetChild(4).GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().sprite = DiaImg;
                    PunchGrid.GetChild(i).GetChild(4).GetChild(1).GetComponentInChildren<Text>().text = "MAX";
                    // 구매 완료
                    tmpDiaBuyWeap += "1*";
                }
                else  /// 아직 만렙만 찍은 상태 - 다이아몬드 구입창 보여줘라.
                {
                    PunchGrid.GetChild(i).GetChild(4).GetChild(1).GetChild(1).gameObject.SetActive(false); // Cover_Btn
                    PunchGrid.GetChild(i).GetChild(4).GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().sprite = DiaImg;
                    PunchGrid.GetChild(i).GetChild(4).GetChild(1).GetComponentInChildren<Text>().text = ((i +1) * 500).ToString();

                    // 미구매
                    tmpDiaBuyWeap += "0*";
                }
            }
            else
            {
                // 미구매
                tmpDiaBuyWeap += "0*";
            }

            /// 구매한 펀치라면 언락 이미지 표기
            UnlockNewPunch(i);
        }


        // 현재 장착 회색 커버 활성화.
        PunchGrid.GetChild(PlayerPrefsManager.GetInstance().PunchIndex).GetChild(4).GetChild(0).GetChild(1).gameObject.SetActive(true);

        /// 수치 변화 저장
        PlayerPrefsManager.GetInstance().SaveWeaponInfo();

        /// 무기 데이터 다이아몬드 구매 여부 저장
        DiaBuyWeaponListSave(tmpDiaBuyWeap);
        thisDiaBuyWeapArray = DiaBuyWeaponListLoad();

    }

    /// <summary>
    /// 펀치 정보 수정하려면
    /// </summary>
    /// <param name="_index"></param>
    /// <param name="_contents"></param>
    void SetPunchLv(int _index, int _contents)
    {
        string result = "Lv. " + _contents;
        PunchGrid.GetChild(_index).GetChild(2).GetComponent<Text>().text = result;
        PlayerPrefsManager.GetInstance().weaponInfo[_index].weaponLevel = _contents;
    }

    void SetPunchText(int _index, float _contents)
    {
        string result = "맷집 " + _contents + "% 증가";
        PunchGrid.GetChild(_index).GetChild(3).GetComponent<Text>().text = result;
        PlayerPrefsManager.GetInstance().weaponInfo[_index].weaponEffect = _contents;
    }

    void SetPunchPrice(int _index, string _contents)
    {
        var value = dts.PanByulGi(_contents);
        string result = UserWallet.GetInstance().SeetheNatural(value);

        //Debug.LogWarning("SeetheNatural" + result);

        PunchGrid.GetChild(_index).GetChild(4).GetChild(1).GetComponentInChildren<Text>().text = result;
        PlayerPrefsManager.GetInstance().weaponInfo[_index].weaponCost = _contents;
    }

    /// <summary>
    /// 초기화 할 때만 호출 
    /// 1. 펀치 이름 보이게
    /// 2. 펀치 덮고있던 이미지 제거
    /// </summary>
    void UnlockNewPunch(int _index)
    {
        // 리스트 언락 트리거 On이면
        if (isUnlock)
        {
            // 가리개 제거
            PunchGrid.GetChild(_index).GetChild(5).gameObject.SetActive(false);
            // 펀치 이름 넣어줌.
            string result = PunchNames[_index];
            PunchGrid.GetChild(_index).GetChild(1).GetComponent<Text>().text = result;
            // 우유 갯수 충분하면 버튼 회색커버 꺼줌
            if (MilkPass(thisWeaponCost) && thisWeaponLevel != 100) PunchGrid.GetChild(_index).GetChild(4).GetChild(1).GetChild(1).gameObject.SetActive(false);
            // 아이콘 색상 
            PunchGrid.GetChild(_index).GetChild(0).GetChild(0).GetComponent<Image>().color = Color.white;
        }
    }

    /// <summary>
    /// 버티기 성공하면 다음 펀치 해금 됨.
    /// </summary>
    public void UnlockNextPunch()
    { 
        int p_among = PlayerPrefsManager.GetInstance().punchAmont;

        int _index = 0;
        // 최근 펀치 어디까지 해금 됐니? 탐색
        for (int i = 0; i< p_among; i++)
        {
            if (!PlayerPrefsManager.GetInstance().weaponInfo[i].isUnlock)
            {
                isUnlock = true;
                _index = i;
                PlayerPrefsManager.GetInstance().weaponInfo[i].isUnlock = true;
                PlayerPrefsManager.GetInstance().SaveWeaponInfo();
                break;
            }
        }

        UnlockNewPunch(_index);
    }

    /// <summary>
    /// 펀치 장착 바꿔줌
    /// </summary>
    void ChangePunch(int _index)
    {
        // ppm 으로 저장도 하고
        PlayerPrefsManager.GetInstance().PunchIndex = _index;
        // 불렛 종류 바꾸고
        tapToSpawnLimit.PunchIndexUpdate(_index);
    }

    /// <summary>
    /// 1. 트레이닝 - 훈련 강화 탭 누르면 국밥 여부 확인
    /// 2. Left 강화 아이콘 눌러도 호출
    /// </summary>
    public void PunchTapClicked()
    {
        int p_among = PlayerPrefsManager.GetInstance().punchAmont;
        // 최근 펀치 어디까지 해금 됐니? 탐색
        for (int i = 0; i < p_among; i++)
        {
            if (PlayerPrefsManager.GetInstance().weaponInfo[i].isUnlock)
            {
                if (PlayerPrefsManager.GetInstance().weaponInfo[i].weaponLevel != 100)
                {
                    // 저장된 해당 펀치 리스트 가져와서 뿌려주고
                    /// thislevel 넣어줌
                    GetThisWeaponInfo(i);
                    // 우유 갯수 충분하면 버튼 회색커버 꺼줌(만렙 아니어야함)
                    if (MilkPass(thisWeaponCost))
                    {
                        PunchGrid.GetChild(i).GetChild(4).GetChild(1).GetChild(1).gameObject.SetActive(false);

                        if (thisWeaponLevel == 100)
                        {
                            PunchGrid.GetChild(i).GetChild(4).GetChild(1).GetChild(1).gameObject.SetActive(true);

                            Debug.LogWarning(i + " 펀치 " + thisWeaponLevel + " 렙.");
                        }
                    }
                    else
                    {
                        PunchGrid.GetChild(i).GetChild(4).GetChild(1).GetChild(1).gameObject.SetActive(true);
                    }

                    // 아이콘 색상 
                    PunchGrid.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().color = Color.white;
                }
            }
        }
    }

    /// <summary>
    /// 장비 장착하기 버튼
    /// </summary>
    public void EquipWeapon()
    {
        UserWallet.GetInstance().ShowAllMoney();

        // 클릭한 지점 이름 얻어오기
        string nameIndex = EventSystem.current.currentSelectedGameObject.transform.parent.parent.name;
        string strTmp = Regex.Replace(nameIndex, @"\D", "");
        int p_index = int.Parse(strTmp) - 1;

        GetThisWeaponInfo(p_index);
        // 0 렙이면 장착하지말고 리턴
        if (thisWeaponLevel == 0 && p_index !=0)
        {
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("레벨이 0인 장비는 장착할 수 없습니다.");
            return;
        }




        // 이전 장착 회색 커버 비활성화
        PunchGrid.GetChild(PlayerPrefsManager.GetInstance().PunchIndex).GetChild(4).GetChild(0).GetChild(1).gameObject.SetActive(false);
        // 현재 장착 회색 커버 활성화.
        PunchGrid.GetChild(p_index).GetChild(4).GetChild(0).GetChild(1).gameObject.SetActive(true);

        ChangePunch(p_index);

        // 회색커버라면 리턴
        //if (PunchGrid.GetChild(p_index).GetChild(4).GetChild(0).GetChild(1).gameObject.activeSelf) return;
    }


    /// <summary>
    /// 펀치 강화 버튼 누르면 호출
    /// </summary>
    public void PunchUpgrade()
    {
        // 클릭한 지점 이름 얻어오기
        string nameIndex = EventSystem.current.currentSelectedGameObject.transform.parent.parent.name;
        string strTmp = Regex.Replace(nameIndex, @"\D", "");
        int p_index = int.Parse(strTmp) -1;
        // 밑으로 분리.
        ClickedPunchUPgra(p_index);

    }



    /// <summary>
    /// 훈련 장비 업그레이드 버튼에 붙여
    /// </summary>
    /// <param name="p_index"></param>
    public void ClickedPunchUPgra(int p_index)
    {
        // 저장된 해당 펀치 리스트 가져와서 뿌려주고
        GetThisWeaponInfo(p_index);


        // 회색 Cover Img로 덮여 있으면 리턴 -> 업그레이드 재화 소모없음 / 레벨 증가 없음.
        if (PunchGrid.GetChild(p_index).GetChild(4).GetChild(1).GetChild(1).gameObject.activeSelf)
        {
            return;
        }

        /// 다이아 구매 || 국밥 구매 판별
        if (thisWeaponLevel >= 100 && thisDiaBuyWeapArray[p_index] == "0")
        {
            thisWeaponLevel = 100;

            float dia = PlayerPrefs.GetFloat("dDiamond");
            float diaPrice = ((p_index + 1) * 500);

            //다이아 체크
            if (dia - diaPrice < 0)
            {
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("보유 다이아가 부족합니다.");
                return;
            }

            // 다이아 체크 통과후 소모
            PlayerPrefs.SetFloat("dDiamond", dia - diaPrice);
            UserWallet.GetInstance().ShowUserDia();

            //구입 했다 도장 찍기
            //thisDiaBuyWeapArray[p_index] = "1";

            string tmpDiaBuyWeap = "";
            for (int i = 0; i < PunchGrid.childCount; i++)
            {
                if (i == p_index || thisDiaBuyWeapArray[i] == "1")
                {
                    tmpDiaBuyWeap += "1*";
                }
                else
                {
                    tmpDiaBuyWeap += "0*";
                }
            }

            /// 무기 데이터 다이아몬드 구매 여부 저장
            DiaBuyWeaponListSave(tmpDiaBuyWeap);
            thisDiaBuyWeapArray = DiaBuyWeaponListLoad();

            ///TODO : 만렙 처리 회색 커버
            PunchGrid.GetChild(p_index).GetChild(4).GetChild(1).GetChild(1).gameObject.SetActive(true); // Cover_Btn
            PunchGrid.GetChild(p_index).GetChild(4).GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().sprite = DiaImg;
            PunchGrid.GetChild(p_index).GetChild(4).GetChild(1).GetComponentInChildren<Text>().text = "MAX";
            //
            var dfsd = PlayerPrefsManager.GetInstance().weaponInfo[p_index].weaponEffect;
            PlayerPrefsManager.GetInstance().Mattzip_Dia_Weap += dfsd;

            return;
        }




        // 해당 코스트 넣어서 통과 못하면 리턴
        if (!MilkPass(thisWeaponCost)) 
            return;

        // 정상적 국밥 소모
        PlayerPrefsManager.GetInstance().gupbap = result;
        UserWallet.GetInstance().ShowUserMilk();
        //레벨업
        thisWeaponLevel++;
        // 근데 만렙이다?
        if (thisWeaponLevel >= 100)
        {
            thisWeaponLevel = 100;
        }

        // 정상적으로 소모했다면 훈련도구 강화
        //float weaponATK = float.Parse(PlayerPrefsManager.GetInstance().weaponColl[p_index, thisWeaponLevel - 1]);
        /// 레벨 1일때 초기값 예외 처리
        float weaponATK = (thisWeaponLevel * (0.5f * (p_index + 1)));


        // 훈련도구 가격
        //string tmpPrice = PlayerPrefsManager.GetInstance().shopColl[p_index, thisWeaponLevel - 1];
        string tmpPrice;
        ///// 레벨 1일때 초기값 예외 처리
        //if (thisWeaponLevel == 1)
        //{
        //    tmpPrice = (thisWeaponLevel * 10).ToString();
        //}
        //else
        //{
        //    /// 첫번재 무기라면 예외처리
        //    if (p_index == 0)
        //    {
        //        tmpPrice = (thisWeaponLevel + 9).ToString();
        //    }
        //    else
        //    {
        //        tmpPrice = (thisWeaponLevel * (5 * p_index)) + (10 + (5 * p_index)).ToString();
        //    }

        //}

        /// 첫번재 무기라면 예외처리
        if (p_index == 0)
            {
                tmpPrice = (thisWeaponLevel + 10).ToString();
            }
            else
            {
                tmpPrice = (((thisWeaponLevel + 1) * (5 * p_index)) + (10 + (5 * p_index))).ToString();
            }

        
        Debug.LogWarning("tmpPrice : " + tmpPrice);

        string weaponPrice = dts.fDoubleToStringNumber(tmpPrice);

        //
        if (MilkPass(weaponPrice))
        {
            PunchGrid.GetChild(p_index).GetChild(4).GetChild(1).GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            PunchGrid.GetChild(p_index).GetChild(4).GetChild(1).GetChild(1).gameObject.SetActive(true);
        }

        // 각 항목 수정
        SetPunchLv(p_index, thisWeaponLevel);
        SetPunchText(p_index, weaponATK);
        SetPunchPrice(p_index, tmpPrice);

        if (thisWeaponLevel == 100)
        {
            PunchGrid.GetChild(p_index).GetChild(4).GetChild(1).GetChild(1).gameObject.SetActive(false); // Cover_Btn
            PunchGrid.GetChild(p_index).GetChild(4).GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().sprite = DiaImg;
            PunchGrid.GetChild(p_index).GetChild(4).GetChild(1).GetComponentInChildren<Text>().text = ((p_index + 1) * 500).ToString();
        }


        /// 수치 변화 저장
        PlayerPrefsManager.GetInstance().weaponInfo[p_index].weaponLevel = thisWeaponLevel;
        PlayerPrefsManager.GetInstance().weaponInfo[p_index].weaponCost = weaponPrice;
        PlayerPrefsManager.GetInstance().weaponInfo[p_index].weaponEffect = weaponATK;
        //퀘스트
        PlayerPrefsManager.GetInstance().questInfo[0].daily_Punch++;

        if (PlayerPrefsManager.GetInstance().questInfo[0].All_Punch < 1000)
        {
            PlayerPrefsManager.GetInstance().questInfo[0].All_Punch++;
        }


        PlayerPrefsManager.GetInstance().SaveWeaponInfo();

        PunchTapClicked();

        UserWallet.GetInstance().ShowAllMoney();
    }

    /// <summary>
    /// 강화 페이지 열때 같이 초기화 해줌. 디스레벨/디스코스트/디스이펙트
    /// </summary>
    /// <param name="p_index"></param>
    void GetThisWeaponInfo(int p_index)
    {         
        // 저장된 해당 펀치 리스트 가져와서 뿌려주고
        var tmpList = PlayerPrefsManager.GetInstance().weaponInfo[p_index];

        thisWeaponLevel = tmpList.weaponLevel;
        thisWeaponCost = tmpList.weaponCost;
        thisWeaponEffect = tmpList.weaponEffect;
        //
        isUnlock = tmpList.isUnlock;
    }

    string result;
    /// <summary>
    /// 우유 소모 로직
    /// </summary>
    /// <returns></returns>
    bool MilkPass(string _somo)
    {
        var mlikAmount = PlayerPrefsManager.GetInstance().gupbap;
        result = dts.SubStringDouble(mlikAmount, _somo);

        if (result != "-1")
        {
            return true;
        }
        else
        {
            return false;
        }

    }
        
    // 펀치 목록
    string[] PunchNames = 
    { "맨주먹",
"물주먹",
"돌주먹",
"불주먹",
"막대사탕",
"당근",
"양송이버섯",
"수박바",
"소시지",
"축구공",
"농구공",
"당구공",
"럭비공",
"셔틀콕",
"숟가락",
"뒤집개",
"호두",
"밀대",
"도마",
"프라이팬",
"장미꽃",
"안전꼬깔",
"볼트",
"너트",
"전구",
"벽돌",
"책",
"대나무",
"뿅망치",
"양동이",
"망치",
"몽키스패너",
"전동드릴",
"확성기",
"캐리어",
"서류가방",
"연료통",
"맥주잔",
"보드카",
"꼬냑",
"술통",
"안전모",
"드럼통",
"텔레비전",
"하이힐",
"워커",
"핸들",
"타이어",
"헬멧",
"참치",
"할로윈 호박",
"마이크",
"파레트",
"통기타",
"일렉기타",
"책상",
"피아노",
"경고표지판",
"의자",
"삽" ,
"리어카",
"곡괭이",
"돈" ,
"활" ,
"권총",
"기관단총",
"샷건",
"저격소총",
"바주카포",
"폭탄",
"탄알집",
"개머리판",
"홀로스코프",
"보통탄",
"산탄총알",
"연막탄",
"수류탄",
"화염병",
"다이너마이트",
"폭격탄",
"킥보드",
"스케이트보드",
"외발자전거",
"자전거",
"스쿠터",
"오토바이",
"택시",
"트럭",
"고속열차",
"포크레인",
"돛단배",
"요트",
"크루즈",
"군함",
"잠수함",
"열기구",
"여객기",
"헬리콥터",
"전투기",
"스텔스"
    };




}
