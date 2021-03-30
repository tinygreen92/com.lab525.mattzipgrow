using EasyMobile;
using UnityEngine;
using UnityEngine.UI;

public class UserWallet : MonoBehaviour
{
    private static UserWallet instance;


    public PunchManager pm;
    [Header("-캐릭터 정보창")]
    public Text ATK_Text;
    public Text HP_Text;
    public Text HP_Recov_Text;
    public Text Deffence_Text;
    public Text CRC_Text;
    public Text CRD_Text;

    [Header("-탑 패널 표기 시")]
    public Text KimchiText;
    public Text GoldText;
    public Text DiaText;
    public Text MilkText;
    public Text KeyText;
    public Text SSalText;
    //

    [Header("-맷집 게이지는 하단에")]
    public Text MattText;
    //public Text MattzipText;
    public Text DefendMattzip_text;
    //[Header("- 맷집 증가량 테스트 Text")]
    //public Text Matt_Test;
    //public Text DefenceText;
    //public string PunchDPS = "20";

    public void RestartGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        /// 리스타트
        RestartAppForAOS();
    }

    public void Ex_MattHitMax()
    {
        PlayerPrefsManager.GetInstance().Mat_Mattzip_Hit += 3E+35f;
    }


    public void AllMoneyTest()
    {
        PlayerPrefsManager.GetInstance().gold = dts.AddStringDouble(PlayerPrefsManager.GetInstance().gold, "3.33E+302");
        PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(PlayerPrefsManager.GetInstance().gupbap, "3.33E+302");
        PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(PlayerPrefsManager.GetInstance().ssalbap, "3.33E+302");
        PlayerPrefsManager.GetInstance().key += 1000;
        PlayerPrefsManager.GetInstance().ticket += 1000;
        PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 100000000000000f);

        ShowAllMoney();
    }

    public void All_IN_ONE()
    {
        PlayerPrefsManager.GetInstance().VIP = 625;
        // VIP 승급에 따라 구매버튼 잠그기.
        VIPManager.GetInstance().VIPINIT();
    }


    /// <summary>
    /// 안드로이드 네이티브 코드
    /// </summary>
    void RestartAppForAOS()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //play모드를 false로.
#else
        AndroidJavaObject AOSUnityActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject baseContext = AOSUnityActivity.Call<AndroidJavaObject>("getBaseContext");
        AndroidJavaObject intentObj = baseContext.Call<AndroidJavaObject>("getPackageManager").Call<AndroidJavaObject>("getLaunchIntentForPackage", baseContext.Call<string>("getPackageName"));
        AndroidJavaObject componentName = intentObj.Call<AndroidJavaObject>("getComponent");
        AndroidJavaObject mainIntent = intentObj.CallStatic<AndroidJavaObject>("makeMainActivity", componentName);
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        mainIntent = mainIntent.Call<AndroidJavaObject>("addFlags", intentClass.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK"));
        mainIntent = mainIntent.Call<AndroidJavaObject>("addFlags", intentClass.GetStatic<int>("FLAG_ACTIVITY_CLEAR_TASK"));
        baseContext.Call("startActivity", mainIntent);
        AndroidJavaClass JavaSystemClass = new AndroidJavaClass("java.lang.System");
        JavaSystemClass.CallStatic("exit", 0);
#endif
    }


    public static UserWallet GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// 모든 재화 갱신.
    /// </summary>
    public void ShowAllMoney()
    {
        ShowUserKimchi();
        ShowUserGold();
        ShowUserDia();
        ShowUserMilk();
        //ShowUserKey();
        PlayerPrefsManager.GetInstance().key += 0;
        ShowUserMatZip();
        ShowUserATK();
        ShowUserHP();
        ShowUserDeffence();
        ShowUserSSalbap();
        ShowUserCritical_2_();
    }


    /// <summary>
    /// 깍두기 최종 획득량 = playerDPS * 유물 깍두기 획득 증가 %
    /// </summary>
    public void ShowUserKimchi()
    {
        string value = PlayerPrefsManager.GetInstance().Kimchi;
        KimchiText.text = dts.fDoubleToGoldOutPut(value);
    }


    /// 골드
    public void ShowUserGold()
    {
        string value = PlayerPrefsManager.GetInstance().gold;
        GoldText.text = dts.fDoubleToGoldOutPut(value);
    }

    /// 다이아
    public void ShowUserDia()
    {
        double dDiamond = PlayerPrefs.GetFloat("dDiamond", 0);
        //
        DiaText.text = SeetheNatural(dDiamond);
        /// 정보 갱신
        PlayerPrefsManager.GetInstance().SavequestInfo();
        PlayerPrefsManager.GetInstance().SavequestInfo2();
        PlayerPrefsManager.GetInstance().SavequestInfo3();
        PlayerPrefsManager.GetInstance().SavequestInfo4();
        PlayerPrefsManager.GetInstance().SavequestInfo5();
        PlayerPrefsManager.GetInstance().SavequestInfo6();

    }

    /// 국밥
    public void ShowUserMilk()
    {
        string value = PlayerPrefsManager.GetInstance().gupbap;
        MilkText.text = SeetheNatural(double.Parse(value));
        /// 국밥에 따라 업글 버튼 새로 고침
        pm.MilkUpdateCup();
    }

    /// 쌀밥
    public void ShowUserSSalbap()
    {
        string value = PlayerPrefsManager.GetInstance().ssalbap;
        SSalText.text = SeetheNatural(double.Parse(value));
    }

    /// 열쇠
    //public void ShowUserKey()
    //{
    //    var value = PlayerPrefsManager.GetInstance().key;
    //    KeyText.text = value + "/20";
    //    PlayerPrefs.Save();
    //}

    /// 우측 하단 맷집 표시기랑 개인 스탯
    public void ShowUserMatZip()
    {
        /// 계산 완료 뽑아먹기
        string value = SeetheTruth(double.Parse(SeetheTruth(PlayerPrefsManager.GetInstance().Mat_Mattzip)));

        ///// 맷집 별로 방어 게이지 곱 연산
        //int mattmp = Mathf.FloorToInt(float.Parse(value) * 0.0001f);
        //if (mattmp != PlayerPrefs.GetFloat("VUVUBA", 0))
        //{
        //    PlayerPrefsManager.GetInstance().Cilcked_Cnt_MattZip = 1.0f + (mattmp * 0.25f);
        //    PlayerPrefs.SetFloat("VUVUBA", mattmp);
        //}

        //Debug.Log("인트 ; " + mattmp);
        //Debug.Log(PlayerPrefsManager.GetInstance().Cilcked_Cnt_MattZip);

        MattText.text = value;
        //MattzipText.text = value;
        DefendMattzip_text.text = value;
    }

    /// 캐릭터 정보 체력
    public void ShowUserHP()
    {
        string value = SeetheTruth(double.Parse(PlayerPrefsManager.GetInstance().Mat_MaxHP));
        HP_Text.text = value;

    }

    public void ShowUserHP_Recov()
    {
        string value = SeetheTruth(double.Parse(PlayerPrefsManager.GetInstance().Mat_Recov));
        HP_Recov_Text.text = value + " /s";

    }


    public void ShowUserDeffence()
    {
        string value = SeetheTruth(double.Parse(PlayerPrefsManager.GetInstance().GetPlayerDefence()));
        Deffence_Text.text = value;

    }

    /// 캐릭터 정보 공격력
    public void ShowUserATK()
    {
        string value = SeetheTruth(double.Parse(SeetheTruth(PlayerPrefsManager.GetInstance().PlayerDPS)));
        ATK_Text.text = value;
    }






    /// <summary>
    /// 캐릭터 정보 크확 크댐
    /// </summary>
    public void ShowUserCritical_2_()
    {
        string value = SeetheTruth(double.Parse(SeetheTruth(PlayerPrefsManager.GetInstance().CriticalDPS)));
        // 크리티컬 대미지
        CRD_Text.text = value;
        // 크리티컬 확률
        CRC_Text.text = PlayerPrefsManager.GetInstance().Critical_Per + "%";
    }






    /// <summary>
    /// 소수점 자리 날려버려
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public string GetMattzipForCul(string value)
    {
        string result = "";
        if (value.Contains("."))
        {
            string[] sNumberList = value.Split('.');
            result = sNumberList[0];
            //Debug.LogWarning("소수점 뽑아먹기 : " + result);
        }
        else
        {
            result = value;
        }
        //Debug.LogWarning("맷집력 : " + result);

        return result;
    }


    /// <summary>
    /// 실제 유저가 볼때는 .00 단위까지만 보이게
    /// 99.888A 가 실제 값 
    /// 99.88A 는 보이는 값
    /// </summary>
    /// <param name="tmpStr"></param>
    public string SeetheTruth(string tmpStr)
    {
        string sResult;

        if (tmpStr.Contains(".")) // 네자리수 이상? 1.000A
        {
            string[] sNumberList = tmpStr.Split('.');
            string tmps = sNumberList[1].Substring(0, 2); // 000K 에서 00만 남기기
            string spmt = sNumberList[1].Substring(3); // 000K 에서 K만 남기기

            sResult = sNumberList[0] + "." + tmps + spmt;
        }
        else // 999 이하.
        {
            sResult = tmpStr + ".00";
        }

        return sResult;
    }


    /// <summary>
    /// 실제 유저가 볼때는 .00 단위까지만 보이게
    /// 99.888A 가 실제 값 
    /// 99.88A 는 보이는 값
    /// </summary>
    /// <param name="tmpStr"></param>
    public string SeetheTruth(double tmpDouble)
    {
        string tmpStr = dts.fDoubleToStringNumber(tmpDouble);

        if (tmpStr.Contains(".") && tmpDouble >= 1000d) // 네자리수 이상? 1.000A
        {
            string[] sNumberList = tmpStr.Split('.');
            string tmps = sNumberList[1].Substring(0, 2); // 000K 에서 00만 남기기
            string spmt = sNumberList[1].Substring(3); // 000K 에서 K만 남기기

            tmpStr = sNumberList[0] + "." + tmps + spmt;
        }
        else // 999 이하.
        {
            tmpStr = tmpDouble.ToString("f2");
        }

        return tmpStr;
    }

    ///// <summary>
    ///// 스트링 쓰니까 이상한대??
    ///// </summary>
    ///// <param name="tmpStr"></param>
    //public string SeetheNatural(string tmpStr)
    //{
    //    string sResult = string.Empty;

    //    if (tmpStr.Contains(".")) // 네자리수 이상? 1.000A
    //    {
    //        string[] sNumberList = tmpStr.Split('.');
    //        string tmps = sNumberList[1].Substring(0, 2); // 000K 에서 00만 남기기
    //        string spmt = sNumberList[1].Substring(3); // 000K 에서 K만 남기기

    //        sResult = sNumberList[0] + "." + tmps + spmt;
    //    }
    //    else // 999 이하.
    //    {
    //        sResult = tmpStr;
    //    }
    //    return sResult;

    //}

    /// <summary>
    /// 실제 유저가 볼때는 1의 단위 안보이게 만들어주는거
    /// 10.000 가 실제 값 
    /// 10 는 보이는 값
    /// </summary>
    /// <param name="tmpStr"></param>
    public string SeetheNatural(double tmpDouble)
    {
        if (tmpDouble == 0) return "0";

        string tmpStr = dts.fDoubleToStringNumber(tmpDouble);

        string sResult = string.Empty;

        if (tmpStr.Contains(".")) // 네자리수 이상? 1.000A
        {
            string[] sNumberList = tmpStr.Split('.');
            string tmps = sNumberList[1].Substring(0, 2); // 000K 에서 00만 남기기
            string spmt = sNumberList[1].Substring(3); // 000K 에서 K만 남기기

            sResult = sNumberList[0] + "." + tmps + spmt;
        }
        else // 999 이하.
        {
            sResult = tmpStr;
        }
        return sResult;

    }

    readonly DoubleToStringNum dts = new DoubleToStringNum();
    [Header("-타격 상자 텍스트")]
    public Text Desc_Text; // "뫄뫄를 획득했다!" 
    public Text Gold_5_Text; // 수치를 표기
    public GameObject GoldIcon; // 아이콘들
    public GameObject GupIcon; // 아이콘들
    public GameObject SalIcon; // 아이콘들
    /// <summary>
    /// 펀치시 드롭되는 럭키 박스에서 호출시
    /// 0.골드 10배 텍스트에 출력 80%
    /// 1.국밥 10배 텍스트에 출력 10 %
    /// 2.쌀밥 10배 텍스트에 출력 10%
    /// </summary>
    public void Get_5_Gold()
    {
        GoldIcon.SetActive(false);
        GupIcon.SetActive(false);
        SalIcon.SetActive(false);

        Gatcha = Random.Range(0f, 100f);

        //var goldPer = PlayerPrefsManager.GetInstance().BG_CoinStat;
        //float artiGoldPer = PlayerPrefsManager.GetInstance().Arti_GoldPer * 1.0f;
        float luckyPer = PlayerPrefsManager.GetInstance().Arti_LuckyBoxPer;
        ///골드 획득  계산식 수정 > 골드 획득량 = 맷집 * 1 * (유니폼 + 스킬 + 유물 + 훈련장)
        string tmpDps = PlayerPrefsManager.GetInstance().Mat_Mattzip;
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

        if (Gatcha < 80f)
        {
            GoldIcon.SetActive(true);
            Desc_Text.text = "골드 상자를 획득했다!";
            tmpDps = dts.multipleStringDouble(tmpDps, 5d * (goldPer + (artiGoldPer * 0.01d)));
            Gold_5_Text.text = SeetheNatural(double.Parse(tmpDps));

        }
        else if (Gatcha < 90f)
        {
            GupIcon.SetActive(true);
            Desc_Text.text = "국밥 상자를 획득했다!";

            target = Random.Range(10, 101);

            /// 획득량 % 증가
            double getAmount = (target * (1.0d + (luckyPer * 0.01d)));
            Gold_5_Text.text = getAmount.ToString("f0");

            Debug.LogError("국밥 Gatcha : " + Gatcha);

        }
        else if (Gatcha <= 100f)
        {
            SalIcon.SetActive(true);
            Desc_Text.text = "쌀밥 상자를 획득했다!";

            target = Random.Range(10, 501);
            /// 획득량 % 증가
            double getAmount = (target * (1.0d + (luckyPer * 0.01d)));
            Gold_5_Text.text = getAmount.ToString("f0");

            Debug.LogError("쌀밥 Gatcha : " + Gatcha);


        }


    }
    [HideInInspector]
    public float Gatcha;           // 골드-국밥-쌀밥 뭐 뽑았냐
    [HideInInspector]
    public int target;              //몇개 뽑았냐


    /// <summary>
    /// [펀치 럭키박스] 광고 안보기 버튼 눌렀을때 기본 제공량만 받아먹기
    /// </summary>
    public void RewordGold5()
    {
        Debug.LogError("Punch Lucky Box : " + Gatcha);




        if (Gatcha < 80f)
        {
            //골드 획득
            string tmpGold = PlayerPrefsManager.GetInstance().gold;
            ///골드 획득  계산식 수정 > 골드 획득량 = 맷집 * 1 * (유니폼 + 스킬 + 유물 + 훈련장)
            string value = PlayerPrefsManager.GetInstance().Mat_Mattzip;
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

            value = dts.multipleStringDouble(value, 5d * (goldPer + (artiGoldPer * 0.01d)));
            //
            PlayerPrefsManager.GetInstance().gold = dts.AddStringDouble(double.Parse(tmpGold), double.Parse(value));
            PopUpObjectManager.GetInstance().GettingGoldMessage(double.Parse(value));
            PopUpObjectManager.GetInstance().ShowWarnnigProcess(UserWallet.GetInstance().SeetheNatural(double.Parse(value)) + " 골드를 획득하셨습니다.");

            Gold_5_Text.text = SeetheNatural(double.Parse(value));

        }
        else if (Gatcha < 90f)
        {
            string gupbap = PlayerPrefsManager.GetInstance().gupbap;

            float luckyPer = PlayerPrefsManager.GetInstance().Arti_LuckyBoxPer;
            double getAmount = (target * (1.0d + (luckyPer * 0.01d)));

            PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(gupbap, getAmount.ToString("f0"));
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("국밥 " + getAmount.ToString("f0") + " 그릇을 획득하셨습니다.");

        }
        else if (Gatcha <= 100f)
        {
            string ssal = PlayerPrefsManager.GetInstance().ssalbap;

            float luckyPer = PlayerPrefsManager.GetInstance().Arti_LuckyBoxPer;
            double getAmount = (target * (1.0d + (luckyPer * 0.01d)));

            PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(ssal, getAmount.ToString("f0"));
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("쌀밥 " + getAmount.ToString("f0") + " 그릇을 획득하셨습니다.");

        }

        // 돈 표시
        ShowAllMoney();
    }











    #region <Rewarded Ads> 다이아 상점 무료 다이아 버튼.

    public void DiamondForFreeAB()
    {
        if (PlayerPrefsManager.GetInstance().questInfo[0].daily_LMITABS >= 10)
        {
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("일일 제한 광고 횟수를 모두 소진하셨습니다.");
            return;
        }

        if (Advertising.IsRewardedAdReady(RewardedAdNetwork.AdMob, AdPlacement.Default))
        {
            Advertising.RewardedAdCompleted += BoosterAdsCompleated;
            Advertising.RewardedAdSkipped += BoosterAdsSkipped;

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
    void BoosterAdsCompleated(RewardedAdNetwork network, AdPlacement location)
    {
        Invoke("BoosterAds", 0.5f);

        Advertising.RewardedAdCompleted -= BoosterAdsCompleated;
        Advertising.RewardedAdSkipped -= BoosterAdsSkipped;


    }

    // Event handler called when a rewarded ad has been skipped
    void BoosterAdsSkipped(RewardedAdNetwork network, AdPlacement location)
    {
        Advertising.RewardedAdCompleted -= BoosterAdsCompleated;
        Advertising.RewardedAdSkipped -= BoosterAdsSkipped;

        PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);
    }

    void BoosterAds()
    {

        PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);
        /// 상점 내 무료다이아 광고 시청당 보상 다이아 50개 고정으로 수정
        string targetDia = "50";

        //int RanDia = Random.Range(0, 100);

        //if (RanDia < 5)
        //{
        //    targetDia = "100";
        //}
        //else if (RanDia < 15)
        //{
        //    targetDia = "50";
        //}
        //else if (RanDia < 40)
        //{
        //    targetDia = "20";
        //}
        //else
        //{
        //    targetDia = "10";
        //}


        //다이아 확률
        //PlayerPrefsManager.GetInstance().diamond = dts.AddStringDouble(PlayerPrefsManager.GetInstance().diamond, targetDia);
        float ddd = PlayerPrefs.GetFloat("dDiamond") + float.Parse(targetDia);

        PlayerPrefs.SetFloat("dDiamond", ddd);
        ShowUserDia();

        PopUpObjectManager.GetInstance().ShowWarnnigProcess(targetDia + " 다이아를 획득하셨습니다.");

        /// 퀘스트
        PlayerPrefsManager.GetInstance().questInfo[0].daily_Abs++;

        if (PlayerPrefsManager.GetInstance().questInfo[0].All_Abs < 1000)
        {
            PlayerPrefsManager.GetInstance().questInfo[0].All_Abs++;
        }

        SucessAbsComp();
    }

    #endregion

    [Header("- 일일 제한 10회 텍스트")]
    public Text DailyText;

    /// <summary>
    /// 정상적으로 광고를 시청했다.
    /// </summary>
    void SucessAbsComp()
    {
        PlayerPrefsManager.GetInstance().questInfo[0].daily_LMITABS++;
        DailyText.text = "일일 제한 10회 ( " + PlayerPrefsManager.GetInstance().questInfo[0].daily_LMITABS + " / 10 )";
    }


}
