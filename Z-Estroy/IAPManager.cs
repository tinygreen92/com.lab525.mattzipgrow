﻿using EasyMobile;
using Lean.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using UnityEngine.UI;

public class IAPManager : MonoBehaviour
{
    [Header("-패키지 상점 데이터")]
    public TextAsset ta;
    [Space(3f)]
    public PlayNANOOExample playNANOO;
    [Header("-구매성공시 팝업 종료")]
    public GameObject shoppopup;
    [Header("-신규 패키지 상점 팝업")]
    public GameObject SomeThingPop;
    public GameObject vatTextObj;
    [Space]
    public Transform SomeThingTrans;
    public Transform BuyButton;
    public Sprite[] ShopImg;



    //public void TextParser()
    //{
    //    string[] line = ta.text.Substring(0, ta.text.Length).Split('\n');
    //    for (int i = 0; i < line.Length; i++)
    //    {
    //        string[] row = line[i].Split('\t');

    //        AddShieldData(
    //            row[0],                 
    //            int.Parse(row[1]),
    //            int.Parse(row[2]),
    //            int.Parse(row[3]),
    //            int.Parse(row[4]),
    //            int.Parse(row[5]),    
    //            int.Parse(row[6]),    
    //            int.Parse(row[7]),    
    //            int.Parse(row[8])
    //            );
    //    }

    //    //PlayerPrefsManager.GetInstance().CybermanInfo(ta.text);
    //}
    //
    //void AddShieldData(string row_0, int row_1, int row_2, int row_3, int row_4, int row_5, int row_6, int row_7, int row_8)
    //{
    //    pakageInfo.Add(new PakageEntry
    //    {
    //        pakaName = row_0,
    //        pakaPrice = row_1,

    //        getDia = row_2,
    //        getShiled = row_3,
    //        getGuapbap = row_4,
    //        getSsalbap = row_5,
    //        getKimchi = row_6,
    //        getKey = row_7,
    //        getTicket = row_8,
    //    });
    //}

    /// <summary>
    /// 무기 관리
    /// </summary>
    [Serializable]
    public class PakageEntry
    {
        public string pakaName;        
        public int pakaPrice;

        public int getDia;
        public int getShiled;
        public int getGuapbap;
        public int getSsalbap;
        public int getKimchi;
        public int getKey;
        public int getTicket;

    }
    // 만든 클래스를 리스트에 담아서 테이블처럼 사용
    List<PakageEntry> pakageInfo = new List<PakageEntry>();

    /// <summary>
    /// 만든 클래스를 리스트에 담아서 테이블처럼 사용
    /// </summary>
    /// <returns></returns>
    internal PakageEntry GetPakInfo(int thisIndex) => pakageInfo[thisIndex];

    private void Start()
    {
        pakageInfo = PlayerPrefsManager.GetInstance().CybermanInfo(ta.text);
    }




    /// <summary>
    /// 구입하면 아이템 넣어주기
    /// </summary>
    /// <param name="_index"></param>
    void GetMyMoneyItem(int _index)
    {
        /// 회색버튼 활성화.
        if(_index > 6) SomeParent.GetComponent<NewPakageItem>().LockLimitBtn();
        /// 순차 지급
        StartCoroutine(MySpecilThanks(_index));
    }

    IEnumerator MySpecilThanks(int _index)
    {
        yield return null;

        switch (_index)
        {
            case 7:  PlayerPrefs.SetInt("limited_01", 525); break;
            case 8:  PlayerPrefs.SetInt("limited_02", 525); break;
            case 9:  PlayerPrefs.SetInt("limited_03", 525); break;
            case 10: PlayerPrefs.SetInt("limited_04", 525); break;
            case 11: PlayerPrefs.SetInt("limited_05", 525); break;
            case 12: PlayerPrefs.SetInt("limited_06", 525); break;
            case 13: PlayerPrefs.SetInt("limited_07", 525); break;
            default: break;
        }
        yield return null;
        PlayerPrefsManager.GetInstance().ShiledTicket += pakageInfo[_index].getShiled;
        yield return null;
        PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(PlayerPrefsManager.GetInstance().gupbap, (pakageInfo[_index].getGuapbap).ToString("F0"));
        yield return null;
        PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(PlayerPrefsManager.GetInstance().ssalbap, (pakageInfo[_index].getSsalbap).ToString("F0"));
        yield return null;
        PlayerPrefsManager.GetInstance().Kimchi = dts.AddStringDouble(PlayerPrefsManager.GetInstance().Kimchi, (pakageInfo[_index].getKimchi).ToString("F0"));
        yield return null;
        PlayerPrefsManager.GetInstance().key += pakageInfo[_index].getKey;
        yield return null;
        PlayerPrefsManager.GetInstance().ticket += pakageInfo[_index].getTicket;
        yield return null;

        float diaPass = pakageInfo[_index].getDia + PlayerPrefs.GetFloat("dDiamond");
        PlayerPrefs.SetFloat("dDiamond", diaPass);
        yield return null;
        UserWallet.GetInstance().ShowAllMoney();
        yield return null;
        /// 일주월 저장
        PlayerPrefsManager.GetInstance().SaveDayLimitData();
    }




    Transform SomeParent;

    /// <summary>
    /// 5개 탭에서 본 팝업 출력
    /// </summary>
    /// <param name="_index"></param>
    public void BuySomeThing()
    {
        SomeParent = EventSystem.current.currentSelectedGameObject.transform.parent;
        int _index = int.Parse(SomeParent.name);

        /// 회색 버튼이면 클릭 반응없이 리턴 시키기
        if (SomeParent.GetComponent<NewPakageItem>().IsGrayScale()) 
            return;


        /// 본 팝업 아이콘/가격표 세팅 해주고
        for (int i = 0; i < SomeThingTrans.childCount; i++)
        {
            SomeThingTrans.GetChild(i).gameObject.SetActive(false);
            BuyButton.GetChild(i).gameObject.SetActive(false);
        }

        /// 아이콘/가격표 켜주고
        SomeThingTrans.GetChild(_index).gameObject.SetActive(true);
        SomeThingTrans.GetChild(_index).GetChild(0).GetComponent<Text>().text = pakageInfo[_index].pakaName;
        BuyButton.GetChild(_index).gameObject.SetActive(true);

        /// 무료 상품들
        if (pakageInfo[_index].pakaPrice == 0)
        {
            BuyButton.GetChild(_index).GetChild(1).GetComponent<Text>().text = "구매";
            vatTextObj.SetActive(false);
        }
        /// 현금으로 사는 것들
        else if (_index < 14 || _index == 18 || _index == 23 || _index == 28)
        {
            /// 원화 / 달러 세팅
            if (LeanLocalization.CurrentLanguage == "Korean")
            {
                BuyButton.GetChild(_index).GetChild(1).GetComponent<Text>().text = $"₩ {pakageInfo[_index].pakaPrice:N0}";
            }
            else
            {
                System.Globalization.NumberFormatInfo numberFormat = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
                BuyButton.GetChild(_index).GetChild(1).GetComponent<Text>().text = pakageInfo[_index].pakaPrice.ToString("C", numberFormat);
            }
            vatTextObj.SetActive(true);
        }
        /// 다이아로 사는 것들
        else
        {
            BuyButton.GetChild(_index).GetChild(1).GetComponent<Text>().text = "구매";
            vatTextObj.SetActive(false);
        }





        /// 본 팝업 켜줘
        SomeThingPop.SetActive(true);
        // 애니메 재생
        SomeThingPop.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        SomeThingPop.GetComponent<Animation>().Play("Roll_Incre");
    }




    public void GetProductsList()
    {
        //IAPProduct[] products = InAppPurchasing.GetAllIAPProducts();

        //// Print all product names
        //foreach (IAPProduct prod in products)
        //{
        //    Debug.Log("Product name: " + prod.Name);
        //}
    }

    DoubleToStringNum dts = new DoubleToStringNum();

    // Successful purchase handler
    void PurchaseCompletedHandler(IAPProduct product)
    {
        //var ppmDia = PlayerPrefsManager.GetInstance().diamond;


        // Compare product name to the generated name constants to determine which product was bought
        switch (product.Name)
        {


            case EM_IAPConstants.Product_dia100:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("다이아 100 구매 성공");
                //PlayerPrefsManager.GetInstance().diamond = dts.AddStringDouble(ppmDia, "100");
                PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 100);
                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                shoppopup.SetActive(false);

                break;

            case EM_IAPConstants.Product_dia600:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("다이아 600 구매 성공");
                //PlayerPrefsManager.GetInstance().diamond = dts.AddStringDouble(ppmDia, "600");
                PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 600);

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                shoppopup.SetActive(false);


                break;

            case EM_IAPConstants.Product_dia1300:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("다이아 1300 구매 성공");
                //PlayerPrefsManager.GetInstance().diamond = dts.AddStringDouble(ppmDia, "1300");
                PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 1300);

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                shoppopup.SetActive(false);


                break;


            case EM_IAPConstants.Product_dia4200:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("다이아 4200 구매 성공");
                //PlayerPrefsManager.GetInstance().diamond = dts.AddStringDouble(ppmDia, "4200");

                PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 4200);


                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                shoppopup.SetActive(false);
                break;


            case EM_IAPConstants.Product_dia7500:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("다이아 7500 구매 성공");
                //PlayerPrefsManager.GetInstance().diamond = dts.AddStringDouble(ppmDia, "7500");

                PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 7500);


                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                shoppopup.SetActive(false);
                break;

            case EM_IAPConstants.Product_dia16000:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("다이아 16000 구매 성공");
                //PlayerPrefsManager.GetInstance().diamond = dts.AddStringDouble(ppmDia, "16000");


                PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 16000);


                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                shoppopup.SetActive(false);


                break;



            /// -------------------------------------------------------------------


            case EM_IAPConstants.Product_auto:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("자동 공격/상자 구매 성공");

                if (PlayerPrefsManager.GetInstance().VIP == 526) PlayerPrefsManager.GetInstance().VIP = 725;
                else if (PlayerPrefsManager.GetInstance().VIP == 527) PlayerPrefsManager.GetInstance().VIP = 825;
                else if (PlayerPrefsManager.GetInstance().VIP == 528) PlayerPrefsManager.GetInstance().VIP = 825;
                else if (PlayerPrefsManager.GetInstance().VIP == 925) PlayerPrefsManager.GetInstance().VIP = 625;
                else if (PlayerPrefsManager.GetInstance().VIP == 0) PlayerPrefsManager.GetInstance().VIP = 525;


                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                shoppopup.SetActive(false);
                break;

            case EM_IAPConstants.Product_nad:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("광고 제거 구매 성공");

                if (PlayerPrefsManager.GetInstance().VIP == 525) PlayerPrefsManager.GetInstance().VIP = 725;
                else if (PlayerPrefsManager.GetInstance().VIP == 527) PlayerPrefsManager.GetInstance().VIP = 925;
                else if (PlayerPrefsManager.GetInstance().VIP == 528) PlayerPrefsManager.GetInstance().VIP = 925;
                else if (PlayerPrefsManager.GetInstance().VIP == 825) PlayerPrefsManager.GetInstance().VIP = 625;
                else if (PlayerPrefsManager.GetInstance().VIP == 0) PlayerPrefsManager.GetInstance().VIP = 526;

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                shoppopup.SetActive(false);
                break;

            case EM_IAPConstants.Product_mattzipbuff:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("공격력 무한 버프 구매 성공");

                if (PlayerPrefsManager.GetInstance().VIP == 525) PlayerPrefsManager.GetInstance().VIP = 825;
                else if (PlayerPrefsManager.GetInstance().VIP == 526) PlayerPrefsManager.GetInstance().VIP = 925;
                else if (PlayerPrefsManager.GetInstance().VIP == 528) PlayerPrefsManager.GetInstance().VIP = 527528;
                else if (PlayerPrefsManager.GetInstance().VIP == 725) PlayerPrefsManager.GetInstance().VIP = 625;
                else if (PlayerPrefsManager.GetInstance().VIP == 0) PlayerPrefsManager.GetInstance().VIP = 527;

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                shoppopup.SetActive(false);
                break;

            case EM_IAPConstants.Product_goldbuff:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("골드 무한 버프 구매 성공");

                if (PlayerPrefsManager.GetInstance().VIP == 525) PlayerPrefsManager.GetInstance().VIP = 825;
                else if (PlayerPrefsManager.GetInstance().VIP == 526) PlayerPrefsManager.GetInstance().VIP = 925;
                else if (PlayerPrefsManager.GetInstance().VIP == 527) PlayerPrefsManager.GetInstance().VIP = 527528;
                else if (PlayerPrefsManager.GetInstance().VIP == 725) PlayerPrefsManager.GetInstance().VIP = 625;
                else if (PlayerPrefsManager.GetInstance().VIP == 0) PlayerPrefsManager.GetInstance().VIP = 528;

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                shoppopup.SetActive(false);
                break;

            /// --- 신규 생성 목록

            case EM_IAPConstants.Product_allin:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                PlayerPrefsManager.GetInstance().VIP = 625;

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                shoppopup.SetActive(false);
                break;

            case EM_IAPConstants.Product_autonoad:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");

                if (PlayerPrefsManager.GetInstance().VIP == 527) PlayerPrefsManager.GetInstance().VIP = 625;
                else if (PlayerPrefsManager.GetInstance().VIP == 528) PlayerPrefsManager.GetInstance().VIP = 625;
                else if (PlayerPrefsManager.GetInstance().VIP == 825) PlayerPrefsManager.GetInstance().VIP = 625;
                else if (PlayerPrefsManager.GetInstance().VIP == 925) PlayerPrefsManager.GetInstance().VIP = 625;
                else PlayerPrefsManager.GetInstance().VIP = 725;

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                shoppopup.SetActive(false);
                break;

            case EM_IAPConstants.Product_autobuff:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");

                if (PlayerPrefsManager.GetInstance().VIP == 526) PlayerPrefsManager.GetInstance().VIP = 625;
                else if (PlayerPrefsManager.GetInstance().VIP == 725) PlayerPrefsManager.GetInstance().VIP = 625;
                else if (PlayerPrefsManager.GetInstance().VIP == 925) PlayerPrefsManager.GetInstance().VIP = 625;
                else PlayerPrefsManager.GetInstance().VIP = 825;

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                shoppopup.SetActive(false);
                break;

            case EM_IAPConstants.Product_noadbuff:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");

                if (PlayerPrefsManager.GetInstance().VIP == 525) PlayerPrefsManager.GetInstance().VIP = 625;
                else if (PlayerPrefsManager.GetInstance().VIP == 725) PlayerPrefsManager.GetInstance().VIP = 625;
                else if (PlayerPrefsManager.GetInstance().VIP == 825) PlayerPrefsManager.GetInstance().VIP = 625;
                else PlayerPrefsManager.GetInstance().VIP = 925;

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                shoppopup.SetActive(false);
                break;





            //
            //
            //
            case EM_IAPConstants.Product_goldmitt:

                int sDataList = PlayerPrefs.GetInt("Pet_BuyData", 000);

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");

                if (sDataList == 000)
                {
                    PlayerPrefs.SetInt("Pet_BuyData", 100);
                }
                else if (sDataList == 010)
                {
                    PlayerPrefs.SetInt("Pet_BuyData", 110);
                }
                else if (sDataList == 001)
                {
                    PlayerPrefs.SetInt("Pet_BuyData", 101);
                }
                else if (sDataList == 011)
                {
                    PlayerPrefs.SetInt("Pet_BuyData", 111);
                }
                else
                {
                    // 중복이면 다이아 더 돌려줘.
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 3000);
                }

                PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 1000);
                PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(PlayerPrefsManager.GetInstance().gupbap, "10000");
                PlayerPrefsManager.GetInstance().key += 50;

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                shoppopup.SetActive(false);
                break;


            case EM_IAPConstants.Product_goldboots:

                sDataList = PlayerPrefs.GetInt("Pet_BuyData", 000);

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");

                if (sDataList == 000)
                {
                    PlayerPrefs.SetInt("Pet_BuyData", 010);
                }
                else if (sDataList == 100)
                {
                    PlayerPrefs.SetInt("Pet_BuyData", 110);
                }
                else if (sDataList == 001)
                {
                    PlayerPrefs.SetInt("Pet_BuyData", 011);
                }
                else if (sDataList == 101)
                {
                    PlayerPrefs.SetInt("Pet_BuyData", 111);
                }
                else
                {
                    // 중복이면 다이아 더 돌려줘.
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 3000);
                }

                PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 1000);
                PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(PlayerPrefsManager.GetInstance().gupbap, "10000");
                PlayerPrefsManager.GetInstance().key += 50;


                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;


                shoppopup.SetActive(false);
                break;


            case EM_IAPConstants.Product_golddduk:

                sDataList = PlayerPrefs.GetInt("Pet_BuyData", 000);

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");

                if (sDataList == 000)
                {
                    PlayerPrefs.SetInt("Pet_BuyData", 001);
                }
                else if (sDataList == 100)
                {
                    PlayerPrefs.SetInt("Pet_BuyData", 101);
                }
                else if (sDataList == 010)
                {
                    PlayerPrefs.SetInt("Pet_BuyData", 011);
                }
                else if (sDataList == 110)
                {
                    PlayerPrefs.SetInt("Pet_BuyData", 111);
                }
                else
                {
                    // 중복이면 다이아 더 돌려줘.
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 3000);
                }


                PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 1000);
                PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(PlayerPrefsManager.GetInstance().gupbap, "10000");
                PlayerPrefsManager.GetInstance().key += 50;

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                shoppopup.SetActive(false);
                break;

            case EM_IAPConstants.Product_goldset:

                sDataList = PlayerPrefs.GetInt("Pet_BuyData", 000);

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");

                if (sDataList == 000)
                {
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 9000);
                }
                else if (sDataList == 100 || sDataList == 010 || sDataList == 001)
                {
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 6000);
                }
                else if (sDataList == 110 || sDataList == 011 || sDataList == 101)
                {
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 3000);
                }

                PlayerPrefs.SetInt("Pet_BuyData", 111);

                PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 3000);
                PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(PlayerPrefsManager.GetInstance().gupbap, "30000");
                PlayerPrefsManager.GetInstance().key += 200;

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                shoppopup.SetActive(false);
                break;


            case EM_IAPConstants.Product_uniform01:

                // 구매 여부 데이터 배열
                string[] uniform_sDataList = (PlayerPrefsManager.GetInstance().Uniform_Data).Split('+');

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");

                if (uniform_sDataList[1] != "1")
                {
                    // 데이터에 구매 완료 표기
                    uniform_sDataList[1] = "1";

                    // 데이터 새로고침
                    string result = "";

                    result += uniform_sDataList[0] + "+";
                    result += uniform_sDataList[1] + "+";
                    result += uniform_sDataList[2] + "+";
                    result += uniform_sDataList[3] + "+";
                    result += uniform_sDataList[4] + "+";
                    result += uniform_sDataList[5] + "+";
                    result += uniform_sDataList[6];

                    PlayerPrefsManager.GetInstance().Uniform_Data = result;
                }
                else
                {
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 1000);
                }

                PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 300);
                PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(PlayerPrefsManager.GetInstance().ssalbap, "3000");
                PlayerPrefsManager.GetInstance().key += 10;

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                shoppopup.SetActive(false);
                break;




            case EM_IAPConstants.Product_uniform02:

                // 구매 여부 데이터 배열
                uniform_sDataList = (PlayerPrefsManager.GetInstance().Uniform_Data).Split('+');

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");

                if (uniform_sDataList[2] != "1")
                {
                    // 데이터에 구매 완료 표기
                    uniform_sDataList[2] = "1";

                    // 데이터 새로고침
                    string result = "";

                    result += uniform_sDataList[0] + "+";
                    result += uniform_sDataList[1] + "+";
                    result += uniform_sDataList[2] + "+";
                    result += uniform_sDataList[3] + "+";
                    result += uniform_sDataList[4] + "+";
                    result += uniform_sDataList[5] + "+";
                    result += uniform_sDataList[6];

                    PlayerPrefsManager.GetInstance().Uniform_Data = result;
                }
                else
                {
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 1000);
                }

                PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 300);
                PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(PlayerPrefsManager.GetInstance().ssalbap, "3000");
                PlayerPrefsManager.GetInstance().key += 10;

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                shoppopup.SetActive(false);
                break;





            case EM_IAPConstants.Product_uniform03:

                // 구매 여부 데이터 배열
                uniform_sDataList = (PlayerPrefsManager.GetInstance().Uniform_Data).Split('+');

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");

                if (uniform_sDataList[3] != "1")
                {
                    // 데이터에 구매 완료 표기
                    uniform_sDataList[3] = "1";

                    // 데이터 새로고침
                    string result = "";

                    result += uniform_sDataList[0] + "+";
                    result += uniform_sDataList[1] + "+";
                    result += uniform_sDataList[2] + "+";
                    result += uniform_sDataList[3] + "+";
                    result += uniform_sDataList[4] + "+";
                    result += uniform_sDataList[5] + "+";
                    result += uniform_sDataList[6];

                    PlayerPrefsManager.GetInstance().Uniform_Data = result;
                }
                else
                {
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 1000);
                }

                PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 300);
                PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(PlayerPrefsManager.GetInstance().ssalbap, "3000");
                PlayerPrefsManager.GetInstance().key += 10;

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                shoppopup.SetActive(false);
                break;




            case EM_IAPConstants.Product_uniform04:

                // 구매 여부 데이터 배열
                uniform_sDataList = (PlayerPrefsManager.GetInstance().Uniform_Data).Split('+');

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");

                if (uniform_sDataList[4] != "1")
                {
                    // 데이터에 구매 완료 표기
                    uniform_sDataList[4] = "1";

                    // 데이터 새로고침
                    string result = "";

                    result += uniform_sDataList[0] + "+";
                    result += uniform_sDataList[1] + "+";
                    result += uniform_sDataList[2] + "+";
                    result += uniform_sDataList[3] + "+";
                    result += uniform_sDataList[4] + "+";
                    result += uniform_sDataList[5] + "+";
                    result += uniform_sDataList[6];

                    PlayerPrefsManager.GetInstance().Uniform_Data = result;
                }
                else
                {
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 2000);
                }

                PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 600);
                PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(PlayerPrefsManager.GetInstance().ssalbap, "6000");
                PlayerPrefsManager.GetInstance().key += 30;

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                shoppopup.SetActive(false);
                break;




            case EM_IAPConstants.Product_uniform05:

                // 구매 여부 데이터 배열
                uniform_sDataList = (PlayerPrefsManager.GetInstance().Uniform_Data).Split('+');

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");

                if (uniform_sDataList[5] != "1")
                {
                    // 데이터에 구매 완료 표기
                    uniform_sDataList[5] = "1";

                    // 데이터 새로고침
                    string result = "";

                    result += uniform_sDataList[0] + "+";
                    result += uniform_sDataList[1] + "+";
                    result += uniform_sDataList[2] + "+";
                    result += uniform_sDataList[3] + "+";
                    result += uniform_sDataList[4] + "+";
                    result += uniform_sDataList[5] + "+";
                    result += uniform_sDataList[6];

                    PlayerPrefsManager.GetInstance().Uniform_Data = result;
                }
                else
                {
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 2000);
                }

                PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 600);
                PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(PlayerPrefsManager.GetInstance().ssalbap, "6000");
                PlayerPrefsManager.GetInstance().key += 30;

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                shoppopup.SetActive(false);
                break;




            case EM_IAPConstants.Product_uniform06:

                // 구매 여부 데이터 배열
                uniform_sDataList = (PlayerPrefsManager.GetInstance().Uniform_Data).Split('+');

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");

                if (uniform_sDataList[6] != "1")
                {
                    // 데이터에 구매 완료 표기
                    uniform_sDataList[6] = "1";

                    // 데이터 새로고침
                    string result = "";

                    result += uniform_sDataList[0] + "+";
                    result += uniform_sDataList[1] + "+";
                    result += uniform_sDataList[2] + "+";
                    result += uniform_sDataList[3] + "+";
                    result += uniform_sDataList[4] + "+";
                    result += uniform_sDataList[5] + "+";
                    result += uniform_sDataList[6];

                    PlayerPrefsManager.GetInstance().Uniform_Data = result;
                }
                else
                {
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 3000);
                }

                PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 1000);
                PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(PlayerPrefsManager.GetInstance().ssalbap, "10000");
                PlayerPrefsManager.GetInstance().key += 50;

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                shoppopup.SetActive(false);
                break;



            /// ---------------------- <새로_추가한_패키지_상점> -------------------------
            /// 

            case EM_IAPConstants.Product_package_01:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(0);

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                SomeThingPop.SetActive(false);
                break;

            case EM_IAPConstants.Product_package_02:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(1);

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                SomeThingPop.SetActive(false);
                break;

            case EM_IAPConstants.Product_package_03:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(2);

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                SomeThingPop.SetActive(false);
                break;

            case EM_IAPConstants.Product_package_04:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(3);

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                SomeThingPop.SetActive(false);
                break;

            case EM_IAPConstants.Product_package_05:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(4);

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                SomeThingPop.SetActive(false);
                break;

            case EM_IAPConstants.Product_package_06:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(5);

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                SomeThingPop.SetActive(false);
                break;

            case EM_IAPConstants.Product_package_07:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(6);

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                SomeThingPop.SetActive(false);
                break;



                /// ------------------ 한정 패키지 





            case EM_IAPConstants.Product_limited_01:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(7);

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                SomeThingPop.SetActive(false);
                break;


            case EM_IAPConstants.Product_limited_02:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(8);

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                SomeThingPop.SetActive(false);
                break;

            case EM_IAPConstants.Product_limited_03:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(9);

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                SomeThingPop.SetActive(false);
                break;

            case EM_IAPConstants.Product_limited_04:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(10);

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                SomeThingPop.SetActive(false);
                break;

            case EM_IAPConstants.Product_limited_05:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(11);

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                SomeThingPop.SetActive(false);
                break;

            case EM_IAPConstants.Product_limited_06:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(12);

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                SomeThingPop.SetActive(false);
                break;

            case EM_IAPConstants.Product_limited_07:

                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(13);

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                SomeThingPop.SetActive(false);
                break;


                /// -------------------------------------------------



            case EM_IAPConstants.Product_day_01:
                PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Day_05 = 6;
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(18);

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                SomeThingPop.SetActive(false);
                break;


            case EM_IAPConstants.Product_week_01:
                PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Week_05 = 99;
                if (PlayerPrefsManager.GetInstance().dayLimitData[0].weekend_Day == 0)
                {
                    PlayerPrefsManager.GetInstance().dayLimitData[0].weekend_Day = UnbiasedTime.Instance.Now().Day;
                }
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(23);

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                SomeThingPop.SetActive(false);
                break;


            case EM_IAPConstants.Product_month_01:

                PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Mouth_05 = 81;
                if (PlayerPrefsManager.GetInstance().dayLimitData[0].mouth_Day == 0)
                {
                    PlayerPrefsManager.GetInstance().dayLimitData[0].mouth_Day = UnbiasedTime.Instance.Now().Day;
                }
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(28);

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                SomeThingPop.SetActive(false);
                break;




        }

        UserWallet.GetInstance().ShowAllMoney();
        // VIP 승급에 따라 구매버튼 잠그기.
        VIPManager.GetInstance().VIPINIT();
        GameObject.Find("CharacterManager").GetComponent<CharacterManager>().UniformInit();
        // 자동 강제 서버 세이브
        PlayerPrefs.Save();
        playNANOO.StorageSaveForBuy(product.Name);
    }

    // Failed purchase handler
    void PurchaseFailedHandler(IAPProduct product, string failureReason)
    {
        //Debug.LogError("The purchase of product " + product.Name + " has failed : " + failureReason);
        PopUpObjectManager.GetInstance().ShowWarnnigProcess("결제 취소");

        // 구매실패시 핸들러 떼주고.
        InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;
    }


    public void Purchase_Product_dia100()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_dia100);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_dia600()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_dia600);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_dia1300()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_dia1300);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_dia4200()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_dia4200);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_dia7500()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_dia7500);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_dia16000()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_dia16000);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_auto()
    {
        //if (PlayerPrefsManager.GetInstance().VIP == 525)
        //{
        //    PopUpObjectManager.GetInstance().ShowWarnnigProcess("이미 구매한 항목입니다.");
        //    return;
        //}

        InAppPurchasing.Purchase(EM_IAPConstants.Product_auto);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }



    /// ---------------------- 신규 생성 목록 ------------
    /// 


    public void Purchase_Product_nad()
    {
        //if (PlayerPrefsManager.GetInstance().VIP == 525)
        //{
        //    PopUpObjectManager.GetInstance().ShowWarnnigProcess("이미 구매한 항목입니다.");
        //    return;
        //}


        InAppPurchasing.Purchase(EM_IAPConstants.Product_nad);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_mattzipbuff()
    {
        //if (PlayerPrefsManager.GetInstance().VIP == 525)
        //{
        //    PopUpObjectManager.GetInstance().ShowWarnnigProcess("이미 구매한 항목입니다.");
        //    return;
        //}


        InAppPurchasing.Purchase(EM_IAPConstants.Product_mattzipbuff);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_goldbuff()
    {
        //if (PlayerPrefsManager.GetInstance().VIP == 525)
        //{
        //    PopUpObjectManager.GetInstance().ShowWarnnigProcess("이미 구매한 항목입니다.");
        //    return;
        //}


        InAppPurchasing.Purchase(EM_IAPConstants.Product_goldbuff);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_allin()
    {
        //if (PlayerPrefsManager.GetInstance().VIP == 525)
        //{
        //    PopUpObjectManager.GetInstance().ShowWarnnigProcess("이미 구매한 항목입니다.");
        //    return;
        //}


        InAppPurchasing.Purchase(EM_IAPConstants.Product_allin);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_autonoad()
    {
        //if (PlayerPrefsManager.GetInstance().VIP == 525)
        //{
        //    PopUpObjectManager.GetInstance().ShowWarnnigProcess("이미 구매한 항목입니다.");
        //    return;
        //}


        InAppPurchasing.Purchase(EM_IAPConstants.Product_autonoad);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_autobuff()
    {
        //if (PlayerPrefsManager.GetInstance().VIP == 525)
        //{
        //    PopUpObjectManager.GetInstance().ShowWarnnigProcess("이미 구매한 항목입니다.");
        //    return;
        //}


        InAppPurchasing.Purchase(EM_IAPConstants.Product_autobuff);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_noadbuff()
    {
        //if (PlayerPrefsManager.GetInstance().VIP == 525)
        //{
        //    PopUpObjectManager.GetInstance().ShowWarnnigProcess("이미 구매한 항목입니다.");
        //    return;
        //}


        InAppPurchasing.Purchase(EM_IAPConstants.Product_noadbuff);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }



    public void Purchase_Product_goldmitt()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_goldmitt);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_goldboots()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_goldboots);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_golddduk()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_golddduk);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_goldset()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_goldset);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_uniform01()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_uniform01);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_uniform02()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_uniform02);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_uniform03()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_uniform03);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_uniform04()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_uniform04);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_uniform05()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_uniform05);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_uniform06()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_uniform06);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }




    /// ----------------- 새로 추가된 패키지 210420 -------------------
    /// 

    public void Purchase_Product_package_01()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_package_01);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_package_02()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_package_02);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_package_03()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_package_03);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_package_04()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_package_04);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_package_05()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_package_05);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_package_06()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_package_06);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_package_07()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_package_07);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }





    /// ----------------------------------------------------------------------






    public void Purchase_Product_limited_01()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_limited_01);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_limited_02()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_limited_02);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_limited_03()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_limited_03);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_limited_04()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_limited_04);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_limited_05()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_limited_05);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_limited_06()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_limited_06);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_limited_07()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_limited_07);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }



    /// ----------------------------------------
    /// 

    public void Purchase_Product_day_01(int _index)
    {
        float diaPass = PlayerPrefs.GetFloat("dDiamond", 0);

        switch (_index)
        {
            case 0:
                InAppPurchasing.Purchase(EM_IAPConstants.Product_day_01);
                // 핸들러 등록
                InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
                break;

            case 1:
                diaPass = PlayerPrefs.GetFloat("dDiamond", 0) - pakageInfo[14].pakaPrice;
                if (diaPass < 0)
                {
                    return;
                }
                PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Day_01 = 46;
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(14);
                SomeThingPop.SetActive(false);
                break;

            case 2:
                diaPass = PlayerPrefs.GetFloat("dDiamond", 0) - pakageInfo[15].pakaPrice;
                if (diaPass < 0)
                {
                    return;
                }
                PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Day_02 = 85;
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(15);
                SomeThingPop.SetActive(false);
                break;

            case 3:
                diaPass = PlayerPrefs.GetFloat("dDiamond", 0) - pakageInfo[16].pakaPrice;
                if (diaPass < 0)
                {
                    return;
                }
                PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Day_03 = 74;
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(16);
                SomeThingPop.SetActive(false);
                break;

            case 4:
                diaPass = PlayerPrefs.GetFloat("dDiamond", 0) - pakageInfo[17].pakaPrice;
                if (diaPass < 0)
                {
                    return;
                }
                PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Day_04 = 47;
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(17);
                SomeThingPop.SetActive(false);
                break;

            default:
                break;
        }

        PlayerPrefs.SetFloat("dDiamond", diaPass);
        PlayerPrefs.Save();
        UserWallet.GetInstance().ShowUserDia();


    }

    public void Purchase_Product_week_01(int _index)
    {
        float diaPass = PlayerPrefs.GetFloat("dDiamond", 0);

        switch (_index)
        {
            case 0:
                InAppPurchasing.Purchase(EM_IAPConstants.Product_week_01);
                // 핸들러 등록
                InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
                break;

            case 1:
                diaPass = PlayerPrefs.GetFloat("dDiamond", 0) - pakageInfo[19].pakaPrice;
                if (diaPass < 0)
                {
                    return;
                }

                PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Week_01 = 41;
                if (PlayerPrefsManager.GetInstance().dayLimitData[0].weekend_Day == 0)
                {
                    PlayerPrefsManager.GetInstance().dayLimitData[0].weekend_Day = (int)UnbiasedTime.Instance.Now().DayOfWeek;
                }
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(19);
                SomeThingPop.SetActive(false);
                break;

            case 2:
                diaPass = PlayerPrefs.GetFloat("dDiamond", 0) - pakageInfo[20].pakaPrice;
                if (diaPass < 0)
                {
                    return;
                }

                PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Week_02 = 22;
                if (PlayerPrefsManager.GetInstance().dayLimitData[0].weekend_Day == 0)
                {
                    PlayerPrefsManager.GetInstance().dayLimitData[0].weekend_Day = (int)UnbiasedTime.Instance.Now().DayOfWeek;
                }
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(20);
                SomeThingPop.SetActive(false);
                break;

            case 3:
                diaPass = PlayerPrefs.GetFloat("dDiamond", 0) - pakageInfo[21].pakaPrice;
                if (diaPass < 0)
                {
                    return;
                }

                PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Week_03 = 38;
                if (PlayerPrefsManager.GetInstance().dayLimitData[0].weekend_Day == 0)
                {
                    PlayerPrefsManager.GetInstance().dayLimitData[0].weekend_Day = (int)UnbiasedTime.Instance.Now().DayOfWeek;
                }
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(21);
                SomeThingPop.SetActive(false);
                break;

            case 4:
                diaPass = PlayerPrefs.GetFloat("dDiamond", 0) - pakageInfo[22].pakaPrice;
                if (diaPass < 0)
                {
                    return;
                }

                PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Week_04 = 67;
                if (PlayerPrefsManager.GetInstance().dayLimitData[0].weekend_Day == 0)
                {
                    PlayerPrefsManager.GetInstance().dayLimitData[0].weekend_Day = (int)UnbiasedTime.Instance.Now().DayOfWeek;
                }
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(22);
                SomeThingPop.SetActive(false);
                break;

            default:
                break;
        }

        PlayerPrefs.SetFloat("dDiamond", diaPass);
        PlayerPrefs.Save();
        UserWallet.GetInstance().ShowUserDia();
    }


    public void Purchase_Product_month_01(int _index)
    {
        float diaPass = PlayerPrefs.GetFloat("dDiamond", 0);

        switch (_index)
        {
            case 0:
                InAppPurchasing.Purchase(EM_IAPConstants.Product_month_01);
                // 핸들러 등록
                InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
                break;

            case 1:
                diaPass = PlayerPrefs.GetFloat("dDiamond", 0) - pakageInfo[24].pakaPrice;
                if (diaPass < 0)
                {
                    return;
                }

                PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Mouth_01 = 10;
                if (PlayerPrefsManager.GetInstance().dayLimitData[0].mouth_Day == 0)
                {
                    PlayerPrefsManager.GetInstance().dayLimitData[0].mouth_Day = UnbiasedTime.Instance.Now().Day;
                }
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(24);
                SomeThingPop.SetActive(false);
                break;

            case 2:
                diaPass = PlayerPrefs.GetFloat("dDiamond", 0) - pakageInfo[25].pakaPrice;
                if (diaPass < 0)
                {
                    return;
                }

                PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Mouth_02 = 29;
                if (PlayerPrefsManager.GetInstance().dayLimitData[0].mouth_Day == 0)
                {
                    PlayerPrefsManager.GetInstance().dayLimitData[0].mouth_Day = UnbiasedTime.Instance.Now().Day;
                }
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(25);
                SomeThingPop.SetActive(false);
                break;

            case 3:
                diaPass = PlayerPrefs.GetFloat("dDiamond", 0) - pakageInfo[26].pakaPrice;
                if (diaPass < 0)
                {
                    return;
                }

                PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Mouth_03 = 93;
                if (PlayerPrefsManager.GetInstance().dayLimitData[0].mouth_Day == 0)
                {
                    PlayerPrefsManager.GetInstance().dayLimitData[0].mouth_Day = UnbiasedTime.Instance.Now().Day;
                }
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(26);
                SomeThingPop.SetActive(false);
                break;

            case 4:
                diaPass = PlayerPrefs.GetFloat("dDiamond", 0) - pakageInfo[27].pakaPrice;
                if (diaPass < 0)
                {
                    return;
                }

                PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Mouth_04 = 51;
                if (PlayerPrefsManager.GetInstance().dayLimitData[0].mouth_Day == 0)
                {
                    PlayerPrefsManager.GetInstance().dayLimitData[0].mouth_Day = UnbiasedTime.Instance.Now().Day;
                }
                PopUpObjectManager.GetInstance().ShowWarnnigProcess("패키지 구매 성공");
                GetMyMoneyItem(27);
                SomeThingPop.SetActive(false);
                break;


            default:
                break;
        }

        PlayerPrefs.SetFloat("dDiamond", diaPass);
        PlayerPrefs.Save();
        UserWallet.GetInstance().ShowUserDia();

    }


}
