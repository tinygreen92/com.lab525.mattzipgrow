using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayNANOO;
using UnityEngine.UI;


public class PlayNANOOExample : MonoBehaviour
{
    Plugin plugin;
    DoubleToStringNum dts = new DoubleToStringNum();

    [Header("- 입력 텍스트")]
    public Text InputText;
    [Header("- 랭킹 카드 부모")]
    public Transform MomCardPos;
    [Header("- 프리페러이언스")]
    public PlayerPrefsManager playerPrefsManager;
    [Header("- PostBoxManager")]
    public PostBoxManager postBoxManager;
    [Header("- PostBoxManager")]
    public RankingPage rankingPage;

    private const string INFI_TOWER = "mattzip-RANK-1FA6C851-CDA0F163";
    private const string MINI_GAME = "mattzip-RANK-6A98CF32-E7586172";
    private const string NEW_RANKING = "mattzip-RANK-00B363A1-8630431C";


    /// <summary>
    /// 이제 플레이팹 접속되어야 나누 접속
    /// </summary>
    public void NanooStart()
    {
        plugin = Plugin.GetInstance();
        plugin.SetUUID(GPGSManager.GetLocalUserId());
        plugin.SetNickname(GPGSManager.GetLocalUserName());
        plugin.SetLanguage(Configure.PN_LANG_KO);
        /// 우편함 체크
        PostboxCheck();
        /// 배너 출력
        OpenBanner();
    }


    ///// <summary>
    ///// Open Forum Banner
    ///// </summary>
    //public void OpenForumView()
    //{
    //    plugin.OpenForumView("https://help.playnanoo.com/mattzip/faq");
    //}

    /// <summary>
    /// Open Forum Banner
    /// </summary>
    public void OpenForumViewIndex(string _idxText)
    {
        plugin.OpenForumView(_idxText);
    }


    /// <summary>
    /// Open Forum Banner
    /// </summary>
    public void OpenBanner()
    {
        plugin.OpenBanner();
    }

    /// <summary>
    /// Open Forum
    /// </summary>
    public void OpenForum()
    {
        plugin.OpenForum();
    }

    /// <summary>
    /// Open HelpDesk
    /// </summary>
    public void OpenHelpDesk()
    {
        plugin.SetHelpDeskOptional("OptionTest1", "ValueTest1");
        plugin.SetHelpDeskOptional("OptionTest2", "ValueTest2");
        plugin.OpenHelpDesk();
    }

    /// <summary>
    /// Data Query in Forum
    /// </summary>
    public void ForumThread()
    {
        plugin.ForumThread(Configure.PN_FORUM_THREAD, 10, (state, message, rawData, dictionary) => {
            Debug.Log(message);
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                ArrayList list = (ArrayList)dictionary["list"];
                foreach (Dictionary<string, object> thread in list)
                {
                    Debug.Log(thread["seq"]);
                    Debug.Log(thread["title"]);
                    Debug.Log(thread["summary"]);
                    Debug.Log(thread["attach_file"]);
                    Debug.Log(thread["url"]);
                    Debug.Log(thread["post_date"]);

                    foreach (string attach in (ArrayList)thread["attach_file"])
                    {
                        Debug.Log(attach);
                    }
                }
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    /// <summary>
    /// Server Time
    /// </summary>
    public void ServerTime()
    {
        plugin.ServerTime((state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(dictionary["timezone"]);
                Debug.Log(dictionary["timestamp"]);
                Debug.Log(dictionary["ISO_8601_date"]);
                Debug.Log(dictionary["date"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    /// <summary>
    /// AccessEvent
    /// </summary>
    public void AccessEvent()
    {
        // 실행
        plugin.AccessEvent((state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                // 서버 시간
                if (dictionary.ContainsKey("server_timestamp"))
                {
                    //Debug.Log("서버 시간 : " + dictionary["server_timestamp"]);
                }

                // 구독 상품 조회
                if (dictionary.ContainsKey("postbox_subscription"))
                {
                    foreach (Dictionary<string, object> subscription in (ArrayList)dictionary["postbox_subscription"])
                    {
                        //Debug.Log("구독 상품 : " + subscription["product"]);
                        //Debug.Log("구독 ttl : " + subscription["ttl"]);
                    }
                }

                // 초대 보상 내역 
                if (dictionary.ContainsKey("invite_rewards"))
                {
                    foreach (Dictionary<string, object> invite in (ArrayList)dictionary["invite_rewards"])
                    {
                        //Debug.Log("초대 보상 아이템 코드 : " + invite["item_code"]);
                        //Debug.Log("초대 보상 아이템 개수 : " + invite["item_count"]);
                    }
                }

                //Debug.Log("AccessEvent 호출");

            }
            else
            {
                Debug.Log("실패");
            }
        });
    }

    /// <summary>
    /// Coupon
    /// </summary>
    public void Coupon()
    {
        string inputTmp = InputText.text;

        plugin.Coupon(inputTmp, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                //Debug.Log(dictionary["code"]);
                //Debug.LogWarning(dictionary["item_code"]);
                //Debug.LogWarning(dictionary["item_count"]);

                string item_code = dictionary["item_code"].ToString();
                string item_count = dictionary["item_count"].ToString();
                item_code = item_code.Replace("\"", "");
                item_count = item_count.Replace("\"", "");

                //CouponCheak(item_code, item_count);
                /// plugin.PostboxItemSend
                PostboxItemSend(item_code, int.Parse(item_count), "");
                InputText.transform.parent.GetComponent<InputField>().text = "";
                InputText.transform.parent.parent.parent.gameObject.SetActive(false);
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("쿠폰이 사용되었습니다 우편함을 확인해주세요.");

                PostboxCheck();
            }
            else
            {
                Debug.Log("Coupon Fail");
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("이미 사용하였거나 잘못된 쿠폰 번호입니다.");
            }
        });
    }




    string[,] tmpUID = new string[100, 4];


    /// <summary>
    /// 우편함에 뭐 들어와있나? 빨간점 체크
    /// </summary>
    public void PostboxCheck()
    {
        AccessEvent();

        string uid = string.Empty;
        string item_code = string.Empty;
        string item_count = string.Empty;
        //
        plugin.PostboxItem((state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                ///
                RedDotManager.GetInstance().PostDotOff0();
                RedDotManager.GetInstance().PostDotOff1();
                //
                ArrayList items = (ArrayList)dictionary["item"];
                foreach (Dictionary<string, object> item in items)
                {
                    //Debug.LogWarning(item["uid"]);
                    //Debug.LogWarning(item["message"]);
                    //Debug.LogWarning(item["item_code"]);
                    //Debug.LogWarning(item["item_count"]);
                    //Debug.LogWarning(item["expire_sec"]);
                    /// 우편함에 아이템 있다
                    RedDotManager.GetInstance().PostDotOn0();
                    RedDotManager.GetInstance().PostDotOn1();

                }

            }
            else
            {
                Debug.Log("PostboxItem Fail");
            }
        });


    }

    /// <summary>
    /// 서버에서 데이터 우편 불러와서 프리팹 생성
    /// </summary>
    public void Postbox()
    {
        PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);

        int _index = 0;
        //
        string uid = string.Empty;
        string item_code = string.Empty;
        string item_count = string.Empty;
        string _message = string.Empty;
        //
        plugin.PostboxItem((state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                ArrayList items = (ArrayList)dictionary["item"];
                foreach (Dictionary<string, object> item in items)
                {
                    //Debug.LogWarning(item["uid"]);
                    //Debug.Log(item["message"]);
                    //Debug.Log(item["item_code"]);
                    //Debug.Log(item["item_count"]);
                    //Debug.Log(item["expire_sec"]);

                    uid = item["uid"].ToString();
                    tmpUID[_index, 0] = uid.Replace("\"", "");

                    item_code = item["item_code"].ToString();
                    tmpUID[_index, 1] = item_code.Replace("\"", "");

                    item_count = item["item_count"].ToString();
                    tmpUID[_index, 2] = item_count.Replace("\"", "");

                    _message = item["message"].ToString();
                    tmpUID[_index, 3] = _message.Replace("\"", "");

                    PostSendAddItem(_index);

                    _index++;
                }

                Debug.Log("PostboxItem Sucess");

                PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);
            }
            else
            {
                Debug.Log("PostboxItem Fail");
                PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);

            }
        });


    }

    /// <summary>
    /// 우편함에 추가하기.
    /// </summary>
    /// <param name="_indx"></param>
    void PostSendAddItem(int _indx)
    {
        Debug.LogWarning("_indx : " + _indx);

        string uid = tmpUID[_indx, 0];

        string item_code = tmpUID[_indx, 1];

        string item_count = tmpUID[_indx, 2];

        string message = tmpUID[_indx, 3];
        Debug.LogWarning("message : " + message);

        //
        postBoxManager.AddPresent(item_code, item_count, uid, message);
    }



    public void Test_TutorialComp()
    {
        //PostboxItemSend("diamond", 100);
        //PostboxItemSend("gupbap", 100);
        //PostboxItemSend("key", 10);
    }

    /// <summary>
    /// Issuing Item in InBox
    /// </summary>
    public void PostboxItemSend(string _code, int _amount, string _msg)
    {
        plugin.PostboxItemSend(_code, _amount, 365, _msg, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                PostboxCheck();
                Debug.Log("PostboxItemSend Pass");
            }
            else
            {
                Debug.Log("PostboxItemSend Fail");
            }
        });
    }


    /// <summary>
    /// Item Use in InBox
    /// </summary>
    public void PostboxItemUse(string _UID)
    {
        plugin.PostboxItemUse(_UID, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                string item_code = dictionary["item_code"].ToString();
                item_code = item_code.Replace("\"", "");
                string item_count = dictionary["item_count"].ToString();
                item_count = item_count.Replace("\"", "");
                //
                CouponCheak(item_code, item_count);

                Debug.Log("PostboxItemUse Pass");
                // 우편함 물건 다 없어졌나 확인.
                PostboxCheck();
            }
            else
            {
                Debug.Log("PostboxItemUse Fail");
            }
        });
    }

    /// <summary>
    /// PostboxItemUse에 서 호출해서 습득
    /// </summary>
    /// <param name="_code"></param>
    /// <param name="_count"></param>
    void CouponCheak(string _code, string _count)
    {
        if (_code == "key")
        {
            PlayerPrefsManager.GetInstance().key += int.Parse(_count);
            UserWallet.GetInstance().ShowUserKey();
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("열쇠 " + _count + " 개 획득.");

            return;
        }
        if (_code == "diamond")
        {
            PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + float.Parse(_count));
            UserWallet.GetInstance().ShowUserDia();
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("다이아 " + _count + " 개 획득.");
            return;

        }
        if (_code == "gold")
        {
            PlayerPrefsManager.GetInstance().gold = dts.AddStringDouble(PlayerPrefsManager.GetInstance().gold, _count);
            UserWallet.GetInstance().ShowUserGold();
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("골드 " + _count + " 획득.");
            return;

        }
        if (_code == "gupbap")
        {
            PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(PlayerPrefsManager.GetInstance().gupbap, _count);
            UserWallet.GetInstance().ShowUserMilk();
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("국밥 " + _count + " 그릇 획득.");
            return;
        }
        if (_code == "ssal")
        {
            PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(PlayerPrefsManager.GetInstance().ssalbap, _count);
            UserWallet.GetInstance().ShowUserSSalbap();
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("쌀밥 " + _count + " 그릇 획득.");
            return;
        }

        if (_code == "ticket")
        {
            PlayerPrefsManager.GetInstance().ticket += int.Parse(_count);
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("입장권 " + _count + " 장 획득.");
            return;
        }

        PlayerPrefs.Save();
    }

    /// <summary>
    /// Delete in InBox
    /// </summary>
    public void PostboxClear()
    {
        plugin.PostboxClear((state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    /// <summary>
    /// Subscription Registration in InBox
    /// </summary>
    public void PostboxSubscriptionRegister()
    {
        plugin.PostboxSubscriptionRegister("PRODUCT_CODE", (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    /// <summary>
    /// Subscription Cancel in InBox
    /// </summary>
    public void PostboxSubscriptionCancel()
    {
        plugin.PostboxSubscriptionCancel("PRODUCT_CODE", (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }




    public Text userID;
    public Text userName;

    public string userid;

    /// <summary>
    /// 실행 버튼 누르면 
    /// </summary>
    public void DATACHAGE()
    {
        plugin.SetUUID(userID.text);
        plugin.SetNickname(userName.text);

        GPGSManager.SetUserDatata(userID.text, userName.text);

        userid = userID.text;

        Debug.LogError(userID.text);
        Debug.LogError(userName.text);

        plugin.StorageSave(userID.text, "AAA", true, (state, message, rawData, dictionary) =>
        {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.LogWarning("StorageSave Success ::");
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("데이터가 정상적으로 저장되었습니다.");

            }
            else
            {
                Debug.Log("StorageSave Fail");
            }
        });

    }


    /// <summary>
    /// Data Save in Cloud Data
    /// </summary>
    public void StorageSave()
    {
        PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);
        PlayerPrefs.Save();
        //
        plugin.StorageSave(GPGSManager.GetLocalUserId(), playerPrefsManager.SaveAllPrefsData(), true, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.LogWarning("StorageSave Success");
                /// 쪼꼬미 데이터도 같이 저장.
                StorageSaveForCheack();
            }
            else
            {
                Debug.LogWarning("StorageSave Fail");
                PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("서버 데이터 저장 오류. 앱 재실행 후 다시 시도해주세요.");
            }
        });
    }

    /// <summary>
    /// 앱 종료할때 자동 저장
    /// </summary>
    public void StorageSaveForBuy()
    {
        //PlayerPrefs.Save();
        //plugin.StorageSave(GPGSManager.GetLocalUserId(), playerPrefsManager.SaveAllPrefsData(), true, (state, message, rawData, dictionary) => {
        //    if (state.Equals(Configure.PN_API_STATE_SUCCESS))
        //    {
        //        Debug.LogWarning("StorageSave Success ::");
        //    }
        //    else
        //    {
        //        Debug.LogWarning("StorageSave Fail");
        //    }
        //});
    }

    public void StorageSaveForCheack()
    {
        PlayerPrefs.Save();
        plugin.StorageSave(GPGSManager.GetLocalUserId() + "_Check", playerPrefsManager.ZZoGGoMiDataSave(), true, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.LogWarning("StorageSave Success ::" + playerPrefsManager.ZZoGGoMiDataSave());
                PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("데이터가 정상적으로 저장되었습니다. 앱이 재실행됩니다.");
                /// 데이터 세이브 플러그 세워주고 종료
                PlayerPrefs.SetInt("isDataSaved", 1);
                PlayerPrefs.Save();
                //
                Invoke(nameof(RestartAppForAOS),1f );
            }
            else
            {
                Debug.LogWarning("StorageSave Fail");
                PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("서버 데이터 저장 오류. 앱 재실행 후 다시 시도해주세요.");
            }
        });
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


    //public void SaveJSON_test(string _payload)
    //{
    //    PlayerPrefs.Save();
    //    plugin.StorageSave(GPGSManager.GetLocalUserId() + "_JSON", _payload, true, (state, message, rawData, dictionary) => {
    //        if (state.Equals(Configure.PN_API_STATE_SUCCESS))
    //        {
    //            Debug.LogWarning("StorageSave Success ");
    //        }
    //        else
    //        {
    //            Debug.LogWarning("StorageSave Fail");
    //        }
    //    });
    //}



    public void StorageLodeForCheack()
    {
        PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);

        plugin.StorageLoad(GPGSManager.GetLocalUserId() + "_Check", (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);

                string data = dictionary["value"].ToString();
                data = data.Replace("\"", "");
                PlayerPrefsManager.GetInstance().ZZoGGoMiDataLoad(data);
                Debug.LogWarning("StorageLoad : " + data);

                PopUpObjectManager.GetInstance().ShowDataCheackPop(true);

            }
            else
            {
                Debug.LogWarning("StorageLoad Fail");

                PopUpObjectManager.GetInstance().ShowDataCheackPop(false);

                PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);

            }
        });
    }

    /// <summary>
    /// Data Load in Cloud Data
    /// </summary>
    public void StorageLoad()
    {
        PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);

        plugin.StorageLoad(GPGSManager.GetLocalUserId(), (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {

                string data = dictionary["value"].ToString();
                data = data.Replace("\"", "");

                Debug.LogWarning("StorageLoad :: " + data);

                playerPrefsManager.LoadAllPrefsData(data);

                //PlayerPrefs.SetInt("is0508shock", 0);
                //PlayerPrefs.SetInt("is0517shock", 0);
            }
            else
            {
                Debug.LogWarning("StorageLoad Fail");
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("저장된 데이터가 존재하지 않습니다.");
                PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);

            }
        });
    }


    /// <summary>
    /// 최초 접속시 이전 맷집력 랭킹 비례 다이아몬드 지급
    /// </summary>
    public void BeforeRankingMatt()
    {
        plugin.RankingPersonal("mattzip-RANK-F81A740E-61075C7F", (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                if (dictionary["ranking"] == null)
                    return;
                float ddd = PlayerPrefs.GetFloat("dDiamond");
                int iRank = (int)dictionary["ranking"];
                /// 101위 부터는 1000개
                if (iRank > 100)
                {
                    ddd += Mathf.RoundToInt(100000 / iRank);
                    ///PostboxItemSend("diamond", ddd, "랭킹 순위권 보상");
                    PlayerPrefs.SetFloat("dDiamond", ddd);
                }
                else
                {
                    ddd += 100;
                    ///PostboxItemSend("diamond", ddd, "랭킹 순위권 보상");
                    PlayerPrefs.SetFloat("dDiamond", ddd);
                }
                UserWallet.GetInstance().ShowUserDia();
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    /// <summary>
    /// Data Query in LeaderBoard
    /// </summary>
    public void RankingMatt()
    {
        PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);

        plugin.Ranking(NEW_RANKING, 50, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                ArrayList list = (ArrayList)dictionary["list"];
                foreach (Dictionary<string, object> rank in list)
                {
                    //Debug.Log(rank["score"]);
                    //Debug.Log(rank["nickname"]);
                    //
                    var score = rank["score"].ToString();

                    if (score.Contains("E+15")) score = ((score.Replace("E+15", "")).Replace(".", "")) + "00";
                    else if (score.Contains("E+14")) score = ((score.Replace("E+14", "")).Replace(".", "") + "0");
                    else if (score.Contains("E+13")) score = ((score.Replace("E+13", "")).Replace(".", ""));


                    var nickname = rank["nickname"].ToString();
                    nickname = nickname.Replace("\"", "");
                    /// 0 은 맷집 보기
                    RankingAddItem(nickname, score, 0);
                }

                PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);

            }
            else
            {
                Debug.Log("Fail");
                PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);

            }
        });
    }
    public void RankingMini()
    {
        PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);

        plugin.Ranking(MINI_GAME, 50, (state, message, rawData, dictionary) =>
        {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                ArrayList list = (ArrayList)dictionary["list"];
                foreach (Dictionary<string, object> rank in list)
                {
                    Debug.Log(rank["score"]);
                    Debug.Log(rank["nickname"]);
                    ///
                    var score = rank["score"].ToString();
                    var nickname = rank["nickname"].ToString();
                    nickname = nickname.Replace("\"", "");
                    /// 2 은 미니 게임 보기라구
                    RankingAddItem(nickname, score, 2);
                }

                PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);

            }
            else
            {
                Debug.LogError("Fail");
                PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);

            }
        });
    }
    /// <summary>
    /// 무한의 탑
    /// </summary>
    public void RankingMuganTop()
    {
        PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);

        plugin.Ranking(INFI_TOWER, 50, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                ArrayList list = (ArrayList)dictionary["list"];
                foreach (Dictionary<string, object> rank in list)
                {
                    Debug.Log(rank["score"]);
                    Debug.Log(rank["nickname"]);
                    //
                    var score = rank["score"].ToString();
                    var nickname = rank["nickname"].ToString();
                    nickname = nickname.Replace("\"", "");
                    /// 1 은 미니게임 보기 => 무한 탑 층수 보기
                    RankingAddItem(nickname, score, 1);
                }

                PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);

            }
            else
            {
                Debug.Log("Fail");
                PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);

            }
        });
    }


    /// <summary>
    /// 불러온 값 랭킹 카드에 추가하기.
    /// </summary>
    /// <param name="_indx"></param>
    void RankingAddItem(string _nick, string _score, int _index)
    {
        Debug.LogWarning("_nick : " + _nick);
        Debug.LogWarning("_score : " + _score);

        rankingPage.CardInit(_nick, _score, _index);
    }


    /// <summary>
    /// 미니게임 미니미니
    /// </summary>
    public void RankingRecordMinini()
    {
        int tmp = PlayerPrefsManager.GetInstance().MaxGet_MiniGame;

        plugin.RankingRecord(MINI_GAME, tmp, "Mini", (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.LogError("Success MiniGame" + tmp);
            }
            else
            {
                Debug.LogError("Fail MiniGame" + tmp);
            }
        });
    }



    /// <summary>
    /// 무한의 탑
    /// </summary>
    public void RankingRecordMuganTop()
    {
        /// TODO : 무한의 탑 층수 등록해야한다.
        int tmp = PlayerPrefsManager.GetInstance().MaxGet_MuganTop - 1;

        plugin.RankingRecord(INFI_TOWER, tmp, "2", (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    ///// <summary>
    ///// 몇대 버텼냐 = 국밥 몇 그릇 먹었냐
    ///// </summary>
    //public void RankingRecordMiniGame()
    //{
    //    //int tmp = PlayerPrefsManager.GetInstance().MaxGet_GookBap;

    //    //plugin.RankingRecord("mattzip-RANK-17C6FA82-16D1D3C5", tmp, "1", (state, message, rawData, dictionary) => {
    //    //    if (state.Equals(Configure.PN_API_STATE_SUCCESS))
    //    //    {
    //    //        Debug.Log("Success");
    //    //    }
    //    //    else
    //    //    {
    //    //        Debug.Log("Fail");
    //    //    }
    //    //});
    //}


    /// <summary>
    /// 맷집 랭킹 기록은 얘만 불러오면 1 이상 자동 갱신 저장.
    /// </summary>
    public void RankingRecordMattzip()
    {
        string tmp = PlayerPrefsManager.GetInstance().Mat_Mattzip;
        long mattzip = long.Parse(UserWallet.GetInstance().GetMattzipForCul(tmp));


        plugin.RankingRecord(NEW_RANKING, mattzip, "0", (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }



    /// <summary>
    /// Personal Query in LeaderBoard
    /// </summary>
    public void RankingPersonal()
    {
        plugin.RankingPersonal(INFI_TOWER, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(dictionary["ranking"]);
                Debug.Log(dictionary["player_data"]);
                Debug.Log(dictionary["total_player"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    /// <summary>
    /// Season Query in LeaderBoard
    /// </summary>
    public void RankingSeason()
    {
        plugin.RankingSeasonInfo("RANKING_CODE", (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(dictionary["season"]);
                Debug.Log(dictionary["expire_sec"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    // Read the GooglePlay receipt of the sample product on Android.
    // Receipt validation is required.
    public void ReadGooglePlayReceipt()
    {
        //if (Application.platform == RuntimePlatform.Android)
        //{
        //    // EM_IAPConstants.Sample_Product is the generated name constant of a product named "Sample Product".
        //    GooglePlayReceipt receipt = InAppPurchasing.GetGooglePlayReceipt(EM_IAPConstants.Product_allbuff);

        //    if (receipt != null)
        //    {
        //        Debug.Log("Package Name: " + receipt.packageName);
        //        Debug.Log("Product ID: " + receipt.productID);
        //        Debug.Log("Purchase Date: " + receipt.purchaseDate.ToShortDateString());
        //        Debug.Log("Purchase State: " + receipt.purchaseState.ToString());
        //        Debug.Log("Transaction ID: " + receipt.transactionID);
        //        Debug.Log("Purchase Token: " + receipt.purchaseToken);
        //    }
        //}
    }


    /// <summary>
    /// Android Verification
    /// </summary>
    public void IapReceiptionAndroid(string _PRODUCT_ID, string _RECEIPT, string _SIGNATURE, string _CURRENCY, double _Price)
    {
        plugin.ReceiptVerificationAOS(_PRODUCT_ID, _RECEIPT, _SIGNATURE, _CURRENCY, _Price, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.LogWarning(dictionary["package"]);
                Debug.LogWarning(dictionary["product_id"]);
                Debug.LogWarning(dictionary["order_id"]);
                Debug.LogWarning("IapReceiptionAndroid Issue Item");
            }
            else
            {
                Debug.LogWarning("IapReceiptionAndroid Fail");
            }
        });
    }

    //public void Save_RECEIPT(string _PRODUCT_ID, string _RECEIPT, string _SIGNATURE, string _CURRENCY, double _Price)
    //{
    //    PlayerPrefs.Save();

    //    string _message = "";

    //    _message += " _PRODUCT_ID : " + _PRODUCT_ID +
    //" _RECEIPT : " + _RECEIPT +
    //" _SIGNATURE : " + _SIGNATURE +
    //" _CURRENCY : " + _CURRENCY +
    //" _Price : " + _Price + " * "
    //;

    //    plugin.StorageSave(GPGSManager.GetLocalUserId() + "_RECEIPT", _message, true, (state, message, rawData, dictionary) => {
    //        if (state.Equals(Configure.PN_API_STATE_SUCCESS))
    //        {
    //            Debug.LogWarning("StorageSave Success ::");
    //        }
    //        else
    //        {
    //            Debug.LogWarning("StorageSave Fail");
    //        }
    //    });
    //}


    /// <summary>
    /// iOS Verification
    /// </summary>
    public void IapReceiptioniOS()
    {
        plugin.ReceiptVerificationIOS("PRODUCT_ID", "RECEIPT", "CURRENCY", 100, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(dictionary["package"]);
                Debug.Log(dictionary["product_id"]);
                Debug.Log(dictionary["order_id"]);
                Debug.Log("Issue Item");
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void IapReceiptionOneStoreKR()
    {
        plugin.ReceiptVerificationOneStoreKR("PRODUCT_ID", "PURCHASE_ID", "RECEIPT", "CURRENCY", 100, true, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(dictionary["package"]);
                Debug.Log(dictionary["product_id"]);
                Debug.Log(dictionary["order_id"]);
                Debug.Log("Issue Item");
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void Invite(string inviteCode)
    {
        plugin.Invite(inviteCode, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                string url = dictionary["url"].ToString();

                plugin.OpenShare("맷집키우기의 세계에 초대합니다.", url);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void OnApplicationFocus(bool focus)
    {
        if (plugin != null && focus)
        {
            if(GPGSManager.GPGS_Progress())
                AccessEvent();
        }
        //Debug.Log("Focus");
    }
}
