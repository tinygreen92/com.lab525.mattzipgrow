using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingPage : MonoBehaviour
{
    public PlayNANOOExample playNANOO;
    [Header("-상단 Clickable 탭")]
    public GameObject powerUP_TAP;          // 클릭된 이미지
    public GameObject trainnig_TAP;         // 클릭된 이미지
    public GameObject mini_TAP;         // 클릭된 이미지
    [Header("-Conent 1번 2번")]
    public RectTransform power_Rect;
    public RectTransform train_Rect;
    public RectTransform mini_Rect;
    [Header("-Viewport 1번 2번")]
    public RectTransform power_View;
    public RectTransform train_View;
    public RectTransform mini_View;
    [Header("-Viewport 1번 2번")]
    public Text MaxValuText;



    ScrollRect thisSCRect;

    /// <summary>
    /// 버튼 클릭시 이 메소드로 호출
    /// </summary>
    public void OpenPopUP()
    {

        thisSCRect = GetComponentInChildren<ScrollRect>();
        thisSCRect.verticalNormalizedPosition = 1f; // 세로 스크롤 뷰

        if (gameObject.activeSelf) 
            return;
        /// 안켜져있으면 켜줌
        gameObject.SetActive(true);
        // 맷집 기록후 열람
        playNANOO.RankingRecordMattzip();
        // 로딩중 애니메이션
        PlayerPrefsManager.GetInstance().IN_APP.SetActive(true);
        Invoke(nameof(InvoRank), 0.6f);
    }

    void InvoRank()
    {
        PlayerPrefsManager.GetInstance().IN_APP.SetActive(false);
        Tap_Click(1);
    }

    /// <summary>
    /// 상단 버튼 누르기 관리
    /// </summary>
    /// <param name="_buttonIndex">버튼 이벤트에 붙일때 인덱스 지정</param>
    public void Tap_Click(int _buttonIndex)
    {
        /// 번역
        bool isKorean = false;
        if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean") isKorean = true;

        switch (_buttonIndex)
        {
            case 1:


                powerUP_TAP.SetActive(true);
                trainnig_TAP.SetActive(false);
                mini_TAP.SetActive(false);

                /// 맷집력 조회
                playNANOO.RankingMatt();
                //playNANOO.BeforeRankingMatt();

                while (train_Rect.childCount != 0)
                {
                    Lean.Pool.LeanPool.Despawn(train_Rect.GetChild(0));
                }
                while (power_Rect.childCount != 0)
                {
                    Lean.Pool.LeanPool.Despawn(power_Rect.GetChild(0));
                }
                while (mini_Rect.childCount != 0)
                {
                    Lean.Pool.LeanPool.Despawn(mini_Rect.GetChild(0));
                }

                InitPowerUP();

                _index_R = 0;
                _index_L = 0;
                _index_RR = 0;
                //
                /// 번역
                if (isKorean)
                    MaxValuText.text = "최대 맷집";
                else
                    MaxValuText.text = "번역 맷집";

                break;

            case 2:
                powerUP_TAP.SetActive(false);
                trainnig_TAP.SetActive(true);
                mini_TAP.SetActive(false);


                // 버티기 기록
                //playNANOO.RankingRecordMuganTop();
                playNANOO.RankingMuganTop();

                while (train_Rect.childCount != 0)
                {
                    Lean.Pool.LeanPool.Despawn(train_Rect.GetChild(0));
                }
                while (power_Rect.childCount != 0)
                {
                    Lean.Pool.LeanPool.Despawn(power_Rect.GetChild(0));
                }
                while (mini_Rect.childCount != 0)
                {
                    Lean.Pool.LeanPool.Despawn(mini_Rect.GetChild(0));
                }

                InitTrain();

                _index_R = 0;
                _index_L = 0;
                _index_RR = 0;

                //
                /// 번역
                if (isKorean)
                    MaxValuText.text = "최대 층수";
                else
                    MaxValuText.text = "번역 층수";

                break;

            case 3:
                powerUP_TAP.SetActive(false);
                trainnig_TAP.SetActive(false);
                mini_TAP.SetActive(true);

                ///미니게임 기록 보기
                //playNANOO.RankingRecordMinini();
                playNANOO.RankingMini();

                while (train_Rect.childCount != 0)
                {
                    Lean.Pool.LeanPool.Despawn(train_Rect.GetChild(0));
                }
                while (power_Rect.childCount != 0)
                {
                    Lean.Pool.LeanPool.Despawn(power_Rect.GetChild(0));
                }
                while (mini_Rect.childCount != 0)
                {
                    Lean.Pool.LeanPool.Despawn(mini_Rect.GetChild(0));
                }

                InitMini();


                _index_R = 0;
                _index_L = 0;
                _index_RR = 0;

                //
                /// 번역
                if (isKorean)
                    MaxValuText.text = "최다 콤보";
                else
                    MaxValuText.text = "번역 콤보";

                break;

        }
    }

    /// <summary>
    /// 상단 탭 누를때 뷰 전환
    /// </summary>
    void InitPowerUP()
    {
        // 컨텐츠 오브젝트 활성화
        power_View.gameObject.SetActive(true);
        train_View.gameObject.SetActive(false);
        mini_View.gameObject.SetActive(false);

        //스크롤 뷰 교체
        thisSCRect.content = power_Rect;
        thisSCRect.viewport = power_View;

        //

    }
    void InitTrain()
    {
        // 컨텐츠 오브젝트 활성화
        power_View.gameObject.SetActive(false);
        train_View.gameObject.SetActive(true);
        mini_View.gameObject.SetActive(false);

        //스크롤 뷰 교체
        thisSCRect.content = train_Rect;
        thisSCRect.viewport = train_View;

    }
    void InitMini()
    {
        // 컨텐츠 오브젝트 활성화
        power_View.gameObject.SetActive(false);
        train_View.gameObject.SetActive(false);
        mini_View.gameObject.SetActive(true);
        //스크롤 뷰 교체
        thisSCRect.content = mini_Rect;
        thisSCRect.viewport = mini_View;

    }

    [Header("- 랭킹 카드 프리팹")]
    public Transform Cart;

    int _index_R = 0;
    int _index_L = 0;
    int _index_RR = 0;
    /// <summary>
    /// 외부에서 이걸로 호출하셈
    /// </summary>
    public void CardInit(string name, string score, int indx)
    {
        int _index = 0;
        if (indx == 0)
        {
            // 맷집 
            _index = _index_R;
        }
        else if(indx == 1)
        {
            //버티기
            _index = _index_L;
        }
        else
        {
            _index = _index_RR;
        }

        var eneObj = Cart;
        //프리팹에서 박스 생성
        Transform initBox = Lean.Pool.LeanPool.Spawn(eneObj);
         
        if (indx == 0) initBox.SetParent(power_Rect); // 스크롤뷰 안쪽에 생성.
        else if (indx == 1) initBox.SetParent(train_Rect); // 스크롤뷰 안쪽에 생성.
        else initBox.SetParent(mini_Rect);

        initBox.localPosition = Vector3.zero; // 뒤틀리는거 방지
        initBox.localScale = new Vector3(1, 1, 1);

        initBox.GetChild(0).GetChild(0).gameObject.SetActive(false);
        initBox.GetChild(0).GetChild(1).gameObject.SetActive(false);
        initBox.GetChild(0).GetChild(2).gameObject.SetActive(false);
        initBox.GetChild(0).GetChild(3).gameObject.SetActive(false);

        if (_index == 0)
        {
            initBox.GetChild(0).GetChild(_index).gameObject.SetActive(true);
        }
        else if (_index == 1)
        {
            initBox.GetChild(0).GetChild(_index).gameObject.SetActive(true);
        }
        else if (_index == 2)
        {
            initBox.GetChild(0).GetChild(_index).gameObject.SetActive(true);
        }
        else
        {
            initBox.GetChild(0).GetChild(3).gameObject.SetActive(true);
            initBox.GetChild(0).GetChild(3).GetChild(0).GetComponent<Text>().text = (_index + 1).ToString();
        }

        initBox.GetChild(1).GetComponent<Text>().text = name;
        initBox.GetChild(2).GetComponent<Text>().text = score;

        if (indx == 0)
        {
            // 맷집 
            _index_R++;
        }
        else if (indx == 1)
        {
            //버티기
            _index_L++;
        }
        else
        {
            //미니 게임
            _index_RR++;
        }
    }


    private void OnDisable()
    {
        while (power_Rect.childCount != 0)
        {
            Lean.Pool.LeanPool.Despawn(power_Rect.GetChild(0));
        }

        while (train_Rect.childCount != 0)
        {
            Lean.Pool.LeanPool.Despawn(train_Rect.GetChild(0));
        }

        while (mini_Rect.childCount != 0)
        {
            Lean.Pool.LeanPool.Despawn(mini_Rect.GetChild(0));
        }

        _index_R = 0;
        _index_L = 0;
        _index_RR = 0;
    }

}
