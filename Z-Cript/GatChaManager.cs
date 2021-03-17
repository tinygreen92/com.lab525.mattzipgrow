using UnityEngine;
using UnityEngine.UI;

public class GatChaManager : MonoBehaviour
{
    DoubleToStringNum dts = new DoubleToStringNum();
    [Header("- 팝업 내용 매니저")]
    public ArtifactPopManager artifactPopManager;

    [Header("- 본체")]
    public Text Arti01_Lv;
    public Text Arti02_Lv;
    public Text Arti03_Lv;
    public Text Arti04_Lv;
    public Text Arti05_Lv;
    public Text Arti06_Lv;
    public Text Arti07_Lv;
    public Text Arti08_Lv;
    public Text Arti09_Lv;
    public Text Arti10_Lv;
    //
    public Text Arti11_Lv;
    public Text Arti12_Lv;
    public Text Arti13_Lv;
    public Text Arti14_Lv;
    public Text Arti15_Lv;
    public Text Arti16_Lv;
    public Text Arti17_Lv;



    public Text Arti01_Effect;
    public Text Arti02_Effect;
    public Text Arti03_Effect;
    public Text Arti04_Effect;
    public Text Arti05_Effect;
    public Text Arti06_Effect;
    public Text Arti07_Effect;
    public Text Arti08_Effect;
    public Text Arti09_Effect;
    public Text Arti10_Effect;
    //
    public Text Arti11_Effect;
    public Text Arti12_Effect;
    public Text Arti13_Effect;
    public Text Arti14_Effect;
    public Text Arti15_Effect;
    public Text Arti16_Effect;
    public Text Arti17_Effect;



    [Header("- 버튼 커버 이미지")]
    public GameObject Gat_01;
    public GameObject Gat_11;


    private readonly int I_Arti_PunchTouch              = 10;
    private readonly int I_Arti_Mattzip                      = 1000;
    private readonly int I_Arti_HP                              = 1000;
    private readonly int I_Arti_GroggyTouch             = 49;
    private readonly int I_Arti_GAL                              = 100;
    private readonly int I_Arti_DefenceTime             = 100;
    private readonly int I_Arti_GoldBox                  = 300;
    private readonly int I_Arti_OffGold                  = 500;
    private readonly int I_Arti_MuganTime               = 300;
    private readonly int I_Arti_AttackPower          = 999;
    private readonly int I_Arti_GoldPer                      = 1000;
    private readonly int I_Arti_LuckyBoxPer             = 1000;
    private readonly int I_Arti_DefencePer          = 500;
    private readonly int I_Arti_GoldUpgrade             = 500;
    private readonly int I_Arti_InfiReword          = 1000;
    private readonly int I_Arti_MiniReword          = 1000;
    private readonly int I_Arti_MiniGameTime        = 300;

    /// <summary>
    /// Start 에서도 한 번 불러준다.
    /// </summary>
    public void RefreshItem()
    {
        var ppm = PlayerPrefsManager.GetInstance();

        if (ppm.Arti_PunchTouch >= 10) ppm.Arti_PunchTouch = 10;
        if (ppm.Arti_Mattzip >= 1000) ppm.Arti_Mattzip = 1000;
        if (ppm.Arti_HP >= 1000) ppm.Arti_HP = 1000;
        if (ppm.Arti_GroggyTouch >= 49) ppm.Arti_GroggyTouch = 49;
        if (ppm.Arti_GAL >= 100) ppm.Arti_GAL = 100;
        /// --------------------------------------------------------------- 신규 추가
        if (ppm.Arti_DefenceTime >= 100) ppm.Arti_DefenceTime = 100;
        if (ppm.Arti_GoldBox >= 300) ppm.Arti_GoldBox = 300;
        if (ppm.Arti_OffGold >= 500) ppm.Arti_OffGold = 500;

        if (ppm.Arti_MuganTime >= 300) ppm.Arti_MuganTime = 300;
        if (ppm.Arti_AttackPower >= 999) ppm.Arti_AttackPower = 999;

        /// --------------------------------------------------------------- 신규 추가 0608
        if (ppm.Arti_GoldPer >= 1000) ppm.Arti_GoldPer = 1000;
        if (ppm.Arti_LuckyBoxPer >= 1000) ppm.Arti_LuckyBoxPer = 1000;
        if (ppm.Arti_DefencePer >= 500) ppm.Arti_DefencePer = 500;
        if (ppm.Arti_GoldUpgrade >= 500) ppm.Arti_GoldUpgrade = 500;
        if (ppm.Arti_InfiReword >= 1000) ppm.Arti_InfiReword = 1000;
        if (ppm.Arti_MiniReword >= 1000) ppm.Arti_MiniReword = 1000;
        if (ppm.Arti_MiniGameTime >= 300) ppm.Arti_MiniGameTime = 300;

        Arti01_Lv.text = "Lv."+ ppm.Arti_PunchTouch + "  ( Max Lv. 10 )";
        Arti02_Lv.text = "Lv."+ ppm.Arti_Mattzip + "  ( Max Lv. 1000 )";
        Arti03_Lv.text = "Lv."+ ppm.Arti_HP + "  ( Max Lv. 1000 )";
        Arti04_Lv.text = "Lv."+ ppm.Arti_GroggyTouch + "  ( Max Lv. 49 )";

        Arti05_Lv.text = "Lv."+ ppm.Arti_GAL + "  ( Max Lv. 100 )";

        Arti01_Effect.text = "초당 터치횟수 "+ ppm.Arti_PunchTouch + "회 증가 ( 레벨당 1회 증가 )";
        Arti02_Effect.text = "맷집 "+string.Format("{0:F1}", ppm.Arti_Mattzip * 0.1f) + "% 증가 ( 레벨당 0.1% 증가 )";
        Arti03_Effect.text = "체력 "+ string.Format("{0:F1}", ppm.Arti_HP * 0.1f) + "% 증가 ( 레벨당 0.1% 증가 )";

        float result = ppm.Arti_GroggyTouch * 0.1f;
        Arti04_Effect.text = "그로기 상태 "+ string.Format("{0:F1}", result) + "초 짧아짐 ( 레벨당 0.1초 감소 )";
        //
        result = ppm.Arti_GAL * 0.1f;
        Arti05_Effect.text = "국밥 버프 시간 "+ string.Format("{0:F1}", result) + "초 증가 ( 레벨당 0.1초 증가 )";

        /// --------------------------------------------------------------- 신규 추가
        Arti06_Lv.text = "Lv." + ppm.Arti_DefenceTime + "  ( Max Lv. 100 )";
        Arti07_Lv.text = "Lv." + ppm.Arti_GoldBox + "  ( Max Lv. 300 )";
        Arti08_Lv.text = "Lv." + ppm.Arti_OffGold + "  ( Max Lv. 500 )";

        Arti09_Lv.text = "Lv." + ppm.Arti_MuganTime + "  ( Max Lv. 300 )";
        Arti10_Lv.text = "Lv." + ppm.Arti_AttackPower + "  ( Max Lv. 999 )";


        result = ppm.Arti_DefenceTime * 0.1f;
        Arti06_Effect.text = "방어전 시간 " + result + "초 감소 ( 레벨당 0.1초 감소 )";
        result = ppm.Arti_GoldBox * 0.1f;
        Arti07_Effect.text = "선물 상자 등장 확률 " + string.Format("{0:F1}", result) + "% 증가 ( 레벨당 0.1% 증가 )";
        Arti08_Effect.text = "방치 재화 획득량 " + ppm.Arti_OffGold + "% 증가 ( 레벨당 1% 증가 )";

        result = ppm.Arti_MuganTime * 0.1f;
        Arti09_Effect.text = "무한의 탑 시간 " + result + "초 증가 ( 레벨당 0.1초 증가 )";
        Arti10_Effect.text = "공격력 " + string.Format("{0:F1}", ppm.Arti_AttackPower * 0.1f) + "% 증가 ( 레벨당 0.1% 증가 )";


        /// -----------------------------------------
        /// 

        /// --------------------------------------------------------------- 신규 추가 0608

        Arti11_Lv.text = "Lv." + ppm.Arti_GoldPer + "  ( Max Lv. 1000 )";
        Arti12_Lv.text = "Lv." + ppm.Arti_LuckyBoxPer + "  ( Max Lv. 1000 )";
        Arti13_Lv.text = "Lv." + ppm.Arti_DefencePer + "  ( Max Lv. 500 )";
        Arti14_Lv.text = "Lv." + ppm.Arti_GoldUpgrade + "  ( Max Lv. 500 )";
        Arti15_Lv.text = "Lv." + ppm.Arti_InfiReword + "  ( Max Lv. 1000 )";
        Arti16_Lv.text = "Lv." + ppm.Arti_MiniReword + "  ( Max Lv. 1000 )";
        Arti17_Lv.text = "Lv." + ppm.Arti_MiniGameTime + "  ( Max Lv. 300 )";

        result = ppm.Arti_GoldPer * 0.5f;
        Arti11_Effect.text = "골드 획득량 " + string.Format("{0:F1}", result) + "% 증가 ( 레벨당 0.5% 증가 )";
        result = ppm.Arti_GoldPer * 0.5f;
        Arti12_Effect.text = "선물상자 보상 획득량 " + string.Format("{0:F1}", result) + "% 증가 ( 레벨당 0.5% 증가 )";
        result = ppm.Arti_DefencePer * 0.1f;
        Arti13_Effect.text = "방어전 대미지 " + string.Format("{0:F1}", result) + "% 감소 ( 레벨당 0.1% 감소 )";
        result = ppm.Arti_GoldUpgrade * 0.1f;
        Arti14_Effect.text = "골드 업그레이드 비용 " + string.Format("{0:F1}", result) + "% 감소 ( 레벨당 0.1% 감소 )";
        result = ppm.Arti_InfiReword * 0.5f;
        Arti15_Effect.text = "무한 버티기 국밥 획득량 " + string.Format("{0:F1}", result) + "% 증가 ( 레벨당 0.5% 증가 )";
        result = ppm.Arti_MiniReword * 0.5f;
        Arti16_Effect.text = "미니게임 쌀밥 획득량 " + string.Format("{0:F1}", result) + "% 증가 ( 레벨당 0.5% 증가 )";
        result = ppm.Arti_MiniGameTime * 0.1f;
        Arti17_Effect.text = "미니게임 시간 " + string.Format("{0:F1}", result) + "초 증가 ( 레벨당 0.1초 증가 )";

        // 증가된 값 = 기본값 + (기본값 X (증가된 확률 (100%) X 0.01))
        // 예) 체력 100 일때 100% 증가하면? 100 + (100 * (100 * 0.01)) = 200 (100% 증가 = 2배 증가)
        // 예) 체력 100 일때 1000% 증가하면? 100 + (100 * (1000 * 0.01)) = 1100 (1000% 증가 = 11배 증가)

        /// -----------------------------------------

        ppm.MattzipArtif = ppm.Arti_Mattzip * 0.1f;
        ppm.Arti_MaxHP = ppm.Arti_HP < 1? "0" : (1.0d + (ppm.Arti_HP * 0.001f)).ToString();
        ppm.GroggyTouch = (5.0f - (ppm.Arti_GroggyTouch * 0.1f));
        ppm.AttackPunch = 1.0f +(ppm.Arti_AttackPower * 0.1f);
        
        ///
        //ppm.LuckyProb = (ppm.Arti_GoldBox * 0.1f);
        PlayerPrefs.SetFloat("LuckyProb", (1.0f + (ppm.Arti_GoldBox * 0.1f)));
        PlayerPrefs.Save();

        //

        if (ppm.Arti_PunchTouch >= I_Arti_PunchTouch &&
            ppm.Arti_Mattzip >= I_Arti_Mattzip &&
            ppm.Arti_HP >= I_Arti_HP &&
            ppm.Arti_GroggyTouch >= I_Arti_GroggyTouch &&
            ppm.Arti_GAL >= I_Arti_GAL &&
            ppm.Arti_DefenceTime >= I_Arti_DefenceTime &&
            ppm.Arti_GoldBox >= I_Arti_GoldBox &&
            ppm.Arti_OffGold >= I_Arti_OffGold &&
            ppm.Arti_MuganTime >= I_Arti_MuganTime &&
            ppm.Arti_AttackPower >= I_Arti_AttackPower &&
            ppm.Arti_GoldPer >= I_Arti_GoldPer &&
            ppm.Arti_LuckyBoxPer >= I_Arti_LuckyBoxPer &&
            ppm.Arti_DefencePer >= I_Arti_DefencePer &&
            ppm.Arti_GoldUpgrade >= I_Arti_GoldUpgrade &&
            ppm.Arti_InfiReword >= I_Arti_InfiReword &&
            ppm.Arti_MiniReword >= I_Arti_MiniReword &&
            ppm.Arti_MiniGameTime >= I_Arti_MiniGameTime
            )
        {
            // 뽑기 버튼 막기
            getGatChaImg[0].SetActive(true);
            getGatChaImg[1].SetActive(true);
        }

        UserWallet.GetInstance().ShowAllMoney();
    }

    // 탭 볼 때마다 새로고침 회색 <-> 주황.
    public void GatchaUpdate()
    {
        if (!DiaPass("100"))
        {
            Gat_01.SetActive(true);
        }
        else
        {
            Gat_01.SetActive(false);
        }

        //

        if (!DiaPass("990"))
        {
            Gat_11.SetActive(true);
        }
        else
        {
            Gat_11.SetActive(false);
        }

        RefreshItem();
    }

    [Header(" - 랜덤박스 에서 호출 유물")]
    public ArtifactPopManager ranArtiPopManager;

    /// <summary>
    /// 골드 랜덤 박스에서 호출해준다.
    /// </summary>
    /// <returns>true 면 뽑기 ㄴㄴ false면 유적 계속 뽑아</returns>
    public bool GatCha_Shop()
    {
        var ppm = PlayerPrefsManager.GetInstance();

        /// TODO : 모든 유물이 만렙이면 어캄?
        if (ppm.Arti_PunchTouch >= I_Arti_PunchTouch &&
            ppm.Arti_Mattzip >= I_Arti_Mattzip &&
            ppm.Arti_HP >= I_Arti_HP &&
            ppm.Arti_GroggyTouch >= I_Arti_GroggyTouch &&
            ppm.Arti_GAL >= I_Arti_GAL &&
            ppm.Arti_DefenceTime >= I_Arti_DefenceTime &&
            ppm.Arti_GoldBox >= I_Arti_GoldBox &&
            ppm.Arti_OffGold >= I_Arti_OffGold &&
            ppm.Arti_MuganTime >= I_Arti_MuganTime &&
            ppm.Arti_AttackPower >= I_Arti_AttackPower &&
            ppm.Arti_GoldPer >= I_Arti_GoldPer &&
            ppm.Arti_LuckyBoxPer >= I_Arti_LuckyBoxPer &&
            ppm.Arti_DefencePer >= I_Arti_DefencePer &&
            ppm.Arti_GoldUpgrade >= I_Arti_GoldUpgrade &&
            ppm.Arti_InfiReword >= I_Arti_InfiReword &&
            ppm.Arti_MiniReword >= I_Arti_MiniReword &&
            ppm.Arti_MiniGameTime >= I_Arti_MiniGameTime
            )

        {
            // 뽑기 버튼 막기

            getGatChaImg[0].SetActive(true);
            getGatChaImg[1].SetActive(true);

            return true;
        }
        else
        {

            GoGoGatCha(0);
            ranArtiPopManager.SetTextAll(index, name, 0);

            return false;
        }

    }

    /// <summary>
    /// 1 가차 100원
    /// </summary>
    public void GatCha_1()
    {
        // 커버 이미지 활성화면 리턴
        if (Gat_01.activeSelf) return;

        var ppm = PlayerPrefsManager.GetInstance();

        /// TODO : 모든 유물이 만렙이면 어캄?
        if (ppm.Arti_PunchTouch >= I_Arti_PunchTouch &&
            ppm.Arti_Mattzip >= I_Arti_Mattzip &&
            ppm.Arti_HP >= I_Arti_HP &&
            ppm.Arti_GroggyTouch >= I_Arti_GroggyTouch &&
            ppm.Arti_GAL >= I_Arti_GAL &&
            ppm.Arti_DefenceTime >= I_Arti_DefenceTime &&
            ppm.Arti_GoldBox >= I_Arti_GoldBox &&
            ppm.Arti_OffGold >= I_Arti_OffGold &&
            ppm.Arti_MuganTime >= I_Arti_MuganTime &&
            ppm.Arti_AttackPower >= I_Arti_AttackPower &&
            ppm.Arti_GoldPer >= I_Arti_GoldPer &&
            ppm.Arti_LuckyBoxPer >= I_Arti_LuckyBoxPer &&
            ppm.Arti_DefencePer >= I_Arti_DefencePer &&
            ppm.Arti_GoldUpgrade >= I_Arti_GoldUpgrade &&
            ppm.Arti_InfiReword >= I_Arti_InfiReword &&
            ppm.Arti_MiniReword >= I_Arti_MiniReword &&
            ppm.Arti_MiniGameTime >= I_Arti_MiniGameTime
            )

        {
            // 뽑기 버튼 막기

            getGatChaImg[0].SetActive(true);
            getGatChaImg[1].SetActive(true);


            return;
        }

        // 다이아 소모
        PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 100);
        UserWallet.GetInstance().ShowUserDia();

        GoGoGatCha(0);
        artifactPopManager.SetTextAll(index, name, 0);

        // 퀘스트
        PlayerPrefsManager.GetInstance().questInfo[0].daily_ArtiGatcha++;

        if (PlayerPrefsManager.GetInstance().questInfo[0].All_Gatcha < 1000)
        {
            PlayerPrefsManager.GetInstance().questInfo[0].All_Gatcha++;
        }

        //
        GatchaUpdate();
    }

    /// <summary>
    /// 11가차 990원
    /// </summary>
    public void GatCha_11()
    {
        // 커버 이미지 활성화면 리턴
        if (Gat_11.activeSelf) return;

        var ppm = PlayerPrefsManager.GetInstance();

        /// TODO : 모든 유물이 만렙이면 어캄?
        if (ppm.Arti_PunchTouch >= I_Arti_PunchTouch &&
            ppm.Arti_Mattzip >= I_Arti_Mattzip &&
            ppm.Arti_HP >= I_Arti_HP &&
            ppm.Arti_GroggyTouch >= I_Arti_GroggyTouch &&
            ppm.Arti_GAL >= I_Arti_GAL &&
            ppm.Arti_DefenceTime >= I_Arti_DefenceTime &&
            ppm.Arti_GoldBox >= I_Arti_GoldBox &&
            ppm.Arti_OffGold >= I_Arti_OffGold &&
            ppm.Arti_MuganTime >= I_Arti_MuganTime &&
            ppm.Arti_AttackPower >= I_Arti_AttackPower &&
            ppm.Arti_GoldPer >= I_Arti_GoldPer &&
            ppm.Arti_LuckyBoxPer >= I_Arti_LuckyBoxPer &&
            ppm.Arti_DefencePer >= I_Arti_DefencePer &&
            ppm.Arti_GoldUpgrade >= I_Arti_GoldUpgrade &&
            ppm.Arti_InfiReword >= I_Arti_InfiReword &&
            ppm.Arti_MiniReword >= I_Arti_MiniReword &&
            ppm.Arti_MiniGameTime >= I_Arti_MiniGameTime
            )

        {
            // 뽑기 버튼 막기

            getGatChaImg[0].SetActive(true);
            getGatChaImg[1].SetActive(true);


            return;
        }


        // 다이아 소모
        //var dia = PlayerPrefsManager.GetInstance().diamond;
        //var result = dts.SubStringDouble(dia, "990");
        //PlayerPrefsManager.GetInstance().diamond = result;

        PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond") - 990);
        UserWallet.GetInstance().ShowUserDia();

        // 랜덤시드 부여하기
        float temp = Time.time * 100f;
        Random.InitState((int)temp);
        for (int i = 1; i < 12; i++)
        {
            GoGoGatCha(i);
        }

        // 팝업 텍스트 채워주기. 11번.
        artifactPopManager.SetTextAll(index, name, 1);

        // 퀘스트
        PlayerPrefsManager.GetInstance().questInfo[0].daily_ArtiGatcha += 11;

        if (PlayerPrefsManager.GetInstance().questInfo[0].All_Gatcha < 1000)
        {
            PlayerPrefsManager.GetInstance().questInfo[0].All_Gatcha += 11;
        }

        if (PlayerPrefsManager.GetInstance().questInfo[0].All_Gatcha >= 1000)
        {
            PlayerPrefsManager.GetInstance().questInfo[0].All_Gatcha = 1000;
        }

        //
        GatchaUpdate();

    }

    [Header("-뽑기버튼 MAX")]
    public GameObject[] getGatChaImg;

    new string[] name = new string[12];
    int[] index = new int[12];
    /// <summary>
    /// 유물 효과 적용로직
    /// </summary>
    /// <param name="seed">몇번 뽑을지?</param>
    public void GoGoGatCha(int seed)
    {
        var ppm = PlayerPrefsManager.GetInstance();

        HELL:

        // 팝업에 전달해줄 값.
        name[seed] = "";
        index[seed] = 0;

        string targetDia = string.Empty;

        float artiGatcha = Random.Range(0f, 100f);

        if (artiGatcha < 1f)
        {
            /// (int)런닝화- 초당 터치 횟수 증가          Arti_PunchTouch
            ppm.Arti_PunchTouch++;
            // 맥스레벨이면 다시 돌려
            if (ppm.Arti_PunchTouch > I_Arti_PunchTouch)
            {
                ppm.Arti_PunchTouch = I_Arti_PunchTouch;
                goto HELL;
            } 

            name[seed] = "\'런닝화\'를 ";
            index[seed] = 0;

        }
        else if (artiGatcha < 15f)
        {
            /// (int)헤드기어- 맷집증가                   Arti_Mattzip
            ppm.Arti_Mattzip++;
            // 맥스레벨이면 다시 돌려
            if (ppm.Arti_Mattzip > I_Arti_Mattzip)
            {
                ppm.Arti_Mattzip = I_Arti_Mattzip;
                goto HELL;
            }
            name[seed] = "\'헤드기어\'를 ";
            index[seed] = 1;

        }
        else if (artiGatcha < 29f)
        {
            /// (int)줄넘기- 체력증가                     Arti_HP
            ppm.Arti_HP++;
            // 맥스레벨이면 다시 돌려
            if (ppm.Arti_HP > I_Arti_HP)
            {
                ppm.Arti_HP = I_Arti_HP;
                goto HELL;
            }
            name[seed] = "\'줄넘기\'를 ";
            index[seed] = 2;


        }
        else if (artiGatcha < 34f)
        {
            /// (int)물총- 그로기 상태 짧아짐             Arti_GroggyTouch
            ppm.Arti_GroggyTouch++;
            // 맥스레벨이면 다시 돌려
            if (ppm.Arti_GroggyTouch > I_Arti_GroggyTouch)
            {
                ppm.Arti_GroggyTouch = I_Arti_GroggyTouch;
                goto HELL;
            }
            name[seed] = "\'물총\'을 ";
            index[seed] = 3;


        }
        else if (artiGatcha < 37f)
        {
            /// (int)국밥- 스킬회복량 증가                Arti_GAL
            ppm.Arti_GAL++;
            // 맥스레벨이면 다시 돌려
            if (ppm.Arti_GAL > I_Arti_GAL)
            {
                ppm.Arti_GAL = I_Arti_GAL;
                goto HELL;
            }
            name[seed] = "\'국밥\'을 ";
            index[seed] = 4;

        }
        else if (artiGatcha < 42f)
        {
            /// (int)모래시계- 방어전 시간 감소                Arti_DefenceTime
            ppm.Arti_DefenceTime++;
            // 맥스레벨이면 다시 돌려
            if (ppm.Arti_DefenceTime > I_Arti_DefenceTime)
            {
                ppm.Arti_DefenceTime = I_Arti_DefenceTime;
                goto HELL;
            }
            name[seed] = "\'모래시계\'를 ";
            index[seed] = 5;

        }
        else if (artiGatcha < 47f)
        {
            /// (int)선물상자- 선물 상자 등장확률 증가                Arti_GoldBox
            ppm.Arti_GoldBox++;
            // 맥스레벨이면 다시 돌려
            if (ppm.Arti_GoldBox > I_Arti_GoldBox)
            {
                ppm.Arti_GoldBox = I_Arti_GoldBox;
                goto HELL;
            }
            name[seed] = "\'선물상자\'를 ";
            index[seed] = 6;

        }
        else if (artiGatcha <= 50f)
        {
            /// (int)머니건- 방치 골드 획득량 증가                Arti_OffGold
            ppm.Arti_OffGold++;
            // 맥스레벨이면 다시 돌려
            if (ppm.Arti_OffGold > I_Arti_OffGold)
            {
                ppm.Arti_OffGold = I_Arti_OffGold;
                goto HELL;
            }
            name[seed] = "\'머니건\'을 ";
            index[seed] = 7;

        }
        else if (artiGatcha <= 53f)
        {
            /// (int)땡땡 종 - 무한의 탑 시간 증가                Arti_MuganTime
            ppm.Arti_MuganTime++;
            // 맥스레벨이면 다시 돌려
            if (ppm.Arti_MuganTime > I_Arti_MuganTime)
            {
                ppm.Arti_MuganTime = I_Arti_MuganTime;
                goto HELL;
            }
            name[seed] = "\'땡땡 종\'을 ";
            index[seed] = 8;

        }
        else if (artiGatcha <= 67f)
        {
            /// (int)샌드백 - 공격력 증가                Arti_AttackPower
            ppm.Arti_AttackPower++;
            // 맥스레벨이면 다시 돌려
            if (ppm.Arti_AttackPower > I_Arti_AttackPower)
            {
                ppm.Arti_AttackPower = I_Arti_AttackPower;
                goto HELL;
            }
            name[seed] = "\'샌드백\'을 ";
            index[seed] = 9;

        }
        ///
        ///
        ///                 신규 유물 추가 0608
        ///
        ///
        else if (artiGatcha <= 72f)
        {
            /// (int)     -골드 획득 증가                
            ppm.Arti_GoldPer++;
            // 맥스레벨이면 다시 돌려
            if (ppm.Arti_GoldPer > I_Arti_GoldPer)
            {
                ppm.Arti_GoldPer = I_Arti_GoldPer;
                goto HELL;
            }
            name[seed] = "\'돈주머니\'를 ";
            index[seed] = 10;

        }
        else if (artiGatcha <= 77f)
        {
            /// (int)       -타격 선물상자 획득량 증가      
            ppm.Arti_LuckyBoxPer++;
            // 맥스레벨이면 다시 돌려
            if (ppm.Arti_LuckyBoxPer > I_Arti_LuckyBoxPer)
            {
                ppm.Arti_LuckyBoxPer = I_Arti_LuckyBoxPer;
                goto HELL;
            }
            name[seed] = "\'돼지저금통\'을 ";
            index[seed] = 11;

        }
        else if (artiGatcha <= 80f)
        {
            /// (int)       -방어전 대미지 % 감소 
            ppm.Arti_DefencePer++;
            // 맥스레벨이면 다시 돌려
            if (ppm.Arti_DefencePer > I_Arti_DefencePer)
            {
                ppm.Arti_DefencePer = I_Arti_DefencePer;
                goto HELL;
            }
            name[seed] = "\'수건\'을 ";
            index[seed] = 12;

        }
        else if (artiGatcha <= 87f)
        {
            /// (int)       -골드 강화 비용 감소  
            ppm.Arti_GoldUpgrade++;
            // 맥스레벨이면 다시 돌려
            if (ppm.Arti_GoldUpgrade > I_Arti_GoldUpgrade)
            {
                ppm.Arti_GoldUpgrade = I_Arti_GoldUpgrade;
                goto HELL;
            }
            name[seed] = "\'할인행사\'를 ";
            index[seed] = 13;

        }
        else if (artiGatcha <= 92f)
        {
            /// (int)       -버티기 보상 강화       
            ppm.Arti_InfiReword++;
            // 맥스레벨이면 다시 돌려
            if (ppm.Arti_InfiReword > I_Arti_InfiReword)
            {
                ppm.Arti_InfiReword = I_Arti_InfiReword;
                goto HELL;
            }
            name[seed] = "\'뚝배기\'를 ";
            index[seed] = 14;

        }
        else if (artiGatcha <= 97f)
        {
            /// (int)       -미니게임 보상 강화  
            ppm.Arti_MiniReword++;
            // 맥스레벨이면 다시 돌려
            if (ppm.Arti_MiniReword > I_Arti_MiniReword)
            {
                ppm.Arti_MiniReword = I_Arti_MiniReword;
                goto HELL;
            }
            name[seed] = "\'밥솥\'을 ";
            index[seed] = 15;

        }
        else if (artiGatcha <= 100.0f)
        {
            /// (int)       -미니게임 시간 증가       
            ppm.Arti_MiniGameTime++;
            // 맥스레벨이면 다시 돌려
            if (ppm.Arti_MiniGameTime > I_Arti_MiniGameTime)
            {
                ppm.Arti_MiniGameTime = I_Arti_MiniGameTime;
                goto HELL;
            }
            name[seed] = "\'시계\'를 ";
            index[seed] = 16;

        }

        // 아이템 효과 적용 바로.
        RefreshItem();
    }


    /// <summary>
    /// 다이아몬드 소모 로직 넣었을때 통과하냐??
    /// </summary>
    /// <returns></returns>
    bool DiaPass(string _diaAmoung)
    {
        //var dia = PlayerPrefsManager.GetInstance().diamond;
        //var result = dts.SubStringDouble(dia, _diaAmoung);

        string result = "";

        if (PlayerPrefs.GetFloat("dDiamond") - float.Parse(_diaAmoung) < 0)
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



}
