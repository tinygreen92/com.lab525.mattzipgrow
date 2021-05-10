using EasyMobile;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;



public class MuganMode : MonoBehaviour
{
    public TutorialMissionManager tmm;

    public Booster_KEY booster_KEY;
    public TapToSpawnLimit tapToSpawnLimit;
    public GotoMINIgame gotoMINIgame;
    //
    public GameObject popupMugan;
    public Text popupMuganName;
    [Header("-쉴드 활성화")]
    public GameObject Left;
    public GameObject Right;
    [Header("-플레이어 체력바")]
    public Image fiilAmount;
    public Text hp_text;
    [Header("-보스 체력바")]
    public Image boss_fiilAmount;
    public Image boss_Time;
    public Text boss_time;
    [Header("-무탑 층수")]
    public Text stageName;


    public void popupStart()
    {
        int Stage = PlayerPrefsManager.GetInstance().MaxGet_MuganTop;

        /// 번역
        if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
        {
            if (Stage >= 201)
            {
                popupMuganName.text = "무한의 탑 클리어!";
                popupMuganName.transform.parent.GetComponent<Button>().enabled = false;
            }
            else
            {
                popupMuganName.text = Stage + "층 입장 >";
            }
        }
        else
        {
            if (Stage >= 201)
            {
                popupMuganName.text = "번역 !";
                popupMuganName.transform.parent.GetComponent<Button>().enabled = false;
            }
            else
            {
                popupMuganName.text = Stage + "번역 입장 >";
            }
        }



        popupMugan.SetActive(true);

        popupMugan.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        popupMugan.GetComponent<Animation>().Play("Roll_Incre");
    }


    bool isStart;

    /// <summary>
    /// 보스 체력 계산해서 가져옴
    /// - 보스 HP 시작값 200
    ///- 보스 HP 증가 계산식 >  200 * 1.3 ^ stage
    /// </summary>
    string GetBossHp(int stage)
    {
        /// 1층일때는 200
        if (stage == 1)
            return "200";
        /// 1층 이상일때
        else
        {
            double tmp = 200d * System.Math.Pow(1.3d, stage);
            return tmp.ToString("F0");
        }
    }

    /// <summary>
    /// 무한의 탑 팝업 버튼에서 호출 "1층 입장 >"
    /// </summary>
    public void StartInfi()
    {
        /// 열쇠 체크
        if (PlayerPrefsManager.GetInstance().key <= 0)
        {
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("열쇠가 부족하여 입장할 수 없습니다.");
            return;
        }
        // 실드  값 초기화
        isWaiting = false;
        // 이어하기 초기화.
        PlayerPrefsManager.GetInstance().isSecondChan = false;
        //입장팝업 꺼주고
        popupMugan.SetActive(false);
        //실드 이미지 꺼주고
        Left.SetActive(false);
        Right.SetActive(false);
        /// 입장 스테이지 써주고
        /// /// 번역
        if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
            stageName.text = "무한의 탑 " + PlayerPrefsManager.GetInstance().MaxGet_MuganTop + "층";
        else
            stageName.text = "번역 탑 " + PlayerPrefsManager.GetInstance().MaxGet_MuganTop + "층";
        //보스 체력 설정
        PlayerPrefsManager.GetInstance().MAX_boss_HP = GetBossHp(PlayerPrefsManager.GetInstance().MaxGet_MuganTop);
        PlayerPrefsManager.GetInstance().bossHP = PlayerPrefsManager.GetInstance().MAX_boss_HP;
        // 입장 완료되면 키 하나 소모
        PlayerPrefsManager.GetInstance().key--;

        tmm.ExUpdateMission(26); /// 미션 업데이트
        tmm.ExUpdateMission(39); /// 미션 업데이트
        tmm.ExUpdateMission(47); /// 미션 업데이트
        tmm.ExUpdateMission(57); /// 미션 업데이트
        tmm.ExUpdateMission(86); /// 미션 업데이트


        // 키회복 코루틴 스타트
        booster_KEY.KeyTimerStart();

        /// 무한의 탑 게임 화면으로 전환.
        gotoMINIgame.ChangeCamMUGANTOP();
        ///
        string maxHP = PlayerPrefsManager.GetInstance().Mat_MaxHP;
        /// 최대 체력으로 회복 해줌
        PlayerPrefsManager.GetInstance().Mat_currentHP = maxHP;
        /// 체력바 게이지 초기화
        fiilAmount.fillAmount = 1f;
        boss_fiilAmount.fillAmount = 1f;
        /// 체력바 수치 텍스트 적어주고
        hp_text.text = UserWallet.GetInstance().SeetheNatural(double.Parse(maxHP)) + "/" + UserWallet.GetInstance().SeetheNatural(double.Parse(maxHP));
        // 카운트 다운 온!
        PreparedPanel.SetActive(true);
        // 3 2 1카운트 다운
        StartCoroutine(CountTimer());
    }

    public Text CountText;
    public GameObject PreparedPanel;

    /// <summary>
    /// 대기화면 카운트다운
    /// </summary>
    /// <returns></returns>
    IEnumerator CountTimer()
    {
        yield return null;

        TimerStart();
        //
        int cnt = 3;
        while (cnt > 0)
        {
            CountText.text = cnt.ToString();
            yield return new WaitForSeconds(1);
            cnt--;
        }

        yield return new WaitForFixedUpdate();
        //
        PlayerPrefsManager.GetInstance().isMuGanTopEnd = false;
        isStart = true;
        ///
        stromking = StartCoroutine(PunchStorm());
        /// 카운트 다운 패널 끄고
        PreparedPanel.SetActive(false);
    }


    /// <summary>
    /// 게임 카운트  시작
    /// </summary>
    public void TimerStart()
    {
        float Maxcnt = 30f + (PlayerPrefsManager.GetInstance().Arti_MuganTime * 0.1f);

        // 보스 타이머 표시
        boss_Time.fillAmount = 1;
        /// 번역
        if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
            boss_time.text = "남은 시간 : " + string.Format("{0:f1}", Maxcnt);
        else
            boss_time.text = "번역 시간 : " + string.Format("{0:f1}", Maxcnt);
    }

    Coroutine stromking;
    /// <summary>
    /// 20초 동안 초당 15회 가격 fixedUpdate
    /// </summary>
    /// <returns></returns>
    IEnumerator PunchStorm()
    {
        float Maxcnt = 30f + (PlayerPrefsManager.GetInstance().Arti_MuganTime * 0.1f);
        float cnt = Maxcnt;

        /// 번역
        if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
        {
            while (cnt > 0)
            {
                yield return new WaitForSeconds(0.05f);

                /// 무한 모드 끝남 스위치 true 되면 카운트 멈춤.
                if (!PlayerPrefsManager.GetInstance().isMuGanTopEnd)
                {
                    cnt -= 0.05f;

                    boss_Time.fillAmount = cnt / Maxcnt;
                    boss_time.text = "남은 시간 : " + string.Format("{0:f1}", cnt);
                }

            }
        }
        else
        {
            while (cnt > 0)
            {
                yield return new WaitForSeconds(0.05f);

                /// 무한 모드 끝남 스위치 true 되면 카운트 멈춤.
                if (!PlayerPrefsManager.GetInstance().isMuGanTopEnd)
                {
                    cnt -= 0.05f;

                    boss_Time.fillAmount = cnt / Maxcnt;
                    boss_time.text = "번역 시간 : " + string.Format("{0:f1}", cnt);
                }

            }
        }





        /// 30초 동안 못이기면 실패.
        StopCoroutine(stromking);
        /// 2번째 기회를 이미 사용했다면 종료
        if (PlayerPrefsManager.GetInstance().isSecondChan)
        {
            EndBtnClicked();
        }
        else
        {
            PlayerPrefsManager.GetInstance().isMuGanTopEnd = true;
            /// TODO : 이어하기 팝업 호출.
            PopUpObjectManager.GetInstance().ShowMuganCountinue();
        }

    }


    DoubleToStringNum dts = new DoubleToStringNum();
    /// <summary>
    /// 보상 받아 버려
    /// </summary>
    public void ClearMuGanTop()
    {
        isStart = false;
        isLeftBtnClicked = false;
        isRightBtnClicked = false;
        //타이머 초기화.
        StopCoroutine(stromking);
        // 무한의 탑 층수.
        int Stage = PlayerPrefsManager.GetInstance().MaxGet_MuganTop;
        /// 다이아 지급
        if ((Stage % 5) == 0)
        {
            int getDia = 50 + (50 * (Stage / 5));
            // 다이아
            PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + getDia);
            PopUpObjectManager.GetInstance().ShowMuganRewordpop(1, getDia);
        }
        /// 국밥 지급
        else if ((Stage % 5) == 2)
        {
            int getGupbap = Stage / 5;
            getGupbap++;
            // 국밥 획득.
            string gupbap = PlayerPrefsManager.GetInstance().gupbap;
            PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(gupbap, (500 * getGupbap).ToString("f0"));
            PopUpObjectManager.GetInstance().ShowMuganRewordpop(2, (500 * getGupbap));
        }
        /// 쌀밥 지급
        else if ((Stage % 5) == 4)
        {
            int getSSSal = Stage / 5;
            getSSSal++;
            // 국밥 획득.
            string gupbap = PlayerPrefsManager.GetInstance().gupbap;
            PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(gupbap, (250 * getSSSal).ToString("f0"));
            PopUpObjectManager.GetInstance().ShowMuganRewordpop(3, (250 * getSSSal));
        }
        /// 그 외는 열쇠 획득.
        else
        {
            PopUpObjectManager.GetInstance().ShowMuganRewordpop(0, 3);
            PlayerPrefsManager.GetInstance().key++;
        }

        if (Stage != 200) 
            PlayerPrefsManager.GetInstance().MaxGet_MuganTop++;
        else if (Stage >= 200) 
            PlayerPrefsManager.GetInstance().MaxGet_MuganTop = 201;

        UserWallet.GetInstance().ShowAllMoney();

        //퀘스트
        PlayerPrefsManager.GetInstance().questInfo2[0].All_Mugan++;

        //실드 꺼져
        Left.SetActive(false);
        Right.SetActive(false);

        Invoke("InvoBackHome", 1.0f);

    }



    /// <summary>
    /// 이어하기 팝업해서 행동
    /// 1. 다이아 소모하고 한번 더.
    /// </summary>
    public void MuganCoutiForDia()
    {
        float dia = PlayerPrefs.GetFloat("dDiamond");
        //다이아 체크
        if (dia < 100)
        {
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("보유 다이아가 부족합니다.");
            else
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("Not enough diamonds.");
            return;
        }
        // 팝업 닫아줌
        PopUpObjectManager.GetInstance().HideMuganCountinue();

        // 다이아 소모 해줌.
        PlayerPrefs.SetFloat("dDiamond", dia - 100);
        UserWallet.GetInstance().ShowUserDia();

        // 이어하기 썼다.
        PlayerPrefsManager.GetInstance().isSecondChan = true;
        // 카운트 멈춰주고.
        StopCoroutine(stromking);
        // 주인공 체력 회복 시키고.
        string maxHP = PlayerPrefsManager.GetInstance().Mat_MaxHP;
        PlayerPrefsManager.GetInstance().Mat_currentHP = maxHP;

        fiilAmount.fillAmount = 1f;
        hp_text.text = UserWallet.GetInstance().SeetheNatural(double.Parse(maxHP)) + "/" + UserWallet.GetInstance().SeetheNatural(double.Parse(maxHP));

        // 카운트 다운 온!
        PreparedPanel.SetActive(true);
        // 3 2 1카운트 다운
        StartCoroutine(CountTimer());

    }


    #region <Rewarded Ads> 이어하기 광고로 보기

    public void YouGoSO()
    {

        int vvip = PlayerPrefsManager.GetInstance().VIP;
        //PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);
        if (vvip == 526 || vvip == 625 || vvip == 725 || vvip == 925)
        {
            YouGoSOAds();
            return;
        }

        if (Advertising.IsRewardedAdReady(RewardedAdNetwork.AdMob, AdPlacement.Default))
        {
            Advertising.RewardedAdCompleted += YouGoSOAdsCompleated;
            Advertising.RewardedAdSkipped += YouGoSOAdsSkipped;

            /// 애드몹 미디에이션 동영상 2순위
            Advertising.ShowRewardedAd(RewardedAdNetwork.AdMob, AdPlacement.Default);
        }
        else
        {
            PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("광고를 준비중입니다. 잠시 후에 시도해주세요.");
        }

    }

    // Event handler called when a rewarded ad has completed
    void YouGoSOAdsCompleated(RewardedAdNetwork network, AdPlacement location)
    {
        Invoke("YouGoSOAds", 0.5f);
        Advertising.RewardedAdCompleted -= YouGoSOAdsCompleated;
        Advertising.RewardedAdSkipped -= YouGoSOAdsSkipped;
    }

    // Event handler called when a rewarded ad has been skipped
    void YouGoSOAdsSkipped(RewardedAdNetwork network, AdPlacement location)
    {
        Advertising.RewardedAdCompleted -= YouGoSOAdsCompleated;
        Advertising.RewardedAdSkipped -= YouGoSOAdsSkipped;

        PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);

    }

    void YouGoSOAds()
    {
        // 팝업 닫아주기.
        PopUpObjectManager.GetInstance().HideMuganCountinue();

        // 이어하기 썼다.
        PlayerPrefsManager.GetInstance().isSecondChan = true;
        // 카운트 멈춰주고.
        StopCoroutine(stromking);
        // 주인공 체력 회복 시키고.
        string maxHP = PlayerPrefsManager.GetInstance().Mat_MaxHP;
        PlayerPrefsManager.GetInstance().Mat_currentHP = maxHP;

        fiilAmount.fillAmount = 1f;
        hp_text.text = UserWallet.GetInstance().SeetheNatural(double.Parse(maxHP)) + "/" + UserWallet.GetInstance().SeetheNatural(double.Parse(maxHP));

        // 카운트 다운 온!
        PreparedPanel.SetActive(true);
        // 3 2 1카운트 다운
        StartCoroutine(CountTimer());

        PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);

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
        }

    }

    #endregion





    /// <summary>
    /// 그만두기 버튼 누른다
    /// </summary>
    public void EndBtnClicked()
    {
        PlayerPrefsManager.GetInstance().isMuGanTopEnd = true;

        //타이머 초기화.
        StopCoroutine(stromking);
        //실드 꺼져
        Left.SetActive(false);
        Right.SetActive(false);

        PopUpObjectManager.GetInstance().ShowWarnnigProcess("무한의 탑 도전실패.");
        //퀘스트
        PlayerPrefsManager.GetInstance().questInfo2[0].All_Mugan++;
        isStart = false;

        Invoke("InvoBackHome", 1.0f);

    }

    void InvoBackHome()
    {
        gotoMINIgame.ChangeCamToHomeMuGan();
    }


    bool isLeftBtnClicked;
    bool isRightBtnClicked;
    /// <summary>
    /// 버튼 컴포넌트 말고 트리거로 쓸 것. 왼쪽
    /// </summary>
    public void LeftBtn_Down()
    {
        // 오른쪽 버튼 눌린 상태면
        if (isRightBtnClicked) RightBtn_Up();

        if (isWaiting) return;

        Left.SetActive(true);
        isLeftBtnClicked = true;
    }
    public void LeftBtn_Up()
    {
        if (isRightBtnClicked) return;

        Left.SetActive(false);
        isLeftBtnClicked = false;
        isWaiting = true;
        Invoke("InvoWaiting", 0.5f);

    }

    /// <summary>
    /// 버튼 컴포넌트 말고 트리거로 쓸 것. 오른쪽
    /// </summary>
    public void RightBtn_Down()
    {
        // 왼쪽 버튼 눌린 상태면
        if (isLeftBtnClicked) LeftBtn_Up();

        if (isWaiting) return;


        Right.SetActive(true);
        isRightBtnClicked = true;
    }
    public void RightBtn_Up()
    {
        if (isLeftBtnClicked) return;

        Right.SetActive(false);
        isRightBtnClicked = false;
        isWaiting = true;
        Invoke("InvoWaiting", 0.5f);

    }
    bool isWaiting;
    /// <summary>
    /// 대기 걸어준다.
    /// </summary>
    void InvoWaiting()
    {
        isWaiting = false;
    }

    void FixedUpdate()
    {
        if (isStart && !PlayerPrefsManager.GetInstance().isMuGanTopEnd)
        {
            // 펀치시작
            tapToSpawnLimit.ClickedMuGanTop();
        }

    }
}
