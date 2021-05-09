using EasyMobile;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GroggyManager : MonoBehaviour
{
    public TutorialMissionManager tmm;
    public GameObject LvUPObjt;
    [Header("- 우편함 갱신시간")]
    public float PostCheckDelay;
    [Header("- 오토 공격 어케댐")]
    public Booster_AUTO Auto; // 히트 바디
    [Header("- 빵돌이")]
    public Image BreadGauge; // 빵 버튼 게이지
    public Image TutorialGauge; // 빵 버튼 게이지
    [Header("-리지드 바디 부모객체")]
    public SpriteRenderer Body; // 히트 바디
    [Header("-HP 바")]
    public Image HP_Bar;
    public Image HP_Ba_Def;
    //[Header("-캐릭터 정보 바")]
    //public Transform[] ChraterInfo;
    [Header("-하단 게이지 바")]
    public Image Mattzip100;
    public Image Skill300;
    [Header("- 하단 버튼 - > 그로기 모드에서 버튼 클릭 안되게")]
    public GameObject GameObject01;
    public GameObject GameObject02;
    public GameObject GameObject03;
    public GameObject GameObject04;
    public GameObject GameObject05;
    public GameObject GameObject06;
    public GameObject GameObject07;
    public GameObject GameObject08;
    public GameObject PANEL;
    readonly int breadCnt; // 빵 30번 클릭할 것

    readonly string _Mattzip;


    /// <summary>
    /// 그로기 모드 진입하면 버튼 클릭 안되게 잠궈줌.
    /// </summary>
    /// <param name="_switch">true = 잠금 / false = 해제</param>
    public void GroggyModeImageLock(bool _switch)
    {
        GameObject01.SetActive(_switch);
        GameObject02.SetActive(_switch);
        GameObject03.SetActive(_switch);
        GameObject04.SetActive(_switch);
        GameObject05.SetActive(_switch);
        GameObject06.SetActive(_switch);
        GameObject07.SetActive(_switch);
        //GameObject08.SetActive(_switch);
        GameObject08.transform.parent.GetComponent<Button>().interactable = !_switch;

        PANEL.SetActive(_switch);
    }

    /// <summary>
    /// 게임 스타트 매니저에서 호출 할 것.
    /// </summary>
    public void HP_barInit()
    {
        var tmp1 = PlayerPrefsManager.GetInstance().Mat_currentHP;
        var tmp2 = PlayerPrefsManager.GetInstance().Mat_MaxHP;
        PlayerPrefsManager.GetInstance().Mat_currentHP = tmp2;
        var resultMax = UserWallet.GetInstance().SeetheNatural(double.Parse(tmp2));
        Debug.LogWarning("Mat_currentHP" + tmp1);
        Debug.LogWarning("Mat_MaxHP" + tmp2);

        HP_Bar.fillAmount = 1;
        HP_Bar.GetComponentInChildren<Text>().text = resultMax + "/" + resultMax;

        /// 껏다 키면 맷집 증가 게이지 복구
        Mat_100_Count();
        //PlayerPrefsManager.GetInstance().Mat_100 = 0;

        /// 껏다 키면 스킬게이지 초기화
        Skill300.fillAmount = 0;
        PlayerPrefsManager.GetInstance().Mat_Skill_300 = 0;
    }


    /// <summary>
    /// 일회용 맷집 증가 메소드
    /// </summary>
    void Mat_100_Count()
    {
        if (!PlayerPrefsManager.GetInstance().isFristGameStart) return;

        /// 누적 대미지 / 증가 필요 대미지
        Mattzip100.fillAmount = PlayerPrefsManager.GetInstance().Mat_100 / PlayerPrefsManager.GetInstance().Cilcked_Cnt_MattZip;
    }
    //Coroutine skillcd;
    //
    ///// <summary>
    ///// 외부에서 사용
    ///// </summary>
    ///// <param name="_Swich"></param>
    //public void SkillCoolDown(bool _Swich)
    //{
    //    if (_Swich)
    //    {
    //        skillcd = StartCoroutine(SkillDownForSeconds());
    //    }
    //    else
    //    {
    //        StopCoroutine(skillcd);
    //    }
    //}

    ///// <summary>
    ///// 스킬 게이지가 1이라도 있으면 1초에 1씩 감소
    ///// </summary>
    ///// <returns></returns>
    //IEnumerator SkillDownForSeconds()
    //{
    //    yield return null;

    //    for(; ; )
    //    {
    //        yield return new WaitForSeconds(1);

    //        if (PlayerPrefsManager.GetInstance().Mat_Skill_300 > 0)
    //        {
    //            PlayerPrefsManager.GetInstance().Mat_Skill_300--;
    //            Skill300.fillAmount = PlayerPrefsManager.GetInstance().Mat_Skill_300 / 300.0f;

    //        }
    //    }
    //}

    Coroutine HP_CD;
    Coroutine PostCheckC;

    /// <summary>
    /// 외부에서 사용
    /// </summary>
    /// <param name="_Swich"></param>
    public void HP_AutoReco(bool _Swich)
    {
        if (_Swich)
        {
            HP_CD = StartCoroutine(HP_AutoRecoForSeconds());
            PostCheckC = StartCoroutine(PostCheckCou());
        }
        else
        {
            StopCoroutine(HP_CD);
            StopCoroutine(PostCheckCou());
        }
    }


    /// <summary>
    /// 우편함 계속 갱신 5분 마다.
    /// </summary>
    /// <returns></returns>
    IEnumerator PostCheckCou()
    {
        yield return null;

        for (; ; )
        {
            yield return new WaitForSeconds(PostCheckDelay);

            GameObject.Find("PlayNanoo").GetComponent<PlayNANOOExample>().PostboxCheck();
        }
    }


    string Auto_currentHP;
    string Auto_maxHP;
    string Auto_tmp;
    double Auto_recov;

    /// <summary>
    ///  무한 모드일때 회복 중지.
    /// </summary>
    public GameObject MuGanCavas;
    /// <summary>
    /// 1초 마다 체력 회복
    /// </summary>
    /// <returns></returns>
    IEnumerator HP_AutoRecoForSeconds()
    {
        yield return null;

        for (; ; )
        {
            yield return new WaitForSeconds(0.05f);

            if (!PlayerPrefsManager.GetInstance().isFristGameStart || MuGanCavas.activeSelf) goto HELL;

            Auto_currentHP = PlayerPrefsManager.GetInstance().Mat_currentHP;
            Auto_maxHP = PlayerPrefsManager.GetInstance().Mat_MaxHP;
            Auto_tmp = dts.SubStringDouble(Auto_currentHP, Auto_maxHP);
            Auto_recov = dts.DevideStringDouble(PlayerPrefsManager.GetInstance().Mat_Recov, "10");

            if (Auto_recov < 1.1d) Auto_recov = 1.0d;

            if (Auto_currentHP == "-1") goto HELL;


            // 그로기 상태가 아니면 지속 회복
            if (!PlayerPrefsManager.GetInstance().isGroggy && Auto_tmp == "-1" || Auto_tmp == "0")
            {
                var tmpHP = dts.AddStringDouble(Auto_currentHP, Auto_recov.ToString("f0"));
                var tmpResult = dts.SubStringDouble(tmpHP, Auto_maxHP);

                if (tmpResult != "-1")
                {
                    PlayerPrefsManager.GetInstance().Mat_currentHP = Auto_maxHP;
                }
                else
                {
                    PlayerPrefsManager.GetInstance().Mat_currentHP = dts.AddStringDouble(Auto_currentHP, Auto_recov.ToString("f0"));
                }

                HP_Bar.fillAmount = (float)dts.DevideStringDouble(Auto_currentHP, Auto_maxHP);
                var dCurrentHP = dts.PanByulGi(Auto_currentHP);
                HP_Bar.GetComponentInChildren<Text>().text = UserWallet.GetInstance().SeetheNatural(dCurrentHP) + "/" + UserWallet.GetInstance().SeetheNatural(double.Parse(Auto_maxHP));
            }

        HELL:
            yield return new WaitForSeconds(0.05f);

        }
    }

    Coroutine HP_CDforDef;
    /// <summary>
    /// 외부에서 사용
    /// </summary>
    /// <param name="_Swich"></param>
    public void HP_AutoRecoForDef(bool _Swich)
    {
        if (_Swich)
        {
            HP_CDforDef = StartCoroutine(HP_AutoRecoDef());
        }
        else
        {
            StopCoroutine(HP_CDforDef);
        }
    }

    /// <summary>
    /// 1초 마다 체력 회복 디펜스 모드에서
    /// </summary>
    /// <returns></returns> 
    IEnumerator HP_AutoRecoDef()
    {
        yield return null;

        for (; ; )
        {
            yield return new WaitForSeconds(0.05f);

            var currentHP = PlayerPrefsManager.GetInstance().Mat_currentHP;
            var maxHP = PlayerPrefsManager.GetInstance().Mat_MaxHP;
            //
            var tmp = dts.SubStringDouble(currentHP, maxHP);
            //
            var recov = dts.DevideStringDouble(PlayerPrefsManager.GetInstance().Mat_Recov, "10");

            if (recov < 1.1d) recov = 1.0d;

            // 그로기 상태가 아니면 지속 회복
            if (!PlayerPrefsManager.GetInstance().isGroggy && tmp == "-1" || tmp == "0")
            {
                var tmpHP = dts.AddStringDouble(currentHP, recov.ToString("f0"));
                var tmpResult = dts.SubStringDouble(tmpHP, maxHP);

                if (tmpResult != "-1")
                {
                    PlayerPrefsManager.GetInstance().Mat_currentHP = maxHP;
                }
                else
                {
                    PlayerPrefsManager.GetInstance().Mat_currentHP = dts.AddStringDouble(currentHP, recov.ToString("f0"));
                }

                HP_Ba_Def.fillAmount = (float)dts.DevideStringDouble(currentHP, maxHP);
                var dCurrentHP = dts.PanByulGi(currentHP);
                HP_Ba_Def.GetComponentInChildren<Text>().text = UserWallet.GetInstance().SeetheNatural(dCurrentHP) + "/" + UserWallet.GetInstance().SeetheNatural(double.Parse(maxHP));
            }

            yield return new WaitForSeconds(0.05f);
        }
    }


    Coroutine groggyTimer;

    public Transform BAP_POS;
    public GameObject BAPfrefab;
    /// <summary>
    /// 그로기 일때 빵 버튼 터치 -> 그로기 시작시 5초 카운트
    /// </summary>
    public void BreadBtnTouch()
    {
        // 너 고소 팝업 타이머 시작
        groggyTimer = StartCoroutine(grogro());
        // 오토 꺼주기
        Auto.BuffImgOnOff(false);


        //var bullet = Lean.Pool.LeanPool.Spawn(BAPfrefab, BAP_POS.position, BAP_POS.rotation);
        //bullet.GetComponent<BulletManager>().BapInit();

        //if (!PlayerPrefsManager.GetInstance().isFristGameStart)
        //{
        //    //국밥 버튼 포커싱 켜주고
        //    GameObject.Find("TUTORIALManager").GetComponent<TutorialManager>().MainObject[7].SetActive(true);

        //    breadCnt++;
        //    TutorialGauge.fillAmount = breadCnt / 10f;

        //    Debug.LogWarning(breadCnt);

        //    if (breadCnt >= 10f)
        //    {
        //        GameObject.Find("TUTORIALManager").GetComponent<TutorialManager>().MainObject[7].SetActive(false);
        //        TutorialGauge.fillAmount = 1f;
        //        PlayerPrefsManager.GetInstance().TurtorialCount = 52525;
        //        GroggyOff();
        //    }
        //}
        //else
        //{
        //    // 너 고소 팝업 타이머 시작
        //    if (breadCnt == 0)
        //    {
        //        groggyTimer = StartCoroutine(grogro());
        //        // 오토 꺼주기
        //        Auto.BuffImgOnOff(false);
        //    }
        //    // 그로기 회복 버튼 터치부분
        //    var btncnt = PlayerPrefsManager.GetInstance().GroggyTouch;
        //    breadCnt++;
        //    BreadGauge.fillAmount = breadCnt / btncnt;

        //    if (breadCnt >= btncnt)
        //    {
        //        GroggyOff();
        //    }
        //}

    }

    /// <summary>
    /// 그로기 모드 풀어주는 메서드
    /// </summary>
    public void GroggyOff()
    {

        StopCoroutine(groggyTimer);


        // 고소 코루틴 풀어주고
        //if (breadCnt != 0 && PlayerPrefsManager.GetInstance().isFristGameStart) StopCoroutine(groggyTimer);
        //breadCnt = 0;
        //BreadGauge.fillAmount = 0.0f;

        // 맷집 표기 온.
        //BreadGauge.transform.parent.GetChild(1).gameObject.SetActive(true);
        UserWallet.GetInstance().ShowUserMatZip();
        //
        //BreadGauge.gameObject.SetActive(false);

        // 최근 체력을 30% 회복
        StartCoroutine(RecoverHP());
        timerImg.transform.parent.gameObject.SetActive(false);
    }

    Coroutine speedUPSkill;
    bool isCRrunning;
    /// <summary>
    /// 국밥 힐링 모드 + 유물 영향 받음
    /// </summary>
    public void HeallingOff()
    {
        // 코루틴 실행중이면 멈추고 시작.
        if (isCRrunning) StopCoroutine(speedUPSkill);

        AudioManager.instance.Btn_healing();

        speedUPSkill = StartCoroutine(InvoSpeedUp());
        //StartCoroutine(InvoHealing());
    }


    public Text GupSkillTimer;
    /// <summary>
    /// 스피드 업!
    /// </summary>
    /// <returns></returns>
    IEnumerator InvoSpeedUp()
    {
        isCRrunning = true;
        // 국밥 버프 속도 증가 유물
        float result = PlayerPrefsManager.GetInstance().Arti_GAL * 0.01f;
        float speedTime = 3.0f + result;

        /// 애니메이션 재생 1.5초
        yield return new WaitForSeconds(1.5f);

        PlayerPrefsManager.GetInstance().isGupSpeed = true;
        BodyRecoveringforHealing();

        /// 번역
        if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
        {
            GupSkillTimer.text = "남은 시간 : " + string.Format("{0:f1}", speedTime);
            GupSkillTimer.gameObject.SetActive(true);

            yield return null;

            while (speedTime > 0)
            {
                yield return new WaitForSeconds(0.05f);

                speedTime -= 0.05f;
                GupSkillTimer.text = "남은 시간 : " + string.Format("{0:f1}", speedTime);

            }
        }
        else
        {
            GupSkillTimer.text = "번역 : " + string.Format("{0:f1}", speedTime);
            GupSkillTimer.gameObject.SetActive(true);

            yield return null;

            while (speedTime > 0)
            {
                yield return new WaitForSeconds(0.05f);

                speedTime -= 0.05f;
                GupSkillTimer.text = "번역 : " + string.Format("{0:f1}", speedTime);

            }
        }

        PlayerPrefsManager.GetInstance().isGupSpeed = false;
        GupSkillTimer.gameObject.SetActive(false);

        isCRrunning = false;

    }

    /// <summary>
    /// 국밥 힐링
    /// </summary>
    /// <returns></returns>
    //IEnumerator InvoHealing()
    //{
    //    yield return null;
    //    // 최근 체력을 10% 채워줌
    //    string tmpMax = PlayerPrefsManager.GetInstance().Mat_MaxHP;

    //    double filledBody = dts.DevideStringDouble(tmpMax, "10");

    //    ///유물 효과 적용.
    //    float Gal_Effect = PlayerPrefsManager.GetInstance().Arti_GAL * 0.5f;
    //    filledBody = filledBody + (filledBody * Gal_Effect * 0.01f);
    //    //
    //    double fullBody = dts.PanByulGi(tmpMax);
    //    string currentHP = dts.fDoubleToStringNumber(filledBody);

    //    // 회복할 양 얼마?
    //    var filledBodyToHeal = dts.AddStringDouble(currentHP, PlayerPrefsManager.GetInstance().Mat_currentHP);
    //    Debug.LogWarning("회복할 양 " + filledBodyToHeal);
    //    // 체력 100% 넘은거 체크
    //    var tmpsdfsaf = dts.SubStringDouble(tmpMax, filledBodyToHeal);
    //    if (tmpsdfsaf == "-1") filledBodyToHeal = tmpMax;
    //    // 조금씩 회복할 량
    //    double progressBody = dts.PanByulGi(PlayerPrefsManager.GetInstance().Mat_currentHP);
    //    Debug.LogWarning("조금씩 회복할 양 " + progressBody);

    //    filledBody = filledBody * 0.01f;

    //    // 체력 표기~ 힐링 량 될때 까지
    //    while (HP_Bar.fillAmount < dts.DevideStringDouble(filledBodyToHeal, tmpMax))
    //    {
    //        yield return new WaitForFixedUpdate();

    //        progressBody += filledBody;
    //        HP_Bar.fillAmount = (float)(progressBody / fullBody);
    //        HP_Bar.GetComponentInChildren<Text>().text = 
    //            UserWallet.GetInstance().SeetheNatural(progressBody) + "/" 
    //            + UserWallet.GetInstance().SeetheNatural(double.Parse(tmpMax));
    //    }

    //    var tmpProgress = dts.fDoubleToStringNumber(progressBody);
    //    Debug.LogWarning("최종 회복량 " + tmpProgress);



    //    PlayerPrefsManager.GetInstance().Mat_currentHP = tmpProgress;

    //    yield return new WaitForSeconds(1.5f);

    //    BodyRecovering();
    ////}


    [Header("- 너 고 소")]
    public Image timerImg;


    /// <summary>
    /// 그로기 타이머
    /// </summary>
    /// <returns></returns>
    IEnumerator grogro()
    {

        float cnt = PlayerPrefsManager.GetInstance().GroggyTouch;
        float Maxcnt = cnt;

        timerImg.fillAmount = 1.0f;

        /// 번역
        if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
        {
            timerImg.GetComponentInChildren<Text>().text = "남은 시간 : " + string.Format("{0:f1}", cnt);

            yield return null;

            timerImg.transform.parent.gameObject.SetActive(true);

            while (cnt > 0)
            {
                yield return new WaitForSeconds(0.05f);

                cnt -= 0.05f;
                timerImg.fillAmount = cnt / Maxcnt;
                timerImg.GetComponentInChildren<Text>().text = "남은 시간 : " + string.Format("{0:f1}", cnt);

            }
        }
        else
        {
            timerImg.GetComponentInChildren<Text>().text = "번역 시간 : " + string.Format("{0:f1}", cnt);

            yield return null;

            timerImg.transform.parent.gameObject.SetActive(true);

            while (cnt > 0)
            {
                yield return new WaitForSeconds(0.05f);

                cnt -= 0.05f;
                timerImg.fillAmount = cnt / Maxcnt;
                timerImg.GetComponentInChildren<Text>().text = "번역 시간 : " + string.Format("{0:f1}", cnt);

            }
        }





        // 그로기 해제.
        GroggyOff();

        //PlayerPrefsManager.GetInstance().isGoingGOSO = true;
        //// 너 고소!!
        //PopUpObjectManager.GetInstance().ShowGoSoProcess();

        //GroggyOff();
    }

    /// <summary>
    /// 삭제된 컨텐츠
    /// </summary>
    public void YouGOSOforDia()
    {
        //var dia = PlayerPrefsManager.GetInstance().diamond;
        //var result = dts.SubStringDouble(dia, "10");
        string result = "";

        if (PlayerPrefs.GetFloat("dDiamond") - 10 < 0)
        {
            result = "-1";
        }

        if (result != "-1")
        {
            // 다이아 소모
            //PlayerPrefsManager.GetInstance().diamond = result;

            PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 20);

            UserWallet.GetInstance().ShowUserDia();
            //
            PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);

            PopUpObjectManager.GetInstance().ShowWarnnigProcess("원만하게 합의하였습니다.");

            GroggyOff();

            PopUpObjectManager.GetInstance().HIdeGoSoProcess();

        }
        else
        {
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("보유 다이아가 부족합니다.");
        }
    }

    #region <Rewarded Ads> 너 고소 다이아대신 동영상 광고

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

        PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);

        PopUpObjectManager.GetInstance().HIdeGoSoProcess();

        PopUpObjectManager.GetInstance().ShowWarnnigProcess("원만하게 합의하였습니다.");

        GroggyOff();


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






    readonly DoubleToStringNum dts = new DoubleToStringNum();
    /// <summary>
    /// 그로기 상태 회복할때 회복한다
    /// </summary>
    /// <returns></returns>
    IEnumerator RecoverHP()
    {
        yield return null;

        AudioManager.instance.Btn_healing();

        string tmpMax = PlayerPrefsManager.GetInstance().Mat_MaxHP;

        double filledBody = dts.DevideStringDouble(PlayerPrefsManager.GetInstance().Mat_MaxHP, "100");
        double progressBody = 0.0f;
        double fullBody = dts.PanByulGi(PlayerPrefsManager.GetInstance().Mat_MaxHP);

        // 맥스 
        while (HP_Bar.fillAmount < 0.3f)
        {
            yield return new WaitForFixedUpdate();

            progressBody += filledBody;
            HP_Bar.fillAmount = (float)(progressBody / fullBody);
            HP_Bar.GetComponentInChildren<Text>().text = UserWallet.GetInstance().SeetheNatural(progressBody) + "/" + UserWallet.GetInstance().SeetheNatural(double.Parse(tmpMax));
        }
        var tmpProgress = dts.fDoubleToStringNumber(progressBody);


        //0.3 피
        PlayerPrefsManager.GetInstance().Mat_currentHP = tmpProgress;
        //HP_Bar.GetComponentInChildren<Text>().text = UserWallet.GetInstance().SeetheNatural(tmpProgress) + "/" + UserWallet.GetInstance().SeetheNatural(tmpMax);
        // 히트 바디 이미지 표기 다시
        BodyRecovering();
    }


    /// <summary>
    /// 가드 올려 이미지로 복귀.
    /// </summary>
    public void BodyRecovering()
    {
        Body.enabled = true;
        Body.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        Body.GetComponent<Animation>().Stop();
        Body.GetComponent<Animation>()["Healling"].speed = 0;
        //
        Body.transform.GetChild(1).gameObject.SetActive(false);
        Body.transform.GetChild(2).gameObject.SetActive(false);
        Body.transform.GetChild(3).gameObject.SetActive(false);
        // 그로기 풀어준다.
        PlayerPrefsManager.GetInstance().isGroggy = false;
        GroggyModeImageLock(false);

        if (!PlayerPrefsManager.GetInstance().isFristGameStart) PlayerPrefsManager.GetInstance().TurtorialCount = 52525;
    }


    /// <summary>
    /// 힐링 복귀
    /// </summary>
    public void BodyRecoveringforHealing()
    {
        Body.enabled = true;
        Body.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        Body.GetComponent<Animation>().Stop();
        Body.GetComponent<Animation>()["Healling"].speed = 0;
        //
        Body.transform.GetChild(1).gameObject.SetActive(false);
        Body.transform.GetChild(2).gameObject.SetActive(false);
        Body.transform.GetChild(3).gameObject.SetActive(false);
        // 그로기 풀어준다.
        PlayerPrefsManager.GetInstance().isGroggy = false;
    }




    [Header("-강화 텍스트 박스")]
    public Text POWER_UP_TEXT;
    public Text POWER_UP_Price;
    public Text POWER_UP_LV;

    public Text HP_UP_TEXT;
    public Text HP_UP_Price;
    public Text HP_UP_LV;

    public Text Recov_UP_TEXT;
    public Text Recov_UP_Price;
    public Text Recov_UP_LV;

    public Text Mat_HP_UP_TEXT;
    public Text Mat_HP_UP_Price;
    public Text Mat_HP_UP_LV;

    //public Text ATK_PER_UP_TEXT;
    //public Text ATK_PER_UP_Price;
    //public Text ATK_PER_UP_LV;

    //public Text HP_PER_UP_TEXT;
    //public Text HP_PER_UP_Price;
    //public Text HP_PER_UP_LV;

    public Text Dia_ATK_PER_UP_TEXT;
    public Text Dia_ATK_PER_UP_Price;
    public Text Dia_ATK_PER_UP_LV;

    public Text Dia_HP_PER_UP_TEXT;
    public Text Dia_HP_PER_UP_Price;
    public Text Dia_HP_PER_UP_LV;

    //0615

    public Text CRC_UP_TEXT;
    public Text CRC_UP_Price;
    public Text CRC_UP_LV;

    public Text CRD_UP_TEXT;
    public Text CRD_UP_Price;
    public Text CRD_UP_LV;

    public Text Dia_CRC_UP_TEXT;
    public Text Dia_CRC_UP_Price;
    public Text Dia_CRC_UP_LV;

    public Text Dia_CRD_UP_TEXT;
    public Text Dia_CRD_UP_Price;
    public Text Dia_CRD_UP_LV;

    //0709



    public Text Dia_Recov_Per_UP_TEXT;
    public Text Dia_Recov_Per_UP_Price;
    public Text Dia_Recov_Per_UP_LV;


    [Header("-버튼 회색으로 덮기")]
    public GameObject POWER_UP_Gray;
    public GameObject HP_UP_Gray;
    public GameObject Recov_UP_Gray;
    public GameObject Mat_HP_UP_Gray;

    //public GameObject ATK_PER_UP_Gray;
    //public GameObject HP_PER_UP_Gray;

    public GameObject Dia_ATK_PER_UP_Gray;
    public GameObject Dia_HP_PER_UP_Gray;

    // 0615

    public GameObject CRC_UP_Gray;
    public GameObject CRD_UP_Gray;

    public GameObject Dia_CRC_UP_Gray;
    public GameObject Dia_CRD_UP_Gray;

    public GameObject Dia_Recov_Per_UP_Gray;

    [Header("-맥스 버튼 덮기")]
    public GameObject POWER_UP_Max;
    public GameObject HP_UP_Max;
    public GameObject Recov_UP_Max;
    public GameObject Mat_HP_UP_May;

    //public GameObject ATK_PER_UP_Max;
    //public GameObject HP_PER_UP_Max;

    public GameObject Dia_ATK_PER_UP_Max;
    public GameObject Dia_HP_PER_UP_Max;

    // 0615

    public GameObject CRC_UP_Max;
    public GameObject CRD_UP_Max;

    public GameObject Dia_CRC_UP_Max;
    public GameObject Dia_CRD_UP_Max;

    public GameObject Dia_Recov_Per_UP_Max;

    [Header("- 방어력 특별 강화")]
    public Text Dia_Deffence_Per_UP_TEXT;
    public Text Dia_Deffence_Per_UP_Price;
    public Text Dia_Deffence_Per_UP_LV;
    public GameObject Dia_Deffence_Per_UP_Gray;
    public GameObject Dia_Deffence_Per_UP_Max;

    string currentAtk;
    string nextAtk;
    string currentHP;
    string nextHP;
    string currentRec;
    string nextRec;

    //string currentAtkPer;
    //string nextAtkPer;
    //string currentHP_Per;
    //string nextHP_Per;

    //string diacurrentAtkPer;
    //string dianextAtkPer;
    //string diacurrentHP_Per;
    //string dianextHP_Per;


    bool isBtnDown1;
    bool isBtnDown2;
    bool isBtnDown3;
    bool isBtnDown4;

    bool isBtnDown5;
    bool isBtnDown6;

    bool isBtnDown7;
    bool isBtnDown8;

    // 0611

    bool isBtnDown9;
    bool isBtnDown10;

    bool isBtnDown11;
    bool isBtnDown12;

    // 0709

    bool isBtnDown13; // 골드 체력 회복
    bool isBtnDown14; // 다이아 체력 회복


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
    /// <summary>
    /// BtnDown2()
    /// </summary>
    public void BtnDown2()
    {
        Invoke("InvoDown2", 0.3f);
    }
    void InvoDown2()
    {
        isBtnDown2 = true;
    }
    public void BtnUP2()
    {
        CancelInvoke("InvoDown2");
        isBtnDown2 = false;
    }

    /// <summary>
    /// BtnUP3()
    /// </summary>
    public void BtnDown3()
    {
        Invoke("InvoDown3", 0.3f);
    }
    void InvoDown3()
    {
        isBtnDown3 = true;
    }
    public void BtnUP3()
    {
        CancelInvoke("InvoDown3");
        isBtnDown3 = false;
    }

    /// <summary>
    /// BtnDown4()
    /// </summary>
    public void BtnDown4()
    {
        Invoke("InvoDown4", 0.3f);
    }
    void InvoDown4()
    {
        isBtnDown4 = true;
    }
    public void BtnUP4()
    {
        CancelInvoke("InvoDown4");
        isBtnDown4 = false;
    }


    /// <summary>
    /// BtnDown4()
    /// </summary>
    public void BtnDown5()
    {
        Invoke("InvoDown5", 0.3f);
    }
    void InvoDown5()
    {
        isBtnDown5 = true;
    }
    public void BtnUP5()
    {
        CancelInvoke("InvoDown5");
        isBtnDown5 = false;
    }


    /// <summary>
    /// BtnDown4()
    /// </summary>
    public void BtnDown6()
    {
        Invoke("InvoDown6", 0.3f);
    }
    void InvoDown6()
    {
        isBtnDown6 = true;
    }
    public void BtnUP6()
    {
        CancelInvoke("InvoDown6");
        isBtnDown6 = false;
    }





    /// <summary>
    /// BtnDown7()
    /// </summary>
    public void BtnDown7()
    {
        Invoke("InvoDown7", 0.3f);
    }
    void InvoDown7()
    {
        isBtnDown7 = true;
    }
    public void BtnUP7()
    {
        CancelInvoke("InvoDown7");
        isBtnDown7 = false;
    }


    /// <summary>
    /// BtnDown8()
    /// </summary>
    public void BtnDown8()
    {
        Invoke("InvoDown8", 0.3f);
    }
    void InvoDown8()
    {
        isBtnDown8 = true;
    }
    public void BtnUP8()
    {
        CancelInvoke("InvoDown8");
        isBtnDown8 = false;
    }


    ///////////////////////////////////////////////////////////////////////
    ///
    ///
    /// 0615 9 ~ 12
    ///
    ///
    ///////////////////////////////////////////////////////////////////////



    /// <summary>
    /// BtnDown9
    /// </summary>
    public void BtnDown9()
    {
        Invoke("InvoDown9", 0.3f);
    }
    void InvoDown9()
    {
        isBtnDown9 = true;
    }
    public void BtnUP9()
    {
        CancelInvoke("InvoDown9");
        isBtnDown9 = false;
    }

    /// <summary>
    /// BtnDown10
    /// </summary>
    public void BtnDown10()
    {
        Invoke("InvoDown10", 0.3f);
    }
    void InvoDown10()
    {
        isBtnDown10 = true;
    }
    public void BtnUP10()
    {
        CancelInvoke("InvoDown10");
        isBtnDown10 = false;
    }

    /// <summary>
    /// BtnDown11
    /// </summary>
    public void BtnDown11()
    {
        Invoke("InvoDown11", 0.3f);
    }
    void InvoDown11()
    {
        isBtnDown11 = true;
    }
    public void BtnUP11()
    {
        CancelInvoke("InvoDown11");
        isBtnDown11 = false;
    }

    /// <summary>
    /// BtnDown12
    /// </summary>
    public void BtnDown12()
    {
        Invoke("InvoDown12", 0.3f);
    }
    void InvoDown12()
    {
        isBtnDown12 = true;
    }
    public void BtnUP12()
    {
        CancelInvoke("InvoDown12");
        isBtnDown12 = false;
    }


    /// <summary>
    /// BtnDown13
    /// </summary>
    public void BtnDown13()
    {
        Invoke("InvoDown13", 0.3f);
    }
    void InvoDown13()
    {
        isBtnDown13 = true;
    }
    public void BtnUP13()
    {
        CancelInvoke("InvoDown13");
        isBtnDown13 = false;
    }

    /// <summary>
    /// BtnDown12
    /// </summary>
    public void BtnDown14()
    {
        Invoke("InvoDown14", 0.3f);
    }
    void InvoDown14()
    {
        isBtnDown14 = true;
    }
    public void BtnUP14()
    {
        CancelInvoke("InvoDown14");
        isBtnDown14 = false;
    }

    /// <summary>
    /// 계속 누르면 강화.
    /// </summary>
    private void FixedUpdate()
    {
        if (isBtnDown1) TEST_Power_UP();
        if (isBtnDown2) TEST_HP_UP();
        if (isBtnDown3) TEST_Recov_UP();
        if (isBtnDown4) TEST_Mattzip_up();

        //if (isBtnDown5) TEST_ATK_PER_UP();
        //if (isBtnDown6) TEST_HP_PER_UP();

        if (isBtnDown7) DIA_ATK_PER_UP();
        if (isBtnDown8) DIA_HP_PER_UP();

        // 0615

        if (isBtnDown9) CRC_UP();
        if (isBtnDown10) CRD_UP();

        if (isBtnDown11) DIA_CRC_UP();
        if (isBtnDown12) DIA_CRD_UP();


        if (isBtnDown14) DIA_RECOV_UP();

        /// 방어력 특별 강화
        if (isBtnDown13) Dia_Deffence_UP();

    }


    int PowerLv;
    int HP_Lv;
    int Rec_Lv;
    int Defend_Lv;
    int ATK_PER_UP_Lv;
    int HP_PER_UP_Lv;
    int Dia_ATK_PER_UP_Lv;
    int Dia_HP_PER_UP_Lv;

    // 0615

    int CRC_Lv;
    int CRD_Lv;
    int Dia_CRC_Lv;
    int Dia_CRD_Lv;

    int Dia_Deffence_Lv;
    int Dia_HPPER_Lv;



    /// <summary>
    /// 골드 소모 CSV 삭제하고 공식으로 대체
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    string GetNormalUpPrice(int index, int lv)
    {
        string _result = "";
        double _defalt = 0;

        switch (index)
        {
            case 0: // 공격력 골드 강화
                _defalt = 10.0d; // 디폴트 값
                if (lv != 0) _defalt *= Math.Pow(1.05, lv);
                _result = _defalt.ToString("f0");

                break;


            case 1: // 체력 골드 강화
                _defalt = 25.0d; // 디폴트 값
                if (lv != 0) _defalt *= Math.Pow(1.07, lv);
                _result = _defalt.ToString("f0");
                break;


            case 2: // 체력회복 골드 강화
                _defalt = 7.0d; // 디폴트 값
                if (lv != 0) _defalt *= Math.Pow(1.05, lv);
                _result = _defalt.ToString("f0");
                break;


            /// 50 * ( 1 + 0.07 ) ^ Lv   
            case 3: // 방어력  골드 강화 
                _defalt = 50.0d; // 디폴트 값
                if (lv != 0) _defalt *= Math.Pow(1.07, lv);
                _result = _defalt.ToString("f0");
                break;

        }

        return _result;
    }


    /// <summary>
    /// 골드 소모 CSV 삭제하고 공식으로 대체
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    double GetPerUpPrice(int index, int lv)
    {
        double _Mutiple = 0;

        switch (index)
        {


            case 4: // 공격력 % 강화.
                _Mutiple = 10000000000000.0d; // 디폴트 값
                if (lv != 0) _Mutiple *= Math.Pow(1.07d, lv);
                break;

            case 5: // 체력 % 강화.
                _Mutiple = 25000000000000.0d; // 디폴트 값
                if (lv != 0) _Mutiple *= Math.Pow(1.07d, lv);
                break;

            case 6: // 치명타 확률 CRC % 강화.
                _Mutiple = 10000.0d; // 디폴트 값
                if (lv != 0) _Mutiple *= Math.Pow(1.07d, lv);
                break;

            case 7: // 치명타 피해 CRD % 강화.
                _Mutiple = 5000.0d; // 디폴트 값
                if (lv != 0) _Mutiple *= Math.Pow(1.07d, lv);
                break;

            case 8: // 체력회복 % 강화.
                _Mutiple = 12500000000000.0d; // 디폴트 값
                if (lv != 0) _Mutiple *= Math.Pow(1.07d, lv);
                break;


        }

        return Math.Round(_Mutiple);
    }


    /// <summary>
    /// 강화 페이지 레벨 초기화.
    /// </summary>
    public void PowerUP_Init()
    {
        bool isKorean = false;
        if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean") isKorean = true;


        PowerLv = PlayerPrefsManager.GetInstance().ATK_Lv;
        HP_Lv = PlayerPrefsManager.GetInstance().Mat_HP_Lv;
        Rec_Lv = PlayerPrefsManager.GetInstance().Recov_Lv;
        Defend_Lv = PlayerPrefsManager.GetInstance().Defence_Lv;

        //ATK_PER_UP_Lv = PlayerPrefsManager.GetInstance().ATK_PER_UP_Lv;
        //HP_PER_UP_Lv = PlayerPrefsManager.GetInstance().HP_PER_UP_Lv;

        Dia_ATK_PER_UP_Lv = PlayerPrefsManager.GetInstance().Dia_ATK_PER_UP_Lv;
        Dia_HP_PER_UP_Lv = PlayerPrefsManager.GetInstance().Dia_HP_PER_UP_Lv;

        // 0615

        CRC_Lv = PlayerPrefsManager.GetInstance().CRC_UP_Lv;
        CRD_Lv = PlayerPrefsManager.GetInstance().CRD_UP_Lv;
        Dia_CRC_Lv = PlayerPrefsManager.GetInstance().Dia_CRC_UP_Lv;
        Dia_CRD_Lv = PlayerPrefsManager.GetInstance().Dia_CRD_UP_Lv;

        //0709

        Dia_Deffence_Lv = PlayerPrefsManager.GetInstance().Dia_Defence_Lv;
        Dia_HPPER_Lv = PlayerPrefsManager.GetInstance().Dia_RECOV_UP_Lv;


        //------------------------------------------------------------------------------------------------------//

        if (PowerLv >= 50000) PowerLv = 50000;
        if (HP_Lv >= 50000) HP_Lv = 50000;
        if (Rec_Lv >= 50000) Rec_Lv = 50000;
        if (Defend_Lv >= 50000) Defend_Lv = 50000;

        if (ATK_PER_UP_Lv >= 10000) ATK_PER_UP_Lv = 10000;
        if (HP_PER_UP_Lv >= 10000) HP_PER_UP_Lv = 10000;

        if (Dia_ATK_PER_UP_Lv >= 999) Dia_ATK_PER_UP_Lv = 999;
        if (Dia_HP_PER_UP_Lv >= 999) Dia_HP_PER_UP_Lv = 999;

        // 0615

        if (CRC_Lv >= 10000) CRC_Lv = 10000;
        if (CRD_Lv >= 10000) CRD_Lv = 10000;
        if (Dia_CRC_Lv >= 500) Dia_CRC_Lv = 500;
        if (Dia_CRD_Lv >= 500) Dia_CRD_Lv = 500;

        //
        if (Dia_Deffence_Lv >= 9999) Dia_Deffence_Lv = 9999;
        if (Dia_HPPER_Lv >= 9999) Dia_HPPER_Lv = 9999;



        POWER_UP_LV.text = "Lv. " + PowerLv;
        HP_UP_LV.text = "Lv. " + HP_Lv;
        Recov_UP_LV.text = "Lv. " + Rec_Lv;
        Mat_HP_UP_LV.text = "Lv. " + Defend_Lv;

        //ATK_PER_UP_LV.text = "Lv. " + ATK_PER_UP_Lv;
        //HP_PER_UP_LV.text = "Lv. " + HP_PER_UP_Lv;

        Dia_ATK_PER_UP_LV.text = "Lv. " + Dia_ATK_PER_UP_Lv;
        Dia_HP_PER_UP_LV.text = "Lv. " + Dia_HP_PER_UP_Lv;

        // 0615

        CRC_UP_LV.text = "Lv. " + CRC_Lv;
        CRD_UP_LV.text = "Lv. " + CRD_Lv;

        Dia_CRC_UP_LV.text = "Lv. " + Dia_CRC_Lv;
        Dia_CRD_UP_LV.text = "Lv. " + Dia_CRD_Lv;

        // 0709

        Dia_Deffence_Per_UP_LV.text = "Lv. " + Dia_Deffence_Lv;
        Dia_Recov_Per_UP_LV.text = "Lv. " + Dia_HPPER_Lv;


        //-------------------------------------------------------------------------------------------------------//
        //

        currentAtk = PowerLv.ToString();
        nextAtk = (PowerLv + 1).ToString();

        if (PowerLv == 0) currentAtk = "1";

        string tmpATK = dts.fDoubleToStringNumber(currentAtk);
        string tmptmpATK = dts.fDoubleToStringNumber(nextAtk);
        /// 스탯 공격력
        PlayerPrefsManager.GetInstance().RawAttackDamage = tmpATK;
        //ChraterInfo[0].GetComponent<Text>().text = UserWallet.GetInstance().SeetheTruth(PlayerPrefsManager.GetInstance().PlayerDPS);
        UserWallet.GetInstance().ShowUserATK();
        /// 번역
        if (isKorean)
            POWER_UP_TEXT.text = "공격력 " + UserWallet.GetInstance().SeetheNatural(double.Parse(tmpATK)) + " > " + UserWallet.GetInstance().SeetheNatural(double.Parse(tmptmpATK));
        else
            POWER_UP_TEXT.text = "번역 " + UserWallet.GetInstance().SeetheNatural(double.Parse(tmpATK)) + " > " + UserWallet.GetInstance().SeetheNatural(double.Parse(tmptmpATK));
        /// 소비 골드 표기
        currentAtk = GetNormalUpPrice(0, PowerLv);

        /// 골드 업그레이드 비용 감소.
        double doublePrice = double.Parse(currentAtk);
        doublePrice = (doublePrice * (1.0d - PlayerPrefsManager.GetInstance().Arti_GoldUpgrade * 0.001d));

        POWER_UP_Price.text = UserWallet.GetInstance().SeetheNatural(doublePrice);

        //-------------------------------------------------------------------------------------------------------//

        currentHP = (HP_Lv * 10).ToString();
        nextHP = ((HP_Lv + 1) * 10).ToString();

        if (HP_Lv == 0) currentHP = "0";

        tmpATK = dts.fDoubleToStringNumber(currentHP);
        tmptmpATK = dts.fDoubleToStringNumber(nextHP);

        tmpATK = dts.AddStringDouble(tmpATK, "100");
        tmptmpATK = dts.AddStringDouble(tmptmpATK, "100");

        PlayerPrefsManager.GetInstance().Stat_MaxHP = tmpATK;
        //ChraterInfo[1].GetComponent<Text>().text = UserWallet.GetInstance().SeetheTruth(PlayerPrefsManager.GetInstance().Mat_MaxHP);
        UserWallet.GetInstance().ShowUserHP();
        /// 번역
        if (isKorean)
            HP_UP_TEXT.text = "체력 " + UserWallet.GetInstance().SeetheNatural(double.Parse(tmpATK)) + " > " + UserWallet.GetInstance().SeetheNatural(double.Parse(tmptmpATK));
        else
            HP_UP_TEXT.text = "번역 " + UserWallet.GetInstance().SeetheNatural(double.Parse(tmpATK)) + " > " + UserWallet.GetInstance().SeetheNatural(double.Parse(tmptmpATK));
        /// 소비 골드 표기
        currentHP = GetNormalUpPrice(1, HP_Lv);


        /// 골드 업그레이드 비용 감소.
        doublePrice = double.Parse(currentHP);
        doublePrice = (doublePrice * (1.0d - PlayerPrefsManager.GetInstance().Arti_GoldUpgrade * 0.001d));

        HP_UP_Price.text = UserWallet.GetInstance().SeetheNatural(doublePrice);

        //-------------------------------------------------------------------------------------------------------//

        currentRec = (Rec_Lv * 5).ToString();
        nextRec = ((Rec_Lv + 1) * 5).ToString();

        if (Rec_Lv == 0) currentRec = "1";

        tmpATK = dts.fDoubleToStringNumber(currentRec);
        tmptmpATK = dts.fDoubleToStringNumber(nextRec);

        //PlayerPrefsManager.GetInstance().Mat_Recov = tmpATK;
        PlayerPrefs.SetString("Stat_Recov", tmpATK);
        //ChraterInfo[2].GetComponent<Text>().text = UserWallet.GetInstance().SeetheNatural(double.Parse(PlayerPrefsManager.GetInstance().Mat_Recov)) + " /s";
        UserWallet.GetInstance().ShowUserHP_Recov();
        /// 번역
        if (isKorean)
            Recov_UP_TEXT.text = "체력 회복력 " + UserWallet.GetInstance().SeetheNatural(double.Parse(tmpATK)) + " > " + UserWallet.GetInstance().SeetheNatural(double.Parse(tmptmpATK));
        else
            Recov_UP_TEXT.text = "번역 회복력 " + UserWallet.GetInstance().SeetheNatural(double.Parse(tmpATK)) + " > " + UserWallet.GetInstance().SeetheNatural(double.Parse(tmptmpATK));
        /// 소비 골드 표기
        currentRec = GetNormalUpPrice(2, Rec_Lv);


        /// 골드 업그레이드 비용 감소.
        doublePrice = double.Parse(currentRec);
        doublePrice = (doublePrice * (1.0d - PlayerPrefsManager.GetInstance().Arti_GoldUpgrade * 0.001d));

        Recov_UP_Price.text = UserWallet.GetInstance().SeetheNatural(doublePrice);


        //-------------------------------------------------------------------------------------------------------//

        currentRec = Defend_Lv.ToString();
        nextRec = (Defend_Lv + 1).ToString();

        tmpATK = currentRec;
        tmptmpATK = nextRec;

        UserWallet.GetInstance().ShowUserDeffence();
        /// 번역
        if (isKorean)
            Mat_HP_UP_TEXT.text = "방어력 " + UserWallet.GetInstance().SeetheNatural(double.Parse(tmpATK)) + " > " + UserWallet.GetInstance().SeetheNatural(double.Parse(tmptmpATK));
        else
            Mat_HP_UP_TEXT.text = "번역 " + UserWallet.GetInstance().SeetheNatural(double.Parse(tmpATK)) + " > " + UserWallet.GetInstance().SeetheNatural(double.Parse(tmptmpATK));
        /// 소비 국밥 표기
        currentRec = GetNormalUpPrice(3, Defend_Lv);

        /// 골드 업그레이드 비용 감소.
        doublePrice = double.Parse(currentRec);
        doublePrice = (doublePrice * (1.0d - PlayerPrefsManager.GetInstance().Arti_GoldUpgrade * 0.001d));

        Mat_HP_UP_Price.text = UserWallet.GetInstance().SeetheNatural(doublePrice);

        //-------------------------------------------------------------------------------------------------------//
        //

        currentAtk = (5 * Dia_ATK_PER_UP_Lv).ToString();
        nextAtk = (5 * (Dia_ATK_PER_UP_Lv + 1)).ToString();

        if (Dia_ATK_PER_UP_Lv == 0) currentAtk = "0";

        tmpATK = (currentAtk);
        tmptmpATK = (nextAtk);

        PlayerPrefsManager.GetInstance().Dia_ATK_PER_UP = tmpATK;
        /// 번역
        if (isKorean)
            Dia_ATK_PER_UP_TEXT.text = "공격력 " + tmpATK + "% > " + tmptmpATK + "%";
        else
            Dia_ATK_PER_UP_TEXT.text = "번역 " + tmpATK + "% > " + tmptmpATK + "%";
        /// 
        //currentAtk = PlayerPrefsManager.GetInstance().diaStatDataColl[1, Dia_ATK_PER_UP_Lv + 1];
        tmpATK = (50 * (Dia_ATK_PER_UP_Lv + 2)).ToString();

        Dia_ATK_PER_UP_Price.text = UserWallet.GetInstance().SeetheNatural(double.Parse(tmpATK));



        //-------------------------------------------------------------------------------------------------------//
        //

        currentAtk = (5 * Dia_HP_PER_UP_Lv).ToString();
        nextAtk = (5 * (Dia_HP_PER_UP_Lv + 1)).ToString();

        tmpATK = (currentAtk);
        tmptmpATK = (nextAtk);

        PlayerPrefsManager.GetInstance().Dia_HP_PER_UP = tmpATK;

        /// 번역
        if (isKorean)
            Dia_HP_PER_UP_TEXT.text = "체력 " + tmpATK + "% > " + tmptmpATK + "%";
        else
            Dia_HP_PER_UP_TEXT.text = "번역 " + tmpATK + "% > " + tmptmpATK + "%";
        /// 
        //currentAtk = PlayerPrefsManager.GetInstance().diaStatDataColl[3, Dia_HP_PER_UP_Lv + 1];
        tmpATK = (100 * (Dia_HP_PER_UP_Lv + 2)).ToString();

        Dia_HP_PER_UP_Price.text = UserWallet.GetInstance().SeetheNatural(double.Parse(tmpATK));

        //-----------------------------------------------------------------------------------------------------
        ///  0615
        // 골드 크확
        ///  
        currentAtk = (0.001f * CRC_Lv).ToString("f3");
        nextAtk = (0.001f * (CRC_Lv + 1)).ToString("f3");

        tmpATK = (currentAtk);
        tmptmpATK = (nextAtk);

        PlayerPrefsManager.GetInstance().CRC_UP = tmpATK;
        /// 번역
        if (isKorean)
            CRC_UP_TEXT.text = "치명타 확률 " + tmpATK + "% > " + tmptmpATK + "%";
        else
            CRC_UP_TEXT.text = "번역 확률 " + tmpATK + "% > " + tmptmpATK + "%";

        /// 소비 골드 표기
        doublePrice = GetPerUpPrice(6, CRC_Lv);

        /// 골드 업그레이드 비용 감소.
        doublePrice = (doublePrice * (1.0d - PlayerPrefsManager.GetInstance().Arti_GoldUpgrade * 0.001d));

        CRC_UP_Price.text = UserWallet.GetInstance().SeetheNatural(doublePrice);



        //-----------------------------------------------------------------------------------------------------
        ///  0615
        // 골드 크댐
        ///  
        currentAtk = (0.5f * CRD_Lv).ToString();
        nextAtk = (0.5f * (CRD_Lv + 1)).ToString();

        tmpATK = (currentAtk);
        tmptmpATK = (nextAtk);

        PlayerPrefsManager.GetInstance().CRD_UP = tmpATK;

        /// 번역
        if (isKorean)
            CRD_UP_TEXT.text = "치명타 대미지 " + tmpATK + "% > " + tmptmpATK + "%";
        else
            CRD_UP_TEXT.text = "번역 대미지 " + tmpATK + "% > " + tmptmpATK + "%";

        /// 소비 골드 표기
        doublePrice = GetPerUpPrice(7, CRD_Lv);

        /// 골드 업그레이드 비용 감소.
        doublePrice = (doublePrice * (1.0d - PlayerPrefsManager.GetInstance().Arti_GoldUpgrade * 0.001d));

        CRD_UP_Price.text = UserWallet.GetInstance().SeetheNatural(doublePrice);

        //-----------------------------------------------------------------------------------------------------
        ///  0615
        // 다이아 크확
        ///  
        currentAtk = (0.05f * Dia_CRC_Lv).ToString();
        nextAtk = (0.05f * (Dia_CRC_Lv + 1)).ToString();

        tmpATK = (currentAtk);
        tmptmpATK = (nextAtk);

        PlayerPrefsManager.GetInstance().Dia_CRC_UP = tmpATK;

        /// 번역
        if (isKorean)
            Dia_CRC_UP_TEXT.text = "치명타 확률 " + tmpATK + "% > " + tmptmpATK + "%";
        else
            Dia_CRC_UP_TEXT.text = "번역 확률 " + tmpATK + "% > " + tmptmpATK + "%";
        /// 
        tmpATK = (100 * (Dia_CRC_Lv + 1)).ToString();

        Dia_CRC_UP_Price.text = UserWallet.GetInstance().SeetheNatural(double.Parse(tmpATK));


        //-----------------------------------------------------------------------------------------------------
        //
        ///  0615
        // 다이아 크댐
        ///  
        currentAtk = (5 * Dia_CRD_Lv).ToString();
        nextAtk = (5 * (Dia_CRD_Lv + 1)).ToString();

        tmpATK = (currentAtk);
        tmptmpATK = (nextAtk);

        PlayerPrefsManager.GetInstance().Dia_CRD_UP = tmpATK;

        /// 번역
        if (isKorean)
            Dia_CRD_UP_TEXT.text = "치명타 대미지 " + tmpATK + "% > " + tmptmpATK + "%";
        else
            Dia_CRD_UP_TEXT.text = "번역 대미지 " + tmpATK + "% > " + tmptmpATK + "%";
        /// 
        tmpATK = (50 * (Dia_CRD_Lv + 1)).ToString();

        Dia_CRD_UP_Price.text = UserWallet.GetInstance().SeetheNatural(double.Parse(tmpATK));


        ///
        //////////////////////////////////////////////////////////
        /// 다이아 방어력 특별 강화
        ///  
        currentAtk = (5 * Dia_Deffence_Lv).ToString();
        nextAtk = (5 * (Dia_Deffence_Lv + 1)).ToString();

        tmpATK = (currentAtk);
        tmptmpATK = (nextAtk);

        /// 번역
        if (isKorean)
            Dia_Deffence_Per_UP_TEXT.text = "방어력 " + tmpATK + "% > " + tmptmpATK + "%";
        else
            Dia_Deffence_Per_UP_TEXT.text = "번역 " + tmpATK + "% > " + tmptmpATK + "%";
        /// 
        tmpATK = Dia_Deffence_Lv < 1 ? "100" : (50 * (Dia_Deffence_Lv + 2)).ToString();

        Dia_Deffence_Per_UP_Price.text = UserWallet.GetInstance().SeetheNatural(double.Parse(tmpATK));




        ///  0615
        // 다이아 체회퍼
        ///  
        tmpATK = (5 * Dia_HPPER_Lv).ToString();
        tmptmpATK = (5 * (Dia_HPPER_Lv + 1)).ToString();

        /// 번역
        if (isKorean)
            Dia_Recov_Per_UP_TEXT.text = "체력 회복력 " + tmpATK + "% > " + tmptmpATK + "%";
        else
            Dia_Recov_Per_UP_TEXT.text = "번역 회복력 " + tmpATK + "% > " + tmptmpATK + "%";
        /// 
        tmpATK = (75 * (Dia_HPPER_Lv + 1)).ToString();

        Dia_Recov_Per_UP_Price.text = UserWallet.GetInstance().SeetheNatural(double.Parse(tmpATK));

        /// 퀘스트 올려줌

        PlayerPrefsManager.GetInstance().questInfo[0].All_Mattzip = PlayerPrefsManager.GetInstance().Defence_Lv;
        PlayerPrefsManager.GetInstance().questInfo[0].All_Atk = PlayerPrefsManager.GetInstance().ATK_Lv;
        PlayerPrefsManager.GetInstance().questInfo[0].All_HP = PlayerPrefsManager.GetInstance().Mat_HP_Lv;
        // 일단 세개

        //PlayerPrefsManager.GetInstance().questInfo4[0].All_Per_Atk = PlayerPrefsManager.GetInstance().ATK_PER_UP_Lv;
        //PlayerPrefsManager.GetInstance().questInfo4[0].All_Per_HP = PlayerPrefsManager.GetInstance().HP_PER_UP_Lv;

        PlayerPrefsManager.GetInstance().questInfo4[0].All_Dia_Atk = PlayerPrefsManager.GetInstance().Dia_ATK_PER_UP_Lv;
        PlayerPrefsManager.GetInstance().questInfo4[0].All_Dia_HP = PlayerPrefsManager.GetInstance().Dia_HP_PER_UP_Lv;


        ////////////////////////////////////////////

        if (coru == null)
            coru = StartCoroutine(GoldChechecak());

        All_GoldCheak();

        PlayerPrefs.Save();
    }

    Coroutine coru;
    WaitForSeconds GC_05_SEC = new WaitForSeconds(0.5f);

    /// <summary>
    /// 회색으로 덮기 판단 0.5초 마다
    /// </summary>
    /// <returns></returns>
    IEnumerator GoldChechecak()
    {
        yield return null;

        while (true)
        {
            yield return GC_05_SEC;
            // 미션 달성한거 뭐있나 초기화.
            All_GoldCheak();
        }
    }


    /// <summary>
    /// 모든 골드
    /// </summary>
    void All_GoldCheak()
    {
        if (!LvUPObjt.activeSelf) return;

        Power_UP_Cheak();
        HP_UP_Cheak();
        Recov_UP_Cheak();
        Mattzip_UP_Cheak();

        //ATK_PER_UP_Cheak();
        //HP_PER_UP_Cheak();
        Dia_ATK_PER_UP_Cheak();
        Dia_HP_PER_UP_Cheak();

        // 0615
        CRC_UP_Cheak();
        CRD_UP_Cheak();
        Dia_CRC_Cheak();
        Dia_CRD_Cheak();

        // 0709
        Dia_Deffence_Cheak();
        DIA_RECOV_UP_Cheak();
    }

    readonly string ATK_PER_UPgoldPass;

    ///// <summary>
    ///// 돈이 안되면 회색으로.
    ///// </summary>
    //bool ATK_PER_UP_Cheak()
    //{
    //    // 현재 공격력 퍼센트 레벨
    //    int PowerLv = PlayerPrefsManager.GetInstance().ATK_PER_UP_Lv;

    //    if (PowerLv >= 10000)
    //    {
    //        ATK_PER_UP_Max.SetActive(true);
    //        return false;
    //    }

    //    // 다음 레벨의 가격 불러오고.
    //    double doublePrice = GetPerUpPrice(4, PowerLv);
    //    /// 골드 업그레이드 비용 감소.
    //    doublePrice = (doublePrice * (1.0d - PlayerPrefsManager.GetInstance().Arti_GoldUpgrade * 0.001d));

    //    ATK_PER_UPgoldPass = dts.SubStringDouble(PlayerPrefsManager.GetInstance().gold, doublePrice);
    //    // 골드 없으면 false
    //    if (ATK_PER_UPgoldPass == "-1")
    //    {
    //        ATK_PER_UP_Gray.SetActive(true);
    //        return false;
    //    }
    //    else // 구매 가능하면 트루
    //    {
    //        ATK_PER_UP_Gray.SetActive(false);
    //        return true;
    //    }

    //}


    readonly string HP_PER_UP_goldPass;

    /// <summary>
    /// 돈이 안되면 회색으로.
    /// </summary>
    //bool HP_PER_UP_Cheak()
    //{
    //    // 현재 공격력 퍼센트 레벨
    //    int PowerLv = PlayerPrefsManager.GetInstance().HP_PER_UP_Lv;

    //    if (PowerLv >= 10000)
    //    {
    //        HP_PER_UP_Max.SetActive(true);
    //        return false;
    //    }

    //    // 다음 레벨의 가격 불러오고.
    //    double doublePrice = GetPerUpPrice(5, PowerLv);

    //    /// 골드 업그레이드 비용 감소.
    //    doublePrice = (doublePrice * (1.0d - PlayerPrefsManager.GetInstance().Arti_GoldUpgrade * 0.001d));

    //    HP_PER_UP_goldPass = dts.SubStringDouble(PlayerPrefsManager.GetInstance().gold, doublePrice);

    //    // 골드 없으면 false
    //    if (HP_PER_UP_goldPass == "-1")
    //    {
    //        HP_PER_UP_Gray.SetActive(true);
    //        return false;
    //    }
    //    else // 구매 가능하면 트루
    //    {
    //        HP_PER_UP_Gray.SetActive(false);
    //        return true;
    //    }

    //}


    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>


    string Dia_ATK_PER_UPgoldPass;

    /// <summary>
    /// 돈이 안되면 회색으로.
    /// </summary>
    bool Dia_ATK_PER_UP_Cheak()
    {
        // 현재 공격력 퍼센트 레벨
        int PowerLv = PlayerPrefsManager.GetInstance().Dia_ATK_PER_UP_Lv;

        if (PowerLv >= 999)
        {
            Dia_ATK_PER_UP_Max.SetActive(true);
            return false;
        }

        // 다음 레벨의 가격 불러오고.
        string nextPrice = (50 * (PowerLv + 2)).ToString();

        if (PlayerPrefs.GetFloat("dDiamond") - float.Parse(nextPrice) < 0)
        {
            Dia_ATK_PER_UPgoldPass = "-1";
        }
        else Dia_ATK_PER_UPgoldPass = (PlayerPrefs.GetFloat("dDiamond") - float.Parse(nextPrice)).ToString();


        /// 다이아 없으면 false
        if (Dia_ATK_PER_UPgoldPass == "-1")
        {
            Dia_ATK_PER_UP_Gray.SetActive(true);
            return false;
        }
        else // 구매 가능하면 트루
        {
            Dia_ATK_PER_UP_Gray.SetActive(false);
            return true;
        }

    }

    string Dia_HP_PER_UP_goldPass;

    /// <summary>
    /// 돈이 안되면 회색으로.
    /// </summary>
    bool Dia_HP_PER_UP_Cheak()
    {
        // 현재 공격력 퍼센트 레벨
        int PowerLv = PlayerPrefsManager.GetInstance().Dia_HP_PER_UP_Lv;

        if (PowerLv >= 999)
        {
            Dia_HP_PER_UP_Max.SetActive(true);
            return false;
        }

        // 다음 레벨의 가격 불러오고.
        string nextPrice = (100 * (PowerLv + 2)).ToString();

        if (PlayerPrefs.GetFloat("dDiamond") - float.Parse(nextPrice) < 0)
        {
            Dia_HP_PER_UP_goldPass = "-1";
        }
        else Dia_HP_PER_UP_goldPass = (PlayerPrefs.GetFloat("dDiamond") - float.Parse(nextPrice)).ToString();

        /// 다이아 없으면 false
        if (Dia_HP_PER_UP_goldPass == "-1")
        {
            Dia_HP_PER_UP_Gray.SetActive(true);
            return false;
        }
        else // 구매 가능하면 트루
        {
            Dia_HP_PER_UP_Gray.SetActive(false);
            return true;
        }

    }

    string CRC_UPgoldPass;

    /// <summary>
    /// 돈이 안되면 회색으로.
    /// </summary>
    bool CRC_UP_Cheak()
    {
        // 현재 레벨
        int PowerLv = PlayerPrefsManager.GetInstance().CRC_UP_Lv;

        if (PowerLv >= 10000)
        {
            CRC_UP_Max.SetActive(true);
            return false;
        }

        // 다음 레벨의 가격 불러오고.
        double doublePrice = GetPerUpPrice(6, PowerLv);
        /// 골드 업그레이드 비용 감소.
        doublePrice = (doublePrice * (1.0d - PlayerPrefsManager.GetInstance().Arti_GoldUpgrade * 0.001d));

        CRC_UPgoldPass = dts.SubStringDouble(PlayerPrefsManager.GetInstance().gold, doublePrice);

        // 골드 없으면 false
        if (CRC_UPgoldPass == "-1")
        {
            CRC_UP_Gray.SetActive(true);
            return false;
        }
        else // 구매 가능하면 트루
        {
            CRC_UP_Gray.SetActive(false);
            return true;
        }

    }
    public void CRC_UP()
    {
        if (!CRC_UP_Cheak()) return;

        //골드 감소 처리
        PlayerPrefsManager.GetInstance().gold = CRC_UPgoldPass;
        UserWallet.GetInstance().ShowUserGold();
        // 맷집 레벨 상승
        PlayerPrefsManager.GetInstance().CRC_UP_Lv++;
        tmm.ExUpdateMission(33); /// 미션 업데이트

        PowerUP_Init();
    }

    string CRD_UPgoldPass;

    /// <summary>
    /// 돈이 안되면 회색으로.
    /// </summary>
    bool CRD_UP_Cheak()
    {
        // 현재 레벨
        int PowerLv = PlayerPrefsManager.GetInstance().CRD_UP_Lv;

        if (PowerLv >= 10000)
        {
            CRD_UP_Max.SetActive(true);
            return false;
        }

        // 다음 레벨의 가격 불러오고.
        double doublePrice = GetPerUpPrice(7, PowerLv);
        /// 골드 업그레이드 비용 감소.
        doublePrice = (doublePrice * (1.0d - PlayerPrefsManager.GetInstance().Arti_GoldUpgrade * 0.001d));

        CRD_UPgoldPass = dts.SubStringDouble(PlayerPrefsManager.GetInstance().gold, doublePrice);

        // 골드 없으면 false
        if (CRD_UPgoldPass == "-1")
        {
            CRD_UP_Gray.SetActive(true);
            return false;
        }
        else // 구매 가능하면 트루
        {
            CRD_UP_Gray.SetActive(false);
            return true;
        }

    }
    public void CRD_UP()
    {
        if (!CRD_UP_Cheak()) return;

        //골드 감소 처리
        PlayerPrefsManager.GetInstance().gold = CRD_UPgoldPass;
        UserWallet.GetInstance().ShowUserGold();
        // 맷집 레벨 상승
        PlayerPrefsManager.GetInstance().CRD_UP_Lv++;
        tmm.ExUpdateMission(38); /// 미션 업데이트


        PowerUP_Init();
    }


    string Dia_CRC_UP_goldPass;

    /// <summary>
    /// 돈이 안되면 회색으로.
    /// </summary>
    bool Dia_CRC_Cheak()
    {
        // 현재 공격력 퍼센트 레벨
        int PowerLv = PlayerPrefsManager.GetInstance().Dia_CRC_UP_Lv;

        if (PowerLv >= 500)
        {
            Dia_CRC_UP_Max.SetActive(true);
            return false;
        }

        // 다음 레벨의 가격 불러오고.
        string nextPrice = (100 * (Dia_CRC_Lv + 1)).ToString();

        if (PlayerPrefs.GetFloat("dDiamond") - float.Parse(nextPrice) < 0)
        {
            Dia_CRC_UP_goldPass = "-1";
        }
        else Dia_CRC_UP_goldPass = (PlayerPrefs.GetFloat("dDiamond") - float.Parse(nextPrice)).ToString();

        /// 다이아 없으면 false
        if (Dia_CRC_UP_goldPass == "-1")
        {
            Dia_CRC_UP_Gray.SetActive(true);
            return false;
        }
        else // 구매 가능하면 트루
        {
            Dia_CRC_UP_Gray.SetActive(false);
            return true;
        }

    }
    public void DIA_CRC_UP()
    {
        if (!Dia_CRC_Cheak()) return;

        ///다이아 감소 처리
        PlayerPrefs.SetFloat("dDiamond", float.Parse(Dia_CRC_UP_goldPass));
        UserWallet.GetInstance().ShowUserDia();
        // 레벨 상승
        PlayerPrefsManager.GetInstance().Dia_CRC_UP_Lv++;

        PowerUP_Init();
    }


    string Dia_CRD_UP_goldPass;

    /// <summary>
    /// 돈이 안되면 회색으로.
    /// </summary>
    bool Dia_CRD_Cheak()
    {
        // 현재 공격력 퍼센트 레벨
        int PowerLv = PlayerPrefsManager.GetInstance().Dia_CRD_UP_Lv;

        if (PowerLv >= 500)
        {
            Dia_CRD_UP_Max.SetActive(true);
            return false;
        }

        // 다음 레벨의 가격 불러오고.
        string nextPrice = (50 * (Dia_CRD_Lv + 1)).ToString();

        if (PlayerPrefs.GetFloat("dDiamond") - float.Parse(nextPrice) < 0)
        {
            Dia_CRD_UP_goldPass = "-1";
        }
        else Dia_CRD_UP_goldPass = (PlayerPrefs.GetFloat("dDiamond") - float.Parse(nextPrice)).ToString();

        /// 다이아 없으면 false
        if (Dia_CRD_UP_goldPass == "-1")
        {
            Dia_CRD_UP_Gray.SetActive(true);
            return false;
        }
        else // 구매 가능하면 트루
        {
            Dia_CRD_UP_Gray.SetActive(false);
            return true;
        }

    }
    public void DIA_CRD_UP()
    {
        if (!Dia_CRD_Cheak()) return;

        ///다이아 감소 처리
        PlayerPrefs.SetFloat("dDiamond", float.Parse(Dia_CRD_UP_goldPass));
        UserWallet.GetInstance().ShowUserDia();
        // 레벨 상승
        PlayerPrefsManager.GetInstance().Dia_CRD_UP_Lv++;

        PowerUP_Init();
    }






























    //public void TEST_ATK_PER_UP()
    //{
    //    if (!ATK_PER_UP_Cheak()) return;

    //    //골드 감소 처리
    //    PlayerPrefsManager.GetInstance().gold = ATK_PER_UPgoldPass;
    //    UserWallet.GetInstance().ShowUserGold();
    //    // 퀘스트
    //    PlayerPrefsManager.GetInstance().questInfo[0].daily_Atk++;
    //    // 맷집 레벨 상승
    //    PlayerPrefsManager.GetInstance().ATK_PER_UP_Lv++;
    //    // 퀘스트

    //    PowerUP_Init();
    //}




    //public void TEST_HP_PER_UP()
    //{
    //    if (!HP_PER_UP_Cheak()) return;

    //    //골드 감소 처리
    //    PlayerPrefsManager.GetInstance().gold = HP_PER_UP_goldPass;
    //    UserWallet.GetInstance().ShowUserGold();
    //    // 퀘스트
    //    PlayerPrefsManager.GetInstance().questInfo[0].daily_HP++;
    //    // 맷집 레벨 상승
    //    PlayerPrefsManager.GetInstance().HP_PER_UP_Lv++;

    //    PowerUP_Init();
    //}







    public void DIA_ATK_PER_UP()
    {
        if (!Dia_ATK_PER_UP_Cheak()) return;

        ///다이아 감소 처리
        PlayerPrefs.SetFloat("dDiamond", float.Parse(Dia_ATK_PER_UPgoldPass));
        UserWallet.GetInstance().ShowUserDia();
        // 퀘스트
        PlayerPrefsManager.GetInstance().questInfo[0].daily_Atk++;

        // 맷집 레벨 상승
        PlayerPrefsManager.GetInstance().Dia_ATK_PER_UP_Lv++;
        // 퀘스트

        PowerUP_Init();
    }




    public void DIA_HP_PER_UP()
    {
        if (!Dia_HP_PER_UP_Cheak()) return;

        ///다이아 감소 처리
        PlayerPrefs.SetFloat("dDiamond", float.Parse(Dia_HP_PER_UP_goldPass));
        UserWallet.GetInstance().ShowUserDia();
        // 퀘스트
        PlayerPrefsManager.GetInstance().questInfo[0].daily_HP++;

        // 맷집 레벨 상승
        PlayerPrefsManager.GetInstance().Dia_HP_PER_UP_Lv++;

        PowerUP_Init();
    }









    /// <summary>
    /// 방어력 다이아몬드 가능?
    /// </summary>
    string Dia_Deffence_Dia_Pass;
    /// <summary>
    /// 돈이 안되면 회색으로.
    /// </summary>
    bool Dia_Deffence_Cheak()
    {
        // 현재 다이아 방어력 체크
        int PowerLv = PlayerPrefsManager.GetInstance().Dia_Defence_Lv;

        if (PowerLv >= 9999)
        {
            Dia_Deffence_Per_UP_Max.SetActive(true);
            return false;
        }

        // 다음 레벨의 가격 불러오고.
        string nextPrice = (50 * (PowerLv + 2)).ToString();

        if (PlayerPrefs.GetFloat("dDiamond") - float.Parse(nextPrice) < 0)
            Dia_Deffence_Dia_Pass = "-1";
        else
            Dia_Deffence_Dia_Pass = (PlayerPrefs.GetFloat("dDiamond") - float.Parse(nextPrice)).ToString();


        // 골드 없으면 false
        if (Dia_Deffence_Dia_Pass == "-1")
        {
            Dia_Deffence_Per_UP_Gray.SetActive(true);
            return false;
        }
        else // 구매 가능하면 트루
        {
            Dia_Deffence_Per_UP_Gray.SetActive(false);
            return true;
        }

    }


    /// <summary>
    /// 외부에서도 단타 치면 작동
    /// </summary>
    public void Dia_Deffence_UP()
    {
        if (!Dia_Deffence_Cheak()) return;

        ///다이아 감소 처리
        PlayerPrefs.SetFloat("dDiamond", float.Parse(Dia_Deffence_Dia_Pass));
        UserWallet.GetInstance().ShowUserDia();
        // 공격력 레벨 상승
        PlayerPrefsManager.GetInstance().Dia_Defence_Lv++;

        PowerUP_Init();
    }




    string DIA_RECOV_UP_goldPass;

    /// <summary>
    /// 돈이 안되면 회색으로.
    /// </summary>
    bool DIA_RECOV_UP_Cheak()
    {
        // 현재 공격력 퍼센트 레벨
        int PowerLv = PlayerPrefsManager.GetInstance().Dia_HP_PER_UP_Lv;

        if (PowerLv >= 9999)
        {
            Dia_Recov_Per_UP_Max.SetActive(true);
            return false;
        }

        // 다음 레벨의 가격 불러오고.
        string nextPrice = (75 * (PowerLv + 1)).ToString();

        if (PlayerPrefs.GetFloat("dDiamond") - float.Parse(nextPrice) < 0)
        {
            DIA_RECOV_UP_goldPass = "-1";
        }
        else DIA_RECOV_UP_goldPass = (PlayerPrefs.GetFloat("dDiamond") - float.Parse(nextPrice)).ToString();

        /// 다이아 없으면 false
        if (DIA_RECOV_UP_goldPass == "-1")
        {
            Dia_Recov_Per_UP_Gray.SetActive(true);
            return false;
        }
        else // 구매 가능하면 트루
        {
            Dia_Recov_Per_UP_Gray.SetActive(false);
            return true;
        }

    }

    public void DIA_RECOV_UP()
    {
        if (!DIA_RECOV_UP_Cheak()) return;

        ///다이아 감소 처리
        PlayerPrefs.SetFloat("dDiamond", float.Parse(DIA_RECOV_UP_goldPass));
        UserWallet.GetInstance().ShowUserDia();

        // 맷집 레벨 상승
        PlayerPrefsManager.GetInstance().Dia_RECOV_UP_Lv++;

        PowerUP_Init();
    }














    /// <summary>
    /// 돈이 안되면 회색으로.
    /// </summary>
    bool Power_UP_Cheak()
    {
        // 현재 공격력 레벨
        int PowerLv = PlayerPrefsManager.GetInstance().ATK_Lv;

        if (PowerLv >= 50000)
        {
            POWER_UP_Max.SetActive(true);
            return false;
        }

        // 다음 레벨의 가격 불러오고.
        string nextPrice = GetNormalUpPrice(0, PowerLv);

        /// 골드 업그레이드 비용 감소.
        double doublePrice = double.Parse(nextPrice);
        doublePrice = (doublePrice * (1.0d - PlayerPrefsManager.GetInstance().Arti_GoldUpgrade * 0.001d));

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

    string Power_UPgoldPass;

    /// <summary>
    ///  테스트가 잘되서 이걸로 씀
    /// </summary>
    public void TEST_Power_UP()
    {
        if (!Power_UP_Cheak()) return;

        //골드 감소 처리
        PlayerPrefsManager.GetInstance().gold = Power_UPgoldPass;
        UserWallet.GetInstance().ShowUserGold();
        // 공격력 레벨 상승
        PlayerPrefsManager.GetInstance().ATK_Lv++;
        tmm.ExUpdateMission(3); /// 미션 업데이트


        // 퀘스트
        PlayerPrefsManager.GetInstance().questInfo[0].daily_Atk++;

        if (PlayerPrefsManager.GetInstance().questInfo[0].All_Atk < 1000)
        {
            PlayerPrefsManager.GetInstance().questInfo[0].All_Atk++;
        }

        PowerUP_Init();

    }


    bool HP_UP_Cheak()
    {
        // 현재 체력력 레벨
        int HP_Lv = PlayerPrefsManager.GetInstance().Mat_HP_Lv;

        if (HP_Lv >= 50000)
        {
            HP_UP_Max.SetActive(true);
            return false;
        }

        // 다음 레벨의 가격 불러오고.
        string nextPrice = GetNormalUpPrice(1, HP_Lv);


        /// 골드 업그레이드 비용 감소.
        double doublePrice = double.Parse(nextPrice);
        doublePrice = (doublePrice * (1.0d - PlayerPrefsManager.GetInstance().Arti_GoldUpgrade * 0.001d));

        HP_UPgoldPass = dts.SubStringDouble(PlayerPrefsManager.GetInstance().gold, doublePrice);

        // 골드 있냐? 
        if (HP_UPgoldPass == "-1")
        {
            HP_UP_Gray.SetActive(true);
            return false;
        }
        else
        {
            HP_UP_Gray.SetActive(false);
            return true;
        }
    }

    string HP_UPgoldPass;

    /// <summary>
    /// [훈련강화]에서 [체력 강화]
    /// </summary>
    public void TEST_HP_UP()
    {
        if (!HP_UP_Cheak()) return;

        //골드 감소 처리
        PlayerPrefsManager.GetInstance().gold = HP_UPgoldPass;
        UserWallet.GetInstance().ShowUserGold();
        // 체력력 레벨 상승
        PlayerPrefsManager.GetInstance().Mat_HP_Lv++;
        // 퀘스트
        PlayerPrefsManager.GetInstance().questInfo[0].daily_HP++;

        if (PlayerPrefsManager.GetInstance().questInfo[0].All_HP < 1000)
        {
            PlayerPrefsManager.GetInstance().questInfo[0].All_HP++;
        }

        PowerUP_Init();
    }



    bool Recov_UP_Cheak()
    {
        // 현재 체력 회복력 레벨
        int Recov_Lv = PlayerPrefsManager.GetInstance().Recov_Lv;

        if (Recov_Lv >= 50000)
        {
            Recov_UP_Max.SetActive(true);
            return false;
        }

        // 다음 레벨의 가격 불러오고.
        string nextPrice = GetNormalUpPrice(2, Recov_Lv);

        /// 골드 업그레이드 비용 감소.
        double doublePrice = double.Parse(nextPrice);
        doublePrice = (doublePrice * (1.0d - PlayerPrefsManager.GetInstance().Arti_GoldUpgrade * 0.001d));

        Rexcov_UPgoldpass = dts.SubStringDouble(PlayerPrefsManager.GetInstance().gold, doublePrice);

        // 골드 있냐? 
        if (Rexcov_UPgoldpass == "-1")
        {
            Recov_UP_Gray.SetActive(true);
            return false;
        }
        else
        {
            Recov_UP_Gray.SetActive(false);
            return true;
        }
    }

    string Rexcov_UPgoldpass;

    public void TEST_Recov_UP()
    {
        if (!Recov_UP_Cheak()) return;

        //골드 감소 처리
        PlayerPrefsManager.GetInstance().gold = Rexcov_UPgoldpass;
        UserWallet.GetInstance().ShowUserGold();
        // 체력 회복력 레벨 상승
        PlayerPrefsManager.GetInstance().Recov_Lv++;
        tmm.ExUpdateMission(15); /// 미션 업데이트

        PowerUP_Init();
    }


    /// <summary>
    /// 방어력 올릴때 골드 여유 있나 체크
    /// </summary>
    /// <returns></returns>
    bool Mattzip_UP_Cheak()
    {
        /// 현재 방어력 레벨
        int Defend_Lv = PlayerPrefsManager.GetInstance().Defence_Lv;

        if (Defend_Lv >= 50000)
        {
            Mat_HP_UP_May.SetActive(true);
            return false;
        }

        // 다음 레벨의 가격 불러오고.
        string nextPrice = GetNormalUpPrice(3, Defend_Lv);

        /// 골드 업그레이드 비용 감소.
        double doublePrice = double.Parse(nextPrice);
        doublePrice = (doublePrice * (1.0d - PlayerPrefsManager.GetInstance().Arti_GoldUpgrade * 0.001d));

        MattzipgupbapPass = dts.SubStringDouble(PlayerPrefsManager.GetInstance().gold, doublePrice);

        // 골드 있냐? 
        if (MattzipgupbapPass == "-1")
        {
            Mat_HP_UP_Gray.SetActive(true);
            return false;
        }
        else
        {
            Mat_HP_UP_Gray.SetActive(false);
            return true;
        }
    }

    string MattzipgupbapPass;
    /// <summary>
    /// 방어력 강화로 대체
    /// 50 * ( 1 + 0.07 ) ^ Lv    
    /// /// </summary>
    public void TEST_Mattzip_up()
    {
        if (!Mattzip_UP_Cheak()) return;

        //골드 감소 처리
        PlayerPrefsManager.GetInstance().gold = MattzipgupbapPass;
        UserWallet.GetInstance().ShowUserGold();
        // 맷집 레벨 상승
        PlayerPrefsManager.GetInstance().Defence_Lv++;
        tmm.ExUpdateMission(5); /// 미션 업데이트
        tmm.ExUpdateMission(24); /// 미션 업데이트

        // 퀘스트
        if (PlayerPrefsManager.GetInstance().questInfo[0].All_Mattzip < 1000)
        {
            PlayerPrefsManager.GetInstance().questInfo[0].All_Mattzip++;
        }

        PowerUP_Init();
    }





















}

