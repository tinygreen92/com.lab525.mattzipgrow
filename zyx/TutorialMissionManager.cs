using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMissionManager : MonoBehaviour
{
    public TextAsset ta;
    [Header("튜토리얼 미션 메인 버튼")]
    public GameObject mainBtnObjt;
    [Header("하단 박스 캔버스")]
    public GameObject boxCanvas;
    public GameObject CoverBtn;
    public Text missionText;
    [Header("보상 획득 팝업")]
    public GameObject GettingPop;
    public GameObject[] GetIcons;
    public Text GetAmount;

    DoubleToStringNum dts = new DoubleToStringNum(); // 단위 변환기

    [Serializable]
    public class MissionEntry
    {
        public string missionReword;        // 미션 Nanoo 보상
        public int missionAmount;           // 보상 갯수
        public int missionPassOrNot;        // 클리어 하면 1
    }

    [TextArea]
    public string[] mTitles;

    // 만든 클래스를 리스트에 담아서 테이블처럼 사용
    public List<MissionEntry> mMissionInfo = new List<MissionEntry>();

    public void Sparser()
    {
        string[] line = ta.text.Substring(0, ta.text.Length).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            mMissionInfo.Add(new MissionEntry
            {
                missionReword = row[0],
                missionAmount = int.Parse(row[1]),
                missionPassOrNot = int.Parse(row[2]),
            });
        }

        SaveMissionInfo();
    }






    public void TEST_SkipTuto()
    {
        mMissionInfo[currentMissionIndex++].missionPassOrNot = -1;
        /// 미션 내용 갱신
        missionText.text = mTitles[currentMissionIndex];
        /// 이미 달성한 경우에 오픈해주기
        UpdateMission(currentMissionIndex);
    }











    public void SaveMissionInfo()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();

        // Info를 바이트 배열로 변환해서 저장
        binaryFormatter.Serialize(memoryStream, mMissionInfo);

        // 그것을 다시 한번 문자열 값으로 변환해서 
        // 스트링 키값으로 PlayerPrefs에 저장
        PlayerPrefs.SetString("missionInfo", Convert.ToBase64String(memoryStream.GetBuffer()));
        PlayerPrefs.Save();
    }

    public bool isLoadMission;
    private int MISSION_CNT;
    /// <summary>
    /// 현재 텍스트에 표기될 미션 인덱스
    /// </summary>
    private int currentMissionIndex;

    public void LoadMissionInfo()
    {
        /// 만약 미션 올 클리어라면 버튼 숨겨줌
        if (PlayerPrefs.GetInt("isTutoAllClear", 0) == 525)
        {
            mainBtnObjt.SetActive(false);
            return;
        }

        string data = PlayerPrefs.GetString("missionInfo");

        if (!string.IsNullOrEmpty(data))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(data));

            // 가져온 데이터를 바이트 배열로 변환 -> 리스트로 캐스팅
            mMissionInfo = (List<MissionEntry>)binaryFormatter.Deserialize(memoryStream);
            MISSION_CNT = mMissionInfo.Count;
        }
        boxCanvas.SetActive(false);
        isLoadMission = true;
        /// 다음 미션은 무엇?
        NextMission();
    }

    public void InitMissionInfo()
    {
        /// 로드 된 미션 정보 있으면 리턴
        if (mMissionInfo.Count != 0) return;

        PlayerPrefs.SetString("missionInfo", ta.text);
        PlayerPrefs.Save();
        /// 리스트 캐스팅
        LoadMissionInfo();
    }


    /// <summary>
    /// 최근 미션 갱신 MISSION_CNT
    /// </summary>
    private void NextMission()
    {
        /// 총 미션 갯수만큼 포문
        for (int i = 0; i < MISSION_CNT; i++)
        {
            /// 0 은 미션중, -1은 미션 끝
            if (mMissionInfo[i].missionPassOrNot != -1)
            {
                currentMissionIndex = i;
                break;
            }
        }
        /// 미션 내용 갱신
        missionText.text = mTitles[currentMissionIndex];
        /// 이미 달성한 경우에 오픈해주기
        if (currentMissionIndex == 1 ||
            currentMissionIndex == 9 ||
            currentMissionIndex == 10 ||
            currentMissionIndex == 20 ||
            currentMissionIndex == 25 ||
            currentMissionIndex == 31 ||
            currentMissionIndex == 36 ||
            currentMissionIndex == 40 ||
            currentMissionIndex == 46 ||
            currentMissionIndex == 50 ||
            currentMissionIndex == 58 ||
            currentMissionIndex == 60 ||
            currentMissionIndex == 67 ||
            currentMissionIndex == 70 ||
            currentMissionIndex == 77 ||
            currentMissionIndex == 80 ||
            currentMissionIndex == 85 ||
            currentMissionIndex >= 91 )
        {
            UpdateMission(currentMissionIndex);
        }
        else
        {
            CoverBtn.SetActive(true);
        }

    }

    /// <summary>
    /// 최근 미션 갱신 MISSION_CNT
    /// </summary>
    void UpdateMission(int _index)
    {
        /// 해당 미션 차례아니면 리턴
        if (currentMissionIndex != _index) return;

        int clearCnt;
        switch (_index)
        {
            case 0: clearCnt = 10; break;
            case 1: clearCnt = 5;
                if (dts.SubStringDouble("5", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "-1" ||
                    dts.SubStringDouble("5", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;
            case 2: clearCnt = 1; break;
            case 3: clearCnt = 1; break;
            case 4: clearCnt = 1; break;
            case 5: clearCnt = 1; break;
            case 6: clearCnt = 1; break;
            case 7: clearCnt = 1; break;
            case 8: clearCnt = 1; break;
            case 9: clearCnt = 5;
                if (dts.SubStringDouble("5", PlayerPrefsManager.GetInstance().Chara_Lv) == "-1" ||
                    dts.SubStringDouble("5", PlayerPrefsManager.GetInstance().Chara_Lv) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;
            case 10: clearCnt = 30;
                if (dts.SubStringDouble("30", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "-1" ||
                    dts.SubStringDouble("30", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;
            case 11: clearCnt = 1; break;
            case 12: clearCnt = 1; break;
            case 13: clearCnt = 1; break;
            case 14: clearCnt = 1; break;
            case 15: clearCnt = 1; break;
            case 16: clearCnt = 1; break;
            case 17: clearCnt = 1; break;
            case 18: clearCnt = 1; break;
            case 19: clearCnt = 1; break;
            case 20: clearCnt = 90;
                if (dts.SubStringDouble("90", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "-1" ||
                    dts.SubStringDouble("90", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;
            case 21: clearCnt = 1; break;
            case 22: clearCnt = 1; break;
            case 23: 
                clearCnt = 1;
                /// TODO : 골드 획득 확률 정상화.
                PlayerPrefsManager.GetInstance().isEmptyLuckBox = false;
                break;
            case 24: clearCnt = 1; break;
            case 25: clearCnt = 20;
                if (dts.SubStringDouble("20", PlayerPrefsManager.GetInstance().Chara_Lv) == "-1" ||
                    dts.SubStringDouble("20", PlayerPrefsManager.GetInstance().Chara_Lv) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;
            case 26: clearCnt = 1; break;
            case 27: clearCnt = 1; break;
            case 28: clearCnt = 1; break;
            case 29: clearCnt = 1; break;
            case 30: clearCnt = 1; break;
            case 31: clearCnt = 150;
                if (dts.SubStringDouble("150", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "-1" ||
                    dts.SubStringDouble("150", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;
            case 32: clearCnt = 1; break;
            case 33: clearCnt = 1; break;
            case 34: clearCnt = 1; break;
            case 35: clearCnt = 1; break;
            case 36: clearCnt = 50;
                if (dts.SubStringDouble("50", PlayerPrefsManager.GetInstance().Chara_Lv) == "-1" ||
                    dts.SubStringDouble("50", PlayerPrefsManager.GetInstance().Chara_Lv) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;
            case 37: clearCnt = 1; break;
            case 38: clearCnt = 1; break;
            case 39: clearCnt = 1; break;
            case 40: clearCnt = 300;
                if (dts.SubStringDouble("300", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "-1" ||
                    dts.SubStringDouble("300", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;
            case 41: clearCnt = 1; break;
            case 42: clearCnt = 1; break;
            case 43: clearCnt = 1; break;
            case 44: clearCnt = 1; break;
            case 45: clearCnt = 1; break;
            case 46: clearCnt = 100;
                if (dts.SubStringDouble("100", PlayerPrefsManager.GetInstance().Chara_Lv) == "-1" ||
                    dts.SubStringDouble("100", PlayerPrefsManager.GetInstance().Chara_Lv) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;
            case 47: clearCnt = 1; break;
            case 48: clearCnt = 1; break;
            case 49: clearCnt = 1; break;
            case 50: clearCnt = 500;
                if (dts.SubStringDouble("500", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "-1" ||
                    dts.SubStringDouble("500", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;
            case 51: clearCnt = 1; break;
            case 52: clearCnt = 1; break;
            case 53: clearCnt = 1; break;
            case 54: clearCnt = 1; break;
            case 55: clearCnt = 1; break;
            case 56: clearCnt = 1; break;
            case 57: clearCnt = 1; break;
            case 58: clearCnt = 150;
                if (dts.SubStringDouble("150", PlayerPrefsManager.GetInstance().Chara_Lv) == "-1" ||
                    dts.SubStringDouble("150", PlayerPrefsManager.GetInstance().Chara_Lv) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;
            case 59: clearCnt = 1; break;
            case 60: clearCnt = 1000;
                if (dts.SubStringDouble("1000", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "-1" ||
                    dts.SubStringDouble("1000", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;
            case 61: clearCnt = 1; break;
            case 62: clearCnt = 1; break;
            case 63: clearCnt = 1; break;
            case 64: clearCnt = 1; break;
            case 65: clearCnt = 1; break;
            case 66: clearCnt = 1; break;
            case 67: clearCnt = 200;
                if (dts.SubStringDouble("200", PlayerPrefsManager.GetInstance().Chara_Lv) == "-1" ||
                    dts.SubStringDouble("200", PlayerPrefsManager.GetInstance().Chara_Lv) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;
            case 68: clearCnt = 1; break;
            case 69: clearCnt = 1; break;
            case 70: clearCnt = 1500;
                if (dts.SubStringDouble("1500", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "-1" ||
                    dts.SubStringDouble("1500", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;
            case 71: clearCnt = 1; break;
            case 72: clearCnt = 1; break;
            case 73: clearCnt = 1; break;
            case 74: clearCnt = 1; break;
            case 75: clearCnt = 1; break;
            case 76: clearCnt = 1; break;
            case 77: clearCnt = 250;
                if (dts.SubStringDouble("250", PlayerPrefsManager.GetInstance().Chara_Lv) == "-1" ||
                    dts.SubStringDouble("250", PlayerPrefsManager.GetInstance().Chara_Lv) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;
            case 78: clearCnt = 1; break;
            case 79: clearCnt = 1; break;
            case 80: clearCnt = 2000;
                if (dts.SubStringDouble("2000", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "-1" ||
                    dts.SubStringDouble("2000", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;
            case 81: clearCnt = 1; break;
            case 82: clearCnt = 1; break;
            case 83: clearCnt = 1; break;
            case 84: clearCnt = 1; break;
            case 85: clearCnt = 300;
                if (dts.SubStringDouble("300", PlayerPrefsManager.GetInstance().Chara_Lv) == "-1" ||
                    dts.SubStringDouble("300", PlayerPrefsManager.GetInstance().Chara_Lv) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;
            case 86: clearCnt = 1; break;
            case 87: clearCnt = 1; break;
            case 88: clearCnt = 1; break;
            case 89: clearCnt = 1; break;
            case 90: clearCnt = 1; break;
            case 91: clearCnt = 2500;
                if (dts.SubStringDouble("2500", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "-1" ||
                dts.SubStringDouble("2500", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;
            case 92: clearCnt = 3000;
                if (dts.SubStringDouble("3000", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "-1" ||
                    dts.SubStringDouble("3000", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;
            case 93: clearCnt = 4000;
                if (dts.SubStringDouble("4000", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "-1" ||
                    dts.SubStringDouble("4000", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;
            case 94: clearCnt = 5000;
                if (dts.SubStringDouble("5000", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "-1" ||
                    dts.SubStringDouble("5000", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;
            case 95: clearCnt = 6000;
                if (dts.SubStringDouble("6000", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "-1" ||
                    dts.SubStringDouble("6000", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;
            case 96: clearCnt = 7000;
                if (dts.SubStringDouble("7000", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "-1" ||
                    dts.SubStringDouble("7000", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;
            case 97: clearCnt = 8000;
                if (dts.SubStringDouble("8000", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "-1" ||
                    dts.SubStringDouble("8000", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;
            case 98: clearCnt = 9000;
                if (dts.SubStringDouble("9000", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "-1" ||
                    dts.SubStringDouble("9000", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;
            case 99: clearCnt = 10000;
                if (dts.SubStringDouble("10000", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "-1" ||
                    dts.SubStringDouble("10000", PlayerPrefsManager.GetInstance().Mat_Mattzip) == "0")
                {
                    mMissionInfo[_index].missionPassOrNot = clearCnt;
                }
                break;

            default: clearCnt = 0; break;
        }

        /// 미션 클리어 가능 경우라면?
        if (mMissionInfo[_index].missionPassOrNot >= clearCnt)
        {
            CoverBtn.SetActive(false);
        }
        else
        {
            CoverBtn.SetActive(true);
        }

    }


    /// <summary>
    /// 외부에서 호출하는 갱신
    /// </summary>
    /// <param name="_Index"></param>
    public void ExUpdateMission(int _Index)
    {
        if (mMissionInfo[_Index].missionPassOrNot == -1
            || currentMissionIndex != _Index) return;

        mMissionInfo[_Index].missionPassOrNot++;
        UpdateMission(_Index);
    }
    public void ExUpdateMission(int _Index, double _Amount)
    {
        if (mMissionInfo[_Index].missionPassOrNot == -1
            || currentMissionIndex != _Index) return;

        mMissionInfo[_Index].missionPassOrNot = (int)_Amount;
        UpdateMission(_Index);
    }




    /// <summary>
    /// 미션 완료했을때 노란 버튼 활성화 될때 클릭하면
    /// </summary>
    public void ClickedMissionBtn()
    {
        /// 회색 버튼이면 눌러도 무반응
        if (CoverBtn.activeSelf) return;

        ///미션 완료했다.
        mMissionInfo[currentMissionIndex].missionPassOrNot = -1;
        SaveMissionInfo();

        /// 완료 팝업
        GetMissionReword(mMissionInfo[currentMissionIndex++]);
        PlayerPrefs.SetInt("isTutoAllClear", currentMissionIndex);
        PlayerPrefs.Save();

        /// 골드박스 확률 일시 증가
        if (currentMissionIndex == 23)
        {
            PlayerPrefsManager.GetInstance().isEmptyLuckBox = true;
        }


        /// 100번째 미션 깨면 끝
        if (currentMissionIndex >= 100)
        {
            PlayerPrefs.SetInt("isTutoAllClear", 525);
            PlayerPrefs.Save();
            SaveMissionInfo();
            mainBtnObjt.SetActive(false);
            return;
        }
        /// 다음 미션 갱신
        NextMission();
    }


    /// <summary>
    /// 받기 누르면 보상 지급
    /// </summary>
    /// <param name="_strIndex"></param>
    void GetMissionReword(MissionEntry _strIndex)
    {
        var ppm = PlayerPrefsManager.GetInstance();
        //
        string tmpGupBap = ppm.gupbap;
        string tmpSSal = ppm.ssalbap;
        string tmpKimchi = ppm.Kimchi;
        float tmpDia = PlayerPrefs.GetFloat("dDiamond");

        /// 팝업 아이콘 초기화\
        for (int i = 0; i < GetIcons.Length; i++)
        {
            GetIcons[i].SetActive(false);
        }

        UserWallet.GetInstance().isNoShow = true;

        /// reword 뽑아내서
        switch (_strIndex.missionReword)
        {
            case "diamond":
                GetIcons[0].SetActive(true);
                GetAmount.text = $"다이아 x {_strIndex.missionAmount}";

                PlayerPrefs.SetFloat("dDiamond", PlayerPrefs.GetFloat("dDiamond", 0) + _strIndex.missionAmount);
                break;
            case "ticket":
                GetIcons[1].SetActive(true);
                GetAmount.text = $"PvP 입장권 x {_strIndex.missionAmount}";

                ppm.ticket += _strIndex.missionAmount;
                break;
            case "shield":
                GetIcons[2].SetActive(true);
                GetAmount.text = $"방패 뽑기권 x {_strIndex.missionAmount}";

                ppm.ShiledTicket += _strIndex.missionAmount;
                break;
            case "ssal":
                GetIcons[3].SetActive(true);
                GetAmount.text = $"쌀밥 x {_strIndex.missionAmount}";

                ppm.ssalbap = dts.AddStringDouble(tmpSSal, _strIndex.missionAmount.ToString());
                break;
            case "gupbap":
                GetIcons[4].SetActive(true);
                GetAmount.text = $"국밥 x {_strIndex.missionAmount}";

                ppm.gupbap = dts.AddStringDouble(tmpGupBap, _strIndex.missionAmount.ToString());
                break;
            case "key":
                GetIcons[5].SetActive(true);
                GetAmount.text = $"열쇠 x {_strIndex.missionAmount}";

                ppm.key += _strIndex.missionAmount;
                break;
            case "kimchi":
                GetIcons[6].SetActive(true);
                GetAmount.text = $"깍두기 x {_strIndex.missionAmount}";

                ppm.Kimchi = dts.AddStringDouble(tmpKimchi, _strIndex.missionAmount.ToString());
                break;

            default: break;
        }

        /// 팝업 호출
        ShowGettingReword();
    }

    /// <summary>
    /// 보상 팝업 띄우기
    /// </summary>
    void ShowGettingReword()
    {
        GettingPop.SetActive(true);
        GettingPop.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        GettingPop.GetComponent<Animation>().Play("Roll_Incre");
    }


    /// <summary>
    /// 받기 누르거나, x 버튼 누르면 닫고 재화 새로고침
    /// </summary>
    public void ClickedSummitBtn()
    {
        PopUpObjectManager.GetInstance().ShowWarnnigProcess("튜토리얼 미션 보상이 지급되었습니다.");

        GettingPop.SetActive(false);
        UserWallet.GetInstance().isNoShow = false;
        UserWallet.GetInstance().ShowAllMoney();
    }




}
