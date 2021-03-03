using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfinitiMode : MonoBehaviour
{
    public TapToSpawnLimit tapToSpawnLimit;
    public GotoMINIgame gotoMINIgame;
    public Booster_KEY booster_KEY;
    //
    [Header("-봇 버튼 비활성화.")]
    public Transform Left;
    public GameObject Blank;

    [Header("-체력바")]
    public Image fiilAmount;
    public Text hp_text;

    [Header("-입장 소탕 팝업")]
    public GameObject EnterPop;
    public Transform SkipGameBtn;

    bool isStart;
    DoubleToStringNum dts = new DoubleToStringNum();
    /// <summary>
    /// 무한 버티기 외부에서 호출
    /// </summary>
    public void StartInfi(string _index)
    {

        // 열쇠 여부 거르고
        if (PlayerPrefsManager.GetInstance().key == 0)
        {
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("열쇠가 부족하여 입장할 수 없습니다.");
            return;
        }

        // 필수 맷집력 거르고
        /// 맷집으로 거르기

        string currentMattzip = UserWallet.GetInstance().GetMattzipForCul(PlayerPrefsManager.GetInstance().Mat_Mattzip);
        //
        var tmp = dts.SubStringDouble(currentMattzip, "1000");
        if (_index == "1000" && tmp == "-1") return;
        tmp = dts.SubStringDouble(currentMattzip, "10000");
        if (_index == "10000" && tmp == "-1") return;
        tmp = dts.SubStringDouble(currentMattzip, "100000");
        if (_index == "100000" && tmp == "-1") return;
        tmp = dts.SubStringDouble(currentMattzip, "1000000");
        if (_index == "1000000" && tmp == "-1") return;
        tmp = dts.SubStringDouble(currentMattzip, "10000000");
        if (_index == "10000000" && tmp == "-1") return;
        tmp = dts.SubStringDouble(currentMattzip, "100000000");
        if (_index == "100000000" && tmp == "-1") return;
        tmp = dts.SubStringDouble(currentMattzip, "1000000000");
        if (_index == "1000000000" && tmp == "-1") return;
        tmp = dts.SubStringDouble(currentMattzip, "10000000000");
        if (_index == "10000000000" && tmp == "-1") return;
        tmp = dts.SubStringDouble(currentMattzip, "100000000000");
        if (_index == "100000000000" && tmp == "-1") return;

        /// 열쇠 충분하다면 입장 /소탕 팝업 호출
        CallPopUp(_index);

    }

    /// <summary>
    /// 입장 팝업 호출할 때 할당
    /// </summary>
    string EnterIndex;
    void CallPopUp(string _index)
    {
        EnterIndex = _index;
        // 기본은 회색 버튼 없는거.
        SkipGameBtn.GetChild(1).gameObject.SetActive(false);
        /// 무한 버티기 각 단계 개인 최고 기록
        string infiPR = PlayerPrefs.GetString("InfiPersonalRecord");
        if (infiPR == "0" || infiPR == "")
        {
            infiPR = "0*0*0*0*0*0*0*0*0*0*";
            PlayerPrefs.SetString("InfiPersonalRecord", "0*0*0*0*0*0*0*0*0*0*");
        } 
        string[] sDataList = (infiPR).Split('*');

        for(int i = 0; i< sDataList.Length; i++)
        {
            Debug.LogWarning(i + " :: " + sDataList[i]);
        }

        switch (EnterIndex)
        {
            case "100":
                if (sDataList[0] == "0") SkipGameBtn.GetChild(1).gameObject.SetActive(true);
                break;

            case "1000":
                if (sDataList[1] == "0") SkipGameBtn.GetChild(1).gameObject.SetActive(true);
                break;

            case "10000":
                if (sDataList[2] == "0") SkipGameBtn.GetChild(1).gameObject.SetActive(true);
                break;

            case "100000":
                if (sDataList[3] == "0") SkipGameBtn.GetChild(1).gameObject.SetActive(true);
                break;

            case "1000000":
                if (sDataList[4] == "0") SkipGameBtn.GetChild(1).gameObject.SetActive(true);
                break;

            case "10000000":
                if (sDataList[5] == "0") SkipGameBtn.GetChild(1).gameObject.SetActive(true);
                break;

            case "100000000":
                if (sDataList[6] == "0") SkipGameBtn.GetChild(1).gameObject.SetActive(true);
                break;

            case "1000000000":
                if (sDataList[7] == "0") SkipGameBtn.GetChild(1).gameObject.SetActive(true);
                break;

            case "10000000000":
                if (sDataList[8] == "0") SkipGameBtn.GetChild(1).gameObject.SetActive(true);
                break;

            case "100000000000":
                if (sDataList[9] == "0") SkipGameBtn.GetChild(1).gameObject.SetActive(true);
                break;

        }

        // 입장소탕 팝업 출력
        EnterPop.SetActive(true);
    }

    /// <summary>
    /// 입장 버튼 누를 때.
    /// </summary>
    public void RealInfiEnter()
    {
        if (PlayerPrefsManager.GetInstance().key <= 0)
        {
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("열쇠가 부족하여 입장할 수 없습니다.");
            return;
        }

        PlayerPrefsManager.GetInstance().key--;
        UserWallet.GetInstance().ShowUserKey();
        // 팝업 꺼주기
        PopUpObjectManager.GetInstance().NewInfinityPopUp.SetActive(false);
        EnterPop.SetActive(false);
        //
        gotoMINIgame.ChangeCamToMiniView();
        booster_KEY.KeyTimerStart();
        //
        Left.GetChild(0).GetChild(0).gameObject.SetActive(true);
        Left.GetChild(1).GetChild(0).gameObject.SetActive(true);
        Left.GetChild(2).GetChild(0).gameObject.SetActive(true);
        Left.GetChild(3).GetChild(0).gameObject.SetActive(true);
        Left.GetChild(4).GetChild(0).gameObject.SetActive(true);
        Left.GetChild(5).GetChild(0).gameObject.SetActive(true);
        Left.GetChild(6).GetChild(0).gameObject.SetActive(true);
        //
        isStart = true;

        Blank.SetActive(true);
        //
        var maxHP = PlayerPrefsManager.GetInstance().Mat_MaxHP;
        PlayerPrefsManager.GetInstance().Mat_currentHP = maxHP;
        fiilAmount.fillAmount = 1f;
        hp_text.text = UserWallet.GetInstance().SeetheNatural(double.Parse(maxHP)) + "/" + UserWallet.GetInstance().SeetheNatural(double.Parse(maxHP));

        // 난이도 몇?
        PlayerPrefsManager.GetInstance().Infi_Index = decimal.Parse(EnterIndex);
        // 초기 공격력? 인덱스 만큼 계속 더해주는 걸로
        PlayerPrefsManager.GetInstance().InfiPunchDPS = (double)decimal.Parse(EnterIndex);

        // 콤보 숫자 초기화.
        PopUpObjectManager.GetInstance().ComboCnt = 0;

        // 트리거 온!!!
        PlayerPrefsManager.GetInstance().isInfinity = true;
    }


    /// <summary>
    /// 소탕 버튼 누를 때.
    /// </summary>
    public void RealInfiSkipEnter()
    {
        /// 그레이 버튼 활성화 시 리턴
        if (SkipGameBtn.GetChild(1).gameObject.activeSelf) return;

        if (PlayerPrefsManager.GetInstance().key <= 0)
        {
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("열쇠가 부족하여 입장할 수 없습니다.");
            return;
        }

        PlayerPrefsManager.GetInstance().key--;
        UserWallet.GetInstance().ShowUserKey();
        booster_KEY.KeyTimerStart();

        // 팝업 꺼주기
        //PopUpObjectManager.GetInstance().NewInfinityPopUp.SetActive(false);
        //EnterPop.SetActive(false);
        //

        /// 무한 버티기 각 단계 개인 최고 기록
        string infiPR = PlayerPrefsManager.GetInstance().InfiPersonalRecord;
        string[] sDataList = (infiPR).Split('*');

        int gettingGoop = 0;

        switch (EnterIndex)
        {
            case "100":
                gettingGoop = int.Parse(sDataList[0]) * 5;
                break;

            case "1000":
                gettingGoop = int.Parse(sDataList[1]) * 30;
                break;

            case "10000":
                gettingGoop = int.Parse(sDataList[2]) * 60;
                break;

            case "100000":
                gettingGoop = int.Parse(sDataList[3]) * 120;
                break;

            case "1000000":
                gettingGoop = int.Parse(sDataList[4]) * 240;
                break;

            case "10000000":
                gettingGoop = int.Parse(sDataList[5]) * 480;
                break;

            case "100000000":
                gettingGoop = int.Parse(sDataList[6]) * 960;
                break;

            case "1000000000":
                gettingGoop = int.Parse(sDataList[7]) * 1920;
                break;

            case "10000000000":
                gettingGoop = int.Parse(sDataList[8]) * 3840;
                break;

            case "100000000000":
                gettingGoop = int.Parse(sDataList[9]) * 19200;
                break;

        }

        /// 국밥 증가
        /// 
        /// 국밥 획득량 % 증가
        double getSSalAmount = ((gettingGoop * 1d) * (1.0d + PlayerPrefsManager.GetInstance().Arti_InfiReword * 0.01d));

        getSSalAmount = getSSalAmount *
            (1.0d + ((PlayerPrefsManager.GetInstance().uniformInfo[3].Uniform_LV +
            (PlayerPrefsManager.GetInstance().uniformInfo[4].Uniform_LV * 0.05d) +
            (PlayerPrefsManager.GetInstance().uniformInfo[5].Skill_LV * 0.5d))


            * 0.01d));

        Debug.LogError("스킵 국밥 : " + getSSalAmount);

        // 국밥 팝업 호출
        PopUpObjectManager.GetInstance().ShowInfinityPopUPSkip(getSSalAmount);

        // 콤보 숫자 초기화.
        PopUpObjectManager.GetInstance().ComboCnt = 0;
    }




    /// <summary>
    /// 그만두기 버튼 누른다
    /// </summary>
    public void EndBtnClicked()
    {
        PlayerPrefsManager.GetInstance().Mat_currentHP = "-1";
    }

    void FixedUpdate()
    {
        if (isStart && !PlayerPrefsManager.GetInstance().isInfinityEnd)
        {
            // 펀치시작
            tapToSpawnLimit.ClickedInfiniti();
        }
        // 끝났냐?
        if (PlayerPrefsManager.GetInstance().isInfinityEnd)
        {
            isStart = false;
            PlayerPrefsManager.GetInstance().isInfinityEnd = false;
            //
            Left.GetChild(0).GetChild(0).gameObject.SetActive(false);
            Left.GetChild(1).GetChild(0).gameObject.SetActive(false);
            Left.GetChild(2).GetChild(0).gameObject.SetActive(false);
            Left.GetChild(3).GetChild(0).gameObject.SetActive(false);
            Left.GetChild(4).GetChild(0).gameObject.SetActive(false);
            Left.GetChild(5).GetChild(0).gameObject.SetActive(false);
            Left.GetChild(6).GetChild(0).gameObject.SetActive(false);


            Blank.SetActive(false);

            tapToSpawnLimit.PunchIndexUpdate(PlayerPrefsManager.GetInstance().PunchIndex);

            //
            var maxHP = PlayerPrefsManager.GetInstance().Mat_MaxHP;
            PlayerPrefsManager.GetInstance().Mat_currentHP = maxHP;
            fiilAmount.fillAmount = 1f;
            hp_text.text = UserWallet.GetInstance().SeetheNatural(double.Parse(maxHP)) + "/" + UserWallet.GetInstance().SeetheNatural(double.Parse(maxHP));

            //
            PlayerPrefsManager.GetInstance().isInfinity = false;
            PlayerPrefsManager.GetInstance().isEndGame = false;
            gotoMINIgame.ChangeCamToHomePage();

        }


    }
}
