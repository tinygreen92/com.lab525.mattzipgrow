using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldBoxHintManager : MonoBehaviour
{
    public GameObject motherObj;
    public GameObject[] iconObj; // A, B, C 세개 돌려가며

    public Text[] TexObj0;  // Lenth(5) / 3가지


    // 하이드 애니메이션 재생 중이니?
    private bool isHideAnim;

    /// <summary>
    /// 골드박스 힌트 재생
    /// </summary>
    public void EnterTheMiniGame(int _index)
    {
        isHideAnim = false;

        if (motherObj.GetComponent<Animation>().isPlaying) return;

        iconObj[0].SetActive(false);
        iconObj[1].SetActive(false);
        iconObj[2].SetActive(false);

        /// 번역
        if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
        {
            switch (_index)
            {
                case 0:
                    iconObj[0].SetActive(true);
                    iconObj[3].GetComponent<Text>().text = "A 골드박스 포함 상품";
                    iconObj[3].transform.parent.GetComponent<Text>().text = "A 골드박스 포함 상품";
                    TexObj0[0].text = "10~100개"; // 다이아
                    TexObj0[1].text = "1~3개"; // 열쇠
                    TexObj0[2].text = "100~1000개"; // 국밥
                    TexObj0[3].text = "50~500개"; // 쌀밥

                    break;


                case 1:
                    iconObj[1].SetActive(true);
                    iconObj[3].GetComponent<Text>().text = "B 골드박스 포함 상품";
                    iconObj[3].transform.parent.GetComponent<Text>().text = "B 골드박스 포함 상품";
                    TexObj0[0].text = "10~300개"; // 다이아
                    TexObj0[1].text = "1~5개"; // 열쇠
                    TexObj0[2].text = "100~3000개"; // 국밥
                    TexObj0[3].text = "50~1500개"; // 쌀밥

                    break;


                case 2:
                    iconObj[2].SetActive(true);
                    iconObj[3].GetComponent<Text>().text = "C 골드박스 포함 상품";
                    iconObj[3].transform.parent.GetComponent<Text>().text = "C 골드박스 포함 상품";
                    TexObj0[0].text = "10~500개"; // 다이아
                    TexObj0[1].text = "1~10개"; // 열쇠
                    TexObj0[2].text = "100~5000개"; // 국밥
                    TexObj0[3].text = "50~2500개"; // 쌀밥

                    break;

            }
        }
        else
        {
            switch (_index)
            {
                case 0:
                    iconObj[0].SetActive(true);
                    iconObj[3].GetComponent<Text>().text = "번역";
                    iconObj[3].transform.parent.GetComponent<Text>().text = "번역 A 골드박스 포함 상품";
                    TexObj0[0].text = "10~100개"; // 다이아
                    TexObj0[1].text = "1~3개"; // 열쇠
                    TexObj0[2].text = "100~1000개"; // 국밥
                    TexObj0[3].text = "50~500개"; // 쌀밥

                    break;


                case 1:
                    iconObj[1].SetActive(true);
                    iconObj[3].GetComponent<Text>().text = "번역 B 골드박스 포함 상품";
                    iconObj[3].transform.parent.GetComponent<Text>().text = "B 골드박스 포함 상품";
                    TexObj0[0].text = "10~300개"; // 다이아
                    TexObj0[1].text = "1~5개"; // 열쇠
                    TexObj0[2].text = "100~3000개"; // 국밥
                    TexObj0[3].text = "50~1500개"; // 쌀밥

                    break;


                case 2:
                    iconObj[2].SetActive(true);
                    iconObj[3].GetComponent<Text>().text = "번역 C 골드박스 포함 상품";
                    iconObj[3].transform.parent.GetComponent<Text>().text = "C 골드박스 포함 상품";
                    TexObj0[0].text = "10~500개"; // 다이아
                    TexObj0[1].text = "1~10개"; // 열쇠
                    TexObj0[2].text = "100~5000개"; // 국밥
                    TexObj0[3].text = "50~2500개"; // 쌀밥

                    break;

            }
        }

        //전체 켜줌
        motherObj.SetActive(true);

        motherObj.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        motherObj.GetComponent<Animation>().Play("Roll_Incre");
    }
    public void HideMiniGame()
    {
        if (isHideAnim) return;

        isHideAnim = true;
        motherObj.SetActive(false);
    }


}
