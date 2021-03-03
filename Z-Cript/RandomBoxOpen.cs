using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomBoxOpen : MonoBehaviour
{
    DoubleToStringNum dts = new DoubleToStringNum();
    Coroutine AmimStart;

    //  활성화 되면 코루틴 시작.
    private void OnEnable()
    {
        AmimStart = StartCoroutine(BoxOpenAnim());
    }


    int index;
    int amout;
    /// <summary>
    /// 타이밍 맞춰서 애니메이션 재생하고 UI 파티클 뿌려주기.
    /// </summary>
    /// <returns></returns>
    IEnumerator BoxOpenAnim()
    {
        yield return null;
        var Salbap = PlayerPrefsManager.GetInstance().ssalbap;

        transform.GetChild(1).GetComponent<Animation>()["Box_open 1"].speed = 1;
        transform.GetChild(1).GetComponent<Animation>().Play("Box_open 1");

        yield return new WaitForSeconds(2.1f);

        if(index == 525)
        {
            bool isArtiComp =  GameObject.Find("GatChaManager").GetComponent<GatChaManager>().GatCha_Shop();
            // 유물 올 컴플릭트라면??
            if(isArtiComp)
            {
                int ranindex = Random.Range(0, 4);

                if(ranindex == 0) amout = Random.Range(1, 4);
                else if(ranindex == 1) amout = Random.Range(10, 101);
                else if(ranindex == 2) amout = Random.Range(100, 1001);
                else if(ranindex == 3) amout = Random.Range(50, 501);

                PopUpObjectManager.GetInstance().ShowMuganRewordpop(ranindex, amout);
                PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(Salbap, amout.ToString());
            }

        }
        else if (index == 625)
        {
            bool isArtiComp = GameObject.Find("GatChaManager").GetComponent<GatChaManager>().GatCha_Shop();

            if (isArtiComp)
            {
                int ranindex = Random.Range(0, 4);

                if (ranindex == 0) amout = Random.Range(1, 6);
                else if (ranindex == 1) amout = Random.Range(10, 301);
                else if (ranindex == 2) amout = Random.Range(100, 3001);
                else if (ranindex == 3) amout = Random.Range(50, 1501);

                PopUpObjectManager.GetInstance().ShowMuganRewordpop(ranindex, amout);
                PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(Salbap, amout.ToString());
            }

        }
        else if (index == 725)
        {
            bool isArtiComp = GameObject.Find("GatChaManager").GetComponent<GatChaManager>().GatCha_Shop();

            if (isArtiComp)
            {
                int ranindex = Random.Range(0, 4);

                if (ranindex == 0) amout = Random.Range(1, 11);
                else if (ranindex == 1) amout = Random.Range(10, 501);
                else if (ranindex == 2) amout = Random.Range(100, 5001);
                else if (ranindex == 3) amout = Random.Range(50, 2501);

                PopUpObjectManager.GetInstance().ShowMuganRewordpop(ranindex, amout);
                PlayerPrefsManager.GetInstance().ssalbap = dts.AddStringDouble(Salbap, amout.ToString());
            }

        }
        else
        {
            PopUpObjectManager.GetInstance().ShowMuganRewordpop(index, amout);
        }

        gameObject.SetActive(false);
        UserWallet.GetInstance().ShowAllMoney();

    }

    /// <summary>
    /// 외부에서 호출
    /// </summary>
    /// <param name="_index"></param>
    /// <param name="_amout"></param>
    public void RandoBoxInit(int _index, int _amout)
    {
        gameObject.SetActive(true);

        index = _index;
        amout = _amout;
    }


}
