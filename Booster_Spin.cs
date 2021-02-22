using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Booster_Spin : MonoBehaviour
{
    [Header("출석 버튼에 달린 카운터")]
    public Text cheackCountText;
    [Header("스핀 텍스트")]
    public Text SpinText;

    // 조작 불가 타이머 스탬프
    private DateTime unbiasedTimerEndTimestamp;
    private DateTime dailyEndTimestamp;
    TimeSpan unbiasedRemaining;
    TimeSpan dailydRemaining;

    private void Awake()
    {
        dailyEndTimestamp = ReadTimestamp("Bosster_Daily", UnbiasedTime.Instance.Now());
    }

    private void Start()
    {
        // 다음날 0시 시간. /  최신값
        DateTime currentTime = UnbiasedTime.Instance.Now().Date.AddDays(1);
        // 값 없으면 다음날 0시 시간. / 저장된 값
        unbiasedTimerEndTimestamp = LoadDateTime().Date;

        if (currentTime > unbiasedTimerEndTimestamp)
        {
            /// 최신값이 바뀌었다 = 날짜가 바뀌었다 = 스핀 조건 충족
            // 데이터값 최신화 시켜주고 스핀 돌리기.
            ResetDailyQuest();
            // 티켓 초기화
            if (PlayerPrefs.GetInt("ticket", 5) < 5)
            {
                PlayerPrefs.SetInt("ticket", 5);
            }
        }
        else /// 그렇지 않다 = 같은 날이다 = 아직 0시 안 지났다. = 스핀 못함.
        {
            //transform.GetChild(0).gameObject.SetActive(true);
        }

        if (unbiasedRemaining.TotalSeconds > 0 && PlayerPrefsManager.GetInstance().NewDailyCount == 1)
        {
            // 출첵 안되게.
            ReverseReset();

            cheackCountText.text = string.Format("{0:00}:{1:00}:{2:00}", unbiasedRemaining.Hours, unbiasedRemaining.Minutes, unbiasedRemaining.Seconds);
        }

    }

    void FixedUpdate()
    {
        unbiasedRemaining = unbiasedTimerEndTimestamp - UnbiasedTime.Instance.Now();

        dailydRemaining = dailyEndTimestamp - UnbiasedTime.Instance.Now();

        ///  3시간 스핀 타이머가 돌아가는중?
        if (dailydRemaining.TotalSeconds > 0)
        {
            transform.GetChild(0).gameObject.SetActive(true);

            transform.GetChild(1).GetComponent<Text>().text = string.Format("{0:00}:{1:00}:{2:00}", dailydRemaining.Hours, dailydRemaining.Minutes, dailydRemaining.Seconds);
            SpinText.text = string.Format("{0:00}:{1:00}:{2:00}", dailydRemaining.Hours, dailydRemaining.Minutes, dailydRemaining.Seconds);

        }
        else
        {
            // 3시간 카운트 끝났으면 0
            PlayerPrefsManager.GetInstance().DailySpinReword = 0;

            transform.GetChild(1).GetComponent<Text>().text = "Spin Now";
            SpinText.text = "Spin Now";
            // 온 했다가 시간 다되면 커버 꺼줌 = 원이미지임
            transform.GetChild(0).gameObject.SetActive(false);

        }




        /// 24시간 카운트가 계속 돌아가는 상태 = 아직 날짜 안지남. / 출첵도 함.
        if (unbiasedRemaining.TotalSeconds > 0 && PlayerPrefsManager.GetInstance().NewDailyCount == 1)
        {
            // 출첵 안되게.
            ReverseReset();
            cheackCountText.text = string.Format("{0:00}:{1:00}:{2:00}", unbiasedRemaining.Hours, unbiasedRemaining.Minutes, unbiasedRemaining.Seconds);

        }
        else // 카운트 0미만 이면 하루 지났다 / 아님 출석 안했다.
        {
            //transform.GetChild(1).GetComponent<Text>().text = "Spin Now";
            //SpinText.text = "Spin Now";
            //// 온 했다가 시간 다되면 커버 꺼줌 = 원이미지임
            //transform.GetChild(0).gameObject.SetActive(false);
            // 날짜 바뀌면 체크.
            ResetDailyQuest();

        }
    }

    public DailyRewardController drc;
    bool isReset;
    /// <summary>
    /// 날짜 바뀌었을때 체크 해준다.
    /// </summary>
    void ResetDailyQuest()
    {
        // 중복 방지. = 계쏙 리셋 되는거 방지.
        if (!isReset && PlayerPrefsManager.GetInstance().isReadyWeapon)
        {
            Debug.LogError("!!!!! Quest Reset !!!!!");

            isReset = true;
            // 다음날 0시 시간. /  최신값
            DateTime currentTime = UnbiasedTime.Instance.Now().Date.AddDays(1);
            // 데이터값 최신화 시켜주고 스핀 돌리기.
            unbiasedTimerEndTimestamp = currentTime;
            // 최신값 세이브
            SaveDateTime(currentTime);

            // 출석 초기화.
            PlayerPrefsManager.GetInstance().NewDailyCount = 0;
            cheackCountText.transform.parent.gameObject.SetActive(false);
            // 일퀘 초기화.
            PlayerPrefsManager.GetInstance().questInfo[0].daily_Abs = 0;
            PlayerPrefsManager.GetInstance().questInfo[0].daily_Atk = 0;
            PlayerPrefsManager.GetInstance().questInfo[0].daily_HP = 0;
            PlayerPrefsManager.GetInstance().questInfo[0].daily_Punch = 0;
            PlayerPrefsManager.GetInstance().questInfo[0].daily_MiniCombo = 0;
            PlayerPrefsManager.GetInstance().questInfo[0].daily_ArtiGatcha = 0;
            PlayerPrefsManager.GetInstance().questInfo[0].daily_LMITABS = 0;

            /// 출석체크 판 초기화.
            if (PlayerPrefsManager.GetInstance().DailyCount_Cheak >= 25) drc.ResetDailyBoard();

            PlayerPrefs.Save();
        }
    }


    /// <summary>
    /// 체크 해제는 한번만 한다.
    /// </summary>
    void ReverseReset()
    {
        if (!isReset) return;

        isReset = false;
    }



    /// <summary>
    /// 3시간 짜리 스핀용
    /// </summary>
    public void TEST_Btn()
    {
        //부스터 버튼이 온 일떄
        if (dailydRemaining.TotalSeconds > 0)
        {
            return;
        }
        else // 부스터 버튼 꺼져있으면 초기화 하고 시간 추가.
        {

            PlayerPrefs.Save();
            dailyEndTimestamp = UnbiasedTime.Instance.Now().AddSeconds(10800 - 1);
            this.WriteTimestamp("Bosster_Daily", dailyEndTimestamp);

            // 3시간 카운트 시작했으면 1
            PlayerPrefsManager.GetInstance().DailySpinReword = 1;
        }

        // 커버 벗기기
        transform.GetChild(0).gameObject.SetActive(false);

    }




    void SaveDateTime(DateTime dateTime)
    {
        string tmp = dateTime.ToString("yyyyMMddHHmmss");
        PlayerPrefs.SetString("Booster_SPIN", tmp);
    }

    DateTime LoadDateTime()
    {
        if (!PlayerPrefs.HasKey("Booster_SPIN")) return UnbiasedTime.Instance.Now().Date.AddDays(1); //내일 0시 반환.
        string data = PlayerPrefs.GetString("Booster_SPIN");
        var saveDateTime = DateTime.ParseExact(data, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);

        return saveDateTime;
    }



    private DateTime ReadTimestamp(string key, DateTime defaultValue)
    {
        long tmp = Convert.ToInt64(PlayerPrefs.GetString(key, "0"));
        if (tmp == 0)
        {
            return defaultValue;
        }
        return DateTime.FromBinary(tmp);
    }

    private void WriteTimestamp(string key, DateTime time)
    {
        PlayerPrefs.SetString(key, time.ToBinary().ToString());
    }
}
