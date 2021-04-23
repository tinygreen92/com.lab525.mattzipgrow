using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Booster_Spin : MonoBehaviour
{
    public GameObject dailyPopup;
    [Header("출석 버튼에 달린 카운터")]
    public Text cheackCountText;
    [Header("스핀 텍스트")]
    public Text SpinText;

    // 조작 불가 타이머 스탬프
    private DateTime unbiasedTimerEndTimestamp;
    private DateTime dailyEndTimestamp;
    TimeSpan unbiasedRemaining;
    TimeSpan dailydRemaining;
    private void Start()
    {
        dailyEndTimestamp = ReadTimestamp("Bosster_Spin", UnbiasedTime.Instance.Now());
        // 다음날 0시 시간. /  최신값
        DateTime currentTime = UnbiasedTime.Instance.Now().Date.AddDays(1);
        // 값 없으면 다음날 0시 시간. / 저장된 값
        unbiasedTimerEndTimestamp = LoadDateTime().Date;
        //Debug.LogError("앞 : " + currentTime  +" / 뒤 : " + unbiasedTimerEndTimestamp);
        /// 최신값이 바뀌었다 = 날짜가 바뀌었다 = 아직 출첵 안했다.
        if (currentTime > unbiasedTimerEndTimestamp)
        {
            Debug.LogError("!!!!! 최신값이 바뀌었다 ResetDailyQuest !!!!!");

            ResetDailyQuest();
        }
    }

    void FixedUpdate()
    {
        dailydRemaining = dailyEndTimestamp - UnbiasedTime.Instance.Now();
        unbiasedRemaining = unbiasedTimerEndTimestamp - UnbiasedTime.Instance.Now();

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
        else // 카운트 0미만 이면 하루 지났다 / 아님 출석 안했다. / 출석 체크 안하고 같은 날에 껏다가 켰다.
        {
            /// 같은 날이면 일퀘 갱신 시도 저지
            if (isSameDayReCheck) return;
            // 날짜 바뀌면 체크.
            ResetDailyQuest();
            isSameDayReCheck = true;

        }
    }

    public DailyRewardController drc;
    bool isReset;

    /// <summary>
    ///  같은 날에 접속 했니? 퀘스트 리셋 하지마
    /// </summary>
    bool isSameDayReCheck;

    /// <summary>
    /// 날짜 바뀌었을때 체크 해준다.
    /// </summary>
    void ResetDailyQuest()
    {
        /// questInfo[0] 로딩 될때까지 루프
        if (!PlayerPrefsManager.GetInstance().isReadyWeapon)
        {
            Invoke(nameof(ResetDailyQuest), 0.6f);
            return;
        }

        // 중복 방지. = 계쏙 리셋 되는거 방지.
        if (!isReset)
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
            PlayerPrefs.SetInt("NewDailyCount", 0);
            cheackCountText.transform.parent.gameObject.SetActive(false);
            /// 출석체크 판 초기화.
            if (PlayerPrefsManager.GetInstance().DailyCount_Cheak >= 25) drc.ResetDailyBoard();

            // 일퀘 초기화.
            PlayerPrefsManager.GetInstance().questInfo[0].daily_Abs = 0;
            PlayerPrefsManager.GetInstance().questInfo[0].daily_Atk = 0;
            PlayerPrefsManager.GetInstance().questInfo[0].daily_HP = 0;
            PlayerPrefsManager.GetInstance().questInfo[0].daily_Punch = 0;
            PlayerPrefsManager.GetInstance().questInfo[0].daily_MiniCombo = 0;
            PlayerPrefsManager.GetInstance().questInfo[0].daily_ArtiGatcha = 0;
            PlayerPrefsManager.GetInstance().questInfo[0].daily_LMITABS = 0;

            /// 일일 / 주간 / 월간 무료 갱신
            PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Day_01 = 0;
            PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Day_02 = 0;
            PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Day_03 = 0;
            PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Day_04 = 0;
            PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Day_05 = 0;
            //RedDotManager.instance.RedDot[9].SetActive(true);
            //RedDotManager.instance.RedDot[10].SetActive(true);

            /// 주간 갱신 
            ResetMonday(PlayerPrefsManager.GetInstance().dayLimitData[0].weekend_Day);

            /// 월간 갱신 1일
            if (UnbiasedTime.Instance.Now().Day == 1 ||
                UnbiasedTime.Instance.Now().Day < PlayerPrefsManager.GetInstance().dayLimitData[0].mouth_Day)
            {
                PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Mouth_01 = 0;
                PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Mouth_02 = 0;
                PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Mouth_03 = 0;
                PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Mouth_04 = 0;
                PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Mouth_05 = 0;
                PlayerPrefsManager.GetInstance().dayLimitData[0].mouth_Day = 0;
                //RedDotManager.instance.RedDot[13].SetActive(true);
                //RedDotManager.instance.RedDot[14].SetActive(true);
            }


            // 광고 리미트 초기화.
            PlayerPrefs.SetInt("Shield10AdsCnt", 0);

            // 티켓이 모자라면 충전
            if (PlayerPrefs.GetInt("ticket", 5) < 5) PlayerPrefs.SetInt("ticket", 5);
            PlayerPrefs.Save();
            
            /// 일/주/월 저장
            PlayerPrefsManager.GetInstance().SaveDayLimitData();


            /// 출석창 호출
            dailyPopup.SetActive(true);
            dailyPopup.GetComponent<Animation>()["Roll_Incre"].speed = 1;
            dailyPopup.GetComponent<Animation>().Play("Roll_Incre");

        }
    }



    /// <summary>
    /// 무슨 요일에 보상을 받았니?
    /// </summary>
    /// <param name="_weekDay"></param>
    void ResetMonday(int _weekDay)
    {
        /// 만약 _weekDay = 5이면 금요일에 패키지 받고 
        /// 월요일 접속 안하고
        /// 다음주 수요일에 접속하는 경우
        /// UnbiasedTime.Instance.Now().DayOfWeek =Wednesday 3
        int dayWeek = (int)UnbiasedTime.Instance.Now().DayOfWeek;
        /// 0 = 일요일은 예외 처리하고 7일 이내에 들어올 경우 초기화
        if (dayWeek != 0 && dayWeek < _weekDay)
        {
            PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Week_01 = 0;
            PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Week_02 = 0;
            PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Week_03 = 0;
            PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Week_04 = 0;
            //RedDotManager.instance.RedDot[11].SetActive(true);
            //RedDotManager.instance.RedDot[12].SetActive(true);
        }
        /// 일요일은 지났는데 일요일에 초기화 했으면
        else if (dayWeek != 0 && _weekDay == 0)
        {
            PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Week_01 = 0;
            PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Week_02 = 0;
            PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Week_03 = 0;
            PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Week_04 = 0;
            //RedDotManager.instance.RedDot[11].SetActive(true);
            //RedDotManager.instance.RedDot[12].SetActive(true);
        }
    }


    /// <summary>
    /// 체크 해제는 한번만 한다.
    /// </summary>
    void ReverseReset()
    {
        if (!isReset) 
            return;
        isSameDayReCheck = false;
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
            dailyEndTimestamp = UnbiasedTime.Instance.Now().AddSeconds(10800 - 1);
            this.WriteTimestamp("Bosster_Spin", dailyEndTimestamp);

            // 3시간 카운트 시작했으면 1
            PlayerPrefsManager.GetInstance().DailySpinReword = 1;
        }

        // 커버 벗기기
        transform.GetChild(0).gameObject.SetActive(false);

    }



    /// <summary>
    /// 출석체크 시간을 저장한다. 조심해.
    /// </summary>
    /// <param name="dateTime"></param>
    void SaveDateTime(DateTime dateTime)
    {
        string tmp = dateTime.ToString("yyyyMMddHHmmss");
        PlayerPrefs.SetString("Daily_Timer", tmp);
        PlayerPrefs.Save();
    }

    DateTime LoadDateTime()
    {
        if (!PlayerPrefs.HasKey("Daily_Timer")) 
            return UnbiasedTime.Instance.Now().Date.AddDays(1); //내일 0시 반환.

        string data = PlayerPrefs.GetString("Daily_Timer");

        return DateTime.ParseExact(data, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
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
        PlayerPrefs.Save();
    }
}
