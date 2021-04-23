using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPakaStarter : MonoBehaviour
{
    public NewPakageItem[] npi;

    private void Start()
    {
        StartCoroutine(bongbong());
    }

    IEnumerator bongbong()
    {
        yield return null;
        // 로딩 다 될때까지 무한 대기
        while (!PlayerPrefsManager.GetInstance().isReadyDayLimit)
        {
            yield return new WaitForFixedUpdate();
        }

        for (int i = 0; i < npi.Length; i++)
        {
            npi[i].FxxkThisWay();
        }
    }
}
