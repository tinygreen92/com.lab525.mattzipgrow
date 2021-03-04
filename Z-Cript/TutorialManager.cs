using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject FakeLoading;

    public Text[] HeroChatText;
    public Image[] HeroCut;
    //
    public Text[] MainChatText;
    public GameObject[] MainObject;

    private string writerText;
    bool isButtonClicked = false;
    Coroutine Tutorial;

    /// <summary>
    /// 튜토리얼 호출 -> 코루틴 시작
    /// </summary>
    public void TutoStart()
    {
        transform.parent.gameObject.SetActive(true);
        Tutorial = StartCoroutine(TextPractice());
        if (PlayerPrefsManager.GetInstance().ATK_Lv == 0) PlayerPrefsManager.GetInstance().ATK_Lv++;
    }

    /// <summary>
    /// 클릭 감지
    /// </summary>
    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) && !PlayerPrefsManager.GetInstance().isFristGameStart)
        {
            isButtonClicked = true;
        }
    }

    int NextNarration = 0;
    /// <summary>
    /// 대화 내용 출력 1
    /// </summary>
    /// <param name="narration">대화 내용 적으라</param>
    /// <returns>코루틴 반환</returns>
    IEnumerator NormalChat(string narration)
    {
        writerText = "";
        HeroChatText[NextNarration].text = writerText;
        int alpha = 0;

        while (alpha < 255)
        {
            yield return new WaitForFixedUpdate();
            alpha+=5;
            HeroCut[NextNarration].color = new Color(1, 1, 1, alpha / 255.0f);
        }

        HeroChatText[NextNarration].transform.parent.gameObject.SetActive(true);

        //텍스트 타이핑 효과
        for (int a = 0; a < narration.Length; a++)
        {
            writerText += narration[a];
            HeroChatText[NextNarration].text = writerText;

            if(NextNarration == 11)
            {
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                yield return null;
            }

        }

        //키를 다시 누를 때까지 무한정 대기
        while (true)
        {
            if (isButtonClicked)
            {
                NextNarration++;
                isButtonClicked = false;
                break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }


    /// <summary>
    /// 대화 내용 출력 1
    /// </summary>
    /// <param name="narration">대화 내용 적으라</param>
    /// <returns>코루틴 반환</returns>
    IEnumerator MainChat(string narration, int _index)
    {
        // 스킵버튼 켜
        MainObject[1].SetActive(true);
        // 회색 음영 켜
        MainObject[0].SetActive(true);

        //
        MainObject[10].SetActive(false);
        writerText = "";
        MainChatText[_index].text = writerText;

        if (_index == 1)
        {
            MainChatText[0].gameObject.SetActive(false);
            MainChatText[1].gameObject.SetActive(true);
        }else if (_index == 0)
        {
            MainChatText[0].gameObject.SetActive(true);
            MainChatText[1].gameObject.SetActive(false);
        }


        //텍스트 타이핑 효과
        for (int a = 0; a < narration.Length; a++)
        {
            yield return new WaitForFixedUpdate();

            writerText += narration[a];
            MainChatText[_index].text = writerText;
        }

        yield return new WaitForSeconds(1f);

        //키를 다시 누를 때까지 무한정 대기
        while (true)
        {
            if (isButtonClicked)
            {
                isButtonClicked = false;
                break;
            }
            yield return new WaitForSeconds(0.5f);

        }
    }

    /// <summary>
    /// 펀치 해봅시다.
    /// </summary>
    /// <returns></returns>
    IEnumerator LetsDoit(string narration, int _index)
    {
        switch (_index)
        {
            case 0:
                // 스킵버튼 없애준다.
                MainObject[1].SetActive(false);
                // 회색 음영 없애준다.
                MainObject[0].SetActive(false);
                MainChatText[0].gameObject.SetActive(false);
                MainChatText[1].gameObject.SetActive(false);
                MainChatText[2].transform.parent.gameObject.SetActive(false);
                MainChatText[3].transform.parent.gameObject.SetActive(false);

                //펀치 열번 할때까지 대기
                while (true)
                {
                    yield return new WaitForFixedUpdate();

                    if (PlayerPrefsManager.GetInstance().TurtorialCount >= 10)
                    {
                        isButtonClicked = false;

                        PlayerPrefsManager.GetInstance().TurtorialCount = 0;

                        break;
                    }
                }


                break;



            case 1:
                // 대사내용 초기화
                writerText = "";
                MainChatText[1].text = writerText;
                //노란색 대사
                MainChatText[0].gameObject.SetActive(false);
                MainChatText[1].gameObject.SetActive(true);
                //
                //텍스트 타이핑 효과
                for (int a = 0; a < narration.Length; a++)
                {
                    yield return new WaitForFixedUpdate();

                    writerText += narration[a];
                    MainChatText[1].text = writerText;
                }
                // 강화 아이콘 하이라이트
                MainObject[2].SetActive(true);

                yield return new WaitForSeconds(1);

                //키 눌러서 GameObject.setFalse 까지 대기 
                while (true)
                {
                    yield return new WaitForSeconds(0.5f);

                    if (!MainObject[2].activeSelf)
                    {
                        isButtonClicked = false;
                        break;
                    }

                }

                break;


            case 2:
                // 훈련 강화 포커싱

                writerText = "";
                // 대사창 닫고
                MainChatText[1].text = "";
                //노란색 대사
                MainChatText[0].gameObject.SetActive(false);
                MainChatText[1].gameObject.SetActive(false);
                MainChatText[2].transform.parent.gameObject.SetActive(true);
                //훈련강화 포커싱 켜주고
                MainObject[3].SetActive(true);
                //
                //텍스트 타이핑 효과
                for (int a = 0; a < narration.Length; a++)
                {
                    yield return new WaitForFixedUpdate();

                    writerText += narration[a];
                    MainChatText[2].text = writerText;
                }
                yield return new WaitForSeconds(1);

                //키 눌러서 GameObject.setFalse 까지 대기 
                while (true)
                {
                    yield return new WaitForFixedUpdate();

                    if (Index_Case_2c)
                    {
                        //공격 강화 포커싱 닫고
                        isButtonClicked = false;

                        break;
                    }
                }

                break;


            case 3:
                // 훈련 강화 대사침

                writerText = "";
                // 대사창 닫고
                MainChatText[2].text = "";
                // 강화 된걸로 교체해줌
                MainObject[4].SetActive(false);
                MainObject[5].SetActive(true);
                //
                Index_Case_2c = false;

                //텍스트 타이핑 효과
                for (int a = 0; a < narration.Length; a++)
                {
                    yield return new WaitForFixedUpdate();

                    writerText += narration[a];
                    MainChatText[2].text = writerText;
                }
                yield return new WaitForSeconds(1);

                //키를 다시 누를 때까지 무한정 대기
                while (true)
                {
                    yield return new WaitForSeconds(0.5f);

                    if (isButtonClicked)
                    {
                        isButtonClicked = false;
                        break;
                    }

                }

                break;

            case 4:
                // 훈련 강화 자매품 소개

                writerText = "";
                MainChatText[2].text = "";
                MainChatText[3].text = "";
                //노란색 대사 위치 바꿔줌
                MainChatText[2].transform.parent.gameObject.SetActive(false);
                MainChatText[3].transform.parent.gameObject.SetActive(true);
                // 그리드 바꿔줌.
                MainObject[9].transform.GetChild(2).gameObject.SetActive(false);
                MainObject[9].transform.GetChild(3).gameObject.SetActive(false);
                MainObject[9].transform.GetChild(4).gameObject.SetActive(false);

                MainObject[6].SetActive(true);


                //텍스트 타이핑 효과
                for (int a = 0; a < narration.Length; a++)
                {
                    yield return new WaitForFixedUpdate();

                    writerText += narration[a];
                    MainChatText[3].text = writerText;
                }
                yield return new WaitForSeconds(1);

                //키를 다시 누를 때까지 무한정 대기
                while (true)
                {
                    yield return new WaitForSeconds(0.5f);

                    if (isButtonClicked)
                    {
                        MainChatText[3].text = "";
                        MainObject[6].SetActive(false);
                        MainObject[3].SetActive(false);
                        isButtonClicked = false;
                        PlayerPrefsManager.GetInstance().TurtorialCount = 0;
                        break;
                    }

                }

                break;


            case 5:
                // 스킵버튼 없애준다.
                MainObject[1].SetActive(false);
                // 회색 음영 없애준다.
                MainObject[0].SetActive(false);
                MainChatText[0].gameObject.SetActive(false);
                MainChatText[1].gameObject.SetActive(false);
                MainChatText[2].transform.parent.gameObject.SetActive(false);
                MainChatText[3].transform.parent.gameObject.SetActive(false);

                PlayerPrefsManager.GetInstance().ATK_Lv++;
                GameObject.Find("GroggyManager").GetComponent<GroggyManager>().PowerUP_Init();

                while (true)
                {
                    yield return new WaitForFixedUpdate();

                    if (PlayerPrefsManager.GetInstance().TurtorialCount >= 1000)
                    {
                        isButtonClicked = false;
                        break;
                    }

                }


                break;

            case 6:
                // 훈련 강화 포커싱

                writerText = "";
                // 대사창 닫고
                MainChatText[1].text = "";
                //노란색 대사
                MainObject[3].SetActive(false);
                MainObject[0].SetActive(true);
                MainChatText[0].gameObject.SetActive(false);
                MainChatText[1].gameObject.SetActive(true);


                //
                //텍스트 타이핑 효과
                for (int a = 0; a < narration.Length; a++)
                {
                    yield return new WaitForFixedUpdate();

                    writerText += narration[a];
                    MainChatText[1].text = writerText;
                }
                yield return new WaitForSeconds(1);

                //키 눌러서 GameObject.setFalse 까지 대기 
                while (true)
                {
                    yield return new WaitForSeconds(0.5f);

                    if (isButtonClicked)
                    {
                        //공격 강화 포커싱 닫고
                        isButtonClicked = false;
                        break;
                    }
                }

                break;




            case 7:
                // 훈련 강화 포커싱

                writerText = "";
                // 대사창 닫고
                MainChatText[1].text = "";

                //타임바 켜줌.
                MainObject[8].SetActive(true);
                //
                //텍스트 타이핑 효과
                for (int a = 0; a < narration.Length; a++)
                {
                    yield return new WaitForFixedUpdate();

                    writerText += narration[a];
                    MainChatText[1].text = writerText;
                }
                yield return new WaitForSeconds(1f);

                //키를 다시 누를 때까지 무한정 대기
                while (true)
                {
                    yield return new WaitForSeconds(0.5f);

                    if (isButtonClicked)
                    {
                        isButtonClicked = false;
                        break;
                    }

                }

                break;


            case 8:
                MainChatText[1].text = "";
                MainChatText[2].text = "";

                // 스킵버튼 없애준다.
                MainObject[1].SetActive(false);
                MainObject[0].SetActive(false);
                MainChatText[0].gameObject.SetActive(false);
                MainChatText[1].gameObject.SetActive(false);
                MainChatText[2].transform.parent.gameObject.SetActive(false);
                MainChatText[3].transform.parent.gameObject.SetActive(false);

                while (true)
                {
                    yield return new WaitForSeconds(1);

                    if (PlayerPrefsManager.GetInstance().TurtorialCount >= 52525)
                    {
                        MainObject[8].SetActive(false);
                        PlayerPrefsManager.GetInstance().TurtorialCount = 0;
                        isButtonClicked = false;

                        break;
                    }

                }

                break;


            case 9:
                // 훈련 강화 대사침

                writerText = "";
                // 대사창 닫고
                MainChatText[1].text = "";
                MainChatText[2].text = "";
                MainChatText[3].text = "";
                MainChatText[1].gameObject.SetActive(false);
                MainChatText[2].transform.parent.gameObject.SetActive(true);

                // 강화 된걸로 교체해줌
                MainObject[11].SetActive(true);
                MainObject[11].transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
                MainObject[11].transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(false);

                //텍스트 타이핑 효과
                for (int a = 0; a < narration.Length; a++)
                {
                    yield return new WaitForFixedUpdate();

                    writerText += narration[a];
                    MainChatText[2].text = writerText;
                }
                yield return new WaitForSeconds(1);

                //키를 다시 누를 때까지 무한정 대기
                while (true)
                {
                    yield return new WaitForSeconds(0.5f);

                    if (isButtonClicked)
                    {
                        isButtonClicked = false;
                        break;
                    }

                }

                break;

            case 10:
                // 훈련 강화 대사침

                writerText = "";
                // 대사창 닫고
                MainChatText[1].text = "";
                MainChatText[2].text = "";
                MainChatText[1].gameObject.SetActive(false);
                MainChatText[2].transform.parent.gameObject.SetActive(true);
                // 강화 된걸로 교체해줌
                MainObject[11].transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
                MainObject[11].transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(true);

                //텍스트 타이핑 효과
                for (int a = 0; a < narration.Length; a++)
                {
                    yield return new WaitForFixedUpdate();

                    writerText += narration[a];
                    MainChatText[2].text = writerText;
                }
                yield return new WaitForSeconds(1);

                //키를 다시 누를 때까지 무한정 대기
                while (true)
                {
                    yield return new WaitForSeconds(0.5f);

                    if (isButtonClicked)
                    {
                        isButtonClicked = false;
                        break;
                    }

                }

                break;


            case 11:
                // 훈련 강화 대사침

                writerText = "";
                // 대사창 닫고
                MainChatText[1].text = "";
                MainChatText[2].text = "";
                MainChatText[1].gameObject.SetActive(true);
                MainChatText[2].transform.parent.gameObject.SetActive(false);

                // 파워업 화면 꺼줌
                MainObject[10].SetActive(false);
                MainObject[11].SetActive(false);
                MainObject[14].SetActive(true);

                //텍스트 타이핑 효과
                for (int a = 0; a < narration.Length; a++)
                {
                    yield return new WaitForFixedUpdate();

                    writerText += narration[a];
                    MainChatText[1].text = writerText;
                }
                yield return new WaitForSeconds(1);

                //키를 다시 누를 때까지 무한정 대기
                while (true)
                {
                    yield return new WaitForSeconds(0.5f);

                    if (isButtonClicked)
                    {
                        isButtonClicked = false;
                        break;
                    }

                }

                break;


            case 12:
                // 훈련 강화 대사침

                writerText = "";
                MainChatText[1].text = "";
                MainChatText[2].text = "";
                MainChatText[1].gameObject.SetActive(true);
                MainChatText[2].transform.parent.gameObject.SetActive(false);
                // 파워업 화면 꺼줌
                MainObject[14].SetActive(false);
                MainObject[13].SetActive(true);

                //텍스트 타이핑 효과
                for (int a = 0; a < narration.Length; a++)
                {
                    yield return new WaitForFixedUpdate();

                    writerText += narration[a];
                    MainChatText[1].text = writerText;
                }
                yield return new WaitForSeconds(1);

                //키를 다시 누를 때까지 무한정 대기
                while (true)
                {
                    yield return new WaitForSeconds(0.5f);

                    if (Index_MiniGamec)
                    {
                        // 스킵버튼 없애준다.
                        MainObject[1].SetActive(false);
                        // 회색 음영 없애준다.
                        MainObject[0].SetActive(false);
                        MainObject[13].SetActive(false);
                        MainObject[16].SetActive(false);
                        //
                        MainChatText[1].gameObject.SetActive(false);
                        MainChatText[2].transform.parent.gameObject.SetActive(false);

                        isButtonClicked = false;
                        break;
                    }

                }

                break;

            case 13:

                MainObject[0].SetActive(false);
                MainChatText[0].gameObject.SetActive(false);
                MainChatText[1].gameObject.SetActive(false);
                MainChatText[2].transform.parent.gameObject.SetActive(false);
                MainChatText[3].transform.parent.gameObject.SetActive(false);
                //펀치 열번 할때까지 대기
                while (true)
                {
                    yield return new WaitForFixedUpdate();
                    // 무한 버티기 끝남?
                    if (PlayerPrefsManager.GetInstance().isTuToInfi)
                    {
                        MainObject[16].SetActive(true);

                        isButtonClicked = false;
                        break;
                    }
                }


                break;


            case 14:
                // 대사내용 초기화
                writerText = "";
                MainChatText[1].text = writerText;
                //노란색 대사
                MainChatText[0].gameObject.SetActive(false);
                MainChatText[1].gameObject.SetActive(true);
                //
                MainObject[7].SetActive(false);
                // 강화 아이콘 하이라이트
                MainObject[15].SetActive(true);
                //텍스트 타이핑 효과
                for (int a = 0; a < narration.Length; a++)
                {
                    yield return new WaitForFixedUpdate();

                    writerText += narration[a];
                    MainChatText[1].text = writerText;
                }

                yield return new WaitForSeconds(1);

                //키 눌러서 GameObject.setFalse 까지 대기 
                while (true)
                {
                    yield return new WaitForSeconds(0.5f);

                    if (!MainObject[15].activeSelf)
                    {
                        isButtonClicked = false;
                        break;
                    }

                }

                break;


            case 15:

                // 스킵버튼 살려
                MainObject[1].SetActive(true);
                // 회색 음영 살려
                MainObject[0].SetActive(true);
                // 대사내용 초기화
                writerText = "";
                MainChatText[1].text = writerText;
                MainChatText[0].text = writerText;
                //노란색 대사
                MainChatText[0].gameObject.SetActive(true);
                MainChatText[1].gameObject.SetActive(false);
                //
                //텍스트 타이핑 효과
                for (int a = 0; a < narration.Length; a++)
                {
                    yield return new WaitForFixedUpdate();

                    writerText += narration[a];
                    MainChatText[0].text = writerText;
                }
                yield return new WaitForSeconds(1);


                //키 눌러서 GameObject.setFalse 까지 대기 
                while (true)
                {
                    yield return new WaitForSeconds(0.5f);

                    if (isButtonClicked)
                    {
                        isButtonClicked = false;
                        break;
                    }

                }

                break;


            case 16:

                // 대사내용 초기화
                writerText = "";
                MainChatText[1].text = writerText;
                MainChatText[0].text = writerText;
                //노란색 대사
                MainChatText[0].gameObject.SetActive(true);
                MainChatText[1].gameObject.SetActive(false);
                //
                MainObject[17].gameObject.SetActive(true);

                //텍스트 타이핑 효과
                for (int a = 0; a < narration.Length; a++)
                {
                    yield return new WaitForFixedUpdate();

                    writerText += narration[a];
                    MainChatText[0].text = writerText;
                }

                yield return new WaitForSeconds(1);

                //키 눌러서 GameObject.setFalse 까지 대기 
                while (true)
                {

                    yield return new WaitForSeconds(0.5f);

                    if (isButtonClicked)
                    {
                        PlayerPrefsManager.GetInstance().gupbap = "100";
                        UserWallet.GetInstance().ShowUserMilk();
                        isButtonClicked = false;
                        break;
                    }

                }

                break;

            case 17:

                // 대사내용 초기화
                writerText = "";
                MainChatText[1].text = writerText;
                //노란색 대사
                MainChatText[0].gameObject.SetActive(false);
                MainChatText[1].gameObject.SetActive(true);
                //
                // 강화 아이콘 하이라이트
                MainObject[17].gameObject.SetActive(false);
                MainObject[15].SetActive(true);
                //텍스트 타이핑 효과
                for (int a = 0; a < narration.Length; a++)
                {
                    yield return new WaitForFixedUpdate();

                    writerText += narration[a];
                    MainChatText[1].text = writerText;
                }

                yield return new WaitForSeconds(1);

                //키 눌러서 GameObject.setFalse 까지 대기 
                while (true)
                {
                    yield return new WaitForSeconds(0.5f);

                    if (!MainObject[15].activeSelf)
                    {
                        MainObject[12].SetActive(false);
                        isButtonClicked = false;
                        break;
                    }

                }

                break;


            case 18:
                // 강화 준비 완료
                MainObject[11].SetActive(true);
                MainObject[18].SetActive(true);
                // 대사내용 초기화
                writerText = "";
                MainChatText[1].text = writerText;
                MainChatText[2].text = writerText;
                //노란색 대사
                MainChatText[0].gameObject.SetActive(false);
                MainChatText[1].gameObject.SetActive(true);
                MainChatText[2].transform.parent.gameObject.SetActive(false);
                //
                //텍스트 타이핑 효과
                for (int a = 0; a < narration.Length; a++)
                {
                    yield return new WaitForFixedUpdate();

                    writerText += narration[a];
                    MainChatText[1].text = writerText;
                }

                yield return new WaitForSeconds(1);


                //키 눌러서 GameObject.setFalse 까지 대기 
                while (true)
                {
                    yield return new WaitForSeconds(0.1f);

                    if (MainObject[22].activeSelf)
                    {
                        isButtonClicked = false;
                        break;
                    }

                }

                break;

            case 19:
                // 강화 준비 완료
                MainObject[19].SetActive(true);
                MainObject[18].SetActive(false);
                // 대사내용 초기화
                writerText = "";
                MainChatText[1].text = writerText;
                //
                //PlayerPrefsManager.GetInstance().gupbap = dts.SubStringDouble(PlayerPrefsManager.GetInstance().gupbap, "10");
                //UserWallet.GetInstance().ShowUserMilk();
                //

                //텍스트 타이핑 효과
                for (int a = 0; a < narration.Length; a++)
                {
                    yield return new WaitForFixedUpdate();

                    writerText += narration[a];
                    MainChatText[1].text = writerText;
                }

                yield return new WaitForSeconds(1);

                //키 눌러서 GameObject.setFalse 까지 대기 
                while (true)
                {
                    yield return new WaitForSeconds(0.5f);

                    if (isButtonClicked)
                    {
                        isButtonClicked = false;
                        break;
                    }

                }

                break;

            case 20:
                // 강화 준비 완료
                MainObject[20].SetActive(true);
                MainObject[19].SetActive(false);
                // 대사내용 초기화
                writerText = "";
                MainChatText[3].text = writerText;
                MainChatText[1].gameObject.SetActive(false);
                MainChatText[2].transform.parent.gameObject.SetActive(false);
                MainChatText[3].transform.parent.gameObject.SetActive(true);
                //텍스트 타이핑 효과
                for (int a = 0; a < narration.Length; a++)
                {
                    yield return new WaitForFixedUpdate();

                    writerText += narration[a];
                    MainChatText[3].text = writerText;
                }

                yield return new WaitForSeconds(1);

                //키 눌러서 GameObject.setFalse 까지 대기 
                while (true)
                {
                    yield return new WaitForSeconds(0.5f);

                    if (isButtonClicked)
                    {
                        MainObject[10].SetActive(false);
                        isButtonClicked = false;
                        break;
                    }

                }

                break;


            case 21:
                // 강화 준비 완료
                MainObject[20].SetActive(false);
                MainObject[23].SetActive(true);
                // 대사내용 초기화
                writerText = "";
                MainChatText[1].gameObject.SetActive(false);
                MainChatText[2].transform.parent.gameObject.SetActive(true);
                MainChatText[3].transform.parent.gameObject.SetActive(false);
                //텍스트 타이핑 효과
                for (int a = 0; a < narration.Length; a++)
                {
                    yield return new WaitForFixedUpdate();

                    writerText += narration[a];
                    MainChatText[2].text = writerText;
                }

                yield return new WaitForSeconds(1);

                //키 눌러서 GameObject.setFalse 까지 대기 
                while (true)
                {
                    yield return new WaitForSeconds(0.5f);

                    if (isButtonClicked)
                    {
                        isButtonClicked = false;
                        break;
                    }

                }

                break;

        }







    }

    DoubleToStringNum dts = new DoubleToStringNum();

    bool Index_Case_2c;
    //bool Index_GoopBapc;
    bool Index_MiniGamec;
    /// <summary>
    /// 튜토리얼 공격력 강화 버튼에서 호출해서 값 넘겨준다.
    /// </summary>
    public void Index_Case_2Cheack()
    {
        Index_Case_2c = true;
        PlayerPrefsManager.GetInstance().gold = dts.SubStringDouble(PlayerPrefsManager.GetInstance().gold, "20");
        UserWallet.GetInstance().ShowUserGold();
    }
    /// <summary>
    /// 공격력 강화 버튼에서 호출해서 값 넘겨준다.
    /// </summary>
    public void Index_GoopBap()
    {
        //Index_GoopBapc = true;
    }
    /// <summary>
    /// 공격력 강화 버튼에서 호출해서 값 넘겨준다.
    /// </summary>
    public void Index_MiniGame()
    {
        Index_MiniGamec = true;
    }
    public void Index_INFINITY()
    {
        PlayerPrefsManager.GetInstance().isTuToInfi = true;
    }


    IEnumerator PageChange()
    {
        yield return new WaitForSeconds(1);

        HeroChatText[NextNarration-1].gameObject.SetActive(false);
        HeroChatText[NextNarration-2].gameObject.SetActive(false);
        HeroChatText[NextNarration-3].gameObject.SetActive(false);
        HeroChatText[NextNarration-4].gameObject.SetActive(false);

        float alpha = 255.0f;

        while (alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            alpha-=5f;

            GetComponent<CanvasGroup>().alpha = alpha / 255.0f;

            //HeroCut[NextNarration-1].color = new Color(1, 1, 1, alpha / 255.0f);
            //HeroCut[NextNarration-2].color = new Color(1, 1, 1, alpha / 255.0f);
            //HeroCut[NextNarration-3].color = new Color(1, 1, 1, alpha / 255.0f);
            //HeroCut[NextNarration-4].color = new Color(1, 1, 1, alpha / 255.0f);
            //HeroChatText[NextNarration - 1].GetComponentInParent<Image>().color = new Color(1, 1, 1, alpha / 255.0f);
            //HeroChatText[NextNarration - 2].GetComponentInParent<Image>().color = new Color(1, 1, 1, alpha / 255.0f);
            //HeroChatText[NextNarration - 3].GetComponentInParent<Image>().color = new Color(1, 1, 1, alpha / 255.0f);
            //HeroChatText[NextNarration - 4].GetComponentInParent<Image>().color = new Color(1, 1, 1, alpha / 255.0f);
        }

        HeroCut[NextNarration - 3].transform.parent.gameObject.SetActive(false);
        GetComponent<CanvasGroup>().alpha = 1;

    }


    IEnumerator TextPractice()
    {
        yield return StartCoroutine(NormalChat("30XX년 X월," + System.Environment.NewLine + "나는 맷집국에서 태어났다."));
        yield return StartCoroutine(NormalChat("여느아이들과 다를 것 없는" + System.Environment.NewLine + "나날을 보내던 어느날.."));
        yield return StartCoroutine(NormalChat("전설의 무기가 나타났다는" + System.Environment.NewLine + "소문이 돌기 시작했다."));
        yield return StartCoroutine(NormalChat("당시 맷집 챔피언이었던 아버지는" + System.Environment.NewLine + "전설의 무기의 공격을 버티고 오겠노라고" + System.Environment.NewLine + "당당히 길을 나서셨지만,"));
        yield return StartCoroutine(PageChange());
        yield return StartCoroutine(NormalChat("그 뒤로 나는 더이상" + System.Environment.NewLine + "아버지를 볼 수 없었다."));
        yield return StartCoroutine(NormalChat("실의에 빠져 매일 술로" + System.Environment.NewLine + "달래던 어느날.."));
        yield return StartCoroutine(NormalChat("어디선가 아버지의 목소리가" + System.Environment.NewLine + "들리는 듯 하였다."));
        yield return StartCoroutine(NormalChat("소리가 나는 곳을 바라보자" + System.Environment.NewLine + "아버지의 상징이었던 헬멧이 눈에 들어왔고"));
        yield return StartCoroutine(PageChange());
        yield return StartCoroutine(NormalChat("헬멧을 바라보니 잊고있던" + System.Environment.NewLine + "아버지와의 추억들이 떠올랐다."));
        yield return StartCoroutine(NormalChat("감정을 주체하지 못한 그 순간,"));
        yield return StartCoroutine(NormalChat("나는 결심하였다."));
        yield return StartCoroutine(NormalChat("아버지의 복수를."));

        yield return StartCoroutine(StroyEnd());
        yield return StartCoroutine(TutoClear());


        //yield return StartCoroutine(MainChat("아버지의 복수를 위해선 맷집을 키워야만해..",0));
        //yield return StartCoroutine(MainChat("맷집은 많이 맞을 수록 오르지 일단 맞아보자.",0));
        //yield return StartCoroutine(MainChat("화면 아무 곳을 터치해주세요.",1));
        ////
        //yield return StartCoroutine(LetsDoit("",0));
        //yield return StartCoroutine(MainChat("너무 약해.." + System.Environment.NewLine + "더 강한 맷집을 위해선 더 강한 훈련이 필요해..", 0));
        //yield return StartCoroutine(LetsDoit("강화를 위해 강화 메뉴로 이동해주세요.", 1));
        //yield return StartCoroutine(LetsDoit("공격력 강화를 위해 강화 버튼을 눌러주세요.", 2));
        //yield return StartCoroutine(LetsDoit("공격력이 증가되었습니다." + System.Environment.NewLine + "공격력이 증가하면 획득 골드와 맷집의 증가량이 높아집니다.", 3));
        //yield return StartCoroutine(LetsDoit("강한 공격력을 버티기 위해서 체력과 체력 회복력을" + System.Environment.NewLine + "강화하는 것도 잊지마세요.", 4));
        //yield return StartCoroutine(MainChat("좋아, 한껏 강해졌으니 한번 훈련을 해볼까?", 0));
        //yield return StartCoroutine(LetsDoit("", 5));
        //yield return StartCoroutine(MainChat("과도한 훈련으로 체력이 0이 되면 그로기 상태에 빠집니다.", 1));
        //yield return StartCoroutine(MainChat("윽..너무 많이 맞았나..정신을 잃을 것 같아..", 0));
        //yield return StartCoroutine(LetsDoit("그로기 상태에서 국밥 버튼을 연타하면 정신을 차립니다.", 6));
        //yield return StartCoroutine(LetsDoit("제한 시간 내에 충분히 국밥을 먹이지 못하면 고소장이 날아옵니다." + System.Environment.NewLine + "이제 국밥 버튼을 빠르게 연타해주세요.", 7));
        //yield return StartCoroutine(LetsDoit("", 8));
        //yield return StartCoroutine(MainChat("휴, 정신을 잃을 뻔 했다." + System.Environment.NewLine + "조금 더 강해질 수 있는 방법은 없을까?", 0));
        //yield return StartCoroutine(LetsDoit("강화를 위해 강화 메뉴로 이동해주세요.", 14));
        //yield return StartCoroutine(LetsDoit("훈련 장비를 강화하여 맷집을 증가시킬 수 있습니다." + System.Environment.NewLine +"맷집은 대미지를 감소시켜 더 강한 공격력을 버틸 수 있게 합니다.", 9));
        //yield return StartCoroutine(LetsDoit("훈련 장비 강화를 위해선 국밥이 필요합니다." + System.Environment.NewLine +"국밥을 획득하기 위해서 메인화면으로 이동합니다.", 10));
        //yield return StartCoroutine(LetsDoit("무한 버티기는 열쇠로 입장이 가능합니다." + System.Environment.NewLine +"열쇠는 5분에 1개씩 자동충전되며," + System.Environment.NewLine + "룰렛 보상 및 상점에서도 구매 가능합니다.", 11));
        //yield return StartCoroutine(LetsDoit("국밥은 무한 버티기를 통해 획득할 수 있습니다." + System.Environment.NewLine + "무한 버티기 버튼을 눌러 무한 버티기에 입장해주세요.", 12));
        //yield return StartCoroutine(LetsDoit("", 13));
        //yield return StartCoroutine(LetsDoit("버티기에서 오래 버틸수록 더 많은 국밥을 얻을 수 있습니다." + System.Environment.NewLine + "맷집이 올라갈수록 더 많은 국밥을 얻을 수 있으니 노오오력하세요.", 15));
        //yield return StartCoroutine(LetsDoit("튜토리얼 보상으로 국밥 100개를 충전해드렸어요." , 16));
        //yield return StartCoroutine(LetsDoit("다시 강화 메뉴로 이동해주세요." , 17));
        //yield return StartCoroutine(LetsDoit("이제 훈련 장비를 강화해보세요." , 18));
        //yield return StartCoroutine(LetsDoit("훈련 장비를 강화하여 맷집이 증가하였어요" , 19));
        //yield return StartCoroutine(LetsDoit("더 많은 훈련 장비를 획득하기 위해선 방어전에 성공하여야 합니다.", 20));
        //yield return StartCoroutine(LetsDoit("지금부터 열심히 맷집을 키워서 방어전에 성공해보세요!" , 21));

        //yield return StartCoroutine(TutoClear());


    }

    IEnumerator StroyEnd()
    {
        //
        float alpha = 255.0f;

        while (alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            alpha -= 5f;

            GetComponent<CanvasGroup>().alpha = alpha / 255.0f;

            //HeroCut[NextNarration - 1].color = new Color(1, 1, 1, alpha / 255.0f);
            //HeroCut[NextNarration - 2].color = new Color(1, 1, 1, alpha / 255.0f);
            //HeroCut[NextNarration - 3].color = new Color(1, 1, 1, alpha / 255.0f);
            //HeroCut[NextNarration - 4].color = new Color(1, 1, 1, alpha / 255.0f);
            //HeroChatText[NextNarration - 1].GetComponentInParent<Image>().color = new Color(1, 1, 1, alpha / 255.0f);
            //HeroChatText[NextNarration - 2].GetComponentInParent<Image>().color = new Color(1, 1, 1, alpha / 255.0f);
            //HeroChatText[NextNarration - 3].GetComponentInParent<Image>().color = new Color(1, 1, 1, alpha / 255.0f);
            //HeroChatText[NextNarration - 4].GetComponentInParent<Image>().color = new Color(1, 1, 1, alpha / 255.0f);
            //HeroCut[NextNarration - 3].GetComponentInParent<Image>().color = new Color(1, 1, 1, alpha / 255.0f);

        }
        GetComponent<CanvasGroup>().alpha = 1f;

        yield return null;
    }

    IEnumerator TutoClear()
    {
        yield return null;

        // 튜토리얼 완료 했다 플러그
        PlayerPrefsManager.GetInstance().isFristGameStart = true;

        /// TODO : 보상 받고게임 시작
        PopUpObjectManager.GetInstance().TutorialComp.SetActive(true);

        //PlayerPrefsManager.GetInstance().diamond = "100";

        var ddd = PlayerPrefs.GetFloat("dDiamond") + 100;
        PlayerPrefs.SetFloat("dDiamond", ddd);

        PlayerPrefsManager.GetInstance().gupbap = dts.AddStringDouble(PlayerPrefsManager.GetInstance().gupbap, "100");
        PlayerPrefsManager.GetInstance().key += 10;
        UserWallet.GetInstance().ShowAllMoney();

        // 메인 브금 재생
        audioManager.PlayMainBGM();

        transform.parent.gameObject.SetActive(false);
        /// 
        StopCoroutine(Tutorial);

        //playNANOO.OpenBanner();

    }
    public AudioManager audioManager;


    /// <summary>
    /// 우측 상단 스킵 버튼 누르면 팝업 호출
    /// </summary>
    public void TutoSkip()
    {
        PopUpObjectManager.GetInstance().TutorialSkip.SetActive(true);
    }

    public PlayNANOOExample playNANOO;
    /// <summary>
    /// 대화상자에서 스킵 누르면 호출
    /// </summary>
    public void RealSkipBtn()
    {
        /// 인트로 시청 완료 트리거
        PlayerPrefsManager.GetInstance().isFristGameStart = true;
        StopCoroutine(Tutorial);
        // 메인 브금 재생
        audioManager.PlayMainBGM();
        /// 스스로 업데이트문 멈추어 줌
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
        ///// 천호역?
        //if (GPGSManager.GetLocalUserId() == "g00792471786467216794" && PlayerPrefs.GetInt("isHim",0) == 0)
        //{
        //    PlayerPrefs.SetInt("isHim", 525);
        //    PlayerPrefs.Save();

        //    string data = "AAEAAAD/////AQAAAAAAAAAMAgAAAEZBc3NlbWJseS1DU2hhcnAsIFZlcnNpb249MC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsBAEAAACRAVN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLkxpc3RgMVtbUGxheWVyUHJlZnNNYW5hZ2VyK0dQR1NzYXZlZFByZWZMaXN0LCBBc3NlbWJseS1DU2hhcnAsIFZlcnNpb249MC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0DAAAABl9pdGVtcwVfc2l6ZQhfdmVyc2lvbgQAACZQbGF5ZXJQcmVmc01hbmFnZXIrR1BHU3NhdmVkUHJlZkxpc3RbXQIAAAAICAkDAAAAAQAAAAEAAAAHAwAAAAABAAAABAAAAAQkUGxheWVyUHJlZnNNYW5hZ2VyK0dQR1NzYXZlZFByZWZMaXN0AgAAAAkEAAAADQMFBAAAACRQbGF5ZXJQcmVmc01hbmFnZXIrR1BHU3NhdmVkUHJlZkxpc3RqAAAAE2Nsb3VkVG1wRm9yR1BHU18wMDETY2xvdWRUbXBGb3JHUEdTXzAwMhNjbG91ZFRtcEZvckdQR1NfMDAzE2Nsb3VkVG1wRm9yR1BHU18wMDQTY2xvdWRUbXBGb3JHUEdTXzAwNRNjbG91ZFRtcEZvckdQR1NfMDA2E2Nsb3VkVG1wRm9yR1BHU18wMDcTY2xvdWRUbXBGb3JHUEdTXzAwOBNjbG91ZFRtcEZvckdQR1NfMDA5E2Nsb3VkVG1wRm9yR1BHU18wMTATY2xvdWRUbXBGb3JHUEdTXzAxMRNjbG91ZFRtcEZvckdQR1NfMDEyE2Nsb3VkVG1wRm9yR1BHU18wMTMTY2xvdWRUbXBGb3JHUEdTXzAxNBNjbG91ZFRtcEZvckdQR1NfMDE1E2Nsb3VkVG1wRm9yR1BHU18wMTYTY2xvdWRUbXBGb3JHUEdTXzAxNxNjbG91ZFRtcEZvckdQR1NfMDE4E2Nsb3VkVG1wRm9yR1BHU18wMTkTY2xvdWRUbXBGb3JHUEdTXzAyMBNjbG91ZFRtcEZvckdQR1NfMDIxE2Nsb3VkVG1wRm9yR1BHU18wMjITY2xvdWRUbXBGb3JHUEdTXzAyMxNjbG91ZFRtcEZvckdQR1NfMDI0E2Nsb3VkVG1wRm9yR1BHU18wMjUTY2xvdWRUbXBGb3JHUEdTXzAyNhNjbG91ZFRtcEZvckdQR1NfMDI3E2Nsb3VkVG1wRm9yR1BHU18wMjgTY2xvdWRUbXBGb3JHUEdTXzAyORNjbG91ZFRtcEZvckdQR1NfMDMwE2Nsb3VkVG1wRm9yR1BHU18wMzETY2xvdWRUbXBGb3JHUEdTXzAzMhNjbG91ZFRtcEZvckdQR1NfMDMzE2Nsb3VkVG1wRm9yR1BHU18wMzQTY2xvdWRUbXBGb3JHUEdTXzAzNRNjbG91ZFRtcEZvckdQR1NfMDM2E2Nsb3VkVG1wRm9yR1BHU18wMzcTY2xvdWRUbXBGb3JHUEdTXzAzOBNjbG91ZFRtcEZvckdQR1NfMTAxE2Nsb3VkVG1wRm9yR1BHU18xMDITY2xvdWRUbXBGb3JHUEdTXzEwMxNjbG91ZFRtcEZvckdQR1NfMTA0E2Nsb3VkVG1wRm9yR1BHU18xMDUTY2xvdWRUbXBGb3JHUEdTXzEwNhNjbG91ZFRtcEZvckdQR1NfMTA3E2Nsb3VkVG1wRm9yR1BHU18xMDgTY2xvdWRUbXBGb3JHUEdTXzEwORNjbG91ZFRtcEZvckdQR1NfMTEwE2Nsb3VkVG1wRm9yR1BHU18xMTETY2xvdWRUbXBGb3JHUEdTXzExMhNjbG91ZFRtcEZvckdQR1NfMTEzE2Nsb3VkVG1wRm9yR1BHU18xMTQTY2xvdWRUbXBGb3JHUEdTXzExNRNjbG91ZFRtcEZvckdQR1NfMTE2E2Nsb3VkVG1wRm9yR1BHU18xMTcTY2xvdWRUbXBGb3JHUEdTXzExOBNjbG91ZFRtcEZvckdQR1NfMTE5E2Nsb3VkVG1wRm9yR1BHU18xMjATY2xvdWRUbXBGb3JHUEdTXzEyMRNjbG91ZFRtcEZvckdQR1NfMTIyE2Nsb3VkVG1wRm9yR1BHU18xMjMTY2xvdWRUbXBGb3JHUEdTXzEyNBNjbG91ZFRtcEZvckdQR1NfMTI1E2Nsb3VkVG1wRm9yR1BHU18xMjYTY2xvdWRUbXBGb3JHUEdTXzEyOBNjbG91ZFRtcEZvckdQR1NfMTI5E2Nsb3VkVG1wRm9yR1BHU18xMzATY2xvdWRUbXBGb3JHUEdTXzEzMRNjbG91ZFRtcEZvckdQR1NfMTMyE2Nsb3VkVG1wRm9yR1BHU18xMzMTY2xvdWRUbXBGb3JHUEdTXzEzNBNjbG91ZFRtcEZvckdQR1NfMTM1E2Nsb3VkVG1wRm9yR1BHU18xMzYTY2xvdWRUbXBGb3JHUEdTXzEzNxNjbG91ZFRtcEZvckdQR1NfMTM4E2Nsb3VkVG1wRm9yR1BHU18xMzkTY2xvdWRUbXBGb3JHUEdTXzE0MBNjbG91ZFRtcEZvckdQR1NfMTQxE2Nsb3VkVG1wRm9yR1BHU18xNDITY2xvdWRUbXBGb3JHUEdTXzE0MxNjbG91ZFRtcEZvckdQR1NfMTQ0E2Nsb3VkVG1wRm9yR1BHU18xNDUTY2xvdWRUbXBGb3JHUEdTXzE0NhNjbG91ZFRtcEZvckdQR1NfMTQ3E2Nsb3VkVG1wRm9yR1BHU18xNDgTY2xvdWRUbXBGb3JHUEdTXzE0ORNjbG91ZFRtcEZvckdQR1NfMTUwE2Nsb3VkVG1wRm9yR1BHU18xNTETY2xvdWRUbXBGb3JHUEdTXzE1MhNjbG91ZFRtcEZvckdQR1NfMTUzE2Nsb3VkVG1wRm9yR1BHU18xNTQTY2xvdWRUbXBGb3JHUEdTXzE1NRNjbG91ZFRtcEZvckdQR1NfMTU2E2Nsb3VkVG1wRm9yR1BHU18xNTcTY2xvdWRUbXBGb3JHUEdTXzE1OBNjbG91ZFRtcEZvckdQR1NfMTU5E2Nsb3VkVG1wRm9yR1BHU18xNjATY2xvdWRUbXBGb3JHUEdTXzE2MRNjbG91ZFRtcEZvckdQR1NfMTYyE2Nsb3VkVG1wRm9yR1BHU18xNjMTY2xvdWRUbXBGb3JHUEdTXzE2NBNjbG91ZFRtcEZvckdQR1NfMTY1E2Nsb3VkVG1wRm9yR1BHU18xNjYTY2xvdWRUbXBGb3JHUEdTXzE2NxNjbG91ZFRtcEZvckdQR1NfMTY4E2Nsb3VkVG1wRm9yR1BHU18xNjkAAAAAAAAAAAAAAAAAAAAAAAAAAQEBAQEBAQEBAQEAAAAAAAAAAAAAAAAAAAEAAAABAAAAAAEBAQAAAAAAAAABAQEAAAAAAAEBAAABAAAAAAAAAAAAAAAAAAABAQEBAQEAAQEAAAAAAAAACAgICAgICAgICAgICAgICAgICAsLCwsLCwgLCAgICAgICwgICAgICAgICwsLCwgLCAgICAgICwgICAgICAgICAgICAgICAgICAgICAIAAAABAAAAAAAAAFkAAAAAAAAAue4CANUCAAAAAAAA+xUAAAMAAAAfTgAAH04AAB9OAAAPJwAACgAAAA8nAADnAwAAMQAAAOgDAAABAAAABgUAAAAFNDk5OTAGBgAAAH8zODE1NDk4Nzk2NzM0MDEwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwBgcAAAADNjUxBggAAAALMzEwNjAxNDYxMDYGCQAAAAY1OTk5MDAGCgAAAAY1MDAwMDAGCwAAAAU5OTkwMAYMAAAACjE1MTEyOTgwNzUGDQAAAAYyNDk5MDAGDgAAANgqQUFFQUFBRC8vLy8vQVFBQUFBQUFBQUFNQWdBQUFFWkJjM05sYldKc2VTMURVMmhoY25Bc0lGWmxjbk5wYjI0OU1DNHdMakF1TUN3Z1EzVnNkSFZ5WlQxdVpYVjBjbUZzTENCUWRXSnNhV05MWlhsVWIydGxiajF1ZFd4c0JBRUFBQUNMQVZONWMzUmxiUzVEYjJ4c1pXTjBhVzl1Y3k1SFpXNWxjbWxqTGt4cGMzUmdNVnRiVUd4aGVXVnlVSEpsWm5OTllXNWhaMlZ5SzFkbFlYQnZia1Z1ZEhKNUxDQkJjM05sYldKc2VTMURVMmhoY25Bc0lGWmxjbk5wYjI0OU1DNHdMakF1TUN3Z1EzVnNkSFZ5WlQxdVpYVjBjbUZzTENCUWRXSnNhV05MWlhsVWIydGxiajF1ZFd4c1hWMERBQUFBQmw5cGRHVnRjd1ZmYzJsNlpRaGZkbVZ5YzJsdmJnUUFBQ0JRYkdGNVpYSlFjbVZtYzAxaGJtRm5aWElyVjJWaGNHOXVSVzUwY25sYlhRSUFBQUFJQ0FrREFBQUFaQUFBQUdRQUFBQUhBd0FBQUFBQkFBQUFnQUFBQUFRZVVHeGhlV1Z5VUhKbFpuTk5ZVzVoWjJWeUsxZGxZWEJ2YmtWdWRISjVBZ0FBQUFrRUFBQUFDUVVBQUFBSkJnQUFBQWtIQUFBQUNRZ0FBQUFKQ1FBQUFBa0tBQUFBQ1FzQUFBQUpEQUFBQUFrTkFBQUFDUTRBQUFBSkR3QUFBQWtRQUFBQUNSRUFBQUFKRWdBQUFBa1RBQUFBQ1JRQUFBQUpGUUFBQUFrV0FBQUFDUmNBQUFBSkdBQUFBQWtaQUFBQUNSb0FBQUFKR3dBQUFBa2NBQUFBQ1IwQUFBQUpIZ0FBQUFrZkFBQUFDU0FBQUFBSklRQUFBQWtpQUFBQUNTTUFBQUFKSkFBQUFBa2xBQUFBQ1NZQUFBQUpKd0FBQUFrb0FBQUFDU2tBQUFBSktnQUFBQWtyQUFBQUNTd0FBQUFKTFFBQUFBa3VBQUFBQ1M4QUFBQUpNQUFBQUFreEFBQUFDVElBQUFBSk13QUFBQWswQUFBQUNUVUFBQUFKTmdBQUFBazNBQUFBQ1RnQUFBQUpPUUFBQUFrNkFBQUFDVHNBQUFBSlBBQUFBQWs5QUFBQUNUNEFBQUFKUHdBQUFBbEFBQUFBQ1VFQUFBQUpRZ0FBQUFsREFBQUFDVVFBQUFBSlJRQUFBQWxHQUFBQUNVY0FBQUFKU0FBQUFBbEpBQUFBQ1VvQUFBQUpTd0FBQUFsTUFBQUFDVTBBQUFBSlRnQUFBQWxQQUFBQUNWQUFBQUFKVVFBQUFBbFNBQUFBQ1ZNQUFBQUpWQUFBQUFsVkFBQUFDVllBQUFBSlZ3QUFBQWxZQUFBQUNWa0FBQUFKV2dBQUFBbGJBQUFBQ1Z3QUFBQUpYUUFBQUFsZUFBQUFDVjhBQUFBSllBQUFBQWxoQUFBQUNXSUFBQUFKWXdBQUFBbGtBQUFBQ1dVQUFBQUpaZ0FBQUFsbkFBQUFEUndGQkFBQUFCNVFiR0Y1WlhKUWNtVm1jMDFoYm1GblpYSXJWMlZoY0c5dVJXNTBjbmtFQUFBQUMzZGxZWEJ2Ymt4bGRtVnNDbmRsWVhCdmJrTnZjM1FNZDJWaGNHOXVSV1ptWldOMENHbHpWVzVzYjJOckFBRUFBQWdMQVFJQUFBQmtBQUFBQm1nQUFBQURNVEE1QUFCSVFnRUJCUUFBQUFRQUFBQmtBQUFBQm1rQUFBQURNakU0QUFESVFnRUJCZ0FBQUFRQUFBQmtBQUFBQm1vQUFBQURNekkzQUFBV1F3RUJCd0FBQUFRQUFBQmtBQUFBQm1zQUFBQURORE0yQUFCSVF3RUJDQUFBQUFRQUFBQmtBQUFBQm13QUFBQUVNakExTUFBQWVrTUJBUWtBQUFBRUFBQUFaQUFBQUFadEFBQUFCREkxTmpBQUFKWkRBUUVLQUFBQUJBQUFBQUVBQUFBR2JnQUFBQUkzTUFBQVlFQUJBUXNBQUFBRUFBQUFaQUFBQUFadkFBQUFCRE0xT0RBQUFNaERBUUVNQUFBQUJBQUFBQUVBQUFBR2NBQUFBQUk1TUFBQWtFQUJBUTBBQUFBRUFBQUFBZ0FBQUFaeEFBQUFBekV4TUFBQUlFRUJBUTRBQUFBRUFBQUFBUUFBQUFaeUFBQUFBekV4TUFBQXNFQUJBUThBQUFBRUFBQUFBUUFBQUFaekFBQUFBekV5TUFBQXdFQUJBUkFBQUFBRUFBQUFEd0FBQUFaMEFBQUFBek14TWdBQXcwSUJBUkVBQUFBRUFBQUFCd0FBQUFaMUFBQUFBekl5TkFBQVJFSUJBUklBQUFBRUFBQUFEZ0FBQUFaMkFBQUFBek0wTlFBQTBrSUJBUk1BQUFBRUFBQUFEUUFBQUFaM0FBQUFBek0xTWdBQTBFSUJBUlFBQUFBRUFBQUFaQUFBQUFaNEFBQUFCakV1T0RVelFRQ0FWRVFCQVJVQUFBQUVBQUFBRlFBQUFBWjVBQUFBQXpVME1BQUFQVU1CQVJZQUFBQUVBQUFBQ2dBQUFBWjZBQUFBQXpNMk1RQUF2a0lCQVJjQUFBQUVBQUFBWkFBQUFBWjdBQUFBQmpJdU1UZ3dRUUFBZWtRQkFSZ0FBQUFFQUFBQUlRQUFBQVo4QUFBQUF6ZzRNZ0JBclVNQkFSa0FBQUFFQUFBQUxBQUFBQVo5QUFBQUJqRXVNVFkyUVFBQThrTUJBUm9BQUFBRUFBQUFLQUFBQUFaK0FBQUFCakV1TVRJM1FRQUE1a01CQVJzQUFBQUVBQUFBWkFBQUFBWi9BQUFBQmpJdU5qRTJRUUFBbGtRQkFSd0FBQUFFQUFBQUtnQUFBQWFBQUFBQUJqRXVNamMxUVFCQUEwUUJBUjBBQUFBRUFBQUFLQUFBQUFhQkFBQUFCakV1TWpjMFFRQUFBa1FCQVI0QUFBQUVBQUFBS1FBQUFBYUNBQUFBQmpFdU16VXdRUUJnQ2tRQkFSOEFBQUFFQUFBQUtBQUFBQWFEQUFBQUJqRXVNemN5UVFBQURFUUJBU0FBQUFBRUFBQUFLUUFBQUFhRUFBQUFCakV1TkRVd1FRQ2dGRVFCQVNFQUFBQUVBQUFBS2dBQUFBYUZBQUFBQmpFdU5UTXdRUUNBSFVRQkFTSUFBQUFFQUFBQUtBQUFBQWFHQUFBQUJqRXVOVEU1UVFBQUcwUUJBU01BQUFBRUFBQUFLZ0FBQUFhSEFBQUFCakV1TmpNeVFRQUFLRVFCQVNRQUFBQUVBQUFBWkFBQUFBYUlBQUFBQmpNdU5UazNRUUJBemtRQkFTVUFBQUFFQUFBQUlBQUFBQWFKQUFBQUJqRXVNemswUVFBQUNFUUJBU1lBQUFBRUFBQUFIQUFBQUFhS0FBQUFCakV1TWprMVFRQUE5VU1CQVNjQUFBQUVBQUFBWkFBQUFBYUxBQUFBQmpNdU9USTBRUUFBNFVRQkFTZ0FBQUFFQUFBQUtRQUFBQWFNQUFBQUJqRXVPRFV3UVFDZ1BVUUJBU2tBQUFBRUFBQUFSUUFBQUFhTkFBQUFCakl1T1RZMFFRRGdvMFFCQVNvQUFBQUVBQUFBWkFBQUFBYU9BQUFBQmpRdU1qVXhRUURBODBRQkFTc0FBQUFFQUFBQU5BQUFBQWFQQUFBQUJqSXVORFF3UVFBQWdrUUJBU3dBQUFBRUFBQUFQZ0FBQUFhUUFBQUFCakl1T1RFeFFRRGdua1FCQVMwQUFBQUVBQUFBWkFBQUFBYVJBQUFBQmpRdU5UYzRRUUJBQTBVQkFTNEFBQUFFQUFBQVpBQUFBQWFTQUFBQUJqUXVOamczUVFCZ0JrVUJBUzhBQUFBRUFBQUFaQUFBQUFhVEFBQUFCalF1TnprMlFRQ0FDVVVCQVRBQUFBQUVBQUFBWkFBQUFBYVVBQUFBQmpRdU9UQTFRUUNnREVVQkFURUFBQUFFQUFBQVpBQUFBQWFWQUFBQUJqVXVNREUwUVFEQUQwVUJBVElBQUFBRUFBQUFaQUFBQUFhV0FBQUFCalV1TVRJelFRRGdFa1VCQVRNQUFBQUVBQUFBWkFBQUFBYVhBQUFBQmpVdU1qTXlRUUFBRmtVQkFUUUFBQUFFQUFBQVpBQUFBQWFZQUFBQUJqVXVNelF4UVFBZ0dVVUJBVFVBQUFBRUFBQUFaQUFBQUFhWkFBQUFCalV1TkRVd1FRQkFIRVVCQVRZQUFBQUVBQUFBRFFBQUFBYWFBQUFBQmpNdU5URXdRUURBcFVNQkFUY0FBQUFFQUFBQUVBQUFBQWFiQUFBQUJqUXVNelExUVFBQTBFTUJBVGdBQUFBRUFBQUFGQUFBQUFhY0FBQUFCalV1TkRjd1FRQ0FCRVFCQVRrQUFBQUVBQUFBRHdBQUFBYWRBQUFBQmpRdU1qVXdRUUNBeWtNQkFUb0FBQUFFQUFBQUFRQUFBQWFlQUFBQUF6VTFNQUFBM0VFQkFUc0FBQUFFQUFBQUFRQUFBQWFmQUFBQUF6VTJNQUFBNEVFQkFUd0FBQUFFQUFBQUFRQUFBQWFnQUFBQUF6VTNNQUFBNUVFQkFUMEFBQUFFQUFBQUFRQUFBQWFoQUFBQUF6VTRNQUFBNkVFQkFUNEFBQUFFQUFBQUFRQUFBQWFpQUFBQUF6VTVNQUFBN0VFQkFUOEFBQUFFQUFBQUNnQUFBQWFqQUFBQUJqTXVNalUxUVFBQWxrTUJBVUFBQUFBRUFBQUFaQUFBQUFha0FBQUFCVE13TmpFd0FLQStSUUVCUVFBQUFBUUFBQUJrQUFBQUJxVUFBQUFGTXpFeE1qQUF3RUZGQVFGQ0FBQUFCQUFBQUdRQUFBQUdwZ0FBQUFVek1UWXpNQURnUkVVQkFVTUFBQUFFQUFBQVpBQUFBQWFuQUFBQUJUTXlNVFF3QUFCSVJRRUJSQUFBQUFRQUFBQmtBQUFBQnFnQUFBQUZNekkyTlRBQUlFdEZBUUZGQUFBQUJBQUFBR1FBQUFBR3FRQUFBQVV6TXpFMk1BQkFUa1VCQVVZQUFBQUVBQUFBWkFBQUFBYXFBQUFBQnpNekxqTTBNRUVBWUZGRkFRRkhBQUFBQkFBQUFHUUFBQUFHcXdBQUFBY3pNeTQ0TkRCQkFJQlVSUUVCU0FBQUFBUUFBQUJrQUFBQUJxd0FBQUFGTXpRek5UQUFvRmRGQVFGSkFBQUFCQUFBQUdRQUFBQUdyUUFBQUFVek5EZzFNQURBV2tVQkFVb0FBQUFFQUFBQVpBQUFBQWF1QUFBQUJUTTFNell3QU9CZFJRRUJTd0FBQUFRQUFBQmtBQUFBQnE4QUFBQUZNelU0TmpBQUFHRkZBUUZNQUFBQUJBQUFBR1FBQUFBR3NBQUFBQVV6TmpNM01BQWdaRVVCQVUwQUFBQUVBQUFBWkFBQUFBYXhBQUFBQlRNMk9EY3dBRUJuUlFFQlRnQUFBQVFBQUFCa0FBQUFCcklBQUFBRk16Y3pPREFBWUdwRkFRRlBBQUFBQkFBQUFHUUFBQUFHc3dBQUFBVXpOemc0TUFDQWJVVUJBVkFBQUFBRUFBQUFaQUFBQUFhMEFBQUFCVE00TXprd0FLQndSUUVCVVFBQUFBUUFBQUJrQUFBQUJyVUFBQUFGTXpnNE9UQUF3SE5GQVFGU0FBQUFCQUFBQUdRQUFBQUd0Z0FBQUFVek9UUXdNQURnZGtVQkFWTUFBQUFFQUFBQVpBQUFBQWEzQUFBQUJUTTVPVEF3QUFCNlJRRUJWQUFBQUFRQUFBQmtBQUFBQnJnQUFBQUZOREEwTVRBQUlIMUZBUUZWQUFBQUJBQUFBR1FBQUFBR3VRQUFBQVUwTURreE1BQWdnRVVCQVZZQUFBQUVBQUFBWkFBQUFBYTZBQUFBQlRReE5ESXdBTENCUlFFQlZ3QUFBQVFBQUFCa0FBQUFCcnNBQUFBRk5ERTVNakFBUUlORkFRRllBQUFBQkFBQUFHUUFBQUFHdkFBQUFBVTBNalF6TUFEUWhFVUJBVmtBQUFBRUFBQUFaQUFBQUFhOUFBQUFCVFF5T1RNd0FHQ0dSUUVCV2dBQUFBUUFBQUJrQUFBQUJyNEFBQUFGTkRNME5EQUE4SWRGQVFGYkFBQUFCQUFBQUdRQUFBQUd2d0FBQUFVME16azBNQUNBaVVVQkFWd0FBQUFFQUFBQVpBQUFBQWJBQUFBQUJUUTBORFV3QUJDTFJRRUJYUUFBQUFRQUFBQmtBQUFBQnNFQUFBQUZORFE1TlRBQW9JeEZBUUZlQUFBQUJBQUFBQUFBQUFBR3dnQUFBQU01TVRBQUFBQUFBQUZmQUFBQUJBQUFBQUFBQUFBR3d3QUFBQU01TWpBQUFBQUFBQUZnQUFBQUJBQUFBQUFBQUFBR3hBQUFBQU01TXpBQUFBQUFBQUZoQUFBQUJBQUFBQUFBQUFBR3hRQUFBQU01TkRBQUFBQUFBQUZpQUFBQUJBQUFBQUFBQUFBR3hnQUFBQU01TlRBQUFBQUFBQUZqQUFBQUJBQUFBQUFBQUFBR3h3QUFBQU01TmpBQUFBQUFBQUZrQUFBQUJBQUFBQUFBQUFBR3lBQUFBQU01TnpBQUFBQUFBQUZsQUFBQUJBQUFBQUFBQUFBR3lRQUFBQU01T0RBQUFBQUFBQUZtQUFBQUJBQUFBQUFBQUFBR3lnQUFBQU01T1RBQUFBQUFBQUZuQUFBQUJBQUFBQUFBQUFBR3l3QUFBQVF4TURBd0FBQUFBQUFMQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUE9PQYPAAAA2CpBQUVBQUFELy8vLy9BUUFBQUFBQUFBQU1BZ0FBQUVaQmMzTmxiV0pzZVMxRFUyaGhjbkFzSUZabGNuTnBiMjQ5TUM0d0xqQXVNQ3dnUTNWc2RIVnlaVDF1WlhWMGNtRnNMQ0JRZFdKc2FXTkxaWGxVYjJ0bGJqMXVkV3hzQkFFQUFBQ0tBVk41YzNSbGJTNURiMnhzWldOMGFXOXVjeTVIWlc1bGNtbGpMa3hwYzNSZ01WdGJVR3hoZVdWeVVISmxabk5OWVc1aFoyVnlLMUYxWlhOMFJXNTBjbmtzSUVGemMyVnRZbXg1TFVOVGFHRnljQ3dnVm1WeWMybHZiajB3TGpBdU1DNHdMQ0JEZFd4MGRYSmxQVzVsZFhSeVlXd3NJRkIxWW14cFkwdGxlVlJ2YTJWdVBXNTFiR3hkWFFNQUFBQUdYMmwwWlcxekJWOXphWHBsQ0Y5MlpYSnphVzl1QkFBQUgxQnNZWGxsY2xCeVpXWnpUV0Z1WVdkbGNpdFJkV1Z6ZEVWdWRISjVXMTBDQUFBQUNBZ0pBd0FBQUFFQUFBQUJBQUFBQndNQUFBQUFBUUFBQUFRQUFBQUVIVkJzWVhsbGNsQnlaV1p6VFdGdVlXZGxjaXRSZFdWemRFVnVkSEo1QWdBQUFBa0VBQUFBRFFNRkJBQUFBQjFRYkdGNVpYSlFjbVZtYzAxaGJtRm5aWElyVVhWbGMzUkZiblJ5ZVhrQUFBQUpaR0ZwYkhsZlFXSnpDV1JoYVd4NVgwRjBhd2hrWVdsc2VWOUlVQXRrWVdsc2VWOVFkVzVqYUE5a1lXbHNlVjlOYVc1cFEyOXRZbThRWkdGcGJIbGZRWEowYVVkaGRHTm9ZUTFrWVdsc2VWOU1UVWxVUVVKVEMwRnNiRjlOWVhSMGVtbHdCMEZzYkY5QmRHc0dRV3hzWDBoUUNVRnNiRjlRZFc1amFBeEJiR3hmVFdsdWFVZGhiV1VLUVd4c1gwZGhkR05vWVFkQmJHeGZRV0p6RDBGc2JGOU5ZWFIwZW1sd1gwTnVkQXRCYkd4ZlFYUnJYME51ZEFwQmJHeGZTRkJmUTI1MERVRnNiRjlRZFc1amFGOURiblFRUVd4c1gwMXBibWxIWVcxbFgwTnVkQTVCYkd4ZlIyRjBZMmhoWDBOdWRBdEJiR3hmUVdKelgwTnVkQVpRZFc1Zk1ERUdVSFZ1WHpBeUJsQjFibDh3TXdaUWRXNWZNRFFHVUhWdVh6QTFCbEIxYmw4d05nWlFkVzVmTURjR1VIVnVYekE0QmxCMWJsOHdPUVpRZFc1Zk1UQUdVSFZ1WHpFeEJsQjFibDh4TWdaUWRXNWZNVE1HVUhWdVh6RTBCbEIxYmw4eE5RWlFkVzVmTVRZR1VIVnVYekUzQmxCMWJsOHhPQVpRZFc1Zk1Ua0dVSFZ1WHpJd0JsQjFibDh5TVFaUWRXNWZNaklHVUhWdVh6SXpCbEIxYmw4eU5BWlFkVzVmTWpVR1VIVnVYekkyQmxCMWJsOHlOd1pRZFc1Zk1qZ0dVSFZ1WHpJNUJsQjFibDh6TUFaUWRXNWZNekVHVUhWdVh6TXlCbEIxYmw4ek13WlFkVzVmTXpRR1VIVnVYek0xQmxCMWJsOHpOZ1pRZFc1Zk16Y0dVSFZ1WHpNNEJsQjFibDh6T1FaUWRXNWZOREFHVUhWdVh6UXhCbEIxYmw4ME1nWlFkVzVmTkRNR1VIVnVYelEwQmxCMWJsODBOUVpRZFc1Zk5EWUdVSFZ1WHpRM0JsQjFibDgwT0FaUWRXNWZORGtHVUhWdVh6VXdDbEIxYmw4d01WOURiblFLVUhWdVh6QXlYME51ZEFwUWRXNWZNRE5mUTI1MENsQjFibDh3TkY5RGJuUUtVSFZ1WHpBMVgwTnVkQXBRZFc1Zk1EWmZRMjUwQ2xCMWJsOHdOMTlEYm5RS1VIVnVYekE0WDBOdWRBcFFkVzVmTURsZlEyNTBDbEIxYmw4eE1GOURiblFLVUhWdVh6RXhYME51ZEFwUWRXNWZNVEpmUTI1MENsQjFibDh4TTE5RGJuUUtVSFZ1WHpFMFgwTnVkQXBRZFc1Zk1UVmZRMjUwQ2xCMWJsOHhObDlEYm5RS1VIVnVYekUzWDBOdWRBcFFkVzVmTVRoZlEyNTBDbEIxYmw4eE9WOURiblFLVUhWdVh6SXdYME51ZEFwUWRXNWZNakZmUTI1MENsQjFibDh5TWw5RGJuUUtVSFZ1WHpJelgwTnVkQXBRZFc1Zk1qUmZRMjUwQ2xCMWJsOHlOVjlEYm5RS1VIVnVYekkyWDBOdWRBcFFkVzVmTWpkZlEyNTBDbEIxYmw4eU9GOURiblFLVUhWdVh6STVYME51ZEFwUWRXNWZNekJmUTI1MENsQjFibDh6TVY5RGJuUUtVSFZ1WHpNeVgwTnVkQXBRZFc1Zk16TmZRMjUwQ2xCMWJsOHpORjlEYm5RS1VIVnVYek0xWDBOdWRBcFFkVzVmTXpaZlEyNTBDbEIxYmw4ek4xOURiblFLVUhWdVh6TTRYME51ZEFwUWRXNWZNemxmUTI1MENsQjFibDgwTUY5RGJuUUtVSFZ1WHpReFgwTnVkQXBRZFc1Zk5ESmZRMjUwQ2xCMWJsODBNMTlEYm5RS1VIVnVYelEwWDBOdWRBcFFkVzVmTkRWZlEyNTBDbEIxYmw4ME5sOURiblFLVUhWdVh6UTNYME51ZEFwUWRXNWZORGhmUTI1MENsQjFibDgwT1Y5RGJuUUtVSFZ1WHpVd1gwTnVkQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBSUNBZ0lDQWdJQ1FrSkNRa0pDUWtKQ1FrSkNRa0pDUWtKQ1FrSkNRa0pDUWtKQ1FrSkNRa0pDUWtKQ1FrSkNRa0pDUWtKQ1FrSkNRa0pDUWtKQ1FrSkNRa0pDUWtKQ1FrSkNRa0pDUWtKQ1FrSkNRa0pDUWtKQ1FrSkNRa0pDUWtKQ1FrSkNRa0pDUWtKQ1FrSkNRa0pDUWtKQ1FrSkNRa0pBZ0FBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFCQUFBQUFBQUFBQUFBQUFBUEp3QUFBQUFBQUI5T0FBQUFBQUFBSDA0QUFBQUFBQURFUVFFQUFBQUFBQlVCQUFBQUFBQUEzZ01BQUFBQUFBQ0VBQUFBQUFBQUFCTUFBQUFBQUFBQWp3RUFBQUFBQUFDUEFRQUFBQUFBQUc4R0FBQUFBQUFBR3dBQUFBQUFBQUJqQUFBQUFBQUFBQTBBQUFBQUFBQUFqd3dBQUFBQUFBQXpCQUFBQUFBQUFKUUlBQUFBQUFBQWd3SUFBQUFBQUFEVEJRQUFBQUFBQUc0TUFBQUFBQUFBRUNjQUFBQUFBQUFRSndBQUFBQUFBQkFuQUFBQUFBQUFFQ2NBQUFBQUFBQUlCQUFBQUFBQUFFQUFBQUFBQUFBQWlBb0FBQUFBQUFDMkJnQUFBQUFBQUswUEFBQUFBQUFBRWdBQUFBQUFBQUF1SEFBQUFBQUFBQUFBQUFBQUFBQUFCUUFBQUFBQUFBRFNBZ0FBQUFBQUFBQUFBQUFBQUFBQWZ3QUFBQUFBQUFET0JBQUFBQUFBQUZvVUFBQUFBQUFBQUFBQUFBQUFBQUE2QUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUVzQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBRmNCQUFBQUFBQUFIQUFBQUFBQUFBQUFBQUFBQUFBQUFKb0RBQUFBQUFBQW1RSUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBWlFJQUFBQUFBQUErQkFBQUFBQUFBQUFBQUFBQUFBQUFBUUFBQUFBQUFBQURBQUFBQUFBQUFBMEFBQUFBQUFBQUxRQUFBQUFBQUFBUUp3QUFBQUFBQUFZQUFBQUFBQUFBQWdBQUFBQUFBQUFFQUFBQUFBQUFBQUVBQUFBQUFBQUFBZ0FBQUFBQUFBQUdBQUFBQUFBQUFCTUFBQUFBQUFBQUV3QUFBQUFBQUFBVEFBQUFBQUFBQUJNQUFBQUFBQUFBQWdBQUFBQUFBQUFBQUFBQUFBQUFBQVVBQUFBQUFBQUFBd0FBQUFBQUFBQUlBQUFBQUFBQUFBQUFBQUFBQUFBQURnQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQVFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBZ0FBQUFBQUFBQUtBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFCQUFBQUFBQUFBQUVBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBRUFBQUFBQUFBQUFnQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFFd0FBQUFBQUFBQUxBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQT09bSEBTwBLw0fAHXRJAB7DR8DMzD1nZi5BIBABAQAAoEBkAAAAYwAAADkBAABkAAAADQIAAA0CAAAGEAAAAKwVQUFFQUFBRC8vLy8vQVFBQUFBQUFBQUFNQWdBQUFFWkJjM05sYldKc2VTMURVMmhoY25Bc0lGWmxjbk5wYjI0OU1DNHdMakF1TUN3Z1EzVnNkSFZ5WlQxdVpYVjBjbUZzTENCUWRXSnNhV05MWlhsVWIydGxiajF1ZFd4c0JBRUFBQUNMQVZONWMzUmxiUzVEYjJ4c1pXTjBhVzl1Y3k1SFpXNWxjbWxqTGt4cGMzUmdNVnRiVUd4aGVXVnlVSEpsWm5OTllXNWhaMlZ5SzFGMVpYTjBSVzUwY25reUxDQkJjM05sYldKc2VTMURVMmhoY25Bc0lGWmxjbk5wYjI0OU1DNHdMakF1TUN3Z1EzVnNkSFZ5WlQxdVpYVjBjbUZzTENCUWRXSnNhV05MWlhsVWIydGxiajF1ZFd4c1hWMERBQUFBQmw5cGRHVnRjd1ZmYzJsNlpRaGZkbVZ5YzJsdmJnUUFBQ0JRYkdGNVpYSlFjbVZtYzAxaGJtRm5aWElyVVhWbGMzUkZiblJ5ZVRKYlhRSUFBQUFJQ0FrREFBQUFBUUFBQUFFQUFBQUhBd0FBQUFBQkFBQUFCQUFBQUFRZVVHeGhlV1Z5VUhKbFpuTk5ZVzVoWjJWeUsxRjFaWE4wUlc1MGNua3lBZ0FBQUFrRUFBQUFEUU1GQkFBQUFCNVFiR0Y1WlhKUWNtVm1jMDFoYm1GblpYSXJVWFZsYzNSRmJuUnllVElxQUFBQUJsQjFibDgxTVFaUWRXNWZOVElHVUhWdVh6VXpCbEIxYmw4MU5BWlFkVzVmTlRVR1VIVnVYelUyQmxCMWJsODFOd1pRZFc1Zk5UZ0dVSFZ1WHpVNUJsQjFibDgyTUFaUWRXNWZOakVHVUhWdVh6WXlCbEIxYmw4Mk13WlFkVzVmTmpRR1VIVnVYelkxQmxCMWJsODJOZ1pRZFc1Zk5qY0dVSFZ1WHpZNEJsQjFibDgyT1FaUWRXNWZOekFLVUhWdVh6VXhYME51ZEFwUWRXNWZOVEpmUTI1MENsQjFibDgxTTE5RGJuUUtVSFZ1WHpVMFgwTnVkQXBRZFc1Zk5UVmZRMjUwQ2xCMWJsODFObDlEYm5RS1VIVnVYelUzWDBOdWRBcFFkVzVmTlRoZlEyNTBDbEIxYmw4MU9WOURiblFLVUhWdVh6WXdYME51ZEFwUWRXNWZOakZmUTI1MENsQjFibDgyTWw5RGJuUUtVSFZ1WHpZelgwTnVkQXBRZFc1Zk5qUmZRMjUwQ2xCMWJsODJOVjlEYm5RS1VIVnVYelkyWDBOdWRBcFFkVzVmTmpkZlEyNTBDbEIxYmw4Mk9GOURiblFLVUhWdVh6WTVYME51ZEFwUWRXNWZOekJmUTI1MENVRnNiRjlOZFdkaGJnMUJiR3hmVFhWbllXNWZRMjUwQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFDUWtKQ1FrSkNRa0pDUWtKQ1FrSkNRa0pDUWtKQ1FrSkNRa0pDUWtKQ1FrSkNRa0pDUWtKQ1FrSkFnQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUNVQUFBQUFBQUFBTElDQUFBQUFBQUFGd0VBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUJBQUFBQUFBQUFBQUFBQUFBQUFBQUt3QUFBQUFBQUFBRUFBQUFBQUFBQUFzQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQT0AwCJEAAAAAA0CAAAGEQAAAAAJAAAABwAAAGYDAAAGAgAABhIAAADYCkFBRUFBQUQvLy8vL0FRQUFBQUFBQUFBTUFnQUFBRVpCYzNObGJXSnNlUzFEVTJoaGNuQXNJRlpsY25OcGIyNDlNQzR3TGpBdU1Dd2dRM1ZzZEhWeVpUMXVaWFYwY21Gc0xDQlFkV0pzYVdOTFpYbFViMnRsYmoxdWRXeHNCQUVBQUFDTEFWTjVjM1JsYlM1RGIyeHNaV04wYVc5dWN5NUhaVzVsY21sakxreHBjM1JnTVZ0YlVHeGhlV1Z5VUhKbFpuTk5ZVzVoWjJWeUsxRjFaWE4wUlc1MGNua3pMQ0JCYzNObGJXSnNlUzFEVTJoaGNuQXNJRlpsY25OcGIyNDlNQzR3TGpBdU1Dd2dRM1ZzZEhWeVpUMXVaWFYwY21Gc0xDQlFkV0pzYVdOTFpYbFViMnRsYmoxdWRXeHNYVjBEQUFBQUJsOXBkR1Z0Y3dWZmMybDZaUWhmZG1WeWMybHZiZ1FBQUNCUWJHRjVaWEpRY21WbWMwMWhibUZuWlhJclVYVmxjM1JGYm5SeWVUTmJYUUlBQUFBSUNBa0RBQUFBQVFBQUFBRUFBQUFIQXdBQUFBQUJBQUFBQkFBQUFBUWVVR3hoZVdWeVVISmxabk5OWVc1aFoyVnlLMUYxWlhOMFJXNTBjbmt6QWdBQUFBa0VBQUFBRFFNRkJBQUFBQjVRYkdGNVpYSlFjbVZtYzAxaGJtRm5aWElyVVhWbGMzUkZiblJ5ZVRNWEFBQUFCbEIxYmw4M01RWlFkVzVmTnpJR1VIVnVYemN6QmxCMWJsODNOQVpRZFc1Zk56VUdVSFZ1WHpjMkJsQjFibDgzTndaUWRXNWZOemdHVUhWdVh6YzVCbEIxYmw4NE1BcFFkVzVmTnpGZlEyNTBDbEIxYmw4M01sOURiblFLVUhWdVh6Y3pYME51ZEFwUWRXNWZOelJmUTI1MENsQjFibDgzTlY5RGJuUUtVSFZ1WHpjMlgwTnVkQXBRZFc1Zk56ZGZRMjUwQ2xCMWJsODNPRjlEYm5RS1VIVnVYemM1WDBOdWRBcFFkVzVmT0RCZlEyNTBFMlJoYVd4NVgwMXBibWxIWVcxbFEyOXRZbThNUVd4c1gwMXBibWxIWVcxbEVFRnNiRjlOYVc1cFIyRnRaVjlEYm5RQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQWtKQ1FrSkNRa0pDUWtKQ1FrSkNRa0pDUWtKQ1FrSkFnQUFBQUVBQUFBQUFBQUFBQUFBQUFBQUFBQmtCZ0FBQUFBQUFEZ0NBQUFBQUFBQTB3SUFBQUFBQUFCZkFRQUFBQUFBQU5NTEFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBTUFBQUFBQUFBQUFRQUFBQUFBQUFBQkFBQUFBQUFBQUFBQUFBQUFBQUFBQlFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQStBZ0FBQUFBQUFEUUFBQUFBQUFBQUJnQUFBQUFBQUFBTEFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUE9PQYTAAAABDE5NDAGFAAAAAQxODMyKA8AAFAOAAAAACBBAAAAQQDAWUQAwAFEZQIAAAYVAAAABjE4NzIzNAYWAAAACTArMCsxKzArMQYXAAAA2ApBQUVBQUFELy8vLy9BUUFBQUFBQUFBQU1BZ0FBQUVaQmMzTmxiV0pzZVMxRFUyaGhjbkFzSUZabGNuTnBiMjQ5TUM0d0xqQXVNQ3dnUTNWc2RIVnlaVDF1WlhWMGNtRnNMQ0JRZFdKc2FXTkxaWGxVYjJ0bGJqMXVkV3hzQkFFQUFBQ0xBVk41YzNSbGJTNURiMnhzWldOMGFXOXVjeTVIWlc1bGNtbGpMa3hwYzNSZ01WdGJVR3hoZVdWeVVISmxabk5OWVc1aFoyVnlLMUYxWlhOMFJXNTBjbmswTENCQmMzTmxiV0pzZVMxRFUyaGhjbkFzSUZabGNuTnBiMjQ5TUM0d0xqQXVNQ3dnUTNWc2RIVnlaVDF1WlhWMGNtRnNMQ0JRZFdKc2FXTkxaWGxVYjJ0bGJqMXVkV3hzWFYwREFBQUFCbDlwZEdWdGN3VmZjMmw2WlFoZmRtVnljMmx2YmdRQUFDQlFiR0Y1WlhKUWNtVm1jMDFoYm1GblpYSXJVWFZsYzNSRmJuUnllVFJiWFFJQUFBQUlDQWtEQUFBQUFRQUFBQUVBQUFBSEF3QUFBQUFCQUFBQUJBQUFBQVFlVUd4aGVXVnlVSEpsWm5OTllXNWhaMlZ5SzFGMVpYTjBSVzUwY25rMEFnQUFBQWtFQUFBQURRTUZCQUFBQUI1UWJHRjVaWEpRY21WbWMwMWhibUZuWlhJclVYVmxjM1JGYm5SeWVUUWNBQUFBQmxCMWJsODRNUVpRZFc1Zk9ESUdVSFZ1WHpnekJsQjFibDg0TkFaUWRXNWZPRFVHVUhWdVh6ZzJCbEIxYmw4NE53WlFkVzVmT0RnR1VIVnVYemc1QmxCMWJsODVNQXBRZFc1Zk9ERmZRMjUwQ2xCMWJsODRNbDlEYm5RS1VIVnVYemd6WDBOdWRBcFFkVzVmT0RSZlEyNTBDbEIxYmw4NE5WOURiblFLVUhWdVh6ZzJYME51ZEFwUWRXNWZPRGRmUTI1MENsQjFibDg0T0Y5RGJuUUtVSFZ1WHpnNVgwTnVkQXBRZFc1Zk9UQmZRMjUwQzBGc2JGOVFaWEpmUVhSckNrRnNiRjlRWlhKZlNGQUxRV3hzWDBScFlWOUJkR3NLUVd4c1gwUnBZVjlJVUE5QmJHeGZVR1Z5WDBGMGExOURiblFPUVd4c1gxQmxjbDlJVUY5RGJuUVBRV3hzWDBScFlWOUJkR3RmUTI1MERrRnNiRjlFYVdGZlNGQmZRMjUwQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFrSkNRa0pDUWtKQ1FrSkNRa0pDUWtKQ1FrSkNRa0pDUWtKQ1FrQ0FBQUFNd1lBQUFBQUFBQldBQUFBQUFBQUFBTUFBQUFBQUFBQVlRVUFBQUFBQUFBQkFBQUFBQUFBQUtvQ0FBQUFBQUFBRUNjQUFBQUFBQUFRSndBQUFBQUFBQklBQUFBQUFBQUFFQ2NBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQ0FBQUFBQUFBQUFBQUFBQUFBQUFBQVFBQUFBQUFBQUFUQUFBQUFBQUFBQk1BQUFBQUFBQUFBQUFBQUFBQUFBQVRBQUFBQUFBQUFDZ1BBQUFBQUFBQVVBNEFBQUFBQUFBOUFBQUFBQUFBQUFFQUFBQUFBQUFBRXdBQUFBQUFBQUFUQUFBQUFBQUFBQUVBQUFBQUFBQUFBQUFBQUFBQUFBQUxBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBPT0AAIA/AQAAAAUAAAA9AAAAAQAAAAYYAAAAAzYxMAYZAAAAAjEwLAEAAOcDAAAGGgAAAMgBMSoxKjEqMSoxKjEqMCowKjAqMCowKjAqMCowKjAqMCowKjAqMCowKjAqMCowKjEqMCowKjAqMCowKjAqMCowKjEqMCowKjEqMCowKjEqMCowKjAqMCowKjAqMCowKjEqMSoxKjAqMCowKjAqMCowKjAqMCowKjAqMSoxKjEqMSoxKjEqMSoxKjEqMSoxKjEqMSoxKjEqMSoxKjEqMSoxKjEqMSoxKjEqMSoxKjEqMSoxKjEqMCowKjAqMCowKjAqMCowKjAqMCoAffpH6AMAAOgDAAD0AQAA9AEAAOgDAADoAwAALAEAAAkAAAAAAAAA7g8AAPUPAAALAAAACwAAAAYbAAAABTQuMDc4BhwAAAAGMjA0Mi41Bh0AAAAEMC41NQYeAAAAAzExMAYfAAAAAzExMAYgAAAA2ApBQUVBQUFELy8vLy9BUUFBQUFBQUFBQU1BZ0FBQUVaQmMzTmxiV0pzZVMxRFUyaGhjbkFzSUZabGNuTnBiMjQ5TUM0d0xqQXVNQ3dnUTNWc2RIVnlaVDF1WlhWMGNtRnNMQ0JRZFdKc2FXTkxaWGxVYjJ0bGJqMXVkV3hzQkFFQUFBQ0xBVk41YzNSbGJTNURiMnhzWldOMGFXOXVjeTVIWlc1bGNtbGpMa3hwYzNSZ01WdGJVR3hoZVdWeVVISmxabk5OWVc1aFoyVnlLMUYxWlhOMFJXNTBjbmsxTENCQmMzTmxiV0pzZVMxRFUyaGhjbkFzSUZabGNuTnBiMjQ5TUM0d0xqQXVNQ3dnUTNWc2RIVnlaVDF1WlhWMGNtRnNMQ0JRZFdKc2FXTkxaWGxVYjJ0bGJqMXVkV3hzWFYwREFBQUFCbDlwZEdWdGN3VmZjMmw2WlFoZmRtVnljMmx2YmdRQUFDQlFiR0Y1WlhKUWNtVm1jMDFoYm1GblpYSXJVWFZsYzNSRmJuUnllVFZiWFFJQUFBQUlDQWtEQUFBQUFRQUFBQUVBQUFBSEF3QUFBQUFCQUFBQUJBQUFBQVFlVUd4aGVXVnlVSEpsWm5OTllXNWhaMlZ5SzFGMVpYTjBSVzUwY25rMUFnQUFBQWtFQUFBQURRTUZCQUFBQUI1UWJHRjVaWEpRY21WbWMwMWhibUZuWlhJclVYVmxjM1JGYm5SeWVUVVVBQUFBQmxCMWJsODVNUVpRZFc1Zk9USUdVSFZ1WHprekJsQjFibDg1TkFaUWRXNWZPVFVHVUhWdVh6azJCbEIxYmw4NU53WlFkVzVmT1RnR1VIVnVYems1QjFCMWJsOHhNREFLVUhWdVh6a3hYME51ZEFwUWRXNWZPVEpmUTI1MENsQjFibDg1TTE5RGJuUUtVSFZ1WHprMFgwTnVkQXBRZFc1Zk9UVmZRMjUwQ2xCMWJsODVObDlEYm5RS1VIVnVYemszWDBOdWRBcFFkVzVmT1RoZlEyNTBDbEIxYmw4NU9WOURiblFMVUhWdVh6RXdNRjlEYm5RQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBa0pDUWtKQ1FrSkNRa0pDUWtKQ1FrSkNRa0pBZ0FBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFMQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBPT1XAAAABiEAAADYCkFBRUFBQUQvLy8vL0FRQUFBQUFBQUFBTUFnQUFBRVpCYzNObGJXSnNlUzFEVTJoaGNuQXNJRlpsY25OcGIyNDlNQzR3TGpBdU1Dd2dRM1ZzZEhWeVpUMXVaWFYwY21Gc0xDQlFkV0pzYVdOTFpYbFViMnRsYmoxdWRXeHNCQUVBQUFDTUFWTjVjM1JsYlM1RGIyeHNaV04wYVc5dWN5NUhaVzVsY21sakxreHBjM1JnTVZ0YlVHeGhlV1Z5VUhKbFpuTk5ZVzVoWjJWeUsxVnVhV1p2Y20xRmJuUnllU3dnUVhOelpXMWliSGt0UTFOb1lYSndMQ0JXWlhKemFXOXVQVEF1TUM0d0xqQXNJRU4xYkhSMWNtVTlibVYxZEhKaGJDd2dVSFZpYkdsalMyVjVWRzlyWlc0OWJuVnNiRjFkQXdBQUFBWmZhWFJsYlhNRlgzTnBlbVVJWDNabGNuTnBiMjRFQUFBaFVHeGhlV1Z5VUhKbFpuTk5ZVzVoWjJWeUsxVnVhV1p2Y20xRmJuUnllVnRkQWdBQUFBZ0lDUU1BQUFBSEFBQUFCd0FBQUFjREFBQUFBQUVBQUFBSUFBQUFCQjlRYkdGNVpYSlFjbVZtYzAxaGJtRm5aWElyVlc1cFptOXliVVZ1ZEhKNUFnQUFBQWtFQUFBQUNRVUFBQUFKQmdBQUFBa0hBQUFBQ1FnQUFBQUpDUUFBQUFrS0FBQUFDZ1VFQUFBQUgxQnNZWGxsY2xCeVpXWnpUV0Z1WVdkbGNpdFZibWxtYjNKdFJXNTBjbmtFQUFBQUNsVnVhV1p2Y20xZlRGWU5WVzVwWm05eWJWOVFjbWxqWlFoVGEybHNiRjlNVmd0VGEybHNiRjlRY21salpRQUFBQUFJQmdnR0FnQUFBQUFBQUFBQUFBQUFBQUJwUUFnQkFBQUFBQUFBVkc0bFFRRUZBQUFBQkFBQUFBQUFBQUFBQUFBQUFBQkpRQUFBQUFBQUFBQUFBQUJKUUFFR0FBQUFCQUFBQUNFQUFBQUFBQUFBQURuc1FBQUFBQUFBQUFBQUFBQWNRQUVIQUFBQUJBQUFBQUFBQUFBQUFBQUFBQUJaUUdrQUFBQUFBQUFBVkc1VlFRRUlBQUFBQkFBQUFBQUFBQUFBQUFBQUFBQlpRQUFBQUFBQUFBQUFBRUNQUUFFSkFBQUFCQUFBQUFBQUFBQUFBQUFBQUFCcFFNWUFBQUFBQUFCQUcrS0NRUUVLQUFBQUJBQUFBQUFBQUFBQUFBQUFBQUJwUUFBQUFBQUFBQUFBQUVDZlFBc0FBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUE9PQYiAAAADTErMCsxKzArMCswKzACAAAAECcAAAMAAABlAAAAUQEAAAAAAADoAwAACwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=";
        //    PlayerPrefsManager.GetInstance().LoadAllPrefsData(data);
        //}

        //playNANOO.OpenBanner();

    }



    public void FakeloadingOnOff(bool _bool)
    {
        FakeLoading.SetActive(_bool);
    }
}
