using EasyMobile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteInit : MonoBehaviour
{
    DoubleToStringNum dts = new DoubleToStringNum();

    public Booster_Spin bspin;

    // 회전속도
    public float rotSpeed = 100f;
    // 돌아갈 트랜스 폼
    public Transform rouletteBoard;

    /// <summary>
    /// 외부에서 롤 호출 해줌 <Button>
    /// </summary>
    public void RollStart()
    {
        if (isSpinning || PlayerPrefsManager.GetInstance().DailySpinReword == 1 || bspin.SpinText.text != "Spin Now") return;

        //3시간 카운터 돌려줌.
        bspin.TEST_Btn();

        float val = Random.Range(1.01f, 3.00f);
        rotSpeed = 100f * val;
        StartCoroutine("TransRoll"); 
    }

    bool isSpinning;
    IEnumerator TransRoll()
    {
        isSpinning = true;
        yield return null;

        for(; ; )
        {
            yield return new WaitForFixedUpdate();
            //회전 속도만큼 룰렛을 회전
            rouletteBoard.Rotate(0, 0, -rotSpeed);
            //룰렛을 감속
            rotSpeed *= 0.96f;

            if (rotSpeed < 0.01f)
            {
                isSpinning = false;
                // 보상 팝업 호출
                ShowRulletReword();
                break;

            }

        }

    }

    /// <summary>
    /// 룰렛 팝업창 꺼줌
    /// </summary>
    public void HideRoulTransform()
    {
        if (isSpinning) return;
        transform.parent.parent.gameObject.SetActive(false);
    }

    [Header("- 룰렛 활성화 커버 이미지")]
    public GameObject _Cover; 

    /// <summary>
    /// 룰렛 멈출때 호출되는 보상 팝업.
    /// </summary>
    private void ShowRulletReword()
    {
        // 회색 커버 덮어줌.
        _Cover.SetActive(true);

        /// TODO : 회전 각도에 따라서 보상 지급.
        var result = rouletteBoard.eulerAngles.z;

        var pgupbap = PlayerPrefsManager.GetInstance().gupbap;
        var pSSalbap = PlayerPrefsManager.GetInstance().ssalbap;
        var pgold = PlayerPrefsManager.GetInstance().gold;
        var pkey = PlayerPrefsManager.GetInstance().key;
        //var pdia = PlayerPrefsManager.GetInstance().diamond;
        var pdps = PlayerPrefsManager.GetInstance().PlayerDPS;

        if (result < 0) result *= -1;


        int r_Index = 0;
        string r_Content = "";

        if (0 < result && result < 36.5)
        {
            PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(pgupbap, "800");
            UserWallet.GetInstance().ShowUserMilk();
            r_Index = 0;
            r_Content = "국밥 800 그릇을 획득했다!";
            tmpContent = "국밥 1600 그릇을 획득했다!";
        }
        else if(36.5 < result && result < 72.5)
        {
            PlayerPrefsManager.GetInstance().key += 5;
            //UserWallet.GetInstance().ShowUserKey();
            r_Index = 1;
            r_Content = "열쇠 5 개를 획득했다!";
            tmpContent = "열쇠 10 개를 획득했다!";

        }
        else if (72.5 < result && result < 109.5)
        {
            PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(pSSalbap, "400");
            UserWallet.GetInstance().ShowUserSSalbap();
            r_Index = 2;
            r_Content = "쌀밥 400 그릇을 획득했다!";
            tmpContent = "쌀밥 800 그릇을 획득했다!";

        }
        else if (109.5 < result && result < 144.5)
        {
            //PlayerPrefsManager.GetInstance().diamond = dts.AddStringDouble(pdia, "10");
            PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 80);

            UserWallet.GetInstance().ShowUserDia();
            r_Index = 3;
            r_Content = "다이아 80 개를 획득했다!";
            tmpContent = "다이아 160 개를 획득했다!";

        }
        else if (144.5 < result && result < 180)
        {
            pdps = dts.multipleStringDouble(pdps, 1600d);
            PlayerPrefsManager.GetInstance().gold = dts.AddStringDouble(pgold, pdps);
            UserWallet.GetInstance().ShowUserGold();
            r_Index = 4;
            r_Content =  UserWallet.GetInstance().SeetheNatural(double.Parse(pdps)) +" 골드를 획득했다!";
            pdps = dts.multipleStringDouble(pdps, 2d);
            tmpContent = UserWallet.GetInstance().SeetheNatural(double.Parse(pdps)) + " 골드를 획득했다!";


        }





        else if (180 < result && result < 216.5)
        {
            PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(pgupbap, "1600");
            UserWallet.GetInstance().ShowUserMilk();
            r_Index = 5;
            r_Content = "국밥 1600 그릇을 획득했다!";
            tmpContent = "국밥 3200 그릇을 획득했다!";
        }
        else if (216.5 < result && result < 252.5)
        {
            PlayerPrefsManager.GetInstance().key += 10;
            //UserWallet.GetInstance().ShowUserKey();
            r_Index = 6;
            r_Content = "열쇠 10 개를 획득했다!";
            tmpContent = "열쇠 20 개를 획득했다!";
        }
        else if (252.5 < result && result < 287.5)
        {
            PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(pSSalbap, "800");
            UserWallet.GetInstance().ShowUserSSalbap();
            r_Index = 7;
            r_Content = "쌀밥 800 그릇을 획득했다!";
            tmpContent = "쌀밥 1600 그릇을 획득했다!";

        }
        else if (287.5 < result && result < 322.5)
        {
            //PlayerPrefsManager.GetInstance().diamond = dts.AddStringDouble(pdia, "20");

            PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 160);

            UserWallet.GetInstance().ShowUserDia();
            r_Index = 8;
            r_Content = "다이아 160 개를 획득했다!";
            tmpContent = "다이아 320 개를 획득했다!";

        }
        else if (322.5 < result && result < 360)
        {
            pdps = dts.multipleStringDouble(pdps, 3200d);
            PlayerPrefsManager.GetInstance().gold = dts.AddStringDouble(pgold, pdps);
            UserWallet.GetInstance().ShowUserGold();
            r_Index = 9;
            r_Content = UserWallet.GetInstance().SeetheNatural(double.Parse(pdps)) + " 골드를 획득했다!";
            pdps = dts.multipleStringDouble(pdps, 2d);
            tmpContent = UserWallet.GetInstance().SeetheNatural(double.Parse(pdps)) + " 골드를 획득했다!";

        }





        tmpIndex = r_Index;
        

        PopUpObjectManager.GetInstance().ShowRouletteReword(r_Index, r_Content);

        // 룰렛 회전창 꺼줌
        HideRoulTransform();
    }


    int tmpIndex;
    string tmpContent;



    #region <Rewarded Ads> 룰렛 뻥튀기

    public void RuelletAds()
    {
        //PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);

        int vvip = PlayerPrefsManager.GetInstance().VIP;
        //PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);
        if (vvip == 526 || vvip == 625 || vvip == 725 || vvip == 925)
        {
            YouGoSOAds();
            return;
        }

        if (Advertising.IsRewardedAdReady(RewardedAdNetwork.AudienceNetwork, AdPlacement.Default))
        {
            Advertising.RewardedAdCompleted += YouGoSOAdsCompleated;
            Advertising.RewardedAdSkipped += YouGoSOAdsSkipped;
            /// 페북 동영상 1순위
            Advertising.ShowRewardedAd(RewardedAdNetwork.AudienceNetwork, AdPlacement.Default);
        }
        else if (Advertising.IsRewardedAdReady(RewardedAdNetwork.AdMob, AdPlacement.Default))
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
        PopUpObjectManager.GetInstance().ShowRoulette3XReword(tmpIndex, tmpContent);

        // 룰렛 회전창 꺼줌
        HideRoulTransform();

        int vvip = PlayerPrefsManager.GetInstance().VIP;
        //PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);
        if (vvip == 526 || vvip == 625 || vvip == 725 || vvip == 925)
        {
            return;
        }

        /// 퀘스트
        PlayerPrefsManager.GetInstance().questInfo[0].daily_Abs++;

        if (PlayerPrefsManager.GetInstance().questInfo[0].All_Abs < 1000)
        {
            PlayerPrefsManager.GetInstance().questInfo[0].All_Abs++;
        }        //

    }

    #endregion





}
