using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MINIgameManager : MonoBehaviour
{
    public TutorialMissionManager tmm;
    public GameObject Btn_Exit;
    [Space]
    public Booster_KEY booster_KEY;
    [Space]
    bool isAnimPlaying = false;
    public GotoMINIgame gotoMINIgame;
    [Header("- 버튼 번쩍이")]
    public GameObject L_Flash;
    public GameObject R_Flash;
    public GameObject AnimFlash;

    [Header("- 프리팹")]
    public Transform F_BOX;
    public Transform L_BOX;
    public Transform L_NONE_BOX;
    public Transform R_BOX;
    public Transform R_NONE_BOX;

    [Header("- 센터 표기 (히트 해야할 이미지)")]
    public GameObject CenterLeftBox;
    public GameObject CenterRightBox;
    public GameObject CenterCenterBox;

    [Header("-부모 오브젝트 스크롤뷰")]
    public ScrollRect R_scrollRect; 
    public ScrollRect L_scrollRect;

    private RectTransform R_Content;
    private RectTransform L_Content;

    private Coroutine moveRoutine; // 코루틴 시작/ 정지 제어용

    private List<int> CenterImgIndx;

    private void Awake()
    {
        CenterImgIndx = new List<int>();

        R_Content = R_scrollRect.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        L_Content = L_scrollRect.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
    }


    /// <summary>
    /// 게임 시작시 양쪽에 8개씩 상자있어야함
    /// 1. 센터 박스는 활성화된 박스로
    /// </summary>
    void InitBox()
    {
        CenterLeftBox.transform.GetChild(0).gameObject.SetActive(false);
        CenterRightBox.transform.GetChild(0).gameObject.SetActive(false);
        CenterCenterBox.transform.GetChild(0).gameObject.SetActive(false);

        CenterLeftBox.SetActive(false);
        CenterRightBox.SetActive(false);
        CenterCenterBox.SetActive(false);

        CenterImgIndx.Clear();
        isAnimPlaying = false;

        var max = R_Content.childCount;
        for (int i = 0; i < max; i++)
        {
            Lean.Pool.LeanPool.Despawn(R_Content.GetChild(0).gameObject);
            Lean.Pool.LeanPool.Despawn(L_Content.GetChild(0).gameObject);
        }

        Gen_L_Box();
        CenterLeftBox.SetActive(true);

        // 랜덤시드 부여하기
        float temp = Time.time * 100f;
        UnityEngine.Random.InitState((int)temp);
        for (int i = 0; i < 8; i++)
        {
            GenGenBox();
        }


        // 스크롤뷰 초기화
        R_scrollRect.horizontalNormalizedPosition = 0f;
        L_scrollRect.horizontalNormalizedPosition = 1f;
    }

    /// <summary>
    /// 별 모양 박스로 바꿔줌
    /// </summary>
    void InitFeverBox()
    {
        CenterImgIndx.Clear();
        CenterLeftBox.SetActive(false);
        CenterRightBox.SetActive(false);

        CenterCenterBox.SetActive(true);
        for (int i = 0; i < R_Content.childCount; i++)
        {
            Lean.Pool.LeanPool.Despawn(R_Content.GetChild(0).gameObject);
            Lean.Pool.LeanPool.Despawn(L_Content.GetChild(0).gameObject);
            Gen_F_Box();
        }

        // 스크롤뷰 초기화
        R_scrollRect.horizontalNormalizedPosition = 0f;
        L_scrollRect.horizontalNormalizedPosition = 1f;
    }


    /// <summary>
    /// 둘 중 하나 리젠
    /// </summary>
    void GenGenBox()
    {
        float BanBan = UnityEngine.Random.Range(0f, 100f);

        if (BanBan < 50f)
        {
            Gen_L_Box();
        }
        else
        {
            Gen_R_Box();
        }
    }


    bool F_switch;
    /// <summary>
    /// 피버 활성화 박스 
    /// </summary>
    void Gen_F_Box()
    {
        if (F_switch)
        {
            CenterImgIndx.Add(0);
        }
        else
        {
            CenterImgIndx.Add(1);
        }
        F_switch = !F_switch;
        ////프리팹에서 박스 생성
        Transform initBox = Lean.Pool.LeanPool.Spawn(F_BOX);
        initBox.SetParent(L_Content); // 스크롤뷰 안쪽에 생성.
        initBox.localPosition = Vector3.zero; // 뒤틀리는거 방지
        initBox.localScale = new Vector3(1, 1, 1);
        //
        ////반대쪽엔 회색 박스
        Transform initGrayBox = Lean.Pool.LeanPool.Spawn(F_BOX);
        initGrayBox.SetParent(R_Content); // 스크롤뷰 안쪽에 생성.
        initGrayBox.localPosition = Vector3.zero; // 뒤틀리는거 방지
        initGrayBox.localScale = new Vector3(1, 1, 1);
    }

    /// <summary>
    /// 왼쪽 활성화 박스 
    /// </summary>
    void Gen_L_Box()
    {
        CenterImgIndx.Add(0);
        ////프리팹에서 박스 생성
        Transform initBox = Lean.Pool.LeanPool.Spawn(L_BOX);
        initBox.SetParent(L_Content); // 스크롤뷰 안쪽에 생성.
        initBox.localPosition = Vector3.zero; // 뒤틀리는거 방지
        initBox.localScale = new Vector3(1, 1, 1);
        //
        ////반대쪽엔 회색 박스
        Transform initGrayBox = Lean.Pool.LeanPool.Spawn(R_NONE_BOX);
        initGrayBox.SetParent(R_Content); // 스크롤뷰 안쪽에 생성.
        initGrayBox.localPosition = Vector3.zero; // 뒤틀리는거 방지
        initGrayBox.localScale = new Vector3(1, 1, 1);
    }

    /// <summary>
    /// 오른쪽 활성화 박스 
    /// </summary>
    void Gen_R_Box()
    {
        CenterImgIndx.Add(1);

        ////프리팹에서 박스 생성
        Transform initBox = Lean.Pool.LeanPool.Spawn(R_BOX);
        initBox.SetParent(R_Content); // 스크롤뷰 안쪽에 생성.
        initBox.localPosition = Vector3.zero; // 뒤틀리는거 방지
        initBox.localScale = new Vector3(1, 1, 1);
        //
        ////반대쪽엔 회색 박스
        Transform initGrayBox = Lean.Pool.LeanPool.Spawn(L_NONE_BOX);
        initGrayBox.SetParent(L_Content); // 스크롤뷰 안쪽에 생성.
        initGrayBox.localPosition = Vector3.zero; // 뒤틀리는거 방지
        initGrayBox.localScale = new Vector3(1, 1, 1);
    }



    [Header("- 미니게임 321 카운터 패널")]
    public GameObject PreparedPanel;
    public Text CountText;

    [Header("- 시간 타이머 / 피버")]
    public Image FeverFillamount;
    public Image TimerFillamount;
    public Text ComboText;

    /// <summary>
    /// 카메라 전환에서 호출
    /// 3.2.1. 해주고 그다음
    /// 게임 스타트 
    /// </summary>
    public void InitMiniGame()
    {
        Btn_Exit.SetActive(true);

        isGameEnding = false;

        isRightBtnClicked = false;
        isLeftBtnClicked = false;

        PlayerPrefsManager.GetInstance().key--;
        tmm.ExUpdateMission(75);


        booster_KEY.KeyTimerStart();

        FlashEnd();
        //미니 게임 남은 시간 표시
        TimerFillamount.fillAmount = 1;
        FeverFillamount.fillAmount = 0;
        Combo = 0;
        ComboText.text = Combo.ToString();
        feverCnt = 0;
        isStart = false;

        InitBox();

        EattingMotion.SetActive(false);

        // 카운트 다운 온!
        PreparedPanel.SetActive(true);
        // 3 2 1카운트 다운
        StartCoroutine(CountTimer());
    }

    bool isStart;
    /// <summary>
    /// 대기화면 카운트다운
    /// </summary>
    /// <returns></returns>
    IEnumerator CountTimer()
    {
        yield return null;

        MiniGameCo = StartCoroutine(TimerReset());

        int cnt = 3;
        while (cnt > 0)
        {
            CountText.text = cnt.ToString();
            yield return new WaitForSeconds(1);
            cnt--;
        }

        PreparedPanel.SetActive(false);

        yield return new WaitForSeconds(1);

        isStart = true;

    }

    /// <summary>
    /// 미니게임 지속 시간 0.5초씩 증가
    /// </summary>
    float cnt;
    Coroutine MiniGameCo;
    /// <summary>
    /// 미니게임 30초 카운트 초기화.
    /// </summary>
    IEnumerator TimerReset()
    {
        yield return null;

        float Maxcnt = 30f + (PlayerPrefsManager.GetInstance().Arti_MiniGameTime * 0.1f);
        //float Maxcnt = 30f;
        cnt = Maxcnt;

        //TimerFillamount.text = "남은 시간 : " + string.Format("{0:f1}", Maxcnt);

        while (cnt > 0)
        {
            yield return new WaitForSeconds(0.05f);

            if (isStart)
            {
                cnt -= 0.05f;

                TimerFillamount.fillAmount = cnt / Maxcnt;
                //TimerFillamount.text = "남은 시간 : " + string.Format("{0:f1}", cnt);
            }

        }

        MiniGameOver();

    }

    DoubleToStringNum dts = new DoubleToStringNum();
    /// <summary>
    /// 시간 다 되거나 / 틀렸을 경우.
    /// </summary>
    void MiniGameOver()
    {
        Btn_Exit.SetActive(false);

        Debug.LogWarning("미니게임 보상 받고 종료");
        //타이머 초기화.
        StopCoroutine(MiniGameCo);

        PopUpObjectManager.GetInstance().ShowWarnnigProcess("미니게임 종료.");
        //퀘스트
        PlayerPrefsManager.GetInstance().questInfo3[0].All_MiniGame++;
        isStart = false;

        Invoke("InvoClear", 1.0f);
    }
    void InvoClear()
    {
        PlayerPrefsManager.GetInstance().questInfo3[0].daily_MiniGameCombo = Combo;
        PlayerPrefsManager.GetInstance().MaxGet_MiniGame = Combo;

        if (Combo >= 30)
        {
            tmm.ExUpdateMission(28);
        }
        else if (Combo >= 50)
        {
            tmm.ExUpdateMission(34);
        }
        else if (Combo >= 100)
        {
            tmm.ExUpdateMission(51);
        }
        else if (Combo >= 150)
        {
            tmm.ExUpdateMission(65);
        }
        else if (Combo >= 200)
        {
            tmm.ExUpdateMission(84);
        }

        /// 쌀밥 유물 획득량 % 증가
        float getSSalAmount = (Combo * (1.0f + PlayerPrefsManager.GetInstance().Arti_MiniReword * 0.005f));
        /// 
        getSSalAmount = getSSalAmount * (1.0f + 
            (
            (PlayerPrefsManager.GetInstance().uniformInfo[5].Uniform_LV + 
            PlayerPrefsManager.GetInstance().uniformInfo[6].Uniform_LV + 
            (PlayerPrefsManager.GetInstance().uniformInfo[6].Skill_LV * 0.5f)) 
            * 0.01f)
            );

        // 팝업 띄워죠라.
        PopUpObjectManager.GetInstance().ShowSSalPopUP(Mathf.RoundToInt(getSSalAmount));

        gotoMINIgame.BackFromMiniGameView();

    }
    /// <summary>
    /// 외부에서 부르는 소탕 메서드
    /// </summary>
    public void ExGameClear()
    {
        PlayerPrefsManager.GetInstance().key--;
        //UserWallet.GetInstance().ShowUserKey();
        booster_KEY.KeyTimerStart();

        int Combo = PlayerPrefsManager.GetInstance().MaxGet_MiniGame;

        /// 쌀밥 획득량 % 증가
        float getSSalAmount = (Combo * (1.0f + PlayerPrefsManager.GetInstance().Arti_MiniReword * 0.005f));

        // 팝업 띄워죠라.
        PopUpObjectManager.GetInstance().ShowSSalPopUPSkip(Mathf.RoundToInt(getSSalAmount));
    }



    /// <summary>
    /// 그만두기 버튼 누른다
    /// </summary>
    public void EndBtnClicked()
    {
        ////타이머 초기화.
        //StopCoroutine(MiniGameCo);

        //PopUpObjectManager.GetInstance().ShowWarnnigProcess("미니게임 종료.");
        ////퀘스트
        //PlayerPrefsManager.GetInstance().questInfo3[0].All_MiniGame_Cnt++;
        //isStart = false;

        //Invoke("InvoBackHome", 1.0f);

        MiniGameOver();
    }
    //void InvoBackHome()
    //{
    //    gotoMINIgame.BackFromMiniGameView();
    //}

    [Header("- 먹는 모습")]
    public GameObject EattingMotion;
    Coroutine Eatting;

    IEnumerator EattingThat()
    {
        yield return null;
        EattingMotion.SetActive(true);
        AudioManager.instance.MiniGameYAM();
        yield return new WaitForSeconds(0.3f);
        EattingMotion.SetActive(false);
    }


    /// <summary>
    /// 버튼 누르면 번쩍
    /// </summary>
    /// <param name="_swich"></param>
    void InvoFlash(bool _swich)
    {
        if(_swich) R_Flash.SetActive(true);
        else L_Flash.SetActive(true);

        Invoke("FlashEnd", 0.2f);
    }
    void FlashEnd()
    {
        R_Flash.SetActive(false);
        L_Flash.SetActive(false);
    }



    bool isLeftBtnClicked;
    bool isRightBtnClicked;

    int Combo;

    /// <summary>
    /// 왼쪽 클릭 입력시 호출
    /// </summary>
    public void LeftClicked()
    {
        InvoFlash(false);

        if (isAnimPlaying || isRightBtnClicked) return;

        isLeftBtnClicked = true;

        if (isFeverTime)
        {
            FeverSuccess();
            TimeUP();
            return;
        }

        /// 센터에 올라온 박스가 옳은 방향인지 체크
        if (CenterLeftBox.activeSelf)
        {
            Eatting = StartCoroutine(EattingThat());
            /// (왼쪽)옳게 입력
            LeftSuccess();
            TimeUP();
            Combo++;
            FeverUP();
            ComboText.text = Combo.ToString();
        }
        else
        {
            /// (공통)잘못 입력 패널티
            WrongClicked();
        }
        /// 
    }

    /// <summary>
    /// 오른쪽 클릭 입력시 호출
    /// </summary>
    public void RightClicked()
    {
        InvoFlash(true);

        if (isAnimPlaying || isLeftBtnClicked) return;

        isRightBtnClicked = true;



        if (isFeverTime)
        {
            FeverSuccess();
            TimeUP();
            return;
        }

        /// 센터에 올라온 박스가 옳은 방향인지 체크
        if (CenterRightBox.activeSelf)
        {
            Eatting = StartCoroutine(EattingThat());
            /// (오른쪽)옳게 입력
            RightSuccess();
            TimeUP();
            Combo++;
            FeverUP();
            ComboText.text = Combo.ToString();
        }
        else
        {
            /// (공통)잘못 입력 패널티
            WrongClicked();
        }
        /// 
    }

    bool isGameEnding;
    /// <summary>
    /// 잘못된 방향키를 입력하였다.
    /// 1. 게임 종료
    /// </summary>
    private void WrongClicked()
    {
        if (isGameEnding) return;
        isGameEnding = true;
        if (Eatting != null) StopCoroutine(Eatting);

        StopCoroutine(MiniGameCo);

        MiniGameOver();
    }

    /// <summary>
    /// 피버타임 발동
    /// </summary>
    bool isFeverTime;
    /// <summary>
    /// 1. 피버타임에는 0.1
    /// 2. 30초 이상이면 증가 안함
    /// </summary>
    void TimeUP()
    {
        if (cnt < 29.8f)
        {
            var bonusTime = Combo / 100;
            /// 콤보시 시간 증가
            if (!isFeverTime)
            {
                cnt += bonusTime > 10 ? (0.2f - (bonusTime * 0.01f)) : 0.1f;
            }
            else
            {
                cnt += 0.1f;
            }
        }
    }


    int feverCnt;
    /// <summary>
    /// 1. 피버타임에는 0.1
    /// 2. 30초 이상이면 증가 안함
    /// </summary>
    void FeverUP()
    {
        AnimFlash.GetComponent<Animation>()["EXP"].speed = 1;
        AnimFlash.GetComponent<Animation>().Play("EXP");

        if (!isFeverTime)
        {
            feverCnt += 1;
            FeverFillamount.fillAmount = feverCnt / 50f;

            if (feverCnt > 50)
            {
                FeverFillamount.fillAmount = 1;
                isFeverTime = true;
                InitFeverBox();
                feverCnt = 0;
                StartCoroutine(FeverTime());
            }
        }


    }

    public GameObject FlashFlash;
    IEnumerator FeverTime()
    {
        FlashFlash.SetActive(true);

        yield return null;
        /// TODO 피버 타임.
        EattingMotion.SetActive(false);

        float cnt = 5f;
        while (cnt > 0)
        {
            yield return new WaitForSeconds(0.05f);


            if (isStart)
            {
                cnt -= 0.05f;

                FeverFillamount.fillAmount = cnt / 5f;

                if(cnt < 4.5f) FlashFlash.SetActive(false);
            }

        }
        // 5초뒤 피버 타임 종료
        isFeverTime = false;
        FeverFillamount.fillAmount = 0;
        InitBox();
    }





    /// <summary>
    /// 왼쪽 잘 눌렀음
    /// </summary>
    private void LeftSuccess()
    {
        /// 센터 박스 제거
        CenterLeftBox.SetActive(false);
        /// 양쪽 한칸씩 땡기고
        MoveLine();
        /// 센터에 블록생성
        if(CenterImgIndx[0] != 0)
        {
            NextBoxBaby("R");
        }
        else
        {
            NextBoxBaby("Left");
        }
        isLeftBtnClicked = false;

        CenterImgIndx.RemoveAt(0);

    }

    /// <summary>
    /// 오른쪽 잘 눌렀음
    /// </summary>
    private void RightSuccess()
    {
        /// 센터 박스 제거
        CenterRightBox.SetActive(false);
        /// 양쪽 한칸씩 땡기고
        MoveLine();
        /// 센터에 블록생성
        if (CenterImgIndx[0] != 0)
        {
            NextBoxBaby("R");
        }
        else
        {
            NextBoxBaby("Left");
        }
        isRightBtnClicked = false;

        CenterImgIndx.RemoveAt(0);

    }

    /// <summary>
    /// 피버에 잘 눌렀음
    /// 
    /// </summary>
    void FeverSuccess()
    {
        Eatting = StartCoroutine(EattingThat());

        Combo +=2;
        ComboText.text = Combo.ToString();

        /// 센터 박스 제거
        CenterCenterBox.SetActive(false);
        /// 양쪽 한칸씩 땡기고
        MoveLine();
        /// 센터에 블록생성
        if (CenterImgIndx[0] != 0)
        {
            NextBoxFever("R");
        }
        else
        {
            NextBoxFever("Left");
        }

        isRightBtnClicked = false;
        isLeftBtnClicked = false;

        CenterImgIndx.RemoveAt(0);

    }



    /// <summary>
    /// 양쪽 박스 한칸씩 땅기고 스크롤뷰 초기화화.
    /// </summary>
    private void MoveLine()
    {
        moveRoutine = StartCoroutine(Progress());
    }

    /// <summary>
    /// 블럭제거 되면 새 블록 생성
    /// </summary>
    private void NextBoxBaby(string Direction)
    {
        if (Direction == "Left")
        {
            CenterLeftBox.SetActive(true);
            CenterLeftBox.GetComponent<Animation>()["MINI_LEFT"].speed = 1;
            CenterLeftBox.GetComponent<Animation>().Play("MINI_LEFT");
            CenterLeftBox.GetComponent<Animation>()["Lunch_Bap"].speed = 1;
            CenterLeftBox.GetComponent<Animation>().Play("Lunch_Bap");
        }
        else
        {
            CenterRightBox.SetActive(true);
            CenterRightBox.GetComponent<Animation>()["MINI_RIGHT"].speed = 1;
            CenterRightBox.GetComponent<Animation>().Play("MINI_RIGHT");
            CenterRightBox.GetComponent<Animation>()["Lunch_Bap"].speed = 1;
            CenterRightBox.GetComponent<Animation>().Play("Lunch_Bap");
        }
    }

    /// <summary>
    /// 피버타임 블록 없애
    /// </summary>
    private void NextBoxFever(string Direction)
    {
        if (Direction == "Left")
        {
            CenterCenterBox.SetActive(true);
            CenterCenterBox.GetComponent<Animation>()["MINI_LEFT"].speed = 1;
            CenterCenterBox.GetComponent<Animation>().Play("MINI_LEFT");
            CenterCenterBox.GetComponent<Animation>()["Lunch_Bap"].speed = 1;
            CenterCenterBox.GetComponent<Animation>().Play("Lunch_Bap");
        }
        else
        {
            CenterCenterBox.SetActive(true);
            CenterCenterBox.GetComponent<Animation>()["MINI_RIGHT"].speed = 1;
            CenterCenterBox.GetComponent<Animation>().Play("MINI_RIGHT");
            CenterCenterBox.GetComponent<Animation>()["Lunch_Bap"].speed = 1;
            CenterCenterBox.GetComponent<Animation>().Play("Lunch_Bap");
        }
    }

    /// <summary>
    /// 양쪽 바깥에서 선형보간으로 안쪽으로 땡기기
    /// </summary>
    /// <returns></returns>
    private IEnumerator Progress()
    {
        isAnimPlaying = true;

        float rate = 10f;

        float progress = 0.0f;

        while (progress <= 1f)
        {
            yield return new WaitForFixedUpdate();

            var moving_R = Mathf.Lerp(0, -180, progress);
            var moving_L = Mathf.Lerp(-1800, -1620, progress);

            R_Content.anchoredPosition = new Vector2(moving_R, 0);
            L_Content.anchoredPosition = new Vector2(moving_L, 0);

            rate++;
            progress += rate * Time.deltaTime;

        }

        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
            moveRoutine = null;
        }

        // 자식 박스 삭제
        Lean.Pool.LeanPool.Despawn(R_Content.GetChild(0).gameObject);
        Lean.Pool.LeanPool.Despawn(L_Content.GetChild(0).gameObject);

        //삭제 하고 새로 생성
        if (isFeverTime)
        {
            Gen_F_Box();
        }
        else
        {
            GenGenBox();
        }

        // 스크롤뷰 초기화
        R_scrollRect.horizontalNormalizedPosition = 0f;
        L_scrollRect.horizontalNormalizedPosition = 1f;

        // 코루틴 끝
        isAnimPlaying = false;

    }






}
