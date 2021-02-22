using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGChanger : MonoBehaviour
{
    [Header("- 캐릭터 창에 보일 이미지")]
    public GameObject[] charaBGmask;

    [Header("- 상점 캔버스")]
    public GameObject[] buyBtn; // lenth = 5 + 1
    public GameObject[] equipBtn; // lenth = 5 + 1 
    public GameObject[] unEquipBtn; // lenth = 5 + 1

    [Header("- 게임 씬")]
    public Sprite[] allImege; // 배경 스프라이트 5 + 1 
    public SpriteRenderer[] backGndObj; // 실제 게임 배경에서 바꿔줄 거


    /// <summary>
    /// 배경 초기 세팅 해준다.
    /// </summary>
    private void Start()
    {
        if (!PlayerPrefs.HasKey("BG_Data") || PlayerPrefs.GetString("BG_Data") == "")
        {
            PlayerPrefsManager.GetInstance().BG_Data = "0+0+0+0+0";
            //Debug.LogWarning("해치웠나?");

        }
        //Debug.LogWarning("님아 : (" + PlayerPrefs.GetString("BG_Data") + ")");

        string[] sDataList = (PlayerPrefsManager.GetInstance().BG_Data).Split('+');

        for (int i = 0; i<sDataList.Length; i++)
        {
            /// 해당 구매 기록이 존재하면
            if (sDataList[i] == "1")
            {
                buyBtn[i].SetActive(false); // 구매버튼 숨겨주고
                equipBtn[i].SetActive(true); // 이큅 버튼 활성화.
            }
        }

        buyBtn[5].SetActive(false); // 구매버튼 숨겨주고
        equipBtn[5].SetActive(true); // 이큅 버튼 활성화.
        unEquipBtn[5].SetActive(true); // 적용 해제 버튼 활성화.

        // 마지막에 착용한거로 배경 세팅.
        SetBG(PlayerPrefs.GetInt("BG_Curent", 525));

        SetBGsTat(PlayerPrefs.GetInt("BG_Curent", 525) - 1);
    }


    /// <summary>
    /// 배경 살때 다이아몬드 소모 로직 넣었을때 통과하냐??
    /// </summary>
    /// <returns></returns>
    bool DiaPassForBG(int _index)
    {
        string result = "";
        int price = 0;

        switch (_index)
        {

            case 525:
                price = 0;
                break;

            case 0:
                price = 500;
                break;

            case 1:
                price = 900;
                break;

            case 2:
                price = 1590;
                break;

            case 3:
                price = 2790;
                break;

            case 4:
                price = 4900;
                break;

        }

        if (PlayerPrefs.GetFloat("dDiamond") - price < 0) result = "-1";

        if (result != "-1") return true;
        else return false;
    }

    /// <summary>
    /// 훈련장 배경 바꾸기할 때 호출
    /// 1. 구매 안했으면 다이아 표기해주고 구매.
    /// </summary>
    public void BG_Purchase(int _index)
    {
        string[] sDataList = (PlayerPrefsManager.GetInstance().BG_Data).Split('+');

        switch (_index)
        {
            case 525:

                break;

            case 0:
                if (DiaPassForBG(_index))
                {
                    /// 구매내역이 있다?
                    if (sDataList[_index] == "1") break;

                    // 구매한적 없다면 다이아 소모
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 500);
                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("성공적으로 구입하셨습니다.");

                    sDataList[_index] = "1";
                }
                else
                {
                    /// 구매내역이 있다?
                    if (sDataList[_index] == "1") break;

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("보유 다이아가 부족합니다.");
                }
                break;

            case 1:
                if (DiaPassForBG(_index))
                {
                    /// 구매내역이 있다?
                    if (sDataList[_index] == "1") break;

                    // 구매한적 없다면 다이아 소모
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 900);
                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("성공적으로 구입하셨습니다.");

                    sDataList[_index] = "1";
                }
                else
                {
                    /// 구매내역이 있다?
                    if (sDataList[_index] == "1") break;

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("보유 다이아가 부족합니다.");
                }
                break;

            case 2:
                if (DiaPassForBG(_index))
                {
                    /// 구매내역이 있다?
                    if (sDataList[_index] == "1") break;

                    // 구매한적 없다면 다이아 소모
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 1590);
                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("성공적으로 구입하셨습니다.");

                    sDataList[_index] = "1";
                }
                else
                {
                    /// 구매내역이 있다?
                    if (sDataList[_index] == "1") break;

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("보유 다이아가 부족합니다.");
                }
                break;

            case 3:
                if (DiaPassForBG(_index))
                {
                    /// 구매내역이 있다?
                    if (sDataList[_index] == "1") break;

                    // 구매한적 없다면 다이아 소모
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 2790);
                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("성공적으로 구입하셨습니다.");

                    sDataList[_index] = "1";
                }
                else
                {
                    /// 구매내역이 있다?
                    if (sDataList[_index] == "1") break;

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("보유 다이아가 부족합니다.");
                }
                break;

            case 4:
                if (DiaPassForBG(_index))
                {
                    /// 구매내역이 있다?
                    if (sDataList[_index] == "1") break;

                    // 구매한적 없다면 다이아 소모
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 4900);
                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("성공적으로 구입하셨습니다.");

                    sDataList[_index] = "1";
                }
                else
                {
                    /// 구매내역이 있다?
                    if (sDataList[_index] == "1") break;

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("보유 다이아가 부족합니다.");
                }
                break;

        }

        UserWallet.GetInstance().ShowAllMoney();
        //
        string result = "";

        result += sDataList[0] + "+";
        result += sDataList[1] + "+";
        result += sDataList[2] + "+";
        result += sDataList[3] + "+";
        result += sDataList[4];

        PlayerPrefsManager.GetInstance().BG_Data = result;
        PlayerPrefs.Save();
    }



    /// <summary>
    /// 훈련장 배경 바꾸기할 때 호출
    /// 2. 구매 했으면 장착 / 해제
    /// </summary>
    public void BG_Epuip(int _index)
    {
        if (_index == 525)
        {
            buyBtn[5].SetActive(false); // 구매버튼 숨겨주고
            equipBtn[5].SetActive(true); // 이큅 버튼 활성화.

            // 배경 장착해줌
            SetBG(525);
            // 실제 스탯 적용
            SetBGsTat(524);
            PlayerPrefsManager.GetInstance().BG_Curent = 525;

            return;
        }


        string[] sDataList = (PlayerPrefsManager.GetInstance().BG_Data).Split('+');

        /// 구매내역이 없으면 접근 못함
        if (sDataList[_index] == "0" || unEquipBtn[_index].activeSelf) return;

        buyBtn[_index].SetActive(false); // 구매버튼 숨겨주고
        equipBtn[_index].SetActive(true); // 이큅 버튼 활성화.

        // 배경 장착해줌
        SetBG(_index + 1);
        // 실제 스탯 적용
        SetBGsTat(_index);



        PlayerPrefsManager.GetInstance().BG_Curent = _index + 1;
    }

    
    /// <summary>
    /// 실제 게임 세계 배경 바꿔준다.
    /// </summary>
    /// <param name="_index"></param>
    void SetBG(int _index)
    {
        // 기본 배경일 경우 예외처리.
        if (_index == 0) return;

        Sprite selectImg = allImege[0];


        for (int i = 0; i < equipBtn.Length; i++)
        {
            unEquipBtn[i].SetActive(false); // 이큅 버튼 초기화.
        }

        if (_index == 525)
        {
            unEquipBtn[5].SetActive(true);

            selectImg = allImege[5];
        }
        else
        {
            unEquipBtn[_index - 1].SetActive(true);

            selectImg = allImege[_index - 1];
        }


        for (int i = 0; i < backGndObj.Length; i++)
        {
            backGndObj[i].sprite = selectImg;
        }
    }


    /// <summary>
    /// 실제 스탯에 영향을 끼쳐 주세요
    /// </summary>
    void SetBGsTat(int _index)
    {
        Debug.LogWarning("메ㅐ소드 안 에 들어옴");


        // 캐릭터 창 배경 초기화.
        for (int i = 0; i < charaBGmask.Length; i++)
        {
            charaBGmask[i].SetActive(false);
        }

        switch (_index)
        {
            case 524:
                // 캐릭터 창 배경 화면 바꿈
                charaBGmask[0].SetActive(true);
                //
                //PlayerPrefsManager.GetInstance().BG_CoinStat = 1f;
                PlayerPrefs.SetFloat("BG_CoinStat", 1f);
                break;

            case 0:
                // 캐릭터 창 배경 화면 바꿈
                charaBGmask[1].SetActive(true);
                //PlayerPrefsManager.GetInstance().BG_CoinStat = 2f;
                PlayerPrefs.SetFloat("BG_CoinStat", 2f);

                break;

            case 1:
                // 캐릭터 창 배경 화면 바꿈
                charaBGmask[2].SetActive(true);
                //PlayerPrefsManager.GetInstance().BG_CoinStat = 3f;
                PlayerPrefs.SetFloat("BG_CoinStat", 3f);

                break;

            case 2:
                // 캐릭터 창 배경 화면 바꿈
                charaBGmask[3].SetActive(true);
                //PlayerPrefsManager.GetInstance().BG_CoinStat = 5f;
                PlayerPrefs.SetFloat("BG_CoinStat", 5f);

                break;

            case 3:
                // 캐릭터 창 배경 화면 바꿈
                charaBGmask[4].SetActive(true);
                //PlayerPrefsManager.GetInstance().BG_CoinStat = 9f;
                PlayerPrefs.SetFloat("BG_CoinStat", 9f);

                break;

            case 4:
                // 캐릭터 창 배경 화면 바꿈
                charaBGmask[5].SetActive(true);
                //PlayerPrefsManager.GetInstance().BG_CoinStat = 17f;
                PlayerPrefs.SetFloat("BG_CoinStat", 17f);

                Debug.LogWarning("적용 안돼? : " + PlayerPrefsManager.GetInstance().BG_CoinStat);
                break;

        }

        PlayerPrefs.Save();

    }


}
