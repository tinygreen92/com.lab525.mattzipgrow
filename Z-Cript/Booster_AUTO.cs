using EasyMobile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Booster_AUTO : MonoBehaviour
{
    [Header("- 클릭동작 땡겨오기")]
    public TapToSpawnLimit tapToSpawnLimit;
    [Header("- 버프 한번에 몇초 증가?")]
    public double addTime = 600;

    // 초당 최대 액션 횟수
    private float fInputActionsPerSecond;
    // 1초에 카운트 할것.
    private float fUserClickedCnt;
    // 조작 불가 타이머 스탬프
    private DateTime unbiasedTimerEndTimestamp;
    TimeSpan unbiasedRemaining;

    private void Start()
    {
        unbiasedTimerEndTimestamp = ReadTimestamp("Booster_AUTO", UnbiasedTime.Instance.Now());
        // 공격 딜레이 1초당 10f
        fInputActionsPerSecond = 5f;
        fUserClickedCnt = 0f;
    }

    /// <summary>
    /// 오토 클릭
    /// </summary>
    public void ClickedAuto()
    {
        tapToSpawnLimit.ClickedSkyBox();

        //// 그로기 상태면 리턴
        //if (PlayerPrefsManager.GetInstance().isGroggy) return;
        ///// 연속클릭 딜레이
        //if (fUserClickedCnt != 0f && Time.unscaledTime < fUserClickedCnt) return;

        //tapToSpawnLimit.ClickedSomeThing();
        //ComputeNextAction();
    }

    void ComputeNextAction()
    {
        /// 초당터치 횟수 증가
        float tmp = fInputActionsPerSecond + PlayerPrefsManager.GetInstance().Arti_PunchTouch;
        // 국밥 스킬 발동하면 공속 1.5배
        if (PlayerPrefsManager.GetInstance().isGupSpeed)
        {
            tmp *= 1.5f;
        }
        else
        {
            tmp *= 1.0f;
        }

        fUserClickedCnt = Time.unscaledTime + (2f / tmp);
    }


    void FixedUpdate ()
    {
        if (!PlayerPrefsManager.GetInstance().isReadyQuest && !PlayerPrefsManager.GetInstance().isReadyWeapon) return;


        if (PlayerPrefsManager.GetInstance().VIP == 525 || PlayerPrefsManager.GetInstance().VIP == 625 || PlayerPrefsManager.GetInstance().VIP == 725 || PlayerPrefsManager.GetInstance().VIP == 825)
        {
            // 온/오프 기능
            if (!transform.GetChild(0).gameObject.activeSelf)
            {
                //오토 공격 반복 시작
                ClickedAuto();
                PlayerPrefsManager.GetInstance().isAutoAtk = true;
                transform.GetChild(1).GetComponent<Text>().text = "자동공격 ON";
            }
            else
            {
                PlayerPrefsManager.GetInstance().isAutoAtk = false;
                transform.GetChild(1).GetComponent<Text>().text = "자동공격 OFF";
            }

            return;
        }
        else
        {
            // 시간 소모 
            unbiasedRemaining = unbiasedTimerEndTimestamp - UnbiasedTime.Instance.Now();

            // 타이머가 돌아가면
            if (unbiasedRemaining.TotalSeconds > 0)
            {
                // 온 받은 순간부터 소모 타이머 카운트 다운
                transform.GetChild(1).GetComponent<Text>().text = string.Format("{0:00}:{1:00}:{2:00}", unbiasedRemaining.Hours, unbiasedRemaining.Minutes, unbiasedRemaining.Seconds);

                //오토 공격 반복 시작
                if (!transform.GetChild(0).gameObject.activeSelf) ClickedAuto();
                // 오토 on/off
                PlayerPrefsManager.GetInstance().isAutoAbsOn = true;
                PlayerPrefsManager.GetInstance().isAutoAtk = true;
            }
            else
            {
                transform.GetChild(1).GetComponent<Text>().text = "00:00:00";
                // 온 했다가 시간 다되면 비활성화 커버 덮어줌.
                transform.GetChild(0).gameObject.SetActive(true);
                // 오토 on/off
                PlayerPrefsManager.GetInstance().isAutoAbsOn = false;
                PlayerPrefsManager.GetInstance().isAutoAtk = false;

            }
        }



    }

    /// <summary>
    /// 테스트용 누르면 시간 추가.
    /// </summary>
    public void TEST_Btn()
    {
        unbiasedRemaining = unbiasedTimerEndTimestamp - UnbiasedTime.Instance.Now();
        // 시간이 남아있다.
        if (unbiasedRemaining.TotalSeconds > 0)
        {
            unbiasedTimerEndTimestamp = unbiasedTimerEndTimestamp.AddSeconds(addTime);
            this.WriteTimestamp("Booster_AUTO", unbiasedTimerEndTimestamp);
        }
        else // 시간 없다.
        {
            unbiasedTimerEndTimestamp = UnbiasedTime.Instance.Now().AddSeconds(addTime -1);
            this.WriteTimestamp("Booster_AUTO", unbiasedTimerEndTimestamp);
        }

        // 커버 벗기기
        transform.GetChild(0).gameObject.SetActive(false);

        // 남은 버프시간 표기

    }

    /// <summary>
    /// [외부버튼] 오토 공격 구매했으면 on/off 기능 달아줌
    /// </summary>
    public void VIP_BUFF()
    {
        if (PlayerPrefsManager.GetInstance().VIP == 525 || PlayerPrefsManager.GetInstance().VIP == 625 || PlayerPrefsManager.GetInstance().VIP == 725 || PlayerPrefsManager.GetInstance().VIP == 825 || PlayerPrefsManager.GetInstance().isAutoAbsOn)
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
    /// 미니게임 종료 후에 오토 클릭 꺼주기
    /// </summary>
    /// <param name="_swith"></param>
    public void BuffImgOnOff(bool _swith)
    {
        // 온/오프 기능
        if (_swith)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }



    #region <Rewarded Ads> 광고보고 버프 적용 if (vvip == 526 || vvip == 725 || vvip == 925)

    public void BuffAds()
    {
        int vvip = PlayerPrefsManager.GetInstance().VIP;
        //PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);
        //if (vvip == 526  || vvip == 725)
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
        PlayerPrefs.Save();
    }



}
