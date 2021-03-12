///
/// https://blueasa.tistory.com/2348
///
using UnityEngine;
using UnityEngine.UI;

public class TapToSpawnLimit : MonoBehaviour
{
    private const int MAX_SPEC_CNT = 20000;
    [Header("+PunchObjectPools")]
    public Transform[] OP;
    public Transform PunchPos;


    // 초당 최대 액션 횟수
    float fInputActionsPerSecond = 5f;
    // 1초에 카운트 할것.
    private float fUserClickedCnt = 0f;

    /// <summary>
    /// 맨땅에 클릭
    /// </summary>
    public void ClickedSkyBox()
    {
        // 그로기 상태면 리턴
        if (PlayerPrefsManager.GetInstance().isGroggy) return;
        /// 연속클릭 딜레이
        if (fUserClickedCnt != 0f && Time.unscaledTime < fUserClickedCnt) return;

        ClickedSomeThing();
        ComputeNextAction();
    }
    void ComputeNextAction()
    {
        /// 초당터치 횟수 증가
        float tmp = fInputActionsPerSecond + PlayerPrefsManager.GetInstance().Arti_PunchTouch;
        // 국밥 스킬 발동하면 공속 1.5배
        if (PlayerPrefsManager.GetInstance().isGupSpeed)
        {
            tmp *= 1.5f;
        }
        else
        {
            tmp *= 1.0f;
        }

        fUserClickedCnt = Time.unscaledTime + (1f / tmp);
    }

    public void ClickedDefenceBody()
    {

        // 그로기 상태면 리턴
        if (PlayerPrefsManager.GetInstance().isGroggy) return;

        if (fUserClickedCnt != 0f && Time.unscaledTime < fUserClickedCnt) return;

        InDefenceModePunch();
        ComputeNext();
    }
    void ComputeNext()
    {
        fUserClickedCnt = Time.unscaledTime + (1f / 15f);
    }


    public void ClickedInfiniti()
    {
        // 그로기 상태면 리턴
        if (PlayerPrefsManager.GetInstance().isGroggy) return;

        if (fUserClickedCnt != 0f && Time.unscaledTime < fUserClickedCnt) return;

        InFinitiModePunch();
        ComputeNextNext();
    }
    void ComputeNextNext()
    {
        fUserClickedCnt = Time.unscaledTime + (1f / 10f);
    }

    /// <summary>
    /// 무한의 탑 파이어볼 발싸
    /// </summary>
    public void ClickedMuGanTop()
    {
        if (fUserClickedCnt != 0f && Time.unscaledTime < fUserClickedCnt) return;

        MUGAN_Punch();
        ComputeNextNextNext();
    }
    void ComputeNextNextNext()
    {
        /// 펀치는 점점 빨라져야 해
        float tmp = 0.5f;
        fUserClickedCnt = Time.unscaledTime + (1f / tmp);
    }





    GameObject tmp;
    GameObject bullet_Left;
    GameObject bullet_Right;

    [Header("-체력 게이지")]
    public Image FilledHP;
    public Image DefenceHP;
    [Header("-맷집 게이지")]
    public Image MattGauge;
    [Header("-꾸짖갈 게이지")]
    public Image SkillGauge;
    [Header("-빵 버튼")]
    public Button BreadBtn;



    bool isPunchSwitch;

    int punchIndex = 0;

    /// <summary>
    /// 외부에서 펀치 인덱스 바꿔줌
    /// </summary>
    /// <param name="_index"></param>
    public void PunchIndexUpdate(int _index)
    {
        punchIndex = _index;
    }

    /// <summary>
    /// 액션이 허용될 때의 동작.
    /// -> 화면 광클할때 동작
    /// </summary>
    public void ClickedSomeThing()
    {
        // 튜토리얼 진행중이면
        if (!PlayerPrefsManager.GetInstance().isFristGameStart)
        {
            // 튜토리얼 10회 카운트 증가
            PlayerPrefsManager.GetInstance().TurtorialCount++;
        }

        // 그로기 상태면 리턴
        if (PlayerPrefsManager.GetInstance().isGroggy) return;
        /// 펀치 모양 프리팹
        tmp = OP[punchIndex].GetComponent<Lean.Pool.LeanGameObjectPool>().Prefab;
        /// 주먹 카운팅 누적 퀘스트
        CountingPunch(punchIndex);

        if (isPunchSwitch)
        {
            bullet_Left = Lean.Pool.LeanPool.Spawn(tmp, PunchPos.GetChild(0).position, PunchPos.GetChild(0).rotation);
            bullet_Left.GetComponent<BulletManager>().BulletInit();
            bullet_Left.GetComponent<BulletManager>().SetFillHP(FilledHP, MattGauge, SkillGauge, BreadBtn);
            isPunchSwitch = !isPunchSwitch;
        }
        else
        {
            bullet_Right = Lean.Pool.LeanPool.Spawn(tmp, PunchPos.GetChild(1).position, PunchPos.GetChild(1).rotation);
            bullet_Right.GetComponent<BulletManager>().BulletInit0();
            bullet_Right.GetComponent<BulletManager>().SetFillHP(FilledHP, MattGauge, SkillGauge, BreadBtn);
            isPunchSwitch = !isPunchSwitch;

        }
    }
    /// <summary>
    /// 누적 퀘스트 카운팅
    /// </summary>
    /// <param name="_index"></param>
    void CountingPunch(int _index)
    {
        var spec = PlayerPrefsManager.GetInstance().questInfo[0];
        var spec2 = PlayerPrefsManager.GetInstance().questInfo2[0];
        var spec3 = PlayerPrefsManager.GetInstance().questInfo3[0];
        var spec4 = PlayerPrefsManager.GetInstance().questInfo4[0];
        var spec5 = PlayerPrefsManager.GetInstance().questInfo5[0];

        long[] Spec = new long[]
        {
            spec.Pun_01,
            spec.Pun_02,
            spec.Pun_03,
            spec.Pun_04,
            spec.Pun_05,
            spec.Pun_06,
            spec.Pun_07,
            spec.Pun_08,
            spec.Pun_09,
            spec.Pun_10,
            spec.Pun_11,
            spec.Pun_12,
            spec.Pun_13,
            spec.Pun_14,
            spec.Pun_15,
            spec.Pun_16,
            spec.Pun_17,
            spec.Pun_18,
            spec.Pun_19,
            spec.Pun_20,
            spec.Pun_21,
            spec.Pun_22,
            spec.Pun_23,
            spec.Pun_24,
            spec.Pun_25,
            spec.Pun_26,
            spec.Pun_27,
            spec.Pun_28,
            spec.Pun_29,
            spec.Pun_30,
            spec.Pun_31,
            spec.Pun_32,
            spec.Pun_33,
            spec.Pun_34,
            spec.Pun_35,
            spec.Pun_36,
            spec.Pun_37,
            spec.Pun_38,
            spec.Pun_39,
            spec.Pun_40,
            spec.Pun_41,
            spec.Pun_42,
            spec.Pun_43,
            spec.Pun_44,
            spec.Pun_45,
            spec.Pun_46,
            spec.Pun_47,
            spec.Pun_48,
            spec.Pun_49,
            spec.Pun_50,
            spec2.Pun_51,
            spec2.Pun_52,
            spec2.Pun_53,
            spec2.Pun_54,
            spec2.Pun_55,
            spec2.Pun_56,
            spec2.Pun_57,
            spec2.Pun_58,
            spec2.Pun_59,
            spec2.Pun_60,
            spec2.Pun_61,
            spec2.Pun_62,
            spec2.Pun_63,
            spec2.Pun_64,
            spec2.Pun_65,
            spec2.Pun_66,
            spec2.Pun_67,
            spec2.Pun_68,
            spec2.Pun_69,
            spec2.Pun_70,
            spec3.Pun_71,
            spec3.Pun_72,
            spec3.Pun_73,
            spec3.Pun_74,
            spec3.Pun_75,
            spec3.Pun_76,
            spec3.Pun_77,
            spec3.Pun_78,
            spec3.Pun_79,
            spec3.Pun_80,
            spec4.Pun_81,
            spec4.Pun_82,
            spec4.Pun_83,
            spec4.Pun_84,
            spec4.Pun_85,
            spec4.Pun_86,
            spec4.Pun_87,
            spec4.Pun_88,
            spec4.Pun_89,
            spec4.Pun_90,
            spec5.Pun_91,
            spec5.Pun_92,
            spec5.Pun_93,
            spec5.Pun_94,
            spec5.Pun_95,
            spec5.Pun_96,
            spec5.Pun_97,
            spec5.Pun_98,
            spec5.Pun_99,
            spec5.Pun_100
        };

        if (Spec[_index] >= MAX_SPEC_CNT)
        {
            switch (_index)
            {
                case 0: PlayerPrefsManager.GetInstance().questInfo[0].Pun_01=MAX_SPEC_CNT; break;
                case 1: PlayerPrefsManager.GetInstance().questInfo[0].Pun_02=MAX_SPEC_CNT; break;
                case 2: PlayerPrefsManager.GetInstance().questInfo[0].Pun_03=MAX_SPEC_CNT; break;
                case 3: PlayerPrefsManager.GetInstance().questInfo[0].Pun_04=MAX_SPEC_CNT; break;
                case 4: PlayerPrefsManager.GetInstance().questInfo[0].Pun_05=MAX_SPEC_CNT; break;
                case 5: PlayerPrefsManager.GetInstance().questInfo[0].Pun_06=MAX_SPEC_CNT; break;
                case 6: PlayerPrefsManager.GetInstance().questInfo[0].Pun_07=MAX_SPEC_CNT; break;
                case 7: PlayerPrefsManager.GetInstance().questInfo[0].Pun_08=MAX_SPEC_CNT; break;
                case 8: PlayerPrefsManager.GetInstance().questInfo[0].Pun_09=MAX_SPEC_CNT; break;
                case 9: PlayerPrefsManager.GetInstance().questInfo[0].Pun_10= MAX_SPEC_CNT; break;
                case 10: PlayerPrefsManager.GetInstance().questInfo[0].Pun_11=MAX_SPEC_CNT; break;
                case 11: PlayerPrefsManager.GetInstance().questInfo[0].Pun_12=MAX_SPEC_CNT; break;
                case 12: PlayerPrefsManager.GetInstance().questInfo[0].Pun_13=MAX_SPEC_CNT; break;
                case 13: PlayerPrefsManager.GetInstance().questInfo[0].Pun_14=MAX_SPEC_CNT; break;
                case 14: PlayerPrefsManager.GetInstance().questInfo[0].Pun_15=MAX_SPEC_CNT; break;
                case 15: PlayerPrefsManager.GetInstance().questInfo[0].Pun_16=MAX_SPEC_CNT; break;
                case 16: PlayerPrefsManager.GetInstance().questInfo[0].Pun_17=MAX_SPEC_CNT; break;
                case 17: PlayerPrefsManager.GetInstance().questInfo[0].Pun_18=MAX_SPEC_CNT; break;
                case 18: PlayerPrefsManager.GetInstance().questInfo[0].Pun_19=MAX_SPEC_CNT; break;
                case 19: PlayerPrefsManager.GetInstance().questInfo[0].Pun_20=MAX_SPEC_CNT; break;
                case 20: PlayerPrefsManager.GetInstance().questInfo[0].Pun_21=MAX_SPEC_CNT; break;
                case 21: PlayerPrefsManager.GetInstance().questInfo[0].Pun_22=MAX_SPEC_CNT; break;
                case 22: PlayerPrefsManager.GetInstance().questInfo[0].Pun_23=MAX_SPEC_CNT; break;
                case 23: PlayerPrefsManager.GetInstance().questInfo[0].Pun_24=MAX_SPEC_CNT; break;
                case 24: PlayerPrefsManager.GetInstance().questInfo[0].Pun_25=MAX_SPEC_CNT; break;
                case 25: PlayerPrefsManager.GetInstance().questInfo[0].Pun_26=MAX_SPEC_CNT; break;
                case 26: PlayerPrefsManager.GetInstance().questInfo[0].Pun_27=MAX_SPEC_CNT; break;
                case 27: PlayerPrefsManager.GetInstance().questInfo[0].Pun_28=MAX_SPEC_CNT; break;
                case 28: PlayerPrefsManager.GetInstance().questInfo[0].Pun_29=MAX_SPEC_CNT; break;
                case 29: PlayerPrefsManager.GetInstance().questInfo[0].Pun_30=MAX_SPEC_CNT; break;
                case 30: PlayerPrefsManager.GetInstance().questInfo[0].Pun_31=MAX_SPEC_CNT; break;
                case 31: PlayerPrefsManager.GetInstance().questInfo[0].Pun_32=MAX_SPEC_CNT; break;
                case 32: PlayerPrefsManager.GetInstance().questInfo[0].Pun_33=MAX_SPEC_CNT; break;
                case 33: PlayerPrefsManager.GetInstance().questInfo[0].Pun_34=MAX_SPEC_CNT; break;
                case 34: PlayerPrefsManager.GetInstance().questInfo[0].Pun_35=MAX_SPEC_CNT; break;
                case 35: PlayerPrefsManager.GetInstance().questInfo[0].Pun_36=MAX_SPEC_CNT; break;
                case 36: PlayerPrefsManager.GetInstance().questInfo[0].Pun_37=MAX_SPEC_CNT; break;
                case 37: PlayerPrefsManager.GetInstance().questInfo[0].Pun_38=MAX_SPEC_CNT; break;
                case 38: PlayerPrefsManager.GetInstance().questInfo[0].Pun_39=MAX_SPEC_CNT; break;
                case 39: PlayerPrefsManager.GetInstance().questInfo[0].Pun_40=MAX_SPEC_CNT; break;
                case 40: PlayerPrefsManager.GetInstance().questInfo[0].Pun_41=MAX_SPEC_CNT; break;
                case 41: PlayerPrefsManager.GetInstance().questInfo[0].Pun_42=MAX_SPEC_CNT; break;
                case 42: PlayerPrefsManager.GetInstance().questInfo[0].Pun_43=MAX_SPEC_CNT; break;
                case 43: PlayerPrefsManager.GetInstance().questInfo[0].Pun_44=MAX_SPEC_CNT; break;
                case 44: PlayerPrefsManager.GetInstance().questInfo[0].Pun_45=MAX_SPEC_CNT; break;
                case 45: PlayerPrefsManager.GetInstance().questInfo[0].Pun_46=MAX_SPEC_CNT; break;
                case 46: PlayerPrefsManager.GetInstance().questInfo[0].Pun_47=MAX_SPEC_CNT; break;
                case 47: PlayerPrefsManager.GetInstance().questInfo[0].Pun_48=MAX_SPEC_CNT; break;
                case 48: PlayerPrefsManager.GetInstance().questInfo[0].Pun_49=MAX_SPEC_CNT; break;
                case 49: PlayerPrefsManager.GetInstance().questInfo[0].Pun_50= MAX_SPEC_CNT; break;
                case 50: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_51=MAX_SPEC_CNT; break;
                case 51: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_52=MAX_SPEC_CNT; break;
                case 52: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_53=MAX_SPEC_CNT; break;
                case 53: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_54=MAX_SPEC_CNT; break;
                case 54: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_55=MAX_SPEC_CNT; break;
                case 55: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_56=MAX_SPEC_CNT; break;
                case 56: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_57=MAX_SPEC_CNT; break;
                case 57: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_58=MAX_SPEC_CNT; break;
                case 58: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_59=MAX_SPEC_CNT; break;
                case 59: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_60=MAX_SPEC_CNT; break;
                case 60: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_61=MAX_SPEC_CNT; break;
                case 61: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_62=MAX_SPEC_CNT; break;
                case 62: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_63=MAX_SPEC_CNT; break;
                case 63: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_64=MAX_SPEC_CNT; break;
                case 64: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_65=MAX_SPEC_CNT; break;
                case 65: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_66=MAX_SPEC_CNT; break;
                case 66: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_67=MAX_SPEC_CNT; break;
                case 67: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_68=MAX_SPEC_CNT; break;
                case 68: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_69=MAX_SPEC_CNT; break;
                case 69: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_70= MAX_SPEC_CNT; break;
                    //
                case 70: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_71 =MAX_SPEC_CNT; break;
                case 71: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_72 =MAX_SPEC_CNT; break;
                case 72: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_73 =MAX_SPEC_CNT; break;
                case 73: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_74 =MAX_SPEC_CNT; break;
                case 74: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_75 =MAX_SPEC_CNT; break;
                case 75: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_76 =MAX_SPEC_CNT; break;
                case 76: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_77 =MAX_SPEC_CNT; break;
                case 77: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_78 =MAX_SPEC_CNT; break;
                case 78: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_79 =MAX_SPEC_CNT; break;
                case 79: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_80 =MAX_SPEC_CNT; break;
                    //                                                                                                                           
                case 80: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_81 =MAX_SPEC_CNT; break;
                case 81: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_82 =MAX_SPEC_CNT; break;
                case 82: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_83 =MAX_SPEC_CNT; break;
                case 83: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_84 =MAX_SPEC_CNT; break;
                case 84: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_85 =MAX_SPEC_CNT; break;
                case 85: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_86 =MAX_SPEC_CNT; break;
                case 86: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_87 =MAX_SPEC_CNT; break;
                case 87: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_88 =MAX_SPEC_CNT; break;
                case 88: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_89 =MAX_SPEC_CNT; break;
                case 89: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_90 =MAX_SPEC_CNT; break;
                //                                                                                                                               
                case 90: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_91 =MAX_SPEC_CNT; break;
                case 91: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_92 =MAX_SPEC_CNT; break;
                case 92: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_93 =MAX_SPEC_CNT; break;
                case 93: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_94 =MAX_SPEC_CNT; break;
                case 94: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_95 =MAX_SPEC_CNT; break;
                case 95: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_96 =MAX_SPEC_CNT; break;
                case 96: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_97 =MAX_SPEC_CNT; break;
                case 97: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_98 =MAX_SPEC_CNT; break;
                case 98: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_99 = MAX_SPEC_CNT; break;
                case 99: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_100 = MAX_SPEC_CNT; break;
            }

            return;
        }

        switch (_index)
        {
            case 0: PlayerPrefsManager.GetInstance().questInfo[0].Pun_01++; break;
            case 1: PlayerPrefsManager.GetInstance().questInfo[0].Pun_02++; break;
            case 2: PlayerPrefsManager.GetInstance().questInfo[0].Pun_03++; break;
            case 3: PlayerPrefsManager.GetInstance().questInfo[0].Pun_04++; break;
            case 4: PlayerPrefsManager.GetInstance().questInfo[0].Pun_05++; break;
            case 5: PlayerPrefsManager.GetInstance().questInfo[0].Pun_06++; break;
            case 6: PlayerPrefsManager.GetInstance().questInfo[0].Pun_07++; break;
            case 7: PlayerPrefsManager.GetInstance().questInfo[0].Pun_08++; break;
            case 8: PlayerPrefsManager.GetInstance().questInfo[0].Pun_09++; break;
            case 9: PlayerPrefsManager.GetInstance().questInfo[0].Pun_10++; break;
            case 10: PlayerPrefsManager.GetInstance().questInfo[0].Pun_11++; break;
            case 11: PlayerPrefsManager.GetInstance().questInfo[0].Pun_12++; break;
            case 12: PlayerPrefsManager.GetInstance().questInfo[0].Pun_13++; break;
            case 13: PlayerPrefsManager.GetInstance().questInfo[0].Pun_14++; break;
            case 14: PlayerPrefsManager.GetInstance().questInfo[0].Pun_15++; break;
            case 15: PlayerPrefsManager.GetInstance().questInfo[0].Pun_16++; break;
            case 16: PlayerPrefsManager.GetInstance().questInfo[0].Pun_17++; break;
            case 17: PlayerPrefsManager.GetInstance().questInfo[0].Pun_18++; break;
            case 18: PlayerPrefsManager.GetInstance().questInfo[0].Pun_19++; break;
            case 19: PlayerPrefsManager.GetInstance().questInfo[0].Pun_20++; break;
            case 20: PlayerPrefsManager.GetInstance().questInfo[0].Pun_21++; break;
            case 21: PlayerPrefsManager.GetInstance().questInfo[0].Pun_22++; break;
            case 22: PlayerPrefsManager.GetInstance().questInfo[0].Pun_23++; break;
            case 23: PlayerPrefsManager.GetInstance().questInfo[0].Pun_24++; break;
            case 24: PlayerPrefsManager.GetInstance().questInfo[0].Pun_25++; break;
            case 25: PlayerPrefsManager.GetInstance().questInfo[0].Pun_26++; break;
            case 26: PlayerPrefsManager.GetInstance().questInfo[0].Pun_27++; break;
            case 27: PlayerPrefsManager.GetInstance().questInfo[0].Pun_28++; break;
            case 28: PlayerPrefsManager.GetInstance().questInfo[0].Pun_29++; break;
            case 29: PlayerPrefsManager.GetInstance().questInfo[0].Pun_30++; break;
            case 30: PlayerPrefsManager.GetInstance().questInfo[0].Pun_31++; break;
            case 31: PlayerPrefsManager.GetInstance().questInfo[0].Pun_32++; break;
            case 32: PlayerPrefsManager.GetInstance().questInfo[0].Pun_33++; break;
            case 33: PlayerPrefsManager.GetInstance().questInfo[0].Pun_34++; break;
            case 34: PlayerPrefsManager.GetInstance().questInfo[0].Pun_35++; break;
            case 35: PlayerPrefsManager.GetInstance().questInfo[0].Pun_36++; break;
            case 36: PlayerPrefsManager.GetInstance().questInfo[0].Pun_37++; break;
            case 37: PlayerPrefsManager.GetInstance().questInfo[0].Pun_38++; break;
            case 38: PlayerPrefsManager.GetInstance().questInfo[0].Pun_39++; break;
            case 39: PlayerPrefsManager.GetInstance().questInfo[0].Pun_40++; break;
            case 40: PlayerPrefsManager.GetInstance().questInfo[0].Pun_41++; break;
            case 41: PlayerPrefsManager.GetInstance().questInfo[0].Pun_42++; break;
            case 42: PlayerPrefsManager.GetInstance().questInfo[0].Pun_43++; break;
            case 43: PlayerPrefsManager.GetInstance().questInfo[0].Pun_44++; break;
            case 44: PlayerPrefsManager.GetInstance().questInfo[0].Pun_45++; break;
            case 45: PlayerPrefsManager.GetInstance().questInfo[0].Pun_46++; break;
            case 46: PlayerPrefsManager.GetInstance().questInfo[0].Pun_47++; break;
            case 47: PlayerPrefsManager.GetInstance().questInfo[0].Pun_48++; break;
            case 48: PlayerPrefsManager.GetInstance().questInfo[0].Pun_49++; break;
            case 49: PlayerPrefsManager.GetInstance().questInfo[0].Pun_50++; break;
            case 50: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_51++; break;
            case 51: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_52++; break;
            case 52: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_53++; break;
            case 53: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_54++; break;
            case 54: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_55++; break;
            case 55: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_56++; break;
            case 56: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_57++; break;
            case 57: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_58++; break;
            case 58: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_59++; break;
            case 59: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_60++; break;
            case 60: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_61++; break;
            case 61: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_62++; break;
            case 62: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_63++; break;
            case 63: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_64++; break;
            case 64: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_65++; break;
            case 65: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_66++; break;
            case 66: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_67++; break;
            case 67: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_68++; break;
            case 68: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_69++; break;
            case 69: PlayerPrefsManager.GetInstance().questInfo2[0].Pun_70++; break;
            case 70: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_71++; break;
            case 71: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_72++; break;
            case 72: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_73++; break;
            case 73: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_74++; break;
            case 74: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_75++; break;
            case 75: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_76++; break;
            case 76: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_77++; break;
            case 77: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_78++; break;
            case 78: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_79++; break;
            case 79: PlayerPrefsManager.GetInstance().questInfo3[0].Pun_80++; break;
            //
            case 80: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_81++; break;
            case 81: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_82++; break;
            case 82: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_83++; break;
            case 83: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_84++; break;
            case 84: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_85++; break;
            case 85: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_86++; break;
            case 86: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_87++; break;
            case 87: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_88++; break;
            case 88: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_89++; break;
            case 89: PlayerPrefsManager.GetInstance().questInfo4[0].Pun_90++; break;
            //
            case 90: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_91++; break;
            case 91: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_92++; break;
            case 92: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_93++; break;
            case 93: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_94++; break;
            case 94: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_95++; break;
            case 95: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_96++; break;
            case 96: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_97++; break;
            case 97: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_98++; break;
            case 98: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_99++; break;
            case 99: PlayerPrefsManager.GetInstance().questInfo5[0].Pun_100++; break;
        }



    }



    public Transform DefencePunchPos;
    /// <summary>
    /// 디펜스 모드에서의 동작.
    /// </summary>
    public void InDefenceModePunch()
    {
        // 그로기 상태면 리턴
        if (PlayerPrefsManager.GetInstance().isGroggy) return;

        tmp = OP[punchIndex+1].GetComponent<Lean.Pool.LeanGameObjectPool>().Prefab;

        if (isPunchSwitch)
        {
            bullet_Left = Lean.Pool.LeanPool.Spawn(tmp, DefencePunchPos.GetChild(0).position, DefencePunchPos.GetChild(0).rotation);
            bullet_Left.GetComponent<BulletManager>().BulletInit();
            bullet_Left.GetComponent<BulletManager>().SetDefenceBar(DefenceHP);
            isPunchSwitch = !isPunchSwitch;
        }
        else
        {
            bullet_Right = Lean.Pool.LeanPool.Spawn(tmp, DefencePunchPos.GetChild(1).position, DefencePunchPos.GetChild(1).rotation);
            bullet_Right.GetComponent<BulletManager>().BulletInit0();
            bullet_Right.GetComponent<BulletManager>().SetDefenceBar(DefenceHP);
            isPunchSwitch = !isPunchSwitch;

        }
    }


    public Transform InfinitiPunchPos;
    /// <summary>
    /// 무한 버티기 모드 도전
    /// </summary>
    public void InFinitiModePunch()
    {
        // 그로기 상태면 리턴
        if (PlayerPrefsManager.GetInstance().isGroggy) return;

        int p_among = PlayerPrefsManager.GetInstance().punchAmont;

        punchIndex = Random.Range(0, p_among);
        tmp = OP[punchIndex].GetComponent<Lean.Pool.LeanGameObjectPool>().Prefab;

        if (isPunchSwitch)
        {
            bullet_Left = Lean.Pool.LeanPool.Spawn(tmp, InfinitiPunchPos.GetChild(0).position, InfinitiPunchPos.GetChild(0).rotation);
            bullet_Left.GetComponent<BulletManager>().BulletInit();
            bullet_Left.GetComponent<BulletManager>().SetFillHP(FilledHP, MattGauge, SkillGauge, BreadBtn);
            isPunchSwitch = !isPunchSwitch;
        }
        else
        {
            bullet_Right = Lean.Pool.LeanPool.Spawn(tmp, InfinitiPunchPos.GetChild(1).position, InfinitiPunchPos.GetChild(1).rotation);
            bullet_Right.GetComponent<BulletManager>().BulletInit0();
            bullet_Right.GetComponent<BulletManager>().SetFillHP(FilledHP, MattGauge, SkillGauge, BreadBtn);
            isPunchSwitch = !isPunchSwitch;

        }
    }

    [Header("-무한의 탑 놈들")]
    public Image Mugan_FilledHP;
    public Image Boss_FilledHP;
    public Transform MUGAN_Pos;
    /// <summary>
    ///  평타 파이어볼
    /// </summary>
    public GameObject MUGAN_Bullet;
    public GameObject MUGAN_StrongBullet;
    public GameObject MUGAN_HealBullet;




    /// <summary>
    /// 무한의~~~ 탑
    /// </summary>
    public void MUGAN_Punch()
    {
        float pun_Random = Random.Range(0, 100f);
        /// 파이어볼 종류 뭐니?
        GameObject bullet;
        /// 5% 5% 90%
        if (pun_Random < 5f)
            bullet = MUGAN_HealBullet;
        else if (pun_Random < 10f)
            bullet = MUGAN_StrongBullet;
        else
            bullet = MUGAN_Bullet;


        if (isPunchSwitch)
        {
            bullet_Left = Lean.Pool.LeanPool.Spawn(bullet, MUGAN_Pos.GetChild(0).position, MUGAN_Pos.GetChild(0).rotation);
            bullet_Left.GetComponent<BulletManager>().BulletInit();
            bullet_Left.GetComponent<BulletManager>().SetMuganBar(Mugan_FilledHP, Boss_FilledHP);
            ///랜덤으로 다음 펀치 바꿔준다.
            if (pun_Random > 50f) 
                isPunchSwitch = !isPunchSwitch;
        }
        else
        {
            bullet_Right = Lean.Pool.LeanPool.Spawn(bullet, MUGAN_Pos.GetChild(1).position, MUGAN_Pos.GetChild(1).rotation);
            bullet_Right.GetComponent<BulletManager>().BulletInit0();
            bullet_Right.GetComponent<BulletManager>().SetMuganBar(Mugan_FilledHP, Boss_FilledHP);
            ///랜덤으로 다음 펀치 바꿔준다.
            if (pun_Random > 50f) 
                isPunchSwitch = !isPunchSwitch;

        }
    }






}
