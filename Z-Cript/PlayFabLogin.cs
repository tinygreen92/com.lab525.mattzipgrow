using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayFabLogin : MonoBehaviour
{
    public TutorialMissionManager tmm;
    public TutorialManager tm;
    [Header("- 닉네임 설정")]
    public InputField nickInputText;
    public GameObject  nickPopObject;
    public GameObject  nickParentsObject;
    //
    public Dictionary<string, CatalogItem> catalogItemDic = new Dictionary<string, CatalogItem>();
    public List<ItemInstance> playerInventory = new List<ItemInstance>();

    public static PlayFabLogin instance;

    public GotoMINIgame gotominigame;
    public TapToSpawnLimit tapToSpawnLimit;

    [Header("- PVP 팝업메뉴")]
    public GameObject pvppoup;
    public GameObject TAB_Fight;
    public GameObject TAB_Ranking;
    public GameObject TAB_Reword;

    public Transform View_Fight;
    public Transform View_Ranking;
    public Transform View_Ranking_Child;
    public Transform View_Reword;

    [Header("- 내부 카드메뉴")]
    public Text[] tierText;
    public Text[] userName;
    public Text[] userMattzip;
    public string[] userInnerId;
    public Text pvpTicket;


    [Header("- 랭킹 카드 프리팹")]
    public Transform RankingCard;

    /* PVP 이미지들*/
    [Header("- PVP 이미지들")]
    public GameObject myBody; // 히트 바디
    public GameObject myHP; // 체력바 통째 - child 텍스트나 fillamount
    public GameObject myWeapon; // 장착한 무기 프리팹 등록
    public GameObject myWeaponPos_Right; // <- 에서 무기 나옴

    public GameObject enemyBody;
    public GameObject enemyHP; // 체력바 통째 - child 텍스트나 fillamount
    public GameObject enemyWeapon; // 장착한 무기 프리팹 등록
    public GameObject enemyWeaponPos_Left; // <- 에서 무기 나옴


    [Header("- 내 상대는 몇 명 중에 고를지?")]
    public int enemyUserAmount = 5; // 내 상대는 몇 명 중에 고를지?
    public Text RedNick; // 내 닉네임
    public Text BlueNick; // 나쁜놈 닉네임


    /* PvP 데이터들*/
    [SerializeField]
    private string myPlayFabId; // 내 플레이펩 아이디.
    [SerializeField]
    private string myDisplayName; // 내 플레이펩 닉네임
    private int myRankingPosition;
    //
    private int rankAmount = 100; // 목록 몇개 불러올지?
    //private Dictionary<string, string> enemyList = new Dictionary<string, string>(); // 상대 id와 승점 담을 Dictionary
    private int myTierScore; // 내 승점
    private int rank_index; // 1위부터 100위 표기
    public string[] enemyAllData; // 0부터 순서대로 차곡 차곡 쌓인다




    string myMattzip;
    string myPunchDPS;
    string myCurrentHP;
    string myMaxHp;
    string enemyMattzip;
    string enemyPunchDPS;
    string enemyCurrentHP;
    string enemyMaxHp;
    //
    int _ACC;
    double _ATK;
    int _BACK_GND;
    float _CRC;
    float _CRD;
    float _HP;
    float _HP_REC;
    double _MATT;
    int _UNIFORM;
    string _userName;



    /// <summary>
    /// 탈주함?
    /// </summary>
    bool isTalZoo;
    void OnApplicationQuit()
    {
        /* 앱이 종료 될 때 처리 */
        if (isTalZoo)
        {
            SubmitScore(myTierScore - 2);
            isTalZoo = false;
            PlayerPrefs.Save();
        }
    }






    /// <summary>
    /// 5개 중에 하나 눌러서 입장.
    /// </summary>
    /// <param name="_index"></param>
    public void ClickedEnterBtn(int _index)
    {
        /// 내가 싸울 유저의 플레이팹 데이터 얻어옴
        _ACC = int.Parse(enemyAllData[(_index * 10)]);
        _ATK = float.Parse(enemyAllData[(_index * 10) + 1]);
        _BACK_GND = int.Parse(enemyAllData[(_index * 10) + 2]);
        _CRC = float.Parse(enemyAllData[(_index * 10) + 3]);
        _CRD = float.Parse(enemyAllData[(_index * 10) + 4]);
        _HP = float.Parse(enemyAllData[(_index * 10) + 5]);
        _HP_REC = float.Parse(enemyAllData[(_index * 10) + 6]);  // 황금 망토

        if (_HP_REC > 50f) _HP_REC = 0;
        else if (_HP_REC > 40f) _HP_REC = 40f;

        _MATT = double.Parse(enemyAllData[(_index * 10) + 7]);
        _UNIFORM = int.Parse(enemyAllData[(_index * 10) + 8]);
        _userName = enemyAllData[(_index * 10) + 9];





        /// 소숫점 절삭 string 맷집+방어
        enemyMattzip = _MATT.ToString("f0");
        /// 절삭 후 맷집 + 방어
        eBlockedDam = double.Parse(enemyMattzip);
        /// 상대방의 맷집 + 방어를 뺀 나의 공격력
        myPunchDPS = dts.SubStringDouble(PlayerPrefsManager.GetInstance().PlayerDPS, enemyMattzip);
        Debug.LogWarning("상대방의 맷집 + 방어를 뺀 나의 공격력 :: " + myPunchDPS);

        if (myPunchDPS == "-1")
        {
            myPunchDPS = "0";
            fAttackedDam = 0;
        }
        else
        {
            fAttackedDam = double.Parse(myPunchDPS);
        }

        /// 내 스펙 기본 세팅 맷집 10% + (0.04d * PlayerPrefsManager.GetInstance().Pet_PVP_Matt_Lv)
        double dsfds = double.Parse(PlayerPrefsManager.GetInstance().GetPvpMattDefence());
        /// 소수점 절삭
        myMattzip = dsfds.ToString("f0");
        // 맷집 뺀 펀치공격력
        enemyPunchDPS = dts.SubStringDouble(_ATK.ToString("f0"), myMattzip);
        Debug.LogWarning("enemyPunchDPS :: " + enemyPunchDPS);

        if (enemyPunchDPS == "-1")
        {
            enemyPunchDPS = "0";
            eAttackedDam = 0;
        }
        else
        {
            eAttackedDam = double.Parse(enemyPunchDPS);
        }

        fBlockedDam = double.Parse(enemyPunchDPS);


        myMaxHp = PlayerPrefsManager.GetInstance().Mat_MaxHP;
        myCurrentHP = myMaxHp;
        enemyMaxHp = _HP.ToString("f0");
        enemyCurrentHP = enemyMaxHp;

        myHP.transform.GetChild(0).GetComponent<Image>().fillAmount = 1f;
        myHP.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = UserWallet.GetInstance().SeetheNatural(float.Parse(myMaxHp)) + "/" + UserWallet.GetInstance().SeetheNatural(float.Parse(myMaxHp));
        myHP.transform.GetChild(1).GetComponent<Text>().text = GPGSManager.GetLocalUserName();

        enemyHP.transform.GetChild(0).GetComponent<Image>().fillAmount = 1f;
        enemyHP.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = UserWallet.GetInstance().SeetheNatural(_HP) + "/" + UserWallet.GetInstance().SeetheNatural(_HP);
        enemyHP.transform.GetChild(1).GetComponent<Text>().text = _userName;
        BlueNick.text = _userName;
        RedNick.text = GPGSManager.GetLocalUserName();

        // 피비피 외적 소요 덮어씌우기
        GameObject.Find("CharacterManager").GetComponent<CharacterManager>().Chage_My_Pvp(PlayerPrefs.GetInt("Uniform_Curent", 0), PlayerPrefs.GetInt("Pet_Curent", 0), 0);
        //에너미
        GameObject.Find("CharacterManager").GetComponent<CharacterManager>().Chage_My_Pvp(_UNIFORM, _ACC, 3);


        // 이제 승리 / 패배 전에 강제 종료 하면 승점 잃음.
        isTalZoo = true;

        // 피비피 게임판 입장
        pvppoup.SetActive(false);
        gotominigame.EnterThePVP_WarGame();
    }


    /* UI 표기 관련 */
    bool isRainkingLoading; // X 눌러서 나가면 false

    /// <summary>
    /// X 표시로 밖으로 나갈때 랭킹 프로세스 초기화
    /// </summary>
    public void StopTheRankLoading()
    {
        isRainkingLoading = false;
        if (popopopom != null) StopCoroutine(popopopom);
        isGetAllDataCheak3 = true;
    }

    /// <summary>
    /// pvp 팝업 버튼 클릭시 이 메소드로 호출
    /// </summary>
    public void OpenPopUP()
    {
        // 텅 빈 화면 
        pvppoup.SetActive(true);
        // 티켓 갯수
        pvpTicket.text = PlayerPrefsManager.GetInstance().ticket.ToString("N0");
        /// 랭킹 갱신
        Tap_Click(0);
    }
    /// <summary>
    /// pvp 상단 버튼 누르기 관리
    /// </summary>
    /// <param name="_buttonIndex">버튼 이벤트에 붙일때 인덱스 지정</param>
    public void Tap_Click(int _buttonIndex)
    {
        switch (_buttonIndex)
        {
            case 0:
                /// 사용자 pvp 데이터 최신화.
                SetDataInit();
                // 로딩중 애니메이션
                PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);
                //SetData();
                TAB_Fight.SetActive(true);
                TAB_Ranking.SetActive(false);
                TAB_Reword.SetActive(false);

                View_Fight.GetChild(0).gameObject.SetActive(false);

                /// TODO : 5개 창 빈칸 만들기.
                for (int i = 0; i < View_Fight.GetChild(0).childCount; i++)
                {
                    tierText[i].text = "";
                    userName[i].text = "";
                    userMattzip[i].text = "";
                }
                // 빈 창에다가 내용물 채워줌
                Init_Fight();

                break;

            case 1:
                //// 들어간다 멈춰줌.
                //isGetAllDataCheak3 = true;
                //if (popopopom != null) StopCoroutine(popopopom);


                TAB_Fight.SetActive(false);
                TAB_Ranking.SetActive(true);
                TAB_Reword.SetActive(false);

                if (isRainkingLoading)
                {
                    View_Fight.gameObject.SetActive(false);
                    View_Ranking.gameObject.SetActive(true);
                    View_Ranking_Child.gameObject.SetActive(true);
                    View_Reword.gameObject.SetActive(false);

                    return;
                }

                isRainkingLoading = true;

                while (View_Ranking.GetChild(0).childCount != 0)
                {
                    Lean.Pool.LeanPool.Despawn(View_Ranking.GetChild(0).GetChild(0));
                }
                // 랭킹 인덱스 초기화.
                rank_index = 0;
                /// 랭킹 표시 초기화
                Init_Rank();
                break;

            case 2:
                //// 들어간다 멈춰줌.
                //isGetAllDataCheak3 = true;
                //if (popopopom != null) StopCoroutine(popopopom);

                if (TAB_Reword.activeSelf) return;

                TAB_Fight.SetActive(false);
                TAB_Ranking.SetActive(false);
                TAB_Reword.SetActive(true);
                //
                View_Ranking.gameObject.SetActive(false);

                View_Fight.gameObject.SetActive(false);
                View_Ranking.gameObject.SetActive(false);
                View_Ranking_Child.gameObject.SetActive(false);
                View_Reword.gameObject.SetActive(true);

                break;
        }
    }

    /// <summary>
    /// 파이팅 페이지에서는 미리 박아둔 배열 5에 텍스트 박스 채워줌 
    /// </summary>
    private void Init_Fight()
    {
        View_Fight.gameObject.SetActive(true);
        View_Ranking.gameObject.SetActive(false);
        View_Ranking_Child.gameObject.SetActive(false);
        View_Reword.gameObject.SetActive(false);
    }

    /// <summary>
    /// 랭킹 표시 초개화는 스크롤뷰에 프리팹 100개 붙여주기
    /// </summary>
    private void Init_Rank()
    {
        View_Fight.gameObject.SetActive(false);
        View_Ranking.gameObject.SetActive(true);
        View_Ranking_Child.gameObject.SetActive(true);
        View_Reword.gameObject.SetActive(false);
        //
        // 플레이팹 랭킹 100개 긁기 -> result
        RequestLeaderboad();

    }


    /// <summary>
    /// 게임 켜면 최소한 이거는 실행되게
    /// </summary>
    public void SetDataInit()
    {
        UpdateUserDataRequest request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                /// 알파벳 순서 enemyAllData 에 차곡 차곡 10개씩 cnt / 10 == index 0 부터 시작
                { "ACC", PlayerPrefs.GetInt("Pet_Curent", 0).ToString() },
                { "ATK", PlayerPrefsManager.GetInstance().PlayerDPS },
                { "BACK_GND", PlayerPrefs.GetInt("PunchIndex", 0).ToString() },
                { "CRC", PlayerPrefsManager.GetInstance().Critical_Per },
                { "CRD", PlayerPrefsManager.GetInstance().CriticalDPS },
                { "HP", PlayerPrefsManager.GetInstance().Mat_MaxHP },
                { "HP_REC", (0.04d * PlayerPrefsManager.GetInstance().Pet_PVP_Matt_Lv).ToString("f2") },
                { "MATT", PlayerPrefsManager.GetInstance().GetPvpMattDefence() },
                { "UNIFORM", PlayerPrefs.GetInt("Uniform_Curent", 0).ToString() },
                { "userName", GPGSManager.GetLocalUserName() },

            },

            Permission = UserDataPermission.Public
        };

        PlayFabClientAPI.UpdateUserData(request,
            (result) =>
            {
                RequestMyBored();
            },
            (error) => Debug.LogWarning("데이터 저장 실패")
            );
    }




    public void Start()
    {
        instance = this;
        /// 플래이팹 로그인 시도
        StartCoroutine(InitPlayfab());
    }


    //private void OnLoginSuccess(LoginResult result)
    //{
    //    Debug.LogError("플레이팹 로그인 성공!");

    //    myPlayFabId = result.PlayFabId;

    //    // 인벤토리 열어봐서 동전 찾기
    //    PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
    //        OnGetUserInventory,
    //        (error) => Debug.LogError(error.GenerateErrorReport()));
    //}

    /// <summary>
    /// 구글 아이디로 플레이팹 로그인 시도
    /// </summary>
    IEnumerator InitPlayfab()
    {
        yield return null;
        while (!GPGSManager.GPGS_Progress())
        {
            yield return new WaitForFixedUpdate();
        }

        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "A42E1";
        }
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest { CustomId = GPGSManager.GetLocalUserId(), CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult obj)
    {
        /// 로그인 성공시 내 아이디 기억
        myPlayFabId = obj.PlayFabId;
        /// 주간 pvp 보상 조회후 지급
        RewordRC();
        /// 맷집 공통 데이터 조회
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
                result =>
                {
                    /// 클라이언트 버전 체크용으로 사용
                    if (result.Data == null || !result.Data.ContainsKey("CurrentVersion"))
                    {
                        Debug.LogWarning("No CurrentVersion");
                    }
                    /// 데이터 호출 성공시.
                    else
                    {
                        /// 최신 버전이면 패스.
                        if (Application.version == result.Data["CurrentVersion"] || Application.version == result.Data["TEST_Mode"])
                        {
                            /// 공지사항 넣어주고
                            //PlayerPrefsManager.instance.CH_NOTICE = result.Data["ChatNotice"];
                        }
                        /// 최신 버전 아님
                        else
                        {
                            /// 업데이트 팝업
                            //PopUpManager.instance.ShowPopUP(31);
                        }
                    }
                },
                error =>
                {
                    Debug.Log("Got error getting titleData:");
                    Debug.Log(error.GenerateErrorReport());
                });
        /// 닉네임 설정 관련
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest { PlayFabId = myPlayFabId },
                                GetAccountSuccess,
                                (error) => Debug.LogError(" GetAccountInfo error"));
    }

    private void GetAccountSuccess(GetAccountInfoResult obj)
    {
        myDisplayName = obj.AccountInfo.TitleInfo.DisplayName;
        /// 닉네임 설정이 안되었다 ||  혹은 도중에 취소했다 (임시로 구글 아이디로 저장)
        if (myDisplayName == null || myDisplayName == myPlayFabId)          
        {
            /// 유료 결제 내역 복구
            var tmp = PlayerPrefs.GetInt("VIP", 0);
            /// 모든 데이터 삭제
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("isFristGameStart", 1);
            PlayerPrefs.SetInt("isSignFirst", 1);
            PlayerPrefs.SetInt("isDataSaved", 1);
            PlayerPrefs.SetInt("VIP", tmp);
            PlayerPrefs.Save();
            //
            /// TODO : 닉네임 설정 팝업창. 표기
            nickParentsObject.SetActive(true);
        }
        else
        {
            /// 재접속시 닉네임 설정되어 있네? 
            GPGSManager.SetNickName(myDisplayName);
            /// 나누 접속
            GameObject.Find("PlayNanoo").GetComponent<PlayNANOOExample>().NanooStart();
            /// 포톤 접속
            GameObject.Find("Scripts").GetComponent<NamePickGui>().AutoStartChat();
            /// 페이크 로딩창 꺼줌 
            tm.FakeloadingOnOff(false);
        }
    }

    /// <summary>
    ///5.SetNickNameCanvas 에서 확인 누르면 닉네임 중복확인 해보기
    /// </summary>
    public void CheckSameName()
    {
        string _userName;
        switch (nickInputText.text.Length)
        {
            case 0:
                return;
            case 1:
                return;
            case 2:
                _userName = nickInputText.text + " ";
                break;
                /// 3글자 이상은 시스템.
            default:
                _userName = nickInputText.text;
                break;
        }
        /// 유저 디스플레이 네임 세팅 nickInputText
        UpdateUserName(_userName);
    }

    /// <summary>
    /// 닉네임 중복확인
    /// </summary>
    /// <param name="_dpName">방금 입력한 닉네임</param>
    void UpdateUserName(string _dpName)
    {
        /// 유저 디스플레이 네임 세팅
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = _dpName },
        (result) =>
        {
            Debug.LogWarning("유저 디스플레이 네임 : " + _dpName);
            myDisplayName = _dpName;
            /// 팝업 씹고 바로 생성.
            OkayMyNick();

            /// 창꺼줌
            nickParentsObject.SetActive(false);
            /// 이걸로 할래? -> 생성하시겠습니까?
            //nickPopObject.SetActive(true);
            //PopUpManager.instance.SetNickCheckTxt(_dpName);
            //PopUpManager.instance.ShowNickPanel(2);
        },
        (error) =>
        {
            /// 중복 !! 경고 팝업 호출 -> 중복된 닉네임입니다.
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("중복된 닉네임입니다.");
        });
    }

    /// <summary>
    /// 팝업에서 닉네임 확정
    /// </summary>
    public void OkayMyNick()
    {
        /// 닉 팝업 끄기
        nickPopObject.SetActive(false);
        /// 닉네임을 설정하자
        GPGSManager.SetNickName(myDisplayName);
        /// 나누 접속
        GameObject.Find("PlayNanoo").GetComponent<PlayNANOOExample>().NanooStart();
        /// 포톤 접속
        GameObject.Find("Scripts").GetComponent<NamePickGui>().AutoStartChat();
        /// 최초 한번 랭킹 차등 보상 지급
        Invoke(nameof(InvoDDD), 2.6f);
        /// 페이크 로딩창 꺼줌
        tm.FakeloadingOnOff(false);
    }

    void InvoDDD()
    {
        /// 최초 접속시 이전 맷집력 랭킹 비례 다이아몬드 지급
        GameObject.Find("PlayNanoo").GetComponent<PlayNANOOExample>().BeforeRankingMatt();
    }

    /// <summary>
    /// 으로 하시겠습니까? -> 취소 누르면 다시 입력 기회 줌
    /// </summary>
    public void NoThisName()
    {
        /// 닉 팝업 끄기
        nickPopObject.SetActive(false);
        /// 널 되돌려 달라고
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = myPlayFabId },
        (result) => myDisplayName = myPlayFabId,
        (error) => Debug.LogError(error.GenerateErrorReport()));
    }



    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        /// 차단 계정 메시지는 여기로
        Debug.LogError(error.GenerateErrorReport());
    }



    /// <summary>
    /// 랭킹 코인 조회
    /// </summary>
    private void RewordRC()
    {
        // 인벤토리 열어봐서 동전 찾기
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            OnGetUserInventory,
            (error) => Debug.LogError(error.GenerateErrorReport()));
    }

    int gold = 0;
    private void OnGetUserInventory(GetUserInventoryResult obj)
    {
        gold = obj.VirtualCurrency["RC"];

        Debug.LogWarning("pvp 랭킹 계수 [RC] : " + gold);

        /// 플레이펩 서버에 PVP에 필요한 데이터 갱신
        SetDataInit();

        // 카탈로그 생성
        GetCatalogItemsRequest request = new GetCatalogItemsRequest() { CatalogVersion = "0" };

        PlayFabClientAPI.GetCatalogItems(request, OnGetCatalogItem,
            (error) => Debug.LogError(error.GenerateErrorReport()));
    }

    void OnGetCatalogItem(GetCatalogItemsResult result)
    {
        //Debug.LogError("OnGetCatalogITem\n" + result.ToJson());

        if (catalogItemDic.Count == 0)
        {
            foreach (CatalogItem item in result.Catalog)
            {
                catalogItemDic.Add(item.ItemId, item);
            }

            //Debug.LogError("완료 : catalogItemDic");
        }

        switch (gold)
        {
            case 0: break;
            case 1:
                if (catalogItemDic.ContainsKey("Ranking_1"))
                {
                    CatalogItem item = catalogItemDic["Ranking_1"];
                    PurchaseItemRequest request = new PurchaseItemRequest()
                    {
                        CatalogVersion = item.CatalogVersion,
                        ItemId = item.ItemId,
                        VirtualCurrency = "RC",
                        Price = (int)item.VirtualCurrencyPrices["RC"],
                    };

                    GameObject.Find("PlayNanoo").GetComponent<PlayNANOOExample>().PostboxItemSend("diamond", 1200, "PvP 주간보상");

                    PlayFabClientAPI.PurchaseItem(request, OnPurchaseItemSuccess, OnPurchaseItemFail);
                }
                break;
            case 2:
                if (catalogItemDic.ContainsKey("Ranking_2"))
                {
                    CatalogItem item = catalogItemDic["Ranking_2"];
                    PurchaseItemRequest request = new PurchaseItemRequest()
                    {
                        CatalogVersion = item.CatalogVersion,
                        ItemId = item.ItemId,
                        VirtualCurrency = "RC",
                        Price = (int)item.VirtualCurrencyPrices["RC"],
                    };
                    GameObject.Find("PlayNanoo").GetComponent<PlayNANOOExample>().PostboxItemSend("diamond", 960, "PvP 주간보상");

                    PlayFabClientAPI.PurchaseItem(request, OnPurchaseItemSuccess, OnPurchaseItemFail);
                }
                break;
            case 3:
                if (catalogItemDic.ContainsKey("Ranking_3"))
                {
                    CatalogItem item = catalogItemDic["Ranking_3"];
                    PurchaseItemRequest request = new PurchaseItemRequest()
                    {
                        CatalogVersion = item.CatalogVersion,
                        ItemId = item.ItemId,
                        VirtualCurrency = "RC",
                        Price = (int)item.VirtualCurrencyPrices["RC"],
                    };
                    GameObject.Find("PlayNanoo").GetComponent<PlayNANOOExample>().PostboxItemSend("diamond", 640, "PvP 주간보상");

                    PlayFabClientAPI.PurchaseItem(request, OnPurchaseItemSuccess, OnPurchaseItemFail);
                }
                break;
            case 4:
                if (catalogItemDic.ContainsKey("Ranking_4-10"))
                {
                    CatalogItem item = catalogItemDic["Ranking_4-10"];
                    PurchaseItemRequest request = new PurchaseItemRequest()
                    {
                        CatalogVersion = item.CatalogVersion,
                        ItemId = item.ItemId,
                        VirtualCurrency = "RC",
                        Price = (int)item.VirtualCurrencyPrices["RC"],
                    };
                    GameObject.Find("PlayNanoo").GetComponent<PlayNANOOExample>().PostboxItemSend("diamond", 500, "PvP 주간보상");

                    PlayFabClientAPI.PurchaseItem(request, OnPurchaseItemSuccess, OnPurchaseItemFail);
                }
                break;
            case 5:
                if (catalogItemDic.ContainsKey("Ranking_11-30"))
                {
                    CatalogItem item = catalogItemDic["Ranking_11-30"];
                    PurchaseItemRequest request = new PurchaseItemRequest()
                    {
                        CatalogVersion = item.CatalogVersion,
                        ItemId = item.ItemId,
                        VirtualCurrency = "RC",
                        Price = (int)item.VirtualCurrencyPrices["RC"],
                    };
                    GameObject.Find("PlayNanoo").GetComponent<PlayNANOOExample>().PostboxItemSend("diamond", 400, "PvP 주간보상");

                    PlayFabClientAPI.PurchaseItem(request, OnPurchaseItemSuccess, OnPurchaseItemFail);
                }
                break;
            case 6:
                if (catalogItemDic.ContainsKey("Ranking_31-50"))
                {
                    CatalogItem item = catalogItemDic["Ranking_31-50"];
                    PurchaseItemRequest request = new PurchaseItemRequest()
                    {
                        CatalogVersion = item.CatalogVersion,
                        ItemId = item.ItemId,
                        VirtualCurrency = "RC",
                        Price = (int)item.VirtualCurrencyPrices["RC"],
                    };
                    GameObject.Find("PlayNanoo").GetComponent<PlayNANOOExample>().PostboxItemSend("diamond", 300, "PvP 주간보상");

                    PlayFabClientAPI.PurchaseItem(request, OnPurchaseItemSuccess, OnPurchaseItemFail);
                }
                break;
            case 7:
                if (catalogItemDic.ContainsKey("Ranking_51-100"))
                {
                    CatalogItem item = catalogItemDic["Ranking_51-100"];
                    PurchaseItemRequest request = new PurchaseItemRequest()
                    {
                        CatalogVersion = item.CatalogVersion,
                        ItemId = item.ItemId,
                        VirtualCurrency = "RC",
                        Price = (int)item.VirtualCurrencyPrices["RC"],
                    };
                    GameObject.Find("PlayNanoo").GetComponent<PlayNANOOExample>().PostboxItemSend("diamond", 200, "PvP 주간보상");

                    PlayFabClientAPI.PurchaseItem(request, OnPurchaseItemSuccess, OnPurchaseItemFail);
                }
                break;
            case 8:
                if (catalogItemDic.ContainsKey("Ranking_101"))
                {
                    CatalogItem item = catalogItemDic["Ranking_101"];
                    PurchaseItemRequest request = new PurchaseItemRequest()
                    {
                        CatalogVersion = item.CatalogVersion,
                        ItemId = item.ItemId,
                        VirtualCurrency = "RC",
                        Price = (int)item.VirtualCurrencyPrices["RC"],
                    };
                    GameObject.Find("PlayNanoo").GetComponent<PlayNANOOExample>().PostboxItemSend("diamond", 100, "PvP 주간보상");

                    PlayFabClientAPI.PurchaseItem(request, OnPurchaseItemSuccess, OnPurchaseItemFail);
                }
                break;

            default:

                if (gold > 9) GameObject.Find("PlayNanoo").GetComponent<PlayNANOOExample>().PostboxItemSend("diamond", 150, "PvP 지난시즌 보상");

                if (catalogItemDic.ContainsKey("Ranking_525"))
                {
                    CatalogItem item = catalogItemDic["Ranking_525"];
                    PurchaseItemRequest request = new PurchaseItemRequest()
                    {
                        CatalogVersion = item.CatalogVersion,
                        ItemId = item.ItemId,
                        VirtualCurrency = "RC",
                        Price = (int)item.VirtualCurrencyPrices["RC"],
                    };
                    PlayFabClientAPI.PurchaseItem(request, OnPurchaseItemSuccess, OnPurchaseItemFail);
                }

                break;
        }


    }

    private void OnPurchaseItemFail(PlayFabError obj)
    {
        Debug.LogError("OnPurchaseItemFail -> 그럼 지갑 초기화 >" + gold + "<");

        SubtractUserVirtualCurrencyRequest request = new SubtractUserVirtualCurrencyRequest() { VirtualCurrency = "RC", Amount = gold };
        PlayFabClientAPI.SubtractUserVirtualCurrency(request, (result) => Debug.LogError("돈 빼기 성공! 현재 돈 : " + result.Balance), (error) => Debug.LogError("돈 빼기 실패"));
    }

    private void OnPurchaseItemSuccess(PurchaseItemResult obj)
    {
        foreach (ItemInstance item in obj.Items)
        {
            //Debug.LogError("item.UnitPrice : " + item.UnitPrice.ToString());
        }
    }





    ///// <summary>
    ///// PVP 데이터 갱신
    ///// </summary>
    //public void SetData()
    //{
    //    UpdateUserDataRequest request = new UpdateUserDataRequest()
    //    {
    //        Data = new Dictionary<string, string>()
    //        {
    //            /// 알파벳 순서 enemyAllData 에 차곡 차곡 10개씩 cnt / 10 == index 0 부터 시작
    //            { "ACC", PlayerPrefs.GetInt("Pet_Curent", 0).ToString() },
    //            { "ATK", PlayerPrefsManager.GetInstance().PlayerDPS },
    //            { "BACK_GND", PlayerPrefs.GetInt("PunchIndex", 0).ToString() },
    //            { "CRC", PlayerPrefsManager.GetInstance().Critical_Per },
    //            { "CRD", PlayerPrefsManager.GetInstance().CriticalDPS },
    //            { "HP", PlayerPrefsManager.GetInstance().Mat_MaxHP },
    //            { "HP_REC", (0.04d * PlayerPrefsManager.GetInstance().Pet_PVP_Matt_Lv).ToString("f2") },
    //            { "MATT", PlayerPrefsManager.GetInstance().Mat_Mattzip },
    //            { "UNIFORM", PlayerPrefs.GetInt("Uniform_Curent", 0).ToString() },
    //            { "userName", GPGSManager.GetLocalUserName() },

    //        },

    //        Permission = UserDataPermission.Public
    //    };

    //    PlayFabClientAPI.UpdateUserData(request,
    //        (result) => SuccessSetData(),
    //        (error) => Debug.LogWarning("데이터 저장 실패")
    //        );
    //}

    //// setData 성공했다면
    //private void SuccessSetData()
    //{
    //    RequestMyBored();
    //}

    /// <summary>
    /// 피비피 내 랭킹
    /// </summary>
    void RequestMyBored()
    {
        PlayFabClientAPI.GetLeaderboardAroundPlayer(new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "PVP_TIER",
            //PlayFabId = _id, //자기 자신이면 생략
            MaxResultsCount = 1 //나만
        }, result =>
        {
            foreach (PlayerLeaderboardEntry entry in result.Leaderboard)
            {
                Debug.LogError(entry.Position + " 내 ID : " + entry.PlayFabId + " PVP_Tier : " + entry.StatValue);
                myTierScore = entry.StatValue;
            }

            // 등록된 데이터 없다면 티어 1000부터 시작
            if (myTierScore == 0)
            {
                SubmitScore(1000);
                myTierScore = 1000;
                //RequestAroundLeaderboard(); 없는거 정상 submit 안에 들어있다.
                if (PlayerPrefsManager.GetInstance().isFirstPVP && gold == 0)
                {
                    // 해당 시즌 초기화 되면 순위권 아니면(인벤토리에 돈 없으면) 다이아 보상
                    GameObject.Find("PlayNanoo").GetComponent<PlayNANOOExample>().PostboxItemSend("diamond", 100, "PvP 주간보상");
                }
                else
                {
                    RewordRC();
                }
                // 해당 시즌 퍼스트블러드 해제
                PlayerPrefsManager.GetInstance().isFirstPVP = false;
            }
            else
            {
                // 5개 유저 불러오기
                RequestAroundLeaderboard();
            }


        },
        FailureCallback);
    }


    bool isGetAllDataCheak3;
    public void GetData(string _playFabId, int _tierScore)
    {
        string tmoStr = "";
        GetUserDataRequest request = new GetUserDataRequest() { PlayFabId = _playFabId };
        PlayFabClientAPI.GetUserData(request, (result) =>
        {
            if (result.Data.Count == 0)
            {
                tmoStr = "Unknown";
            }
            else tmoStr = result.Data["userName"].Value;

            // 다음 정보가 없으면?
            if (_playFabId.Length == 0)
            {
                isGetAllDataCheak3 = true;
            }
            else
            {
                CardInit(tmoStr, _tierScore.ToString());
                rank_index++;
                Debug.LogWarning("_playFabId : " + _playFabId + "들어간다~");
            }

            isGetAllDataCheak2 = true;

        },
        (error) => Debug.LogWarning("데이터 불러오기 실패"));
    }

    public Transform myRankingBox;
    public void GetMyData(string _playFabId, int _tierScore)
    {

        myRankingBox.GetChild(0).GetChild(3).GetChild(0).GetComponent<Text>().text = (myRankingPosition + 1).ToString();

        /// PVP 한적이 없다?
        if (myTierScore == 1000 && !PlayerPrefsManager.GetInstance().isFirstPVP) myRankingBox.GetChild(0).GetChild(3).GetChild(0).GetComponent<Text>().text = "-";

        myRankingBox.GetChild(0).GetChild(3).gameObject.SetActive(true);
        myRankingBox.GetChild(1).GetComponent<Text>().text = GPGSManager.GetLocalUserName();
        myRankingBox.GetChild(2).GetComponent<Text>().text = _tierScore.ToString();
    }

    /// <summary>
    /// 플레이팹스 고유 아이디로 접근해야함 / 타이틀 데이터는 public 이어야 함
    /// </summary>
    /// <param name="_playFabId"></param>
    public void GetAllData(string _playFabId, int _cnt)
    {
        // 인텍스 0 부터 시작 0~9 / 10~ 19/ 20~29
        _cnt = _cnt * 10;

        GetUserDataRequest request = new GetUserDataRequest() { PlayFabId = _playFabId };
        PlayFabClientAPI.GetUserData(request, (result) =>
        {
            if (result.Data.Count == 1)
            {
                enemyAllData[_cnt] = "3";
                enemyAllData[_cnt + 1] = "1";
                enemyAllData[_cnt + 2] = "5";
                enemyAllData[_cnt + 3] = "1";
                enemyAllData[_cnt + 4] = "1";
                enemyAllData[_cnt + 5] = "525";
                enemyAllData[_cnt + 6] = "0"; // 황금망토
                enemyAllData[_cnt + 7] = "1"; //  세팅 맷집 10%
                enemyAllData[_cnt + 8] = "2";
                enemyAllData[_cnt + 9] = "unknown";
            }
            else
            {
                foreach (KeyValuePair<string, UserDataRecord> eachData in result.Data)
                {
                    //Debug.Log(eachData.Key + " : " + eachData.Value.Value + "\n");
                    enemyAllData[_cnt] = eachData.Value.Value;
                    //
                    _cnt++;
                }
            }



            isGetAllDataCheak = true;
        },

        (error) => Debug.LogWarning("데이터 불러오기 실패"));
    }




    /// <summary>
    /// 랭킹 보드에 점수 등록
    /// </summary>
    /// <param name="playerScore"></param>
    public void SubmitScore(int playerScore)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
            new StatisticUpdate {
                StatisticName = "PVP_TIER",
                Value = playerScore
            }
        }
        }, result => OnStatisticsUpdated(result), FailureCallback);
    }


    /// <summary>
    /// 내 주변 랭킹 읽어오기 -> 팝업 아이콘 클릭시.
    /// </summary>
    public void RequestAroundLeaderboard()
    {
        PlayFabClientAPI.GetLeaderboardAroundPlayer(new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "PVP_TIER",
            //PlayFabId = _id, //자기 자신이면 생략
            MaxResultsCount = enemyUserAmount + 1 // 앞뒤로 3명 3명 채워서 뽑기
        }, result => DisplayAroundLeaderboard(result), FailureCallback);
    }

    private void DisplayAroundLeaderboard(GetLeaderboardAroundPlayerResult result)
    {
        // 0~ 5 카운트 
        int _index = 0;

        foreach (PlayerLeaderboardEntry entry in result.Leaderboard)
        {
            //Debug.LogError(entry.Position + " Leaderboard ID : " + entry.PlayFabId + " Tier Score : " + entry.StatValue);
            //자기 자신은 제외하고
            if (entry.PlayFabId == myPlayFabId)
            {
                myRankingPosition = entry.Position;
                myTierScore = entry.StatValue;
                continue;
            }
            else
            {
                // 티어 점수 채우기 
                tierText[_index].text = entry.StatValue.ToString();
                // 내부 아이디 저장
                userInnerId[_index] = entry.PlayFabId;
                // 다음
                _index++;
            }
        }

        /// 데이터 다 긁어오기
        StartCoroutine(Cocomong());

    }



    bool isGetAllDataCheak;
    /// <summary>
    /// 서버 통신 대기 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator Cocomong()
    {
        yield return null;

        for (int _index = 0; _index < 5; _index++)
        {
            isGetAllDataCheak = false;
            // 내부 아이디 획득 후 일괄 데이터 저장
            GetAllData(userInnerId[_index], _index);
            while (!isGetAllDataCheak)
            {
                yield return new WaitForFixedUpdate();
            }
            // 나누 아이디로 변환해서 텍스트 채우기
            userName[_index].text = enemyAllData[(10 * _index) + 9];
            // 맷집 택스트 채우기
            userMattzip[_index].text = enemyAllData[(10 * _index) + 7];
        }

        PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);
        View_Fight.GetChild(0).gameObject.SetActive(true);

    }


    /// <summary>
    /// 랭킹 보드에서 데이터 읽어오기
    /// 1. DisplayLeaderboard().entry.PlayFabId 로 플레이어 시드 획득
    /// 2. 
    /// 3. GetData(PlayFabId)[key, value]로 pvp에 필요한 데이터 접근
    /// 3. 
    /// </summary>
    public void RequestLeaderboad()
    {
        // 0-100 count
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = "PVP_TIER",
            StartPosition = 0,
            MaxResultsCount = rankAmount
        }, result => DisplayLeaderboard(result), FailureCallback);
    }

    string[] innerPlayFabId = new string[100];
    int[] innerStatValue = new int[100];
    private void DisplayLeaderboard(GetLeaderboardResult result)
    {
        // 내 기록 열람
        GetMyData(myPlayFabId, myTierScore);

        // 0~ 100 카운트 
        int _index = 0;

        foreach (PlayerLeaderboardEntry entry in result.Leaderboard)
        {
            //Debug.LogError(entry.Position + " Leaderboard ID : " + entry.PlayFabId + " Tier Score : " + entry.StatValue);
            // 이걸로 플레이어 데이터 긁어오기 가능
            //GetData(entry.PlayFabId, entry.StatValue);
            innerPlayFabId[_index] = entry.PlayFabId;
            innerStatValue[_index] = entry.StatValue;

            _index++;
        }
        /// 데이터 다 긁어오기
        popopopom = StartCoroutine(popomong());
    }
    Coroutine popopopom;
    bool isGetAllDataCheak2;
    /// <summary>
    /// 서버 통신 대기 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator popomong()
    {
        yield return null;
        isGetAllDataCheak3 = false;

        for (int _index = 0; _index < 100; _index++)
        {
            yield return null;
            isGetAllDataCheak2 = false;
            // 내부 아이디 획득 후 일괄 데이터 저장
            GetData(innerPlayFabId[_index], innerStatValue[_index]);

            while (!isGetAllDataCheak2)
            {
                yield return new WaitForFixedUpdate();
            }

            if (isGetAllDataCheak3)
            {
                isGetAllDataCheak3 = false;
                break;
            }

        }

        PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);
    }


    private void OnStatisticsUpdated(UpdatePlayerStatisticsResult updateResult)
    {
        // 5개 유저 불러오기
        RequestAroundLeaderboard();
        Debug.LogError("Successfully submitted high score");
    }

    private void FailureCallback(PlayFabError error)
    {
        PopUpObjectManager.GetInstance().HIdeInfiProcess();

        /// 플레이팹에서 플레이어 정보 얻어오는데 실패했따.
        Debug.LogWarning("플레이팹에서 플레이어 정보 얻어오는데 실패했따. information:");
        Debug.LogError(error.GenerateErrorReport());
    }


    /// <summary>
    /// 랭킹 페이지 진입시 100개 프리팹 뿌려주기
    /// </summary>
    /// <param name="name">나누 닉네임</param>
    /// <param name="score">순위</param>
    public void CardInit(string name, string score)
    {
        Transform eneObj = RankingCard;
        //프리팹에서 박스 생성
        Transform initBox = Lean.Pool.LeanPool.Spawn(eneObj);

        initBox.SetParent(View_Ranking.GetChild(0)); // 스크롤뷰 안쪽에 생성.

        initBox.localPosition = Vector3.zero; // 뒤틀리는거 방지
        initBox.localScale = new Vector3(1, 1, 1);

        // 순위표 달아주기
        initBox.GetChild(0).GetChild(0).gameObject.SetActive(false);
        initBox.GetChild(0).GetChild(1).gameObject.SetActive(false);
        initBox.GetChild(0).GetChild(2).gameObject.SetActive(false);
        initBox.GetChild(0).GetChild(3).gameObject.SetActive(false);

        if (rank_index == 0)
        {
            initBox.GetChild(0).GetChild(rank_index).gameObject.SetActive(true);
        }
        else if (rank_index == 1)
        {
            initBox.GetChild(0).GetChild(rank_index).gameObject.SetActive(true);
        }
        else if (rank_index == 2)
        {
            initBox.GetChild(0).GetChild(rank_index).gameObject.SetActive(true);
        }
        else
        {
            initBox.GetChild(0).GetChild(3).gameObject.SetActive(true);
            initBox.GetChild(0).GetChild(3).GetChild(0).GetComponent<Text>().text = (rank_index + 1).ToString();
        }

        initBox.GetChild(1).GetComponent<Text>().text = name;
        initBox.GetChild(2).GetComponent<Text>().text = score;


    }





    /* PVP 매니저 */
    /* PVP 매니저 */
    /* PVP 매니저 */
    /* PVP 매니저 */
    /* PVP 매니저 */

    [Header("- 321 카운터 패널")]
    public GameObject PreparedPanel;
    public Text CountText; // 321 텍스트
    public Text TimerFillamount; // 피비피 시간 텍스트
    [Header("- 애니메이션 패널")]
    public GameObject AniPanel;



    /// <summary>
    /// 카메라 전환에서 호출
    /// 3.2.1. 해주고 그다음
    /// 게임 스타트 
    /// </summary>
    public void InitMiniGame()
    {
        PlayerPrefsManager.GetInstance().ticket--;
        PlayerPrefsManager.GetInstance().isPVPtoEnd = false;
        PlayerPrefs.Save();
        isStart = false;
        AniPanel.SetActive(true);
        AniPanel.GetComponent<Animation>()["BlueScroll"].speed = 1;
        AniPanel.GetComponent<Animation>().Play("BlueScroll");
        //
        Invoke("RealStart", 2.0f);
    }



    void RealStart()
    {
        AniPanel.GetComponent<Animation>()["BlueScroll"].speed = 1;
        AniPanel.SetActive(false);

        // 카운트 다운 온!
        PreparedPanel.SetActive(true);
        // 3 2 1카운트 다운
        StartCoroutine(CountTimer());
    }



    bool isStart;
    //
    double MY_PunchCnt;
    double fBlockedDam;
    double fAttackedDam;

    double Enemy_PunchCnt;
    double eBlockedDam;
    double eAttackedDam;

    /// <summary>
    /// 신규 방어구 pvp 속도증가 퍼센트 증가
    /// </summary>
    float PvP_PunchSpeed;

    /// <summary>
    /// 대기화면 카운트다운
    /// </summary>
    /// <returns></returns>
    IEnumerator CountTimer()
    {
        yield return null;
        TimerFillamount.text = string.Format("{0:f0}", 30f);

        MY_PunchCnt = 0;
        Enemy_PunchCnt = 0;

        MiniGameCo = StartCoroutine(TimerReset());

        CountText.text = "FIGHT!";

        yield return new WaitForSeconds(1f);

        PreparedPanel.SetActive(false);

        /// 주먹 미리 세팅
        PvP_PunchSpeed = PlayerPrefsManager.GetInstance().Pet_PVP_Speed_Lv * 0.05f;

        int punchIndex = PlayerPrefsManager.GetInstance().PunchIndex;
        myWeapon = tapToSpawnLimit.OP[punchIndex].GetComponent<Lean.Pool.LeanGameObjectPool>().Prefab;
        //
        if (_BACK_GND == 525) _BACK_GND = 0;
        punchIndex = _BACK_GND;
        enemyWeapon = tapToSpawnLimit.OP[punchIndex].GetComponent<Lean.Pool.LeanGameObjectPool>().Prefab;

        isStart = true;
        isWinLose = false;

    }

    /// <summary>
    /// 미니게임 지속 시간
    /// </summary>
    float cnt;
    Coroutine MiniGameCo;
    /// <summary>
    /// 미니게임 30초 카운트 초기화.
    /// </summary>
    IEnumerator TimerReset()
    {
        yield return null;

        float Maxcnt = 30f;

        cnt = Maxcnt;

        //TimerFillamount.text = "남은 시간 : " + string.Format("{0:f1}", Maxcnt);

        while (cnt > 0)
        {
            yield return new WaitForSeconds(1f);

            if (isStart)
            {
                cnt -= 1f;

                TimerFillamount.text = string.Format("{0:f0}", cnt);
            }

        }
        Debug.LogWarning("타임 오버");
        StartCoroutine(PVP_GameOver(false));

    }



    [Header("- 전투 결과 페이지")]
    public GameObject resultPanel;

    public Text blockedDam;
    public Text attackedDam;
    public Text AllDam;

    public Text EnemyblockedDam;
    public Text EnemyattackedDam;
    public Text EnemyAllDam;

    public Text TierText;

    void FixedUpdate()
    {
        if (isStart && !PlayerPrefsManager.GetInstance().isPVPtoEnd)
        {
            // 펀치시작
            MY_ClickedPVP_Fight();
            Enemy_ClickedPVP_Fight();
        }

        if (Input.GetMouseButtonDown(0) && isResultPagein)
        {
            isButtonClicked = true;
        }

    }
    // 1초에 카운트 할것.
    private float MY_fUserClickedCnt = 0f;
    private float Enemy_fUserClickedCnt = 0f;
    float tmp = 7f;             /// pvp 주먹속도 기본 값


    /// <summary>
    /// 내 주먹 발싸 시작
    /// </summary>
    public void MY_ClickedPVP_Fight()
    {
        if (MY_fUserClickedCnt != 0f && Time.unscaledTime < MY_fUserClickedCnt) return;

        MY_PVP_Punch();
        MY_ComputeNextPVP();
    }

    /// <summary>
    /// 상대 주먹 발싸 시작
    /// </summary>
    public void Enemy_ClickedPVP_Fight()
    {
        if (Enemy_fUserClickedCnt != 0f && Time.unscaledTime < Enemy_fUserClickedCnt) return;

        Enemy_PVP_Punch();
        Enemy_ComputeNextPVP();
    }


    GameObject bullet_Left;
    GameObject bullet_Right;

    DoubleToStringNum dts = new DoubleToStringNum();

    private void Enemy_ComputeNextPVP()
    {
        Enemy_fUserClickedCnt = Time.unscaledTime + (1f / tmp);
    }

    void MY_ComputeNextPVP()
    {
        MY_fUserClickedCnt = Time.unscaledTime + (1f / (tmp + PvP_PunchSpeed));
    }

    bool isWinLose;

    private void Enemy_PVP_Punch()
    {
        bullet_Left = Lean.Pool.LeanPool.Spawn(enemyWeapon, enemyWeaponPos_Left.transform.position, enemyWeaponPos_Left.transform.rotation);
        bullet_Left.GetComponent<BulletManager>().BulletInit();

        Enemy_PunchCnt += 1.0d;

        myCurrentHP = dts.SubStringDouble(myCurrentHP, enemyPunchDPS);

        if (myCurrentHP == "-1" || myCurrentHP == "0")
        {
            // 그로기 상태 돌입
            myBody.GetComponent<SpriteRenderer>().enabled = false;
            myBody.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            myBody.GetComponent<Animation>().Play("Groggy");

            myCurrentHP = "0";
            myHP.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "0/" + UserWallet.GetInstance().SeetheNatural(double.Parse(myMaxHp));
            myHP.transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
            if (!isWinLose) StartCoroutine(PVP_GameOver(false));
            return;
        }


        myHP.transform.GetChild(0).GetComponent<Image>().fillAmount = (float)dts.DevideStringDouble(myCurrentHP, myMaxHp);
        myHP.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = UserWallet.GetInstance().SeetheNatural(double.Parse(myCurrentHP)) + "/" + UserWallet.GetInstance().SeetheNatural(double.Parse(myMaxHp));

    }

    /// <summary>
    /// 
    /// </summary>
    void MY_PVP_Punch()
    {
        bullet_Right = Lean.Pool.LeanPool.Spawn(myWeapon, myWeaponPos_Right.transform.position, myWeaponPos_Right.transform.rotation);
        bullet_Right.GetComponent<BulletManager>().BulletInit0();

        MY_PunchCnt += 1.0d;

        enemyCurrentHP = dts.SubStringDouble(enemyCurrentHP, myPunchDPS);

        if (enemyCurrentHP == "-1" || enemyCurrentHP == "0")
        {
            // 그로기 상태 돌입
            enemyBody.GetComponent<SpriteRenderer>().enabled = false;
            enemyBody.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            enemyBody.GetComponent<Animation>().Play("Groggy");

            enemyCurrentHP = "0";
            enemyHP.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "0/" + UserWallet.GetInstance().SeetheNatural(double.Parse(enemyMaxHp));
            enemyHP.transform.GetChild(0).GetComponent<Image>().fillAmount = 0;

            if (!isWinLose) StartCoroutine(PVP_GameOver(true));
            return;
        }

        enemyHP.transform.GetChild(0).GetComponent<Image>().fillAmount = (float)dts.DevideStringDouble(enemyCurrentHP, enemyMaxHp);
        enemyHP.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = UserWallet.GetInstance().SeetheNatural(double.Parse(enemyCurrentHP)) + "/" + UserWallet.GetInstance().SeetheNatural(double.Parse(enemyMaxHp));

    }

    string[] tmpString = new string[7];
    bool isButtonClicked;
    bool isResultPagein;
    /// <summary>
    /// 시간 다 되거나 / 승리 / 패배했을 경우
    /// </summary>
    IEnumerator PVP_GameOver(bool _isVictory)
    {
        isWinLose = true;
        yield return null;

        // 펀치 그만
        PlayerPrefsManager.GetInstance().isPVPtoEnd = true;

        blockedDam.text = "";
        attackedDam.text = "";
        AllDam.text = "";

        EnemyblockedDam.text = "";
        EnemyattackedDam.text = "";
        EnemyAllDam.text = "";


        TierText.text = "";


        //타이머 정지.
        StopCoroutine(MiniGameCo);

        // 펀치 그만
        PlayerPrefsManager.GetInstance().isPVPtoEnd = true;

        for (int i = 0; i < resultPanel.transform.childCount; i++)
        {
            resultPanel.transform.GetChild(i).gameObject.SetActive(false);
        }

        resultPanel.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        myBody.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        enemyBody.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

        // 결과 저장
        tmpString[0] = "막은 대미지       " + UserWallet.GetInstance().SeetheNatural(fBlockedDam * MY_PunchCnt);
        tmpString[1] = "입힌 대미지      " + UserWallet.GetInstance().SeetheNatural(fAttackedDam * MY_PunchCnt);
        tmpString[2] = "최종 대미지       " + UserWallet.GetInstance().SeetheNatural((fBlockedDam * MY_PunchCnt) + (fAttackedDam * MY_PunchCnt));
        /// 에너미
        tmpString[4] = "막은 대미지       " + UserWallet.GetInstance().SeetheNatural(eBlockedDam * Enemy_PunchCnt);
        tmpString[5] = "입힌 대미지      " + UserWallet.GetInstance().SeetheNatural(eAttackedDam * Enemy_PunchCnt);
        tmpString[6] = "최종 대미지       " + UserWallet.GetInstance().SeetheNatural((eBlockedDam * Enemy_PunchCnt) + (eAttackedDam * Enemy_PunchCnt));

        Debug.LogWarning("보상 받고 종료");

        tmm.ExUpdateMission(33); /// 미션 업데이트
        tmm.ExUpdateMission(54); /// 미션 업데이트
        tmm.ExUpdateMission(69); /// 미션 업데이트
        tmm.ExUpdateMission(78); /// 미션 업데이트
        tmm.ExUpdateMission(88); /// 미션 업데이트


        /// 승리하면 3점
        if (_isVictory)
        {
            resultPanel.transform.GetChild(5).gameObject.SetActive(true);

            myTierScore += 3;
            //TierText.text = myTierScore + "(+3)";
            tmpString[3] = myTierScore + "(+3)";

            PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 5);
            UserWallet.GetInstance().ShowUserDia();

            //GameObject.Find("PlayNanoo").GetComponent<PlayNANOOExample>().PostboxItemSend("diamond", 5, "PvP 승리 보상");
            GameObject.Find("PlayNanoo").GetComponent<PlayNANOOExample>().WriteChikenCoupon("PVP_VICTORY", $"나의 1대 공격력 : {fAttackedDam}");


        }
        /// 패배하면 -2
        else
        {
            resultPanel.transform.GetChild(6).gameObject.SetActive(true);

            myTierScore -= 2;
            //TierText.text = myTierScore + "(-2)";
            tmpString[3] = myTierScore + "(-2)";
        }
        yield return new WaitForSeconds(1f);
        isResultPagein = true;
        resultPanel.transform.GetChild(5).gameObject.SetActive(false);
        resultPanel.transform.GetChild(6).gameObject.SetActive(false);
        resultPanel.transform.GetChild(0).gameObject.SetActive(true);
        resultPanel.transform.GetChild(1).gameObject.SetActive(true);
        resultPanel.transform.GetChild(2).gameObject.SetActive(true);
        //resultPanel.transform.GetChild(3).gameObject.SetActive(true);
        //resultPanel.transform.GetChild(4).gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        if (isButtonClicked)
        {
            blockedDam.text = tmpString[0];
            attackedDam.text = tmpString[1];
            AllDam.text = tmpString[2];
            TierText.text = tmpString[3];
            //
            EnemyblockedDam.text = tmpString[4];
            EnemyattackedDam.text = tmpString[5];
            EnemyAllDam.text = tmpString[6];
            //
            goto HELL;
        }

        //// 결과 창 띄워랑
        string writerText = "";
        string narration = tmpString[0];
        //텍스트 타이핑 효과
        for (int a = 0; a < narration.Length; a++)
        {
            writerText += narration[a];
            blockedDam.text = writerText;
            yield return null;
        }
        writerText = "";
        narration = tmpString[4];
        //텍스트 타이핑 효과
        for (int a = 0; a < narration.Length; a++)
        {
            writerText += narration[a];
            EnemyblockedDam.text = writerText;
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        if (isButtonClicked)
        {
            blockedDam.text = tmpString[0];
            attackedDam.text = tmpString[1];
            AllDam.text = tmpString[2];
            TierText.text = tmpString[3];
            //
            EnemyblockedDam.text = tmpString[4];
            EnemyattackedDam.text = tmpString[5];
            EnemyAllDam.text = tmpString[6];
            //
            goto HELL;
        }

        writerText = "";
        narration = tmpString[1];
        for (int a = 0; a < narration.Length; a++)
        {
            writerText += narration[a];
            attackedDam.text = writerText;
            yield return null;
        }
        writerText = "";
        narration = tmpString[5];
        //텍스트 타이핑 효과
        for (int a = 0; a < narration.Length; a++)
        {
            writerText += narration[a];
            EnemyattackedDam.text = writerText;
            yield return null;
        }


        yield return new WaitForSeconds(0.3f);

        if (isButtonClicked)
        {
            blockedDam.text = tmpString[0];
            attackedDam.text = tmpString[1];
            AllDam.text = tmpString[2];
            TierText.text = tmpString[3];
            //
            EnemyblockedDam.text = tmpString[4];
            EnemyattackedDam.text = tmpString[5];
            EnemyAllDam.text = tmpString[6];
            //
            goto HELL;
        }

        writerText = "";
        narration = tmpString[2];
        for (int a = 0; a < narration.Length; a++)
        {
            writerText += narration[a];
            AllDam.text = writerText;
            yield return null;
        }
        writerText = "";
        narration = tmpString[6];
        //텍스트 타이핑 효과
        for (int a = 0; a < narration.Length; a++)
        {
            writerText += narration[a];
            EnemyAllDam.text = writerText;
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        if (isButtonClicked)
        {
            blockedDam.text = tmpString[0];
            attackedDam.text = tmpString[1];
            AllDam.text = tmpString[2];
            TierText.text = tmpString[3];
            //
            EnemyblockedDam.text = tmpString[4];
            EnemyattackedDam.text = tmpString[5];
            EnemyAllDam.text = tmpString[6];
            //
            goto HELL;
        }
        writerText = "";
        narration = tmpString[3];
        for (int a = 0; a < narration.Length; a++)
        {
            writerText += narration[a];
            TierText.text = writerText;
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        if (isButtonClicked)
        {
            blockedDam.text = tmpString[0];
            attackedDam.text = tmpString[1];
            AllDam.text = tmpString[2];
            TierText.text = tmpString[3];
            //
            EnemyblockedDam.text = tmpString[4];
            EnemyattackedDam.text = tmpString[5];
            EnemyAllDam.text = tmpString[6];
            //
            goto HELL;
        }

    HELL:
        //이번 시즌 결투장 이용했다
        PlayerPrefsManager.GetInstance().isFirstPVP = true;
        // 탈주 안했다.
        isTalZoo = false;
        // 퀘스트
        if (PlayerPrefsManager.GetInstance().questInfo6[0].All_PVPGame < 1000)
        {
            PlayerPrefsManager.GetInstance().questInfo6[0].All_PVPGame++;
        }


        SubmitScore(myTierScore);

        if (_isVictory) resultPanel.transform.GetChild(3).gameObject.SetActive(true);

        yield return new WaitForSeconds(0.3f);
        resultPanel.transform.GetChild(4).gameObject.SetActive(true);


        isStart = false;
        isButtonClicked = false;
        isResultPagein = false;
    }


    /// <summary>
    /// 그로기 모드 풀어줌 확인 버튼 누를때
    /// </summary>
    public void RevovGrogy()
    {
        myBody.GetComponent<SpriteRenderer>().enabled = true;
        myBody.GetComponent<Animation>().Stop();
        //
        myBody.transform.GetChild(1).gameObject.SetActive(false);
        myBody.transform.GetChild(2).gameObject.SetActive(false);

        enemyBody.GetComponent<SpriteRenderer>().enabled = true;
        enemyBody.GetComponent<Animation>().Stop();
        //
        enemyBody.transform.GetChild(1).gameObject.SetActive(false);
        enemyBody.transform.GetChild(2).gameObject.SetActive(false);
    }




}