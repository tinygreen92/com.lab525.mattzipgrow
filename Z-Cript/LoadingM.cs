using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Networking;

public class LoadingM : MonoBehaviour
{
    public bool debugBtton;

    // fill 이미지 쓸 경우 넣고.
    public Image loadingBar;
    [Header("100%되면 파칭")]
    public Image loadingComp;
    private bool isClickToStart;
    [Header("업데이트 알람")]
    public GameObject updatePop;

    private string gameVer;
    private string url;

    /// <summary>
    /// 파이어베이스 초기화도 같이 해야한다
    /// </summary>
    private void Start()
    {

        loadingBar.fillAmount = 0;

        ClikMe.GetComponent<Button>().interactable = false;

        gameVer = Application.version;
        url = "https://play.google.com/store/apps/details?id=com.lab525.mattzipgrow";

        /// TODO : 버전 체크
        StartCoroutine(PlayStoreVersionCheck());



    }

    private IEnumerator PlayStoreVersionCheck()
    {
        WWW www = new WWW(url);
        yield return www;

        //인터넷 연결 에러가 없다면, 
        if (www.error == null)
        {
            int index = www.text.IndexOf("<span class=\"htlgb\">" + gameVer);

            /// index 가 -1 이면 버전 다른거임
            if (index.Equals(-1))
            {
                // 팝업 띄운다.
                updatePop.SetActive(true);
                ////버전이 다르므로, 마켓으로 보낸다.
                //OpenURLMyGame();
                //SceneManager.LoadScene("MainScene");
            }
            else
            {
                string versionText = www.text.Substring(index, 30);

                //플레이스토어에 올라간 APK의 버전을 가져온다.
                int softwareVersion = versionText.IndexOf(">");
                string playStoreVersion = versionText.Substring(softwareVersion + 1, Application.version.Length);

                Debug.LogError("gameVer : " + gameVer);
                Debug.LogError("playStoreVersion : " + playStoreVersion);

                //버전이 같다면,
                if (playStoreVersion.Equals(gameVer))
                {
                    //게임 씬으로 넘어간다.
                    StartCoroutine(LoadAsyncScene());
                }
                else
                {
                    // 팝업 띄운다.
                    updatePop.SetActive(true);
                    ////버전이 다르므로, 마켓으로 보낸다.
       
                }
            }


        }
        else
        {
            //인터넷 연결 에러시
            Debug.LogError("연결 에러 " + www.error);
            // 어쩔 수 없지 넘어가
            //게임 씬으로 넘어간다.
            StartCoroutine(LoadAsyncScene());

        }
    }

    public void UpdatePopClicked()
    {
        //게임 씬으로 넘어간다.
        StartCoroutine(LoadAsyncScene());
    }



    IEnumerator LoadAsyncScene()
    {
        yield return null;
        AsyncOperation asyncScene = SceneManager.LoadSceneAsync("SampleScene");
        asyncScene.allowSceneActivation = false;

        float timeC = 0;
        float timeD = 0;

        while (!asyncScene.isDone)
        {
            yield return new WaitForFixedUpdate();

            timeC += 0.01f;
            if (asyncScene.progress >= 0.9f)
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 0.7f, timeC);
                /// 디버그 모드일때 돌리고
                if (debugBtton)
                {
                    StartCoroutine(RealStart(asyncScene));
                    StopCoroutine(LoadAsyncScene());
                    break;
                }
                /// 실제 빌드일때 돌리고
                if (GPGSManager.GPGS_Progress())
                {
                    loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 1f, timeC);

                    if (loadingBar.fillAmount == 1f)
                    {
                        timeD += 0.01f;

                        loadingComp.fillAmount = Mathf.Lerp(loadingComp.fillAmount, 1f, timeD);

                        if (loadingComp.fillAmount == 1f)
                        {
                            StartCoroutine(RealStart(asyncScene));
                            StopCoroutine(LoadAsyncScene());
                        }

                    }
                }
            }
            else
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, asyncScene.progress, timeC);
                if (loadingBar.fillAmount >= asyncScene.progress)
                {
                    timeC = 0f;
                }
            }
        }
    }

    public GameObject ClikMe;
    IEnumerator RealStart(AsyncOperation asyncScene)
    {
        yield return new WaitForSeconds(1);

        var isSignFirst = PlayerPrefs.GetInt("isSignFirst", 0);

        if (isSignFirst != 0)
        {
            ClikMe.SetActive(true);
        }
        else
        {
            SignME.SetActive(true);
        }

        yield return new WaitForSeconds(1);

        while (!isClickToStart)
        {
            yield return new WaitForFixedUpdate();

            ClikMe.SetActive(true);
            ClikMe.GetComponent<Button>().interactable = true;
        }

        asyncScene.allowSceneActivation = true;

    }
    public GameObject SignME;

    /// <summary>
    /// 첫 실행시 동의 클릭하면 다시는 안 뜸
    /// </summary>
    public void TimeToStart()
    {
        PlayerPrefs.SetInt("isSignFirst", 1);
        PlayerPrefs.Save();
        isClickToStart = true;
    }

}