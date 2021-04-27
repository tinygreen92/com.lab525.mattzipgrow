using EasyMobile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MySDgatcha { ads_3, diamond_50, diamond_490, diamond_1390, ticket_1, ticket_10, ticket_30, diamond_2190, ticket_50 }

public class ShieldManager : MonoBehaviour
{
    public Transform playerShiledPos;
    [Header("-재화 자동 갱신")]
    public GameObject InfiShiledObj;
    public TutorialMissionManager tmm;
    public Text timeText;
    [Header("-뽑기 팝업창")]
    public GameObject leftShieldPop;
    public GameObject realGodChaPop;
    public Text countDailyText;
    [Header("-확인 창창")]
    public GameObject[] GrayBtns;
    public GameObject[] moneyIcons;
    public Image cardIcon;
    public Sprite[] cardSprs;
    public Text moneyText;
    [Header("-뽑기 성공후 연출 팝업")]
    public GameObject godChaNewShiled;
    public Button invibtn;
    /// <summary>
    /// 1 / 3 / 11 / 33
    /// </summary>
    public Transform[] Gods;
    [Header("-뽑기 방패 아이콘")]
    public Sprite[] shiledSprs;


    readonly float[] probs_S = new float[] { 10f,
                                        9f,
                                        8f,
                                        7f,
                                        6.5f,
                                        6f,
                                        5.5f,
                                        5f,
                                        4.5f,
                                        4f,
                                        3.55f,
                                        3.35f,
                                        3.15f,
                                        2.95f,
                                        2.75f,
                                        2.55f,
                                        2.35f,
                                        2.15f,
                                        1.95f,
                                        1.75f,
                                        1.55f,
                                        1.35f,
                                        1.15f,
                                        1.05f,
                                        0.95f,
                                        0.75f,
                                        0.55f,
                                        0.35f,
                                        0.15f,
                                        0.1f };


    /// <summary>
    /// 60초 * 30 = 30분 대기
    /// </summary>
    readonly double addTime = 60 * 30;

    // 조작 불가 타이머 스탬프
    private DateTime unbiasedTimerEndTimestamp;
    TimeSpan unbiasedRemaining;
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
        PlayerPrefs.SetInt("Shield10AdsCnt", PlayerPrefs.GetInt("Shield10AdsCnt", 0) + 1);
        PlayerPrefs.Save();
    }
    private void Start()
    {
        unbiasedTimerEndTimestamp = ReadTimestamp("Shield_Time", UnbiasedTime.Instance.Now());
        /// 스프라이트 적용
        EquipSpriteShiled(PlayerPrefsManager.GetInstance().ShieldIndex);
    }

    void FixedUpdate()
    {
        if (!PlayerPrefsManager.GetInstance().isReadyQuest && !PlayerPrefsManager.GetInstance().isReadyWeapon) return;

        // 시간 소모 
        unbiasedRemaining = unbiasedTimerEndTimestamp - UnbiasedTime.Instance.Now();

        /// 타이머 시동중
        if (unbiasedRemaining.TotalSeconds > 0)
        {
            GrayBtns[6].SetActive(true);
            // 온 받은 순간부터 소모 타이머 카운트 다운
            timeText.text = string.Format("{0:00}:{1:00}:{2:00}", unbiasedRemaining.Hours, unbiasedRemaining.Minutes, unbiasedRemaining.Seconds);

        }
        /// 타이머 멈춤
        else
        {
            GrayBtns[6].SetActive(false);
            timeText.text = "00:00:00";
        }
    }

    /// <summary>
    /// 플레이어 스프라이트에 방패 씌워줌
    /// </summary>
    /// <param name="thisIndex"></param>
    public void EquipSpriteShiled(int thisIndex)
    {
        int tLenth = playerShiledPos.childCount;

        for (int i = 0; i < tLenth; i++)
        {
            playerShiledPos.GetChild(i).gameObject.SetActive(false);
        }

        /// 인덱스 -1은 아님
        if(thisIndex >= 0)
        {
            playerShiledPos.GetChild(thisIndex).gameObject.SetActive(true);
        }
    }




    public void OpenLeftPop()
    {
        for (int i = 0; i < GrayBtns.Length; i++)
        {
            GrayBtns[i].SetActive(true);
        }


        /// 회색버튼 계산
        if (Mathf.FloorToInt(PlayerPrefs.GetFloat("dDiamond")) - 50 >= 0)
        {
            GrayBtns[0].SetActive(false);
            if (Mathf.FloorToInt(PlayerPrefs.GetFloat("dDiamond")) - 490 >= 0)
            {
                GrayBtns[1].SetActive(false);
                if (Mathf.FloorToInt(PlayerPrefs.GetFloat("dDiamond")) - 1390 >= 0)
                {
                    GrayBtns[2].SetActive(false);
                    if (Mathf.FloorToInt(PlayerPrefs.GetFloat("dDiamond")) - 2190 >= 0)
                    {
                        GrayBtns[7].SetActive(false);
                    }
                }
            }
        }
        /// 트켓
        if (PlayerPrefsManager.GetInstance().ShiledTicket - 1 >= 0)
        {
            GrayBtns[3].SetActive(false);
            if (PlayerPrefsManager.GetInstance().ShiledTicket - 10 >= 0)
            {
                GrayBtns[4].SetActive(false);
                if (PlayerPrefsManager.GetInstance().ShiledTicket - 30 >= 0)
                {
                    GrayBtns[5].SetActive(false);
                    if (PlayerPrefsManager.GetInstance().ShiledTicket - 50 >= 0)
                    {
                        GrayBtns[8].SetActive(false);
                    }
                }
            }
        }


        countDailyText.text = "일일 제한 (" + PlayerPrefs.GetInt("Shield10AdsCnt", 0).ToString("D2") + " / 10)";
        /// 창 열기
        leftShieldPop.SetActive(true);
        leftShieldPop.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        leftShieldPop.GetComponent<Animation>().Play("Roll_Incre");
    }



    /// <summary>
    /// 광고 버튼 활성화일때만 먹을 수 있음
    /// </summary>
    public void Ads_Btn_Btn()
    {
        if (GrayBtns[6].activeSelf || PlayerPrefs.GetInt("Shield10AdsCnt", 0) > 9) return;
        /// TODO : 광고 표시 하기.
        BuffAds();
    }


    #region <Rewarded Ads> 광고보고 방패 뽑음

    void BuffAds()
    {
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
        Invoke(nameof(GodChaAdsInvo), 0.5f);
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

    void GodChaAdsInvo()
    {
        PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);

        UnlockNewShield(MySDgatcha.ads_3);
        StartCoroutine(WhiteFlesh(Gods[1]));

        /// 클릭 막은 상태로 ㄱ
        invibtn.interactable = false;
        /// 서-순
        godChaNewShiled.SetActive(true);
        godChaNewShiled.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        godChaNewShiled.GetComponent<Animation>().Play("Roll_Incre");

        unbiasedTimerEndTimestamp = UnbiasedTime.Instance.Now().AddSeconds(addTime - 1);
        WriteTimestamp("Shield_Time", unbiasedTimerEndTimestamp);

        /// 퀘스트
        PlayerPrefsManager.GetInstance().questInfo[0].daily_Abs++;

        if (PlayerPrefsManager.GetInstance().questInfo[0].All_Abs < 1000)
        {
            PlayerPrefsManager.GetInstance().questInfo[0].All_Abs++;
        }
    }


    #endregion


    /// <summary>
    /// 롱 클릭하면 쉴드 업그레이드 해줌
    /// </summary>
    /// <param name="p_index"></param>
    internal void LongClicedUpgradeBtn(int p_index)
    {
        for (int i = 1; i < InfinityContent.childCount; i++)
        {
            if (int.Parse(InfinityContent.GetChild(i).name) == p_index)
            {
                InfinityContent.GetChild(i).GetComponent<ShieldItem>().ClickedUpgradeBtn();
                return;
            }

        }

    }



    /// <summary>
    /// 재화 충분하면 뽑기 연출 호출 
    /// </summary>
    public void OpenrealGodChaPop()
    {

        switch (_kindofamount)
        {
            /// 다이아로 결제

            case 1:
                if (Mathf.FloorToInt(PlayerPrefs.GetFloat("dDiamond")) - 50 >= 0)
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 50);
                else
                    return;
                UnlockNewShield(MySDgatcha.diamond_50);
                StartCoroutine(WhiteFlesh(Gods[0]));
                break;

            case 11:
                if (Mathf.FloorToInt(PlayerPrefs.GetFloat("dDiamond")) - 490 >= 0)
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 490);
                else
                    return;
                UnlockNewShield(MySDgatcha.diamond_490);
                StartCoroutine(WhiteFlesh(Gods[2]));
                break;

            case 33:
                if (Mathf.FloorToInt(PlayerPrefs.GetFloat("dDiamond")) - 1390 >= 0)
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 1390);
                else
                    return;
                UnlockNewShield(MySDgatcha.diamond_1390);
                StartCoroutine(WhiteFlesh(Gods[3]));
                break;

            case 55:
                if (Mathf.FloorToInt(PlayerPrefs.GetFloat("dDiamond")) - 2190 >= 0)
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 2190);
                else
                    return;
                UnlockNewShield(MySDgatcha.diamond_2190);
                StartCoroutine(WhiteFlesh(Gods[4]));
                break;


            /// 0 붙은건 티켓으로 결제

            case 10:
                if (PlayerPrefsManager.GetInstance().ShiledTicket - 1 >= 0)
                    PlayerPrefsManager.GetInstance().ShiledTicket -= 1;
                else
                    return;
                UnlockNewShield(MySDgatcha.ticket_1);
                StartCoroutine(WhiteFlesh(Gods[0]));
                break;

            case 110:
                if (PlayerPrefsManager.GetInstance().ShiledTicket - 10 >= 0)
                    PlayerPrefsManager.GetInstance().ShiledTicket -= 10;
                else
                    return;
                UnlockNewShield(MySDgatcha.ticket_10);
                StartCoroutine(WhiteFlesh(Gods[2]));
                break;

            case 330:
                if (PlayerPrefsManager.GetInstance().ShiledTicket - 30 >= 0)
                    PlayerPrefsManager.GetInstance().ShiledTicket -= 30;
                else
                    return;
                UnlockNewShield(MySDgatcha.ticket_30);
                StartCoroutine(WhiteFlesh(Gods[3]));
                break;

            case 550:
                if (PlayerPrefsManager.GetInstance().ShiledTicket - 50 >= 0)
                    PlayerPrefsManager.GetInstance().ShiledTicket -= 50;
                else
                    return;
                UnlockNewShield(MySDgatcha.ticket_50);
                StartCoroutine(WhiteFlesh(Gods[4]));
                break;

            default:
                break;
        }
        PlayerPrefs.Save();
        /// 클릭 막은 상태로 ㄱ
        invibtn.interactable = false;
        /// 서-순
        godChaNewShiled.SetActive(true);
        godChaNewShiled.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        godChaNewShiled.GetComponent<Animation>().Play("Roll_Incre");
    }




    /// <summary>
    /// 몇 회 뽑나 저장
    /// </summary>
    int _kindofamount;


    public void ClickedPopEnable(int _ck)
    {
        moneyIcons[0].SetActive(false);
        moneyIcons[1].SetActive(false);

        _kindofamount = _ck;
        
        switch (_kindofamount)
        {
            case 1:
                if (GrayBtns[0].activeSelf) return;
                moneyIcons[0].SetActive(true);
                moneyText.text = "x50";
                cardIcon.sprite = cardSprs[0];
                break;

            case 11:
                if (GrayBtns[1].activeSelf) return;
                moneyIcons[0].SetActive(true);
                moneyText.text = "x490";
                cardIcon.sprite = cardSprs[1];

                break;

            case 33:
                if (GrayBtns[2].activeSelf) return;
                moneyIcons[0].SetActive(true);
                moneyText.text = "x1390";
                cardIcon.sprite = cardSprs[2];

                break;

            case 55:
                if (GrayBtns[7].activeSelf) return;
                moneyIcons[0].SetActive(true);
                moneyText.text = "x2190";
                cardIcon.sprite = cardSprs[3];

                break;


            /// 0 붙은건 티켓으로 결제

            case 10:
                if (GrayBtns[3].activeSelf) return;
                moneyIcons[1].SetActive(true);
                moneyText.text = "x1";
                cardIcon.sprite = cardSprs[0];

                break;

            case 110:
                if (GrayBtns[4].activeSelf) return;
                moneyIcons[1].SetActive(true);
                moneyText.text = "x10";
                cardIcon.sprite = cardSprs[1];

                break;

            case 330:
                if (GrayBtns[5].activeSelf) return;
                moneyIcons[1].SetActive(true);
                moneyText.text = "x30";
                cardIcon.sprite = cardSprs[2];

                break;

            case 550:
                if (GrayBtns[8].activeSelf) return;
                moneyIcons[1].SetActive(true);
                moneyText.text = "x50";
                cardIcon.sprite = cardSprs[3];

                break;

            default:
                return;
        }

        /// 창 열기
        realGodChaPop.SetActive(true);
        realGodChaPop.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        realGodChaPop.GetComponent<Animation>().Play("Roll_Incre");

    }


    readonly WaitForSeconds JJumO = new WaitForSeconds(0.1f);

    /// <summary>
    /// 뽑기 연출 끝나면 클릭 가능하게
    /// </summary>
    IEnumerator WhiteFlesh(Transform _God)
    {
        for (int i = 0; i < Gods.Length; i++)
        {
            Gods[i].gameObject.SetActive(false);
        }

        int godLenth = _God.childCount;

        switch (godLenth)
        {
            case 1:
                Gods[0].GetChild(0).gameObject.SetActive(false);
                Gods[0].GetChild(0).GetChild(0).gameObject.SetActive(true);
                
                Gods[0].gameObject.SetActive(true);
                yield return JJumO;
                Gods[0].GetChild(0).gameObject.SetActive(true);
                yield return JJumO;
                Gods[0].GetChild(0).GetChild(0).gameObject.SetActive(false);
                break;

            case 3:
                for (int i = 0; i < godLenth; i++)
                {
                    Gods[1].GetChild(i).gameObject.SetActive(false);
                    Gods[1].GetChild(i).GetChild(0).gameObject.SetActive(true);
                }

                Gods[1].gameObject.SetActive(true);
                yield return JJumO;

                for (int i = 0; i < godLenth; i++)
                {
                    Gods[1].GetChild(i).gameObject.SetActive(true);
                    yield return JJumO;
                    Gods[1].GetChild(i).GetChild(0).gameObject.SetActive(false);
                }
                break;

            case 11:
                for (int i = 0; i < godLenth; i++)
                {
                    Gods[2].GetChild(i).gameObject.SetActive(false);
                    Gods[2].GetChild(i).GetChild(0).gameObject.SetActive(true);
                }

                Gods[2].gameObject.SetActive(true);
                yield return JJumO;

                for (int i = 0; i < godLenth; i++)
                {
                    Gods[2].GetChild(i).gameObject.SetActive(true);
                    yield return JJumO;
                    Gods[2].GetChild(i).GetChild(0).gameObject.SetActive(false);
                }
                break;

            case 33:
                for (int i = 0; i < godLenth; i++)
                {
                    Gods[3].GetChild(i).gameObject.SetActive(false);
                    Gods[3].GetChild(i).GetChild(0).gameObject.SetActive(true);
                }

                Gods[3].gameObject.SetActive(true);
                yield return JJumO;

                for (int i = 0; i < godLenth; i++)
                {
                    Gods[3].GetChild(i).gameObject.SetActive(true);
                    yield return JJumO;
                    Gods[3].GetChild(i).GetChild(0).gameObject.SetActive(false);
                }
                break;

            case 55:
                for (int i = 0; i < godLenth; i++)
                {
                    Gods[4].GetChild(i).gameObject.SetActive(false);
                    Gods[4].GetChild(i).GetChild(0).gameObject.SetActive(true);
                }

                Gods[4].gameObject.SetActive(true);
                yield return JJumO;

                for (int i = 0; i < godLenth; i++)
                {
                    Gods[4].GetChild(i).gameObject.SetActive(true);
                    yield return JJumO;
                    Gods[4].GetChild(i).GetChild(0).gameObject.SetActive(false);
                }
                break;

            default:
                break;
        }

        /// 클릭 가능.
        invibtn.interactable = true;
        /// 재화에 따른 버튼 갱신
        OpenLeftPop();
    }









    [Header("-초기 데이터")]
    public TextAsset ta;
    [Header("-무한 스크롤 방패")]
    public Transform InfinityContent;

    [Header("-방패 이미지 30개")]
    public Sprite[] shieldImgs;


    /// <summary>
    /// 방패 이름
    /// </summary>
    public string[] shieldNames =
    {   "C",
        "C+",
        "CC",
        "CC+",
        "CCC",
        "CCC+",
        "B",
        "B+",
        "BB",
        "BB+",
        "BBB",
        "BBB+",
        "A",
        "A+",
        "AA",
        "AA+",
        "AAA",
        "AAA+",
        "S",
        "S+",
        "SS",
        "SS+",
        "SSS",
        "SSS+",
        "R",
        "R+",
        "RR",
        "RR+",
        "RRR",
        "RRR+"
    };

    /// <summary>
    /// 호출 시점에서 쉴드 0개 상태라면 초기 세팅
    /// </summary>
    public void InitShieldInfo()
    {
        /// 깍두기 체크 0.5 코루틴 시작
        if (coru == null)
            coru = StartCoroutine(KimchChechecak());
        /// 쉴드 정보 있다면 리턴
        if (PlayerPrefsManager.GetInstance().shieldInfo.Count != 0) return;

        PlayerPrefs.SetString("shieldInfo", ta.text);
        PlayerPrefs.Save();
        /// 리스트 캐스팅
        PlayerPrefsManager.GetInstance().LoadShieldInfo();

    }

    Coroutine coru;
    WaitForSeconds GC_05_SEC = new WaitForSeconds(0.5f);
    string myKimchi;


    /// <summary>
    /// 회색으로 덮기 판단 0.5초 마다
    /// </summary>
    /// <returns></returns>
    IEnumerator KimchChechecak()
    {
        yield return null;

        while (true)
        {
            yield return GC_05_SEC;

            if (InfiShiledObj.activeSelf)
            {
                myKimchi = PlayerPrefsManager.GetInstance().Kimchi;

                for (int i = 1; i < InfinityContent.childCount; i++)
                {
                    InfinityContent.GetChild(i).GetComponent<ShieldItem>().BuyBtnBye(myKimchi);
                }
            }
        }
    }


    /// <summary>
    /// 모든 장착 버튼 노란색으로 바꾸기
    /// </summary>
    internal void RefreshEuipGrayBtn()
    {
        for (int i = 1; i < InfinityContent.childCount; i++)
        {
            InfinityContent.GetChild(i).GetComponent<ShieldItem>().SetAllEpuipBtnToGray();
        }
    }


    /// <summary>
    /// 모든 회색 커버 제거
    /// </summary>
    void RefreshCover()
    {
        for (int i = 1; i < InfinityContent.childCount; i++)
        {
            InfinityContent.GetChild(i).GetComponent<ShieldItem>().SetDefaltInfo();
        }
    }


    /// <summary>
    /// 새로운 쉴드 해제 (몇번 뽑인지?)
    /// </summary>
    /// <param name="cnt"></param>
    void UnlockNewShield(MySDgatcha myS)
    {
        int cnt;
        Transform tmps;

        System.Random seedRnd = new System.Random();
        int startIndex = seedRnd.Next();

        switch (myS)
        {
            case MySDgatcha.ads_3:
                cnt = 3;
                tmps = Gods[1];
                break;
            case MySDgatcha.diamond_50:
                cnt = 1;
                tmps = Gods[0];
                break;
            case MySDgatcha.diamond_490:
                cnt = 11;
                tmps = Gods[2];
                break;
            case MySDgatcha.diamond_1390:
                cnt = 33;
                tmps = Gods[3];
                break;
            case MySDgatcha.ticket_1:
                cnt = 1;
                tmps = Gods[0];
                break;
            case MySDgatcha.ticket_10:
                cnt = 11;
                tmps = Gods[2];
                break;
            case MySDgatcha.ticket_30:
                cnt = 33;
                tmps = Gods[3];
                break;
            case MySDgatcha.diamond_2190:
                cnt = 55;
                tmps = Gods[4];
                break;
            case MySDgatcha.ticket_50:
                cnt = 55;
                tmps = Gods[4];
                break;





            default:
                cnt = 0;
                tmps = Gods[1];
                break;
        }

        tmm.ExUpdateMission(7); /// 미션 업데이트
        tmm.ExUpdateMission(63); /// 미션 업데이트
        tmm.ExUpdateMission(73); /// 미션 업데이트

        /// 횟수만큼 랜덤 돌려
        for (int i = 0; i < cnt; i++)
        {
            tmps.GetChild(i).GetComponent<Image>().sprite = RandomPick(++startIndex);
        }
        
        /// 정보 저장
        PlayerPrefsManager.GetInstance().SaveShieldInfo();
        // 해금
        RefreshCover();
    }


    /// <summary>
    /// 확률에 따라 랜덤 해제
    /// </summary>
    /// <param name="seed"></param>
    Sprite RandomPick(int seed)
    {
        int _index = 0;
        // 가중치 랜덤
        float returnValue = GetRandom(probs_S, seed);

        switch (returnValue)
        {
            case 10f:   _index = 0; break;
            case 9f:    _index = 1; break;
            case 8f:    _index = 2; break;
            case 7f:    _index = 3; break;
            case 6.5f:  _index = 4; break;
            case 6f:    _index = 5; break;
            case 5.5f:  _index = 6; break;
            case 5f:    _index = 7; break;
            case 4.5f:  _index = 8; break;
            case 4f:    _index = 9; break;
            case 3.55f: _index = 10; break;
            case 3.35f: _index = 11; break;
            case 3.15f: _index = 12; break;
            case 2.95f: _index = 13; break;
            case 2.75f: _index = 14; break;
            case 2.55f: _index = 15; break;
            case 2.35f: _index = 16; break;
            case 2.15f: _index = 17; break;
            case 1.95f: _index = 18; break;
            case 1.75f: _index = 19; break;
            case 1.55f: _index = 20; break;
            case 1.35f: _index = 21; break;
            case 1.15f: _index = 22; break;
            case 1.05f: _index = 23; break;
            case 0.95f: _index = 24; break;
            case 0.75f: _index = 25; break;
            case 0.55f: _index = 26; break;
            case 0.35f: _index = 27; break;
            case 0.15f: _index = 28; break;
            case 0.1f:  _index = 29; break;
        }

        /// 첫 획득이면 회색 커버 제거해주고
        if (!PlayerPrefsManager.GetInstance().shieldInfo[_index].isUnlock)
        {
            PlayerPrefsManager.GetInstance().shieldInfo[_index].isUnlock = true;
        }
        /// 갯수 증가.
        if (PlayerPrefsManager.GetInstance().shieldInfo[_index].amount < int.MaxValue -1)
        {
            PlayerPrefsManager.GetInstance().shieldInfo[_index].amount++;
        }

        return shiledSprs[_index];
    }



    /// <summary>
    ///  랜덤 시드를 통해서 가중치 랜덤 뽑아낸다
    /// </summary>
    /// <param name="inputDatas"></param>
    /// <param name="seed"></param>
    /// <returns></returns>
    public float GetRandom(float[] inputDatas, int seed)
    {
        System.Random random = new System.Random(seed);

        float total = 0;
        for (int i = 0; i < inputDatas.Length; i++)
        {
            total += inputDatas[i];
        }
        float pivot = (float)random.NextDouble() * total;
        for (int i = 0; i < inputDatas.Length; i++)
        {
            if (pivot < inputDatas[i])
            {
                return inputDatas[i];
            }
            else
            {
                pivot -= inputDatas[i];
            }
        }
        return inputDatas[inputDatas.Length - 1];
    }



    public void sparser()
    {
        string[] line = ta.text.Substring(0, ta.text.Length).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            PlayerPrefsManager.GetInstance().AddShieldData(
                //row[0],      // 등급 
                float.Parse(row[1]),      // 장착 %    
                float.Parse(row[2]),      // 보유 %
                float.Parse(row[3]),      // 강화 성공률
                float.Parse(row[4]),      // 성공 차감율
                float.Parse(row[5]),      // 레벨당 방어력 증가치 %
                row[6]       // 레벨당 비용 시작값
                );
        }

        PlayerPrefsManager.GetInstance().SaveShieldInfo();
    }



}
