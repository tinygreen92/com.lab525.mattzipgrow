using EasyMobile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Booster_Body : MonoBehaviour
{
    [Header("- 클릭동작 땡겨오기")]
    public TapToSpawnLimit tapToSpawnLimit;
    [Header("- 버프 한번에 몇초 증가?")]
    public double addTime = 300;

    // 조작 불가 타이머 스탬프
    private DateTime unbiasedTimerEndTimestamp;
    TimeSpan unbiasedRemaining;

    private void Awake()
    {
        unbiasedTimerEndTimestamp = ReadTimestamp("Booster_Body", UnbiasedTime.Instance.Now());
    }


    void FixedUpdate()
    {
        if (!PlayerPrefsManager.GetInstance().isReadyQuest && !PlayerPrefsManager.GetInstance().isReadyWeapon) return;


        if (PlayerPrefsManager.GetInstance().VIP == 527 || PlayerPrefsManager.GetInstance().VIP == 625 || PlayerPrefsManager.GetInstance().VIP == 825 || PlayerPrefsManager.GetInstance().VIP == 925)
        {
            // 온/오프 기능
            if (!transform.GetChild(0).gameObject.activeSelf)
            {
                transform.GetChild(1).GetComponent<Text>().text = "공격력버프 ON";
                PlayerPrefsManager.GetInstance().isBoosterMattzip = true;
                UserWallet.GetInstance().ShowUserMatZip();
            }
            else
            {
                transform.GetChild(1).GetComponent<Text>().text = "공격력버프 OFF";
                PlayerPrefsManager.GetInstance().isBoosterMattzip = false;
                UserWallet.GetInstance().ShowUserMatZip();

            }

            return;
        }
        else
        {
            // 시간 소모 
            unbiasedRemaining = unbiasedTimerEndTimestamp - UnbiasedTime.Instance.Now();

            // 타이머가 끝나면?
            if (unbiasedRemaining.TotalSeconds > 0)
            {
                transform.GetChild(0).gameObject.SetActive(false);

                // 온 받은 순간부터 소모 타이머 카운트 다운
                transform.GetChild(1).GetComponent<Text>().text = string.Format("{0:00}:{1:00}:{2:00}", unbiasedRemaining.Hours, unbiasedRemaining.Minutes, unbiasedRemaining.Seconds);


                PlayerPrefsManager.GetInstance().isBoosterMattzip = true;
                UserWallet.GetInstance().ShowUserMatZip();

            }
            else
            {
                transform.GetChild(1).GetComponent<Text>().text = "00:00:00";
                // 온 했다가 시간 다되면 비활성화 커버 덮어줌.
                transform.GetChild(0).gameObject.SetActive(true);

                PlayerPrefsManager.GetInstance().isBoosterMattzip = false;
                UserWallet.GetInstance().ShowUserMatZip();
            }
        }

    }

    /// <summary>
    /// 테스트용 누르면 시간 추가.
    /// </summary>
    public void TEST_Btn()
    {
        //부스터 버튼이 온 일떄
        if (unbiasedRemaining.TotalSeconds > 0)
        {
            unbiasedTimerEndTimestamp = unbiasedTimerEndTimestamp.AddSeconds(addTime);
            this.WriteTimestamp("Booster_Body", unbiasedTimerEndTimestamp);
        }
        else // 부스터 버튼 꺼져있으면 초기화 하고 시간 추가.
        {
            unbiasedTimerEndTimestamp = UnbiasedTime.Instance.Now().AddSeconds(addTime - 10);
            this.WriteTimestamp("Booster_Body", unbiasedTimerEndTimestamp);
        }

        // 커버 벗기기
        transform.GetChild(0).gameObject.SetActive(false);

    }

    /// <summary>
    /// 구매했으면 on/off 기능 달아줌
    /// </summary>
    public void VIP_BUFF()
    {
        if (PlayerPrefsManager.GetInstance().VIP == 527 || PlayerPrefsManager.GetInstance().VIP == 625 || PlayerPrefsManager.GetInstance().VIP == 825 || PlayerPrefsManager.GetInstance().VIP == 925 || PlayerPrefsManager.GetInstance().isBoosterMattzip)
        {
            // 온/오프 기능
            if (!transform.GetChild(0).gameObject.activeSelf)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }

            return;
        }
    }


    /// <summary>
    /// 상점에서 구입하면 1200초
    /// </summary>
    public void BuyBodyShop()
    {
        //부스터 버튼이 온 일떄
        if (unbiasedRemaining.TotalSeconds > 0)
        {
            unbiasedTimerEndTimestamp = unbiasedTimerEndTimestamp.AddSeconds(1200);
            this.WriteTimestamp("Booster_Body", unbiasedTimerEndTimestamp);
        }
        else // 부스터 버튼 꺼져있으면 초기화 하고 시간 추가.
        {
            unbiasedTimerEndTimestamp = UnbiasedTime.Instance.Now().AddSeconds(1200 - 1);
            this.WriteTimestamp("Booster_Body", unbiasedTimerEndTimestamp);
        }

        // 커버 벗기기
        transform.GetChild(0).gameObject.SetActive(false);

    }


    #region <Rewarded Ads> 광고보고 버프 적용

    public void BuffAds()
    {
        int vvip = PlayerPrefsManager.GetInstance().VIP;
        //PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);
        //if (vvip == 526 || vvip == 725)
        //{
        //    BuffAdsInvo();
        //    return;
        //}


        if (Advertising.IsRewardedAdReady(RewardedAdNetwork.AdMob, AdPlacement.Default))
        {
            Advertising.RewardedAdCompleted += BuffAdsCompleated;
            Advertising.RewardedAdSkipped += BuffAdsSkipped;

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
    void BuffAdsCompleated(RewardedAdNetwork network, AdPlacement location)
    {
        Invoke("BuffAdsInvo", 0.5f);
        Advertising.RewardedAdCompleted -= BuffAdsCompleated;
        Advertising.RewardedAdSkipped -= BuffAdsSkipped;
    }

    // Event handler called when a rewarded ad has been skipped
    void BuffAdsSkipped(RewardedAdNetwork network, AdPlacement location)
    {
        Advertising.RewardedAdCompleted -= BuffAdsCompleated;
        Advertising.RewardedAdSkipped -= BuffAdsSkipped;
        PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);

    }

    void BuffAdsInvo()
    {

        PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);

        PopUpObjectManager.GetInstance().HIdeBuffProcess();

        TEST_Btn();

        int vvip = PlayerPrefsManager.GetInstance().VIP;
        if (vvip == 526 || vvip == 725)
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
    }
}
