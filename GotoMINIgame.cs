using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GotoMINIgame : MonoBehaviour
{
    [Header("- PVP 설정")]
    public PlayFabLogin playfabMag;
    public GameObject PVP_Canvas;
    public GameObject TOP_Panel;

    [Header("- 출첵버튼 등등")]
    public GameObject dailyCheckBtn;
    public Image MainHP_fill;
    public Text MainHP_Text;
    [Header("- 버프창 꺼주실 거죠")]
    public GameObject TopCanvasLeft;
    public Booster_AUTO Auto;
    [Header("- 메인 카메라")]
    public Transform MainCamera;
    [Header("- 캔버스 전환")]
    public GameObject MainCanvas;
    public GameObject DefenceModeCanvas;
    public GameObject MiniGameCanvas;
    public GameObject MuGanGameCanvas;
    [Header("- 설정 버튼 오브젝트")]
    public GameObject PakageBtn;
    public GameObject ConfigBtn;
    public GameObject DailyCheckBtn;
    public GameObject FAQ_Btn;
    public GameObject CHat_Btn;
    public GameObject DefendMode;
    public GameObject InfiEndBtn;
    public GameObject PVP_btn;
    public GameObject Invite_btn;
    [Header("-리지드 바디 부모객체")]
    public SpriteRenderer MainBody; // 히트 바디
    public SpriteRenderer DefendBody; // 히트 바디
    [Header("-좌측 버프 버튼")]
    public GameObject LeftBuffDisp;



    /// <summary>
    ///  메인 홈페이지로 돌아오면 체력 풀피로 만들어 줌
    /// </summary>
    public void BackToFuture()
    {
        var tmp = PlayerPrefsManager.GetInstance().Mat_MaxHP;
        var tmpText = UserWallet.GetInstance().SeetheNatural(double.Parse(tmp));
        PlayerPrefsManager.GetInstance().Mat_currentHP = tmp;
        MainHP_fill.fillAmount = 1f;
        MainHP_Text.text = tmpText + "/" + tmpText;

        dailyCheckBtn.SetActive(true);
    }

    /// <summary>
    ///  만약 그로기 상태였다면 강제로 풀어줌.
    ///  (단, 너 고소 상태는 버튼 터치 안먹히니까 예외.)
    /// </summary>
    void GroggyReset()
    {
        // 그로기 초기화.
        PlayerPrefsManager.GetInstance().isGroggy = false;
        MainBody.enabled = true;
        DefendBody.enabled = true;
        MainBody.GetComponent<Animation>().Stop();
        DefendBody.GetComponent<Animation>().Stop();
    }

    /// <summary>
    /// 메인 씬에서 무한 버티기 아이콘 클릭하면 
    /// 1. 카메라 움직이고.
    /// 2. 메인 UI 꺼주고
    /// 3. 미니 UI 켜준다.
    /// </summary>
    public void ChangeCamToMiniView()
    {
        // 무한 버티기 
        Auto.BuffImgOnOff(false);
        //MainCanvas.SetActive(false);
        //MiniGameCanvas.SetActive(true);
        // 설정버튼 숨김
        ConfigBtn.SetActive(false);
        // 출석버튼 숨김
        DailyCheckBtn.SetActive(false);
        PVP_btn.SetActive(false);
        FAQ_Btn.SetActive(false);
        CHat_Btn.SetActive(false);
        PakageBtn.SetActive(false);
        Invite_btn.SetActive(false);

        DefendMode.SetActive(false);
        // 그만두기 버튼 활성화
        InfiEndBtn.SetActive(true);
        // 좌측 버프 아이콘 숨기기
        LeftBuffDisp.SetActive(false);

        GroggyReset();

        AudioManager.instance.BnextBGM();

        MainCamera.position = new Vector3(0, -20, -10);


        // 국밥 콤보 카운트 초기화.
        PopUpObjectManager.GetInstance().ComboCnt = 0;
        PopUpObjectManager.GetInstance().SetComboText();
        PopUpObjectManager.GetInstance().ShowInfiCombo(true);

    }

    /// <summary>
    /// 무한 버티기 에서 나가기
    /// 역순.
    /// </summary>
    public void ChangeCamToHomePage()
    {
        //MiniGameCanvas.SetActive(false);
        //MainCanvas.SetActive(true);
        // 설정버튼 표시
        ConfigBtn.SetActive(true);
        // 출석버튼 표시
        DailyCheckBtn.SetActive(true);
        PVP_btn.SetActive(true);
        FAQ_Btn.SetActive(true);
        CHat_Btn.SetActive(true);
        PakageBtn.SetActive(true);
        Invite_btn.SetActive(true);

        DefendMode.SetActive(true);
        // 그만두기 버튼 비활성화
        InfiEndBtn.SetActive(false);
        // 좌측 버프 아이콘 표시
        LeftBuffDisp.SetActive(true);
        // 
        AudioManager.instance.PlayMainBGM();
        GroggyReset();

        MainCamera.position = new Vector3(0, 0, -10);

        PopUpObjectManager.GetInstance().ShowInfiCombo(false);

        BackToFuture();

    }


    public DenfenceMode denfenceMode;
    public GameObject coverImg;
    /// <summary>
    /// 메인 씬에서 방어전 버튼 누르면
    /// </summary>
    public void DefenceModeOn()
    {
        //쿨타임 중이라 회색이면 리턴처리
        if (coverImg.activeSelf)
        {
            PopUpObjectManager.GetInstance().ShowNewDefencePop();
            return;
        }

        // 펀치 갯수
        int punchAmong = PlayerPrefsManager.GetInstance().punchAmont;

        int nextWeaponIndex = 0;
        for (int i = 0; i < punchAmong; i++)
        {
            // i 는 다음에 해금할 무기
            if (!PlayerPrefsManager.GetInstance().weaponInfo[i].isUnlock)
            {
                nextWeaponIndex = i;
                break;
            }
        }
        // (i-1) 지금 해금 된 무기.
        // PunchIndex 는 현재 장착한 무기.
        // 최신 무기가 아니면?
        if (PlayerPrefsManager.GetInstance().PunchIndex != (nextWeaponIndex - 1))
        {
            //Debug.LogWarning("iiiiiii" + nextWeaponIndex);
            //Debug.LogWarning("PlayerPrefsManager.GetInstance().PunchIndex L:: " + PlayerPrefsManager.GetInstance().PunchIndex);
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("먼저 새로운 훈련 장비를 장착하세요.");

            return;
        }

        // 펀치 잠시 꺼주기
        Auto.BuffImgOnOff(false);

        MainCamera.position = new Vector3(30, 0, -10);

        MainCanvas.SetActive(false);
        DefenceModeCanvas.SetActive(true);
        // 설정버튼 숨김
        ConfigBtn.SetActive(false);
        // 출석버튼 숨김
        DailyCheckBtn.SetActive(false);
        PVP_btn.SetActive(false);
        FAQ_Btn.SetActive(false);
        CHat_Btn.SetActive(false);
        PakageBtn.SetActive(false);
        Invite_btn.SetActive(false);


        GroggyReset();

        denfenceMode.CameraOn();

        AudioManager.instance.BnextBGM();


    }

    /// <summary>
    /// 방어전에서 나가기
    /// 역순.
    /// </summary>
    public void DefenceModeOff()
    {
        DefenceModeCanvas.SetActive(false);
        MainCanvas.SetActive(true);
        // 설정버튼 표시
        ConfigBtn.SetActive(true);
        // 출석버튼 표시
        PVP_btn.SetActive(true);
        DailyCheckBtn.SetActive(true);
        FAQ_Btn.SetActive(true);
        CHat_Btn.SetActive(true);
        PakageBtn.SetActive(true);
        Invite_btn.SetActive(true);

        GroggyReset();

        MainCamera.position = new Vector3(0, 0, -10);

        AudioManager.instance.PlayMainBGM();

        BackToFuture();

    }



    /// <summary>
    /// 메인 씬에서 무한의 탑으로 들어가기.
    /// </summary>
    public void ChangeCamMUGANTOP()
    {
        PlayerPrefsManager.GetInstance().isMuGanTopEnd = false;

        MainCanvas.SetActive(false);
        TopCanvasLeft.SetActive(false);
        MuGanGameCanvas.SetActive(true);
        // 설정버튼 숨김
        ConfigBtn.SetActive(false);
        // 출석버튼 숨김
        DailyCheckBtn.SetActive(false);
        PVP_btn.SetActive(false);
        FAQ_Btn.SetActive(false);
        CHat_Btn.SetActive(false);
        PakageBtn.SetActive(false);
        Invite_btn.SetActive(false);







        // 펀치 잠시 꺼주기
        Auto.BuffImgOnOff(false);

        AudioManager.instance.MuganBGM();

        MainCamera.position = new Vector3(30, -20, -10);





    }

    /// <summary>
    /// 무한의 탑 무한의 탑 무한으 ㅣ탑 탑ㅌㅇ탑티오피
    /// 역순.
    /// </summary>
    public void ChangeCamToHomeMuGan()
    {
        MuGanGameCanvas.SetActive(false);
        TopCanvasLeft.SetActive(true);

        MainCanvas.SetActive(true);
        // 설정버튼 표시
        ConfigBtn.SetActive(true);
        // 출석버튼 표시
        DailyCheckBtn.SetActive(true);
        FAQ_Btn.SetActive(true);
        CHat_Btn.SetActive(true);
        PakageBtn.SetActive(true);
        PVP_btn.SetActive(true);
        Invite_btn.SetActive(true);

        // 
        AudioManager.instance.PlayMainBGM();

        MainCamera.position = new Vector3(0, 0, -10);

        BackToFuture();


    }


    [Header("- 미니 게임 매니저 ")]
    public MINIgameManager miniGameMag;
    /// <summary>
    /// 빵우유 먹기 미니 게임 진입
    /// 1. 카메라 위치는 - 40
    /// </summary>
    public void EnterTheMiniGameView()
    {
        // 열쇠 체크체크
        if (PlayerPrefsManager.GetInstance().key == 0)
        {
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("열쇠가 부족하여 입장할 수 없습니다.");
            return;
        }

        Auto.BuffImgOnOff(false);
        MainCanvas.SetActive(false);
        MiniGameCanvas.SetActive(true);
        // 설정버튼 숨김
        ConfigBtn.SetActive(false);
        // 출석버튼 숨김
        DailyCheckBtn.SetActive(false);
        PVP_btn.SetActive(false);
        FAQ_Btn.SetActive(false);
        CHat_Btn.SetActive(false);
        PakageBtn.SetActive(false);
        Invite_btn.SetActive(false);

        DefendMode.SetActive(false);
        //
        GroggyReset();
        //
        AudioManager.instance.MiniGameBGM();

        MainCamera.position = new Vector3(0, -40, -10);

        //게임 스타트
        miniGameMag.InitMiniGame();

    }


    /// <summary>
    /// 빵우유 먹기 미니 게임 소탕
    /// 열쇠 소모하고 최대 스테이지 불러와서 보상 지금
    /// </summary>
    public void MiniGameSpeedClear()
    {
        // 회색버튼 활성화 라면 리턴
        if (PopUpObjectManager.GetInstance().MINIMINIGAME.transform.GetChild(1).GetChild(3).GetChild(1).GetChild(1).gameObject.activeSelf) return;

        // 열쇠 체크체크
        if (PlayerPrefsManager.GetInstance().key == 0)
        {
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("열쇠가 부족하여 소탕할 수 없습니다.");
            return;
        }

        // 최대 기록만큼 쌀밥 지급
        miniGameMag.ExGameClear();



    }



    /// <summary>
    ///  빵 우유 미니 게임에서 홈페이지로 진입.
    /// </summary>
    public void BackFromMiniGameView()
    {
        MiniGameCanvas.SetActive(false);
        MainCanvas.SetActive(true);
        TOP_Panel.SetActive(true);
        PVP_Canvas.SetActive(false);

        // 설정버튼 표시
        ConfigBtn.SetActive(true);
        // 출석버튼 표시
        DailyCheckBtn.SetActive(true);
        FAQ_Btn.SetActive(true);
        CHat_Btn.SetActive(true);
        PakageBtn.SetActive(true);
        PVP_btn.SetActive(true);
        Invite_btn.SetActive(true);

        DefendMode.SetActive(true);
        // 
        AudioManager.instance.PlayMainBGM();
        GroggyReset();

        MainCamera.position = new Vector3(0, 0, -10);

        // 체력 회복.
        BackToFuture();

        GameObject.Find("PlayfabManager").GetComponent<PlayFabLogin>().RevovGrogy();
    }



    /// <summary>
    /// pvp 진입 진입
    /// 1. 카메라 위치는 y +20
    /// </summary>
    public void EnterThePVP_WarGame()
    {
        // 초대장 체크체크
        if (PlayerPrefsManager.GetInstance().ticket == 0)
        {
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("입장권이 부족하여 입장할 수 없습니다.");
            return;
        }

        Auto.BuffImgOnOff(false);
        MainCanvas.SetActive(false);
        PVP_Canvas.SetActive(true);
        TOP_Panel.SetActive(false);
        // 설정버튼 숨김
        ConfigBtn.SetActive(false);
        // 출석버튼 숨김
        DailyCheckBtn.SetActive(false);
        FAQ_Btn.SetActive(false);
        CHat_Btn.SetActive(false);
        PakageBtn.SetActive(false);
        PVP_btn.SetActive(false);
        Invite_btn.SetActive(false);

        DefendMode.SetActive(false);
        //
        GroggyReset();
        //
        AudioManager.instance.PVP_BGM();

        MainCamera.position = new Vector3(0, 20, -10);

        //게임 스타트
        playfabMag.InitMiniGame();

    }























}
