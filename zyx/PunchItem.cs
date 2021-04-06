using System;
using UnityEngine;
using UnityEngine.UI;


public class PunchItem : MonoBehaviour
{
    readonly DoubleToStringNum dts = new DoubleToStringNum();

    public PunchManager pm;
    [Header("- 회색 커버 오브젝트")]
    public GameObject GrayImage;

    [Header("- 아이콘")]
    public Image spriteBox;

    [Header("- 정보 표기 부분")]
    public Text NameBox;
    public Text LevelBox;
    public Text DescBox;

    [Header("- 버튼 부분")]
    public GameObject EquipButton;
    public GameObject MaxButton;
    public Text UpgradeBox;
    public Image somoIcon;

    /// <summary>
    /// 펀치인덱스 0~99 
    /// </summary>
    int thisIndex;
    int thisWeaponLevel;
    string thisWeaponCost;
    float thisWeaponEffect;

    #region [초기 세팅] + 변동 정보 새로고침

    public void BoxInfoUpdate(int cnt)
    {
        /// 인덱스 설정 -> 이 스크립트 전체
        thisIndex = cnt;
        // 서순 1
        SetUpdateInfo();
        // 서순 2
        SetDefaltInfo();
    }

    /// <summary>
    /// 아이콘/이름/회색커버 기본 정보 새로고침
    /// </summary>
    void SetDefaltInfo()
    {
        spriteBox.sprite = pm.punchImgs[thisIndex];
        NameBox.text = pm.PunchNames[thisIndex];
        /// 회색 커버
        SetAllGrayCover(thisIndex);
        /// 장착 여부 버튼
        SetAllEpuipBtnToGray();
    }

    /// <summary>
    /// 방어전 성공시 해금
    /// </summary>
    public void SetAllGrayCover(int p_index)
    {
        if (thisIndex != p_index) return;

        /// isUnlock 이면 전체 회색 화면 False
        if (PlayerPrefsManager.GetInstance().weaponInfo[thisIndex].isUnlock)
        {
            // 아이콘 색상 
            spriteBox.color = Color.white;
            GrayImage.SetActive(false);
        }
        else
        {
            spriteBox.color = Color.black;
            GrayImage.SetActive(true);
        }
    }

    /// <summary>
    /// 변동 정보 출력
    /// </summary>
    public void SetUpdateInfo()
    {
        /// 저장된 해당 펀치 리스트 가져와서 뿌려주고
        var tmpList = PlayerPrefsManager.GetInstance().weaponInfo[thisIndex];

        thisWeaponLevel = tmpList.weaponLevel;
        thisWeaponEffect = tmpList.weaponEffect;
        thisWeaponCost = tmpList.weaponCost;

        LevelBox.text = "Lv. " + thisWeaponLevel;
        DescBox.text = "공격력 " + thisWeaponEffect.ToString("f1") + "% 증가";
        /// 버튼 갱신
        SetGoobapBtn(thisWeaponCost);
    }

    /// <summary>
    /// 소모 계산 완료된 국밥량
    /// </summary>
    string posibleGupbap;

    /// <summary>
    /// 버튼 정보 새로 고침
    /// </summary>
    /// <param name="_somo"></param>
    /// <returns></returns>
    void SetGoobapBtn(string _somo)
    {
        /// 다이아로 구매 완료 했다.
        if (pm.thisDiaBuyWeapArray[thisIndex] != "0")
        {
            SetMaxPunch(thisIndex);
        }

        /// 100렙은 넘었는데 구매 안했다.
        else if(thisWeaponLevel >= 100)
        {
            MaxButton.SetActive(false); // 노란 이미지
            somoIcon.sprite = pm.DiaImg;
            UpgradeBox.text = ((thisIndex + 1) * 100).ToString();
        }

        /// 99렙 밑으로 국밥으로 강화중이다
        else
        {
            ///버튼 아이콘 국밥 or 다이아
            somoIcon.sprite = pm.GookBapImg;
            UpgradeBox.text = UserWallet.GetInstance().SeetheNatural(dts.PanByulGi(_somo));
            posibleGupbap = dts.SubStringDouble(PlayerPrefsManager.GetInstance().gupbap, _somo);
            /// 재화 소모 가능한지
            if (posibleGupbap != "-1")
            {
                MaxButton.SetActive(false);
            }
            /// ㄴㄴ 업글 불가
            else
            {
                MaxButton.SetActive(true);
            }
        }

    }

    /// <summary>
    /// 다이아로 구매 완료하면 버튼 회색 MAX 글자 처리.
    /// </summary>
    /// <param name="p_index"></param>
    public void SetMaxPunch(int p_index)
    {
        if (thisIndex != p_index) return;

        MaxButton.SetActive(true); // Cover_Btn
        somoIcon.sprite = pm.DiaImg;
        UpgradeBox.text = "MAX";
    }


    #endregion

    #region [정보 갱신] 변동 정보 새로고침


    /// <summary>
    /// 외부에서 클릭하는 장착 버튼
    /// </summary>
    public void ClickedEquipBtn()
    {
        /// 회색 이미지 활성화라면 리턴
        if (EquipButton.activeSelf) return;

        /// 0 렙이면 장착하지말고 리턴
        if (thisWeaponLevel == 0 && thisIndex != 0)
        {
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("레벨이 0인 장비는 장착할 수 없습니다.");
            return;
        }

        /// 펀치 불렛 바꿔주기.
        pm.ChangePunch(thisIndex);
        /// 이전 [장착 버튼] 새로고침
        pm.RefreshEuipGrayBtn();
    }

    // 훈련도구 가격
    string tmpPrice;


    /// <summary>
    /// 외부에서 클릭하는 훈련도구 강화 버튼
    /// </summary>
    public void ClickedUpgradeBtn()
    {
        /// 회색버튼이면 리턴
        if (MaxButton.activeSelf) return;
        /// 다이아 구매 || 국밥 구매 판별
        if (thisWeaponLevel >= 100 && pm.thisDiaBuyWeapArray[thisIndex] == "0")
        {
            /// 펀치 매니저에서 팝업 호출
            pm.ShowDiaPunchOkay(thisIndex);
            return;
        }

        /// 정상적 국밥 소모
        PlayerPrefsManager.GetInstance().gupbap = posibleGupbap;
        UserWallet.GetInstance().ShowUserMilk();
        //레벨업
        thisWeaponLevel++;
        // 근데 만렙이다?
        if (thisWeaponLevel >= 100)
        {
            thisWeaponLevel = 100;
        }
        /// 첫번재 무기라면 예외처리
        if (thisIndex == 0)
        {
            tmpPrice = (thisWeaponLevel + 10).ToString();
        }
        else
        {
            tmpPrice = (((thisWeaponLevel + 1) * (5 * thisIndex)) + (10 + (5 * thisIndex))).ToString();
        }

        /// 수치 변화 저장
        PlayerPrefsManager.GetInstance().weaponInfo[thisIndex].weaponLevel = thisWeaponLevel;
        PlayerPrefsManager.GetInstance().weaponInfo[thisIndex].weaponCost = dts.fDoubleToStringNumber(tmpPrice);
        PlayerPrefsManager.GetInstance().weaponInfo[thisIndex].weaponEffect = (thisWeaponLevel * (0.1f * (thisIndex + 1)));

        /// 퀘스트
        PlayerPrefsManager.GetInstance().questInfo[0].daily_Punch++;

        if (PlayerPrefsManager.GetInstance().questInfo[0].All_Punch < 1000)
        {
            PlayerPrefsManager.GetInstance().questInfo[0].All_Punch++;
        }

        /// 펀치 정보 저장
        PlayerPrefsManager.GetInstance().SaveWeaponInfo();

        /// 텍스트 갱신
        SetUpdateInfo();
    }







    /// <summary>
    /// 현재 장착 회색 커버 활성화
    /// true = 회색 / false = 노란색
    /// </summary>
    /// <param name="isSetActive"></param>
    public void SetAllEpuipBtnToGray()
    {
        /// 현재 장착 펀치라면 노란색
        if (thisIndex == PlayerPrefsManager.GetInstance().PunchIndex)
        {
            EquipButton.SetActive(true);
        }
        else
        {
            EquipButton.SetActive(false);
        }
    }



    #endregion



}
