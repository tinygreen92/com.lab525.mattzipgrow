using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VIPManager : MonoBehaviour
{
    static VIPManager instance;
    public static VIPManager GetInstance() { return instance; }
    private void Awake() { instance = this;}


    [Header("- 광고 광고 아이콘 덮어줄 버튼.")]
    public GameObject[] AdsIcons;
    [Header("- 특별 상점 순서대로 나열")]
    public GameObject[] inAppItems;
    // 단일 구매자는 각각 다른거 살수도 있고 패키지 살수도 있음.
    /// (i)오토 공격 구매자          VIP = 525
    /// (i)광고제거 구매자           VIP = 526
    /// (i)맷집무한 구매자           VIP = 527
    /// (i)골드무한 구매자           VIP = 528
    /// 
    // 버프만 두개 구매한 놈도 따로 관리.
    /// (i)버프 두개구매자           VIP = 527528
    /// 
    // 패키지 구매한 놈들은 자기한테 없는거만 활성화.
    /// (i)올인원 구매자              VIP = 625
    /// (i)자동+광고 구매자           VIP = 725    // 얘는 특별취급. 버프 아무거나 한개 사도 올인원으로 승급.
    /// (i)자동+버프 구매자           VIP = 825
    /// (i)광고+버프 구매자           VIP = 925
    /// 
    ///


    /// <summary>
    /// 게임 시작할 때 호출
    /// 패키지 상품 구매시 호출
    /// 구매 가능/불가능 딱 정해준다.
    /// </summary>
    public void VIPINIT()
    {
        var vip = PlayerPrefsManager.GetInstance().VIP;
        // 아무것도 안 구입 = 모든 구매 상품 열려있다.
        if (vip == 0)
        {
            for(int i =0; i< inAppItems.Length; i++)
            {
                inAppItems[i].SetActive(false);
                inAppItems[i].transform.parent.GetComponent<Button>().interactable = true;
            }
        }
        // 0번 오토공격 구매
        if(vip == 525)
        {
            inAppItems[0].SetActive(true);
            inAppItems[0].transform.parent.GetComponent<Button>().interactable = false;
        }
        else if (vip == 526)
        {
            inAppItems[1].SetActive(true);
            inAppItems[1].transform.parent.GetComponent<Button>().interactable = false;
        }
        else if (vip == 527)
        {
            inAppItems[2].SetActive(true);
            inAppItems[2].transform.parent.GetComponent<Button>().interactable = false;
        }
        else if (vip == 528)
        {
            inAppItems[3].SetActive(true);
            inAppItems[3].transform.parent.GetComponent<Button>().interactable = false;
        }

        // 버프만 두개 구매한 놈도 따로 관리.
        /// (i)버프 두개구매자           VIP = 527528
        /// 
        // 패키지 구매한 놈들은 자기한테 없는거만 활성화.
        /// (4)올인원 구매자              VIP = 625
        /// (5)자동+광고 구매자           VIP = 725    // 얘는 특별취급. 버프 아무거나 한개 사도 올인원으로 승급.
        /// (6)자동+버프 구매자           VIP = 825
        /// (7)광고+버프 구매자           VIP = 925
        /// 
        /// (0)오토 공격 구매자          VIP = 525
        /// (1)광고제거 구매자           VIP = 526
        /// (2)맷집무한 구매자           VIP = 527
        /// (3)골드무한 구매자           VIP = 528
        //올인원 = 전부 구매 금지
        if (vip == 625)
        {
            for (int i = 0; i < inAppItems.Length; i++)
            {
                inAppItems[i].SetActive(true);
                inAppItems[i].transform.parent.GetComponent<Button>().interactable = false;
            }
        }
        // 자동 + 광고 = 잠궈준다.
        else if (vip == 725)
        {
            inAppItems[0].SetActive(true);
            inAppItems[0].transform.parent.GetComponent<Button>().interactable = false;
            inAppItems[1].SetActive(true);
            inAppItems[1].transform.parent.GetComponent<Button>().interactable = false;
            inAppItems[5].SetActive(true);
            inAppItems[5].transform.parent.GetComponent<Button>().interactable = false;
        }
        // 자동 + 버프 = 잠궈준다.
        else if (vip == 825)
        {
            inAppItems[0].SetActive(true);
            inAppItems[0].transform.parent.GetComponent<Button>().interactable = false;
            inAppItems[2].SetActive(true);
            inAppItems[2].transform.parent.GetComponent<Button>().interactable = false;
            inAppItems[3].SetActive(true);
            inAppItems[3].transform.parent.GetComponent<Button>().interactable = false;
            inAppItems[6].SetActive(true);
            inAppItems[6].transform.parent.GetComponent<Button>().interactable = false;
        }
        // 광고 + 버프 = 잠궈준다.
        else if (vip == 925)
        {
            inAppItems[1].SetActive(true);
            inAppItems[1].transform.parent.GetComponent<Button>().interactable = false;
            inAppItems[2].SetActive(true);
            inAppItems[2].transform.parent.GetComponent<Button>().interactable = false;
            inAppItems[3].SetActive(true);
            inAppItems[3].transform.parent.GetComponent<Button>().interactable = false;
            inAppItems[7].SetActive(true);
            inAppItems[7].transform.parent.GetComponent<Button>().interactable = false;
        }

        AdsIconsInit();
    }



    /// <summary>
    /// 조건 되면 광고 아이콘 잠금.
    /// </summary>
    public void AdsIconsInit()
    {
        var vvip = PlayerPrefsManager.GetInstance().VIP;

        if (vvip == 526 || vvip == 625 || vvip == 725 || vvip == 925)
        {
            for (int i = 0; i < AdsIcons.Length; i++)
            {
                AdsIcons[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < AdsIcons.Length; i++)
            {
                AdsIcons[i].SetActive(false);
            }
        }
    }


}

