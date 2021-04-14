using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagManager : MonoBehaviour
{
    public TutorialMissionManager tmm;

    /// <summary>
    /// BuffFlags[0] = 체력 회복력    is1Recov
    /// BuffFlags[0] = 체력           is2Stamina
    /// BuffFlags[0] = 공격력         is3ATK
    /// BuffFlags[0] = 맷집           is4Mattzip
    /// </summary>
    [Header("-메인 씬 배경에 깃발들")]
    public GameObject[] BuffFlags;

    [Header("- 공격력 % 증가")]
    public Text is3ATK_Lv;        // 레벨 표기
    public Text is3ATK_Desc;        // 성능 표기
    public Text is3ATK_Price;        // 가격 표기
    public GameObject is3ATK_Gray;
    public GameObject is3ATK_Max;

    [Header("- 맷집 % 증가")]
    public Text is4Mattzip_Lv;        // 레벨 표기
    public Text is4Mattzip_Desc;        // 성능 표기
    public Text is4Mattzip_Price;        // 가격 표기
    public GameObject is4Mattzip_Gray;
    public GameObject is4Mattzip_Max;

    [Header("- 체력 % 증가")]
    public Text is2Stamina_Lv;        // 레벨 표기
    public Text is2Stamina_Desc;        // 성능 표기
    public Text is2Stamina_Price;        // 가격 표기
    public GameObject is2Stamina_Gray;
    public GameObject is2Stamina_Max;

    [Header("- 체력 회복력 % 증가")]
    public Text is1Recov_Lv;        // 레벨 표기
    public Text is1Recov_Desc;        // 성능 표기
    public Text is1Recov_Price;        // 가격 표기
    public GameObject is1Recov_Gray;
    public GameObject is1Recov_Max;

    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            BuffFlags[i].SetActive(false);
        }
    }

    DoubleToStringNum dts = new DoubleToStringNum();




    /// <summary>
    /// 깃발 레벨업시 쌀밥 소모 공식
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    double GetPerUpPrice(int index, int lv)
    {
        double _defalt = 0;

        switch (index)
        {
            case 0: // 공격력  강화.
                if (lv != 0)
                {
                    _defalt = 5d * Math.Pow(1.03d, lv);
                }
                else _defalt = 5d;
                break;

            case 1: // 체력  강화.
                if (lv != 0)
                {
                    _defalt = 13d * Math.Pow(1.05d, lv);
                }
                else _defalt = 13d;
                break;

            case 2: // 맷집 강화
                if (lv != 0)
                {
                    _defalt = 25d * Math.Pow(1.07d, lv);
                }
                else _defalt = 25d;
                break;

            case 3: // 체력 회복
                if (lv != 0)
                {
                    _defalt = 6d * Math.Pow(1.04d, lv);
                }
                else _defalt = 6d;
                break;

        }

        return Math.Round(_defalt);
    }


    /// <summary>
    /// 기본 플래그 세팅
    /// 구매 여부 받아와서 뿌려줌
    /// !!!! 게임 시작 할 때도 한번 불러와라.
    /// 
    /// </summary>
    public void InitFlags()
    {
        var ppm = PlayerPrefsManager.GetInstance();

        // 구매 한 만큼 켜줌. (얘네들들이 레벨이다.)
        if (ppm.is1Recov > 0) BuffFlags[0].SetActive(true);
        if (ppm.is2Stamina > 0) BuffFlags[1].SetActive(true);
        if (ppm.is3ATK > 0) BuffFlags[2].SetActive(true);
        if (ppm.is4Mattzip > 0) BuffFlags[3].SetActive(true);

        if (ppm.is1Recov >= 9999) ppm.is1Recov = 9999;
        if (ppm.is2Stamina >= 9999) ppm.is2Stamina = 9999;
        if (ppm.is3ATK >= 9999) ppm.is3ATK = 9999;
        if (ppm.is4Mattzip >= 9999) ppm.is4Mattzip = 9999;

        is3ATK_Lv.text = "Lv. " + ppm.is3ATK;
        is4Mattzip_Lv.text = "Lv. " + ppm.is4Mattzip;
        is2Stamina_Lv.text = "Lv. " + ppm.is2Stamina;
        is1Recov_Lv.text = "Lv. " + ppm.is1Recov;


        double currentAtk = 0;

        float tmpATK = ppm.is3ATK * 0.25f;
        // 플로트 파서
        PlayerPrefsManager.GetInstance().Stat_is3ATK = tmpATK;
        is3ATK_Desc.text = "공격력 " + string.Format("{0:F2}", tmpATK) + "% 증가";
        /// 소비 골드 표기
        currentAtk = GetPerUpPrice(0, ppm.is3ATK);
        is3ATK_Price.text = UserWallet.GetInstance().SeetheNatural(currentAtk);

        //===========================================================================================

        tmpATK = (ppm.is4Mattzip * 0.1f);
        // 플로트 파서
        PlayerPrefsManager.GetInstance().Stat_is4Deffence = tmpATK;
        is4Mattzip_Desc.text = "방어력 " + string.Format("{0:F2}", tmpATK) + "% 증가";
        /// 소비 골드 표기
        currentAtk = GetPerUpPrice(2, ppm.is4Mattzip);
        is4Mattzip_Price.text = UserWallet.GetInstance().SeetheNatural(currentAtk);

        //===========================================================================================

        tmpATK = (ppm.is2Stamina * 0.5f);
        // 플로트 파서
        PlayerPrefsManager.GetInstance().Stat_is2Stamina = tmpATK;
        is2Stamina_Desc.text = "체력 " + string.Format("{0:F2}", tmpATK) + "% 증가";
        /// 소비 골드 표기
        currentAtk = GetPerUpPrice(1, ppm.is2Stamina);
        is2Stamina_Price.text = UserWallet.GetInstance().SeetheNatural(currentAtk);


        //===========================================================================================

        tmpATK = (ppm.is1Recov * 0.3f);
        // 플로트 파서
        PlayerPrefsManager.GetInstance().Stat_is1Recov = tmpATK;
        is1Recov_Desc.text = "체력 회복 " + string.Format("{0:F2}", tmpATK) + "% 증가";
        /// 소비 골드 표기
        currentAtk = GetPerUpPrice(3, ppm.is1Recov);
        is1Recov_Price.text = UserWallet.GetInstance().SeetheNatural(currentAtk);


        is2Stamina_Cheak();
        is3ATK_Cheak();
        is1Recov_Cheak();
        is4Mattzip_Cheak();

        UserWallet.GetInstance().ShowAllMoney();

        PlayerPrefs.Save();
    }

    bool isBtnDown1;
    bool isBtnDown2;
    bool isBtnDown3;
    bool isBtnDown4;

    /// <summary>
    /// 계속 누르면 강화.
    /// </summary>
    private void FixedUpdate()
    {
        if (isBtnDown2) TEST_isBtnDown2();
        if (isBtnDown3) TEST_isBtnDown3();
        if (isBtnDown4) TEST_isBtnDown4();
        if (isBtnDown1) TEST_isBtnDown1();

    }

    /// <summary>
    /// 소지금 검사
    /// </summary>
    string GoldPass;

    /// <summary>
    /// 돈이 안되면 회색으로.
    /// </summary>
    bool is2Stamina_Cheak()
    {
        // 현재 공격력 레벨
        int is2Stamina = PlayerPrefsManager.GetInstance().is2Stamina;

        if (is2Stamina >= 9999)
        {
            is2Stamina_Max.SetActive(true);
            return false;
        }

        // 다음 레벨의 가격 불러오고.
        string nextPrice = GetPerUpPrice(1, is2Stamina).ToString("f0");
        Debug.LogWarning("ssalbap : " + PlayerPrefsManager.GetInstance().ssalbap);

        GoldPass = dts.SubStringDouble(PlayerPrefsManager.GetInstance().ssalbap, nextPrice);

        Debug.LogWarning("GoldPass " + GoldPass);
        Debug.LogWarning("nextPrice " + nextPrice);

        // 골드 없으면 false
        if (GoldPass == "-1")
        {
            is2Stamina_Gray.SetActive(true);
            return false;
        }
        else // 구매 가능하면 트루
        {
            is2Stamina_Gray.SetActive(false);
            return true;
        }

    }

    /// <summary>
    ///  체력 증가
    /// </summary>
    public void TEST_isBtnDown2()
    {
        if (!is2Stamina_Cheak()) return;

        //쌀밥 감소 처리
        PlayerPrefsManager.GetInstance().ssalbap = GoldPass;
        UserWallet.GetInstance().ShowUserSSalbap();
        PlayerPrefs.Save();

        // 레벨 상승
        PlayerPrefsManager.GetInstance().is2Stamina++;

        tmm.ExUpdateMission(18); /// 미션 업데이트
        tmm.ExUpdateMission(19); /// 미션 업데이트

        InitFlags();
    }


    /// <summary>
    /// 돈이 안되면 회색으로.
    /// </summary>
    bool is3ATK_Cheak()
    {
        // 현재 공격력 레벨
        int is3ATK = PlayerPrefsManager.GetInstance().is3ATK;

        if (is3ATK >= 9999)
        {
            is3ATK_Max.SetActive(true);
            return false;
        }

        // 다음 레벨의 가격 불러오고.
        string nextPrice = GetPerUpPrice(0, is3ATK).ToString("f0");

        GoldPass = dts.SubStringDouble(PlayerPrefsManager.GetInstance().ssalbap, dts.fDoubleToStringNumber(nextPrice));

        // 골드 없으면 false
        if (GoldPass == "-1")
        {
            is3ATK_Gray.SetActive(true);
            return false;
        }
        else // 구매 가능하면 트루
        {
            is3ATK_Gray.SetActive(false);
            return true;
        }

    }

    /// <summary>
    ///  공격력 증가
    /// </summary>
    public void TEST_isBtnDown3()
    {
        if (!is3ATK_Cheak()) return;

        //쌀밥 감소 처리
        PlayerPrefsManager.GetInstance().ssalbap = GoldPass;
        UserWallet.GetInstance().ShowUserSSalbap();
        PlayerPrefs.Save();

        // 레벨 상승
        PlayerPrefsManager.GetInstance().is3ATK++;

        tmm.ExUpdateMission(18); /// 미션 업데이트
        tmm.ExUpdateMission(19); /// 미션 업데이트

        InitFlags();
    }



    /// <summary>
    /// 돈이 안되면 회색으로.
    /// </summary>
    bool is4Mattzip_Cheak()
    {
        // 현재 공격력 레벨
        int is4Mattzip = PlayerPrefsManager.GetInstance().is4Mattzip;

        if (is4Mattzip >= 9999)
        {
            is4Mattzip_Max.SetActive(true);
            return false;
        }

        // 다음 레벨의 가격 불러오고.
        string nextPrice = GetPerUpPrice(2, is4Mattzip).ToString("f0");

        GoldPass = dts.SubStringDouble(PlayerPrefsManager.GetInstance().ssalbap, dts.fDoubleToStringNumber(nextPrice));

        // 골드 없으면 false
        if (GoldPass == "-1")
        {
            is4Mattzip_Gray.SetActive(true);
            return false;
        }
        else // 구매 가능하면 트루
        {
            is4Mattzip_Gray.SetActive(false);
            return true;
        }

    }

    /// <summary>
    ///  맷집 증가
    /// </summary>
    public void TEST_isBtnDown4()
    {
        if (!is4Mattzip_Cheak()) return;

        //쌀밥 감소 처리
        PlayerPrefsManager.GetInstance().ssalbap = GoldPass;
        UserWallet.GetInstance().ShowUserSSalbap();
        PlayerPrefs.Save();

        // 레벨 상승
        PlayerPrefsManager.GetInstance().is4Mattzip++;

        tmm.ExUpdateMission(18); /// 미션 업데이트
        tmm.ExUpdateMission(19); /// 미션 업데이트

        InitFlags();
    }




    /// <summary>
    /// 돈이 안되면 회색으로.
    /// </summary>
    bool is1Recov_Cheak()
    {
        // 현재 공격력 레벨
        int is1Recov = PlayerPrefsManager.GetInstance().is1Recov;

        if (is1Recov >= 9999)
        {
            is1Recov_Max.SetActive(true);
            return false;
        }

        // 다음 레벨의 가격 불러오고.
        string nextPrice = GetPerUpPrice(3, is1Recov).ToString("f0");

        GoldPass = dts.SubStringDouble(PlayerPrefsManager.GetInstance().ssalbap, dts.fDoubleToStringNumber(nextPrice));

        // 골드 없으면 false
        if (GoldPass == "-1")
        {
            is1Recov_Gray.SetActive(true);
            return false;
        }
        else // 구매 가능하면 트루
        {
            is1Recov_Gray.SetActive(false);
            return true;
        }

    }

    /// <summary>
    ///  체력 회복력 증가
    /// </summary>
    public void TEST_isBtnDown1()
    {
        if (!is1Recov_Cheak()) return;

        //쌀밥 감소 처리
        PlayerPrefsManager.GetInstance().ssalbap = GoldPass;
        UserWallet.GetInstance().ShowUserSSalbap();
        PlayerPrefs.Save();

        // 레벨 상승
        PlayerPrefsManager.GetInstance().is1Recov++;

        tmm.ExUpdateMission(18); /// 미션 업데이트
        tmm.ExUpdateMission(19); /// 미션 업데이트

        InitFlags();
    }









    public void BtnDown1()
    {
        Invoke("InvoDown1", 0.3f);
    }
    void InvoDown1()
    {
        isBtnDown1 = true;
    }
    public void BtnUP1()
    {
        CancelInvoke("InvoDown1");
        isBtnDown1 = false;
    }
    /// <summary>
    /// BtnDown2()
    /// </summary>
    public void BtnDown2()
    {
        Invoke("InvoDown2", 0.3f);
    }
    void InvoDown2()
    {
        isBtnDown2 = true;
    }
    public void BtnUP2()
    {
        CancelInvoke("InvoDown2");
        isBtnDown2 = false;
    }

    /// <summary>
    /// BtnUP3()
    /// </summary>
    public void BtnDown3()
    {
        Invoke("InvoDown3", 0.3f);
    }
    void InvoDown3()
    {
        isBtnDown3 = true;
    }
    public void BtnUP3()
    {
        CancelInvoke("InvoDown3");
        isBtnDown3 = false;
    }

    /// <summary>
    /// BtnDown4()
    /// </summary>
    public void BtnDown4()
    {
        Invoke("InvoDown4", 0.3f);
    }
    void InvoDown4()
    {
        isBtnDown4 = true;
    }
    public void BtnUP4()
    {
        CancelInvoke("InvoDown4");
        isBtnDown4 = false;
    }


















}
