using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoDisposePopUp : MonoBehaviour
{
    private IEnumerator countPerCont; // 코코코 코지마
    private float _conuntTime;

    public float conuntTime = 2.0f;
    public Text noticeText;

    private void OnEnable()
    {
        _conuntTime = conuntTime;
        noticeText.text = _conuntTime + " 초 뒤 창 닫힘.";
        countPerCont = CountPerSecond();
        StartCoroutine(countPerCont);
    }


    /// <summary>
    /// 2초 카운터
    /// </summary>
    /// <returns>코루틴</returns>
    IEnumerator CountPerSecond()
    {
        for(; ; )
        {
            yield return new WaitForSeconds(1);

            _conuntTime -= 1.0f;
            noticeText.text = _conuntTime.ToString("D1") + " 초 뒤 창 닫힘";

            if (_conuntTime <= 0)
            {
                gameObject.SetActive(false);
            }

        }
    }

    private void OnDisable()
    {
        if (countPerCont != null) StopCoroutine(countPerCont);
    }

}
