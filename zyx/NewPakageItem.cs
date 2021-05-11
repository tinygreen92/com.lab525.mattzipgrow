using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPakageItem : MonoBehaviour
{
    public IAPManager im;

    [Header("- 아이콘")]
    public Image spriteBox;

    [Header("- 정보 표기 부분")]
    [Space]
    public Text vatText;
    public Text[] shopDesc;
    [Space]
    public Text nameBox;
    public Text costBox;

    public GameObject diaIcon;

    [Header("- 버튼 부분")]
    public GameObject MaxButton;
    public Text buttoName;



    /// <summary>
    /// 회색 버튼 활성화 = 클릭 안됨
    /// </summary>
    public void LockLimitBtn()
    {
        MaxButton.SetActive(true);
    }

    /// <summary>
    /// 회색 덮여있니?
    /// </summary>
    /// <returns></returns>
    public bool IsGrayScale()
    {
        if (MaxButton.activeSelf)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void FxxkThisWay()
    {
        int thisIndex = int.Parse(name);

        switch (thisIndex)
        {
            case 7:  if(PlayerPrefs.HasKey("limited_01")) MaxButton.SetActive(true); break;
            case 8:  if(PlayerPrefs.HasKey("limited_02")) MaxButton.SetActive(true); break;
            case 9:  if(PlayerPrefs.HasKey("limited_03")) MaxButton.SetActive(true); break;
            case 10: if(PlayerPrefs.HasKey("limited_04")) MaxButton.SetActive(true); break;
            case 11: if(PlayerPrefs.HasKey("limited_05")) MaxButton.SetActive(true); break;
            case 12: if(PlayerPrefs.HasKey("limited_06")) MaxButton.SetActive(true); break;
            case 13: if(PlayerPrefs.HasKey("limited_07")) MaxButton.SetActive(true); break;

            case 14: if(PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Day_01 != 0) MaxButton.SetActive(true); break;
            case 15: if(PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Day_02 != 0) MaxButton.SetActive(true); break;
            case 16: if(PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Day_03 != 0) MaxButton.SetActive(true); break;
            case 17: if(PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Day_04 != 0) MaxButton.SetActive(true); break;
            case 18: if(PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Day_05 != 0) MaxButton.SetActive(true); break;
            case 19: if(PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Week_01 != 0) MaxButton.SetActive(true); break;
            case 20: if(PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Week_02 != 0) MaxButton.SetActive(true); break;
            case 21: if(PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Week_03 != 0) MaxButton.SetActive(true); break;
            case 22: if(PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Week_04 != 0) MaxButton.SetActive(true); break;
            case 23: if(PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Week_05 != 0) MaxButton.SetActive(true); break;
            case 24: if(PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Mouth_01 != 0) MaxButton.SetActive(true); break;
            case 25: if(PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Mouth_02 != 0) MaxButton.SetActive(true); break;
            case 26: if(PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Mouth_03 != 0) MaxButton.SetActive(true); break;
            case 27: if(PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Mouth_04 != 0) MaxButton.SetActive(true); break;
            case 28: if(PlayerPrefsManager.GetInstance().dayLimitData[0].dia_Mouth_05 != 0) MaxButton.SetActive(true); break;

            default: break;
        }

        /// 번역
        bool isKorean = false;
        if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean") isKorean = true;

        var tmpInfo = im.GetPakInfo(thisIndex);
        /// 이미지 스프라이트 교체
        spriteBox.sprite = im.ShopImg[thisIndex];

        /// 패키지 상품 7개
        if (thisIndex < 7)
        {
            /// 번역
            if (isKorean)
                vatText.text = "※ VAT 포함";
            else
                vatText.text = "※ VAT included";
            vatText.gameObject.SetActive(true);
        }
        /// 한정 패키지
        else if (thisIndex < 14)
        {
            /// 번역
            if (isKorean)
                vatText.text = "※ VAT 포함 / 청약철회 불가상품";
            else
                vatText.text = "※ VAT included";
            vatText.gameObject.SetActive(true);
        }
        else
        {
            vatText.gameObject.SetActive(false);
        }

        if (isKorean)
            nameBox.text = tmpInfo.pakaName; /// 해당 아이템은 숫자로 구성되어야 함
        else
            nameBox.text = im.pakageEnglishInfos[thisIndex];



        /// 14미만은 결제 금액 표기해주시고
        if (thisIndex < 14 || thisIndex == 18 || thisIndex == 23 || thisIndex == 28)
        {
            diaIcon.SetActive(false);
            /// 원화 / 달러 세팅
            if (isKorean)
            {
                costBox.text = $"₩ {tmpInfo.pakaPrice:N0}";
            }
            else
            {
                System.Globalization.NumberFormatInfo numberFormat = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
                costBox.text = tmpInfo.pakaPrice.ToString("C", numberFormat);
            }
        }
        /// 아니면 다이아 표시
        else
        {
            costBox.text = tmpInfo.pakaPrice.ToString("N0");

            if (costBox.text == "0")
            {
                costBox.text = "-";
                diaIcon.SetActive(false);
            }
            else
            {
                diaIcon.SetActive(true);
            }
        }



        /// 번역
        if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
        {
            /// 포함 상품 표기
            if (tmpInfo.getDia != 0)
            {
                shopDesc[0].text = $"다이아 x {tmpInfo.getDia:N0}";
                shopDesc[0].gameObject.SetActive(true);
            }
            if (tmpInfo.getShiled != 0)
            {
                shopDesc[1].text = $"방패뽑기권 x {tmpInfo.getShiled:N0}";
                shopDesc[1].gameObject.SetActive(true);
            }
            if (tmpInfo.getGuapbap != 0)
            {
                shopDesc[2].text = $"국밥 x {tmpInfo.getGuapbap:N0}";
                shopDesc[2].gameObject.SetActive(true);
            }
            if (tmpInfo.getSsalbap != 0)
            {
                shopDesc[3].text = $"쌀밥 x {tmpInfo.getSsalbap:N0}";
                shopDesc[3].gameObject.SetActive(true);
            }
            if (tmpInfo.getKimchi != 0)
            {
                shopDesc[4].text = $"깍두기 x {tmpInfo.getKimchi:N0}";
                shopDesc[4].gameObject.SetActive(true);
            }
            if (tmpInfo.getKey != 0)
            {
                shopDesc[5].text = $"열쇠 x {tmpInfo.getKey:N0}";
                shopDesc[5].gameObject.SetActive(true);
            }
            if (tmpInfo.getTicket != 0)
            {
                shopDesc[6].text = $"PvP입장권 x {tmpInfo.getTicket:N0}";
                shopDesc[6].gameObject.SetActive(true);
            }
        }
        else
        {
            /// 포함 상품 표기
            if (tmpInfo.getDia != 0)
            {
                shopDesc[0].text = $"Diamond x {tmpInfo.getDia:N0}";
                shopDesc[0].gameObject.SetActive(true);
            }
            if (tmpInfo.getShiled != 0)
            {
                shopDesc[1].text = $"Shield ticket x {tmpInfo.getShiled:N0}";
                shopDesc[1].gameObject.SetActive(true);
            }
            if (tmpInfo.getGuapbap != 0)
            {
                shopDesc[2].text = $"Korean soup x {tmpInfo.getGuapbap:N0}";
                shopDesc[2].gameObject.SetActive(true);
            }
            if (tmpInfo.getSsalbap != 0)
            {
                shopDesc[3].text = $"Rice x {tmpInfo.getSsalbap:N0}";
                shopDesc[3].gameObject.SetActive(true);
            }
            if (tmpInfo.getKimchi != 0)
            {
                shopDesc[4].text = $"Radish kimchi x {tmpInfo.getKimchi:N0}";
                shopDesc[4].gameObject.SetActive(true);
            }
            if (tmpInfo.getKey != 0)
            {
                shopDesc[5].text = $"Key x {tmpInfo.getKey:N0}";
                shopDesc[5].gameObject.SetActive(true);
            }
            if (tmpInfo.getTicket != 0)
            {
                shopDesc[6].text = $"PvP ticket x {tmpInfo.getTicket:N0}";
                shopDesc[6].gameObject.SetActive(true);
            }
        }




    }

}
