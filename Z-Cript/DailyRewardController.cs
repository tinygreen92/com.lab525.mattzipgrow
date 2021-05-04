using UnityEngine;
using System;
using UnityEngine.UI;

public class DailyRewardController : MonoBehaviour
{

    private DoubleToStringNum dts = new DoubleToStringNum();

    public GameObject go7Popup;
    [Header("획득 아이템 팝업")]
    public GameObject getItemPop;
    public GameObject[] getItemPopImg;
    public Text getItemPopText;

    public GameObject dailyRewardBtn;

    [Header("Button Pannel Child")]
    public GameObject[] btnPanelChild; // 20일치 출석부 오브젝트 붙일거. 0~19
    [Header("출석 버튼")]
    public GameObject grayCheackBtn;

    private int tmpDailyCnt;

    void Start()
    {
        Init();
    }
    
    private void Init()
    {

        //이미 출석을 한 상태에서 게임 껐다 켜면?
        if (PlayerPrefsManager.GetInstance().NewDailyCount == 1)
        {
            // 시작할때 회색 버튼 활성화.
            grayCheackBtn.SetActive(true);
            // 
        }
        else if (PlayerPrefsManager.GetInstance().DailyCount_Cheak >= 25) // 마지막 출석한지 하루 지나면?
        {
            PlayerPrefsManager.GetInstance().DailyCount_Cheak = 0;
        }
        // 출석일 수?
        tmpDailyCnt = PlayerPrefsManager.GetInstance().DailyCount_Cheak;

        Debug.LogError("출석 일수 :: " + tmpDailyCnt);
        /// 버튼 오브젝트에 GO 배열 만들고 ALL OFF
        for (int i = tmpDailyCnt; i < btnPanelChild.Length; i++)
        {
            btnPanelChild[i].transform.GetChild(2).gameObject.SetActive(false); // 출첵완료 이미지
        }



    }

    /// <summary>
    /// 25일 출석 다하면 출석판 초기화.
    /// </summary>
    public void ResetDailyBoard()
    {
        PlayerPrefsManager.GetInstance().DailyCount_Cheak = 0;

        for (int i = 0; i < btnPanelChild.Length; i++)
        {
            btnPanelChild[i].transform.GetChild(2).gameObject.SetActive(false); // 출첵완료 이미지
        }
    }


    public void GrebThisDaily()
    {
        /// 출석창 호출
        go7Popup.SetActive(true);
        go7Popup.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        go7Popup.GetComponent<Animation>().Play("Roll_Incre");
    }

    public void GrabCancelBtn()
    {
        if (PlayerPrefsManager.GetInstance().NewDailyCount == 1)
        {
            go7Popup.SetActive(false);
        }
        else
        {
            GrabDailyReward();
        }
    }

    /// <summary>
    /// 출석 해당 버튼 클릭시 보상 획득 
    /// </summary>
    public void GrabDailyReward()
    {
        if (PlayerPrefsManager.GetInstance().NewDailyCount == 1) return;

        tmpDailyCnt = PlayerPrefsManager.GetInstance().DailyCount_Cheak;

        /// TODO : 보상획득 다이아 / 토파즈 / 엘릭서
        // 출석 했다.
        PlayerPrefsManager.GetInstance().NewDailyCount = 1;

        // 버튼 회색으로 만들고 24시간 카운터 돌리기
        grayCheackBtn.SetActive(true);

        /// 해당 보상 지급
        GetRewordForClick(tmpDailyCnt);

        // 출첵완료 이미지
        btnPanelChild[tmpDailyCnt].transform.GetChild(2).gameObject.SetActive(true);

        // 카운터 하나 올리고 저장
        tmpDailyCnt++;
        PlayerPrefsManager.GetInstance().DailyCount_Cheak = tmpDailyCnt;

        PlayerPrefs.Save();
    }


    /// <summary>
    /// 해당 출첵에 해당하는 보상 지급
    /// </summary>
    /// <param name="clickIndex"></param>
    private void GetRewordForClick(int clickIndex)
    {
        var ppm = PlayerPrefsManager.GetInstance();
        //
        string tmpGupBap = ppm.gupbap;
        string tmpSSal = ppm.ssalbap;
        float tmpDia = PlayerPrefs.GetFloat("dDiamond");

        switch (clickIndex)
        {
            ///getItemPopImg 0 = 국밥 / 1 = 키 / 2 = 다이아 / 3 = 쌀밥
            /////////////////////////////////
            case 0:
                ppm.gupbap = dts.AddStringDouble(tmpGupBap, "500");
                GetRewordImgText(0, "국밥 500그릇을 얻었다!");

                break;

            case 1:
                ppm.key += 5;
                GetRewordImgText(1, "열쇠 5개를 얻었다!");

                break;

            case 2:
                PlayerPrefs.SetFloat("dDiamond", tmpDia + 50);
                GetRewordImgText(2, "다이아 50개를 얻었다!");

                break;

            case 3:
                ppm.ssalbap = dts.AddStringDouble(tmpSSal, "250");
                GetRewordImgText(3, "쌀밥 250그릇을 얻었다!");

                break;

            case 4:
                PlayerPrefs.SetFloat("dDiamond", tmpDia + 100);
                GetRewordImgText(2, "다이아 100개를 얻었다!");

                break;

            case 5:
                ppm.gupbap = dts.AddStringDouble(tmpGupBap, "1000");
                GetRewordImgText(0, "국밥 1000그릇을 얻었다!");

                break;

            case 6:
                ppm.key += 10;
                GetRewordImgText(1, "열쇠 10개를 얻었다!");

                break;

            case 7:
                PlayerPrefs.SetFloat("dDiamond", tmpDia + 50);
                GetRewordImgText(2, "다이아 50개를 얻었다!");

                break;

            case 8:
                ppm.ssalbap = dts.AddStringDouble(tmpSSal, "500");
                GetRewordImgText(3, "쌀밥 500개를 얻었다!");

                break;

            case 9:
                PlayerPrefs.SetFloat("dDiamond", tmpDia + 150);
                GetRewordImgText(2, "다이아 150개를 얻었다!");

                break;


            case 10:
                ppm.gupbap = dts.AddStringDouble(tmpGupBap, "1500");
                GetRewordImgText(0, "국밥 1500그릇을 얻었다!");

                break;

            case 11:
                ppm.key += 15;
                GetRewordImgText(1, "열쇠 15개를 얻었다!");

                break;

            case 12:
                PlayerPrefs.SetFloat("dDiamond", tmpDia + 50);
                GetRewordImgText(2, "다이아 50개를 얻었다!");

                break;

            case 13:
                ppm.ssalbap = dts.AddStringDouble(tmpSSal, "750");
                GetRewordImgText(3, "쌀밥 750개를 얻었다!");

                break;

            case 14:
                PlayerPrefs.SetFloat("dDiamond", tmpDia + 200);
                GetRewordImgText(2, "다이아 200개를 얻었다!");

                break;


            case 15:
                ppm.gupbap = dts.AddStringDouble(tmpGupBap, "2000");
                GetRewordImgText(0, "국밥 2000개를 얻었다!");

                break;

            case 16:
                ppm.key += 20;
                GetRewordImgText(1, "열쇠 20개를 얻었다!");

                break;

            case 17:
                PlayerPrefs.SetFloat("dDiamond", tmpDia + 50);
                GetRewordImgText(2, "다이아 50개를 얻었다!");

                break;

            case 18:
                ppm.ssalbap = dts.AddStringDouble(tmpSSal, "1000");
                GetRewordImgText(3, "쌀밥 1000그릇을 얻었다!");

                break;

            case 19:
                PlayerPrefs.SetFloat("dDiamond", tmpDia + 250);
                GetRewordImgText(2, "다이아 250개를 얻었다!");

                break;

            case 20:
                ppm.gupbap = dts.AddStringDouble(tmpGupBap, "3000");
                GetRewordImgText(0, "국밥 3000그릇을 얻었다!");

                break;

            case 21:
                ppm.key += 30;
                GetRewordImgText(1, "열쇠 30개를 얻었다!");

                break;

            case 22:
                PlayerPrefs.SetFloat("dDiamond", tmpDia + 50);
                GetRewordImgText(2, "다이아 50개를 얻었다!");

                break;

            case 23:
                ppm.ssalbap = dts.AddStringDouble(tmpSSal, "2000");
                GetRewordImgText(3, "쌀밥 2000그릇을 얻었다!");

                break;

            case 24:
                PlayerPrefs.SetFloat("dDiamond", tmpDia + 500);
                GetRewordImgText(2, "다이아 500개를 얻었다!");

                break;




                /////////////////////////////////
        }

        UserWallet.GetInstance().ShowAllMoney();
    }


    /// <summary>
    /// 팝업도 여기에서 띄워준다
    ///getItemPopImg 0 = 국밥 / 1 = 키 / 2 = 다이아 / 3 = 쌀밥
    /// </summary>
    /// <param name="_index">0 = 다이아 , 1 = 토파즈, 2 = 엘릭서</param>
    /// <param name="_str">여기에 하고싶은 말을 적으시오</param>
    private void GetRewordImgText(int _index, string _str)
    {
        getItemPopImg[0].SetActive(false);
        getItemPopImg[1].SetActive(false);
        getItemPopImg[2].SetActive(false);
        getItemPopImg[3].SetActive(false);
        //
        getItemPopImg[_index].SetActive(true);
        //
        getItemPopText.text = _str;

        /// 출석체크 팝업 출력
        getItemPop.SetActive(true);
        getItemPop.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        getItemPop.GetComponent<Animation>().Play("Roll_Incre");
    }


}
