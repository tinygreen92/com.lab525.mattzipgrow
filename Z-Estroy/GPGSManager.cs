using EasyMobile;
using System.Collections;
using UnityEngine.SocialPlatforms;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GPGSManager : MonoBehaviour
{
    public LoadingM lm;
    [Header("- 경고 텍스트")]
    public GameObject loginPlease;


    private static string userID = "UnknownUser"; // 유저 ID 저장하라.
    private static string userName = "UnknownUser"; // 유저 닉네임 저장하라.
    private static bool isUserLogin; // 유저 로그인 했냐?


    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        ///디버그 모드면 리턴
        if (lm.debugBtton)
        {
            return;
        }

        StartCoroutine(NickUpd());
    }

    public static string GetLocalUserId()
    {
        //int RanRotat = UnityEngine.Random.Range(0, 659421);
        //userID = userID + RanRotat;

        return userID;
    }

    public static string GetLocalUserName()
    {
        //int RanRotat = UnityEngine.Random.Range(0, 659421);
        //userName = userName + RanRotat;
        Debug.LogError("userName : " + userName);
        return userName;
    }

    public static bool GPGS_Progress()
    {
        return isUserLogin;
    }

    public static void SetUserDatata(string _ID, string _Name)
    {
        userID = _ID; // 유저 ID 저장하라.
        userName = _Name; // 유저 닉네임 저장하라.
    }



    // 구글 로그인 되면 바로 닉네임 설정하게
    private IEnumerator NickUpd()
    {
        yield return new WaitForSeconds(1.0f);

        GameServices.Init();

        yield return new WaitForSeconds(2.0f);

        // 랜덤 초기화
        float RanRotat = UnityEngine.Random.Range(0, 659421);
        int logCnt = 0;

        while (!Social.localUser.authenticated)
        {
            yield return new WaitForSeconds(2.0f);

            if (!Social.localUser.authenticated) GameServices.Init();

            logCnt++;

            if (logCnt >= 3) break;
        }

        /// 3 번 시도 후에도 구글 로그인 안되었으면
        if (!Social.localUser.authenticated)
        {
            userID = "u-" + RanRotat.ToString();
            userName = "Geust-" + RanRotat.ToString();

            if(loginPlease != null) loginPlease.SetActive(true);

        }

        while (Social.localUser.authenticated)
        {
            yield return new WaitForSeconds(2.0f);

            userID = Social.localUser.id;
            userName = Social.localUser.userName;

            if (loginPlease != null) loginPlease.SetActive(false);

            // 로그인 됐으면 로딩바 올려준다.
            isUserLogin = true;

            break;
        }

        //게임 세이브 오픈
        //InitGameSave();

    }


    public void ShowLeaderboard()
    {
        // Check for initialization before showing leaderboard UI
        if (GameServices.IsInitialized())
        {
            GameServices.ShowLeaderboardUI();

            //Debug.LogWarning("아이디 :: " + Social.localUser.id);
        }
        else
        {
#if UNITY_ANDROID
            GameServices.Init();    // start a new initialization process
#elif UNITY_IOS
                Debug.Log("Cannot show leaderboard UI: The user is not logged in to Game Center.");
#endif
        }
    }

    //public void ReportScore(long _score)
    //{
    //    GameServices.ReportScore(_score, EM_GameServicesConstants.Leaderboard_leaderboard_ranking);
    //    GameServices.LoadLocalUserScore(EM_GameServicesConstants.Leaderboard_leaderboard_ranking, OnLocalUserScoreLoaded);

    //}


    // Score loaded callback
    public void OnLocalUserScoreLoaded(string leaderboardName, IScore score)
    {
        if (score != null)
        {
            Debug.Log("Your score is: " + score.value);
        }
        else
        {
            Debug.Log("You don't have any score reported to leaderboard " + leaderboardName);
        }
    }


    // Users loaded callback
    void OnUsersLoaded(IUserProfile[] users)
    {
        if (users.Length > 0)
        {
            foreach (IUserProfile user in users)
            {
                Debug.Log("User's name: " + user.userName + "; ID: " + user.id);
            }
        }
        else
        {
            Debug.Log("Couldn't find any user with the specified IDs.");
        }
    }






}
