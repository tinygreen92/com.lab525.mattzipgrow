using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class TutorialMissionManager : MonoBehaviour
{
    public TextAsset ta;
    [Header("튜토리얼 미션 메인 버튼")]
    public GameObject mainBtnObjt;
    [Header("하단 박스 캔버스")]
    public GameObject boxCanvas;
    public GameObject CoverBtn;


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

        PlayerPrefsManager.GetInstance().SaveShieldInfo();
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

    public void LoadMissionInfo()
    {
        string data = PlayerPrefs.GetString("missionInfo");

        if (!string.IsNullOrEmpty(data))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(data));

            // 가져온 데이터를 바이트 배열로 변환 -> 리스트로 캐스팅
            mMissionInfo = (List<MissionEntry>)binaryFormatter.Deserialize(memoryStream);
        }
    }

    public void InitMissionInfo()
    {
        if (mMissionInfo.Count != 0) return;

        PlayerPrefs.SetString("missionInfo", ta.text);
        PlayerPrefs.Save();
        /// 리스트 캐스팅
        LoadMissionInfo();
    }
}
