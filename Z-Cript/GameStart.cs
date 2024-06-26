﻿/**
 * @author : tinygreen92 
 * @date   : 2020-03-02
 * @desc   : 로팅 씬 이후에 바로 실행해주는 값 모여라
 *           
 */
using Lean.Localization;
using System.Collections;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public bool isDebugMode = false;
    [Space]
    [Header("-로컬라이징")]
    public GameObject leanObject;
    public LeanLocalization leanLocalization;
    [Header("-뉴 패키지 상점 내용물 세팅")]
    public NewPakaStarter[] news;
    public GameObject newsObject;

    [Header("-게임시작시 켰다가 꺼줌.")]
    public GameObject[] AllObject;

    [Header("-외부 API 초기화")]
    public FriendManager friendManager;
    public TutorialMissionManager tutorialMissionManager;
    public ShieldManager shieldManager;
    public InfiniteScroll InfinityContent;
    public InfiniteScroll InfinityQuest;
    public InfiniteScroll InfinityShield;
    public CharacterManager charactherMang;
    public GroggyManager groggyManager;
    public TutorialManager tutorialManager;
    public OfflineManager offlineManager;
    public PunchManager punchManager;
    public QuestManager questManager;
    public TapToSpawnLimit tapToSpawnLimit;
    public AudioManager audioManager;
    public VIPManager vipManager;
    public FlagManager flagManager;


    [Header("-비워두는게 정상")]
    public ConfigManager configManager;

    int tmpVIP;
    int tmpS2;

    void InvoAwaker()
    {
        PlayerPrefs.SetInt("isFristGameStart", 1);
        PlayerPrefs.SetInt("isSignFirst", 1);
        PlayerPrefs.SetInt("isDataSaved", 1);

        PlayerPrefs.SetInt("isBeginingS2", tmpS2);
        PlayerPrefs.SetInt("VIP", tmpVIP);
        PlayerPrefs.Save();
        // VIP 계급 초기화.
        vipManager.VIPINIT();
        // 튜토리얼 창 활성화.
        tutorialManager.gameObject.SetActive(true);
        /// 튜토리얼 스타트하면 ATK_Lv++;
        tutorialManager.TutoStart();
    }

    private void Awake()
    {
        // 프레임레이트 고정
        Application.targetFrameRate = 60;
        // 오디오 매니저 세팅
        audioManager.AudioInit();


        //// 렉걸리는 것들 켰다가 꺼줌.
        //for (int i = 0; i < AllObject.Length; i++)
        //{
        //    AllObject[i].SetActive(true);
        //}
        /// 
        if (isDebugMode)
        {
            GPGSManager.isUserLogin = true;
            return;
        }

        ///
        /// 여기 아래는 모바일 기기에서만 적용하게끔.
        ///
        // 로그 비활성화
        Debug.unityLogger.logEnabled = false;
        // 화면 꺼짐 방지
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    readonly DoubleToStringNum dts = new DoubleToStringNum();

    private void Start()
    {
        //if (PlayerPrefsManager.GetInstance().IsEnglish)
        //{
        //    leanObject.SetActive(false);
        //    leanLocalization.SetCurrentLanguage("English");
        //}
        //else
        //{
        //    leanObject.SetActive(true);
        //    leanLocalization.SetCurrentLanguage("Korean");
        //}

        ///// 오디오 매니저 세팅
        //audioManager.AudioSetting();
        ///// 튜토리얼 세팅
        //tutorialManager.FakeloadingOnOff(true);

        ///// 데이터 세이브가 일어났다면 있던 데이터 지워줌
        //if (PlayerPrefs.GetInt("isDataSaved", 0) == 1)
        //{
        //    /// 닉네임 설정 준비중이면?
        //    tmpS2 = PlayerPrefs.GetInt("isBeginingS2", 0);
        //    /// 유료 결제 내역 복구
        //    tmpVIP = PlayerPrefs.GetInt("VIP", 0);

        //    Debug.LogError("데이터 세이브가 일어났다면 있던 데이터 지워줌");

        //    PlayerPrefs.DeleteAll();
        //    PlayerPrefs.Save();
        //    /// 0.6초 뒤에 원래 성능 복구
        //    Invoke(nameof(InvoAwaker), 0.3f);
        //}
        //else
        //{
        //    // 튜토리얼 창 활성화.
        //    tutorialManager.gameObject.SetActive(true);
        //    /// 튜토리얼 스타트하면 ATK_Lv++;
        //    tutorialManager.TutoStart();
        //    // VIP 계급 초기화.
        //    vipManager.VIPINIT();
        //}


        StartCoroutine(SetPart_01());
    }

    /// <summary>
    /// 테스트 환경에서 언어 고르면 true
    /// </summary>
    bool isStartLang;

    public void SetStartLang(bool isCheck)
    {
        PlayerPrefsManager.GetInstance().isDataLoaded = true;

        if (PlayerPrefsManager.GetInstance().IsEnglish)
        {
            PlayerPrefsManager.GetInstance().IsEnglish = false;
            leanLocalization.SetCurrentLanguage("Korean");
        }
        else
        {
            PlayerPrefsManager.GetInstance().IsEnglish = true;
            leanLocalization.SetCurrentLanguage("English");
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }


    IEnumerator SetPart_01()
    {
        //while (!isStartLang)
        //{
        //    yield return new WaitForFixedUpdate();
        //}


        if (PlayerPrefsManager.GetInstance().IsEnglish)
        {
            //leanObject.SetActive(false);
            leanLocalization.SetCurrentLanguage("English");
        }
        else
        {
            //leanObject.SetActive(true);
            leanLocalization.SetCurrentLanguage("Korean");
        }

        /// 오디오 매니저 세팅
        audioManager.AudioSetting();
        /// 튜토리얼 세팅
        tutorialManager.FakeloadingOnOff(true);

        /// 데이터 세이브가 일어났다면 있던 데이터 지워줌
        if (PlayerPrefs.GetInt("isDataSaved", 0) == 1)
        {
            /// 닉네임 설정 준비중이면?
            tmpS2 = PlayerPrefs.GetInt("isBeginingS2", 0);
            /// 유료 결제 내역 복구
            tmpVIP = PlayerPrefs.GetInt("VIP", 0);

            Debug.LogError("데이터 세이브가 일어났다면 있던 데이터 지워줌");

            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            /// 0.6초 뒤에 원래 성능 복구
            Invoke(nameof(InvoAwaker), 0.3f);
        }
        else
        {
            // 튜토리얼 창 활성화.
            tutorialManager.gameObject.SetActive(true);
            /// 튜토리얼 스타트하면 ATK_Lv++;
            tutorialManager.TutoStart();
            // VIP 계급 초기화.
            vipManager.VIPINIT();
        }





        yield return new WaitForSeconds(0.4f);

        //튜토리얼 안했어?? 데이터 올 리셋
        //if (!PlayerPrefsManager.GetInstance().isFristGameStart) PlayerPrefs.DeleteAll();

        //for (int i = 0; i < AllObject.Length; i++)
        //{
        //    AllObject[i].SetActive(false);
        //}
        // GPSS 긁어오기
        GetComponent<GPGS_Linker>().Init();

        // CSV 시트 긁기
        PlayerPrefsManager.GetInstance().InitMegaDamData();
        //PlayerPrefsManager.GetInstance().InitMuganData();
        


        PlayerPrefsManager.GetInstance().LoadWeaponInfo();
        PlayerPrefsManager.GetInstance().LoadquestInfo();
        PlayerPrefsManager.GetInstance().LoaduniformData();

        ///
        ///     추가된 퀘스트 
        ///
        PlayerPrefsManager.GetInstance().LoadquestInfo2();
        PlayerPrefsManager.GetInstance().LoadquestInfo3();
        PlayerPrefsManager.GetInstance().LoadquestInfo4();
        PlayerPrefsManager.GetInstance().LoadquestInfo5();
        PlayerPrefsManager.GetInstance().LoadquestInfo6();
        ///
        ///     추가된 퀘스트 
        ///

        tutorialMissionManager.LoadMissionInfo();       /// 튜토리얼 미션 불러오기

        while (!tutorialMissionManager.isLoadMission)
        {
            yield return new WaitForFixedUpdate();
        }
        // 로딩 다 될때까지 무한 대기
        while (!PlayerPrefsManager.GetInstance().isReadyQuest)
        {
            yield return new WaitForFixedUpdate();
        }
        while (!PlayerPrefsManager.GetInstance().isReadyWeapon)
        {
            yield return new WaitForFixedUpdate();
        }
        while (!PlayerPrefsManager.GetInstance().isReadyQuest2)
        {
            yield return new WaitForFixedUpdate();
        }
        while (!PlayerPrefsManager.GetInstance().isReadyQuest3)
        {
            yield return new WaitForFixedUpdate();
        }
        while (!PlayerPrefsManager.GetInstance().isReadyQuest4)
        {
            yield return new WaitForFixedUpdate();
        }
        while (!PlayerPrefsManager.GetInstance().isReadyQuest5)
        {
            yield return new WaitForFixedUpdate();
        }
        while (!PlayerPrefsManager.GetInstance().isReadyQuest6)
        {
            yield return new WaitForFixedUpdate();
        }

        PlayerPrefsManager.GetInstance().LoadDayLimitData(); /// 일간 주간 월간 데이터 로드

        /// 일 주 월 데이터 로딩 다 될때까지 무한 대기
        while (!PlayerPrefsManager.GetInstance().isReadyDayLimit)
        {
            yield return new WaitForFixedUpdate();
        }

        /// 일 주 월 데이터 로딩 다 될때까지 무한 대기
        while (!PlayerPrefsManager.GetInstance().isReadySpinReset)
        {
            yield return new WaitForFixedUpdate();
        }
        /// 뉴 패키지 상점 내용물 추가
        /// 뉴 패키지 상점 내용물 추가
        newsObject.SetActive(true);

        for (int i = 0; i < news.Length; i++)
        {
            news[i].StartBongbong();
            yield return new WaitForFixedUpdate();
        }

        /// 뉴 패키지 상점 내용물 추가
        /// 뉴 패키지 상점 내용물 추가



        /// 방패 리스트 추가
        /// 방패 리스트 추가

        shieldManager.InitShieldInfo(); // LoadWeaponInfo() 에서 추가 안됐다면

        tutorialMissionManager.InitMissionInfo();  // LoadMissionInfo() 에서 추가 안됐다면

        /// 방패 리스트 추가
        /// 방패 리스트 추가


        string tmpPrice;
        string weaponPrice;
        bool isUnlock;

        // 무기 리스트와 퀘스트 리스트가 비어 있다면?
        if (PlayerPrefsManager.GetInstance().weaponInfo.Count == 0)
        {
            // 무기 50개까지 레벨 0 초기화.
            for (int i = 0; i < 50; i++)
            {
                tmpPrice = ((i + 1) * 100).ToString();
                weaponPrice = dts.fDoubleToStringNumber(tmpPrice);
                isUnlock = false;

                if (i == 0) isUnlock = true;

                PlayerPrefsManager.GetInstance().AddWeaponData(0, weaponPrice, 0, isUnlock);
            }
        }

        /// 추가된 무기 리스트
        if (PlayerPrefsManager.GetInstance().weaponInfo.Count == 50 || PlayerPrefsManager.GetInstance().weaponInfo.Count == 65)
        {
            // 무기 50개 이후 레벨 0 초기화.
            for (int i = 50; i < 70; i++)
            {
                tmpPrice = ((i + 1) * 100).ToString();
                weaponPrice = dts.fDoubleToStringNumber(tmpPrice);
                isUnlock = false;

                PlayerPrefsManager.GetInstance().AddWeaponData(0, weaponPrice, 0, isUnlock);
            }
        }

        /// 추가된 무기 리스트 0517
        if (PlayerPrefsManager.GetInstance().weaponInfo.Count == 70)
        {
            // 무기 50개 이후 레벨 0 초기화.
            for (int i = 70; i < 80; i++)
            {
                tmpPrice = ((i + 1) * 100).ToString();
                weaponPrice = dts.fDoubleToStringNumber(tmpPrice);
                isUnlock = false;

                PlayerPrefsManager.GetInstance().AddWeaponData(0, weaponPrice, 0, isUnlock);
            }

        }

        /// 추가된 무기 리스트 0529
        if (PlayerPrefsManager.GetInstance().weaponInfo.Count == 80)
        {
            // 무기 50개 이후 레벨 0 초기화.
            for (int i = 80; i < 90; i++)
            {
                tmpPrice = ((i + 1) * 100).ToString();
                weaponPrice = dts.fDoubleToStringNumber(tmpPrice);
                isUnlock = false;

                PlayerPrefsManager.GetInstance().AddWeaponData(0, weaponPrice, 0, isUnlock);
            }
        }

        /// 추가된 무기 리스트 0622
        if (PlayerPrefsManager.GetInstance().weaponInfo.Count == 90)
        {
            // 무기 50개 이후 레벨 0 초기화.
            for (int i = 90; i < 100; i++)
            {
                tmpPrice = ((i + 1) * 100).ToString();
                weaponPrice = dts.fDoubleToStringNumber(tmpPrice);
                isUnlock = false;

                PlayerPrefsManager.GetInstance().AddWeaponData(0, weaponPrice, 0, isUnlock);
            }
            //무기 정보 pref에 저장.
            PlayerPrefsManager.GetInstance().SaveWeaponInfo();
        }

        /// 유니폼 데이터 초기화.
        if (PlayerPrefsManager.GetInstance().uniformInfo.Count == 0)
        {
            // 유니폼 초기화
            for (int i = 0; i < 7; i++)
            {
                PlayerPrefsManager.GetInstance().AdduniformData(0, 0, 0, 0);
            }
            //pref에 저장.
            PlayerPrefsManager.GetInstance().SaveuniformData();
        }

        // 그리고!!!! 
        /// 퀘스트 관리용 초기화.
        if (PlayerPrefsManager.GetInstance().questInfo.Count == 0)
        {
            PlayerPrefsManager.GetInstance().AddQuestData();
        }
        /// 퀘스트 관리용 초기화. ~~~ 업데이트 05-06
        if (PlayerPrefsManager.GetInstance().questInfo2.Count == 0)
        {
            PlayerPrefsManager.GetInstance().AddQuestData2();
        }
        /// 퀘스트 관리용 초기화. ~~~ 업데이트 05-17
        if (PlayerPrefsManager.GetInstance().questInfo3.Count == 0)
        {
            PlayerPrefsManager.GetInstance().AddQuestData3();
        }
        /// 퀘스트 관리용 초기화. ~~~ 업데이트 05-29
        if (PlayerPrefsManager.GetInstance().questInfo4.Count == 0)
        {
            PlayerPrefsManager.GetInstance().AddQuestData4();
        }
        /// 퀘스트 관리용 초기화. ~~~ 업데이트 06-22
        if (PlayerPrefsManager.GetInstance().questInfo5.Count == 0)
        {
            PlayerPrefsManager.GetInstance().AddQuestData5();
        }
        /// 퀘스트 관리용 초기화. ~~~ 업데이트 07-16
        if (PlayerPrefsManager.GetInstance().questInfo6.Count == 0)
        {
            PlayerPrefsManager.GetInstance().AddQuestData6();
        }
        /// 부스터 - 획득 골드 2배
        PlayerPrefsManager.GetInstance().isGoldTriple = false;
        /// 부스터 -공격력 2배로 변경
        PlayerPrefsManager.GetInstance().isBoosterMattzip = false;

        /// 체력 두자리 오류 해결
        string errorHp = PlayerPrefs.GetString("Mat_currentHP");

        if (errorHp.Contains(".")) // 네자리수 이상?
        {
            string[] sNumberList = errorHp.Split('.');

            /// 00A 인가 000A 인가 판별
            if (sNumberList[1].Length == 3)
            {
                string alpa = sNumberList[1].Substring(0, 2); // 00K 에서 00만 남기기.
                string beta = sNumberList[1].Substring(2); // 00K 에서 K만 남기기.
                string sResult = sNumberList[0] + "." + alpa + "0" + beta;
                // 고쳐줌.
                PlayerPrefsManager.GetInstance().Mat_currentHP = sResult;
            }
        }

        /// 디버그 모드 아닐때만  안드로이드 기능 데려오기.
        if (!isDebugMode)
        {
            configManager = GameObject.Find("ConfigManager").GetComponent<ConfigManager>();
        }
        
        /// 뉴 패키지 상점 화면 끔
        newsObject.SetActive(false);



        ///
        ///
        ///  새로 추가 되는 초기 값 여기다가 적어!!!
        ///
        ///

        PlayerPrefs.SetString("Dia_HP_PER_UP", "1");
        PlayerPrefs.SetString("Dia_ATK_PER_UP", "1");
        //
        PlayerPrefs.SetString("CRC_UP", "1");
        PlayerPrefs.SetString("CRD_UP", "1");
        PlayerPrefs.SetString("Dia_CRC_UP", "1");
        PlayerPrefs.SetString("Dia_CRD_UP", "1");

        //0601
        if (PlayerPrefs.GetString("diaBuyWeaponList") == "" || !PlayerPrefs.HasKey("diaBuyWeaponList"))
            PlayerPrefs.SetString("diaBuyWeaponList", "525*");

        //0622
        if (PlayerPrefs.GetString("InfiPersonalRecord") == "0" || !PlayerPrefs.HasKey("InfiPersonalRecord"))
            PlayerPrefs.SetString("InfiPersonalRecord", "0*0*0*0*0*0*0*0*0*0*");

        if (PlayerPrefs.GetInt("MaxGet_MuganTop2") == 0 || !PlayerPrefs.HasKey("MaxGet_MuganTop2"))
            PlayerPrefs.SetInt("MaxGet_MuganTop2", 1);

        PlayerPrefs.Save();

        //// 골드 알파벳
        //string oldValue = PlayerPrefs.GetString("gold", "0");
        //double dGold = dts.PanByulGi(oldValue);

        //float로 저장된 다이아도 double로 바꿔주기
        //double dDia = PlayerPrefs.GetFloat("dDiamond", 0);
        //Debug.LogWarning("dDia " + dDia);
        //국밥 알파벳
        //oldValue = PlayerPrefs.GetString("gupbap", "0");
        //double dGupbab = dts.PanByulGi(oldValue);

        //// 체력
        //oldValue = PlayerPrefs.GetString("Mat_MaxHP", "100");
        //PlayerPrefsManager.GetInstance().Mat_MaxHP = dts.PanByulGi(oldValue).ToString();
        ////Debug.LogError("초반 맥스" + dts.PanByulGi(oldValue).ToString());
        //PlayerPrefsManager.GetInstance().Mat_currentHP = PlayerPrefsManager.GetInstance().Mat_MaxHP;

        //// 체력 회복량
        //oldValue = PlayerPrefs.GetString("Mat_Recov", "1");
        //PlayerPrefsManager.GetInstance().Mat_Recov = dts.PanByulGi(oldValue).ToString();

        //PlayerPrefs.SetString("gold", dGold.ToString("f0"));
        //PlayerPrefs.SetString("diamond", dDia.ToString("f0"));
        //PlayerPrefs.SetString("gupbap", dGupbab.ToString("f0"));
        ////쌀밥
        //if (!PlayerPrefs.HasKey("ssalbap")) 
        //    PlayerPrefs.SetString("ssalbap", "0");
        //if (PlayerPrefs.GetString("ssalbap") == "") 
        //    PlayerPrefs.SetString("ssalbap", "0");

        ///
        ///
        /// 0517 재화 단위 해제
        /// 
        ///

        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_01_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_01_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_02_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_02_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_03_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_03_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_04_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_04_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_05_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_05_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_06_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_06_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_07_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_07_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_08_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_08_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_09_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_09_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_10_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_10_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_11_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_11_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_12_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_12_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_13_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_13_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_14_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_14_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_15_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_15_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_16_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_16_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_17_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_17_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_18_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_18_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_19_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_19_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_20_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_20_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_21_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_21_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_22_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_22_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_23_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_23_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_24_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_24_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_25_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_25_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_26_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_26_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_27_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_27_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_28_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_28_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_29_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_29_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_30_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_30_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_31_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_31_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_32_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_32_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_33_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_33_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_34_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_34_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_35_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_35_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_36_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_36_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_37_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_37_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_38_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_38_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_39_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_39_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_40_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_40_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_41_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_41_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_42_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_42_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_43_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_43_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_44_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_44_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_45_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_45_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_46_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_46_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_47_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_47_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_48_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_48_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_49_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_49_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo[0].Pun_50_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo[0].Pun_50_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo2[0].Pun_51_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo2[0].Pun_51_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo2[0].Pun_52_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo2[0].Pun_52_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo2[0].Pun_53_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo2[0].Pun_53_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo2[0].Pun_54_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo2[0].Pun_54_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo2[0].Pun_55_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo2[0].Pun_55_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo2[0].Pun_56_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo2[0].Pun_56_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo2[0].Pun_57_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo2[0].Pun_57_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo2[0].Pun_58_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo2[0].Pun_58_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo2[0].Pun_59_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo2[0].Pun_59_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo2[0].Pun_60_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo2[0].Pun_60_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo2[0].Pun_61_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo2[0].Pun_61_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo2[0].Pun_62_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo2[0].Pun_62_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo2[0].Pun_63_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo2[0].Pun_63_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo2[0].Pun_64_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo2[0].Pun_64_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo2[0].Pun_65_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo2[0].Pun_65_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo2[0].Pun_66_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo2[0].Pun_66_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo2[0].Pun_67_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo2[0].Pun_67_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo2[0].Pun_68_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo2[0].Pun_68_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo2[0].Pun_69_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo2[0].Pun_69_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo2[0].Pun_70_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo2[0].Pun_70_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo3[0].Pun_71_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo3[0].Pun_71_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo3[0].Pun_72_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo3[0].Pun_72_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo3[0].Pun_73_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo3[0].Pun_73_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo3[0].Pun_74_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo3[0].Pun_74_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo3[0].Pun_75_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo3[0].Pun_75_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo3[0].Pun_76_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo3[0].Pun_76_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo3[0].Pun_77_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo3[0].Pun_77_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo3[0].Pun_78_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo3[0].Pun_78_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo3[0].Pun_79_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo3[0].Pun_79_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo3[0].Pun_80_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo3[0].Pun_80_Cnt = 19;
        //
        if (PlayerPrefsManager.GetInstance().questInfo4[0].Pun_81_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo4[0].Pun_81_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo4[0].Pun_82_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo4[0].Pun_82_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo4[0].Pun_83_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo4[0].Pun_83_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo4[0].Pun_84_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo4[0].Pun_84_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo4[0].Pun_85_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo4[0].Pun_85_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo4[0].Pun_86_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo4[0].Pun_86_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo4[0].Pun_87_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo4[0].Pun_87_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo4[0].Pun_88_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo4[0].Pun_88_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo4[0].Pun_89_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo4[0].Pun_89_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo4[0].Pun_90_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo4[0].Pun_90_Cnt = 19;
        //
        if (PlayerPrefsManager.GetInstance().questInfo5[0].Pun_91_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo5[0].Pun_91_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo5[0].Pun_92_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo5[0].Pun_92_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo5[0].Pun_93_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo5[0].Pun_93_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo5[0].Pun_94_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo5[0].Pun_94_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo5[0].Pun_95_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo5[0].Pun_95_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo5[0].Pun_96_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo5[0].Pun_96_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo5[0].Pun_97_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo5[0].Pun_97_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo5[0].Pun_98_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo5[0].Pun_98_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo5[0].Pun_99_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo5[0].Pun_99_Cnt = 19;
        if (PlayerPrefsManager.GetInstance().questInfo5[0].Pun_100_Cnt >= 19) PlayerPrefsManager.GetInstance().questInfo5[0].Pun_100_Cnt = 19;

        // 배경 깃발 초기화.
        flagManager.InitFlags();
        // 동료 초기화.
        friendManager.InitFriend();
        // 캐릭터 레벨 초기화
        charactherMang.Characther_UP_Update();
        // 강화 페이지 레벨 초기화.
        groggyManager.PowerUP_Init();
        // 체력 회복 자동 코루틴 <시작>
        groggyManager.HP_AutoReco(true);
        // 오프라인 보상 띄우기
        offlineManager.OfflineInit();
        // 미션 이미지 초기화.
        questManager.SpecialMissonImgInit();
        // 펀치 초기화
        punchManager.PunchInit();
        // 무기 인덱스 저장된거 불러오기
        tapToSpawnLimit.PunchIndexUpdate(PlayerPrefsManager.GetInstance().PunchIndex);

        //지갑 표시
        UserWallet.GetInstance().ShowAllMoney();
        // 초기 HP/공격력/맷집  표기
        groggyManager.HP_barInit();
        // 유니폼 초기화
        charactherMang.UniformInit();
        /// 인피니티 스크롤 호출 
        InfinityContent.ListStart();
        InfinityQuest.ListStart();

        /// 방패도 동일하게 될련지
        InfinityShield.ListStartShield();

    }


    public void RateMePlz()
    {
        configManager.RateApp();
    }


    public void Email()
    {
        configManager.ContactUs();
    }
}
