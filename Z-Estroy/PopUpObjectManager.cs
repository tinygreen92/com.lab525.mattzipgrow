using EasyMobile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpObjectManager : MonoBehaviour
{
    public TutorialMissionManager tmm;

    private static PopUpObjectManager instance;

    public static PopUpObjectManager GetInstance()
    {
        return instance;
    }
    DoubleToStringNum dts = new DoubleToStringNum();

    /*********************************************************/
    /*                                                       */
    /*         각종 팝업 오브젝트를 품으시오                 */
    /*                                                       */
    /*********************************************************/
    [Header("-TEST 텍스트")]
    public Text[] TestText;
    [Header("-PvP 진입 경고 메시지")]
    public GameObject PVP_EnterPop;
    [Header("-코인 획득시 얼마 획득 했나 표기")]
    public Transform GettingGoldMessege;
    [Header("-데이터 체크 팝업.")]
    public GameObject DATA_CheackPop;
    [Header("-무한 버티기 새로운 팝업.")]
    public GameObject NewInfinityPopUp;
    [Header("-무한 버티기 콤보")]
    public GameObject InfinityPopUp;
    [Header("-룰렛 팝업")]
    public GameObject RoulettePopUp;
    public Transform IconMom;
    public Text DescText;
    [Header("-버프 팝업")]
    public GameObject Buff_PopUP;
    [Header("-버티기 팝업")]
    public GameObject Infiniti_PopUP;
    [Header("-상점 팝업")]
    public GameObject ShopPopUP;
    [Header("-럭키 박스 팝업")]
    public GameObject LuckyBoxPopUP;
    [Header("-고소장 팝업")]
    public GameObject GOSOPopUP;
    [Header("-경고창 팝업")]
    public GameObject WarnningPopUP;
    [Header("-IAP 버튼 아이콘 부모 객체")]
    public Transform ShopIcon;
    public Transform BuyBtn;
    [Header("-펀치 해제 팝업")]
    public GameObject NewPunchPopUp;
    public Transform IconDad;
    [Header("- 튜토리얼 완료/ 스킵 ")]
    public GameObject TutorialComp;
    public GameObject TutorialSkip;
    [Header("- 버티기 보상 팝업 ")]
    public GameObject InfinityPopUP;
    [Header("- 방어전 시간 스킵 팝업 ")]
    public GameObject NewDefencePopUP;
    [Header("- 무한의 탑 보상 팝업 ")]
    public GameObject MuganRewordpop;
    [Header("- 무한의 탑 이어하기 ")]
    public GameObject MuganCountinue;
    [Header("- 미니게임 입장 ")]
    public GameObject MINIMINIGAME;










    /// <summary>
    /// 피빕 입장 팝업
    /// </summary>
    public void EnterThe_PVP()
    {
        isHideAnim = false;

        if (PVP_EnterPop.GetComponent<Animation>().isPlaying) return;

        //전체 켜줌
        PVP_EnterPop.SetActive(true);

        PVP_EnterPop.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        PVP_EnterPop.GetComponent<Animation>().Play("Roll_Incre");
    }
    public void HidePVP()
    {
        if (isHideAnim) return;

        isHideAnim = true;
        PVP_EnterPop.SetActive(false);
    }











    /// <summary>
    ///     [Header("-코인 획득시 얼마 획득 했나 표기")]
    /// </summary>
    public void GettingGoldMessage(double _getGold)
    {
        CancelInvoke("HIdeGoldProcess");

        var goldText = GettingGoldMessege.GetChild(0).GetChild(0).GetComponent<Text>();
        var result = " + ";
        result += dts.fDoubleToStringNumber(_getGold);
        goldText.text = result;

        GettingGoldMessege.gameObject.SetActive(true);

        Invoke("HIdeGoldProcess", 1f);
    }
    void HIdeGoldProcess()
    {
        GettingGoldMessege.gameObject.SetActive(false);
    }













    /// <summary>
    /// 불러올 때 나누 호출. 
    /// </summary>
    public void ShowDataCheackPop(bool _switch)
    {
        if (!_switch)
        {
            ShowWarnnigProcess("저장된 데이터가 존재하지 않습니다.");
        }
        else
        {
            Invoke("InitZZoGGo", 0.25f);
        }
        
    }


    void InitZZoGGo()
    {
        DATA_CheackPop.transform.GetChild(0).GetChild(1).GetComponent<Text>().text =
    "데이터 저장 일시 : " + PlayerPrefs.GetString("Z_Date")  + System.Environment.NewLine +
    "맷집 : " + UserWallet.GetInstance().SeetheTruth(double.Parse(PlayerPrefs.GetString("Z_Mattzip"))) + System.Environment.NewLine +
    "다이아몬드 : " + UserWallet.GetInstance().SeetheTruth(double.Parse(PlayerPrefs.GetString("Z_Dia"))) + System.Environment.NewLine +
    "국밥 : " + UserWallet.GetInstance().SeetheTruth(double.Parse(PlayerPrefs.GetString("Z_Gup"))) + System.Environment.NewLine +
    "쌀밥 : " + UserWallet.GetInstance().SeetheTruth(double.Parse(PlayerPrefs.GetString("Z_SSal"))) + System.Environment.NewLine +
    "열쇠 : " + PlayerPrefs.GetString("Z_Key");

        DATA_CheackPop.SetActive(true);
    }






    /// <summary>
    /// 미니게임 입장 팝업
    /// </summary>
    public void EnterTheMiniGame()
    {
        isHideAnim = false;

        if (MINIMINIGAME.GetComponent<Animation>().isPlaying) return;


        int Combo = PlayerPrefsManager.GetInstance().MaxGet_MiniGame;

        // 처음 실행시 소탕 회색버튼
        if (Combo == 0) MINIMINIGAME.transform.GetChild(1).GetChild(3).GetChild(1).GetChild(1).gameObject.SetActive(true);
        else MINIMINIGAME.transform.GetChild(1).GetChild(3).GetChild(1).GetChild(1).gameObject.SetActive(false);
        
        //전체 켜줌
        MINIMINIGAME.SetActive(true);

        MINIMINIGAME.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        MINIMINIGAME.GetComponent<Animation>().Play("Roll_Incre");
    }
    public void HideMiniGame()
    {
        if (isHideAnim) return;

        isHideAnim = true;
        MINIMINIGAME.SetActive(false);
    }



    /// <summary>
    /// 무한의 탑 이어하기 팝업
    /// </summary>
    public void ShowMuganCountinue()
    {
        isHideAnim = false;

        if (MuganCountinue.GetComponent<Animation>().isPlaying) return;

        //전체 켜줌
        MuganCountinue.SetActive(true);

        MuganCountinue.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        MuganCountinue.GetComponent<Animation>().Play("Roll_Incre");
    }
    public void HideMuganCountinue()
    {
        if (isHideAnim) return;

        isHideAnim = true;
        MuganCountinue.SetActive(false);
    }


    /// <summary>
    /// 무한의 탑 해금
    /// </summary>
    /// <param name="_index"></param>
    public void ShowMuganRewordpop(int _index, int _Amount)
    {
        isHideAnim = false;

        if (MuganRewordpop.GetComponent<Animation>().isPlaying) return;

        MuganRewordpop.transform.GetChild(1).GetChild(1).GetChild(0).gameObject.SetActive(false);
        MuganRewordpop.transform.GetChild(1).GetChild(1).GetChild(1).gameObject.SetActive(false);
        MuganRewordpop.transform.GetChild(1).GetChild(1).GetChild(2).gameObject.SetActive(false);
        MuganRewordpop.transform.GetChild(1).GetChild(1).GetChild(3).gameObject.SetActive(false);

        if (_index == 0)
        {
            MuganRewordpop.transform.GetChild(1).GetChild(1).GetChild(_index).gameObject.SetActive(true);
            MuganRewordpop.transform.GetChild(1).GetChild(2).GetComponent<Text>().text = "열쇠 "+ _Amount + "개 획득하였습니다.";

        }
        else if(_index == 1)
        {
            MuganRewordpop.transform.GetChild(1).GetChild(1).GetChild(_index).gameObject.SetActive(true);
            MuganRewordpop.transform.GetChild(1).GetChild(2).GetComponent<Text>().text = _Amount + "다이아를 획득하였습니다.";

        }
        else if(_index == 2)
        {
            MuganRewordpop.transform.GetChild(1).GetChild(1).GetChild(_index).gameObject.SetActive(true);
            MuganRewordpop.transform.GetChild(1).GetChild(2).GetComponent<Text>().text = "국밥 "+ _Amount + "그릇 획득하였습니다.";

        }
        else if (_index == 3)
        {
            MuganRewordpop.transform.GetChild(1).GetChild(1).GetChild(_index).gameObject.SetActive(true);
            MuganRewordpop.transform.GetChild(1).GetChild(2).GetComponent<Text>().text = "쌀밥 " + _Amount + "그릇 획득하였습니다.";

        }

        //전체 켜줌
        MuganRewordpop.SetActive(true);

        MuganRewordpop.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        MuganRewordpop.GetComponent<Animation>().Play("Roll_Incre");
        MuganRewordpop.transform.GetChild(0).GetComponent<Animation>().Play("Roll_Pop");

    }
    public void HIdeMuganRewordpop()
    {
        if (isHideAnim) return;

        isHideAnim = true;
        MuganRewordpop.SetActive(false);
    }


    public void ShowNewDefencePop()
    {
        NewDefencePopUP.SetActive(true);
    }
    public void HideNewDefencePop()
    {
        NewDefencePopUP.SetActive(false);
    }

    void ShopIAPInit()
    {
        for (int i = 0; i< ShopIcon.childCount; i++)
        {
            ShopIcon.GetChild(i).gameObject.SetActive(false);
            BuyBtn.GetChild(i).gameObject.SetActive(false);
        }

    }

    private void Awake()
    {
        instance = this;
        //
        RoulettePopUp.SetActive(false);
        ShopPopUP.SetActive(false);
        LuckyBoxPopUP.SetActive(false);
        GOSOPopUP.SetActive(false);
        WarnningPopUP.SetActive(false);
    }

    // 하이드 애니메이션 재생 중이니?
    private bool isHideAnim;

    /// <summary>
    /// 보상 팝업 기본 템플릿
    /// </summary>
    public void ShowRouletteReword(int _index, string _content)
    {
        isHideAnim = false;

        if (RoulettePopUp.GetComponent<Animation>().isPlaying) return;

        if (_index > 4) _index -= 5;

        IconMom.GetChild(_index).gameObject.SetActive(true);
        DescText.text = _content;
        //버튼 다른거 켜줌.
        RoulettePopUp.transform.GetChild(1).GetChild(3).gameObject.SetActive(true);
        RoulettePopUp.transform.GetChild(1).GetChild(4).gameObject.SetActive(false);
        //전체 켜줌
        RoulettePopUp.SetActive(true);

        RoulettePopUp.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        RoulettePopUp.GetComponent<Animation>().Play("Roll_Incre");
        RoulettePopUp.transform.GetChild(0).GetComponent<Animation>().Play("Roll_Pop");
    }
    /// <summary>
    /// (룰렛)동영상 광고보면 이거 호출
    /// </summary>
    /// <param name="_index"></param>
    /// <param name="_content"></param>
    public void ShowRoulette3XReword(int _index, string _content)
    {
        isHideAnim = false;

        if (RoulettePopUp.GetComponent<Animation>().isPlaying) return;

        /// 2배 보상 지급 -> 기본 지급 된거에 한번 더 지급하는 형식
        /// 
        var ppm = PlayerPrefsManager.GetInstance();
        var pgupbap = ppm.gupbap;
        var pssalbap = ppm.ssalbap;
        var pgold = ppm.gold;
        var pkey = ppm.key;
        //var pdia = ppm.diamond;
        var pdps = ppm.PlayerDPS;
        pdps = dts.multipleStringDouble(pdps, 1600d);

        switch (_index)
        {
            /// 쌀밥 추가

            case 0: ppm.gupbap = dts.AddStringDouble(pgupbap, "800"); break;
            case 1: ppm.key += 5; break;

            case 2: ppm.ssalbap = dts.AddStringDouble(pssalbap, "400"); break;

            case 3: PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 80); break;
            case 4: ppm.gold = dts.AddStringDouble(pgold, pdps); break;
                //
            case 5: ppm.gupbap = dts.AddStringDouble(pgupbap, "1600"); break;
            case 6: ppm.key += 10; break;
            case 7: ppm.ssalbap = dts.AddStringDouble(pssalbap, "800"); break;

            case 8:  PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 160); break;
            case 9:
                pdps = dts.multipleStringDouble(pdps, 2d);
                ppm.gold = dts.AddStringDouble(pgold, pdps);
                break;




        }

        UserWallet.GetInstance().ShowAllMoney();

        if (_index > 4) _index -= 5;

        IconMom.GetChild(_index).gameObject.SetActive(true);
        DescText.text = _content;

        //버튼 다른거 켜줌.
        RoulettePopUp.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
        RoulettePopUp.transform.GetChild(1).GetChild(4).gameObject.SetActive(true);
        //
        RoulettePopUp.SetActive(true);
  

        RoulettePopUp.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        RoulettePopUp.GetComponent<Animation>().Play("Roll_Incre");
        RoulettePopUp.transform.GetChild(0).GetComponent<Animation>().Play("Roll_Pop");
    }
    public void HIdeRouletteReword()
    {
        if (isHideAnim) return;
        //
        IconMom.GetChild(0).gameObject.SetActive(false);
        IconMom.GetChild(1).gameObject.SetActive(false);
        IconMom.GetChild(2).gameObject.SetActive(false);
        IconMom.GetChild(3).gameObject.SetActive(false);
        IconMom.GetChild(4).gameObject.SetActive(false);

        isHideAnim = true;
        RoulettePopUp.SetActive(false);
    }

    /// <summary>
    /// 상점 팝업 기본 띠용
    /// </summary>
    public void ShowShopProcess(int _index)
    {
        isHideAnim = false;

        if (ShopPopUP.GetComponent<Animation>().isPlaying) return;
        // 상점 아이콘 리셋 시켜주고
        ShopIAPInit();
        // 오토 공격 체크해서 이거 못 사게 해준다.
        if (PlayerPrefsManager.GetInstance().VIP == 525 && _index == 6) return;


        ShopIcon.GetChild(_index).gameObject.SetActive(true);
        BuyBtn.GetChild(_index).gameObject.SetActive(true);

        ShopPopUP.SetActive(true);

        ShopPopUP.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        ShopPopUP.GetComponent<Animation>().Play("Roll_Incre");
    }
    public void HIdeShopProcess()
    {
        if (isHideAnim) return;
        //
        isHideAnim = true;
        ShopPopUP.SetActive(false);
    }

    /// <summary>
    /// 골드 박스 클릭시 광고볼래 그냥 10배 할래
    /// </summary>
    public void ShowLuckyBoxProcess()
    {
        isHideAnim = false;

        if (LuckyBoxPopUP.GetComponent<Animation>().isPlaying) return;

        tmm.ExUpdateMission(23); /// 미션 업데이트


        LuckyBoxPopUP.SetActive(true);
        LuckyBoxPopUP.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        LuckyBoxPopUP.GetComponent<Animation>().Play("Roll_Incre");
    }

    /// <summary>
    /// 펀치 럭키박스 클릭시 애니메이션 재생하고 팝업 닫아
    /// </summary>
    public void HIdeLuckyProcess()
    {
        if (isHideAnim) return;
        //
        isHideAnim = true;
        LuckyBoxPopUP.SetActive(false);
    }

    /// <summary>
    /// 고소장 팝업
    /// </summary>
    public void ShowGoSoProcess()
    {
        isHideAnim = false;

        if (GOSOPopUP.GetComponent<Animation>().isPlaying) return;

        GOSOPopUP.SetActive(true);

        GOSOPopUP.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        GOSOPopUP.GetComponent<Animation>().Play("Roll_Incre");
    }
    public void HIdeGoSoProcess()
    {
        if (isHideAnim) return;
        //
        PlayerPrefsManager.GetInstance().isGoingGOSO = false;
        isHideAnim = true;
        GOSOPopUP.SetActive(false);
    }

    [Header("- 버프 팝업")]
    public Transform MotherIconPos;
    public Transform MotherBtn;
    public Text BuffDesc;
    int BuffKind;
    /// <summary>
    /// 버프 팝업
    /// </summary>
    public void ShowBuffProcess(int _Index)
    {
        isHideAnim = false;
        BuffKind = _Index;
        if (Buff_PopUP.GetComponent<Animation>().isPlaying) return;


        switch (_Index)
        {
            case 0:  /// 자동공격

                if (PlayerPrefsManager.GetInstance().VIP == 525 || PlayerPrefsManager.GetInstance().VIP == 625 || PlayerPrefsManager.GetInstance().VIP == 725 || PlayerPrefsManager.GetInstance().VIP == 825 || PlayerPrefsManager.GetInstance().isAutoAbsOn)
                {
                    return; /// VIP 결제 유저라면 600초 없다.
                }

                BuffDesc.text = "600초동안 자동공격";
                MotherIconPos.GetChild(0).gameObject.SetActive(true);
                MotherIconPos.GetChild(1).gameObject.SetActive(false);
                MotherIconPos.GetChild(2).gameObject.SetActive(false);

                MotherBtn.GetChild(0).gameObject.SetActive(true);
                MotherBtn.GetChild(1).gameObject.SetActive(false);
                MotherBtn.GetChild(2).gameObject.SetActive(false);
                break;

            case 1: /// 골드 획득 2배

                if (PlayerPrefsManager.GetInstance().VIP == 528 || PlayerPrefsManager.GetInstance().VIP == 625 || PlayerPrefsManager.GetInstance().VIP == 825 || PlayerPrefsManager.GetInstance().VIP == 925)
                {
                    return;
                }

                BuffDesc.text = "300초동안 골드 획득 2배";
                MotherIconPos.GetChild(0).gameObject.SetActive(false);
                MotherIconPos.GetChild(1).gameObject.SetActive(true);
                MotherIconPos.GetChild(2).gameObject.SetActive(false);

                MotherBtn.GetChild(0).gameObject.SetActive(false);
                MotherBtn.GetChild(1).gameObject.SetActive(true);
                MotherBtn.GetChild(2).gameObject.SetActive(false);
                break;

            case 2: /// 공격력 2배

                if (PlayerPrefsManager.GetInstance().VIP == 527 || PlayerPrefsManager.GetInstance().VIP == 625 || PlayerPrefsManager.GetInstance().VIP == 825 || PlayerPrefsManager.GetInstance().VIP == 925)
                {
                    return;
                }

                BuffDesc.text = "300초동안 공격력 2배";
                MotherIconPos.GetChild(0).gameObject.SetActive(false);
                MotherIconPos.GetChild(1).gameObject.SetActive(false);
                MotherIconPos.GetChild(2).gameObject.SetActive(true);

                MotherBtn.GetChild(0).gameObject.SetActive(false);
                MotherBtn.GetChild(1).gameObject.SetActive(false);
                MotherBtn.GetChild(2).gameObject.SetActive(true);
                break;
        }

        Buff_PopUP.SetActive(true);
        Buff_PopUP.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        Buff_PopUP.GetComponent<Animation>().Play("Roll_Incre");
    }
    public void HIdeBuffProcess()
    {
        if (isHideAnim) return;
        //
        isHideAnim = true;
        Buff_PopUP.SetActive(false);
    }


    [Header("- 버티기 맷집 거르기 버튼 커버 ")]
    public Transform btnSpace;
    /// <summary>
    /// 무한 버티기 팝업
    /// </summary>
    public void ShowInfiProcess()
    {
        /// 맷집으로 거르기

        string currentMattzip = UserWallet.GetInstance().GetMattzipForCul(PlayerPrefsManager.GetInstance().Mat_Mattzip);
        //
        for (int i = 1; i< btnSpace.childCount; i++)
        {
            btnSpace.GetChild(i).GetChild(1).gameObject.SetActive(false);
        }
        //
        /// 무한 버티기 각 단계 개인 최고 기록
        string infiPR = PlayerPrefsManager.GetInstance().InfiPersonalRecord;
        string[] sDataList = (infiPR).Split('*');

        var tmp = dts.SubStringDouble(currentMattzip, "1100");
        if (tmp == "-1")
        {
            btnSpace.GetChild(1).GetChild(1).gameObject.SetActive(true);
        }

        tmp = dts.SubStringDouble(currentMattzip, "12100");
        if (tmp == "-1")
        {
            btnSpace.GetChild(2).GetChild(1).gameObject.SetActive(true);
        }

        tmp = dts.SubStringDouble(currentMattzip, "133100");
        if (tmp == "-1")
        {
            btnSpace.GetChild(3).GetChild(1).gameObject.SetActive(true);
        }

        tmp = dts.SubStringDouble(currentMattzip, "1464100");
        if (tmp == "-1")
        {
            btnSpace.GetChild(4).GetChild(1).gameObject.SetActive(true);
        }

        tmp = dts.SubStringDouble(currentMattzip, "16105100");
        if (tmp == "-1")
        {
            btnSpace.GetChild(5).GetChild(1).gameObject.SetActive(true);
        }

        tmp = dts.SubStringDouble(currentMattzip, "177156100");
        if (tmp == "-1")
        {
            btnSpace.GetChild(6).GetChild(1).gameObject.SetActive(true);
        }

        tmp = dts.SubStringDouble(currentMattzip, "1948717100");
        if (tmp == "-1")
        {
            btnSpace.GetChild(7).GetChild(1).gameObject.SetActive(true);
        }

        tmp = dts.SubStringDouble(currentMattzip, "21435888100");
        if (tmp == "-1")
        {
            btnSpace.GetChild(8).GetChild(1).gameObject.SetActive(true);
        }

        tmp = dts.SubStringDouble(currentMattzip, "235794769100");
        if (tmp == "-1")
        {
            btnSpace.GetChild(9).GetChild(1).gameObject.SetActive(true);
        }


        /// 새 데이터로 덮어쓰기
        infiPR = "";
        for (int i = 0; i < sDataList.Length-1; i++)
        {
            infiPR += sDataList[i] + "*";
        }

        PlayerPrefsManager.GetInstance().InfiPersonalRecord = infiPR;

        isHideAnim = false;

        if (Infiniti_PopUP.GetComponent<Animation>().isPlaying) return;

        Infiniti_PopUP.SetActive(true);

        Infiniti_PopUP.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        Infiniti_PopUP.GetComponent<Animation>().Play("Roll_Incre");
    }

    public void HIdeInfiProcess()
    {
        if (isHideAnim) return;
        //
        isHideAnim = true;
        Infiniti_PopUP.SetActive(false);
    }

    /// <summary>
    /// 경고창 1초 팝업
    /// </summary>
    public void ShowWarnnigProcess(string tmp)
    {
        CancelInvoke("HIdeWarnnigProcess");
        //
        WarnningPopUP.SetActive(true);
        WarnningPopUP.GetComponentInChildren<Text>().text = tmp;
        //AudioManager.instance.Btn_warnnig();
        Invoke("HIdeWarnnigProcess", 1.6f);
    }
    void HIdeWarnnigProcess()
    {
        WarnningPopUP.SetActive(false);
    }


    /// <summary>
    /// 방어전 성공. 훈련도구 해금 
    /// </summary>
    /// <param name="_index"></param>
    public void ShowNewPunch(int _index)
    {
        isHideAnim = false;

        if (NewPunchPopUp.GetComponent<Animation>().isPlaying) return;

        IconDad.GetChild(_index).gameObject.SetActive(true);
        //전체 켜줌
        NewPunchPopUp.SetActive(true);

        NewPunchPopUp.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        NewPunchPopUp.GetComponent<Animation>().Play("Roll_Incre");
        NewPunchPopUp.transform.GetChild(0).GetComponent<Animation>().Play("Roll_Pop");

    }
    public void HIdeNewPunch()
    {
        if (isHideAnim) return;
        //
        for (int i = 0; i < IconDad.childCount; i++)
        {
            IconDad.GetChild(i).gameObject.SetActive(false);
        }
        isHideAnim = true;
        NewPunchPopUp.SetActive(false);
    }


    public GameObject OldPunchPopUp;
    public Transform MotherIcon;
    /// <summary>
    /// 방어전 출발 하기전 무슨 도구냐?
    /// </summary>
    /// <param name="_index"></param>
    public void ShowOldPunch(int _index)
    {
        isHideAnim = false;

        if (OldPunchPopUp.GetComponent<Animation>().isPlaying) return;

        var punchMax = PlayerPrefsManager.GetInstance().punchAmont -1;

        if (_index == 525)
        {
            ShowWarnnigProcess("모든 훈련도구를 획득하셨습니다!");
            return;
        }


        MotherIcon.GetChild(_index).gameObject.SetActive(true);
        //전체 켜줌
        OldPunchPopUp.SetActive(true);

    }
    public void HIdeOldPunch()
    {
        if (isHideAnim) return;
        //
        for (int i = 0; i < MotherIcon.childCount; i++)
        {
            MotherIcon.GetChild(i).gameObject.SetActive(false);
        }
        isHideAnim = true;
        OldPunchPopUp.SetActive(false);
    }




    public GameObject InfinityPopUPSkip;
    /// <summary>
    /// 버티기 모드 성공 보상 지급
    /// </summary>
    /// <param name="_index"></param>
    public void ShowInfinityPopUPSkip(double _amount)
    {
        isHideAnim = false;
        if (InfinityPopUPSkip.GetComponent<Animation>().isPlaying) return;

        InfinityPopUPSkip.transform.GetChild(1).GetChild(2).GetComponent<Text>().text = "국밥 " + _amount.ToString("f0") + " 그릇을 얻었다!";
        PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(PlayerPrefsManager.GetInstance().gupbap, _amount.ToString("f0"));

        UserWallet.GetInstance().ShowUserMilk();
        //전체 켜줌
        InfinityPopUPSkip.SetActive(true);
        InfinityPopUPSkip.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        InfinityPopUPSkip.GetComponent<Animation>().Play("Roll_Incre");
        InfinityPopUPSkip.transform.GetChild(0).GetComponent<Animation>().Play("Roll_Pop");
    }


    string GupBapAmount;
    /// <summary>
    /// 버티기 모드 성공 보상 지급
    /// </summary>
    /// <param name="_index"></param>
    public void ShowInfinityPopUP(double _amount)
    {
        // 광고볼지도 모르느끼 저장.
        GupBapAmount = _amount.ToString("f0");

        isHideAnim = false;
        if (InfinityPopUP.GetComponent<Animation>().isPlaying) return;

        //버튼 다른거 켜줌.
        InfinityPopUP.transform.GetChild(1).GetChild(3).gameObject.SetActive(true);
        InfinityPopUP.transform.GetChild(1).GetChild(4).gameObject.SetActive(false);

        InfinityPopUP.transform.GetChild(1).GetChild(2).GetComponent<Text>().text = "국밥 " + GupBapAmount + " 그릇을 얻었다!";
        PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(PlayerPrefsManager.GetInstance().gupbap, GupBapAmount);
        UserWallet.GetInstance().ShowUserMilk();
        //전체 켜줌
        InfinityPopUP.SetActive(true);
        InfinityPopUP.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        InfinityPopUP.GetComponent<Animation>().Play("Roll_Incre");
        InfinityPopUP.transform.GetChild(0).GetComponent<Animation>().Play("Roll_Pop");

    }
    public void ShowInfinity3X_PopUP(string s_amount)
    {

        double _amount = double.Parse(s_amount);

        isHideAnim = false;
        if (InfinityPopUP.GetComponent<Animation>().isPlaying) return;

        //버튼 다른거 켜줌.
        InfinityPopUP.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
        InfinityPopUP.transform.GetChild(1).GetChild(4).gameObject.SetActive(true);

        InfinityPopUP.transform.GetChild(1).GetChild(2).GetComponent<Text>().text = "국밥 " + (_amount * 2).ToString("f0") + " 그릇을 얻었다!";
        PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(PlayerPrefsManager.GetInstance().gupbap, _amount.ToString("f0"));
        UserWallet.GetInstance().ShowUserMilk();
        //전체 켜줌
        InfinityPopUP.SetActive(true);
        InfinityPopUP.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        InfinityPopUP.GetComponent<Animation>().Play("Roll_Incre");
        InfinityPopUP.transform.GetChild(0).GetComponent<Animation>().Play("Roll_Pop");

    }

    public void InfinityGoopBapOff()
    {
        // 잠시 오토 클릭 멈춰준다.
        //PlayerPrefsManager.GetInstance().isNotMainGame = false;
    }


    /// <summary>
    /// 무한 버티기 콤보 부모 활성화.
    /// 1. 게임 진입할 때 켜주고
    /// 2. 진입 빼면 꺼줌
    /// </summary>
    public void ShowInfiCombo(bool _swich)
    {
        InfinityPopUp.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "남은시간 : 60.0";
        InfinityPopUp.transform.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = 1f;

        InfinityPopUp.SetActive(_swich);
    }

    public int ComboCnt;

    /// <summary>
    /// 콤보 박스안에 숫자 집어 넣을 것.
    /// </summary>
    public void SetComboText()
    {
        InfinityPopUp.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "남은시간 : " + (60.0f - (ComboCnt * 0.1f)).ToString("f1");
        InfinityPopUp.transform.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = (600f - (ComboCnt * 1f)) / 600f;
    }








    #region <Rewarded Ads> 럭키 골드 상자 뻥튀기 광고

    public void GoldBoxFantasy()
    {
        //PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);

        int vvip = PlayerPrefsManager.GetInstance().VIP;
        //PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);
        if (vvip == 526 || vvip == 625 || vvip == 725 || vvip == 925)
        {
            GoldBoxAds();
            return;
        }

        if (Advertising.IsRewardedAdReady(RewardedAdNetwork.AdMob, AdPlacement.Default))
        {
            Advertising.RewardedAdCompleted += GoldBoxAdsCompleated;
            Advertising.RewardedAdSkipped += GoldBoxAdsSkipped;

            /// 애드몹 미디에이션 동영상 2순위
            Advertising.ShowRewardedAd(RewardedAdNetwork.AdMob, AdPlacement.Default);
        }
        else
        {
            ShowWarnnigProcess("광고를 준비중입니다. 잠시 후에 시도해주세요.");
            PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);

        }

    }

    // Event handler called when a rewarded ad has completed
    void GoldBoxAdsCompleated(RewardedAdNetwork network, AdPlacement location)
    {
        HIdeLuckyProcess();
        Invoke("GoldBoxAds", 0.5f);
        Advertising.RewardedAdCompleted -= GoldBoxAdsCompleated;
        Advertising.RewardedAdSkipped -= GoldBoxAdsSkipped;

    }

    // Event handler called when a rewarded ad has been skipped
    void GoldBoxAdsSkipped(RewardedAdNetwork network, AdPlacement location)
    {
        Advertising.RewardedAdCompleted -= GoldBoxAdsCompleated;
        Advertising.RewardedAdSkipped -= GoldBoxAdsSkipped;

        PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);
    }

    /// <summary>
    /// 펀치 골드박스에서 골드 2배 적용으로 변경
    /// </summary>
    void GoldBoxAds()
    {
        PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);
        HIdeLuckyProcess();

        float Gatcha = UserWallet.GetInstance().Gatcha;
        int target = UserWallet.GetInstance().target;

        if (Gatcha < 80f)
        {
            //골드 획득
            var tmpGold = PlayerPrefsManager.GetInstance().gold;
            ///골드 획득  계산식 수정 > 골드 획득량 = 맷집 * 1 * (유니폼 + 스킬 + 유물 + 훈련장)
            var value = PlayerPrefsManager.GetInstance().Mat_Mattzip;
            /// 훈련장 골드 버프
            double goldPer = PlayerPrefsManager.GetInstance().BG_CoinStat;
            double artiGoldPer = 1d * (
                // 골드 증가 유물
                PlayerPrefsManager.GetInstance().Arti_GoldPer +
                //유니폼 골드증가
                PlayerPrefsManager.GetInstance().uniformInfo[1].Uniform_LV +
                PlayerPrefsManager.GetInstance().uniformInfo[2].Uniform_LV +
                // 캐릭터 스킬 골드 증가
                PlayerPrefsManager.GetInstance().uniformInfo[3].Skill_LV);

            /// 2배 지급이므로, 다시한번더 같은 양을 더해줌
            value = dts.multipleStringDouble(value, 5d * (goldPer + (artiGoldPer * 0.01d)));

            PlayerPrefsManager.GetInstance().gold = dts.AddStringDouble(double.Parse(tmpGold), double.Parse(value));
            GettingGoldMessage(double.Parse(value) * 2d);
            ShowWarnnigProcess(UserWallet.GetInstance().SeetheNatural(double.Parse(value) * 2d) +" 골드를 획득하셨습니다.");
        }
        else if (Gatcha < 90f)
        {
            float luckyPer = PlayerPrefsManager.GetInstance().Arti_LuckyBoxPer;

            double getAmount = (target * (1.0d + (luckyPer * 0.01d)));
            string gupbap = PlayerPrefsManager.GetInstance().gupbap;
            PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(gupbap, getAmount.ToString("f0"));
            ShowWarnnigProcess("국밥 " + (getAmount * 2d).ToString("f0") + " 그릇을 획득하셨습니다.");

        }
        else if (Gatcha <= 100f)
        {
            float luckyPer = PlayerPrefsManager.GetInstance().Arti_LuckyBoxPer;

            double getAmount = (target * (1.0d + (luckyPer * 0.01d)));
            string ssal = PlayerPrefsManager.GetInstance().ssalbap;
            PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(ssal, getAmount.ToString("f0"));
            ShowWarnnigProcess("쌀밥 " + (getAmount * 2d).ToString("f0") + " 그릇을 획득하셨습니다.");

        }








        UserWallet.GetInstance().ShowUserGold();








        int vvip = PlayerPrefsManager.GetInstance().VIP;
        if (vvip == 526 || vvip == 625 || vvip == 725 || vvip == 925)
        {
            return;
        }

        /// 퀘스트
        PlayerPrefsManager.GetInstance().questInfo[0].daily_Abs++;


        if (PlayerPrefsManager.GetInstance().questInfo[0].All_Abs < 1000)
        {
            PlayerPrefsManager.GetInstance().questInfo[0].All_Abs++;
        }        //
    }

    #endregion



    #region <Rewarded Ads> 무한 버티기 국밥 뻥튀기

    public void GupBapFantasy()
    {
        /// 튜토리얼 중이면 광고 X
        if (!PlayerPrefsManager.GetInstance().isFristGameStart) return;

        //PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);

        int vvip = PlayerPrefsManager.GetInstance().VIP;
        //PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);
        if (vvip == 526 || vvip == 625 || vvip == 725 || vvip == 925)
        {
            GupBapFantasyAds();
            return;
        }

        if (Advertising.IsRewardedAdReady(RewardedAdNetwork.AdMob, AdPlacement.Default))
        {
            Advertising.RewardedAdCompleted += GupBapFantasyCompleated;
            Advertising.RewardedAdSkipped += GupBapFantasySkipped;

            /// 애드몹 미디에이션 동영상 2순위
            Advertising.ShowRewardedAd(RewardedAdNetwork.AdMob, AdPlacement.Default);
        }
        else
        {
            ShowWarnnigProcess("광고를 준비중입니다. 잠시 후에 시도해주세요.");
            PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);

        }

    }

    // Event handler called when a rewarded ad has completed
    void GupBapFantasyCompleated(RewardedAdNetwork network, AdPlacement location)
    {
        HIdeLuckyProcess();
        Invoke("GupBapFantasyAds", 0.5f);
        Advertising.RewardedAdCompleted -= GupBapFantasyCompleated;
        Advertising.RewardedAdSkipped -= GupBapFantasySkipped;

    }

    // Event handler called when a rewarded ad has been skipped
    void GupBapFantasySkipped(RewardedAdNetwork network, AdPlacement location)
    {
        Advertising.RewardedAdCompleted -= GupBapFantasyCompleated;
        Advertising.RewardedAdSkipped -= GupBapFantasySkipped;

        PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);
    }

    /// <summary>
    /// 광고 보면 5배 광고 박스
    /// </summary>
    void GupBapFantasyAds()
    {
        PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);
        HIdeLuckyProcess();

        ShowInfinity3X_PopUP(GupBapAmount);

        int vvip = PlayerPrefsManager.GetInstance().VIP;
        //PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);
        if (vvip == 526 || vvip == 625 || vvip == 725 || vvip == 925)
        {
            return;
        }

        /// 퀘스트
        PlayerPrefsManager.GetInstance().questInfo[0].daily_Abs++;

        if (PlayerPrefsManager.GetInstance().questInfo[0].All_Abs < 1000)
        {
            PlayerPrefsManager.GetInstance().questInfo[0].All_Abs++;
        }        //
    }

    #endregion




    public GameObject MiniGamereword;
    public GameObject MiniGamerewordSkip;
    int SSalBapAmount;
    /// <summary>
    /// 미니게임 보수 지급.
    /// </summary>
    /// <param name="_index"></param>
    public void ShowSSalPopUPSkip(float _amount)
    {
        MiniGamerewordSkip.transform.GetChild(1).GetChild(2).GetComponent<Text>().text = "쌀밥 " + _amount.ToString("f0") + " 그릇을 얻었다!";
        PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(PlayerPrefsManager.GetInstance().ssalbap, _amount.ToString("f0"));
        UserWallet.GetInstance().ShowUserSSalbap();
        PlayerPrefs.Save();
        //전체 켜줌
        MiniGamerewordSkip.SetActive(true);
        MiniGamerewordSkip.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        MiniGamerewordSkip.GetComponent<Animation>().Play("Roll_Incre");
        MiniGamerewordSkip.transform.GetChild(0).GetComponent<Animation>().Play("Roll_Pop");

    }
    /// <summary>
    /// 미니게임 보수 지급.
    /// </summary>
    /// <param name="_index"></param>
    public void ShowSSalPopUP(int _amount)
    {
        // 광고볼지도 모르느끼 저장.
        SSalBapAmount = _amount;

        //버튼 다른거 켜줌.
        MiniGamereword.transform.GetChild(1).GetChild(3).gameObject.SetActive(true);
        MiniGamereword.transform.GetChild(1).GetChild(4).gameObject.SetActive(false);

        MiniGamereword.transform.GetChild(1).GetChild(2).GetComponent<Text>().text = "쌀밥 " + _amount + " 그릇을 얻었다!";
        PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(PlayerPrefsManager.GetInstance().ssalbap, _amount.ToString("f0"));
        UserWallet.GetInstance().ShowUserSSalbap();
        PlayerPrefs.Save();
        //전체 켜줌
        MiniGamereword.SetActive(true);
        MiniGamereword.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        MiniGamereword.GetComponent<Animation>().Play("Roll_Incre");
        MiniGamereword.transform.GetChild(0).GetComponent<Animation>().Play("Roll_Pop");

    }
    public void ShowSSal3X_PopUP(int _amount)
    {
        //버튼 다른거 켜줌.
        MiniGamereword.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
        MiniGamereword.transform.GetChild(1).GetChild(4).gameObject.SetActive(true);

        Debug.LogWarning("_amount X3 " + _amount);
        Debug.LogWarning("SSalBapAmount X3 " + SSalBapAmount);


        MiniGamereword.transform.GetChild(1).GetChild(2).GetComponent<Text>().text = "쌀밥 " + (SSalBapAmount * 2) + " 그릇을 얻었다!";
        PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(PlayerPrefsManager.GetInstance().ssalbap, SSalBapAmount.ToString("f0"));
        UserWallet.GetInstance().ShowUserSSalbap();
        PlayerPrefs.Save();
        //전체 켜줌
        MiniGamereword.SetActive(true);
        MiniGamereword.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        MiniGamereword.GetComponent<Animation>().Play("Roll_Incre");
        MiniGamereword.transform.GetChild(0).GetComponent<Animation>().Play("Roll_Pop");

    }



    #region <Rewarded Ads> 미니 게임 쌀밥 뻥튀기

    public void SSalBapFantasy()
    {
        /// 튜토리얼 중이면 광고 X
        if (!PlayerPrefsManager.GetInstance().isFristGameStart) return;

        //PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);

        int vvip = PlayerPrefsManager.GetInstance().VIP;
        //PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);
        if (vvip == 526 || vvip == 625 || vvip == 725 || vvip == 925)
        {
            SSalBapFantasyAds();
            return;
        }

        if (Advertising.IsRewardedAdReady(RewardedAdNetwork.AdMob, AdPlacement.Default))
        {
            Advertising.RewardedAdCompleted += SSalBapFantasyCompleated;
            Advertising.RewardedAdSkipped += SSalBapFantasySkipped;

            /// 애드몹 미디에이션 동영상 2순위
            Advertising.ShowRewardedAd(RewardedAdNetwork.AdMob, AdPlacement.Default);
        }
        else
        {
            ShowWarnnigProcess("광고를 준비중입니다. 잠시 후에 시도해주세요.");
            PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);

        }

    }

    // Event handler called when a rewarded ad has completed
    void SSalBapFantasyCompleated(RewardedAdNetwork network, AdPlacement location)
    {
        HIdeLuckyProcess();
        Invoke("SSalBapFantasyAds", 0.5f);
        Advertising.RewardedAdCompleted -= SSalBapFantasyCompleated;
        Advertising.RewardedAdSkipped -= SSalBapFantasySkipped;

    }

    // Event handler called when a rewarded ad has been skipped
    void SSalBapFantasySkipped(RewardedAdNetwork network, AdPlacement location)
    {
        Advertising.RewardedAdCompleted -= SSalBapFantasyCompleated;
        Advertising.RewardedAdSkipped -= SSalBapFantasySkipped;

        PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);
    }

    /// <summary>
    /// 광고 보면 3배 광고 박스
    /// </summary>
    void SSalBapFantasyAds()
    {
        PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);
        HIdeLuckyProcess();

        ShowSSal3X_PopUP(SSalBapAmount);

        int vvip = PlayerPrefsManager.GetInstance().VIP;
        //PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);
        if (vvip == 526 || vvip == 625 || vvip == 725 || vvip == 925)
        {
            return;
        }

        /// 퀘스트
        PlayerPrefsManager.GetInstance().questInfo[0].daily_Abs++;

        if (PlayerPrefsManager.GetInstance().questInfo[0].All_Abs < 1000)
        {
            PlayerPrefsManager.GetInstance().questInfo[0].All_Abs++;
        }        //
    }

    #endregion












}
