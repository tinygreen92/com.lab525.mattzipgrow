using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPakaStarter : MonoBehaviour
{
    public NewPakageItem[] npi;

    public void StartBongbong()
    {
        for (int i = 0; i < npi.Length; i++)
        {
            npi[i].FxxkThisWay();
        }
    }
}
