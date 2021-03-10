using EasyMobile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

public class IAPManager : MonoBehaviour
{
    public PlayNANOOExample playNANOO;
    [Header("-구매성공시 팝업 종료")]
    public GameObject shoppopup;

    public void GetProductsList()
    {
        IAPProduct[] products = InAppPurchasing.GetAllIAPProducts();

        // Print all product names
        foreach (IAPProduct prod in products)
        {
            Debug.Log("Product name: " + prod.Name);
        }

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



            ///
            ///
            ///


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
                PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(PlayerPrefs.GetString("gupbap"), "10000");
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
                PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(PlayerPrefs.GetString("gupbap"), "10000");
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
                PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(PlayerPrefs.GetString("gupbap"), "10000");
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
                PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(PlayerPrefs.GetString("gupbap"), "30000");
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
                PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(PlayerPrefs.GetString("ssalbap"), "3000");
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
                PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(PlayerPrefs.GetString("ssalbap"), "3000");
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
                PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(PlayerPrefs.GetString("ssalbap"), "3000");
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
                PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(PlayerPrefs.GetString("ssalbap"), "6000");
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
                PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(PlayerPrefs.GetString("ssalbap"), "6000");
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
                PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(PlayerPrefs.GetString("ssalbap"), "10000");
                PlayerPrefsManager.GetInstance().key += 50;

                InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
                InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

                shoppopup.SetActive(false);
                break;






        }

        UserWallet.GetInstance().ShowAllMoney();
        // VIP 승급에 따라 구매버튼 잠그기.
        VIPManager.GetInstance().VIPINIT();
        GameObject.Find("CharacterManager").GetComponent<CharacterManager>().UniformInit();
        // 자동 강제 서버 세이브
        PlayerPrefs.Save();
        playNANOO.StorageSaveForBuy();
    }

    // Failed purchase handler
    void PurchaseFailedHandler(IAPProduct product, string failureReason)
    {
        Debug.LogError("The purchase of product " + product.Name + " has failed : " + failureReason);
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



















}
