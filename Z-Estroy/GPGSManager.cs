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
    public static bool isUserLogin; // 유저 로그인 했냐?


    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        ///디버그 모드면 리턴
        if (lm.debugBtton)
        {
            isUserLogin = true;
            return;
        }
        /// 구글 로그인 시도
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

    public static void SetNickName(string _Name)
    {
        userName = _Name; // 유저 닉네임 저장하라.
    }



    /// <summary>
    ///  구글 로그인 시도
    /// </summary>
    /// <returns></returns>
    private IEnumerator NickUpd()
    {
        yield return new WaitForSeconds(1.0f);

        if (!Social.localUser.authenticated)
        {
            GameServices.Init();
        }

        yield return new WaitForSeconds(2.0f);

        // 랜덤 초기화
        //float RanRotat = UnityEngine.Random.Range(0, 659421);
        int logCnt = 0;

        while (!Social.localUser.authenticated)
        {
            yield return new WaitForSeconds(2.0f);

            if (!Social.localUser.authenticated) GameServices.Init();

            logCnt++;

            if (logCnt >= 3) break;
        }

        /// 3 번 시도 후에도 구글 로그인 안되었으면 임시 번호 부여
        if (!Social.localUser.authenticated)
        {
            //userID = "u-" + RanRotat.ToString();
            //userName = "Geust-" + RanRotat.ToString();
            /// 그리고 구글 로그인 해주세요 팝업 출력
            if(loginPlease != null) 
                loginPlease.SetActive(true);
        }
        /// 구글 로그인 로그인 확인 되면
        else
        {
            userID = Social.localUser.id;
            userName = Social.localUser.userName;
            /// 로그인 플르지 팝업 꺼줌
            if (loginPlease != null)
                loginPlease.SetActive(false);

            // 로그인 됐으면 로딩바 올려준다.
            isUserLogin = true;
        }
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
