using EasyMobile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ConfigManager : MonoBehaviour
{
    public static ConfigManager Instance;

    // App-specific metadata
    public string APP_NAME = "[YOUR_APP_NAME]";

    public string APPSTORE_ID = "[YOUR_APPSTORE_ID]";
    // App Store id

    public string BUNDLE_ID = "[YOUR_BUNDLE_ID]";
    // app bundle id

    [HideInInspector]
    public string APPSTORE_LINK = "itms-apps://itunes.apple.com/app/id";
    // App Store link

    [HideInInspector]
    public string PLAYSTORE_LINK = "market://details?id=";
    // Google Play store link

    [HideInInspector]
    public string APPSTORE_SHARE_LINK = "https://itunes.apple.com/app/id";
    // App Store link

    [HideInInspector]
    public string PLAYSTORE_SHARE_LINK = "https://play.google.com/store/apps/details?id=";
    // Google Play store link

    // Publisher links
    public string APPSTORE_HOMEPAGE = "[YOUR_APPSTORE_PUBLISHER_LINK]";
    // e.g itms-apps://itunes.apple.com/artist/[publisher-name]/[publisher-id]

    public string PLAYSTORE_HOMEPAGE = "[YOUR_GOOGLEPLAY_PUBLISHER_NAME]";
    // e.g https://play.google.com/store/apps/developer?id=[PUBLISHER_NAME]

    public string FACEBOOK_ID = "[YOUR_FACEBOOK_PAGE_ID]";

    public string SUPPORT_EMAIL = "[YOUR_SUPPORT_EMAIL]";

    public string HELP_LINK = "[도움말페이지]";

    [HideInInspector]
    public string FACEBOOK_LINK = "https://facebook.com/";

    [Header("-이용약관 및 개인방침")]
    public string E_DRAGON = "https://docs.google.com/document/d/14v9oRf-erPSEQ5k0qd0IFK8pbc0CTGZ2WHoFhVulTYM";
    public string DOG_DRAGON = "https://docs.google.com/document/d/1ed5Yb8HihIglOKNJHNBR_on-ZHSOdyiHXYFt8QuQMF0";

    void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        APPSTORE_LINK += APPSTORE_ID;
        PLAYSTORE_LINK += BUNDLE_ID;
        APPSTORE_SHARE_LINK += APPSTORE_ID;
        PLAYSTORE_SHARE_LINK += BUNDLE_ID;
        FACEBOOK_LINK += FACEBOOK_ID;
    }

    /// <summary>
    /// 이용약관
    /// </summary>
    public void OpenE_Dragon()
    {
        Application.OpenURL(ConfigManager.Instance.E_DRAGON);
    }

    /// <summary>
    /// 이용약관
    /// </summary>
    public void OpenDOG_DRAGON()
    {
        Application.OpenURL(ConfigManager.Instance.DOG_DRAGON);
    }




    /// <summary>
    /// 525랩 메인 페이지 띄우기
    /// </summary>
    public void ShowMoreGames()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.IPhonePlayer:
                Application.OpenURL(ConfigManager.Instance.APPSTORE_HOMEPAGE);
                break;

            case RuntimePlatform.Android:
                Application.OpenURL(ConfigManager.Instance.PLAYSTORE_HOMEPAGE);
                break;
        }
    }

    /// <summary>
    /// 페이스북 링크
    /// </summary>
    public void OpenFacebookPage()
    {
        Application.OpenURL(ConfigManager.Instance.FACEBOOK_LINK);
    }


    /// <summary>
    /// 페이스북 링크
    /// </summary>
    public void OpenHelpPage()
    {
        Application.OpenURL(ConfigManager.Instance.HELP_LINK);
    }


    /// <summary>
    /// 아마존 상점 페이지 오픈
    /// </summary>
    public void RateApp()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.IPhonePlayer:
                Application.OpenURL(ConfigManager.Instance.APPSTORE_LINK);
                break;

            case RuntimePlatform.Android:
                Application.OpenURL(ConfigManager.Instance.PLAYSTORE_LINK);
                break;
        }
    }

    /// <summary>
    /// 이메일 연결할 때 이거 쓰고
    /// </summary>
    public void ContactUs()
    {
        string email = ConfigManager.Instance.SUPPORT_EMAIL;
        string subject = EscapeURL(ConfigManager.Instance.APP_NAME + " [" + Application.version + "] Support");
        string body = EscapeURL("");
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    public static string EscapeURL(string url)
    {
        return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
    }





    
    /// <summary>
    /// 따블 뒤로 가기 게임 종료
    /// </summary>
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            NativeQuitPop();
        }
    }

    /// <summary>
    /// 버튼에도 붙이고 업뎃에도 붙이고
    /// </summary>
    public void NativeQuitPop()
    {
        // Show a three button alert and handle its OnComplete event
        NativeUI.AlertPopup alert = NativeUI.ShowTwoButtonAlert(
            "종료",
            "맷집 키우기를 종료할까요?",
            "취소",
            "종료"
            );

        // Subscribe to the event
        if (alert != null)
        {
            alert.OnComplete += OnAlertCompleteHandler;
        }
    }

    // The event handler
    void OnAlertCompleteHandler(int buttonIndex)
    {
        switch (buttonIndex)
        {
            case 0:
                // Button 1 was clicked
                break;
            case 1:
                PlayerPrefs.Save();
                Application.Quit();
                break;
            default:
                break;
        }
    }







}
