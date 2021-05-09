using EasyMobile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DenfenceMode : MonoBehaviour
{
    public TutorialMissionManager tmm;

    public SpriteRenderer HitBody;
    public Image FilledHp;
    public Image Timer;
    public Text CountText;
    public Button GiveUPBtn;
    public Button StartBtn;
    public GameObject PreparedPanel;
    public TapToSpawnLimit tapToSpawnLimit;

    private void Start()
    {
        unbiasedTimerEndTimestamp = ReadTimestamp("DenfenceMode", UnbiasedTime.Instance.Now());
        unbiasedRemaining = unbiasedTimerEndTimestamp - UnbiasedTime.Instance.Now();

        if (unbiasedRemaining.TotalSeconds > 0)
        {
            isCoolOn = true;
            CoolTimeText.gameObject.SetActive(true);
            CoolTimeCover.gameObject.SetActive(true);
            popUpTime2.gameObject.SetActive(true);
        }
        else
        {
            popUpTime2.gameObject.SetActive(false);
        }


    }

    /// <summary>
    /// 방어전 노란 버튼 누르면 호출
    /// </summary>
    public void ShowDefencePop()
    {
        int thisWeapon = 0;

        for (int i = 0; i < PlayerPrefsManager.GetInstance().punchAmont; i++)
        {
            // i 는 다음에 해금할 무기
            if (!PlayerPrefsManager.GetInstance().weaponInfo[i].isUnlock)
            {
                thisWeapon = i;
                break;
            }
            else // 무기 다 해금함?
            {
                if (i == PlayerPrefsManager.GetInstance().punchAmont - 1)
                {
                    thisWeapon = 526;
                    break;
                }
            }

        }

        PopUpObjectManager.GetInstance().ShowOldPunch(thisWeapon - 1);
    }

    /// <summary>
    /// 씬 입장시 초기화.
    /// </summary>
    public void CameraOn()
    {
        // 
        isPunchStart = false;
        // 카운트 텍스트 초기화
        CountText.text = "";
        StartBtn.gameObject.SetActive(true);
        PreparedPanel.SetActive(true);
        // 처음 히트 바디 상태로
        HitBody.enabled = true;
        HitBody.GetComponent<Animation>().Stop();
        //
        HitBody.transform.GetChild(1).gameObject.SetActive(false);
        HitBody.transform.GetChild(2).gameObject.SetActive(false);

        var hphpMax = PlayerPrefsManager.GetInstance().Mat_MaxHP;
        var hphpCur = PlayerPrefsManager.GetInstance().Mat_currentHP;
        //FilledHp.fillAmount = 1f;
        //FilledHp.GetComponentInChildren<Text>().text = UserWallet.GetInstance().SeetheNatural(hphpMax) + "/" + UserWallet.GetInstance().SeetheNatural(hphpMax);

        DefenceStart();
    }


    /// <summary>
    /// 스타트 <버튼> 누르면 초기값 세팅
    /// </summary>
    public void DefenceStart()
    {
        // 쿨타임 돌고 있으면?
        if (isCoolOn)
        {
            PopUpObjectManager.GetInstance().ShowNewDefencePop();
            return;
        }

        // 타이머 초기화.
        unbiasedTimerEndTimestamp = ReadTimestamp("DenfenceMode", UnbiasedTime.Instance.Now());
        unbiasedRemaining = unbiasedTimerEndTimestamp - UnbiasedTime.Instance.Now();

        if (unbiasedRemaining.TotalSeconds > 0)
        {
            // 시간 남아있으면 타이머 온 해준다.
            isCoolOn = true;
            CoolTimeText.gameObject.SetActive(true);
            popUpTime2.gameObject.SetActive(true);

        }

        BodyRecovering();

        StartBtn.gameObject.SetActive(false);
        // 타이머 표시
        Timer.transform.parent.gameObject.SetActive(true);

        float cnt = 20f;
        cnt -= PlayerPrefsManager.GetInstance().Arti_DefenceTime * 0.1f;
        Timer.fillAmount = 1;
        /// 번역
        if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
            Timer.GetComponentInChildren<Text>().text = "남은 시간 : " + string.Format("{0:f1}", cnt);
        else
            Timer.GetComponentInChildren<Text>().text = "Time remaining : " + string.Format("{0:f1}", cnt);


        // 카운트 다운
        StartCoroutine(CountTimer());
    }

    /// <summary>
    /// 씬 입장시 히트바디 활성화.
    /// </summary>
    private void BodyRecovering()
    {
        HitBody.enabled = true;
        HitBody.GetComponent<Animation>().Stop();
        //
        HitBody.transform.GetChild(1).gameObject.SetActive(false);
        HitBody.transform.GetChild(2).gameObject.SetActive(false);
        //
        string hphp = PlayerPrefsManager.GetInstance().Mat_MaxHP;
        PlayerPrefsManager.GetInstance().Mat_currentHP = PlayerPrefsManager.GetInstance().Mat_MaxHP;
        FilledHp.fillAmount = 1f;
        FilledHp.GetComponentInChildren<Text>().text = UserWallet.GetInstance().SeetheNatural(double.Parse(hphp)) + "/" + UserWallet.GetInstance().SeetheNatural(double.Parse(hphp));
    }

    /// <summary>
    /// 대기화면 카운트다운
    /// </summary>
    /// <returns></returns>
    IEnumerator CountTimer()
    {
        int cnt = 3;
        while (cnt > 0)
        {
            CountText.text = cnt.ToString();
            yield return new WaitForSeconds(1);
            cnt--;
        }

        PreparedPanel.SetActive(false);
        stromking = StartCoroutine(PunchStorm());
    }

    Coroutine stromking;
    /// <summary>
    /// 20초 동안 초당 15회 가격 fixedUpdate
    /// </summary>
    /// <returns></returns>
    IEnumerator PunchStorm()
    {
        // 오토 힐링
        groggyManager.HP_AutoRecoForDef(true);
        isPunchStart = true;

        float cnt = 20f;
        /// 방어전 시간 감소 유물 적용
        cnt -= PlayerPrefsManager.GetInstance().Arti_DefenceTime * 0.1f;


        var cntMax = cnt;

        while (cnt > 0)
        {
            yield return new WaitForSeconds(0.05f);

            cnt -= 0.05f;

            Timer.fillAmount = cnt / cntMax;
            /// 번역
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                Timer.GetComponentInChildren<Text>().text = "남은 시간 : " + string.Format("{0:f1}", cnt);
            else
                Timer.GetComponentInChildren<Text>().text = "Time remaining : " + string.Format("{0:f1}", cnt);
        }

        /// 20초 동안 버티면 성공
        StopCoroutine(stromking);
        DefenceSuccess();
    }

    public PunchManager punchManager;
    public GotoMINIgame gotoMINIgame;


    /// <summary>
    /// 방어전 성공 보상 지급
    /// </summary>
    public void DefenceSuccess()
    {
        isPunchStart = false;
        CoolTimeCover.gameObject.SetActive(true);
        PopUpObjectManager.GetInstance().ShowNewPunch(PlayerPrefsManager.GetInstance().PunchIndex);
        // 다음 레벨 무기 해제 시켜줌.
        punchManager.UnlockNextPunch();
        tmm.ExUpdateMission(21); /// 미션 업데이트
        tmm.ExUpdateMission(29); /// 미션 업데이트
        tmm.ExUpdateMission(41); /// 미션 업데이트
        tmm.ExUpdateMission(48); /// 미션 업데이트
        tmm.ExUpdateMission(55); /// 미션 업데이트
        tmm.ExUpdateMission(61); /// 미션 업데이트
        tmm.ExUpdateMission(71); /// 미션 업데이트
        tmm.ExUpdateMission(82); /// 미션 업데이트
        tmm.ExUpdateMission(89); /// 미션 업데이트

        //
        CoolDownStart(300);
        groggyManager.HP_AutoRecoForDef(false);
        Timer.transform.parent.gameObject.SetActive(false);
        StopCoroutine(stromking);

        StartCoroutine(InvoReturnMainGame());
    }

    /// <summary>
    /// 테스트용 
    /// </summary>
    public void TestSuccess()
    {
        DefenceSuccess();
    }


    /// <summary>
    /// 방어전 실패했다.
    /// </summary>
    public void DefenceFailed()
    {
        isPunchStart = false;
        CoolTimeCover.gameObject.SetActive(true);
        PopUpObjectManager.GetInstance().ShowWarnnigProcess("방어실패. 충분히 성장 후 도전해 주세요.");
        Timer.transform.parent.gameObject.SetActive(false);

        CoolDownStart(300);
        groggyManager.HP_AutoRecoForDef(false);
        StopCoroutine(stromking);



        StartCoroutine(InvoReturnMainGame());

    }

    bool isRating;
    IEnumerator InvoReturnMainGame()
    {
        yield return new WaitForSeconds(1.5f);

        //
        if (PlayerPrefsManager.GetInstance().weaponInfo[2].isUnlock && !isRating)
        {
            // 2강 하면 리뷰좀
            StoreReview.RequestRating();
            isRating = true;
        }
        PlayerPrefsManager.GetInstance().Mat_currentHP = PlayerPrefsManager.GetInstance().Mat_MaxHP;
        gotoMINIgame.DefenceModeOff();
        tapToSpawnLimit.PunchIndexUpdate(PlayerPrefsManager.GetInstance().PunchIndex);
    }


    /// <summary>
    /// 포기하기
    /// </summary>
    public void StopTheStorm()
    {
        DefenceFailed();
    }

    public GroggyManager groggyManager;
    public Text CoolTimeText;
    public Image CoolTimeCover;
    // 조작 불가 타이머 스탬프
    DateTime unbiasedTimerEndTimestamp;
    TimeSpan unbiasedRemaining;

    bool isPunchStart;

    void FixedUpdate()
    {
        if (isPunchStart)
        {
            // 펀치시작
            tapToSpawnLimit.ClickedDefenceBody();
        }

        /// 버티기 실패하고 사망.
        if (PlayerPrefsManager.GetInstance().DefendTrigger == 1)
        {
            StopCoroutine(stromking);
            DefenceFailed();
            //
            PlayerPrefsManager.GetInstance().DefendTrigger = 0;
        }

        // 쿨타임 돌때
        if (isCoolOn)
        {
            // 시간 소모 
            unbiasedRemaining = unbiasedTimerEndTimestamp - UnbiasedTime.Instance.Now();
            WriteTimestamp("DenfenceMode", unbiasedTimerEndTimestamp);

            if (unbiasedRemaining.TotalSeconds > 0)
            {
                // 온 받은 순간부터 소모 타이머 카운트 다운
                CoolTimeText.text = string.Format("{0:00}:{1:00}", unbiasedRemaining.Minutes, unbiasedRemaining.Seconds);
                /// 번역
                if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                {
                    popUpTime.text = "남은 시간 " + string.Format("{0:00}:{1:00}", unbiasedRemaining.Minutes, unbiasedRemaining.Seconds);
                    popUpTime2.text = "남은 시간 " + string.Format("{0:00}:{1:00}", unbiasedRemaining.Minutes, unbiasedRemaining.Seconds);
                }
                else
                {
                    popUpTime.text = "Time remaining " + string.Format("{0:00}:{1:00}", unbiasedRemaining.Minutes, unbiasedRemaining.Seconds);
                    popUpTime2.text = "Time remaining " + string.Format("{0:00}:{1:00}", unbiasedRemaining.Minutes, unbiasedRemaining.Seconds);
                }

            }
            else // 쿨타임 끝
            {
                CoolTimeCover.gameObject.SetActive(false);
                CoolTimeText.gameObject.SetActive(false);
                popUpTime2.gameObject.SetActive(false);
                isCoolOn = false;
            }
        }
    }


    #region <Rewarded Ads> 방어전 초기화 광고 스킵 

    public void TEST_ADS_SKIP_COOLTIME()
    {
        //PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);

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
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("광고를 준비중입니다. 잠시 후에 시도해주세요.");
            PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);

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
        PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);
        //
        CoolTimeCover.gameObject.SetActive(false);
        CoolTimeText.gameObject.SetActive(false);
        popUpTime2.gameObject.SetActive(false);

        isCoolOn = false;
        unbiasedRemaining = unbiasedTimerEndTimestamp - unbiasedTimerEndTimestamp;

        PopUpObjectManager.GetInstance().HideNewDefencePop();

        gotoMINIgame.DefenceModeOn();

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
        //
    }

    #endregion




    DoubleToStringNum dts = new DoubleToStringNum();
    /// <summary>
    /// 다이아몬드 소모 로직 넣었을때 통과하냐??
    /// </summary>
    /// <returns></returns>
    bool DiaPass()
    {
        //var dia = PlayerPrefsManager.GetInstance().diamond;
        //var result = dts.SubStringDouble(dia, "100");

        string result = "";

        if (PlayerPrefs.GetFloat("dDiamond") - 100 < 0)
        {
            result = "-1";
        }

        if (result != "-1")
        {

            return true;
        }
        else
        {
            return false;
        }

    }


    /// <summary>
    /// 다이아 받고 방어전 쿨타임 초기화
    /// </summary>
    public void Test_Reset_CoolTime()
    {
        if (DiaPass())
        {
            // 다이아 소모
            //var dia = PlayerPrefsManager.GetInstance().diamond;
            //PlayerPrefsManager.GetInstance().diamond = dts.SubStringDouble(dia, "100");

            PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 100);
            UserWallet.GetInstance().ShowUserDia();

        }
        else
        {
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("보유 다이아가 부족합니다.");
            return;
        }

        CoolTimeCover.gameObject.SetActive(false);
        CoolTimeText.gameObject.SetActive(false);
        popUpTime2.gameObject.SetActive(false);

        isCoolOn = false;
        unbiasedRemaining = unbiasedTimerEndTimestamp - unbiasedTimerEndTimestamp;
        //
        PopUpObjectManager.GetInstance().HideNewDefencePop();

        gotoMINIgame.DefenceModeOn();
    }


    [Header("- 방어전 빨리빨리 팝업 쿨타임")]
    public Text popUpTime;
    public Text popUpTime2;
    bool isCoolOn;
    /// <summary>
    /// 방어전 끝났으면 쿨타임 걸어준다.
    /// </summary>
    /// <param name="_addTime"></param>
    void CoolDownStart(double _addTime)
    {
        isCoolOn = true;
        CoolTimeText.gameObject.SetActive(true);
        popUpTime2.gameObject.SetActive(true);

        //
        unbiasedTimerEndTimestamp = UnbiasedTime.Instance.Now().AddSeconds(_addTime);
        WriteTimestamp("DenfenceMode", unbiasedTimerEndTimestamp);
    }






    private DateTime ReadTimestamp(string key, DateTime defaultValue)
    {
        long tmp = Convert.ToInt64(PlayerPrefs.GetString(key, "0"));
        if (tmp == 0)
        {
            return defaultValue;
        }
        return DateTime.FromBinary(tmp);
    }

    private void WriteTimestamp(string key, DateTime time)
    {
        PlayerPrefs.SetString(key, time.ToBinary().ToString());
        PlayerPrefs.Save();
    }


}
