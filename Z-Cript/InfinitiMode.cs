using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfinitiMode : MonoBehaviour
{
    public TutorialMissionManager tmm;
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
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("열쇠가 부족하여 입장할 수 없습니다.");
            else
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("Not enough key.");
            return;
        }

        // 필수 맷집력 거르고
        /// 맷집으로 거르기
        string currentMattzip = UserWallet.GetInstance().GetMattzipForCul(PlayerPrefsManager.GetInstance().Mat_Mattzip);
        /// 1단계
        string tmp = "100";
        /// 2단계
        tmp = dts.SubStringDouble(currentMattzip, "1100");
        if (_index == "1050" && tmp == "-1") return;
        /// 3단계
        tmp = dts.SubStringDouble(currentMattzip, "12100");
        if (_index == "11025" && tmp == "-1") return;
        /// 4단계
        tmp = dts.SubStringDouble(currentMattzip, "133100");
        if (_index == "115763" && tmp == "-1") return;
        /// 5단계
        tmp = dts.SubStringDouble(currentMattzip, "1464100");
        if (_index == "1215506" && tmp == "-1") return;
        /// 6단계
        tmp = dts.SubStringDouble(currentMattzip, "16105100");
        if (_index == "12762816" && tmp == "-1") return;
        /// 7단계
        tmp = dts.SubStringDouble(currentMattzip, "177156100");
        if (_index == "134009564" && tmp == "-1") return;
        /// 8단계
        tmp = dts.SubStringDouble(currentMattzip, "1948717100");
        if (_index == "1407100423" && tmp == "-1") return;
        /// 9단계
        tmp = dts.SubStringDouble(currentMattzip, "21435888100");
        if (_index == "14774554438" && tmp == "-1") return;
        /// 10단계
        tmp = dts.SubStringDouble(currentMattzip, "235794769100");
        if (_index == "155132821598" && tmp == "-1") return;

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

            case "1050":
                if (sDataList[1] == "0") SkipGameBtn.GetChild(1).gameObject.SetActive(true);
                break;

            case "11025":
                if (sDataList[2] == "0") SkipGameBtn.GetChild(1).gameObject.SetActive(true);
                break;

            case "115763":
                if (sDataList[3] == "0") SkipGameBtn.GetChild(1).gameObject.SetActive(true);
                break;

            case "1215506":
                if (sDataList[4] == "0") SkipGameBtn.GetChild(1).gameObject.SetActive(true);
                break;

            case "12762816":
                if (sDataList[5] == "0") SkipGameBtn.GetChild(1).gameObject.SetActive(true);
                break;

            case "134009564":
                if (sDataList[6] == "0") SkipGameBtn.GetChild(1).gameObject.SetActive(true);
                break;

            case "1407100423":
                if (sDataList[7] == "0") SkipGameBtn.GetChild(1).gameObject.SetActive(true);
                break;

            case "14774554438":
                if (sDataList[8] == "0") SkipGameBtn.GetChild(1).gameObject.SetActive(true);
                break;

            case "155132821598":
                if (sDataList[9] == "0") SkipGameBtn.GetChild(1).gameObject.SetActive(true);
                break;

        }

        // 입장소탕 팝업 출력
        EnterPop.SetActive(true);
        EnterPop.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        EnterPop.GetComponent<Animation>().Play("Roll_Incre");
    }

    /// <summary>
    /// 무한버티기 입장 버튼 누를 때.
    /// </summary>
    public void RealInfiEnter()
    {
        if (PlayerPrefsManager.GetInstance().key <= 0)
        {
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("열쇠가 부족하여 입장할 수 없습니다.");
            else
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("Not enough key.");
            return;
        }

        PlayerPrefsManager.GetInstance().key--;
        tmm.ExUpdateMission(12); /// 미션 업데이트
        tmm.ExUpdateMission(37); /// 미션 업데이트
        tmm.ExUpdateMission(52); /// 미션 업데이트
        tmm.ExUpdateMission(79); /// 미션 업데이트
        Debug.LogError("EnterIndex : " + EnterIndex);
        // 2단계 버티기
        if (EnterIndex == "1050")
        {
            tmm.ExUpdateMission(66); /// 미션 업데이트
        }


        // 팝업 꺼주기
        PopUpObjectManager.GetInstance().NewInfinityPopUp.SetActive(false);
        EnterPop.SetActive(false);

        /// 
        tmm.mainBtnObjt.SetActive(false);
        //
        gotoMINIgame.ChangeCamToMiniView();
        booster_KEY.KeyTimerStart();
        //
        Left.GetChild(0).GetChild(1).gameObject.SetActive(true);
        Left.GetChild(1).GetChild(1).gameObject.SetActive(true);
        Left.GetChild(2).GetChild(1).gameObject.SetActive(true);
        Left.GetChild(3).GetChild(1).gameObject.SetActive(true);
        Left.GetChild(4).GetChild(4).gameObject.SetActive(true);
        Left.GetChild(5).GetChild(1).gameObject.SetActive(true);
        Left.GetChild(6).GetChild(1).gameObject.SetActive(true);
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
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("열쇠가 부족하여 입장할 수 없습니다.");
            else
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("Not enough key.");
            return;
        }

        PlayerPrefsManager.GetInstance().key--;
        //UserWallet.GetInstance().ShowUserKey();
        booster_KEY.KeyTimerStart();

        // 팝업 꺼주기
        //PopUpObjectManager.GetInstance().NewInfinityPopUp.SetActive(false);
        //EnterPop.SetActive(false);
        //

        /// 무한 버티기 각 단계 개인 최고 기록
        string infiPR = PlayerPrefsManager.GetInstance().InfiPersonalRecord;
        string[] sDataList = (infiPR).Split('*');

        float gettingGoop = 0;

        switch (EnterIndex)
        {
            case "100":
                gettingGoop = int.Parse(sDataList[0]) * 5f;
                break;

            case "1050":
                gettingGoop = int.Parse(sDataList[1]) * 11f;
                break;

            case "11025":
                gettingGoop = int.Parse(sDataList[2]) * 24f;
                break;

            case "115763":
                gettingGoop = int.Parse(sDataList[3]) * 54f;
                break;

            case "1215506":
                gettingGoop = int.Parse(sDataList[4]) * 106f;
                break;

            case "12762816":
                gettingGoop = int.Parse(sDataList[5]) * 217f;
                break;

            case "134009564":
                gettingGoop = int.Parse(sDataList[6]) * 440f;
                break;

            case "1407100423":
                gettingGoop = int.Parse(sDataList[7]) * 887f;
                break;

            case "14774554438":
                gettingGoop = int.Parse(sDataList[8]) * 1782f;
                break;

            case "155132821598":
                gettingGoop = int.Parse(sDataList[9]) * 3573f;
                break;

        }

        /// 국밥 증가
        /// 
        /// 국밥 획득량 % 증가
        float getSSalAmount = ((gettingGoop * 1f) * (1.0f + PlayerPrefsManager.GetInstance().Arti_InfiReword * 0.01f));

        getSSalAmount = getSSalAmount *
            (1.0f + 
            ((PlayerPrefsManager.GetInstance().uniformInfo[3].Uniform_LV +
            (PlayerPrefsManager.GetInstance().uniformInfo[4].Uniform_LV * 0.05f) +
            (PlayerPrefsManager.GetInstance().uniformInfo[5].Skill_LV * 0.5f))
            * 0.01f));

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
            Left.GetChild(0).GetChild(1).gameObject.SetActive(false);
            Left.GetChild(1).GetChild(1).gameObject.SetActive(false);
            Left.GetChild(2).GetChild(1).gameObject.SetActive(false);
            Left.GetChild(3).GetChild(1).gameObject.SetActive(false);
            Left.GetChild(4).GetChild(4).gameObject.SetActive(false);
            Left.GetChild(5).GetChild(1).gameObject.SetActive(false);
            Left.GetChild(6).GetChild(1).gameObject.SetActive(false);


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


            /// 튜토리얼 버튼 등장 
            tmm.ShowTutoCancas();

            gotoMINIgame.ChangeCamToHomePage();

        }


    }
}
