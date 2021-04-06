using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldItem : MonoBehaviour
{
    readonly DoubleToStringNum dts = new DoubleToStringNum();

    public ShieldManager pm;
    [Header("- 회색 커버 오브젝트")]
    public GameObject GrayImage;

    [Header("- 아이콘")]
    public Image spriteBox;

    [Header("- 정보 표기 부분")]
    public Text NameBox;
    public Text LevelBox;
    public Text DescBox; // 장착 방어력
    public Text ownedBox; // 보유 방어력
    public Text fussionBox; // 합성 확률 텍스트 박스 강화 성공 확률 99.5%
    //[Space]
    //public Text rankUpBox;  // 합성 ?

    [Header("- 버튼 부분")]
    public GameObject EquipButton;
    public GameObject MaxButton;
    public Text UpgradeBox;



    /// <summary>
    /// 펀치인덱스 0~99 
    /// </summary>
    int thisIndex;
    int thisShieldLevel;
    string thisShieldCost;
    float thisShieldEffect;
    float thisOwnedEffect;

    float thisSuccedFussion;

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
    internal void SetDefaltInfo()
    {
        spriteBox.sprite = pm.shieldImgs[thisIndex];
        NameBox.text = pm.shieldNames[thisIndex];
        /// 회색 커버
        SetAllGrayCover(thisIndex);
        /// 장착 여부 버튼
        SetAllEpuipBtnToGray();
    }

    /// <summary>
    /// 최초 획득시 해금
    /// </summary>
    public void SetAllGrayCover(int p_index)
    {
        if (thisIndex != p_index) return;

        /// isUnlock 이면 전체 회색 화면 False
        if (PlayerPrefsManager.GetInstance().shieldInfo[thisIndex].isUnlock)
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
        var tmpList = PlayerPrefsManager.GetInstance().shieldInfo[thisIndex];

        thisShieldLevel = tmpList.shieldLevel;
        thisShieldEffect = thisShieldLevel < 1 ? 0 : tmpList.equipedEffect + (tmpList.upperEfect * thisShieldLevel);
        thisShieldCost = dts.multipleStringDouble(tmpList.shieldCost, (1 + 0.3f * thisShieldLevel));
        thisOwnedEffect = thisShieldLevel < 1 ? 0 : tmpList.ownedEffect;
        thisSuccedFussion = tmpList.powerUpper - (tmpList.powerMinusPer * thisShieldLevel);

        LevelBox.text = "Lv. " + thisShieldLevel;
        DescBox.text = "장착 방어력 " + thisShieldEffect.ToString("f1") + "% 증가";
        ownedBox.text = "보유 방어력 " + thisOwnedEffect.ToString("f1") + "% 증가";
        /// 합성 확률
        fussionBox.text = "강화 성공 확률 " + thisSuccedFussion.ToString("f1") + "%";
        /// 버튼 갱신
        SetGoobapBtn(dts.fDoubleToStringNumber(thisShieldCost));
    }

    /// <summary>
    /// 소모 계산 완료된 깍두기
    /// </summary>
    string posibleKimchi;

    /// <summary>
    /// 버튼 정보 새로 고침
    /// </summary>
    /// <param name="_somo"></param>
    /// <returns></returns>
    void SetGoobapBtn(string _somo)
    {

        /// 100렙 넘음
        if (thisShieldLevel >= 100)
        {
            SetMaxPunch(thisIndex);
        }
        /// 99렙 밑으로 국밥으로 강화중이다
        else
        {
            UpgradeBox.text = UserWallet.GetInstance().SeetheNatural(dts.PanByulGi(_somo));
            posibleKimchi = dts.SubStringDouble(PlayerPrefsManager.GetInstance().Kimchi, _somo);
            /// 재화 소모 가능한지 버튼 세팅
            if (posibleKimchi != "-1")
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
    /// 현재 재화가 업글 가능하면 버튼 변경
    /// </summary>
    internal void BuyBtnBye(string _MyKimchi)
    {
        var _somo = dts.fDoubleToStringNumber(dts.multipleStringDouble(thisShieldCost, (1 + 0.3f * thisShieldLevel)));
        
        if (dts.SubStringDouble(_MyKimchi, _somo) != "-1")
        {
            MaxButton.SetActive(false);
        }
        /// ㄴㄴ 업글 불가
        else
        {
            MaxButton.SetActive(true);
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
        UpgradeBox.text = "MAX";
    }


    #endregion

    #region [정보 갱신] 변동 정보 새로고침


    /// <summary>
    /// 외부에서 클릭하는 장착 버튼
    /// </summary>
    public void ClickedEquipBtn()
    {
        /// 회색 이미지 활성화라면 = (이미 장착상태다) 리턴
        if (EquipButton.activeSelf) 
            return;

        /// 0 렙이면 장착하지말고 리턴 (이런 경우는 없지만)
        if (thisShieldLevel == 0 && thisIndex != 0)
        {
            PopUpObjectManager.GetInstance().ShowWarnnigProcess("레벨이 0인 장비는 장착할 수 없습니다.");
            return;
        }

        /// TODO : 장착 누르면 방어력 적용
        PlayerPrefsManager.GetInstance().ShieldIndex = thisIndex + 1;
        PlayerPrefsManager.GetInstance().Defence_Shiled = thisShieldEffect.ToString("f1");

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

        /// 정상적 깍두기 소모
        PlayerPrefsManager.GetInstance().Kimchi = posibleKimchi;
        UserWallet.GetInstance().ShowUserMilk();
        //레벨업
        thisShieldLevel++;
        // 근데 만렙이다?
        if (thisShieldLevel >= 100)
        {
            thisShieldLevel = 100;
        }

        tmpPrice = PlayerPrefsManager.GetInstance().shieldInfo[thisIndex].shieldCost;
        /// 0렙은 기본 값으로
        if (thisShieldLevel > 0) tmpPrice = dts.multipleStringDouble(tmpPrice, (1 + 0.3f * thisShieldLevel));


        /// 수치 변화 저장
        PlayerPrefsManager.GetInstance().shieldInfo[thisIndex].shieldLevel = thisShieldLevel;
        
        
        ///// 퀘스트
        //PlayerPrefsManager.GetInstance().questInfo[0].daily_Punch++;

        //if (PlayerPrefsManager.GetInstance().questInfo[0].All_Punch < 1000)
        //{
        //    PlayerPrefsManager.GetInstance().questInfo[0].All_Punch++;
        //}

        /// 정보 저장
        PlayerPrefsManager.GetInstance().SaveShieldInfo();

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
        /// 현재 장착 방패라면 노란색
        if (thisIndex == PlayerPrefsManager.GetInstance().ShieldIndex)
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
