using EasyMobile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfflineManager : MonoBehaviour
{
    [Header("- 돈/국/밥")]
    public Text rewordText;
    public Text GupBapText;
    public Text SSalText;
    public GameObject rewordPOPup;

    [Header("- 일반게임 히트 바디")]
    public GameObject hitBody;

    DoubleToStringNum dts = new DoubleToStringNum();

    // 조작 불가 타이머 스탬프
    private DateTime unbiasedTimerEndTimestamp;
    DateTime unbiasedRemaining;

    private bool isPaused = false; // 앱 일시정지

    /// <summary>
    /// 앱 일시정지 일때
    /// </summary>
    /// <param name="pause"></param>
    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            isPaused = true;
            /* 앱이 비활성화 되었을 때 처리 */
            unbiasedRemaining = UnbiasedTime.Instance.Now();
            SaveDateTime(unbiasedRemaining);

        }
        else
        {
            if (isPaused)
            {
                isPaused = false;
                /* 앱이 활성화 되었을 때 처리 */
                /// TODO : 오프라인 보상 처리
                OfflineInit();
            }
        }
    }

    /// <summary>
    /// 앱이 완전히 종료 될때.
    /// </summary>
    void OnApplicationQuit()
    {
        /* 앱이 종료 될 때 처리 */
        unbiasedRemaining = UnbiasedTime.Instance.Now();
        SaveDateTime(unbiasedRemaining);
    }


    void SaveDateTime(DateTime dateTime)
    {
        string tmp = dateTime.ToString("yyyyMMddHHmmss");
        PlayerPrefs.SetString("DateTime", tmp);
        PlayerPrefs.Save();
    }

    DateTime LoadDateTime()
    {
        if(!PlayerPrefsManager.GetInstance().isFristGameStart) 
            return UnbiasedTime.Instance.Now();

        /// 불러올 시간 데이터가 존재한다먄?
        string data = PlayerPrefs.GetString("DateTime", "19000101120000");
        var saveDateTime = DateTime.ParseExact(data, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);

        return saveDateTime;
    }

    public PlayNANOOExample playNANOO;



    string gettingGold;
    double gettingGupBap;
    double gettingSSal;
    float gettingMatt;
    int timeBae = 0;
    DateTime dateTime;

    /// <summary>
    /// 앱 켰을 때 오프라인 보상 
    /// </summary>
    public void OfflineInit()
    {
        /// 세이브 로드로 앱 재실행하면 보상 주지마.
        if (PlayerPrefsManager.GetInstance().isDataLoaded || PlayerPrefs.GetInt("isDataSaved", 0) == 1)
        {
            PlayerPrefsManager.GetInstance().isDataLoaded = false;
            PlayerPrefs.SetInt("isDataSaved", 0);
            PlayerPrefs.Save();
            return;
        }

        //

        // 고소장 켜져있으면 고소장 뛰워줌.
        if (PlayerPrefsManager.GetInstance().isGoingGOSO)
        {
            PlayerPrefsManager.GetInstance().isGroggy = true;
            // 서있는 스프라이트 안보이게 꺼줌
            hitBody.GetComponent<SpriteRenderer>().enabled = false;
            // 그로기 애니메이션 재생
            hitBody.GetComponent<Animation>().Play("Groggy");

            PopUpObjectManager.GetInstance().ShowGoSoProcess();
            return;
        }
        else
        {
            //고소장 아니면 그로기 꺼줌.
            PlayerPrefsManager.GetInstance().isGroggy = false;
        }

        dateTime = LoadDateTime();

        /// 1분 이상 떠나있었다면
        if (dateTime.AddSeconds(61) < UnbiasedTime.Instance.Now())
        {
            TimeSpan resultTime = (UnbiasedTime.Instance.Now() - dateTime);
            // 분 단위로 저장.
            timeBae = (resultTime.Hours * 60) + resultTime.Minutes;

            Debug.LogWarning("오프라인 몇 분? : " + timeBae);

            /// 1분 이하라면 리턴
            if (timeBae == 0) 
                return;
            /// 최대 오프라인 시간은 6시간
            else if (timeBae >= 360)
                timeBae = 360;

            /// start ---------------- 골드 / 국밥 / 쌀밥  ------------------

            // 최근 획득 골드 공식 시작
            gettingGold = PlayerPrefsManager.GetInstance().Mat_Mattzip;

            float ArtiGold = 1.0f 
                + (PlayerPrefsManager.GetInstance().Arti_OffGold * 0.5f) // 오프라인 보상 유물
                + (PlayerPrefsManager.GetInstance().uniformInfo[4].Skill_LV * 0.005f); // 오프라인 보상 강화
            //골드 를 곱해 준다.
            gettingGold = dts.multipleStringDouble(gettingGold, (timeBae * ArtiGold * 20f)); // 분당 10대. 100 = 1000 ;
            gettingGupBap = timeBae * ArtiGold * 2f; // 분당 2개;
            gettingSSal = timeBae  * ArtiGold * 1f; // 분당 1개;


            //획득 골드 만큼 복제해서 팝업 텍스트에 표기
            rewordText.text = $"x {UserWallet.GetInstance().SeetheNatural(double.Parse(gettingGold))}";
            GupBapText.text = $"x {UserWallet.GetInstance().SeetheNatural(gettingGupBap)}";
            SSalText.text = $"x {UserWallet.GetInstance().SeetheNatural(gettingSSal)}";

            // 오프라인 보상창 열어준다.
            rewordPOPup.SetActive(true);
        }
    }




    /// <summary>
    /// 실제로 맷집 게이지 올려주기 광고 안봄 = 1f / 광고 봄 = 2f
    /// </summary>
    /// <param name="_Bae">1f 혹은 5f</param>
    void RewordFriendBae(float _Bae)
    {
        PlayerPrefsManager.GetInstance().Mat_100 += Mathf.CeilToInt(gettingMatt * _Bae);
    }




    #region <Rewarded Ads> 오프라인 광고 뻥튀기

    public void GoldBoxFantasy()
    {
        //PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);

        int vvip = PlayerPrefsManager.GetInstance().VIP;
        //PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);
        if (vvip == 526 || vvip == 625 || vvip == 725 || vvip == 925)
        {
            GetDoubleGold();
            rewordPOPup.SetActive(false);
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
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("광고를 준비중입니다. 잠시 후에 시도해주세요.");
            PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);
        }

    }

    // Event handler called when a rewarded ad has completed
    void GoldBoxAdsCompleated(RewardedAdNetwork network, AdPlacement location)
    {
        rewordPOPup.SetActive(false);

        Invoke("GetDoubleGold", 0.5f);
        Advertising.RewardedAdCompleted -= GoldBoxAdsCompleated;
        Advertising.RewardedAdSkipped -= GoldBoxAdsSkipped;
    }

    // Event handler called when a rewarded ad has been skipped
    void GoldBoxAdsSkipped(RewardedAdNetwork network, AdPlacement location)
    {
        Invoke("GetNormalGold", 0.5f);

        Advertising.RewardedAdCompleted -= GoldBoxAdsCompleated;
        Advertising.RewardedAdSkipped -= GoldBoxAdsSkipped;

        PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);
    }

    public void GetDoubleGold()
    {
        PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);

        var gold = PlayerPrefsManager.GetInstance().gold;

        //Debug.LogWarning("오프라인 gold " + gold);
        //Debug.LogWarning("오프라인 gettingGold " + gettingGold);
        /// 광고 보고 골드 5배에서 2배로 변경
        gettingGold = dts.multipleStringDouble(gettingGold, 2.0f);
        PlayerPrefsManager.GetInstance().gold = dts.AddStringDouble(gold, gettingGold);


        PopUpObjectManager.GetInstance().ShowWarnnigProcess(
            UserWallet.GetInstance().SeetheNatural(double.Parse(gettingGold)) +
            " 골드를 획득하셨습니다.");



        float ArtiGold = 1.0f + (PlayerPrefsManager.GetInstance().Arti_OffGold * 0.5f) + (PlayerPrefsManager.GetInstance().uniformInfo[4].Skill_LV * 0.005f);
        //골드 를 곱해 준다.
        gettingGupBap = timeBae * 2f * ArtiGold * 5f; // 분당 2개;
        gettingSSal = timeBae * 1f * ArtiGold * 5f; // 분당 2개;
        /// 국밥이랑 쌀밥 다섯배 -> 2배로 올려
        PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(double.Parse(PlayerPrefsManager.GetInstance().gupbap), gettingGupBap); // 분당 2개;
        PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(double.Parse(PlayerPrefsManager.GetInstance().ssalbap), gettingSSal); // 분당 2개;
        /// 맷집 게이지 2배 올려주기
        RewordFriendBae(2f); 


        UserWallet.GetInstance().ShowAllMoney();

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
    /// 광고 안 보면 일반 보상 먹음
    /// </summary>
    public void GetNormalGold()
    {
        var gold = PlayerPrefsManager.GetInstance().gold;
        PlayerPrefsManager.GetInstance().gold = dts.AddStringDouble(gold, gettingGold);
        /// 국밥이랑 쌀밥
        PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(double.Parse(PlayerPrefsManager.GetInstance().gupbap), gettingGupBap); // 분당 2개;
        PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(double.Parse(PlayerPrefsManager.GetInstance().ssalbap), gettingSSal); // 분당 1개;
        /// 맷집 게이지 1배 올려주기
        RewordFriendBae(1f);
        ///
        PopUpObjectManager.GetInstance().ShowWarnnigProcess(
    UserWallet.GetInstance().SeetheNatural(double.Parse(gettingGold)) +
    " 골드를 획득하셨습니다.");

        UserWallet.GetInstance().ShowAllMoney();
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
