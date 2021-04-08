using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PostBoxManager : MonoBehaviour
{
    [Header("- 우편 프리팹")]
    public Transform Cart;
    [Header("- 프리팹 추가할 트랜스폼")]
    public Transform Moderpos;


    /// <summary>
    /// 쿠폰 검증에 통과하면 우편함에 아이템 추가.
    /// </summary>
    /// <param name="_code"></param>
    /// <param name="_amount"></param>
    /// <param name="_uid"></param>
    public void AddPresent(string _code, string _amount, string _uid, string _message)
    {
        var eneObj = Cart;
        //프리팹에서 박스 생성
        Transform initBox = Lean.Pool.LeanPool.Spawn(eneObj);

        int mCnt = initBox.GetChild(0).childCount;

        initBox.SetParent(Moderpos); // 스크롤뷰 안쪽에 생성.
        initBox.localPosition = Vector3.zero; // 뒤틀리는거 방지
        initBox.localScale = new Vector3(1, 1, 1);
        
        for (int i = 0; i < mCnt; i++)
        {
            initBox.GetChild(0).GetChild(i).gameObject.SetActive(false);
        }

        switch (_code)
        {
            case "diamond": initBox.GetChild(0).GetChild(0).gameObject.SetActive(true); break;
            case "gold": initBox.GetChild(0).GetChild(1).gameObject.SetActive(true); break;
            case "key": initBox.GetChild(0).GetChild(2).gameObject.SetActive(true); break;
            case "gupbap": initBox.GetChild(0).GetChild(3).gameObject.SetActive(true); break;
            case "ssal": initBox.GetChild(0).GetChild(4).gameObject.SetActive(true); break;
            case "ticket": initBox.GetChild(0).GetChild(5).gameObject.SetActive(true); break;
                //
            case "kimchi": initBox.GetChild(0).GetChild(6).gameObject.SetActive(true); break;
            case "shield": initBox.GetChild(0).GetChild(7).gameObject.SetActive(true); break;
        }

        initBox.GetComponent<PostCart>().SetPostContent(_code, _amount, _uid, _message);
    }

    private void OnDisable()
    {
        while (Moderpos.childCount != 0)
        {
            Lean.Pool.LeanPool.Despawn(Moderpos.GetChild(0));
        }
    }
}
