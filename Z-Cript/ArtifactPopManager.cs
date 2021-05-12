using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactPopManager : MonoBehaviour
{
    public GameObject Btn1set;
    public GameObject Btn2set;
    [Space]
    public Transform IconSet; // 아이콘 부모
    public Text DescText; // 설명 텍스트.

    private void OnEnable()
    {
        if (GetComponent<Animation>().isPlaying) return;

        GetComponent<Animation>()["Roll_Incre"].speed = 1;
        GetComponent<Animation>().Play("Roll_Incre");
        transform.GetChild(0).GetComponent<Animation>().Play("Roll_Pop");
    }


    int[] index = new int[12];
    string[] mGname = new string[12];
    int seed = 0;

    /// <summary>
    /// GatChaManager 에서 호출해서 텍스트 채워줌.
    /// </summary>
    public void SetTextAll(int[] _index, string[] _name, int _seed)
    {
        index = _index;
        mGname = _name;
        seed = _seed;
        /// 버튼 교란
        if (seed == 0 || seed > 10)
        {
            Btn2set.SetActive(false);
            Btn1set.SetActive(true);
        }
        else
        {
            Btn2set.SetActive(true);
            Btn1set.SetActive(false);
        }
        /// 아이콘
        switch (_index[seed])
        {
            case 0: IconSet.GetChild(0).gameObject.SetActive(true); break;
            case 1: IconSet.GetChild(1).gameObject.SetActive(true); break;
            case 2: IconSet.GetChild(2).gameObject.SetActive(true); break;
            case 3: IconSet.GetChild(3).gameObject.SetActive(true); break;
            case 4: IconSet.GetChild(4).gameObject.SetActive(true); break;
            case 5: IconSet.GetChild(5).gameObject.SetActive(true); break;
            case 6: IconSet.GetChild(6).gameObject.SetActive(true); break;
            case 7: IconSet.GetChild(7).gameObject.SetActive(true); break;
            case 8: IconSet.GetChild(8).gameObject.SetActive(true); break;
            case 9: IconSet.GetChild(9).gameObject.SetActive(true); break;
                /// 신규 유물 0608
            case 10: IconSet.GetChild(10).gameObject.SetActive(true); break;
            case 11: IconSet.GetChild(11).gameObject.SetActive(true); break;
            case 12: IconSet.GetChild(12).gameObject.SetActive(true); break;
            case 13: IconSet.GetChild(13).gameObject.SetActive(true); break;
            case 14: IconSet.GetChild(14).gameObject.SetActive(true); break;
            case 15: IconSet.GetChild(15).gameObject.SetActive(true); break;
            case 16: IconSet.GetChild(16).gameObject.SetActive(true); break;
                /// 신규 유물 0408
            case 17: IconSet.GetChild(17).gameObject.SetActive(true); break;
            case 18: IconSet.GetChild(18).gameObject.SetActive(true); break;
            case 19: IconSet.GetChild(19).gameObject.SetActive(true); break;
            case 20: IconSet.GetChild(20).gameObject.SetActive(true); break;
            case 21: IconSet.GetChild(21).gameObject.SetActive(true); break;


        }
        if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
            DescText.text = mGname[seed] + "획득하였습니다.";
        else
            DescText.text = "You got " + mGname[seed];

        /// 무야호 사운드
        AudioManager.instance.Btn_Mooyaho();
        gameObject.SetActive(true);
    }


    // 스킵이나 X 누르면 호출
    public void HideArtiPop()
    {
        /// 인트로 시청 안했으면 리턴
        if (!PlayerPrefsManager.GetInstance().isFristGameStart) return;

        for (int i = 0; i < IconSet.childCount; i++)
        {
            IconSet.GetChild(i).gameObject.SetActive(false);
        }

        gameObject.SetActive(false);
    }

    // 확인 누르면 계속 확인.
    public void CheakArtiPop()
    {
        HideArtiPop();

        if (seed == 0 || seed > 10) 
            return;
        seed++;
        //Debug.LogWarning("seed " + seed);
        // 한번더?
        RetryGatcha();
    }

    // 한번더 가챠.
    void RetryGatcha()
    {
        GetComponent<Animation>()["Roll_Incre"].speed = 1;
        GetComponent<Animation>().Play("Roll_Incre");
        transform.GetChild(0).GetComponent<Animation>().Play("Roll_Pop");

        SetTextAll(index, mGname, seed);
    }
}
