using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPakaStarter : MonoBehaviour
{
    public Text[] TabNamebox;
    public NewPakageItem[] npi;

    public void StartBongbong()
    {
        for (int i = 0; i < npi.Length; i++)
        {
            npi[i].FxxkThisWay();
        }

        /// 탭 리네임
        if (TabNamebox.Length > 1)
        {
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
            {
                TabNamebox[0].text = "일일 패키지";
                TabNamebox[1].text = "주간 패키지";
                TabNamebox[2].text = "월간 패키지";
                TabNamebox[3].text = "일반 패키지";
                TabNamebox[4].text = "한정 패키지";
            }

            else
            {
                TabNamebox[0].text = "Daily package";
                TabNamebox[1].text = "Weekly package";
                TabNamebox[2].text = "Monthly package";
                TabNamebox[3].text = "일반 패키지";
                TabNamebox[4].text = "한정 패키지";
            }
        }

    }
}
