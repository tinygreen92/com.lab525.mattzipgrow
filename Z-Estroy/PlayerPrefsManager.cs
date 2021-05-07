/**
 * @author : tinygreen92 
 * @date   : 2020-03-06
 * @desc   : PlayerPrefs 관리. GPGS 저장도 여기서.
 *           
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;   // 리스트를 쓰기 위해 추가합니다.
// BinaryFormatter를 사용하기 위해서는 반드시 아래의 네임스페이스를 추가해줘야 해요.
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using CodeStage.AntiCheat.ObscuredTypes;

public class PlayerPrefsManager : MonoBehaviour
{

    public TutorialMissionManager tmm;
    static PlayerPrefsManager instance;

    public PlayNANOOExample playNANOO;

    DoubleToStringNum dts = new DoubleToStringNum(); // 단위 변환기

    //List<Dictionary<string, object>> weaponsData;
    //List<Dictionary<string, object>> shopData;
    //List<Dictionary<string, object>> trainingData;
    List<Dictionary<string, object>> magaDamageData;

    //List<Dictionary<string, object>> muganTopCollData;
    //List<Dictionary<string, object>> flagData;

    //List<Dictionary<string, object>> DiaStatData;


    ////MainGameDam
    //[HideInInspector]
    //public string[,] weaponColl;
    //[HideInInspector]
    //public string[,] shopColl;
    //[HideInInspector]
    //public string[,] trainColl;
    [HideInInspector]
    public string[] megaDamColl;

    //public string[,] muganTopColl;
    //[HideInInspector]
    //public string[,] flagDataColl;


    //[HideInInspector]
    //public string[,] diaStatDataColl;


    public static PlayerPrefsManager GetInstance()
    {
        return instance;
    }

    [HideInInspector]
    public readonly ObscuredInt punchAmont = 100; // 펀치갯수

    void Awake()
    {
        instance = this;
        magaDamageData = CSVReader.Read("MainGameDam"); // 방어전 대미지
        // 초기값 필요해?
        megaDamColl = new string[punchAmont];
    }


    ///// <summary>
    ///// 시트 미리 메모리에 올려두기
    ///// </summary>
    //public void InitMuganData()
    //{
    //    string tmp = "0";

    //    for (int j = 0; j < 200; j++)
    //    {
    //        tmp = muganTopCollData[j]["PhaseDamage"].ToString();
    //        muganTopColl[0, j] = dts.fDoubleToStringNumber(tmp);

    //        tmp = muganTopCollData[j]["BossHp"].ToString();
    //        muganTopColl[1, j] = dts.fDoubleToStringNumber(tmp);
    //    }

    //}

    /// <summary>
    /// 방어 전 시트 올려두기
    /// </summary>
    public void InitMegaDamData()
    {
        string tmp = "0";

        for (int j = 1; j < punchAmont; j++)
        {
            tmp = magaDamageData[j]["Damage"].ToString();
            megaDamColl[j - 1] = tmp;
        }
    }


    [HideInInspector]
    public bool isUIAinmPlay;   // UI 애니메이션 재생중?
    [Header("- 코인 프리팹")]
    public GameObject kimchiCoin;
    public GameObject CoinOB;
    public GameObject BapOB;
    public Transform CoinPos;
    public Transform KimchiPos;
    [Header("- 박스 프리팹")]
    public GameObject LucBox;
    [Header("- 인앱 결제 대기창")]
    public GameObject IN_APP;


    // 버티기 모드 시작했니? 트리거
    [HideInInspector]
    public bool isInfinity;
    // 버티기 모드 난이도
    [HideInInspector]
    public decimal Infi_Index;
    // 얻을 국밥
    [HideInInspector]
    public int getGupBap;
    // 버티기 모드 끝났냐
    [HideInInspector]
    public bool isInfinityEnd;
    // 무한의 탑 모드 포기했냐
    [HideInInspector]
    public bool isMuGanTopEnd;
    // 더블 체크
    [HideInInspector]
    public bool isEndGame;
    /// <summary>
    /// 체력감소 한턴에 하나만.
    /// </summary>
    [HideInInspector]
    public bool isHPsubing;
    /// <summary>
    /// 무한 버티기 모드 dps
    /// </summary>
    [HideInInspector]
    public double InfiPunchDPS;
    /// <summary>
    /// (b)그로기 상태 돈벌기 무시 상태.                 isGroggy;      
    /// </summary>
    [HideInInspector]
    public bool isGroggy;
    /// <summary>
    /// (b)오토 공격 동영상 한번 보고 On 상태. 
    /// </summary>
    [HideInInspector]
    public bool isAutoAbsOn;
    /// <summary>
    /// 튜토리얼에 쓰는 따라해보세요 횟수.
    /// </summary>
    [HideInInspector]
    public int TurtorialCount;
    /// <summary>
    /// 튜토리얼에 쓰는 튜토리얼 버티기 게임이오
    /// </summary>
    [HideInInspector]
    public bool isTuToInfi;
    /// <summary>
    /// 스택 열번 쌓으면 골드 최소값 증가하오.
    /// </summary>
    [HideInInspector]
    public int GoldStack;
    /// <summary>
    /// 무한의 탑 보스 체력
    /// </summary>
    [HideInInspector]
    public string bossHP;
    [HideInInspector]
    public string MAX_boss_HP;
    /// <summary>
    /// 무한 탑 이어하기는 한번만 하는 불린 값
    /// </summary>
    [HideInInspector]
    public bool isSecondChan;
    /// <summary>
    /// 자동 공격 ON = true / off = false
    /// </summary>
    [HideInInspector]
    public bool isAutoAtk;
    /// <summary>
    /// 국밥 공속 업 ON = true / off = false
    /// </summary>
    [HideInInspector]
    public bool isGupSpeed;
    // 피비피 스타트 스위치
    [HideInInspector]
    public bool isPVPtoEnd;


    ///----------------------------------------------------------------------------------
    /// 
    /// (b)튜토리얼 완료 여부                          isFristGameStart
    /// (i)데일리 출첵 여부                            DailyCount
    /// 
    /// (i)현재 장착한 펀치 인덱스                     PunchIndex
    /// (i)방어전 성공 여부 성공 = 0 / 실패 = 1        DefendTrigger
    /// 
    /// (b)음소거 설정.                                   isAllmute
    /// (b)고소장 받은 상태인가? 예스                     isGoingGOSO
    /// (b)데이터 불러오기 후에는 오프라인 없음                 isDataLoaded;      
    /// 
    ///----------------------------------------------------------------------------------





    #region 시스템 저장 테이블 

    // 튜토리얼 트리거
    public bool isFirstPVP
    {
        get
        {
            var tmp = PlayerPrefs.GetInt("isFirstPVP", 0); // 값 존재 안하면 0값.
            if (tmp == 0)
                return false;
            else
                return true;
        }
        set
        {
            if (value) // true 값이면?
                PlayerPrefs.SetInt("isFirstPVP", 1);
            else
                PlayerPrefs.SetInt("isFirstPVP", 0);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// true 라면 튜토리얼 인트로를 시청한 상태다.
    /// 다음 실행시 튜토리얼 건너뛰기 자동
    /// </summary>
    public bool isFristGameStart
    {
        get
        {
            // 값 존재 안하면 0값.
            if (PlayerPrefs.GetInt("isFristGameStart", 0) == 0)
                return false;
            else
                return true;
        }
        set
        {

            if (value) // true 값이면?
                PlayerPrefs.SetInt("isFristGameStart", 1);
            else
                PlayerPrefs.SetInt("isFristGameStart", 0);
            //
            PlayerPrefs.Save();

        }
    }

    /// <summary>
    /// 24시간 출석 체크용.
    /// </summary>
    public int NewDailyCount
    {
        get
        {
            // 출석일 0일 초기값.
            if (!PlayerPrefs.HasKey("NewDailyCount")) return 0;
            // 값 존재 안하면 0값.
            var _daily = PlayerPrefs.GetInt("NewDailyCount");

            return _daily;
        }

        set
        {
            PlayerPrefs.SetInt("NewDailyCount", value);
            PlayerPrefs.Save();
        }
    }

    // 3시간 짜리 스핀 보상용
    public int DailySpinReword
    {
        get
        {
            // 출석일 0일 초기값.
            if (!PlayerPrefs.HasKey("DailySpinReword")) return 0;
            // 값 존재 안하면 0값.
            var _daily = PlayerPrefs.GetInt("DailySpinReword");

            return _daily;
        }

        set
        {
            PlayerPrefs.SetInt("DailySpinReword", value);
            PlayerPrefs.Save();

        }
    }

    /// <summary>
    /// (i)현재 장착한 펀치 인덱스           PunchIndex
    /// </summary>
    public int PunchIndex
    {
        get
        {
            if (!PlayerPrefs.HasKey("PunchIndex")) return 0;
            var _daily = PlayerPrefs.GetInt("PunchIndex");

            return _daily;
        }

        set { PlayerPrefs.SetInt("PunchIndex", value); PlayerPrefs.Save(); }
    }

    /// (i)방어전 성공 여부 성공 = 0 / 실패 = 1           DefendTrigger
    public int DefendTrigger
    {
        get
        {
            if (!PlayerPrefs.HasKey("DefendTrigger")) return 0;
            var _daily = PlayerPrefs.GetInt("DefendTrigger");

            return _daily;
        }

        set { PlayerPrefs.SetInt("DefendTrigger", value); PlayerPrefs.Save(); }
    }

    /// 음소거 설정.
    public bool isAllmute
    {
        get
        {
            if (!PlayerPrefs.HasKey("isAllmute")) return false;
            var tmp = PlayerPrefs.GetInt("isAllmute", 0);
            if (tmp == 0) return false;
            else return true;
        }

        set
        {
            if (value == false)
                PlayerPrefs.SetInt("isAllmute", 0);
            else
                PlayerPrefs.SetInt("isAllmute", 1);

            PlayerPrefs.Save();
        }
    }

    /// 고소장 받은 상태인가? 예스
    public bool isGoingGOSO
    {
        get
        {
            if (!PlayerPrefs.HasKey("isGoingGOSO")) return false;
            var tmp = PlayerPrefs.GetInt("isGoingGOSO", 0);
            if (tmp == 0) return false;
            else return true;
        }

        set
        {
            if (value == false)
                PlayerPrefs.SetInt("isGoingGOSO", 0);
            else
                PlayerPrefs.SetInt("isGoingGOSO", 1);

            PlayerPrefs.Save();
        }
    }


    /// (b)데이터 불러오기 후에는 오프라인 없음                 isDataLoaded;      
    public bool isDataLoaded
    {
        get
        {
            if (!PlayerPrefs.HasKey("isDataLoaded")) return false;
            var tmp = PlayerPrefs.GetInt("isDataLoaded", 0);
            if (tmp == 0) return false;
            else return true;
        }

        set
        {
            if (value == false)
                PlayerPrefs.SetInt("isDataLoaded", 0);
            else
                PlayerPrefs.SetInt("isDataLoaded", 1);

            PlayerPrefs.Save();
        }
    }


    // 24시간 출석 체크용.
    public int DailyCount_Cheak
    {
        get
        {
            // 출석일 0일 초기값.
            if (!PlayerPrefs.HasKey("DailyCount_Cheak")) return 0;
            // 값 존재 안하면 0값.
            var _daily = PlayerPrefs.GetInt("DailyCount_Cheak", 0);

            return _daily;
        }

        set { PlayerPrefs.SetInt("DailyCount_Cheak", value); PlayerPrefs.Save(); }
    }


    #endregion


    ///----------------------------------------------------------------------------------
    /// 
    /// (s)골드              gold
    /// (s)다이아몬드        diamond
    /// (s)국밥              gupbap
    /// (s)쌀밥              ssalbap
    /// (i)열쇠              key
    /// 
    /// 
    /// 
    /// 
    /// (i)오토 공격 구매자          VIP = 525
    /// (i)광고제거 구매자           VIP = 526
    /// (i)맷집무한 구매자           VIP = 527
    /// (i)골드무한 구매자           VIP = 528
    /// 
    /// (i)버프 두개구매자           VIP = 527528
    /// 
    /// (i)올인원 구매자              VIP = 625
    /// (i)자동+광고 구매자           VIP = 725
    /// (i)자동+버프 구매자           VIP = 825
    /// (i)광고+버프 구매자           VIP = 925
    /// 
    ///----------------------------------------------------------------------------------

    #region Player 재화 테이블 

    /// (s)골드              gold
    public string gold
    {
        get
        {
            // gold 값 존재하지 않으면 0 반환
            if (!PlayerPrefs.HasKey("gold")) return "0";
            // 존재해?
            var _gold = PlayerPrefs.GetString("gold"); // 1E + 308 값
            //var result = dts.fDoubleToStringNumber(_gold); // double.Tosting 값
            var result = double.Parse(_gold).ToString("f0"); // double.Tosting 값

            return result;
        }

        set
        {
            var result = dts.SubStringDouble(value, "9.99E+302"); // double.Tosting 값
            if (result == "-1")
                PlayerPrefs.SetString("gold", value);
            else
                PlayerPrefs.SetString("gold", "9.99E+302");

            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// 추가 재화 김치
    /// </summary>
    public string Kimchi
    {
        get
        {
            var _gold = PlayerPrefs.GetString("diamond", "0"); // 1E + 308 값
            var result = double.Parse(_gold).ToString("f0"); // double.Tosting 값

            return result;
        }

        set
        {
            var result = dts.SubStringDouble(value, "9.99E+302"); // double.Tosting 값
            if (result == "-1")
                PlayerPrefs.SetString("diamond", value);
            else
                PlayerPrefs.SetString("diamond", "9.99E+302");

            PlayerPrefs.Save();
        }
    }

    /// (s)국밥            gupbap
    public string gupbap
    {
        get
        {
            if (!PlayerPrefs.HasKey("gupbap")) return "0";
            var _petFood = PlayerPrefs.GetString("gupbap");
            //var result = dts.fDoubleToStringNumber(_petFood);
            var result = double.Parse(_petFood).ToString("f0");

            return result;
        }

        set
        {
            var result = dts.SubStringDouble(value, "9.99E+302"); // double.Tosting 값
            if (result == "-1")
                PlayerPrefs.SetString("gupbap", value);
            else
                PlayerPrefs.SetString("gupbap", "9.99E+302");

            PlayerPrefs.Save();
        }
    }

    /// (s)쌀밥            ssalbap
    public string ssalbap
    {
        get
        {
            if (!PlayerPrefs.HasKey("ssalbap")) return "0";
            var _petFood = PlayerPrefs.GetString("ssalbap");

            //var result = dts.fDoubleToStringNumber(_petFood);
            var result = double.Parse(_petFood).ToString("f0");

            return result;
        }

        set
        {
            var result = dts.SubStringDouble(value, "9.99E+302"); // double.Tosting 값
            if (result == "-1")
                PlayerPrefs.SetString("ssalbap", value);
            else
                PlayerPrefs.SetString("ssalbap", "9.99E+302");
            PlayerPrefs.Save();
        }
    }

    /// (i) 열쇠                 key
    public int key
    {
        get
        {
            if (!PlayerPrefs.HasKey("key")) return 20;
            var _key = PlayerPrefs.GetInt("key");
            return _key;
        }

        set
        {
            int kkey = value;
            // 최대 열쇠 범위 20억
            if (value >= 2000000000) kkey = 2000000000;
            //
            PlayerPrefs.SetInt("key", kkey);
            PlayerPrefs.Save();
            //
            UserWallet.GetInstance().SetKeyAmount(kkey);

        }
    }


    /// (i) 피빕 입장권                 ticket
    public int ticket
    {
        get
        {
            var _key = PlayerPrefs.GetInt("ticket", 5);
            return _key;
        }

        set
        {
            int kkey = value;
            // 최대 열쇠 범위 20억
            if (value >= 2000000000) kkey = 2000000000;

            PlayerPrefs.SetInt("ticket", kkey);
            PlayerPrefs.Save();
        }
    }



    /// (i)오토 공격 구매자          VIP
    public int VIP
    {
        get
        {
            if (!PlayerPrefs.HasKey("VIP")) return 0;
            var _key = PlayerPrefs.GetInt("VIP");
            return _key;
        }

        set
        {
            PlayerPrefs.SetInt("VIP", value);
            PlayerPrefs.Save();
        }
    }

    #endregion


    ///----------------------------------------------------------------------------------
    /// 
    /// (float)훈련장 배경으로 코인 뻥튀기            BG_CoinStat    
    /// 
    /// (bool)골드 획득량 세배 (X3)             isGoldTriple
    /// (string)주먹 최종 공격력                  PlayerDPS    
    /// (string)주먹 자체 순수공격력              RawAttackDamage    
    /// (float) 주먹 강화 퍼센트 배율             AttackPerPunch
    ///                                         
    /// (string)체력                              Mat_HP
    /// (string)최근 체력                         Mat_currentHP
    ///                                         
    /// (string)맷집                              Mat_Mattzip
    /// (bool)맷집 세배                           isBoosterMattzip -> 공격력 2배로 바꿔
    /// (float)맷집 소줏점 히트                     Mat_Mattzip_Hit
    /// (float)맷집 스탯                        MattzipStat
    /// (float)맷집 유물 (유물 강화로만 증가)                       MattzipArtif
    /// 
    /// (string)체력 회복량                       Mat_Recov
    /// 
    /// (int)맷집 100카운트                       Mat_100
    ///                                         
    /// (int)스킬 500카운트                       Mat_Skill_500
    ///                                         
    /// (float)아이템 획득확률<럭키박스>          LuckyProb
    /// 
    /// (int)공격력 레벨                          ATK_Lv
    /// (int)체력 레벨                            Mat_HP_Lv
    /// (int)회복량 레벨                          Recov_Lv
    /// (int)맷집 레벨                            Mattzip_Lv
    /// (float)그로기 터치 횟수                          GroggyTouch
    /// 
    /// (int)런닝화- 초당 터치 횟수 증가          Arti_PunchTouch
    /// (int)헤드기어- 맷집증가                   Arti_Mattzip
    /// (int)줄넘기- 체력증가                     Arti_HP
    /// (int)물총- 그로기 상태 짧아짐             Arti_GroggyTouch
    /// (int)국밥 - 국밥 스킬 지속 시간 증가                Arti_GAL
    /// ----- 추가 -----
    /// (int)모래시계- 방어전 시간 감소                Arti_DefenceTime
    /// (int)선물상자- 선물 상자 등장확률 증가                Arti_GoldBox
    /// (int)머니건- 방치 골드 획득량 증가                Arti_OffGold
    /// 
    /// 
    /// (int)무한 버티기에서 최대로 얻은 국밥                MaxGet_GookBap
    /// 
    /// 
    ///----------------------------------------------------------------------------------

    #region 플레이어 능력치 테이블



    /// <summary>
    /// (string)1~ 10단계의 무한버티기 기록            InfiPersonalRecord 
    ///  1*0*2*0*3*0*4*0*5*0*6*0*7*0*8*0*9*0*10*0*
    /// </summary>
    public string InfiPersonalRecord
    {
        get
        {
            if (!PlayerPrefs.HasKey("InfiPersonalRecord")) return "0*0*0*0*0*0*0*0*0*0*";
            var tmp = PlayerPrefs.GetString("InfiPersonalRecord", "0*0*0*0*0*0*0*0*0*0*");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetString("InfiPersonalRecord", value);
            PlayerPrefs.Save();
        }
    }







    /// <summary>
    /// (float)훈련장 배경으로 코인 뻥튀기            BG_CoinStat    
    /// </summary>
    public float BG_CoinStat
    {
        get
        {
            if (!PlayerPrefs.HasKey("BG_CoinStat")) return 1f;
            var tmp = PlayerPrefs.GetFloat("BG_CoinStat", 1f);
            return tmp;
        }

        set
        {
            PlayerPrefs.SetFloat("BG_CoinStat", value);
            PlayerPrefs.Save();
        }
    }


    /// <summary>
    /// (string)훈련장 배경 데이터            BG_Data    
    /// </summary>
    public string BG_Data
    {
        get
        {
            if (!PlayerPrefs.HasKey("BG_Data")) return "0+0+0+0+0";
            var tmp = PlayerPrefs.GetString("BG_Data", "0+0+0+0+0");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetString("BG_Data", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// (string)훈련장 배경 데이터 현재 배경            BG_Curent    
    /// </summary>
    public int BG_Curent
    {
        get
        {
            if (!PlayerPrefs.HasKey("BG_Curent")) return 525;
            var tmp = PlayerPrefs.GetInt("BG_Curent", 525);
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("BG_Curent", value);
            PlayerPrefs.Save();
        }
    }





    /// <summary>
    /// (string)유니폼 장착 데이터            Uniform_Data    
    /// </summary>
    public string Uniform_Data
    {
        get
        {
            var tmp = PlayerPrefs.GetString("Uniform_Data", "1+0+0+0+0+0+0");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetString("Uniform_Data", value);
            PlayerPrefs.Save();
        }
    }



    /// (bool)골드 획득량  (X3)       isGoldTriple
    public bool isGoldTriple
    {
        get
        {
            if (!PlayerPrefs.HasKey("isGoldTriple")) return false;

            var _intBoosterDPS = PlayerPrefs.GetInt("isGoldTriple", 0);

            if (_intBoosterDPS == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        set
        {
            if (value == false)
            {
                PlayerPrefs.SetInt("isGoldTriple", 0);
            }
            else
            {
                PlayerPrefs.SetInt("isGoldTriple", 1);
            }
            PlayerPrefs.Save();
        }
    }

    double criDPS;

    /// <summary>
    /// (string)크리티컬 공격력            CriticalDPS    
    /// </summary>
    public string CriticalDPS
    {
        /// 크리티컬 = 공격력 x 크리티컬대미지
        get
        {
            criDPS = _PlayerDPSdouble * (float.Parse(CRD_UP) + float.Parse(Dia_CRD_UP) * 0.01d);
            return (_PlayerDPSdouble + criDPS).ToString("f0");
        }

        set
        {
            PlayerPrefs.SetString("CriticalDPS", value);
            PlayerPrefs.Save();
        }
    }

    string _Critical_Per;
    /// <summary>
    /// 크리티컬 확률
    /// </summary>
    public string Critical_Per
    {
        get
        {
            _Critical_Per = (float.Parse(CRC_UP) * float.Parse(Dia_CRC_UP)).ToString("f3");
            return _Critical_Per;
        }

        set
        {
            PlayerPrefs.SetString("Critical_Per", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// 공격력 10 이상이면 true
    /// </summary>
    public bool isDPS10Kimchi;

    string _PlayerDPS;
    string CharaDps;

    string tmp1;
    double tmp2;

    double _PlayerDPSdouble;
    /// <summary>
    /// (string)주먹 최종 공격력            PlayerDPS    
    /// [ ( 스탯 공격력 + 레벨 공격력 ) * ( 특별강화 % + 스킬 % + 장비 % + 구매 % + 깃발 % + 유물 % ) ] * 버프 100%
    /// </summary>
    public string PlayerDPS
    {
        get
        {
            _PlayerDPS = "0";
            /// [레벨 공격력] - 캐릭터 레벨 올리면 증가
            CharaDps = PlayerPrefs.GetString("Chara_Attack_UP", "0");

            /// ( 스탯 공격력 + 레벨 공격력 ) 
            tmp1 = dts.AddStringDouble(CharaDps, RawAttackDamage);

            /// ( 특별강화 % + 스킬 % + 장비 % + 구매 % + 깃발 % + 유물 % )
            tmp2 = 0.01d
                * (double.Parse(Dia_ATK_PER_UP) // 특별강화 %
                + uniformInfo[0].Skill_LV // 스킬 %
                + weaponInfo[PunchIndex].weaponEffect // 장비 %
                + Punch_Dia_Weap // 영구구매 %
                + Stat_is3ATK  // 깃발 %
                + AttackPunch); // 유물 %
            /// 기본 배율 보정
            tmp2 += 1.0d;

            /// 맷집3배 버프 삭제후 -> 공격력 2배 버프로 변경됨
            if (isBoosterMattzip)
                _PlayerDPSdouble = (double.Parse(tmp1) * tmp2 * 2d); //플로트 최종 공격  -> 외부에 저장되어서 크리티컬 공격력에 계산
            else
                _PlayerDPSdouble = (double.Parse(tmp1) * tmp2); //플로트 최종 공격  -> 외부에 저장되어서 크리티컬 공격력에 계산

            /// 깍두기 10 이상일때 절반으로 줄여.
            if (_PlayerDPSdouble >= 10) isDPS10Kimchi = true;

            ///스트링 최종 공격
            _PlayerDPS = _PlayerDPSdouble.ToString("f0");

            return _PlayerDPS;
        }

        set
        {
            PlayerPrefs.SetString("PlayerDPS", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// 플레이어 공격력 더블 가져올 것.
    /// </summary>
    /// <returns></returns>
    public double GetPlayerDouble()
    {
        return _PlayerDPSdouble;
    }

    /// <summary>
    /// 공격력에 double 배를 하고 int 를 더한다.
    /// </summary>
    /// <param name="_Bae"></param>
    /// <param name="_plus"></param>
    /// <returns></returns>
    public double GetDPS_Bae(double _Bae, int _plus)
    {
        var tmp = _PlayerDPSdouble * _Bae;

        if (tmp > double.MaxValue)
        {
            return double.MaxValue;
        }
        else
        {
            return (_PlayerDPSdouble * _Bae) + _plus;
        }
    }


    private string rawAttackDamage = "1";
    /// <summary>
    /// (string)[스탯 공격력]            RawAttackDamage    
    /// </summary>
    public string RawAttackDamage = "1";


    /// <summary>
    /// (float) 공격력 유물 %            AttackPunch
    /// </summary>
    public float AttackPunch
    {
        get
        {
            // 실질 플레이어 대미지
            if (!PlayerPrefs.HasKey("AttackPerPunch")) return 0;
            var tmp = PlayerPrefs.GetFloat("AttackPerPunch");
            //Debug.LogWarning("tmp : " + tmp);
            return tmp;
        }

        set
        {
            PlayerPrefs.SetFloat("AttackPerPunch", value);
            PlayerPrefs.Save();
        }
    }

    double MaxHPtmp1;
    double MaxHPtmp2;

    /// <summary>
    /// (string)플레이어 체력                        Mat_HP
    /// ( 스탯 체력 + 레벨 체력 ) * ( 특별강화 % + 스킬 % + 깃발 % + 유물 % )
    /// </summary>
    public string Mat_MaxHP
    {
        get
        {
            /// ( 스탯 체력 + 레벨 체력 )
            MaxHPtmp1 = double.Parse(Stat_MaxHP) + double.Parse(PlayerPrefs.GetString("Chara_HP_UP", "0"));

            /// ( 특별강화 % + 스킬 % + 깃발 % + 유물 % )
            MaxHPtmp2 = 0.01d
                * (double.Parse(Dia_HP_PER_UP) // 특별강화 %
                + Stat_is2Stamina // 깃발 %
                + uniformInfo[1].Skill_LV // 스킬 %
                + double.Parse(Arti_MaxHP) // 유물 %
                );

            /// 기본 배율 보정
            MaxHPtmp2 += 1.0d;

            string ttmpmp = (MaxHPtmp1 * MaxHPtmp2).ToString("f0");
            return ttmpmp;
        }

        set
        {
            PlayerPrefs.SetString("Mat_MaxHP", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// (string)스탯 체력 10 단위                        Stat_MaxHP
    /// </summary>
    public string Stat_MaxHP
    {
        get
        {
            if (!PlayerPrefs.HasKey("Stat_MaxHP")) return "0";
            var tmp = PlayerPrefs.GetString("Stat_MaxHP");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetString("Stat_MaxHP", value);
            PlayerPrefs.Save();

            //if (Arti_MaxHP != "0")
            //{
            //    var result = double.Parse(Arti_MaxHP);
            //    Mat_MaxHP = dts.multipleStringDouble(value, result);
            //}
            //else
            //{
            //    Mat_MaxHP = value;
            //}
        }
    }

    /// <summary>
    /// (string)유물 체력                        Arti_MaxHP
    /// </summary>
    public string Arti_MaxHP
    {
        get
        {
            if (!PlayerPrefs.HasKey("Arti_MaxHP")) return "0";
            var tmp = PlayerPrefs.GetString("Arti_MaxHP");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetString("Arti_MaxHP", value);
            PlayerPrefs.Save();

            //if (value != "0")
            //{
            //    double dresult = double.Parse(value);
            //    Mat_MaxHP = dts.multipleStringDouble(Stat_MaxHP, dresult);
            //}
            //else
            //    Mat_MaxHP = Stat_MaxHP;
        }
    }

    /// <summary>
    /// (string)최근 체력                        Mat_currentHP
    /// </summary>
    public string Mat_currentHP
    {
        get
        {
            if (!PlayerPrefs.HasKey("Mat_currentHP")) return "100";
            var tmp = PlayerPrefs.GetString("Mat_currentHP");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetString("Mat_currentHP", value);
            PlayerPrefs.Save();
        }
    }



    [HideInInspector]
    public double dMattZip;

    /// <summary>
    /// (string)맷집                        tmppp.ToString("f0");
    /// </summary>
    public string Mat_Mattzip
    {
        get
        {
            /// Mat_Mattzip_Hit 맞으면 증가
            /// MattzipArtif 유물 뽑기 하면 증가
            dMattZip = Mat_Mattzip_Hit + (Mat_Mattzip_Hit * MattzipArtif * 0.01f);

            ///	1.7976931348623158 E + 308
            if (dMattZip >= 9.99E+302)
                dMattZip = 9.99E+302;

            return dMattZip.ToString("f0");
        }

        set
        {

            PlayerPrefs.SetString("Mat_Mattzip", value);
            PlayerPrefs.Save();
        }
    }

    /// (bool)맷집 세배                           isBoosterMattzip
    public bool isBoosterMattzip
    {
        get
        {
            if (!PlayerPrefs.HasKey("isBoosterMattzip")) return false;

            var _intBoosterDPS = PlayerPrefs.GetInt("isBoosterMattzip");
            UserWallet.GetInstance().ShowUserMatZip();

            if (_intBoosterDPS == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        set
        {
            if (value == false)
            {
                PlayerPrefs.SetInt("isBoosterMattzip", 0);
            }
            else
            {
                PlayerPrefs.SetInt("isBoosterMattzip", 1);
            }
            UserWallet.GetInstance().ShowUserMatZip();
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// 맞아서 증가하는 맷집 레벨 (1씩 증가)
    /// </summary>
    public float Mat_Mattzip_Hit
    {
        get
        {
            if (!PlayerPrefs.HasKey("Mat_Mattzip_Hit")) return 0;
            var tmp = PlayerPrefs.GetFloat("Mat_Mattzip_Hit");
            return tmp;
        }

        set
        {
            ///3.402823466 E + 38
            float kkey = value;
            if (value >= 9E+35f)
                kkey = 9E+35f;

            PlayerPrefs.SetFloat("Mat_Mattzip_Hit", kkey);
            PlayerPrefs.Save();

            Invoke(nameof(myInvoExMission), 1.0f);
        }
    }
    void myInvoExMission()
    {
        tmm.ExUpdateMission(1, dMattZip); /// 미션 업데이트
        tmm.ExUpdateMission(10, dMattZip); /// 미션 업데이트
        tmm.ExUpdateMission(20, dMattZip); /// 미션 업데이트
        tmm.ExUpdateMission(31, dMattZip); /// 미션 업데이트
        tmm.ExUpdateMission(40, dMattZip); /// 미션 업데이트
        tmm.ExUpdateMission(50, dMattZip); /// 미션 업데이트
        tmm.ExUpdateMission(60, dMattZip); /// 미션 업데이트
        tmm.ExUpdateMission(70, dMattZip); /// 미션 업데이트
        tmm.ExUpdateMission(80, dMattZip); /// 미션 업데이트
        tmm.ExUpdateMission(91, dMattZip); /// 미션 업데이트
        tmm.ExUpdateMission(92, dMattZip); /// 미션 업데이트
        tmm.ExUpdateMission(93, dMattZip); /// 미션 업데이트
        tmm.ExUpdateMission(94, dMattZip); /// 미션 업데이트
        tmm.ExUpdateMission(95, dMattZip); /// 미션 업데이트
        tmm.ExUpdateMission(96, dMattZip); /// 미션 업데이트
        tmm.ExUpdateMission(97, dMattZip); /// 미션 업데이트
        tmm.ExUpdateMission(98, dMattZip); /// 미션 업데이트
        tmm.ExUpdateMission(99, dMattZip); /// 미션 업데이트
    }


    /// (float)맷집 유물 (유물 강화로만 증가)                       MattzipArtif
    public float MattzipArtif
    {
        get
        {
            if (!PlayerPrefs.HasKey("MattzipArtif")) return 0;
            var tmp = PlayerPrefs.GetFloat("MattzipArtif");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetFloat("MattzipArtif", value);
            PlayerPrefs.Save();
        }
    }


    float tmp;
    float ttmmpp;

    /// <summary>
    /// (string)체력 회복량                       Mat_Recov
    /// ( 스탯 체력회복력 + 레벨 체력회복력 ) * ( 특별강화 % + 스킬 % + 깃발 % + 유물 % )
    /// </summary>
    public string Mat_Recov
    {
        get
        {
            /// ( 스탯 체력회복력 + 레벨 체력회복력 )
            tmp = float.Parse(PlayerPrefs.GetString("Stat_Recov", "1")) + float.Parse(PlayerPrefs.GetString("Chara_Recov_UP", "0"));
            /// ( 특별강화 % + 스킬 % + 깃발 % + 유물 % )
            ttmmpp = 0.01f
                * (Dia_RECOV_UP_Lv   // 특별강화 %
                + uniformInfo[3].Skill_LV // 스킬 %
                + Stat_is1Recov // 깃발 %
                + (Arti_HEALLING_UP * 0.5f) // 유물 %
                );


            return (tmp + ttmmpp).ToString("f0");
        }

        set
        {
            PlayerPrefs.SetString("Mat_Recov", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// 미사용
    /// </summary>
    public float ArtiRecov
    {
        get
        {
            return PlayerPrefs.GetFloat("ArtiRecov", 0);
        }

        set
        {
            PlayerPrefs.SetFloat("ArtiRecov", value);
            PlayerPrefs.Save();
        }
    }



    /// <summary>
    /// pvp 방어력은 상대방의 공격력 - ( 맷집 + 방어력 ) 만큼 체력이 깎임
    /// </summary>
    /// <returns></returns>
    public string GetPvpMattDefence()
    {
        return dts.AddStringDouble(Mat_Mattzip, GetPlayerDefence().ToString("f0"));
    }



    float Def_result1;
    double Def_result2;
    /// <summary>
    /// 유저 방어력 double 배출
    /// (스탯 방어력 + 레벨 방어력 ) * ( 방패 착용 방어력 % + 방패 보유 방어력 % + 깃발 % + 유물 % + 특별강화 %)  
    /// </summary>
    /// <returns></returns>
    public double GetPlayerDefence()
    {
        /// (스탯 방어력 + 레벨 방어력 )
        Def_result1 = Defence_Lv + PlayerPrefs.GetFloat("Chara_Defence_UP", 0);
        /// ( 방패 착용 방어력 % + 방패 보유 방어력 % + 깃발 % + 유물 % + 특별강화 % )  
        Def_result2 = 0.01f
            * (
              dts.PanByulGi(Defence_Shiled) // 방패 착용 방어력 %
            + float.Parse(Defence_Dia_Shiled)   // 방패 보유 방어력 %
            + Stat_is4Deffence  // 깃발 %
            + (Arti_DEF_UP * 0.5f)  // 유물 % 1렙당 0.5% 증가
            + (Dia_Defence_Lv * 5f)   // 특별강화 % 1렙당 5% 증가
            );
        /// 기본 배율 보정
        Def_result2 += 1.0f;
        return (Def_result1 * Def_result2);
    }

    /// <summary>
    /// 현재 장착한 쉴드 [인덱스 -1]
    /// </summary>
    public int ShieldIndex
    {
        get
        {
            var tmp = PlayerPrefs.GetInt("Gold_RECOV_UP_Lv", 0);
            return tmp - 1;
        }

        set
        {
            PlayerPrefs.SetInt("Gold_RECOV_UP_Lv", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// (int)방어력 특별 강화 레벨
    /// </summary>
    public int Dia_Defence_Lv
    {
        get
        {
            return PlayerPrefs.GetInt("HP_PER_UP_Lv", 0);
        }

        set
        {
            PlayerPrefs.SetInt("HP_PER_UP_Lv", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// int 로 바꿀것
    /// </summary>
    public int ShiledTicket
    {
        get
        {
            if (!PlayerPrefs.HasKey("MattzipStat")) return 0;
            var _key = PlayerPrefs.GetInt("MattzipStat");
            return _key;
        }

        set
        {
            int kkey = value;
            // 최대 열쇠 범위 20억
            if (value >= 2000000000) kkey = 2000000000;
            //
            PlayerPrefs.SetInt("MattzipStat", kkey);
            PlayerPrefs.Save();
            //
            UserWallet.GetInstance().SetSD_TikAmount(kkey);
        }
    }


    /// <summary>
    /// [스탯 방어력] 레벨
    /// </summary>
    public int Defence_Lv
    {
        get
        {
            if (!PlayerPrefs.HasKey("Mattzip_Lv")) return 0;
            var tmp = PlayerPrefs.GetInt("Mattzip_Lv");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Mattzip_Lv", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// (string) [착용 방패 방어력 * 방패 증가유물]
    /// </summary>
    public string Defence_Shiled
    {
        get
        {
            var tmp = PlayerPrefs.GetString("ATK_PER_UP", "0");

            /// 장착 방패 방어력 증가 유물 적용
            tmp = dts.multipleStringDouble(tmp, 1f + (Arti_SHILED_UP * 0.1f));

            return tmp;
        }

        set
        {
            PlayerPrefs.SetString("ATK_PER_UP", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// (string)[보유 방패 방어력]
    /// </summary>
    public string Defence_Dia_Shiled
    {
        get
        {
            if (!PlayerPrefs.HasKey("HP_PER_UP")) return "0";
            var tmp = PlayerPrefs.GetString("HP_PER_UP");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetString("HP_PER_UP", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// 미사용
    /// </summary>
    public int ATK_PER_UP_Lv
    {
        get
        {
            return PlayerPrefs.GetInt("ATK_PER_UP_Lv", 0);
        }

        set
        {
            PlayerPrefs.SetInt("ATK_PER_UP_Lv", value);
            PlayerPrefs.Save();
        }
    }






    /// (string)다이아 공격력 % 증가량                       ATK_PER_UP
    public string Dia_ATK_PER_UP
    {
        get
        {
            if (!PlayerPrefs.HasKey("Dia_ATK_PER_UP")) return "1";
            var tmp = PlayerPrefs.GetString("Dia_ATK_PER_UP");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetString("Dia_ATK_PER_UP", value);
            PlayerPrefs.Save();
        }
    }

    /// (string)체력 % 증가량                       HP_PER_UP
    public string Dia_HP_PER_UP
    {
        get
        {
            if (!PlayerPrefs.HasKey("Dia_HP_PER_UP")) return "1";
            var tmp = PlayerPrefs.GetString("Dia_HP_PER_UP");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetString("Dia_HP_PER_UP", value);
            PlayerPrefs.Save();
        }
    }


    /// (string)다이아 공격력 % 증가량 레벨                     Dia_ATK_PER_UP_Lv
    public int Dia_ATK_PER_UP_Lv
    {
        get
        {
            if (!PlayerPrefs.HasKey("Dia_ATK_PER_UP_Lv")) return 0;
            var tmp = PlayerPrefs.GetInt("Dia_ATK_PER_UP_Lv");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Dia_ATK_PER_UP_Lv", value);
            PlayerPrefs.Save();
        }
    }


    /// (string)다이아 체력 % 증가량 레벨                        Dia_HP_PER_UP_Lv
    public int Dia_HP_PER_UP_Lv
    {
        get
        {
            if (!PlayerPrefs.HasKey("Dia_HP_PER_UP_Lv")) return 0;
            var tmp = PlayerPrefs.GetInt("Dia_HP_PER_UP_Lv");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Dia_HP_PER_UP_Lv", value);
            PlayerPrefs.Save();
        }
    }



    /// (int)골드 크리티컬 확률 % 증가량 레벨                        CRC_UP_Lv
    public int CRC_UP_Lv
    {
        get
        {
            if (!PlayerPrefs.HasKey("CRC_UP_Lv")) return 0;
            var tmp = PlayerPrefs.GetInt("CRC_UP_Lv");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("CRC_UP_Lv", value);
            PlayerPrefs.Save();
        }
    }

    /// (int)골드 크리티컬 대미지 % 증가량 레벨                        CRD_UP_Lv
    public int CRD_UP_Lv
    {
        get
        {
            if (!PlayerPrefs.HasKey("CRD_UP_Lv")) return 0;
            var tmp = PlayerPrefs.GetInt("CRD_UP_Lv");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("CRD_UP_Lv", value);
            PlayerPrefs.Save();
        }
    }



    /// (int)다이아 크리티컬 확률 % 증가량 레벨                        Dia_CRC_UP_Lv
    public int Dia_CRC_UP_Lv
    {
        get
        {
            if (!PlayerPrefs.HasKey("Dia_CRC_UP_Lv")) return 0;
            var tmp = PlayerPrefs.GetInt("Dia_CRC_UP_Lv");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Dia_CRC_UP_Lv", value);
            PlayerPrefs.Save();
        }
    }

    /// (int)다이아 크리티컬 대미지 % 증가량 레벨                        Dia_CRD_UP_Lv
    public int Dia_CRD_UP_Lv
    {
        get
        {
            if (!PlayerPrefs.HasKey("Dia_CRD_UP_Lv")) return 0;
            var tmp = PlayerPrefs.GetInt("Dia_CRD_UP_Lv");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Dia_CRD_UP_Lv", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// (string)특별강화 치명타 % 증가량                       Dia_CRC_UP
    /// </summary>
    public string Dia_CRC_UP
    {
        get
        {
            if (!PlayerPrefs.HasKey("Dia_CRC_UP")) return "1";
            var tmp = PlayerPrefs.GetString("Dia_CRC_UP");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetString("Dia_CRC_UP", value);
            PlayerPrefs.Save();
        }
    }

    /// (string)다이아 크리댐 % 증가량                       Dia_CRD_UP
    public string Dia_CRD_UP
    {
        get
        {
            if (!PlayerPrefs.HasKey("Dia_CRD_UP")) return "1";
            var tmp = PlayerPrefs.GetString("Dia_CRD_UP");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetString("Dia_CRD_UP", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// (string)스탯 치명타 확률 % 증가량                       CRC_UP
    /// </summary>
    public string CRC_UP
    {
        get
        {
            if (!PlayerPrefs.HasKey("CRC_UP")) return "1";
            var tmp = PlayerPrefs.GetString("CRC_UP");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetString("CRC_UP", value);
            PlayerPrefs.Save();
        }
    }

    /// (string)다이아 크리댐 % 증가량                       CRD_UP
    public string CRD_UP
    {
        get
        {
            if (!PlayerPrefs.HasKey("CRD_UP")) return "1";
            var tmp = PlayerPrefs.GetString("CRD_UP");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetString("CRD_UP", value);
            PlayerPrefs.Save();
        }
    }


    /// (int)다이아 체력 회복 % 레벨
    public int Dia_RECOV_UP_Lv
    {
        get
        {
            var tmp = PlayerPrefs.GetInt("Dia_RECOV_UP_Lv", 0);
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Dia_RECOV_UP_Lv", value);
            PlayerPrefs.Save();
        }
    }







    /// <summary>
    /// 맷집 상승을 위해 누적한 대미지
    /// </summary>
    public float Mat_100
    {
        get
        {
            var tmp = PlayerPrefs.GetFloat("Mat_100", 0);
            return tmp;
        }

        set
        {
            PlayerPrefs.SetFloat("Mat_100", value);
            PlayerPrefs.Save();
        }
    }

    /// (int)스킬 300카운트                 Mat_Skill_500
    public int Mat_Skill_300
    {
        get
        {
            if (!PlayerPrefs.HasKey("Mat_Skill_300")) return 0;
            var tmp = PlayerPrefs.GetInt("Mat_Skill_300");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Mat_Skill_300", value);
            PlayerPrefs.Save();
        }
    }

    public bool isEmptyLuckBox;

    private int luckyBoxCount;

    public int LuckyBoxCount
    { 
        get
        {
            if (luckyBoxCount > 3) luckyBoxCount = 3;
            else if (luckyBoxCount < 0) luckyBoxCount = 0;
            return luckyBoxCount; 
        }

        set
        {
            luckyBoxCount = value;
        }
    }

    /// (float)아이템 획득확률<럭키박스>                 LuckyProb
    public float LuckyProb
    {
        get
        {
            if (!PlayerPrefs.HasKey("LuckyProb")) return 1.0f;
            var tmp = PlayerPrefs.GetFloat("LuckyProb");
            return tmp;
        }
    }

    /// (int)공격력 레벨                          ATK_Lv
    public int ATK_Lv
    {
        get
        {
            if (!PlayerPrefs.HasKey("ATK_Lv")) return 0;
            var tmp = PlayerPrefs.GetInt("ATK_Lv");
            return tmp;
        }

        set
        {
            /// 치킨 감지
            if (value - PlayerPrefs.GetInt("ATK_Lv") != 1)
            {
                playNANOO.WriteChikenCoupon("FISHING_ATK_Lv", $"시도 값 : {value}");
                return;
            }

            PlayerPrefs.SetInt("ATK_Lv", value);
            PlayerPrefs.Save();
        }
    }

    /// (int)체력 레벨                            Mat_HP_Lv
    public int Mat_HP_Lv
    {
        get
        {
            if (!PlayerPrefs.HasKey("Mat_HP_Lv")) return 0;
            var tmp = PlayerPrefs.GetInt("Mat_HP_Lv");
            return tmp;
        }

        set
        {
            /// 치킨 감지
            if (value - PlayerPrefs.GetInt("Mat_HP_Lv") != 1)
            {
                playNANOO.WriteChikenCoupon("FISHING_Mat_HP_Lv", $"시도 값 : {value}");
                return;
            }
            PlayerPrefs.SetInt("Mat_HP_Lv", value);
            PlayerPrefs.Save();
        }
    }

    /// (int)회복량 레벨                          Recov_Lv
    public int Recov_Lv
    {
        get
        {
            if (!PlayerPrefs.HasKey("Recov_Lv")) return 0;
            var tmp = PlayerPrefs.GetInt("Recov_Lv");
            return tmp;
        }

        set
        {
            /// 치킨 감지
            if (value - PlayerPrefs.GetInt("Recov_Lv") != 1)
            {
                playNANOO.WriteChikenCoupon("FISHING_Recov_Lv", $"시도 값 : {value}");
                return;
            }
            PlayerPrefs.SetInt("Recov_Lv", value);
            PlayerPrefs.Save();
        }
    }




    /// (float)그로기 터치 횟수                          GroggyTouch
    public float GroggyTouch
    {
        get
        {
            if (!PlayerPrefs.HasKey("GroggyTouch")) return 5.0f;
            var tmp = PlayerPrefs.GetFloat("GroggyTouch");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetFloat("GroggyTouch", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// (int)런닝화- 초당 터치 횟수 증가          Arti_PunchTouch
    /// </summary>
    public int Arti_PunchTouch
    {
        get
        {
            if (!PlayerPrefs.HasKey("Arti_PunchTouch")) return 0;
            var tmp = PlayerPrefs.GetInt("Arti_PunchTouch");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Arti_PunchTouch", value);
            PlayerPrefs.Save();
        }
    }

    /// (int)헤드기어- 맷집증가                   Arti_Mattzip
    public int Arti_Mattzip
    {
        get
        {
            if (!PlayerPrefs.HasKey("Arti_Mattzip")) return 0;
            var tmp = PlayerPrefs.GetInt("Arti_Mattzip");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Arti_Mattzip", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// (int)줄넘기- 체력 증가 레벨                     Arti_HP
    /// </summary>
    public int Arti_HP
    {
        get
        {
            if (!PlayerPrefs.HasKey("Arti_HP")) return 0;
            var tmp = PlayerPrefs.GetInt("Arti_HP");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Arti_HP", value);
            PlayerPrefs.Save();
        }
    }

    /// (int)물총- 그로기 상태 짧아짐             Arti_GroggyTouch
    public int Arti_GroggyTouch
    {
        get
        {
            if (!PlayerPrefs.HasKey("Arti_GroggyTouch")) return 0;
            var tmp = PlayerPrefs.GetInt("Arti_GroggyTouch");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Arti_GroggyTouch", value);
            PlayerPrefs.Save();
        }
    }

    /// (int)국밥 - 국밥 스킬 지속 시간 증가                Arti_GAL
    public int Arti_GAL
    {
        get
        {
            if (!PlayerPrefs.HasKey("Arti_GAL")) return 0;
            var tmp = PlayerPrefs.GetInt("Arti_GAL");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Arti_GAL", value);
            PlayerPrefs.Save();
        }
    }


    /// (int)모래시계- 방어전 시간 감소                Arti_DefenceTime
    public int Arti_DefenceTime
    {
        get
        {
            if (!PlayerPrefs.HasKey("Arti_DefenceTime")) return 0;
            var tmp = PlayerPrefs.GetInt("Arti_DefenceTime");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Arti_DefenceTime", value);
            PlayerPrefs.Save();
        }
    }

    /// (int)선물상자- 선물 상자 등장확률 증가                Arti_GoldBox
    public int Arti_GoldBox
    {
        get
        {
            if (!PlayerPrefs.HasKey("Arti_GoldBox")) return 0;
            var tmp = PlayerPrefs.GetInt("Arti_GoldBox");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Arti_GoldBox", value);
            PlayerPrefs.Save();
        }
    }

    /// (int)머니건- 방치 골드 획득량 증가                Arti_OffGold
    public int Arti_OffGold
    {
        get
        {
            if (!PlayerPrefs.HasKey("Arti_OffGold")) return 0;
            var tmp = PlayerPrefs.GetInt("Arti_OffGold");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Arti_OffGold", value);
            PlayerPrefs.Save();
        }
    }

    /// (int)땡땡 종 - 무한의 탑 시간 증가                Arti_MuganTime
    public int Arti_MuganTime
    {
        get
        {
            if (!PlayerPrefs.HasKey("Arti_MuganTime")) return 0;
            var tmp = PlayerPrefs.GetInt("Arti_MuganTime");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Arti_MuganTime", value);
            PlayerPrefs.Save();
        }
    }

    /// (int)샌드백 - 공격력 증가                Arti_AttackPower
    public int Arti_AttackPower
    {
        get
        {
            if (!PlayerPrefs.HasKey("Arti_AttackPower")) return 0;
            var tmp = PlayerPrefs.GetInt("Arti_AttackPower");


            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Arti_AttackPower", value);
            PlayerPrefs.Save();
        }
    }

    //
    //
    // 0608
    //
    //

    /// (int)     -골드 획득 증가                
    public int Arti_GoldPer
    {
        get
        {
            var tmp = PlayerPrefs.GetInt("Arti_GoldPer", 0);
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Arti_GoldPer", value);
            PlayerPrefs.Save();
        }
    }

    /// (int)       -타격 선물상자 획득량 증가                
    public int Arti_LuckyBoxPer
    {
        get
        {
            var tmp = PlayerPrefs.GetInt("Arti_LuckyBoxPer", 0);
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Arti_LuckyBoxPer", value);
            PlayerPrefs.Save();
        }
    }

    /// (int)       -방어전 대미지 % 감소                
    public int Arti_DefencePer
    {
        get
        {
            var tmp = PlayerPrefs.GetInt("Arti_DefencePer", 0);
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Arti_DefencePer", value);
            PlayerPrefs.Save();
        }
    }

    /// (int)       -골드 강화 비용 감소                
    public int Arti_GoldUpgrade
    {
        get
        {
            var tmp = PlayerPrefs.GetInt("Arti_GoldUpgrade", 0);
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Arti_GoldUpgrade", value);
            PlayerPrefs.Save();
        }
    }

    /// (int)       -무한 버티기 보상 강화    국밥            
    public int Arti_InfiReword
    {
        get
        {
            var tmp = PlayerPrefs.GetInt("Arti_InfiReword", 0);
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Arti_InfiReword", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// (int)       -미니게임 보상 강화      쌀밥     
    /// Max = 1000
    /// </summary>
    public int Arti_MiniReword
    {
        get
        {
            var tmp = PlayerPrefs.GetInt("Arti_MiniReword", 0);
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Arti_MiniReword", value);
            PlayerPrefs.Save();
        }
    }

    /// (int)       -미니게임 시간 증가             
    public int Arti_MiniGameTime
    {
        get
        {
            var tmp = PlayerPrefs.GetInt("Arti_MiniGameTime", 0);
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Arti_MiniGameTime", value);
            PlayerPrefs.Save();
        }
    }





    /// <summary>
    /// 비브라늄 - 방어력 증가 유물
    /// </summary>
    public int Arti_DEF_UP
    {
        get
        {
            var tmp = PlayerPrefs.GetInt("Arti_DEF_UP", 0);
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Arti_DEF_UP", value);
            PlayerPrefs.Save();
        }
    }
    /// <summary>
    /// 강철망치 - 방패 방어력 증가 유물
    /// </summary>
    public int Arti_SHILED_UP
    {
        get
        {
            var tmp = PlayerPrefs.GetInt("Arti_SHILED_UP", 0);
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Arti_SHILED_UP", value);
            PlayerPrefs.Save();
        }
    }
    /// <summary>
    /// 에너지드링크 - 체력회복력 증가 유물
    /// </summary>
    public int Arti_HEALLING_UP
    {
        get
        {
            var tmp = PlayerPrefs.GetInt("Arti_HEALLING_UP", 0);
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Arti_HEALLING_UP", value);
            PlayerPrefs.Save();
        }
    }
    /// <summary>
    /// 무 - 깍두기 획득량 증가
    /// </summary>
    public int Arti_KIMCHI_UP
    {
        get
        {
            var tmp = PlayerPrefs.GetInt("Arti_KIMCHI_UP", 0);
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Arti_KIMCHI_UP", value);
            PlayerPrefs.Save();
        }
    }
    /// <summary>
    /// 방탄조끼 - 맷집 증가량 증가
    /// </summary>
    public int Arti_MattGrow_UP
    {
        get
        {
            var tmp = PlayerPrefs.GetInt("Arti_MattGrow_UP", 0);
            return tmp;
        }

        set
        {
            PlayerPrefs.SetInt("Arti_MattGrow_UP", value);
            PlayerPrefs.Save();
        }
    }











    /// 1번 깃발 버프 - 체력 회복력 레벨
    public int is1Recov
    {
        get
        {
            if (!PlayerPrefs.HasKey("is1Recov")) return 0;
            var tmp = PlayerPrefs.GetInt("is1Recov");
            return tmp;
        }

        set
        {
            if (value - PlayerPrefs.GetInt("is1Recov") != 1)
            {
                playNANOO.WriteChikenCoupon("FISHING_is1Recov", $"시도 값 : {value}");
                return;
            }
            PlayerPrefs.SetInt("is1Recov", value);
            PlayerPrefs.Save();
        }
    }

    /// 2번 깃발 버프 - 체력 레벨
    public int is2Stamina
    {
        get
        {
            if (!PlayerPrefs.HasKey("is2Stamina")) return 0;
            var tmp = PlayerPrefs.GetInt("is2Stamina");
            return tmp;
        }

        set
        {
            if (value - PlayerPrefs.GetInt("is2Stamina") != 1)
            {
                playNANOO.WriteChikenCoupon("FISHING_is2Stamina", $"시도 값 : {value}");
                return;
            }
            PlayerPrefs.SetInt("is2Stamina", value);
            PlayerPrefs.Save();
        }
    }

    /// 3번 깃발 버프 - 공격 레벨
    public int is3ATK
    {
        get
        {
            if (!PlayerPrefs.HasKey("is3ATK")) return 0;
            var tmp = PlayerPrefs.GetInt("is3ATK");
            return tmp;
        }

        set
        {
            if (value - PlayerPrefs.GetInt("is3ATK") != 1)
            {
                playNANOO.WriteChikenCoupon("FISHING_is3ATK", $"시도 값 : {value}");
                return;
            }
            PlayerPrefs.SetInt("is3ATK", value);
            PlayerPrefs.Save();
        }
    }

    /// 4번 깃발 버프 - 방어력 레벨
    public int is4Mattzip
    {
        get
        {
            if (!PlayerPrefs.HasKey("is4Mattzip")) return 0;
            var tmp = PlayerPrefs.GetInt("is4Mattzip");
            return tmp;
        }

        set
        {
            if (value - PlayerPrefs.GetInt("is4Mattzip") != 1)
            {
                playNANOO.WriteChikenCoupon("FISHING_is4Mattzip", $"시도 값 : {value}");
                return;
            }
            PlayerPrefs.SetInt("is4Mattzip", value);
            PlayerPrefs.Save();
        }
    }




    /// <summary>
    /// 깃발 버프 <체력 회복력>
    /// </summary>
    public float Stat_is1Recov
    {
        get
        {
            if (!PlayerPrefs.HasKey("Stat_is1Recov")) return 0;
            var tmp = PlayerPrefs.GetFloat("Stat_is1Recov");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetFloat("Stat_is1Recov", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// 깃발 버프 <체력>
    /// </summary>
    public float Stat_is2Stamina
    {
        get
        {
            if (!PlayerPrefs.HasKey("Stat_is2Stamina")) return 0;
            var tmp = PlayerPrefs.GetFloat("Stat_is2Stamina");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetFloat("Stat_is2Stamina", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// 깃발 버프 <공격력>
    /// </summary>
    public float Stat_is3ATK
    {
        get
        {
            if (!PlayerPrefs.HasKey("Stat_is3ATK")) return 0;
            var tmp = PlayerPrefs.GetFloat("Stat_is3ATK");
            return tmp;
        }

        set
        {
            PlayerPrefs.SetFloat("Stat_is3ATK", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// 깃발 버프 <방어력>
    /// </summary>
    public float Stat_is4Deffence
    {
        get
        {
            if (!PlayerPrefs.HasKey("Stat_is4Mattzip")) return 0;
            var tmp = PlayerPrefs.GetFloat("Stat_is4Mattzip");
            return tmp * 0.01f;
        }

        set
        {
            PlayerPrefs.SetFloat("Stat_is4Mattzip", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// 랭킹 등록용 최대 국밥.
    /// </summary>
    public int MaxGet_GookBap
    {
        get
        {
            if (!PlayerPrefs.HasKey("MaxGet_GookBap")) return 0;
            var _MaxGet_GookBap = PlayerPrefs.GetInt("MaxGet_GookBap");
            return _MaxGet_GookBap;
        }

        set
        {
            // 값비교
            if (value >= MaxGet_GookBap)
            {
                PlayerPrefs.SetInt("MaxGet_GookBap", value);
                PlayerPrefs.Save();
            }

        }
    }

    /// <summary>
    /// (시작값 1) 랭킹 등록용 최대 무한의 탑 정복 수
    /// </summary>
    public int MaxGet_MuganTop
    {
        ///
        ///     0513 업데이트 전 이전 유저는 PlayerPrefs.GetInt("MaxGet_MuganTop");
        ///
        get
        {
            var tmp = PlayerPrefs.GetInt("MaxGet_MuganTop2", 1);
            return tmp;
        }

        set
        {
            // 값비교
            if (value >= PlayerPrefs.GetInt("MaxGet_MuganTop2", 1))
            {
                PlayerPrefs.SetInt("MaxGet_MuganTop2", value);
                PlayerPrefs.Save();
                // 무탑 랭킹 등록
                playNANOO.RankingRecordMuganTop();
            }

        }
    }


    /// <summary>
    /// 랭킹 등록용 최대 미니 게임 콤보
    /// </summary>
    public int MaxGet_MiniGame
    {
        get
        {
            if (!PlayerPrefs.HasKey("MaxGet_MiniGame")) return 0;
            var tmp = PlayerPrefs.GetInt("MaxGet_MiniGame");
            return tmp;
        }

        set
        {
            // 값비교
            if (value >= MaxGet_MiniGame)
            {
                PlayerPrefs.SetInt("MaxGet_MiniGame", value);
                PlayerPrefs.Save();
                playNANOO.RankingRecordMinini();

            }

        }
    }




    /// <summary>
    /// 맷집 증가에 필요한 대미지 게이지 (맷집 게이지 감소 장신구 적용) 
    /// </summary>
    public float Cilcked_Cnt_MattZip
    {
        get
        {
            var tmp = PlayerPrefs.GetFloat("Cilcked_Cnt_MattZip", 5f);
            tmp = tmp - (tmp * Pet_Touch_Lv * 0.01f);
            return tmp;
        }

        set
        {
            PlayerPrefs.SetFloat("Cilcked_Cnt_MattZip", value);
            PlayerPrefs.Save();
        }
    }



    /// <summary>
    /// 훈련장비 다이아 구매시 → 공격력 증가로 수정
    /// </summary>
    public float Punch_Dia_Weap
    {
        get
        {
            var tmp = PlayerPrefs.GetFloat("Mattzip_Dia_Weap", 0);
            return tmp;
        }

        set
        {
            PlayerPrefs.SetFloat("Mattzip_Dia_Weap", value);
            PlayerPrefs.Save();
        }
    }

    /// (int)유저 레벨                          Chara_Lv
    public int Chara_Lv
    {
        get
        {
            var tmp = PlayerPrefs.GetInt("Chara_Lv", 1);
            return tmp;
        }

        set
        {
            if (value - PlayerPrefs.GetInt("Chara_Lv", 1) != 1)
            {
                playNANOO.WriteChikenCoupon("FISHING_Chara_Lv", $"시도 값 : {value}");
                return;
            }
            PlayerPrefs.SetInt("Chara_Lv", value);
            PlayerPrefs.Save();
        }
    }














    /// <summary>
    /// 1. 맷집 증가에 필요한 대미지 게이지 감소 %
    /// </summary>
    public int Pet_Touch_Lv
    {
        get
        {
            var tmp = PlayerPrefs.GetInt("Pet_Touch_Lv", 0);
            return tmp;
        }

        set
        {
            if (value - PlayerPrefs.GetInt("Pet_Touch_Lv") != 1)
            {
                playNANOO.WriteChikenCoupon("FISHING_Pet_Touch_Lv", $"시도 값 : {value}");
                return;
            }
            PlayerPrefs.SetInt("Pet_Touch_Lv", value);
            PlayerPrefs.Save();
        }
    }


    /// <summary>
    /// 2. 국밥 버프 요구 터치 횟수 차감 ( 회 )
    /// </summary>
    public int Pet_Buff_Lv
    {
        get
        {
            var tmp = PlayerPrefs.GetInt("Pet_Buff_Lv", 0);
            return tmp;
        }

        set
        {
            if (value - PlayerPrefs.GetInt("Pet_Buff_Lv") != 1)
            {
                playNANOO.WriteChikenCoupon("FISHING_Pet_Buff_Lv", $"시도 값 : {value}");
                return;
            }
            PlayerPrefs.SetInt("Pet_Buff_Lv", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// 3. 맷집 증가량 증가 (현재 공격력의 1 %)( % )"
    /// </summary>
    public int Pet_Matt_Up_Lv
    {
        get
        {
            var tmp = PlayerPrefs.GetInt("Pet_Matt_Up_Lv", 0);
            return tmp;
        }

        set
        {
            if (value - PlayerPrefs.GetInt("Pet_Matt_Up_Lv") != 1)
            {
                playNANOO.WriteChikenCoupon("FISHING_Pet_Matt_Up_Lv", $"시도 값 : {value}");
                return;
            }
            PlayerPrefs.SetInt("Pet_Matt_Up_Lv", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// 4. PVP 맷집 적용 퍼센트 증가
    /// </summary>
    public int Pet_PVP_Matt_Lv
    {
        get
        {
            var tmp = PlayerPrefs.GetInt("Pet_PVP_Matt_Lv", 0);
            return tmp;
        }

        set
        {
            if (value - PlayerPrefs.GetInt("Pet_PVP_Matt_Lv") != 1)
            {
                playNANOO.WriteChikenCoupon("FISHING_Pet_PVP_Matt_Lv", $"시도 값 : {value}");
                return;
            }
            PlayerPrefs.SetInt("Pet_PVP_Matt_Lv", value);
            PlayerPrefs.Save();
        }
    }


    //Pet_PVP_Speed_Lv

    /// <summary>
    /// 5. PVP 속도증가 퍼센트 증가
    /// </summary>
    public int Pet_PVP_Speed_Lv
    {
        get
        {
            var tmp = PlayerPrefs.GetInt("Pet_PVP_Speed_Lv", 0);
            return tmp;
        }

        set
        {
            if (value - PlayerPrefs.GetInt("Pet_PVP_Speed_Lv") != 1)
            {
                playNANOO.WriteChikenCoupon("FISHING_Pet_PVP_Speed_Lv", $"시도 값 : {value}");
                return;
            }
            PlayerPrefs.SetInt("Pet_PVP_Speed_Lv", value);
            PlayerPrefs.Save();
        }
    }


    /// <summary>
    /// 1번 동료 - 오프라인 공격력 증가
    /// </summary>
    public int Friend_01_OfflineAtk_Lv
    {
        get
        {
            var tmp = PlayerPrefs.GetInt("Friend_01_OfflineAtk_Lv", 0);
            return tmp;
        }

        set
        {
            if (value - PlayerPrefs.GetInt("Friend_01_OfflineAtk_Lv") != 1)
            {
                playNANOO.WriteChikenCoupon("FISHING_Friend_01_OfflineAtk_Lv", $"시도 값 : {value}");
                return;
            }
            PlayerPrefs.SetInt("Friend_01_OfflineAtk_Lv", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// 2번 동료 - 오프라인 시간 증가
    /// </summary>
    public int Friend_02_OffTimeUp_Lv
    {
        get
        {
            var tmp = PlayerPrefs.GetInt("Friend_02_OffTimeUp_Lv", 0);
            return tmp;
        }

        set
        {
            if (value - PlayerPrefs.GetInt("Friend_02_OffTimeUp_Lv") != 1)
            {
                playNANOO.WriteChikenCoupon("FISHING_Friend_02_OffTimeUp_Lv", $"시도 값 : {value}");
                return;
            }
            PlayerPrefs.SetInt("Friend_02_OffTimeUp_Lv", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// 오프라인 공격속도 증가
    /// </summary>
    public int Friend_03_OffSpped_Lv
    {
        get
        {
            var tmp = PlayerPrefs.GetInt("Friend_03_OffSpped_Lv", 0);
            return tmp;
        }

        set
        {
            if (value - PlayerPrefs.GetInt("Friend_03_OffSpped_Lv") != 1)
            {
                playNANOO.WriteChikenCoupon("FISHING_Friend_03_OffSpped_Lv", $"시도 값 : {value}");
                return;
            }
            PlayerPrefs.SetInt("Friend_03_OffSpped_Lv", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// 오프라인 맷집 증가율 증가
    /// </summary>
    public int Friend_04_MattzipPer_Lv
    {
        get
        {
            var tmp = PlayerPrefs.GetInt("Friend_04_MattzipPer_Lv", 0);
            return tmp;
        }

        set
        {
            if (value - PlayerPrefs.GetInt("Friend_04_MattzipPer_Lv") != 1)
            {
                playNANOO.WriteChikenCoupon("FISHING_Friend_04_MattzipPer_Lv", $"시도 값 : {value}");
                return;
            }
            PlayerPrefs.SetInt("Friend_04_MattzipPer_Lv", value);
            PlayerPrefs.Save();
        }
    }














    #endregion


    ///----------------------------------------------------------------------------------
    /// 
    /// (int)현재 무기 레벨 1/100               weaponLevel
    /// (string)레벨업시 소모 골드              weaponCost
    /// (bool)무기 잠겼는지 확인                isUnlock
    /// 
    ///----------------------------------------------------------------------------------

    #region 무기 관리 정보


    /// <summary>
    /// 무기 관리
    /// </summary>
    [Serializable]
    public class WeaponEntry
    {
        public int weaponLevel; // 현재 무기 레벨 1/100
        public string weaponCost; // 레벨업시 소모 골드
        public float weaponEffect; // 해당 레벨 효과
        //
        public bool isUnlock; // 다음 레벨 해제 했냐

    }

    // 만든 클래스를 리스트에 담아서 테이블처럼 사용
    public List<WeaponEntry> weaponInfo = new List<WeaponEntry>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="wl">현재 무기 레벨 1/100</param>
    /// <param name="wc">레벨업시 소모 골드</param>
    /// <param name="we">해당 레벨 효과</param>
    /// <param name="isUnlock">다음 레벨 해제 했냐</param>
    public void AddWeaponData(int wl, string wc, float we, bool unlock)
    {
        //새로운 데이터를 추가해주고
        weaponInfo.Add(new WeaponEntry
        {
            weaponLevel = wl,
            weaponCost = wc,
            weaponEffect = we,
            isUnlock = unlock
        });
    }

    [HideInInspector]
    public bool isReadyWeapon;

    public void SaveWeaponInfo()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();

        // Info를 바이트 배열로 변환해서 저장
        binaryFormatter.Serialize(memoryStream, weaponInfo);

        // 그것을 다시 한번 문자열 값으로 변환해서 
        // 스트링 키값으로 PlayerPrefs에 저장
        PlayerPrefs.SetString("weaponInfo", Convert.ToBase64String(memoryStream.GetBuffer()));
        PlayerPrefs.Save();

        isReadyWeapon = true;
    }

    public void LoadWeaponInfo()
    {
        string data = PlayerPrefs.GetString("weaponInfo");

        if (!string.IsNullOrEmpty(data))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(data));

            // 가져온 데이터를 바이트 배열로 변환 -> 리스트로 캐스팅
            weaponInfo = (List<WeaponEntry>)binaryFormatter.Deserialize(memoryStream);

            /// 공격력 적용 수치를 변경함 값으로 바꿔줌.
            for (int i = 0; i < weaponInfo.Count; i++)
            {
                weaponInfo[i].weaponEffect = (weaponInfo[i].weaponLevel * (0.1f * (i + 1)));
            }
        }

        /// 방패 리스팅도 같이 해줘라.
        LoadShieldInfo();

        isReadyWeapon = true;
    }


    #endregion


    #region 방패 관리 정보


    /// <summary>
    /// 무기 관리
    /// </summary>
    [Serializable]
    public class ShieldEntry
    {
        public int shieldLevel;         // 맥스레벨은 100
        public bool isUnlock;           // 최초 획득했는지?
        //                                      [0] 등급 abc
        public float equipedEffect;         // [1] 장착 %    
        public float ownedEffect;           // [2] 보유 %
        public float powerUpper;            // [3] 강화 성공률
        public float powerMinusPer;         // [4] 성공 차감율
        public float upperEfect;            // [5] 레벨당 방어력 증가치 %
        public string shieldCost;           // [6] 레벨업시 소모 깍두기
        //
        public int amount;
    }

    // 만든 클래스를 리스트에 담아서 테이블처럼 사용
    public List<ShieldEntry> shieldInfo = new List<ShieldEntry>();

    /// <summary>
    /// 실드 총 갯수만큼 반복
    /// </summary>
    public void AddShieldData(float row_1, float row_2, float row_3, float row_4, float row_5, string row_6)
    {
        shieldInfo.Add(new ShieldEntry
        {
            shieldLevel = 0,
            isUnlock = false,
            //
            equipedEffect = row_1,
            ownedEffect = row_2,
            powerUpper = row_3,
            powerMinusPer = row_4,
            upperEfect = row_5,
            shieldCost = row_6,
            amount = 0
        });
    }




    public List<IAPManager.PakageEntry> CybermanInfo(string data)
    {
        if (!string.IsNullOrEmpty(data))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(data));

            // 가져온 데이터를 바이트 배열로 변환 -> 리스트로 캐스팅
            return (List<IAPManager.PakageEntry>)binaryFormatter.Deserialize(memoryStream);
        }
        else
        {
            return null;
        }
    }



    /// <summary>
    /// 실드 현재 정보 저장
    /// </summary>
    public void SaveShieldInfo()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();

        // Info를 바이트 배열로 변환해서 저장
        binaryFormatter.Serialize(memoryStream, shieldInfo);

        // 그것을 다시 한번 문자열 값으로 변환해서 
        // 스트링 키값으로 PlayerPrefs에 저장
        PlayerPrefs.SetString("shieldInfo", Convert.ToBase64String(memoryStream.GetBuffer()));
        PlayerPrefs.Save();
    }

    public void LoadShieldInfo()
    {
        string data = PlayerPrefs.GetString("shieldInfo");

        if (!string.IsNullOrEmpty(data))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(data));

            // 가져온 데이터를 바이트 배열로 변환 -> 리스트로 캐스팅
            shieldInfo = (List<ShieldEntry>)binaryFormatter.Deserialize(memoryStream);
        }
    }



    #endregion







    #region 업적 관리 정보


    /// <summary>
    /// 업적 관리임
    /// </summary>
    [Serializable]
    public class QuestEntry
    {
        public int daily_Abs;
        public int daily_Atk;
        public int daily_HP;
        public int daily_Punch;
        /// <summary>
        /// 무한 버티기 5회 하기
        /// </summary>
        public int daily_MiniCombo;
        public int daily_ArtiGatcha;
        public int daily_LMITABS;

        //
        public long All_Mattzip;
        public long All_Atk;
        public long All_HP;
        public long All_Punch;
        public long All_MiniGame;
        public long All_Gatcha;
        public long All_Abs;
        //
        public long All_Mattzip_Cnt;
        public long All_Atk_Cnt;
        public long All_HP_Cnt;
        public long All_Punch_Cnt;
        public long All_MiniGame_Cnt;
        public long All_Gatcha_Cnt;
        public long All_Abs_Cnt;
        //
        public long Pun_01;
        public long Pun_02;
        public long Pun_03;
        public long Pun_04;
        public long Pun_05;
        public long Pun_06;
        public long Pun_07;
        public long Pun_08;
        public long Pun_09;
        public long Pun_10;
        public long Pun_11;
        public long Pun_12;
        public long Pun_13;
        public long Pun_14;
        public long Pun_15;
        public long Pun_16;
        public long Pun_17;
        public long Pun_18;
        public long Pun_19;
        public long Pun_20;
        public long Pun_21;
        public long Pun_22;
        public long Pun_23;
        public long Pun_24;
        public long Pun_25;
        public long Pun_26;
        public long Pun_27;
        public long Pun_28;
        public long Pun_29;
        public long Pun_30;
        public long Pun_31;
        public long Pun_32;
        public long Pun_33;
        public long Pun_34;
        public long Pun_35;
        public long Pun_36;
        public long Pun_37;
        public long Pun_38;
        public long Pun_39;
        public long Pun_40;
        public long Pun_41;
        public long Pun_42;
        public long Pun_43;
        public long Pun_44;
        public long Pun_45;
        public long Pun_46;
        public long Pun_47;
        public long Pun_48;
        public long Pun_49;
        public long Pun_50;
        //
        public long Pun_01_Cnt;
        public long Pun_02_Cnt;
        public long Pun_03_Cnt;
        public long Pun_04_Cnt;
        public long Pun_05_Cnt;
        public long Pun_06_Cnt;
        public long Pun_07_Cnt;
        public long Pun_08_Cnt;
        public long Pun_09_Cnt;
        public long Pun_10_Cnt;
        public long Pun_11_Cnt;
        public long Pun_12_Cnt;
        public long Pun_13_Cnt;
        public long Pun_14_Cnt;
        public long Pun_15_Cnt;
        public long Pun_16_Cnt;
        public long Pun_17_Cnt;
        public long Pun_18_Cnt;
        public long Pun_19_Cnt;
        public long Pun_20_Cnt;
        public long Pun_21_Cnt;
        public long Pun_22_Cnt;
        public long Pun_23_Cnt;
        public long Pun_24_Cnt;
        public long Pun_25_Cnt;
        public long Pun_26_Cnt;
        public long Pun_27_Cnt;
        public long Pun_28_Cnt;
        public long Pun_29_Cnt;
        public long Pun_30_Cnt;
        public long Pun_31_Cnt;
        public long Pun_32_Cnt;
        public long Pun_33_Cnt;
        public long Pun_34_Cnt;
        public long Pun_35_Cnt;
        public long Pun_36_Cnt;
        public long Pun_37_Cnt;
        public long Pun_38_Cnt;
        public long Pun_39_Cnt;
        public long Pun_40_Cnt;
        public long Pun_41_Cnt;
        public long Pun_42_Cnt;
        public long Pun_43_Cnt;
        public long Pun_44_Cnt;
        public long Pun_45_Cnt;
        public long Pun_46_Cnt;
        public long Pun_47_Cnt;
        public long Pun_48_Cnt;
        public long Pun_49_Cnt;
        public long Pun_50_Cnt;

    }

    // 만든 클래스를 리스트에 담아서 테이블처럼 사용
    public List<QuestEntry> questInfo = new List<QuestEntry>();

    public void AddQuestData()
    {
        Debug.LogError("AddQuestData()");
        //새로운 데이터를 추가해주고
        questInfo.Add(new QuestEntry
        {
            daily_Abs = 0,
            daily_Atk = 0,
            daily_HP = 0,
            daily_Punch = 0,
            daily_MiniCombo = 0,
            daily_ArtiGatcha = 0,
            daily_LMITABS = 0,
            All_Mattzip = 0,
            All_Atk = 0,
            All_HP = 0,
            All_Punch = 0,
            All_MiniGame = 0,
            All_Gatcha = 0,
            All_Abs = 0,
            All_Mattzip_Cnt = 0,
            All_Atk_Cnt = 0,
            All_HP_Cnt = 0,
            All_Punch_Cnt = 0,
            All_MiniGame_Cnt = 0,
            All_Gatcha_Cnt = 0,
            All_Abs_Cnt = 0,
            Pun_01 = 0,
            Pun_02 = 0,
            Pun_03 = 0,
            Pun_04 = 0,
            Pun_05 = 0,
            Pun_06 = 0,
            Pun_07 = 0,
            Pun_08 = 0,
            Pun_09 = 0,
            Pun_10 = 0,
            Pun_11 = 0,
            Pun_12 = 0,
            Pun_13 = 0,
            Pun_14 = 0,
            Pun_15 = 0,
            Pun_16 = 0,
            Pun_17 = 0,
            Pun_18 = 0,
            Pun_19 = 0,
            Pun_20 = 0,
            Pun_21 = 0,
            Pun_22 = 0,
            Pun_23 = 0,
            Pun_24 = 0,
            Pun_25 = 0,
            Pun_26 = 0,
            Pun_27 = 0,
            Pun_28 = 0,
            Pun_29 = 0,
            Pun_30 = 0,
            Pun_31 = 0,
            Pun_32 = 0,
            Pun_33 = 0,
            Pun_34 = 0,
            Pun_35 = 0,
            Pun_36 = 0,
            Pun_37 = 0,
            Pun_38 = 0,
            Pun_39 = 0,
            Pun_40 = 0,
            Pun_41 = 0,
            Pun_42 = 0,
            Pun_43 = 0,
            Pun_44 = 0,
            Pun_45 = 0,
            Pun_46 = 0,
            Pun_47 = 0,
            Pun_48 = 0,
            Pun_49 = 0,
            Pun_50 = 0,
            Pun_01_Cnt = 0,
            Pun_02_Cnt = 0,
            Pun_03_Cnt = 0,
            Pun_04_Cnt = 0,
            Pun_05_Cnt = 0,
            Pun_06_Cnt = 0,
            Pun_07_Cnt = 0,
            Pun_08_Cnt = 0,
            Pun_09_Cnt = 0,
            Pun_10_Cnt = 0,
            Pun_11_Cnt = 0,
            Pun_12_Cnt = 0,
            Pun_13_Cnt = 0,
            Pun_14_Cnt = 0,
            Pun_15_Cnt = 0,
            Pun_16_Cnt = 0,
            Pun_17_Cnt = 0,
            Pun_18_Cnt = 0,
            Pun_19_Cnt = 0,
            Pun_20_Cnt = 0,
            Pun_21_Cnt = 0,
            Pun_22_Cnt = 0,
            Pun_23_Cnt = 0,
            Pun_24_Cnt = 0,
            Pun_25_Cnt = 0,
            Pun_26_Cnt = 0,
            Pun_27_Cnt = 0,
            Pun_28_Cnt = 0,
            Pun_29_Cnt = 0,
            Pun_30_Cnt = 0,
            Pun_31_Cnt = 0,
            Pun_32_Cnt = 0,
            Pun_33_Cnt = 0,
            Pun_34_Cnt = 0,
            Pun_35_Cnt = 0,
            Pun_36_Cnt = 0,
            Pun_37_Cnt = 0,
            Pun_38_Cnt = 0,
            Pun_39_Cnt = 0,
            Pun_40_Cnt = 0,
            Pun_41_Cnt = 0,
            Pun_42_Cnt = 0,
            Pun_43_Cnt = 0,
            Pun_44_Cnt = 0,
            Pun_45_Cnt = 0,
            Pun_46_Cnt = 0,
            Pun_47_Cnt = 0,
            Pun_48_Cnt = 0,
            Pun_49_Cnt = 0,
            Pun_50_Cnt = 0
        });

        SavequestInfo();
    }

    [HideInInspector]
    public bool isReadyQuest;

    public void SavequestInfo()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();

        // Info를 바이트 배열로 변환해서 저장
        binaryFormatter.Serialize(memoryStream, questInfo);

        // 그것을 다시 한번 문자열 값으로 변환해서 
        // 스트링 키값으로 PlayerPrefs에 저장
        PlayerPrefs.SetString("questInfo", Convert.ToBase64String(memoryStream.GetBuffer()));
        PlayerPrefs.Save();
        isReadyQuest = true;
    }

    public void LoadquestInfo()
    {
        string data = PlayerPrefs.GetString("questInfo");
        Debug.LogError("LoadquestInfo() ㄴ" + data + "ㄱ");
        if (!string.IsNullOrEmpty(data))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(data));

            // 가져온 데이터를 바이트 배열로 변환 -> 리스트로 캐스팅
            questInfo = (List<QuestEntry>)binaryFormatter.Deserialize(memoryStream);
        }

        isReadyQuest = true;
    }


    #endregion



    #region 업적 관리 정보 업데이트 05-06


    /// <summary>
    /// 업적 관리임
    /// </summary>
    [Serializable]
    public class QuestEntry2
    {
        //
        public long Pun_51;
        public long Pun_52;
        public long Pun_53;
        public long Pun_54;
        public long Pun_55;
        public long Pun_56;
        public long Pun_57;
        public long Pun_58;
        public long Pun_59;
        public long Pun_60;
        public long Pun_61;
        public long Pun_62;
        public long Pun_63;
        public long Pun_64;
        public long Pun_65;
        public long Pun_66;
        public long Pun_67;
        public long Pun_68;
        public long Pun_69;
        public long Pun_70;

        //
        public long Pun_51_Cnt;
        public long Pun_52_Cnt;
        public long Pun_53_Cnt;
        public long Pun_54_Cnt;
        public long Pun_55_Cnt;
        public long Pun_56_Cnt;
        public long Pun_57_Cnt;
        public long Pun_58_Cnt;
        public long Pun_59_Cnt;
        public long Pun_60_Cnt;
        public long Pun_61_Cnt;
        public long Pun_62_Cnt;
        public long Pun_63_Cnt;
        public long Pun_64_Cnt;
        public long Pun_65_Cnt;
        public long Pun_66_Cnt;
        public long Pun_67_Cnt;
        public long Pun_68_Cnt;
        public long Pun_69_Cnt;
        public long Pun_70_Cnt;

        //
        public long All_Mugan;
        public long All_Mugan_Cnt;


    }

    // 만든 클래스를 리스트에 담아서 테이블처럼 사용
    public List<QuestEntry2> questInfo2 = new List<QuestEntry2>();

    public void AddQuestData2()
    {
        //새로운 데이터를 추가해주고
        questInfo2.Add(new QuestEntry2
        {
            Pun_51 = 0,
            Pun_52 = 0,
            Pun_53 = 0,
            Pun_54 = 0,
            Pun_55 = 0,
            Pun_56 = 0,
            Pun_57 = 0,
            Pun_58 = 0,
            Pun_59 = 0,
            Pun_60 = 0,
            Pun_61 = 0,
            Pun_62 = 0,
            Pun_63 = 0,
            Pun_64 = 0,
            Pun_65 = 0,
            Pun_66 = 0,
            Pun_67 = 0,
            Pun_68 = 0,
            Pun_69 = 0,
            Pun_70 = 0,
            Pun_51_Cnt = 0,
            Pun_52_Cnt = 0,
            Pun_53_Cnt = 0,
            Pun_54_Cnt = 0,
            Pun_55_Cnt = 0,
            Pun_56_Cnt = 0,
            Pun_57_Cnt = 0,
            Pun_58_Cnt = 0,
            Pun_59_Cnt = 0,
            Pun_60_Cnt = 0,
            Pun_61_Cnt = 0,
            Pun_62_Cnt = 0,
            Pun_63_Cnt = 0,
            Pun_64_Cnt = 0,
            Pun_65_Cnt = 0,
            Pun_66_Cnt = 0,
            Pun_67_Cnt = 0,
            Pun_68_Cnt = 0,
            Pun_69_Cnt = 0,
            Pun_70_Cnt = 0,
            All_Mugan = 0,
            All_Mugan_Cnt = 0
        });

        SavequestInfo2();
    }

    [HideInInspector]
    public bool isReadyQuest2;

    public void SavequestInfo2()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();

        // Info를 바이트 배열로 변환해서 저장
        binaryFormatter.Serialize(memoryStream, questInfo2);

        // 그것을 다시 한번 문자열 값으로 변환해서 
        // 스트링 키값으로 PlayerPrefs에 저장
        PlayerPrefs.SetString("questInfo2", Convert.ToBase64String(memoryStream.GetBuffer()));
        PlayerPrefs.Save();
        isReadyQuest2 = true;
    }

    public void LoadquestInfo2()
    {
        string data = PlayerPrefs.GetString("questInfo2");
        Debug.LogError("로드인포2 data : " + data);
        if (!string.IsNullOrEmpty(data))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(data));

            // 가져온 데이터를 바이트 배열로 변환 -> 리스트로 캐스팅
            questInfo2 = (List<QuestEntry2>)binaryFormatter.Deserialize(memoryStream);
        }

        isReadyQuest2 = true;
    }


    #endregion


    #region 업적 관리 정보 업데이트 05-17


    /// <summary>
    /// 업적 관리임
    /// </summary>
    [Serializable]
    public class QuestEntry3
    {
        //
        public long Pun_71;
        public long Pun_72;
        public long Pun_73;
        public long Pun_74;
        public long Pun_75;
        public long Pun_76;
        public long Pun_77;
        public long Pun_78;
        public long Pun_79;
        public long Pun_80;
        //
        public long Pun_71_Cnt;
        public long Pun_72_Cnt;
        public long Pun_73_Cnt;
        public long Pun_74_Cnt;
        public long Pun_75_Cnt;
        public long Pun_76_Cnt;
        public long Pun_77_Cnt;
        public long Pun_78_Cnt;
        public long Pun_79_Cnt;
        public long Pun_80_Cnt;
        //
        public long daily_MiniGameCombo;
        public long All_MiniGame;
        public long All_MiniGame_Cnt;


    }

    // 만든 클래스를 리스트에 담아서 테이블처럼 사용
    public List<QuestEntry3> questInfo3 = new List<QuestEntry3>();

    public void AddQuestData3()
    {
        //새로운 데이터를 추가해주고
        questInfo3.Add(new QuestEntry3
        {
            Pun_71 = 0,
            Pun_72 = 0,
            Pun_73 = 0,
            Pun_74 = 0,
            Pun_75 = 0,
            Pun_76 = 0,
            Pun_77 = 0,
            Pun_78 = 0,
            Pun_79 = 0,
            Pun_80 = 0,
            Pun_71_Cnt = 0,
            Pun_72_Cnt = 0,
            Pun_73_Cnt = 0,
            Pun_74_Cnt = 0,
            Pun_75_Cnt = 0,
            Pun_76_Cnt = 0,
            Pun_77_Cnt = 0,
            Pun_78_Cnt = 0,
            Pun_79_Cnt = 0,
            Pun_80_Cnt = 0,
            daily_MiniGameCombo = 0,
            All_MiniGame = 0,
            All_MiniGame_Cnt = 0
        });

        SavequestInfo3();
    }

    [HideInInspector]
    public bool isReadyQuest3;

    public void SavequestInfo3()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();

        // Info를 바이트 배열로 변환해서 저장
        binaryFormatter.Serialize(memoryStream, questInfo3);

        // 그것을 다시 한번 문자열 값으로 변환해서 
        // 스트링 키값으로 PlayerPrefs에 저장
        PlayerPrefs.SetString("questInfo3", Convert.ToBase64String(memoryStream.GetBuffer()));
        PlayerPrefs.Save();
        isReadyQuest3 = true;
    }

    public void LoadquestInfo3()
    {
        string data = PlayerPrefs.GetString("questInfo3");

        if (!string.IsNullOrEmpty(data))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(data));

            // 가져온 데이터를 바이트 배열로 변환 -> 리스트로 캐스팅
            questInfo3 = (List<QuestEntry3>)binaryFormatter.Deserialize(memoryStream);
        }

        isReadyQuest3 = true;
    }


    #endregion



    #region 업적 관리 정보 업데이트 05-29


    /// <summary>
    /// 업적 관리임
    /// </summary>
    [Serializable]
    public class QuestEntry4
    {
        //
        public long Pun_81;
        public long Pun_82;
        public long Pun_83;
        public long Pun_84;
        public long Pun_85;
        public long Pun_86;
        public long Pun_87;
        public long Pun_88;
        public long Pun_89;
        public long Pun_90;
        //
        public long Pun_81_Cnt;
        public long Pun_82_Cnt;
        public long Pun_83_Cnt;
        public long Pun_84_Cnt;
        public long Pun_85_Cnt;
        public long Pun_86_Cnt;
        public long Pun_87_Cnt;
        public long Pun_88_Cnt;
        public long Pun_89_Cnt;
        public long Pun_90_Cnt;
        //
        public long All_Per_Atk;
        public long All_Per_HP;
        public long All_Dia_Atk;
        public long All_Dia_HP;
        public long All_Per_Atk_Cnt;
        public long All_Per_HP_Cnt;
        public long All_Dia_Atk_Cnt;
        public long All_Dia_HP_Cnt;


    }

    // 만든 클래스를 리스트에 담아서 테이블처럼 사용
    public List<QuestEntry4> questInfo4 = new List<QuestEntry4>();

    public void AddQuestData4()
    {
        //새로운 데이터를 추가해주고
        questInfo4.Add(new QuestEntry4
        {
            Pun_81 = 0,
            Pun_82 = 0,
            Pun_83 = 0,
            Pun_84 = 0,
            Pun_85 = 0,
            Pun_86 = 0,
            Pun_87 = 0,
            Pun_88 = 0,
            Pun_89 = 0,
            Pun_90 = 0,
            Pun_81_Cnt = 0,
            Pun_82_Cnt = 0,
            Pun_83_Cnt = 0,
            Pun_84_Cnt = 0,
            Pun_85_Cnt = 0,
            Pun_86_Cnt = 0,
            Pun_87_Cnt = 0,
            Pun_88_Cnt = 0,
            Pun_89_Cnt = 0,
            Pun_90_Cnt = 0,
            All_Per_Atk = 0, // 누적 미션 골드 공격 퍼센트
            All_Per_HP = 0,
            All_Dia_Atk = 0, // 누적 미션 다이아 공격 퍼센트
            All_Dia_HP = 0,
            All_Per_Atk_Cnt = 0, // 누적 미션 골드 공격 퍼센트
            All_Per_HP_Cnt = 0,
            All_Dia_Atk_Cnt = 0, // 누적 미션 다이아 공격 퍼센트
            All_Dia_HP_Cnt = 0

        });

        SavequestInfo4();
    }

    [HideInInspector]
    public bool isReadyQuest4;

    public void SavequestInfo4()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();

        // Info를 바이트 배열로 변환해서 저장
        binaryFormatter.Serialize(memoryStream, questInfo4);

        // 그것을 다시 한번 문자열 값으로 변환해서 
        // 스트링 키값으로 PlayerPrefs에 저장
        PlayerPrefs.SetString("questInfo4", Convert.ToBase64String(memoryStream.GetBuffer()));
        PlayerPrefs.Save();
        isReadyQuest4 = true;
    }

    public void LoadquestInfo4()
    {
        string data = PlayerPrefs.GetString("questInfo4");

        if (!string.IsNullOrEmpty(data))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(data));

            // 가져온 데이터를 바이트 배열로 변환 -> 리스트로 캐스팅
            questInfo4 = (List<QuestEntry4>)binaryFormatter.Deserialize(memoryStream);
        }

        isReadyQuest4 = true;
    }


    #endregion





    #region 업적 관리 정보 업데이트 06-22


    /// <summary>
    /// 업적 관리임
    /// </summary>
    [Serializable]
    public class QuestEntry5
    {
        //
        public long Pun_91;
        public long Pun_92;
        public long Pun_93;
        public long Pun_94;
        public long Pun_95;
        public long Pun_96;
        public long Pun_97;
        public long Pun_98;
        public long Pun_99;
        public long Pun_100;
        //
        public long Pun_91_Cnt;
        public long Pun_92_Cnt;
        public long Pun_93_Cnt;
        public long Pun_94_Cnt;
        public long Pun_95_Cnt;
        public long Pun_96_Cnt;
        public long Pun_97_Cnt;
        public long Pun_98_Cnt;
        public long Pun_99_Cnt;
        public long Pun_100_Cnt;



    }

    // 만든 클래스를 리스트에 담아서 테이블처럼 사용
    public List<QuestEntry5> questInfo5 = new List<QuestEntry5>();

    public void AddQuestData5()
    {
        //새로운 데이터를 추가해주고
        questInfo5.Add(new QuestEntry5
        {
            Pun_91 = 0,
            Pun_92 = 0,
            Pun_93 = 0,
            Pun_94 = 0,
            Pun_95 = 0,
            Pun_96 = 0,
            Pun_97 = 0,
            Pun_98 = 0,
            Pun_99 = 0,
            Pun_100 = 0,
            Pun_91_Cnt = 0,
            Pun_92_Cnt = 0,
            Pun_93_Cnt = 0,
            Pun_94_Cnt = 0,
            Pun_95_Cnt = 0,
            Pun_96_Cnt = 0,
            Pun_97_Cnt = 0,
            Pun_98_Cnt = 0,
            Pun_99_Cnt = 0,
            Pun_100_Cnt = 0

        });

        SavequestInfo5();
    }

    [HideInInspector]
    public bool isReadyQuest5;

    public void SavequestInfo5()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();

        // Info를 바이트 배열로 변환해서 저장
        binaryFormatter.Serialize(memoryStream, questInfo5);

        // 그것을 다시 한번 문자열 값으로 변환해서 
        // 스트링 키값으로 PlayerPrefs에 저장
        PlayerPrefs.SetString("questInfo5", Convert.ToBase64String(memoryStream.GetBuffer()));
        PlayerPrefs.Save();
        isReadyQuest5 = true;
    }

    public void LoadquestInfo5()
    {
        string data = PlayerPrefs.GetString("questInfo5");

        if (!string.IsNullOrEmpty(data))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(data));

            // 가져온 데이터를 바이트 배열로 변환 -> 리스트로 캐스팅
            questInfo5 = (List<QuestEntry5>)binaryFormatter.Deserialize(memoryStream);
        }

        isReadyQuest5 = true;
    }


    #endregion




    #region 업적 관리 정보 업데이트 07-16


    /// <summary>
    /// 업적 관리임
    /// </summary>
    [Serializable]
    public class QuestEntry6
    {
        //
        public long All_PVPGame;
        //
        public long All_PVPGame_Cnt;
    }

    // 만든 클래스를 리스트에 담아서 테이블처럼 사용
    public List<QuestEntry6> questInfo6 = new List<QuestEntry6>();

    public void AddQuestData6()
    {
        //새로운 데이터를 추가해주고
        questInfo6.Add(new QuestEntry6
        {
            All_PVPGame = 0,
            All_PVPGame_Cnt = 0,
        });

        SavequestInfo6();
    }

    [HideInInspector]
    public bool isReadyQuest6;

    public void SavequestInfo6()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();

        // Info를 바이트 배열로 변환해서 저장
        binaryFormatter.Serialize(memoryStream, questInfo6);

        // 그것을 다시 한번 문자열 값으로 변환해서 
        // 스트링 키값으로 PlayerPrefs에 저장
        PlayerPrefs.SetString("questInfo6", Convert.ToBase64String(memoryStream.GetBuffer()));
        PlayerPrefs.Save();
        isReadyQuest6 = true;
    }

    public void LoadquestInfo6()
    {
        string data = PlayerPrefs.GetString("questInfo6");

        if (!string.IsNullOrEmpty(data))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(data));

            // 가져온 데이터를 바이트 배열로 변환 -> 리스트로 캐스팅
            questInfo6 = (List<QuestEntry6>)binaryFormatter.Deserialize(memoryStream);
        }

        isReadyQuest6 = true;
    }


    #endregion



    #region 유니폼 데이터 관리 06-22


    /// <summary>
    /// 유니폼 데이터 관리 06-22
    /// </summary>
    [Serializable]
    public class UniformEntry
    {
        /// <summary>
        /// 유니폼 - 능력에 관여
        /// </summary>
        public int Uniform_LV; // 현재 무기 레벨 1/100
        public double Uniform_Price; // 레벨업시 소모 골드
        /// <summary>
        /// 캐릭터 - 스킬 에 관여
        /// </summary>
        public int Skill_LV; // 현재 무기 레벨 1/100
        public double Skill_Price; // 레벨업시 소모 골드

    }

    /// <summary>
    /// 유니폼은 유니폼 레벨 스킬은 스킬 레벨 공용으로 사용중
    /// </summary>
    public List<UniformEntry> uniformInfo = new List<UniformEntry>();

    public void AdduniformData(int _LV, double _Price, int s_LV, double s_Price)
    {
        //새로운 데이터를 추가해주고
        uniformInfo.Add(new UniformEntry
        {
            Uniform_LV = _LV,
            Uniform_Price = _Price,
            Skill_LV = s_LV,
            Skill_Price = s_Price
        });

    }

    public void SaveuniformData()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();

        // Info를 바이트 배열로 변환해서 저장
        binaryFormatter.Serialize(memoryStream, uniformInfo);

        // 그것을 다시 한번 문자열 값으로 변환해서 
        // 스트링 키값으로 PlayerPrefs에 저장
        PlayerPrefs.SetString("uniformInfo", Convert.ToBase64String(memoryStream.GetBuffer()));
        PlayerPrefs.Save();
    }

    public void LoaduniformData()
    {
        string data = PlayerPrefs.GetString("uniformInfo");

        if (!string.IsNullOrEmpty(data))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(data));

            // 가져온 데이터를 바이트 배열로 변환 -> 리스트로 캐스팅
            uniformInfo = (List<UniformEntry>)binaryFormatter.Deserialize(memoryStream);
        }
    }


    #endregion




    /// <summary>
    /// 유니폼 데이터 관리 06-22
    /// </summary>
    [Serializable]
    public class LimitData
    {
        public int dia_Day_01;
        public int dia_Day_02;
        public int dia_Day_03;
        public int dia_Day_04;
        public int dia_Day_05;

        public int dia_Week_01;
        public int dia_Week_02;
        public int dia_Week_03;
        public int dia_Week_04;
        public int dia_Week_05;

        public int dia_Mouth_01;
        public int dia_Mouth_02;
        public int dia_Mouth_03;
        public int dia_Mouth_04;
        public int dia_Mouth_05;

        /// 주간/월간 패키지 체크 날짜
        public int weekend_Day;
        public int mouth_Day;
    }

    /// <summary>
    /// 유니폼은 유니폼 레벨 스킬은 스킬 레벨 공용으로 사용중
    /// </summary>
    public List<LimitData> dayLimitData = new List<LimitData>();

    [HideInInspector]
    public bool isReadyDayLimit;

    public void SaveDayLimitData()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();

        // Info를 바이트 배열로 변환해서 저장
        binaryFormatter.Serialize(memoryStream, dayLimitData);

        // 그것을 다시 한번 문자열 값으로 변환해서 
        // 스트링 키값으로 PlayerPrefs에 저장
        PlayerPrefs.SetString("LimitData", Convert.ToBase64String(memoryStream.GetBuffer()));
        PlayerPrefs.Save();
        isReadyDayLimit = true;
    }

    public void LoadDayLimitData()
    {
        string data = PlayerPrefs.GetString("LimitData");

        if (!string.IsNullOrEmpty(data))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(data));

            // 가져온 데이터를 바이트 배열로 변환 -> 리스트로 캐스팅
            dayLimitData = (List<LimitData>)binaryFormatter.Deserialize(memoryStream);
        }
        else
        {
            Debug.LogError(" 로드 월주월 데이타");
            dayLimitData.Add(new LimitData
            {
                dia_Day_01 = 0,
                dia_Day_02 = 0,
                dia_Day_03 = 0,
                dia_Day_04 = 0,
                dia_Day_05 = 0,

                dia_Week_01 = 0,
                dia_Week_02 = 0,
                dia_Week_03 = 0,
                dia_Week_04 = 0,
                dia_Week_05 = 0,

                dia_Mouth_01 = 0,
                dia_Mouth_02 = 0,
                dia_Mouth_03 = 0,
                dia_Mouth_04 = 0,
                dia_Mouth_05 = 0,

                weekend_Day = 0,
                mouth_Day = 0,
            });
        }

        isReadyDayLimit = true;
    }



    //#region 재화 관리 정보


    ///// <summary>
    ///// 재화 관리 10.00CX 제한
    ///// </summary>
    //[Serializable]
    //public class AllMoneyEntry
    //{
    //    public double dGold;            // 골드 
    //    public double dDiamond;         // 다이아
    //    public double dGupbap;          // 국밥
    //    public double dSSalbap;         // 쌀밥

    //}

    //// 만든 클래스를 리스트에 담아서 테이블처럼 사용
    //public List<AllMoneyEntry> allMoneyInfo = new List<AllMoneyEntry>();


    //public void AddAllMoneyData(double _Gold, double _Dia, double _Gupbap, double _SSalbap)
    //{
    //    //새로운 데이터를 추가해주고
    //    allMoneyInfo.Add(new AllMoneyEntry
    //    {
    //        dGold = _Gold,
    //        dDiamond = _Dia,
    //        dGupbap = _Gupbap,
    //        dSSalbap = _SSalbap
    //    });

    //    PlayerPrefs.SetString("gold", _Gold.ToString());
    //    PlayerPrefs.SetString("diamond", _Dia.ToString());
    //    PlayerPrefs.SetString("gupbap", _Gupbap.ToString());
    //    PlayerPrefs.SetString("ssalbap", _SSalbap.ToString());

    //    SaveAllMoneyData();
    //}

    //public bool isAllMoneyReady;

    //public void SaveAllMoneyData()
    //{
    //    BinaryFormatter binaryFormatter = new BinaryFormatter();
    //    MemoryStream memoryStream = new MemoryStream();

    //    // Info를 바이트 배열로 변환해서 저장
    //    binaryFormatter.Serialize(memoryStream, weaponInfo);

    //    // 그것을 다시 한번 문자열 값으로 변환해서 
    //    // 스트링 키값으로 PlayerPrefs에 저장
    //    PlayerPrefs.SetString("AllMoneyData", Convert.ToBase64String(memoryStream.GetBuffer()));
    //    PlayerPrefs.Save();

    //    isAllMoneyReady = true;
    //}

    //public void LoadAllMoneyData()
    //{
    //    string data = PlayerPrefs.GetString("AllMoneyData");

    //    if (!string.IsNullOrEmpty(data))
    //    {
    //        BinaryFormatter binaryFormatter = new BinaryFormatter();
    //        MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(data));

    //        // 가져온 데이터를 바이트 배열로 변환 -> 리스트로 캐스팅
    //        weaponInfo = (List<WeaponEntry>)binaryFormatter.Deserialize(memoryStream);

    //    }

    //    isAllMoneyReady = true;
    //}


    //#endregion









    #region 구글 클라우드용 pp 복사값


    [Serializable]
    public class GPGSsavedPrefList
    {
        public int cloudTmpForGPGS_001;
        public string cloudTmpForGPGS_002;
        public int cloudTmpForGPGS_003;
        public int cloudTmpForGPGS_004;
        public int cloudTmpForGPGS_005;
        public int cloudTmpForGPGS_006;
        public int cloudTmpForGPGS_007;
        public float cloudTmpForGPGS_008;
        public int cloudTmpForGPGS_009;
        public int cloudTmpForGPGS_010;
        public int cloudTmpForGPGS_011;
        public int cloudTmpForGPGS_012;
        public int cloudTmpForGPGS_013;
        public int cloudTmpForGPGS_014;
        public int cloudTmpForGPGS_015;
        public int cloudTmpForGPGS_016;
        public int cloudTmpForGPGS_017;
        public int cloudTmpForGPGS_018;
        public int cloudTmpForGPGS_019;
        public string cloudTmpForGPGS_020;
        public string cloudTmpForGPGS_021;
        public string cloudTmpForGPGS_022;
        public string cloudTmpForGPGS_023;
        public string cloudTmpForGPGS_024;
        public string cloudTmpForGPGS_025;
        public string cloudTmpForGPGS_026;
        public string cloudTmpForGPGS_027;
        public string cloudTmpForGPGS_028;
        public string cloudTmpForGPGS_029;
        public string cloudTmpForGPGS_030;
        public float cloudTmpForGPGS_031;
        public int cloudTmpForGPGS_032;
        public float cloudTmpForGPGS_033;
        public float cloudTmpForGPGS_034;
        public float cloudTmpForGPGS_035;
        public float cloudTmpForGPGS_036;
        public int cloudTmpForGPGS_037;
        public float cloudTmpForGPGS_038;
        //0515
        public int cloudTmpForGPGS_101;
        public int cloudTmpForGPGS_102;
        public int cloudTmpForGPGS_103;
        public int cloudTmpForGPGS_104;
        public int cloudTmpForGPGS_105;
        public string cloudTmpForGPGS_106;
        public string cloudTmpForGPGS_107;
        public float cloudTmpForGPGS_108;
        //
        public int cloudTmpForGPGS_109;
        public int cloudTmpForGPGS_110;
        public string cloudTmpForGPGS_111;
        //
        public int cloudTmpForGPGS_112;
        public int cloudTmpForGPGS_113;
        public int cloudTmpForGPGS_114;
        public int cloudTmpForGPGS_115;
        public string cloudTmpForGPGS_116;
        public string cloudTmpForGPGS_117;
        public string cloudTmpForGPGS_118;
        public int cloudTmpForGPGS_119;
        public int cloudTmpForGPGS_120;
        public float cloudTmpForGPGS_121;
        public float cloudTmpForGPGS_122;
        public float cloudTmpForGPGS_123;
        public float cloudTmpForGPGS_124;
        public int cloudTmpForGPGS_125;
        public string cloudTmpForGPGS_126;
        //
        public string cloudTmpForGPGS_127;
        public string cloudTmpForGPGS_128;
        public string cloudTmpForGPGS_129;
        public float cloudTmpForGPGS_130;
        public int cloudTmpForGPGS_131;
        public int cloudTmpForGPGS_132;
        public int cloudTmpForGPGS_133;
        public int cloudTmpForGPGS_134;
        public string cloudTmpForGPGS_135;
        public string cloudTmpForGPGS_136;
        public int cloudTmpForGPGS_137;
        public int cloudTmpForGPGS_138;

        //0601
        public string cloudTmpForGPGS_139 = "525*";
        public float cloudTmpForGPGS_140 = 0;

        //0608
        public int cloudTmpForGPGS_141;
        public int cloudTmpForGPGS_142;
        public int cloudTmpForGPGS_143;
        public int cloudTmpForGPGS_144;
        public int cloudTmpForGPGS_145;
        public int cloudTmpForGPGS_146;
        public int cloudTmpForGPGS_147;
        public int cloudTmpForGPGS_148 = 0;
        public int cloudTmpForGPGS_149 = 0;

        //0615
        public int cloudTmpForGPGS_150;
        public int cloudTmpForGPGS_151;
        public int cloudTmpForGPGS_152;
        public int cloudTmpForGPGS_153;
        public string cloudTmpForGPGS_154;
        public string cloudTmpForGPGS_155;
        public string cloudTmpForGPGS_156;
        public string cloudTmpForGPGS_157;

        //0622
        public string cloudTmpForGPGS_158;
        public string cloudTmpForGPGS_159;
        public int cloudTmpForGPGS_160;

        public string cloudTmpForGPGS_161;
        public string cloudTmpForGPGS_162;
        public int cloudTmpForGPGS_163;

        public int cloudTmpForGPGS_164;
        public int cloudTmpForGPGS_165;
        public int cloudTmpForGPGS_166;
        public int cloudTmpForGPGS_167;
        public int cloudTmpForGPGS_168;
        public int cloudTmpForGPGS_169;

        //0710
        public int cloudTmpForGPGS_170;
        public int cloudTmpForGPGS_171;
        public int cloudTmpForGPGS_172;
        public int cloudTmpForGPGS_173;
        public int cloudTmpForGPGS_174;
        //
        //0727
        public int cloudTmpForGPGS_175;
        public int cloudTmpForGPGS_176;
        //0331
        public int cloudTmpForGPGS_177;
        public int cloudTmpForGPGS_178;
        public int cloudTmpForGPGS_179;
        public int cloudTmpForGPGS_180;

        public string cloudTmpForGPGS_181;

        public int cloudTmpForGPGS_182;
        public int cloudTmpForGPGS_183;
        public int cloudTmpForGPGS_184;
        public int cloudTmpForGPGS_185;
        public int cloudTmpForGPGS_186;

    }

    //public List<GPGSsavedPrefList> listGPGS = new List<GPGSsavedPrefList>();

    /// <summary>
    /// 나누에서 데이터 저장하는 것
    /// </summary>
    /// <returns></returns>
    public string SaveAllPrefsData()
    {
        SaveWeaponInfo();
        SavequestInfo();
        //
        SavequestInfo2();
        SavequestInfo3();
        SavequestInfo4();
        SavequestInfo5();
        SavequestInfo6();

        List<GPGSsavedPrefList> listGPGS = new List<GPGSsavedPrefList>
        {
            new GPGSsavedPrefList
            {
                cloudTmpForGPGS_001 = PlayerPrefs.GetInt("isFristGameStart", 1),
                cloudTmpForGPGS_002 = PlayerPrefs.GetString("Shield_Time", "0"),
                cloudTmpForGPGS_003 = PlayerPrefs.GetInt("PunchIndex", 0),
                cloudTmpForGPGS_004 = PlayerPrefs.GetInt("DefendTrigger", 0),
                cloudTmpForGPGS_005 = PlayerPrefs.GetInt("key", 20),
                cloudTmpForGPGS_006 = PlayerPrefs.GetInt("VIP", 0),
                cloudTmpForGPGS_007 = PlayerPrefs.GetInt("isGoldTriple", 0),
                cloudTmpForGPGS_008 = PlayerPrefs.GetFloat("Mat_100", 0),
                cloudTmpForGPGS_009 = PlayerPrefs.GetInt("Mat_Skill_300", 0),
                cloudTmpForGPGS_010 = PlayerPrefs.GetInt("ATK_Lv", 0),
                cloudTmpForGPGS_011 = PlayerPrefs.GetInt("Mat_HP_Lv", 0),
                cloudTmpForGPGS_012 = PlayerPrefs.GetInt("Recov_Lv", 0),
                cloudTmpForGPGS_013 = PlayerPrefs.GetInt("Mattzip_Lv", 0),
                cloudTmpForGPGS_014 = PlayerPrefs.GetInt("Arti_PunchTouch", 0),
                cloudTmpForGPGS_015 = PlayerPrefs.GetInt("Arti_Mattzip", 0),
                cloudTmpForGPGS_016 = PlayerPrefs.GetInt("Arti_HP", 0),
                cloudTmpForGPGS_017 = PlayerPrefs.GetInt("Arti_GroggyTouch", 0),
                cloudTmpForGPGS_018 = PlayerPrefs.GetInt("Arti_GAL", 0),
                cloudTmpForGPGS_019 = PlayerPrefs.GetInt("isBoosterMattzip", 0),
                cloudTmpForGPGS_020 = PlayerPrefs.GetString("missionInfo"),
                cloudTmpForGPGS_021 = PlayerPrefs.GetString("gold", "0"),
                cloudTmpForGPGS_022 = PlayerPrefs.GetString("diamond", "0"),
                cloudTmpForGPGS_023 = PlayerPrefs.GetString("gupbap", "0"),
                cloudTmpForGPGS_024 = PlayerPrefs.GetString("Mat_MaxHP", "100"),
                cloudTmpForGPGS_025 = PlayerPrefs.GetString("Stat_MaxHP", "0"),
                cloudTmpForGPGS_026 = PlayerPrefs.GetString("Arti_MaxHP", "0"),
                cloudTmpForGPGS_027 = PlayerPrefs.GetString("Mat_currentHP", "100"),
                cloudTmpForGPGS_028 = PlayerPrefs.GetString("Mat_Recov", "1"),
                cloudTmpForGPGS_029 = PlayerPrefs.GetString("weaponInfo"),
                cloudTmpForGPGS_030 = PlayerPrefs.GetString("questInfo"),
                cloudTmpForGPGS_031 = PlayerPrefs.GetFloat("Mat_Mattzip_Hit", 0),
                cloudTmpForGPGS_032 = PlayerPrefs.GetInt("MattzipStat", 0),
                cloudTmpForGPGS_033 = PlayerPrefs.GetFloat("MattzipArtif", 0),
                cloudTmpForGPGS_034 = PlayerPrefs.GetFloat("AttackPerPunch", 0),
                cloudTmpForGPGS_035 = PlayerPrefs.GetFloat("GroggyTouch", 0),
                cloudTmpForGPGS_036 = PlayerPrefs.GetFloat("LuckyProb", 1.0f),
                cloudTmpForGPGS_037 = PlayerPrefs.GetInt("MaxGet_GookBap", 0),
                cloudTmpForGPGS_038 = PlayerPrefs.GetFloat("Cilcked_Cnt_MattZip", 5f),
                //0515
                cloudTmpForGPGS_101 = PlayerPrefs.GetInt("Arti_DefenceTime", 0),
                cloudTmpForGPGS_102 = PlayerPrefs.GetInt("Arti_GoldBox", 0),
                cloudTmpForGPGS_103 = PlayerPrefs.GetInt("Arti_OffGold", 0),
                cloudTmpForGPGS_104 = PlayerPrefs.GetInt("MaxGet_MuganTop", 1),
                cloudTmpForGPGS_105 = PlayerPrefs.GetInt("isTutoAllClear", 0),
                cloudTmpForGPGS_106 = PlayerPrefs.GetString("LimitData"),
                cloudTmpForGPGS_107 = PlayerPrefs.GetString("questInfo2"),
                cloudTmpForGPGS_108 = PlayerPrefs.GetFloat("dDiamond", 0),
                //0518
                cloudTmpForGPGS_109 = PlayerPrefs.GetInt("isAllmute", 0),
                cloudTmpForGPGS_110 = PlayerPrefs.GetInt("Shield10AdsCnt", 0),  /// is0517shock
                cloudTmpForGPGS_111 = PlayerPrefs.GetString("shieldInfo"),


                cloudTmpForGPGS_112 = PlayerPrefs.GetInt("is1Recov", 0),
                cloudTmpForGPGS_113 = PlayerPrefs.GetInt("is2Stamina", 0),
                cloudTmpForGPGS_114 = PlayerPrefs.GetInt("is3ATK", 0),
                cloudTmpForGPGS_115 = PlayerPrefs.GetInt("is4Mattzip", 0),

                cloudTmpForGPGS_116 = PlayerPrefs.GetString("questInfo3"),

                cloudTmpForGPGS_117 = PlayerPrefs.GetString("ATK_PER_UP", "0"),
                cloudTmpForGPGS_118 = PlayerPrefs.GetString("HP_PER_UP", "0"),

                cloudTmpForGPGS_119 = PlayerPrefs.GetInt("ATK_PER_UP_Lv", 0),
                cloudTmpForGPGS_120 = PlayerPrefs.GetInt("HP_PER_UP_Lv", 0),

                cloudTmpForGPGS_121 = PlayerPrefs.GetFloat("Stat_is1Recov", 0),
                cloudTmpForGPGS_122 = PlayerPrefs.GetFloat("Stat_is2Stamina", 0),
                cloudTmpForGPGS_123 = PlayerPrefs.GetFloat("Stat_is3ATK", 0),
                cloudTmpForGPGS_124 = PlayerPrefs.GetFloat("Stat_is4Mattzip", 0),

                cloudTmpForGPGS_125 = PlayerPrefs.GetInt("MaxGet_MiniGame", 0),
                cloudTmpForGPGS_126 = PlayerPrefs.GetString("ssalbap", "0"),

                //
                cloudTmpForGPGS_127 = PlayerPrefs.GetString("Daily_Timer", "0"),
                cloudTmpForGPGS_128 = PlayerPrefs.GetString("BG_Data", "0+0+0+0+0"),
                cloudTmpForGPGS_129 = PlayerPrefs.GetString("questInfo4"),

                cloudTmpForGPGS_130 = PlayerPrefs.GetFloat("BG_CoinStat", 1f),

                cloudTmpForGPGS_131 = PlayerPrefs.GetInt("DailySpinReword", 0),
                cloudTmpForGPGS_132 = PlayerPrefs.GetInt("BG_Curent", 525),
                cloudTmpForGPGS_133 = PlayerPrefs.GetInt("Dia_ATK_PER_UP_Lv", 0),
                cloudTmpForGPGS_134 = PlayerPrefs.GetInt("Dia_HP_PER_UP_Lv", 0),

                cloudTmpForGPGS_135 = PlayerPrefs.GetString("Dia_ATK_PER_UP", "1"),
                cloudTmpForGPGS_136 = PlayerPrefs.GetString("Dia_HP_PER_UP", "1"),

                cloudTmpForGPGS_137 = PlayerPrefs.GetInt("Arti_MuganTime", 0),
                cloudTmpForGPGS_138 = PlayerPrefs.GetInt("Arti_AttackPower", 0),

                //0601
                cloudTmpForGPGS_139 = PlayerPrefs.GetString("diaBuyWeaponList", "525*"),
                cloudTmpForGPGS_140 = PlayerPrefs.GetFloat("Mattzip_Dia_Weap", 0),

                //0608
                cloudTmpForGPGS_141 = PlayerPrefs.GetInt("Arti_GoldPer", 0),
                cloudTmpForGPGS_142 = PlayerPrefs.GetInt("Arti_LuckyBoxPer", 0),
                cloudTmpForGPGS_143 = PlayerPrefs.GetInt("Arti_DefencePer", 0),
                cloudTmpForGPGS_144 = PlayerPrefs.GetInt("Arti_GoldUpgrade", 0),
                cloudTmpForGPGS_145 = PlayerPrefs.GetInt("Arti_InfiReword", 0),
                cloudTmpForGPGS_146 = PlayerPrefs.GetInt("Arti_MiniReword", 0),
                cloudTmpForGPGS_147 = PlayerPrefs.GetInt("Arti_MiniGameTime", 0),
                cloudTmpForGPGS_148 = PlayerPrefs.GetInt("DailyCount_Cheak", 0),
                cloudTmpForGPGS_149 = PlayerPrefs.GetInt("NewDailyCount", 0),

                //0615
                cloudTmpForGPGS_150 = PlayerPrefs.GetInt("CRC_UP_Lv", 0),
                cloudTmpForGPGS_151 = PlayerPrefs.GetInt("CRD_UP_Lv", 0),
                cloudTmpForGPGS_152 = PlayerPrefs.GetInt("Dia_CRC_UP_Lv", 0),
                cloudTmpForGPGS_153 = PlayerPrefs.GetInt("Dia_CRD_UP_Lv", 0),
                cloudTmpForGPGS_154 = PlayerPrefs.GetString("CRC_UP", "1"),
                cloudTmpForGPGS_155 = PlayerPrefs.GetString("CRD_UP", "1"),
                cloudTmpForGPGS_156 = PlayerPrefs.GetString("Dia_CRC_UP", "1"),
                cloudTmpForGPGS_157 = PlayerPrefs.GetString("Dia_CRD_UP", "1"),

                //0622
                cloudTmpForGPGS_158 = PlayerPrefs.GetString("InfiPersonalRecord", "0*0*0*0*0*0*0*0*0*0*"),
                cloudTmpForGPGS_159 = PlayerPrefs.GetString("questInfo5", "1"),
                cloudTmpForGPGS_160 = PlayerPrefs.GetInt("MaxGet_MuganTop2", 1),

                cloudTmpForGPGS_161 = PlayerPrefs.GetString("uniformInfo"),
                cloudTmpForGPGS_162 = PlayerPrefs.GetString("Uniform_Data", "1+0+0+0+0+0+0"),
                cloudTmpForGPGS_163 = PlayerPrefs.GetInt("Uniform_Curent", 0),

                cloudTmpForGPGS_164 = PlayerPrefs.GetInt("Chara_Lv", 0),
                cloudTmpForGPGS_165 = PlayerPrefs.GetInt("Pet_Curent", 0),
                cloudTmpForGPGS_166 = PlayerPrefs.GetInt("Pet_BuyData", 000),
                cloudTmpForGPGS_167 = PlayerPrefs.GetInt("Pet_Touch_Lv", 0),
                cloudTmpForGPGS_168 = PlayerPrefs.GetInt("Pet_Buff_Lv", 0),
                cloudTmpForGPGS_169 = PlayerPrefs.GetInt("Pet_Matt_Up_Lv", 0),

                //0710
                cloudTmpForGPGS_170 = PlayerPrefs.GetInt("ticket", 5),
                cloudTmpForGPGS_171 = PlayerPrefs.GetInt("isFirstPVP", 0),
                cloudTmpForGPGS_172 = PlayerPrefs.GetInt("Gold_RECOV_UP_Lv", 0),
                cloudTmpForGPGS_173 = PlayerPrefs.GetInt("Dia_RECOV_UP_Lv", 0),
                cloudTmpForGPGS_174 = PlayerPrefs.GetInt("is0707shock", 0),

                // 0717
                cloudTmpForGPGS_175 = PlayerPrefs.GetInt("Pet_BuyData_Cape", 0),
                cloudTmpForGPGS_176 = PlayerPrefs.GetInt("Pet_PVP_Matt_Lv", 0),

                // 0803
                //cloudTmpForGPGS_177 = PlayerPrefs.GetInt("Pet_BuyData_Something", 0),
                //cloudTmpForGPGS_178 = PlayerPrefs.GetInt("Pet_PVP_Speed_Lv", 0),
                cloudTmpForGPGS_177 = PlayerPrefs.GetInt("Friend_01_OfflineAtk_Lv", 0),
                cloudTmpForGPGS_178 = PlayerPrefs.GetInt("Friend_02_OffTimeUp_Lv", 0),
                cloudTmpForGPGS_179 = PlayerPrefs.GetInt("Friend_03_OffSpped_Lv", 0),
                cloudTmpForGPGS_180 = PlayerPrefs.GetInt("Friend_04_MattzipPer_Lv", 0),

                cloudTmpForGPGS_181 = PlayerPrefs.GetString("MyArtiList", "525*"),

                cloudTmpForGPGS_182 = PlayerPrefs.GetInt("Arti_DEF_UP", 0),
                cloudTmpForGPGS_183 = PlayerPrefs.GetInt("Arti_SHILED_UP", 0),
                cloudTmpForGPGS_184 = PlayerPrefs.GetInt("Arti_HEALLING_UP", 0),
                cloudTmpForGPGS_185 = PlayerPrefs.GetInt("Arti_KIMCHI_UP", 0),
                cloudTmpForGPGS_186 = PlayerPrefs.GetInt("Arti_MattGrow_UP", 0),



            }
        };

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();

        binaryFormatter.Serialize(memoryStream, listGPGS);

        ///// 데이터 저장해라.
        //PlayerPrefs.SetString("GAME_DATA_MATT", Convert.ToBase64String(memoryStream.GetBuffer()));
        //PlayerPrefs.Save();

        return Convert.ToBase64String(memoryStream.GetBuffer());
    }

    /// <summary>
    /// 불러오기 버튼 누르면 호출
    /// </summary>
    /// <param name="_Data"></param>
    public void LoadAllPrefsData(string _Data)
    {
        string data = _Data;

        if (!string.IsNullOrEmpty(data))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(data));

            //listGPGS = (List<GPGSsavedPrefList>)binaryFormatter.Deserialize(memoryStream);
            /// 데이터 넣어줘라.
            InitAllGameData((List<GPGSsavedPrefList>)binaryFormatter.Deserialize(memoryStream));

            Debug.LogWarning("LoadAllPrefsData :: Success");
        }
        else
        {
            Debug.LogWarning("LoadAllPrefsData :: Faill");
        }



    }

    /// <summary>
    /// 데이터 불러오기 시에 실제로 게임에 데이터 씌워주기
    /// </summary>
    void InitAllGameData(List<GPGSsavedPrefList> listGPGS)
    {
        PlayerPrefs.SetString("weaponInfo", listGPGS[0].cloudTmpForGPGS_029);

        PlayerPrefs.SetString("questInfo", listGPGS[0].cloudTmpForGPGS_030);
        PlayerPrefs.SetString("questInfo2", listGPGS[0].cloudTmpForGPGS_107);
        PlayerPrefs.SetString("questInfo3", listGPGS[0].cloudTmpForGPGS_116);
        PlayerPrefs.SetString("questInfo4", listGPGS[0].cloudTmpForGPGS_129);
        PlayerPrefs.SetString("questInfo5", listGPGS[0].cloudTmpForGPGS_159);
        //
        PlayerPrefs.SetString("shieldInfo", listGPGS[0].cloudTmpForGPGS_111);
        PlayerPrefs.SetString("uniformInfo", listGPGS[0].cloudTmpForGPGS_161);

        PlayerPrefs.SetString("missionInfo", listGPGS[0].cloudTmpForGPGS_020);
        PlayerPrefs.SetString("LimitData", listGPGS[0].cloudTmpForGPGS_106);

        LoadDayLimitData();
        //
        LoadWeaponInfo();
        LoadquestInfo();
        LoadquestInfo2();
        LoadquestInfo3();
        LoadquestInfo4();
        LoadquestInfo5();
        LoadquestInfo6();
        //
        //LoadAllMoneyData();
        LoaduniformData();

        /// 미션 로드
        PlayerPrefs.SetInt("isTutoAllClear", listGPGS[0].cloudTmpForGPGS_105);
        tmm.LoadMissionInfo();

        //서순
        PlayerPrefs.SetInt("isFristGameStart", listGPGS[0].cloudTmpForGPGS_001);
        PlayerPrefs.SetString("Shield_Time", listGPGS[0].cloudTmpForGPGS_002);
        PlayerPrefs.SetInt("PunchIndex", listGPGS[0].cloudTmpForGPGS_003);
        //PlayerPrefs.SetInt("PunchIndex", 0);
        PlayerPrefs.SetInt("DefendTrigger", listGPGS[0].cloudTmpForGPGS_004);
        PlayerPrefs.SetInt("key", listGPGS[0].cloudTmpForGPGS_005);
        PlayerPrefs.SetInt("VIP", listGPGS[0].cloudTmpForGPGS_006);
        PlayerPrefs.SetInt("isGoldTriple", listGPGS[0].cloudTmpForGPGS_007);
        PlayerPrefs.SetFloat("Mat_100", listGPGS[0].cloudTmpForGPGS_008);
        PlayerPrefs.SetInt("Mat_Skill_300", listGPGS[0].cloudTmpForGPGS_009);
        PlayerPrefs.SetInt("ATK_Lv", listGPGS[0].cloudTmpForGPGS_010);
        PlayerPrefs.SetInt("Mat_HP_Lv", listGPGS[0].cloudTmpForGPGS_011);
        PlayerPrefs.SetInt("Recov_Lv", listGPGS[0].cloudTmpForGPGS_012);
        PlayerPrefs.SetInt("Mattzip_Lv", listGPGS[0].cloudTmpForGPGS_013);
        PlayerPrefs.SetInt("Arti_PunchTouch", listGPGS[0].cloudTmpForGPGS_014);
        PlayerPrefs.SetInt("Arti_Mattzip", listGPGS[0].cloudTmpForGPGS_015);
        PlayerPrefs.SetInt("Arti_HP", listGPGS[0].cloudTmpForGPGS_016);
        PlayerPrefs.SetInt("Arti_GroggyTouch", listGPGS[0].cloudTmpForGPGS_017);
        PlayerPrefs.SetInt("Arti_GAL", listGPGS[0].cloudTmpForGPGS_018);
        PlayerPrefs.SetInt("isBoosterMattzip", listGPGS[0].cloudTmpForGPGS_019);
        PlayerPrefs.SetString("gold", listGPGS[0].cloudTmpForGPGS_021);
        PlayerPrefs.SetString("diamond", listGPGS[0].cloudTmpForGPGS_022);
        PlayerPrefs.SetString("gupbap", listGPGS[0].cloudTmpForGPGS_023);
        PlayerPrefs.SetString("Mat_MaxHP", listGPGS[0].cloudTmpForGPGS_024);
        PlayerPrefs.SetString("Stat_MaxHP", listGPGS[0].cloudTmpForGPGS_025);
        PlayerPrefs.SetString("Arti_MaxHP", listGPGS[0].cloudTmpForGPGS_026);
        PlayerPrefs.SetString("Mat_currentHP", listGPGS[0].cloudTmpForGPGS_027);
        PlayerPrefs.SetString("Mat_Recov", listGPGS[0].cloudTmpForGPGS_028);
        PlayerPrefs.SetFloat("Mat_Mattzip_Hit", listGPGS[0].cloudTmpForGPGS_031);
        PlayerPrefs.SetInt("MattzipStat", listGPGS[0].cloudTmpForGPGS_032);
        PlayerPrefs.SetFloat("MattzipArtif", listGPGS[0].cloudTmpForGPGS_033);
        PlayerPrefs.SetFloat("AttackPerPunch", listGPGS[0].cloudTmpForGPGS_034);
        PlayerPrefs.SetFloat("GroggyTouch", listGPGS[0].cloudTmpForGPGS_035);
        PlayerPrefs.SetFloat("LuckyProb", listGPGS[0].cloudTmpForGPGS_036);
        PlayerPrefs.SetInt("MaxGet_GookBap", listGPGS[0].cloudTmpForGPGS_037);
        PlayerPrefs.SetFloat("Cilcked_Cnt_MattZip", listGPGS[0].cloudTmpForGPGS_038);
        //0515
        PlayerPrefs.SetInt("Arti_DefenceTime", listGPGS[0].cloudTmpForGPGS_101);
        PlayerPrefs.SetInt("Arti_GoldBox", listGPGS[0].cloudTmpForGPGS_102);
        PlayerPrefs.SetInt("Arti_OffGold", listGPGS[0].cloudTmpForGPGS_103);
        PlayerPrefs.SetInt("MaxGet_MuganTop", listGPGS[0].cloudTmpForGPGS_104);
        PlayerPrefs.SetInt("Shield10AdsCnt", listGPGS[0].cloudTmpForGPGS_110);  /// is0517shock
        PlayerPrefs.SetFloat("dDiamond", listGPGS[0].cloudTmpForGPGS_108);
        //0517
        PlayerPrefs.SetInt("isAllmute", listGPGS[0].cloudTmpForGPGS_109);
        //
        PlayerPrefs.SetInt("is1Recov", listGPGS[0].cloudTmpForGPGS_112);
        PlayerPrefs.SetInt("is2Stamina", listGPGS[0].cloudTmpForGPGS_113);
        PlayerPrefs.SetInt("is3ATK", listGPGS[0].cloudTmpForGPGS_114);
        PlayerPrefs.SetInt("is4Mattzip", listGPGS[0].cloudTmpForGPGS_115);
        PlayerPrefs.SetString("ATK_PER_UP", listGPGS[0].cloudTmpForGPGS_117);
        PlayerPrefs.SetString("HP_PER_UP", listGPGS[0].cloudTmpForGPGS_118);
        PlayerPrefs.SetInt("ATK_PER_UP_Lv", listGPGS[0].cloudTmpForGPGS_119);
        PlayerPrefs.SetInt("HP_PER_UP_Lv", listGPGS[0].cloudTmpForGPGS_120);
        PlayerPrefs.SetFloat("Stat_is1Recov", listGPGS[0].cloudTmpForGPGS_121);
        PlayerPrefs.SetFloat("Stat_is2Stamina", listGPGS[0].cloudTmpForGPGS_122);
        PlayerPrefs.SetFloat("Stat_is3ATK", listGPGS[0].cloudTmpForGPGS_123);
        PlayerPrefs.SetFloat("Stat_is4Mattzip", listGPGS[0].cloudTmpForGPGS_124);
        PlayerPrefs.SetInt("MaxGet_MiniGame", listGPGS[0].cloudTmpForGPGS_125);
        PlayerPrefs.SetString("ssalbap", listGPGS[0].cloudTmpForGPGS_126);
        //0529
        PlayerPrefs.SetString("Daily_Timer", listGPGS[0].cloudTmpForGPGS_127);
        PlayerPrefs.SetString("BG_Data", listGPGS[0].cloudTmpForGPGS_128);
        PlayerPrefs.SetFloat("BG_CoinStat", listGPGS[0].cloudTmpForGPGS_130);
        PlayerPrefs.SetInt("DailySpinReword", listGPGS[0].cloudTmpForGPGS_131);
        PlayerPrefs.SetInt("BG_Curent", listGPGS[0].cloudTmpForGPGS_132);
        PlayerPrefs.SetInt("Dia_ATK_PER_UP_Lv", listGPGS[0].cloudTmpForGPGS_133);
        PlayerPrefs.SetInt("Dia_HP_PER_UP_Lv", listGPGS[0].cloudTmpForGPGS_134);
        PlayerPrefs.SetString("Dia_ATK_PER_UP", listGPGS[0].cloudTmpForGPGS_135);
        PlayerPrefs.SetString("Dia_HP_PER_UP", listGPGS[0].cloudTmpForGPGS_136);

        PlayerPrefs.SetInt("Arti_MuganTime", listGPGS[0].cloudTmpForGPGS_137);
        PlayerPrefs.SetInt("Arti_AttackPower", listGPGS[0].cloudTmpForGPGS_138);
        //0601
        PlayerPrefs.SetString("diaBuyWeaponList", listGPGS[0].cloudTmpForGPGS_139);
        PlayerPrefs.SetFloat("Mattzip_Dia_Weap", listGPGS[0].cloudTmpForGPGS_140);
        //0608
        PlayerPrefs.SetInt("Arti_GoldPer", listGPGS[0].cloudTmpForGPGS_141);
        PlayerPrefs.SetInt("Arti_LuckyBoxPer", listGPGS[0].cloudTmpForGPGS_142);
        PlayerPrefs.SetInt("Arti_DefencePer", listGPGS[0].cloudTmpForGPGS_143);
        PlayerPrefs.SetInt("Arti_GoldUpgrade", listGPGS[0].cloudTmpForGPGS_144);
        PlayerPrefs.SetInt("Arti_InfiReword", listGPGS[0].cloudTmpForGPGS_145);
        PlayerPrefs.SetInt("Arti_MiniReword", listGPGS[0].cloudTmpForGPGS_146);
        PlayerPrefs.SetInt("Arti_MiniGameTime", listGPGS[0].cloudTmpForGPGS_147);
        PlayerPrefs.SetInt("DailyCount_Cheak", listGPGS[0].cloudTmpForGPGS_148);
        PlayerPrefs.SetInt("NewDailyCount", listGPGS[0].cloudTmpForGPGS_149);
        //0615
        PlayerPrefs.SetInt("CRC_UP_Lv", listGPGS[0].cloudTmpForGPGS_150);
        PlayerPrefs.SetInt("CRD_UP_Lv", listGPGS[0].cloudTmpForGPGS_151);
        PlayerPrefs.SetInt("Dia_CRC_UP_Lv", listGPGS[0].cloudTmpForGPGS_152);
        PlayerPrefs.SetInt("Dia_CRD_UP_Lv", listGPGS[0].cloudTmpForGPGS_153);
        PlayerPrefs.SetString("CRC_UP", listGPGS[0].cloudTmpForGPGS_154);
        PlayerPrefs.SetString("CRD_UP", listGPGS[0].cloudTmpForGPGS_155);
        PlayerPrefs.SetString("Dia_CRC_UP", listGPGS[0].cloudTmpForGPGS_156);
        PlayerPrefs.SetString("Dia_CRD_UP", listGPGS[0].cloudTmpForGPGS_157);

        //0622
        PlayerPrefs.SetString("InfiPersonalRecord", listGPGS[0].cloudTmpForGPGS_158);
        //PlayerPrefs.SetString("weaponInfo", listGPGS[0].cloudTmpForGPGS_029);
        //PlayerPrefs.SetString("questInfo", listGPGS[0].cloudTmpForGPGS_030);
        //PlayerPrefs.SetString("questInfo2", listGPGS[0].cloudTmpForGPGS_107);
        //PlayerPrefs.SetString("questInfo3", listGPGS[0].cloudTmpForGPGS_116);
        //PlayerPrefs.SetString("questInfo4", listGPGS[0].cloudTmpForGPGS_129);
        ///PlayerPrefs.SetString("questInfo5", listGPGS[0].cloudTmpForGPGS_159);
        PlayerPrefs.SetInt("MaxGet_MuganTop2", listGPGS[0].cloudTmpForGPGS_160);
        //PlayerPrefs.SetString("uniformInfo", listGPGS[0].cloudTmpForGPGS_161);

        PlayerPrefs.SetString("Uniform_Data", listGPGS[0].cloudTmpForGPGS_162);
        PlayerPrefs.SetInt("Uniform_Curent", listGPGS[0].cloudTmpForGPGS_163);
        PlayerPrefs.SetInt("Chara_Lv", listGPGS[0].cloudTmpForGPGS_164);
        PlayerPrefs.SetInt("Pet_Curent", listGPGS[0].cloudTmpForGPGS_165);
        PlayerPrefs.SetInt("Pet_BuyData", listGPGS[0].cloudTmpForGPGS_166);
        PlayerPrefs.SetInt("Pet_Touch_Lv", listGPGS[0].cloudTmpForGPGS_167);
        PlayerPrefs.SetInt("Pet_Buff_Lv", listGPGS[0].cloudTmpForGPGS_168);
        PlayerPrefs.SetInt("Pet_Matt_Up_Lv", listGPGS[0].cloudTmpForGPGS_169);
        //
        //0706
        PlayerPrefs.SetInt("ticket", listGPGS[0].cloudTmpForGPGS_170);
        PlayerPrefs.SetInt("isFirstPVP", listGPGS[0].cloudTmpForGPGS_171);
        PlayerPrefs.SetInt("Gold_RECOV_UP_Lv", listGPGS[0].cloudTmpForGPGS_172);
        PlayerPrefs.SetInt("Dia_RECOV_UP_Lv", listGPGS[0].cloudTmpForGPGS_173);
        PlayerPrefs.SetInt("is0707shock", listGPGS[0].cloudTmpForGPGS_174);
        //
        //0727 Gold_Matto
        PlayerPrefs.SetInt("Pet_BuyData_Cape", listGPGS[0].cloudTmpForGPGS_175);
        PlayerPrefs.SetInt("Pet_PVP_Matt_Lv", listGPGS[0].cloudTmpForGPGS_176);


        /// ----------------

        PlayerPrefs.SetInt("Friend_01_OfflineAtk_Lv", listGPGS[0].cloudTmpForGPGS_177);
        PlayerPrefs.SetInt("Friend_02_OffTimeUp_Lv", listGPGS[0].cloudTmpForGPGS_178);
        PlayerPrefs.SetInt("Friend_03_OffSpped_Lv", listGPGS[0].cloudTmpForGPGS_179);
        PlayerPrefs.SetInt("Friend_04_MattzipPer_Lv", listGPGS[0].cloudTmpForGPGS_180);

        PlayerPrefs.SetString("MyArtiList", listGPGS[0].cloudTmpForGPGS_181);

        PlayerPrefs.SetInt("Arti_DEF_UP", listGPGS[0].cloudTmpForGPGS_182);
        PlayerPrefs.SetInt("Arti_SHILED_UP", listGPGS[0].cloudTmpForGPGS_183);
        PlayerPrefs.SetInt("Arti_HEALLING_UP", listGPGS[0].cloudTmpForGPGS_184);
        PlayerPrefs.SetInt("Arti_KIMCHI_UP", listGPGS[0].cloudTmpForGPGS_185);
        PlayerPrefs.SetInt("Arti_MattGrow_UP", listGPGS[0].cloudTmpForGPGS_186);


        /// 삭제 해줄 것들 광고 쿨타임 등등
        PlayerPrefs.DeleteKey("Bosster_Spin");
        PlayerPrefs.DeleteKey("Booster_AUTO");
        PlayerPrefs.DeleteKey("Booster_Body");
        PlayerPrefs.DeleteKey("Booster_KEY");
        PlayerPrefs.DeleteKey("Booster_Power");
        PlayerPrefs.DeleteKey("DenfenceMode");

        /// 로드한다 트리거
        isDataLoaded = true;
        PlayerPrefs.SetInt("isDataSaved", 0);
        PlayerPrefs.Save();

        /// 씬 갱신
        RestartAppForAOS();
        //UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    #endregion

    /// <summary>
    /// 안드로이드 네이티브 코드
    /// </summary>
    void RestartAppForAOS()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //play모드를 false로.
#else
        AndroidJavaObject AOSUnityActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject baseContext = AOSUnityActivity.Call<AndroidJavaObject>("getBaseContext");
        AndroidJavaObject intentObj = baseContext.Call<AndroidJavaObject>("getPackageManager").Call<AndroidJavaObject>("getLaunchIntentForPackage", baseContext.Call<string>("getPackageName"));
        AndroidJavaObject componentName = intentObj.Call<AndroidJavaObject>("getComponent");
        AndroidJavaObject mainIntent = intentObj.CallStatic<AndroidJavaObject>("makeMainActivity", componentName);
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        mainIntent = mainIntent.Call<AndroidJavaObject>("addFlags", intentClass.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK"));
        mainIntent = mainIntent.Call<AndroidJavaObject>("addFlags", intentClass.GetStatic<int>("FLAG_ACTIVITY_CLEAR_TASK"));
        baseContext.Call("startActivity", mainIntent);
        AndroidJavaClass JavaSystemClass = new AndroidJavaClass("java.lang.System");
        JavaSystemClass.CallStatic("exit", 0);
#endif
    }


    #region 쪼꼬미 데이타용 저장고



    public string ZZoGGoMiDataSave()
    {
        string result = "";

        result += DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "*";
        result += Mat_Mattzip + "*";
        result += PlayerPrefs.GetFloat("dDiamond", 0) + "*";
        result += gupbap + "*";
        result += ssalbap + "*";
        result += key;

        return result;
    }

    public void ZZoGGoMiDataLoad(string _Data)
    {
        string[] sDataList = (_Data).Split('*');

        PlayerPrefs.SetString("Z_Date", sDataList[0]);
        PlayerPrefs.SetString("Z_Mattzip", sDataList[1]);
        PlayerPrefs.SetString("Z_Dia", sDataList[2]);
        PlayerPrefs.SetString("Z_Gup", sDataList[3]);
        PlayerPrefs.SetString("Z_SSal", sDataList[4]);
        PlayerPrefs.SetString("Z_Key", sDataList[5]);

        PlayerPrefs.Save();
    }





    #endregion










}