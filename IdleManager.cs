using EasyMobile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleManager : MonoBehaviour
{
    [Header("대기중 위치 / 가리는 위치")]
    public Transform DisabledIdle_pos;
    public Transform AbledIdle_pos;
    //
    RectTransform ThisTrans; // 이 스크립트 붙은 rect

    //bool isAnimPlaying; // 코루틴ing 체크

    Coroutine moveRoutine;

    public void IdleMode_On()
    {
        moveRoutine = StartCoroutine(Progress());
    }

    public void IdleMode_Off()
    {
        moveRoutine = StartCoroutine(ProgressReverse());
    }

    /// <summary>
    /// 방치 모드 화면 가리기
    /// </summary>
    /// <returns></returns>
    private IEnumerator Progress()
    {
        //isAnimPlaying = true;

        float rate = 5f;
        float progress = 0.0f;

        while (progress <= 1f)
        {
            yield return new WaitForFixedUpdate();

            var moving = Mathf.Lerp(DisabledIdle_pos.position.y, AbledIdle_pos.position.y, progress);

            transform.position = new Vector2(AbledIdle_pos.position.x, moving);

            progress += rate * Time.deltaTime;
        }

        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
            moveRoutine = null;
        }

        // 코루틴 끝
        //isAnimPlaying = false;


        if (PlayerPrefsManager.GetInstance().VIP == 526 || PlayerPrefsManager.GetInstance().VIP == 625 || PlayerPrefsManager.GetInstance().VIP == 725 ||  PlayerPrefsManager.GetInstance().VIP == 925)
        {
            //
        }
        else
        {
            // 광고표기
            Advertising.ShowBannerAd(BannerAdNetwork.AdMob, BannerAdPosition.Top, BannerAdSize.SmartBanner);
        }



    }


    /// <summary>
    /// 방치모드 해제
    /// </summary>
    /// <returns></returns>
    private IEnumerator ProgressReverse()
    {
        //isAnimPlaying = true;

        float rate = 10f;
        float progress = 0.0f;

        while (progress <= 1f)
        {
            yield return new WaitForFixedUpdate();

            var moving = Mathf.Lerp(AbledIdle_pos.position.y, DisabledIdle_pos.position.y, progress);

            transform.position = new Vector2(AbledIdle_pos.position.x, moving);

            progress += rate * Time.deltaTime;
        }

        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
            moveRoutine = null;
        }

        // 코루틴 끝
        //isAnimPlaying = false;

        // 광고 숨기기
        Advertising.HideBannerAd();

    }
}








