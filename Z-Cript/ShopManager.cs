using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    DoubleToStringNum dts = new DoubleToStringNum();

    public GameObject SomeThingPop;
    public Transform SomeThingTrans;
    [Header("-부스터 땡겨오기")]
    public Booster_Power booster_Power;
    public Booster_Body booster_Body;

    [Header("-100배 표기할 텍스트 박스")]
    public Text Gold100Text;


    /// <summary>
    /// 다이아몬드 소모 로직 넣었을때 통과하냐??
    /// </summary>
    /// <returns></returns>
    bool DiaPass()
    {
        //var dia = PlayerPrefsManager.GetInstance().diamond;
        //var result = dts.SubStringDouble(dia, "100");
        string result = "";

        if (PlayerPrefs.GetFloat("dDiamond") - 100 < 0)
        {
            result = "-1";
        }


        if (result != "-1")
        {

            return true;
        }
        else
        {
            return false;
        }

    }


    /// <summary>
    /// 확정 구매 버튼 누르면
    /// </summary>
    /// <param name="_index"></param>
    public void ShowBuySomePop()
    {
        switch (normal_dex)
        {
            // 골드
            case 0:
                if (DiaPass())
                {
                    // 다이아 소모
                    //PlayerPrefsManager.GetInstance().diamond = result;
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 100);


                    GetNormalGold();

                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("성공적으로 구입하셨습니다.");
                    else
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Successfully purchased.");
                }
                else
                {
                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("보유 다이아가 부족합니다.");
                    else
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Not enough diamonds.");

                }
                break;

            // 열쇠
            case 1:
                if (DiaPass())
                {
                    // 다이아 소모
                    //PlayerPrefsManager.GetInstance().diamond = result;
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 100);


                    PlayerPrefsManager.GetInstance().key += 10;
                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("성공적으로 구입하셨습니다.");
                    else
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Successfully purchased.");
                }
                else 
                {
                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("보유 다이아가 부족합니다.");
                    else
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Not enough diamonds.");
                }

                break;

            // 골드 2배
            case 2:
                if (DiaPass())
                {
                    // 다이아 소모
                    //PlayerPrefsManager.GetInstance().diamond = result;
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 100);

                    booster_Power.BuyPowerShop();
                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("성공적으로 구입하셨습니다.");
                    else
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Successfully purchased.");
                }
                else
                {
                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("보유 다이아가 부족합니다.");
                    else
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Not enough diamonds.");

                }

                break;

            // 공격 2배
            case 3:
                if (DiaPass())
                {
                    // 다이아 소모
                    //PlayerPrefsManager.GetInstance().diamond = result;
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 100);

                    booster_Body.BuyBodyShop();
                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("성공적으로 구입하셨습니다.");
                    else
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Successfully purchased.");
                }
                else
                {
                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("보유 다이아가 부족합니다.");
                    else
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Not enough diamonds.");

                }

                break;

            // 우유
            case 4:
                if (DiaPass())
                {
                    // 다이아 소모
                    //PlayerPrefsManager.GetInstance().diamond = result;
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 100);

                    var mlikAmount = PlayerPrefsManager.GetInstance().gupbap;
                    PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(mlikAmount, "1000");

                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("성공적으로 구입하셨습니다.");
                    else
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Successfully purchased.");
                }
                else
                {
                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("보유 다이아가 부족합니다.");
                    else
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Not enough diamonds.");

                }
                break;

            // 쌀밥
            case 8:
                if (DiaPass())
                {
                    // 다이아 소모
                    //PlayerPrefsManager.GetInstance().diamond = result;
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 100);

                    var mlikAmount = PlayerPrefsManager.GetInstance().ssalbap;
                    PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(mlikAmount, "500");

                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("성공적으로 구입하셨습니다.");
                    else
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Successfully purchased.");
                }
                else
                {
                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("보유 다이아가 부족합니다.");
                    else
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Not enough diamonds.");

                }
                break;

            // pvp 입장권
            case 9:
                if (DiaPass())
                {
                    // 다이아 소모
                    //PlayerPrefsManager.GetInstance().diamond = result;
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 100);

                    PlayerPrefsManager.GetInstance().ticket += 5;

                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("성공적으로 구입하셨습니다.");
                    else
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Successfully purchased.");
                }
                else
                {
                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("보유 다이아가 부족합니다.");
                    else
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Not enough diamonds.");

                }
                break;
        }

        UserWallet.GetInstance().ShowAllMoney();

    }

    /// <summary>
    ///  노말 상품 팝업 호출할때 할당 인덱스
    /// </summary>
    int normal_dex;
    /// <summary>
    /// 다이아 100개 짜리 상품 팔때 팝업 호출
    /// </summary>
    /// <param name="_index"></param>
    public void BuySomeThing(int _index)
    {
        normal_dex = _index;
        /// 아이콘 세팅 해주고
        for (int i = 0; i < SomeThingTrans.childCount; i++)
        {
            SomeThingTrans.GetChild(i).gameObject.SetActive(false);
        }
        switch (_index)
        {
            // 골드
            case 0:
                /// 팝업 켜주고
                SomeThingTrans.GetChild(0).gameObject.SetActive(true);
                break;

            // 열쇠
            case 1:
                /// 팝업 켜주고
                SomeThingTrans.GetChild(1).gameObject.SetActive(true);
                break;

            // 골드 2배
            case 2:
                /// 팝업 켜주고
                SomeThingTrans.GetChild(2).gameObject.SetActive(true);
                break;

            // 공격 2배
            case 3:
                /// 팝업 켜주고
                SomeThingTrans.GetChild(3).gameObject.SetActive(true);
                break;

            // 국밥
            case 4:
                /// 팝업 켜주고
                SomeThingTrans.GetChild(4).gameObject.SetActive(true);
                break;

            // 쌀밥
            case 8:
                /// 팝업 켜주고
                SomeThingTrans.GetChild(5).gameObject.SetActive(true);
                break;

            // pvp 입장권
            case 9:
                /// 팝업 켜주고
                SomeThingTrans.GetChild(6).gameObject.SetActive(true);
                break;
        }

        SomeThingPop.SetActive(true);
        // 애니메 재생
        SomeThingPop.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        SomeThingPop.GetComponent<Animation>().Play("Roll_Incre");
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
            case 0:
                if (DiaPassForBG(_index))
                {
                    /// 구매내역이 있다?
                    if (sDataList[_index] == "1") break;

                    // 구매한적 없다면 다이아 소모
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 500);
                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("성공적으로 구입하셨습니다.");
                    else
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Successfully purchased.");
                    sDataList[_index] = "1";
                }
                else
                {
                    /// 구매내역이 있다?
                    if (sDataList[_index] == "1") break;

                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("보유 다이아가 부족합니다.");
                    else
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Not enough diamonds.");
                }
                break;

            case 1:
                if (DiaPassForBG(_index))
                {
                    /// 구매내역이 있다?
                    if (sDataList[_index] == "1") break;

                    // 구매한적 없다면 다이아 소모
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 900);
                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("성공적으로 구입하셨습니다.");
                    else
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Successfully purchased.");
                    sDataList[_index] = "1";
                }
                else
                {
                    /// 구매내역이 있다?
                    if (sDataList[_index] == "1") break;

                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("보유 다이아가 부족합니다.");
                    else
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Not enough diamonds.");
                }
                break;

            case 2:
                if (DiaPassForBG(_index))
                {
                    /// 구매내역이 있다?
                    if (sDataList[_index] == "1") break;

                    // 구매한적 없다면 다이아 소모
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 1590);
                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("성공적으로 구입하셨습니다.");
                    else
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Successfully purchased.");
                    sDataList[_index] = "1";
                }
                else
                {
                    /// 구매내역이 있다?
                    if (sDataList[_index] == "1") break;

                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("보유 다이아가 부족합니다.");
                    else
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Not enough diamonds.");
                }
                break;

            case 3:
                if (DiaPassForBG(_index))
                {
                    /// 구매내역이 있다?
                    if (sDataList[_index] == "1") break;

                    // 구매한적 없다면 다이아 소모
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 2790);
                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("성공적으로 구입하셨습니다.");
                    else
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Successfully purchased.");
                    sDataList[_index] = "1";
                }
                else
                {
                    /// 구매내역이 있다?
                    if (sDataList[_index] == "1") break;

                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("보유 다이아가 부족합니다.");
                    else
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Not enough diamonds.");
                }
                break;

            case 4:
                if (DiaPassForBG(_index))
                {
                    /// 구매내역이 있다?
                    if (sDataList[_index] == "1") break;

                    // 구매한적 없다면 다이아 소모
                    PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 4900);
                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("성공적으로 구입하셨습니다.");
                    else
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Successfully purchased.");
                    sDataList[_index] = "1";
                }
                else
                {
                    /// 구매내역이 있다?
                    if (sDataList[_index] == "1") break;

                    if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("보유 다이아가 부족합니다.");
                    else
                        PopUpObjectManager.GetInstance().ShowWarnnigProcess("Not enough diamonds.");
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
        string[] sDataList = (PlayerPrefsManager.GetInstance().BG_Data).Split('+');

        switch (_index)
        {
            case 0:
                /// 구매내역이 없으면 접근 못함
                if (sDataList[_index] == "0") break;

                break;

            case 1:
                /// 구매내역이 없으면 접근 못함
                if (sDataList[_index] == "0") break;

                break;

            case 2:
                /// 구매내역이 없으면 접근 못함
                if (sDataList[_index] == "0") break;

                break;

            case 3:
                /// 구매내역이 없으면 접근 못함
                if (sDataList[_index] == "0") break;

                break;

            case 4:
                /// 구매내역이 없으면 접근 못함
                if (sDataList[_index] == "0") break;

                break;

        }

    }





    [Header("- 상자 오픈 애니메")]
    public RandomBoxOpen rdopen;

    /// <summary>
    /// 골드 소모 컨텐츠 골드 소모함.
    /// </summary>
    public void GoldBoxOpen(int _index)
    {

        float motherRan = Random.Range(0, 100f);
        float childRan = Random.Range(0, 100f);

        //var dia = PlayerPrefsManager.GetInstance().diamond;
        var gupbap = PlayerPrefsManager.GetInstance().gupbap;
        var Salbap = PlayerPrefsManager.GetInstance().ssalbap;
        var key = PlayerPrefsManager.GetInstance().key;

        switch (_index)
        {
            case 5:
                // A 골드박스
                if(motherRan < 20f)      //다이아
                {
                    int diaRan = 10;

                    if (childRan < 50f)
                    {
                        diaRan += Random.Range(0, 20);
                    }
                    else if (childRan < 85f)
                    {
                        diaRan += Random.Range(20, 60);
                    }
                    else
                    {
                        diaRan += Random.Range(60, 91);
                    }

                    //PlayerPrefsManager.GetInstance().diamond = dts.AddStringDouble(dia, diaRan.ToString());
                    //PlayerPrefsManager.GetInstance().diamond = result;
                    var ddd = PlayerPrefs.GetFloat("dDiamond") + diaRan;
                    PlayerPrefs.SetFloat("dDiamond", ddd);


                    rdopen.RandoBoxInit(1, diaRan);

                    //PopUpObjectManager.GetInstance().ShowWarnnigProcess("다이아 "+ diaRan + "개 획득!");


                }
                else if (motherRan < 40f) //국밥
                {
                    int diaRan = 100;

                    if (childRan < 50f)
                    {
                        diaRan += Random.Range(0, 200);
                    }
                    else if (childRan < 85f)
                    {
                        diaRan += Random.Range(200, 600);
                    }
                    else
                    {
                        diaRan += Random.Range(600, 901);
                    }

                    PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(gupbap, diaRan.ToString("f0"));

                    rdopen.RandoBoxInit(2, diaRan);

                    //PopUpObjectManager.GetInstance().ShowWarnnigProcess("국밥 " + diaRan + "그릇 획득!");

                }
                else if (motherRan < 60f) //키
                {
                    int diaRan = 1;

                    if (childRan < 50f)
                    {
                        diaRan += 0;
                    }
                    else if (childRan < 85f)
                    {
                        diaRan += 1;
                    }
                    else
                    {
                        diaRan += 2;
                    }

                    PlayerPrefsManager.GetInstance().key += diaRan;

                    rdopen.RandoBoxInit(0, diaRan);


                    //PopUpObjectManager.GetInstance().ShowWarnnigProcess("열쇠 " + diaRan + "개 획득!");

                }
                else if (motherRan < 80f) //쌀밥
                {
                    int diaRan = 50;

                    if (childRan < 50f)
                    {
                        diaRan += Random.Range(0, 100);
                    }
                    else if (childRan < 85f)
                    {
                        diaRan += Random.Range(100, 300);
                    }
                    else
                    {
                        diaRan += Random.Range(300, 451);
                    }

                    PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(Salbap, diaRan.ToString("f0"));

                    rdopen.RandoBoxInit(3, diaRan);

                    //PopUpObjectManager.GetInstance().ShowWarnnigProcess("국밥 " + diaRan + "그릇 획득!");

                }
                else // 유물
                {
                    rdopen.RandoBoxInit(525, 100);

                    //GameObject.Find("GatChaManager").GetComponent<GatChaManager>().GatCha_Shop();
                }

                break;


            case 6:
                // B 골드박스
                if (motherRan < 10f)      //다이아
                {
                    int diaRan = 10;

                    if (childRan < 50f)
                    {
                        diaRan += Random.Range(0, 50);
                    }
                    else if (childRan < 85f)
                    {
                        diaRan += Random.Range(50, 200);
                    }
                    else
                    {
                        diaRan += Random.Range(200, 291);
                    }

                    //PlayerPrefsManager.GetInstance().diamond = dts.AddStringDouble(dia, diaRan.ToString());

                    var ddd = PlayerPrefs.GetFloat("dDiamond") + diaRan;
                    PlayerPrefs.SetFloat("dDiamond", ddd);

                    rdopen.RandoBoxInit(1, diaRan);


                    //PopUpObjectManager.GetInstance().ShowWarnnigProcess("다이아 " + diaRan + "개 획득!");


                }
                else if (motherRan < 20f) //국밥
                {
                    int diaRan = 100;

                    if (childRan < 50f)
                    {
                        diaRan += Random.Range(0, 500);
                    }
                    else if (childRan < 85f)
                    {
                        diaRan += Random.Range(500, 2000);
                    }
                    else
                    {
                        diaRan += Random.Range(2000, 2901);
                    }

                    PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(gupbap, diaRan.ToString("f0"));

                    rdopen.RandoBoxInit(2, diaRan);

                    //PopUpObjectManager.GetInstance().ShowWarnnigProcess("국밥 " + diaRan + "그릇 획득!");

                }
                else if (motherRan < 30f) //키
                {
                    int diaRan = 0;

                    if (childRan < 50f)
                    {
                        diaRan += 1;
                    }
                    else if (childRan < 85f)
                    {
                        diaRan += Random.Range(2, 4);
                    }
                    else
                    {
                        diaRan += Random.Range(4, 6);
                    }

                    PlayerPrefsManager.GetInstance().key += diaRan;

                    rdopen.RandoBoxInit(0, diaRan);

                    //PopUpObjectManager.GetInstance().ShowWarnnigProcess("열쇠 " + diaRan + "개 획득!");

                }
                else if (motherRan < 40f) //쌀밥
                {
                    int diaRan = 50;

                    if (childRan < 50f)
                    {
                        diaRan += Random.Range(0, 250);
                    }
                    else if (childRan < 85f)
                    {
                        diaRan += Random.Range(250, 1000);
                    }
                    else
                    {
                        diaRan += Random.Range(1000, 1451);
                    }

                    PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(Salbap, diaRan.ToString("f0"));

                    rdopen.RandoBoxInit(3, diaRan);

                    //PopUpObjectManager.GetInstance().ShowWarnnigProcess("국밥 " + diaRan + "그릇 획득!");

                }
                else // 유물
                {

                    rdopen.RandoBoxInit(625, 5);

                    //GameObject.Find("GatChaManager").GetComponent<GatChaManager>().GatCha_Shop();
                }

                break;

            case 7:
                // A 골드박스
                if (motherRan < 5f)      //다이아
                {
                    int diaRan = 10;

                    if (childRan < 50f)
                    {
                        diaRan += Random.Range(0, 50);
                    }
                    else if (childRan < 85f)
                    {
                        diaRan += Random.Range(50, 300);
                    }
                    else
                    {
                        diaRan += Random.Range(300, 491);
                    }

                    //PlayerPrefsManager.GetInstance().diamond = dts.AddStringDouble(dia, diaRan.ToString());

                    var ddd = PlayerPrefs.GetFloat("dDiamond") + diaRan;
                    PlayerPrefs.SetFloat("dDiamond", ddd);

                    rdopen.RandoBoxInit(1, diaRan);
                    
                    //PopUpObjectManager.GetInstance().ShowWarnnigProcess("다이아 " + diaRan + "개 획득!");


                }
                else if (motherRan < 10f) //국밥
                {
                    int diaRan = 100;

                    if (childRan < 50f)
                    {
                        diaRan += Random.Range(0, 500);
                    }
                    else if (childRan < 85f)
                    {
                        diaRan += Random.Range(500, 3000);
                    }
                    else
                    {
                        diaRan += Random.Range(3000, 4901);
                    }

                    PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(gupbap, diaRan.ToString("f0"));

                    rdopen.RandoBoxInit(2, diaRan);


                    //PopUpObjectManager.GetInstance().ShowWarnnigProcess("국밥 " + diaRan + "그릇 획득!");

                }
                else if (motherRan < 15f) //키
                {
                    int diaRan = 1;

                    if (childRan < 50f)
                    {
                        diaRan += Random.Range(0, 4);
                    }
                    else if (childRan < 85f)
                    {
                        diaRan += Random.Range(4, 7);
                    }
                    else
                    {
                        diaRan += Random.Range(7, 10);
                    }

                    PlayerPrefsManager.GetInstance().key += diaRan;

                    rdopen.RandoBoxInit(0, diaRan);

                    //PopUpObjectManager.GetInstance().ShowWarnnigProcess("열쇠 " + diaRan + "개 획득!");

                }
                else if (motherRan < 20f) //쌀밥
                {
                    int diaRan = 50;

                    if (childRan < 50f)
                    {
                        diaRan += Random.Range(0, 450);
                    }
                    else if (childRan < 85f)
                    {
                        diaRan += Random.Range(450, 1450);
                    }
                    else
                    {
                        diaRan += Random.Range(1450, 2451);
                    }

                    PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(Salbap, diaRan.ToString("f0"));

                    rdopen.RandoBoxInit(3, diaRan);

                    //PopUpObjectManager.GetInstance().ShowWarnnigProcess("국밥 " + diaRan + "그릇 획득!");

                }
                else // 유물
                {
                    rdopen.RandoBoxInit(725, 10);

                    //GameObject.Find("GatChaManager").GetComponent<GatChaManager>().GatCha_Shop();

                }

                break;


        }

        UserWallet.GetInstance().ShowAllMoney();
    }




    ///// <summary>
    ///// 랜덤 골드 상자 뽑기.
    ///// </summary>
    //void InitChanFunc(float[] _data)
    //{
    //    //float[] _data = new float[200];
    //    int MAX = _data.Length;

    //    for (int i = 0; i < MAX; i++)
    //    {
    //        _data[i] = Random.Range(10f, 10000f);
    //    }

    //    float tmp = 0;
    //    float[] _list = new float[MAX];

    //    for (int i = 0; i < MAX; i++)
    //    {
    //        tmp += _data[i];
    //        _list[i] = tmp;
    //    }

    //    float[] result = _list;

    //    for (int i = 0; i < MAX; i++)
    //    {
    //        result[i] = _list[i] / tmp;
    //    }

    //    ChanOutput(result, _data);
    //}

    //void ChanOutput(float[] _data, float[] _rawData)
    //{

    //    for (int i = 0; i < _data.Length; i++)
    //    {
    //        float rand = Random.Range(0, 1f);

    //        if (rand < _data[i])
    //        {
    //            Debug.LogWarning("rawData : " + _rawData[i]);
    //        }
    //        else
    //        {
    //            Debug.LogWarning("rand : " + rand);
    //        }
    //    }
    //}




    /// <summary>
    /// 골드 100배 온.
    /// </summary>
    public void GetNormalGold()
    {
        // 최근 획득 골드
        var gettingGold = PlayerPrefsManager.GetInstance().PlayerDPS;
        //골드 100배를 곱해 준다.
        gettingGold = dts.multipleStringDouble(gettingGold, 200f);

        var gold = PlayerPrefsManager.GetInstance().gold;
        var result = dts.AddStringDouble(gold, gettingGold);

        PlayerPrefsManager.GetInstance().gold = result;
        UserWallet.GetInstance().ShowUserGold();
        UserWallet.GetInstance().ShowUserDia();
    }

    /// <summary>
    /// 골드 100배 온.
    /// </summary>
    public void ViewNormalGold()
    {
        // 최근 획득 골드
        var gettingGold = PlayerPrefsManager.GetInstance().PlayerDPS;
        gettingGold = dts.multipleStringDouble(gettingGold, 200f);

        Gold100Text.text = UserWallet.GetInstance().SeetheNatural(double.Parse(gettingGold)) + "골드";
    }





}


