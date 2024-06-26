﻿using EasyMobile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IAPManager : MonoBehaviour
{
    [Header(" - 패키지 영문 이름 스트링스")]
    public string[] pakageEnglishInfos;
    [Space]
    public Text[] shopDesc;

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
    /// 패키지 상품관리
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
        /// 번역
        bool isKorean = false;
        if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean") isKorean = true;

        SomeParent = EventSystem.current.currentSelectedGameObject.transform.parent.parent;
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
        /// 
        /// 번역
        if (isKorean)
            SomeThingTrans.GetChild(_index).GetChild(0).GetComponent<Text>().text = pakageInfo[_index].pakaName;
        else
            SomeThingTrans.GetChild(_index).GetChild(0).GetComponent<Text>().text = pakageEnglishInfos[_index];
        
        
        SomeThingTrans.GetChild(_index).gameObject.SetActive(true);
        BuyButton.GetChild(_index).gameObject.SetActive(true);

        /// 무료 상품들
        if (pakageInfo[_index].pakaPrice == 0)
        {
            /// 번역
            if (isKorean)
                BuyButton.GetChild(_index).GetChild(1).GetComponent<Text>().text = "구매";
            else
                BuyButton.GetChild(_index).GetChild(1).GetComponent<Text>().text = "Buy";
            vatTextObj.SetActive(false);
        }
        /// 현금으로 사는 것들
        else if (_index < 14 || _index == 18 || _index == 23 || _index == 28)
        {
            BuyButton.GetChild(_index).GetChild(1).GetComponent<Text>().text = $"₩ {pakageInfo[_index].pakaPrice:N0}";
            vatTextObj.SetActive(true);
        }
        /// 다이아로 사는 것들
        else
        {
            /// 번역
            if (isKorean)
                BuyButton.GetChild(_index).GetChild(1).GetComponent<Text>().text = "구매";
            else
                BuyButton.GetChild(_index).GetChild(1).GetComponent<Text>().text = "Buy";
            vatTextObj.SetActive(false);
        }

        var tmpInfo = GetPakInfo(_index);

        for (int i = 0; i < shopDesc.Length; i++)
        {
            shopDesc[i].gameObject.SetActive(false);
        }

        /// 번역
        if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
        {
            ///// 포함 상품 표기
            //if (tmpInfo.getDia != 0)
            //{
            //    shopDesc[0].text = $"다이아 x {tmpInfo.getDia:N0}";
            //    shopDesc[0].gameObject.SetActive(true);
            //}
            //if (tmpInfo.getShiled != 0)
            //{
            //    shopDesc[1].text = $"방패뽑기권 x {tmpInfo.getShiled:N0}";
            //    shopDesc[1].gameObject.SetActive(true);
            //}
            //if (tmpInfo.getGuapbap != 0)
            //{
            //    shopDesc[2].text = $"국밥 x {tmpInfo.getGuapbap:N0}";
            //    shopDesc[2].gameObject.SetActive(true);
            //}
            //if (tmpInfo.getSsalbap != 0)
            //{
            //    shopDesc[3].text = $"쌀밥 x {tmpInfo.getSsalbap:N0}";
            //    shopDesc[3].gameObject.SetActive(true);
            //}
            //if (tmpInfo.getKimchi != 0)
            //{
            //    shopDesc[4].text = $"깍두기 x {tmpInfo.getKimchi:N0}";
            //    shopDesc[4].gameObject.SetActive(true);
            //}
            //if (tmpInfo.getKey != 0)
            //{
            //    shopDesc[5].text = $"열쇠 x {tmpInfo.getKey:N0}";
            //    shopDesc[5].gameObject.SetActive(true);
            //}
            //if (tmpInfo.getTicket != 0)
            //{
            //    shopDesc[6].text = $"PvP입장권 x {tmpInfo.getTicket:N0}";
            //    shopDesc[6].gameObject.SetActive(true);
            //}

            shopDesc[0].transform.parent.parent.parent.gameObject.SetActive(false);
        }
        else
        {
            shopDesc[0].transform.parent.parent.parent.gameObject.SetActive(true);

            /// 포함 상품 표기
            if (tmpInfo.getDia != 0)
            {
                shopDesc[0].text = $"Diamond <color=#2196f3>x {tmpInfo.getDia:N0}</color>";
                shopDesc[0].gameObject.SetActive(true);
            }
            if (tmpInfo.getShiled != 0)
            {
                shopDesc[1].text = $"Shield ticket <color=#2196f3>x {tmpInfo.getShiled:N0}</color>";
                shopDesc[1].gameObject.SetActive(true);
            }
            if (tmpInfo.getGuapbap != 0)
            {
                shopDesc[2].text = $"Korean soup <color=#2196f3>x {tmpInfo.getGuapbap:N0}</color>";
                shopDesc[2].gameObject.SetActive(true);
            }
            if (tmpInfo.getSsalbap != 0)
            {
                shopDesc[3].text = $"Rice <color=#2196f3>x {tmpInfo.getSsalbap:N0}</color>";
                shopDesc[3].gameObject.SetActive(true);
            }
            if (tmpInfo.getKimchi != 0)
            {
                shopDesc[4].text = $"Radish kimchi <color=#2196f3>x {tmpInfo.getKimchi:N0}</color>";
                shopDesc[4].gameObject.SetActive(true);
            }
            if (tmpInfo.getKey != 0)
            {
                shopDesc[5].text = $"Key <color=#2196f3>x {tmpInfo.getKey:N0}</color>";
                shopDesc[5].gameObject.SetActive(true);
            }
            if (tmpInfo.getTicket != 0)
            {
                shopDesc[6].text = $"PvP ticket <color=#2196f3>x {tmpInfo.getTicket:N0}</color>";
                shopDesc[6].gameObject.SetActive(true);
            }
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

        if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
        {
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

        }
        else
        {
            switch (product.Name)
            {

                case EM_IAPConstants.Product_dia100:

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Dia x 100 purchase success.");
                    //PlayerPrefsManager.GetInstance().diamond = dts.AddStringDouble(ppmDia, "100");
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 100);
                    InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                    InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                    shoppopup.SetActive(false);

                    break;

                case EM_IAPConstants.Product_dia600:

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Dia x 600 purchase success.");
                    //PlayerPrefsManager.GetInstance().diamond = dts.AddStringDouble(ppmDia, "600");
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 600);

                    InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                    InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                    shoppopup.SetActive(false);


                    break;

                case EM_IAPConstants.Product_dia1300:

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Dia x 1,300 purchase success.");
                    //PlayerPrefsManager.GetInstance().diamond = dts.AddStringDouble(ppmDia, "1300");
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 1300);

                    InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                    InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                    shoppopup.SetActive(false);


                    break;


                case EM_IAPConstants.Product_dia4200:

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Dia x 4,200 purchase success.");
                    //PlayerPrefsManager.GetInstance().diamond = dts.AddStringDouble(ppmDia, "4200");

                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 4200);


                    InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                    InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                    shoppopup.SetActive(false);
                    break;


                case EM_IAPConstants.Product_dia7500:

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Dia x 7,500 purchase success.");
                    //PlayerPrefsManager.GetInstance().diamond = dts.AddStringDouble(ppmDia, "7500");

                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 7500);


                    InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                    InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                    shoppopup.SetActive(false);
                    break;

                case EM_IAPConstants.Product_dia16000:

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Dia x 16,00 purchase success.");
                    //PlayerPrefsManager.GetInstance().diamond = dts.AddStringDouble(ppmDia, "16000");


                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") + 16000);


                    InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                    InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                    shoppopup.SetActive(false);


                    break;



                /// -------------------------------------------------------------------


                case EM_IAPConstants.Product_auto:

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Auto/box purchase success.");

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

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Remove Ad purchase success.");

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

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Attack buff purchase success.");

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

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Gold buff purchase success.");

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

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Package purchase success.");
                    PlayerPrefsManager.GetInstance().VIP = 625;

                    InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                    InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                    shoppopup.SetActive(false);
                    break;

                case EM_IAPConstants.Product_autonoad:

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Package purchase success.");

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

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Package purchase success.");

                    if (PlayerPrefsManager.GetInstance().VIP == 526) PlayerPrefsManager.GetInstance().VIP = 625;
                    else if (PlayerPrefsManager.GetInstance().VIP == 725) PlayerPrefsManager.GetInstance().VIP = 625;
                    else if (PlayerPrefsManager.GetInstance().VIP == 925) PlayerPrefsManager.GetInstance().VIP = 625;
                    else PlayerPrefsManager.GetInstance().VIP = 825;

                    InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                    InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                    shoppopup.SetActive(false);
                    break;

                case EM_IAPConstants.Product_noadbuff:

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Package purchase success.");

                    if (PlayerPrefsManager.GetInstance().VIP == 525) PlayerPrefsManager.GetInstance().VIP = 625;
                    else if (PlayerPrefsManager.GetInstance().VIP == 725) PlayerPrefsManager.GetInstance().VIP = 625;
                    else if (PlayerPrefsManager.GetInstance().VIP == 825) PlayerPrefsManager.GetInstance().VIP = 625;
                    else PlayerPrefsManager.GetInstance().VIP = 925;

                    InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                    InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                    shoppopup.SetActive(false);
                    break;



                /// ---------------------- <새로_추가한_패키지_상점> -------------------------
                /// 

                case EM_IAPConstants.Product_package_01:

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Package purchase success.");
                    GetMyMoneyItem(0);

                    InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                    InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                    SomeThingPop.SetActive(false);
                    break;

                case EM_IAPConstants.Product_package_02:

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Package purchase success.");
                    GetMyMoneyItem(1);

                    InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                    InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                    SomeThingPop.SetActive(false);
                    break;

                case EM_IAPConstants.Product_package_03:

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Package purchase success.");
                    GetMyMoneyItem(2);

                    InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                    InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                    SomeThingPop.SetActive(false);
                    break;

                case EM_IAPConstants.Product_package_04:

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Package purchase success.");
                    GetMyMoneyItem(3);

                    InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                    InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                    SomeThingPop.SetActive(false);
                    break;

                case EM_IAPConstants.Product_package_05:

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Package purchase success.");
                    GetMyMoneyItem(4);

                    InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                    InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                    SomeThingPop.SetActive(false);
                    break;

                case EM_IAPConstants.Product_package_06:

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Package purchase success.");
                    GetMyMoneyItem(5);

                    InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                    InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                    SomeThingPop.SetActive(false);
                    break;

                case EM_IAPConstants.Product_package_07:

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Package purchase success.");
                    GetMyMoneyItem(6);

                    InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                    InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                    SomeThingPop.SetActive(false);
                    break;



                /// ------------------ 한정 패키지 





                case EM_IAPConstants.Product_limited_01:

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Package purchase success.");
                    GetMyMoneyItem(7);

                    InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                    InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                    SomeThingPop.SetActive(false);
                    break;


                case EM_IAPConstants.Product_limited_02:

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Package purchase success.");
                    GetMyMoneyItem(8);

                    InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                    InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                    SomeThingPop.SetActive(false);
                    break;

                case EM_IAPConstants.Product_limited_03:

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Package purchase success.");
                    GetMyMoneyItem(9);

                    InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                    InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                    SomeThingPop.SetActive(false);
                    break;

                case EM_IAPConstants.Product_limited_04:

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Package purchase success.");
                    GetMyMoneyItem(10);

                    InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                    InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                    SomeThingPop.SetActive(false);
                    break;

                case EM_IAPConstants.Product_limited_05:

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Package purchase success.");
                    GetMyMoneyItem(11);

                    InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                    InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                    SomeThingPop.SetActive(false);
                    break;

                case EM_IAPConstants.Product_limited_06:

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Package purchase success.");
                    GetMyMoneyItem(12);

                    InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                    InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                    SomeThingPop.SetActive(false);
                    break;

                case EM_IAPConstants.Product_limited_07:

                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Package purchase success.");
                    GetMyMoneyItem(13);

                    InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                    InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                    SomeThingPop.SetActive(false);
                    break;


                /// -------------------------------------------------



                case EM_IAPConstants.Product_day_01:
                    PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Day_05 = 6;
                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Package purchase success.");
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
                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Package purchase success.");
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
                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Package purchase success.");
                    GetMyMoneyItem(28);

                    InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                    InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                    SomeThingPop.SetActive(false);
                    break;




            }

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
        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Canceled payment.");

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
        //InAppPurchasing.Purchase(EM_IAPConstants.Product_goldmitt);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_goldboots()
    {
        //InAppPurchasing.Purchase(EM_IAPConstants.Product_goldboots);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_golddduk()
    {
        //InAppPurchasing.Purchase(EM_IAPConstants.Product_golddduk);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_goldset()
    {
        //InAppPurchasing.Purchase(EM_IAPConstants.Product_goldset);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_uniform01()
    {
        //InAppPurchasing.Purchase(EM_IAPConstants.Product_uniform01);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_uniform02()
    {
        //InAppPurchasing.Purchase(EM_IAPConstants.Product_uniform02);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_uniform03()
    {
        //InAppPurchasing.Purchase(EM_IAPConstants.Product_uniform03);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_uniform04()
    {
        //InAppPurchasing.Purchase(EM_IAPConstants.Product_uniform04);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_uniform05()
    {
        //InAppPurchasing.Purchase(EM_IAPConstants.Product_uniform05);
        // 핸들러 등록
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    public void Purchase_Product_uniform06()
    {
        //InAppPurchasing.Purchase(EM_IAPConstants.Product_uniform06);
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

        if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
        {
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
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("다이아몬드 부족");
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
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("다이아몬드 부족");
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
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("다이아몬드 부족");
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
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("다이아몬드 부족");
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

        }
        else
        {
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
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Not enough diamonds.");
                        return;
                    }
                    PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Day_01 = 46;
                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Package purchase success.");
                    GetMyMoneyItem(14);
                    SomeThingPop.SetActive(false);
                    break;

                case 2:
                    diaPass = PlayerPrefs.GetFloat("dDiamond", 0) - pakageInfo[15].pakaPrice;
                    if (diaPass < 0)
                    {
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Not enough diamonds.");
                        return;
                    }
                    PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Day_02 = 85;
                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Package purchase success.");
                    GetMyMoneyItem(15);
                    SomeThingPop.SetActive(false);
                    break;

                case 3:
                    diaPass = PlayerPrefs.GetFloat("dDiamond", 0) - pakageInfo[16].pakaPrice;
                    if (diaPass < 0)
                    {
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Not enough diamonds.");
                        return;
                    }
                    PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Day_03 = 74;
                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Package purchase success.");
                    GetMyMoneyItem(16);
                    SomeThingPop.SetActive(false);
                    break;

                case 4:
                    diaPass = PlayerPrefs.GetFloat("dDiamond", 0) - pakageInfo[17].pakaPrice;
                    if (diaPass < 0)
                    {
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Not enough diamonds.");
                        return;
                    }
                    PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Day_04 = 47;
                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("Package purchase success.");
                    GetMyMoneyItem(17);
                    SomeThingPop.SetActive(false);
                    break;

                default:
                    break;
            }

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
                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("다이아몬드 부족");
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
                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("다이아몬드 부족");
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
                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("다이아몬드 부족");
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
                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("다이아몬드 부족");
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
                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("다이아몬드 부족");
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
                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("다이아몬드 부족");
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
                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("다이아몬드 부족");
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
                    PopUpObjectManager.GetInstance().ShowWarnnigProcess("다이아몬드 부족");
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
