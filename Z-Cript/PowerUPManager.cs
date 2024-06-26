﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUPManager : MonoBehaviour
{
    public PunchManager pm;
    public GameObject helpme;
    [Space]
    public RectTransform rt;
    [Space]
    private float rtChogi;
    public GroggyManager groggyManager;
    public ScrollRect topSCrect;
    [Header("-상단 Clickable 탭")]
    public GameObject powerUP_TAP;          // 클릭된 이미지
    public GameObject trainnig_TAP;         // 클릭된 이미지
    public GameObject artfact_TAP;          // 클릭된 이미지
    [Header("-Conent 1번 2번 3번")]
    public RectTransform power_Rect;
    public RectTransform train_Rect;
    public RectTransform artf_Rect;
    [Header("-Viewport 1번 2번 3번")]
    public RectTransform power_View;
    public RectTransform train_View;
    public RectTransform arti_View;         // 왼쪽 논 스크롤 패널
    [Header("-세로 SC ")]
    public ScrollRect verticalSCRect;

    [Header("- 깃발 전용")]
    public GameObject Flag_TAP;          // 클릭된 이미지
    public RectTransform Conent_4;
    public RectTransform Viewport_4;

    [Header("- 특별 강화전용")]
    public GameObject Special_TAP;          // 클릭된 이미지
    public RectTransform Conent_5;
    public RectTransform Viewport_5;

    [Header("- 방패 합성 전용")]
    public GameObject Fussion_TAP;          // 클릭된 이미지
    public RectTransform Conent_6;
    public RectTransform Viewport_6;

    Text[] punchSet;
    bool isInitFinish;
    ScrollRect thisSCRect;


    public void HelpmeDora()
    {
        helpme.SetActive(true);
        helpme.GetComponent<Animation>()["Roll_Incre"].speed = 1;
        helpme.GetComponent<Animation>().Play("Roll_Incre");
    }


    /// <summary>
    /// 처음 실행시펀치 갯수만큼 초기화.
    /// </summary>
    public void Init()
    {
        if (isInitFinish) return;

        // 디폴트 펀치 갯수
        int p_among = PlayerPrefsManager.GetInstance().punchAmont;

        punchSet = new Text[p_among];

        //펀치 50개 정의
        //for (int i = 0; i < p_among; i++)
        //{
        //    punchSet[i] = power_Rect.GetChild(i).GetChild(4).GetComponentInChildren<Text>();
        //}
        /// 한번만 실행하라.
        Debug.LogError("펀치 초기화 한번만 실행하라.");
        isInitFinish = true;
    }

    /// <summary>
    /// 장비 버튼 클릭시 이 메소드로 호출
    /// </summary>
    public void OpenPopUP()
    {
        if(gameObject.name == "POWERUP_POP")
        {
            Init();
        }


        /// TODO : 에니메이션 효과
        if (gameObject.activeSelf) return;
        gameObject.SetActive(true);

        /// TODO : getcomp 쓰는 거 전부 여기로 몰아 넣기
        thisSCRect = GetComponentInChildren<ScrollRect>();

        thisSCRect.horizontalNormalizedPosition = 0f; // 모든 스크롤뷰 왼쪽 정렬 시켜
        verticalSCRect.verticalNormalizedPosition = 1f; // 세로 스크롤 뷰 = 유물 재정렬

        if (rt != null)
        {
            /// rt 있는 Euip_Canvas 에서만 초기값.
            rtChogi = rt.anchoredPosition.x;
        }

        if (gameObject.name == "STATUS_POP")
        {
            Tap_Click_First(1);
            groggyManager.PowerUP_Init();
        }
        else if (gameObject.name == "Artifect_POP")
        {
            Tap_Click(3);
        }
        else
        {
            Tap_Click(1);
        }


    }

    public void ScrollEndLeft()
    {
        thisSCRect.horizontalNormalizedPosition = 0f; // 모든 스크롤뷰 왼쪽 정렬 시켜
    }

    public void ScrollEndRight()
    {
        thisSCRect.horizontalNormalizedPosition = 1f; // 모든 스크롤뷰 오른쪽 정렬 시켜
    }

    /// <summary>
    /// 캐릭터 정보에서만 상단 버튼 누르기 관리
    /// </summary>
    /// <param name="_buttonIndex">버튼 이벤트에 붙일때 인덱스 지정</param>
    public void Tap_Click(int _buttonIndex)
    {
        /// 방패 합성 오브젝트가 비어있지 않다면 스크롤 활성화.
        if (Fussion_TAP != null)
        {
            topSCrect.enabled = true;
        }

        switch (_buttonIndex)
        {
            case 1:
                powerUP_TAP.SetActive(true);
                trainnig_TAP.SetActive(false);
                artfact_TAP.SetActive(false);
                artf_Rect.gameObject.SetActive(false);

                if (Flag_TAP != null) Flag_TAP.SetActive(false);
                if (Special_TAP != null) Special_TAP.SetActive(false);
                if (Fussion_TAP != null) Fussion_TAP.SetActive(false);
                // 훈련강화 페이지 새로 고침
                if (name == "POWERUP_POP")
                {
                    groggyManager.PowerUP_Init();
                    pm.BoxInfoUpdate();
                }

                InitPowerUP();

                break;

            case 2:
                powerUP_TAP.SetActive(false);
                trainnig_TAP.SetActive(true);
                artfact_TAP.SetActive(false);
                artf_Rect.gameObject.SetActive(false);

                if (Flag_TAP != null) Flag_TAP.SetActive(false);
                if (Special_TAP != null) Special_TAP.SetActive(false);
                if (Fussion_TAP != null) Fussion_TAP.SetActive(false);


                InitTrain();
                break;


            case 3:
                powerUP_TAP.SetActive(false);
                trainnig_TAP.SetActive(false);
                artfact_TAP.SetActive(true);
                artf_Rect.gameObject.SetActive(true);

                if (Flag_TAP != null) Flag_TAP.SetActive(false);
                if (Special_TAP != null) Special_TAP.SetActive(false);
                if (Fussion_TAP != null) Fussion_TAP.SetActive(false);


                InitArti();
                break;




            case 4:
                powerUP_TAP.SetActive(false);
                trainnig_TAP.SetActive(false);
                artfact_TAP.SetActive(true);
                artf_Rect.gameObject.SetActive(true);

                if (Flag_TAP != null) Flag_TAP.SetActive(false);
                if (Special_TAP != null) Special_TAP.SetActive(false);
                if (Fussion_TAP != null) Fussion_TAP.SetActive(false);



                InitAllMission();
                break;


            case 5:
                powerUP_TAP.SetActive(false);
                trainnig_TAP.SetActive(false);
                artfact_TAP.SetActive(false);
                artf_Rect.gameObject.SetActive(false);

                if (Flag_TAP != null)
                {
                    Flag_TAP.SetActive(true);
                    Conent_4.gameObject.SetActive(true);
                }
                if (Special_TAP != null) Special_TAP.SetActive(false);
                if (Fussion_TAP != null) Fussion_TAP.SetActive(false);

                InitFlagPage();
                break;


            case 6:
                powerUP_TAP.SetActive(false);
                trainnig_TAP.SetActive(false);
                artfact_TAP.SetActive(false);
                artf_Rect.gameObject.SetActive(false);

                if (Flag_TAP != null) Flag_TAP.SetActive(false);
                if (Fussion_TAP != null) Fussion_TAP.SetActive(false);

                if (Special_TAP != null)
                {
                    Special_TAP.SetActive(true);
                    Conent_5.gameObject.SetActive(true);
                }

                InitSpecialPage();
                break;

            case 7:
                powerUP_TAP.SetActive(false);
                trainnig_TAP.SetActive(false);
                artfact_TAP.SetActive(false);
                artf_Rect.gameObject.SetActive(false);

                if (Flag_TAP != null) Flag_TAP.SetActive(false);
                if (Special_TAP != null) Special_TAP.SetActive(false);

                if (Fussion_TAP != null)
                {
                    Fussion_TAP.SetActive(true);
                    Conent_6.gameObject.SetActive(true);
                }

                InitFussionPage();
                break;


        }
    }

    /// <summary>
    /// 캐릭터 정보에서만 쓰는거
    /// </summary>
    /// <param name="_buttonIndex"></param>
    public void Tap_Click_First(int _buttonIndex)
    {
        switch (_buttonIndex)
        {
            case 1:
                powerUP_TAP.SetActive(true);
                trainnig_TAP.SetActive(false);
                artfact_TAP.SetActive(false);
                artf_Rect.gameObject.SetActive(true);

                if (Flag_TAP != null) Flag_TAP.SetActive(false);
                //if (Special_TAP != null) Special_TAP.SetActive(false);
                InitArti();

                break;

            case 2:
                powerUP_TAP.SetActive(false);
                trainnig_TAP.SetActive(true);
                artfact_TAP.SetActive(false);
                artf_Rect.gameObject.SetActive(false);

                if (Flag_TAP != null) Flag_TAP.SetActive(false);
                //if (Special_TAP != null) Special_TAP.SetActive(false);

                InitTrain();
                break;

            case 3:
                powerUP_TAP.SetActive(false);
                trainnig_TAP.SetActive(false);
                artfact_TAP.SetActive(true);
                artf_Rect.gameObject.SetActive(false);

                if (Flag_TAP != null) Flag_TAP.SetActive(false);
                //if (Special_TAP != null) Special_TAP.SetActive(false);
                InitPowerUP();

                break;

            case 5:
                powerUP_TAP.SetActive(false);
                trainnig_TAP.SetActive(false);
                artfact_TAP.SetActive(false);
                artf_Rect.gameObject.SetActive(false);

                if (Flag_TAP != null)
                {
                    Flag_TAP.SetActive(true);
                    Conent_4.gameObject.SetActive(true);
                }
                if (Special_TAP != null) Special_TAP.SetActive(false);

                InitFlagPage();
                break;
        }
    }



    /// <summary>
    /// 파워업 누를때
    /// </summary>
    void InitPowerUP()
    {
        // 컨텐츠 오브젝트 활성화
        power_View.gameObject.SetActive(true);
        train_View.gameObject.SetActive(false);
        arti_View.gameObject.SetActive(false);
        if (Viewport_4 != null) Viewport_4.gameObject.SetActive(false);
        if (Viewport_5 != null) Viewport_5.gameObject.SetActive(false);
        if (Viewport_6 != null) Viewport_6.gameObject.SetActive(false);

        //스크롤 뷰 교체
        thisSCRect.content = power_Rect;
        thisSCRect.viewport = power_View;
        thisSCRect.horizontalNormalizedPosition = 0f; // 모든 스크롤뷰 왼쪽 정렬 시켜

        if (name == "POWERUP_POP")
        {
            float pp = Mathf.Floor(PlayerPrefsManager.GetInstance().PunchIndex / 4f);
            rt.anchoredPosition = new Vector2(rtChogi -(1650 *pp), 0);
        }
    }
    void InitTrain()
    {
        // 컨텐츠 오브젝트 활성화
        power_View.gameObject.SetActive(false);
        train_View.gameObject.SetActive(true);
        arti_View.gameObject.SetActive(false);
        if (Viewport_4 != null) Viewport_4.gameObject.SetActive(false);
        if (Viewport_5 != null) Viewport_5.gameObject.SetActive(false);
        if (Viewport_6 != null) Viewport_6.gameObject.SetActive(false);

        //스크롤 뷰 교체
        thisSCRect.content = train_Rect;
        thisSCRect.viewport = train_View;
        thisSCRect.horizontalNormalizedPosition = 0f; // 모든 스크롤뷰 왼쪽 정렬 시켜
    }
    void InitArti()
    {
        // 컨텐츠 오브젝트 활성화
        power_View.gameObject.SetActive(false);
        train_View.gameObject.SetActive(false);
        arti_View.gameObject.SetActive(true);
        if (Viewport_4 != null) Viewport_4.gameObject.SetActive(false);
        if (Viewport_5 != null) Viewport_5.gameObject.SetActive(false);
        if (Viewport_6 != null) Viewport_6.gameObject.SetActive(false);

        //스크롤 뷰 교체
        thisSCRect.content = train_Rect; /// train_Rect 가 맞음.
        thisSCRect.viewport = arti_View;
        thisSCRect.horizontalNormalizedPosition = 0f; // 모든 스크롤뷰 왼쪽 정렬 시켜
    }
    void InitAllMission()
    {
        // 컨텐츠 오브젝트 활성화
        power_View.gameObject.SetActive(false);
        train_View.gameObject.SetActive(false);
        arti_View.gameObject.SetActive(true);
        if (Viewport_4 != null) Viewport_4.gameObject.SetActive(false);
        if (Viewport_5 != null) Viewport_5.gameObject.SetActive(false);
        if (Viewport_6 != null) Viewport_6.gameObject.SetActive(false);

        //스크롤 뷰 교체
        thisSCRect.content = artf_Rect; /// 미션페이지에서 씀
        thisSCRect.viewport = arti_View;
        thisSCRect.horizontalNormalizedPosition = 0f; // 모든 스크롤뷰 왼쪽 정렬 시켜
    }
    void InitFlagPage()
    {
        // 컨텐츠 오브젝트 활성화
        power_View.gameObject.SetActive(false);
        train_View.gameObject.SetActive(false);
        arti_View.gameObject.SetActive(false);

        if(Viewport_4 != null) Viewport_4.gameObject.SetActive(true);
        if (Viewport_5 != null) Viewport_5.gameObject.SetActive(false);
        if (Viewport_6 != null) Viewport_6.gameObject.SetActive(false);

        //스크롤 뷰 교체
        thisSCRect.content = Conent_4;
        thisSCRect.viewport = Viewport_4;
        thisSCRect.horizontalNormalizedPosition = 0f; // 모든 스크롤뷰 왼쪽 정렬 시켜
    }
    void InitSpecialPage()
    {
        // 컨텐츠 오브젝트 활성화
        power_View.gameObject.SetActive(false);
        train_View.gameObject.SetActive(false);
        arti_View.gameObject.SetActive(false);

        if (Viewport_4 != null) Viewport_4.gameObject.SetActive(false);
        if (Viewport_5 != null) Viewport_5.gameObject.SetActive(true);
        if (Viewport_6 != null) Viewport_6.gameObject.SetActive(false);
        //스크롤 뷰 교체
        thisSCRect.content = Conent_5;
        thisSCRect.viewport = Viewport_5;
        thisSCRect.horizontalNormalizedPosition = 0f; // 모든 스크롤뷰 왼쪽 정렬 시켜
    }
    void InitFussionPage()
    {
        // 컨텐츠 오브젝트 활성화
        power_View.gameObject.SetActive(false);
        train_View.gameObject.SetActive(false);
        arti_View.gameObject.SetActive(false);

        if (Viewport_4 != null) Viewport_4.gameObject.SetActive(false);
        if (Viewport_5 != null) Viewport_5.gameObject.SetActive(false);
        if (Viewport_6 != null) Viewport_6.gameObject.SetActive(true);
        //스크롤 뷰 교체
        thisSCRect.content = Conent_6;
        thisSCRect.viewport = Viewport_6;
        ///thisSCRect.horizontalNormalizedPosition = 0f; // 모든 스크롤뷰 왼쪽 정렬 시켜

        /// 방패 합성 오브젝트가 비어있지 않다면 스크롤 X 
        if (Fussion_TAP != null)
        {
            topSCrect.enabled = false;
        }
    }

    public void SetThisPunchPrice(int _Index)
    {

        punchSet[_Index].text = null;
    }












}
