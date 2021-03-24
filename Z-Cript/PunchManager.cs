using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PunchManager : MonoBehaviour
{
    DoubleToStringNum dts = new DoubleToStringNum();

    public Transform InfinityContent;
    [Header("-펀치 이미지 100개")]
    public Sprite[] punchImgs;
    [Header("-다이아 영구 해제 팝업 오브젝트")]
    public GameObject SomeThingPop;
    [Header("-만렙 찍으면 다이아로 산다 스프라이트")]
    public Sprite DiaImg;
    public Sprite GookBapImg;

    [Header("-클릭 발싸 펀치 수정할거")]
    public TapToSpawnLimit tapToSpawnLimit;

    // 데이터 임시 저장
    public string[] thisDiaBuyWeapArray;



    #region 0601 무기 누적 구매 세이브/로드

    /// <summary>
    /// 펀치 영구 구매 배열 반환
    /// </summary>
    /// <returns></returns>
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
        /// 총 펀치 갯수 현재 100 개
        int ccnt = PlayerPrefsManager.GetInstance().punchAmont;
        /// 다이아로 펀치 해금 초기화.
        PlayerPrefsManager.GetInstance().Mattzip_Dia_Weap = 0;

        for (int i = 0; i < ccnt; i++)
        {
            if (PlayerPrefsManager.GetInstance().weaponInfo[i].weaponLevel == 100)
            {
                ///이미 다이아몬드로 샀는가?
                if (tmpDiaBuyWeapArray[i] == "1")
                {
                    /// 능력치 총합 더 해주고.
                    // 구매 완료
                    PlayerPrefsManager.GetInstance().Mattzip_Dia_Weap += 10f * (i + 1);
                    tmpDiaBuyWeap += "1*";
                }
                else  /// 아직 만렙만 찍은 상태 - 다이아몬드 구입창 보여줘라.
                {
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
        /// 수치 변화 저장
        PlayerPrefsManager.GetInstance().SaveWeaponInfo();

        /// 무기 데이터 다이아몬드 구매 여부 저장
        PlayerPrefs.SetString("diaBuyWeaponList", tmpDiaBuyWeap);
        PlayerPrefs.Save();
        thisDiaBuyWeapArray = DiaBuyWeaponListLoad();

        /// 첫 펀치 불러오기
        tapToSpawnLimit.PunchIndexUpdate(PlayerPrefsManager.GetInstance().PunchIndex);
        RefreshEuipGrayBtn();
    }

    /// <summary>
    /// 모든 장착 버튼 노란색으로 바꾸기
    /// </summary>
    internal void BoxInfoUpdate()
    {
        for (int i = 1; i < InfinityContent.childCount; i++)
        {
            InfinityContent.GetChild(i).
                GetComponent<PunchItem>().BoxInfoUpdate
                (int.Parse(InfinityContent.GetChild(i).name));
        }
    }

    /// <summary>
    /// 모든 장착 버튼 노란색으로 바꾸기
    /// </summary>
    internal void RefreshEuipGrayBtn()
    {
        for (int i = 1; i < InfinityContent.childCount; i++)
        {
            InfinityContent.GetChild(i).GetComponent<PunchItem>().SetAllEpuipBtnToGray();
        }
    }

    /// <summary>
    /// 초기화 할 때만 호출 
    /// 1. 펀치 이름 보이게
    /// 2. 펀치 덮고있던 이미지 제거
    /// </summary>
    internal void UnlockNewPunch(int _index)
    {
        for (int i = 1; i < InfinityContent.childCount; i++)
        {
            InfinityContent.GetChild(i).GetComponent<PunchItem>().SetAllGrayCover(_index);
        }
    }

    /// <summary>
    /// 버티기 성공하면 다음 펀치 해금 됨.
    /// </summary>
    public void UnlockNextPunch()
    {
        int p_among = PlayerPrefsManager.GetInstance().punchAmont;
        // 최근 펀치 어디까지 해금 됐니? 탐색
        for (int _index = 0; _index < p_among; _index++)
        {
            if (!PlayerPrefsManager.GetInstance().weaponInfo[_index].isUnlock)
            {
                PlayerPrefsManager.GetInstance().weaponInfo[_index].isUnlock = true;
                PlayerPrefsManager.GetInstance().SaveWeaponInfo();
                /// 회색 덮개 해제
                UnlockNewPunch(_index);

                return;
            }
        }

    }

    /// <summary>
    /// 펀치 장착 바꿔줌
    /// </summary>
    internal void ChangePunch(int _index)
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
        //int p_among = PlayerPrefsManager.GetInstance().punchAmont;
        //// 최근 펀치 어디까지 해금 됐니? 탐색
        //for (int i = 0; i < p_among; i++)
        //{
        //    if (PlayerPrefsManager.GetInstance().weaponInfo[i].isUnlock)
        //    {
        //        if (PlayerPrefsManager.GetInstance().weaponInfo[i].weaponLevel != 100)
        //        {
        //            // 저장된 해당 펀치 리스트 가져와서 뿌려주고
        //            /// thislevel 넣어줌
        //            GetThisWeaponInfo(i);
        //            // 우유 갯수 충분하면 버튼 회색커버 꺼줌(만렙 아니어야함)
        //            if (GupbapPass(thisWeaponCost))
        //            {
        //                //PunchGrid.GetChild(i).GetChild(4).GetChild(1).GetChild(1).gameObject.SetActive(false);

        //                if (thisWeaponLevel == 100)
        //                {
        //                    //PunchGrid.GetChild(i).GetChild(4).GetChild(1).GetChild(1).gameObject.SetActive(true);

        //                    Debug.LogWarning(i + " 펀치 " + thisWeaponLevel + " 렙.");
        //                }
        //            }
        //            else
        //            {
        //                //PunchGrid.GetChild(i).GetChild(4).GetChild(1).GetChild(1).gameObject.SetActive(true);
        //            }

        //            // 아이콘 색상 
        //            //PunchGrid.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().color = Color.white;
        //        }
        //    }
        //}
    }

    /// <summary>
    /// 장비 장착하기 버튼
    /// </summary>
    public void EquipWeapon()
    {
        //UserWallet.GetInstance().ShowAllMoney();

        //// 클릭한 지점 이름 얻어오기
        //string nameIndex = EventSystem.current.currentSelectedGameObject.transform.parent.parent.name;
        //string strTmp = Regex.Replace(nameIndex, @"\D", "");
        //int p_index = int.Parse(strTmp) - 1;

        ////GetThisWeaponInfo(p_index);
        //// 0 렙이면 장착하지말고 리턴
        //if (thisWeaponLevel == 0 && p_index != 0)
        //{
        //    PopUpObjectManager.GetInstance().ShowWarnnigProcess("레벨이 0인 장비는 장착할 수 없습니다.");
        //    return;
        //}




        // 이전 장착 회색 커버 비활성화
        //PunchGrid.GetChild(PlayerPrefsManager.GetInstance().PunchIndex).GetChild(4).GetChild(0).GetChild(1).gameObject.SetActive(false);
        // 현재 장착 회색 커버 활성화.
        //PunchGrid.GetChild(p_index).GetChild(4).GetChild(0).GetChild(1).gameObject.SetActive(true);

        //ChangePunch(p_index);

        // 회색커버라면 리턴
        //if (PunchGrid.GetChild(p_index).GetChild(4).GetChild(0).GetChild(1).gameObject.activeSelf) return;
    }


    /// <summary>
    /// 펀치 강화 버튼 누르면 호출
    /// </summary>
    public void PunchUpgrade()
    {
        //// 클릭한 지점 이름 얻어오기
        //string nameIndex = EventSystem.current.currentSelectedGameObject.transform.parent.parent.name;
        //string strTmp = Regex.Replace(nameIndex, @"\D", "");
        //int p_index = int.Parse(strTmp) - 1;
        //// 밑으로 분리.
        //ClickedPunchUPgra(p_index);

    }



    /// <summary>
    /// 훈련 장비 업그레이드 버튼에 붙여
    /// </summary>
    /// <param name="p_index"></param>
    internal void LongClicedUpgradeBtn(int p_index)
    {
        for (int i = 1; i < InfinityContent.childCount; i++)
        {
            if (int.Parse(InfinityContent.GetChild(i).name) == p_index)
            {
                InfinityContent.GetChild(i).GetComponent<PunchItem>().ClickedUpgradeBtn();
                return;
            }

        }


        ///// 저장된 해당 펀치 리스트 가져와서 뿌려주고
        ////GetThisWeaponInfo(p_index);
        ///// 해당 펀치 회색 Cover Img로 덮여 있으면 리턴 -> 업그레이드 재화 소모없음 / 레벨 증가 없음.
        ////if (PunchGrid.GetChild(p_index).GetChild(4).GetChild(1).GetChild(1).gameObject.activeSelf)
        ////{
        ////    return;
        ////}

        ///// 다이아 구매 || 국밥 구매 판별
        //if (thisWeaponLevel >= 100 && thisDiaBuyWeapArray[p_index] == "0")
        //{
        //    ShowDiaPunchOkay(p_index);
        //    return;
        //}

        //// 해당 코스트 넣어서 통과 못하면 리턴
        //if (!GupbapPass(thisWeaponCost))
        //    return;

        //// 정상적 국밥 소모
        //PlayerPrefsManager.GetInstance().gupbap = result;
        //UserWallet.GetInstance().ShowUserMilk();
        ////레벨업
        //thisWeaponLevel++;
        //// 근데 만렙이다?
        //if (thisWeaponLevel >= 100)
        //{
        //    thisWeaponLevel = 100;
        //}
        ///// 레벨 1일때 초기값 예외 처리 (맷집 최대 증가량)
        //float weaponATK = (thisWeaponLevel * (0.1f * (p_index + 1)));
        //// 훈련도구 가격
        //string tmpPrice;

        ///// 첫번재 무기라면 예외처리
        //if (p_index == 0)
        //{
        //    tmpPrice = (thisWeaponLevel + 10).ToString();
        //}
        //else
        //{
        //    tmpPrice = (((thisWeaponLevel + 1) * (5 * p_index)) + (10 + (5 * p_index))).ToString();
        //}


        //Debug.LogWarning("tmpPrice : " + tmpPrice);

        //string weaponPrice = dts.fDoubleToStringNumber(tmpPrice);

        ////
        //if (GupbapPass(weaponPrice))
        //{
        //    //PunchGrid.GetChild(p_index).GetChild(4).GetChild(1).GetChild(1).gameObject.SetActive(false);
        //}
        //else
        //{
        //    //PunchGrid.GetChild(p_index).GetChild(4).GetChild(1).GetChild(1).gameObject.SetActive(true);
        //}

        //// 각 항목 수정

        //if (thisWeaponLevel == 100)
        //{
        //    //PunchGrid.GetChild(p_index).GetChild(4).GetChild(1).GetChild(1).gameObject.SetActive(false); // Cover_Btn
        //    //PunchGrid.GetChild(p_index).GetChild(4).GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().sprite = DiaImg;
        //    //PunchGrid.GetChild(p_index).GetChild(4).GetChild(1).GetComponentInChildren<Text>().text = ((p_index + 1) * 100).ToString();
        //}


        ///// 수치 변화 저장
        //PlayerPrefsManager.GetInstance().weaponInfo[p_index].weaponLevel = thisWeaponLevel;
        //PlayerPrefsManager.GetInstance().weaponInfo[p_index].weaponCost = weaponPrice;
        //PlayerPrefsManager.GetInstance().weaponInfo[p_index].weaponEffect = weaponATK;
        ////퀘스트
        //PlayerPrefsManager.GetInstance().questInfo[0].daily_Punch++;

        //if (PlayerPrefsManager.GetInstance().questInfo[0].All_Punch < 1000)
        //{
        //    PlayerPrefsManager.GetInstance().questInfo[0].All_Punch++;
        //}


        //PlayerPrefsManager.GetInstance().SaveWeaponInfo();

        //PunchTapClicked();

        //UserWallet.GetInstance().ShowAllMoney();
    }

    /// <summary>
    /// 펀치 레벨 100일때 팝업 띄워줌
    /// </summary>
    public void ShowDiaPunchOkay(int p_index)
    {
        SD_PunchIndex = p_index;
        // 애니메 재생
        SomeThingPop.SetActive(true);
        SomeThingPop.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        SomeThingPop.GetComponent<Animation>().Play("Roll_Incre");
    }

    int SD_PunchIndex;

    /// <summary>
    /// 외부에서 클릭
    /// 팝업 띄워주면 다이아 있을때 영구 보유
    /// </summary>
    public void SummitDiaPunch()
    {
        float dia = PlayerPrefs.GetFloat("dDiamond");
        float diaPrice = ((SD_PunchIndex + 1) * 100);

        //다이아 체크
        if (dia - diaPrice < 0)
        {
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("보유 다이아가 부족합니다.");
            return;
        }

        // 다이아 체크 통과후 소모
        PlayerPrefs.SetFloat("dDiamond", dia - diaPrice);
        UserWallet.GetInstance().ShowUserDia();

        /// 무기 데이터 다이아몬드 구매 여부 저장
        string tmpDiaBuyWeap = "";
        for (int i = 0; i < PlayerPrefsManager.GetInstance().weaponInfo.Count; i++)
        {
            if (i == SD_PunchIndex || thisDiaBuyWeapArray[i] == "1")
            {
                tmpDiaBuyWeap += "1*";
            }
            else
            {
                tmpDiaBuyWeap += "0*";
            }
        }
        PlayerPrefs.SetString("diaBuyWeaponList", tmpDiaBuyWeap);
        PlayerPrefs.Save();
        thisDiaBuyWeapArray = DiaBuyWeaponListLoad();

        /// 다이아로 구매했으면 맷집 증가 효과 더해줌 -> 한번만 적용됨
        var thisWeaponEff = PlayerPrefsManager.GetInstance().weaponInfo[SD_PunchIndex].weaponEffect;
        PlayerPrefsManager.GetInstance().Mattzip_Dia_Weap += thisWeaponEff;

        /// 업글 완료시 창 닫아줌
        SomeThingPop.SetActive(false);
        ///다이아로 구매했으면 만렙 처리 회색 커버
        LockMaxButton();
    }


    /// <summary>
    /// 다이아 구매 성공하면 MAX 버튼 활성화
    /// </summary>
    /// <param name="_index"></param>
    internal void LockMaxButton()
    {
        for (int i = 1; i < InfinityContent.childCount; i++)
        {
            InfinityContent.GetChild(i).GetComponent<PunchItem>().SetMaxPunch(SD_PunchIndex);
        }
    }


    /// <summary>
    /// 강화 페이지 열때 같이 초기화 해줌. 디스레벨/디스코스트/디스이펙트
    /// </summary>
    /// <param name="p_index"></param>
    void GetThisWeaponInfo(int p_index)
    {
    //    // 저장된 해당 펀치 리스트 가져와서 뿌려주고
    //    var tmpList = PlayerPrefsManager.GetInstance().weaponInfo[p_index];

    //    thisWeaponLevel = tmpList.weaponLevel;
    //    thisWeaponCost = tmpList.weaponCost;
    //    thisWeaponEffect = tmpList.weaponEffect;
    //    isUnlock = tmpList.isUnlock;
    }

    string result;

    /// <summary>
    /// 국밥 소모 로직
    /// </summary>
    /// <returns></returns>
    //bool GupbapPass(string _somo)
    //{
    //    var mlikAmount = PlayerPrefsManager.GetInstance().gupbap;
    //    result = dts.SubStringDouble(mlikAmount, _somo);

    //    if (result != "-1")
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    // 펀치 목록
    public string[] PunchNames =
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
