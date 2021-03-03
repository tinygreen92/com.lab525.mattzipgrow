using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUPObjectLinker : MonoBehaviour
{
    PopUpObjectManager popupManager;

    /// <summary>
    /// 선물 상자 터치할때 
    /// </summary>
    public void BoxClick()
    {
        ////자동공격이면 상장 클릭해도 리턴 
        //if (PlayerPrefsManager.GetInstance().VIP == 525 || PlayerPrefsManager.GetInstance().VIP == 625 || PlayerPrefsManager.GetInstance().VIP == 725 || PlayerPrefsManager.GetInstance().VIP == 825)
        //{
        //    return;
        //}

        // 텍스트 박스 글 내용 채워줌
        UserWallet.GetInstance().Get_5_Gold();
        //오브젝트 가지고 와서
        popupManager = GameObject.Find("PopUpObjectManager").GetComponent<PopUpObjectManager>();
        // 열리는 애니메이션
        popupManager.ShowLuckyBoxProcess();
        // 그리고 클릭당한 박스 삭제
        GetComponentInParent<CoinManager>().BoxDelete();
    }
}
