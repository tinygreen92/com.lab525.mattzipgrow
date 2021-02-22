using EasyMobile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Booster_KEY : MonoBehaviour
{
    // 조작 불가 타이머 스탬프
    private DateTime unbiasedTimerEndTimestamp;
    TimeSpan unbiasedRemaining;

    private void Awake()
    {
        unbiasedTimerEndTimestamp = ReadTimestamp("Booster_KEY", UnbiasedTime.Instance.Now());
        unbiasedRemaining = unbiasedTimerEndTimestamp - UnbiasedTime.Instance.Now();

        // 타이머 시간이 남아있다?
        if (unbiasedRemaining.TotalSeconds > 0)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            // 타이머 실행
            isTimerOn = true;
        }
        else /// 타이머 끝남.
        {
            // 타이머 숫자 숨겨준다.
            transform.GetChild(1).GetComponent<Text>().text = "00:00";
            
            // 타이머 끝났고 5개 미만이었으면 5개 풀충해주자
            if (PlayerPrefsManager.GetInstance().key < 20)
            {
                PlayerPrefsManager.GetInstance().key = 20;
                UserWallet.GetInstance().ShowUserKey();
            }

        }
    }

    void FixedUpdate()
    {
        if (!PlayerPrefsManager.GetInstance().isReadyQuest && !PlayerPrefsManager.GetInstance().isReadyWeapon) return;

        var keyAmount = PlayerPrefsManager.GetInstance().key;
        // 키가 4개 이하일때 타이머 실행 && 카운트 초기화가 됐을때
        if (keyAmount < 20 && isTimerOn)
        {
            transform.GetChild(1).gameObject.SetActive(true);

            // 시간 소모 
            unbiasedRemaining = unbiasedTimerEndTimestamp - UnbiasedTime.Instance.Now();
            this.WriteTimestamp("Booster_KEY", unbiasedTimerEndTimestamp);
            // 타이머가 아직 도는중
            if (unbiasedRemaining.TotalSeconds > 0)
            {
                transform.GetChild(1).GetComponent<Text>().text = string.Format("{0:00}:{1:00}", unbiasedRemaining.Minutes, unbiasedRemaining.Seconds);
                // 키가 정상적으로 됐으면 타이머 꺼줌.
                if (keyAmount >= 20)
                {
                    isTimerOn = false;
                    UserWallet.GetInstance().ShowUserKey();
                    transform.GetChild(1).GetComponent<Text>().text = "00:00";
                }
            }
            else // 타이머 끝났거나 열쇠 많음
            {
                if(isTimerOn) // 타이머만 끝남
                {
                    PlayerPrefsManager.GetInstance().key++;
                    UserWallet.GetInstance().ShowUserKey();
                    keyAmount = PlayerPrefsManager.GetInstance().key;
                }
                else // 타이머 안끝났는데 열쇠 많음.
                {
                    UserWallet.GetInstance().ShowUserKey();
                    isTimerOn = false;
                }

                // 초기값 5분
                transform.GetChild(1).GetComponent<Text>().text = "05:00";

                // 키가 정상적으로 됐으면 타이머 꺼줌.
                if (keyAmount >= 20)
                {
                    // 5개 이상이면 타이머 오브젝트 숨겨줌
                    transform.GetChild(1).GetComponent<Text>().text = "00:00";
                    UserWallet.GetInstance().ShowUserKey();
                    // 타이머 꺼줌
                    isTimerOn = false;
                }
                else /// 키가 아직 모자라다. -> 한바퀴 더 돌것.
                {
                    transform.GetChild(1).gameObject.SetActive(true);

                    unbiasedTimerEndTimestamp = UnbiasedTime.Instance.Now().AddSeconds(300);
                    this.WriteTimestamp("Booster_KEY", unbiasedTimerEndTimestamp);
                }
            }
        }
    }


    bool isTimerOn;

    /// <summary>
    /// 열쇠가 5개 이하로 내려가면 카운트
    /// 열쇠 소모 순간에 외부에서 호출 함
    /// </summary>
    public void KeyTimerStart()
    {
        // 타이머 돌고있으면 리턴
        if (isTimerOn) return;

        var keyAmount = PlayerPrefsManager.GetInstance().key;
        // 4개보다 많으면 호출 ㄴㄴ함
        if (keyAmount > 19) return;


        // 타이머 표시
        transform.GetChild(1).gameObject.SetActive(true);
        //초기화 하고 시간 추가.
        unbiasedTimerEndTimestamp = UnbiasedTime.Instance.Now().AddSeconds(300);
        this.WriteTimestamp("Booster_KEY", unbiasedTimerEndTimestamp);

        if (keyAmount < 20)
        {
            isTimerOn = true;
        }
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
