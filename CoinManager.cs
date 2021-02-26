using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    bool isBanjun;
    public void CoinInit(bool _isCritic)
    {
        // 로테이션 각도
        float RanRotat = Random.Range(-3.5f, 3.5f);
        // 힘 초기화.
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().useGravity = true;

        if (isBanjun)
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(RanRotat, 20, 0), ForceMode.Impulse);
            isBanjun = !isBanjun;
        }
        else
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(-RanRotat, 20, 0), ForceMode.Impulse);
            isBanjun = !isBanjun;
        }

        /// 자식 있는 건 LuckyBox일 경우 해당
        if (transform.childCount == 1 && PlayerPrefsManager.GetInstance().VIP == 0)
        {
            DestroyBox();
        }
        else if (transform.childCount == 1 && !PlayerPrefsManager.GetInstance().isAutoAtk)
        {
            DestroyBox();
        }
        else
        {
            DestroyCoin(_isCritic);
        }

        
    }

    public void BAPInit()
    {
        // 로테이션 각도
        float RanRotat = Random.Range(-3.5f, 3.5f);
        // 힘 초기화.
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().useGravity = true;

        if (isBanjun)
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(RanRotat, 20, 0), ForceMode.Impulse);
            isBanjun = !isBanjun;
        }
        else
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(-RanRotat, 20, 0), ForceMode.Impulse);
            isBanjun = !isBanjun;
        }
        Invoke("DestroyBAP", 1.5f);
    }

    void DestroyBAP()
    {
        // 힘 초기화.
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().useGravity = false;

        Lean.Pool.LeanPool.Despawn(gameObject);
    }



    Coroutine boxRoutine;

    /// <summary>
    /// 코인이 아니고 박스 지워짐 처리 시작
    /// </summary>
    private void DestroyBox()
    {
        boxRoutine = StartCoroutine(Boxing());
    }

    private IEnumerator Boxing()
    {
        float RanRotat = Random.Range(-1f, -2f);

        while (transform.position.y > RanRotat)
        {
            yield return null;
        }

        // 번쩍 번쩍
        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);

        // 힘 초기화.
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().useGravity = false;

        yield return new WaitForSeconds(10);

        BoxDelete();
    }

    public void BoxDelete()
    {
        if (boxRoutine != null)
        {
            StopCoroutine(boxRoutine);
            boxRoutine = null;
            transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            Lean.Pool.LeanPool.Despawn(gameObject);
        }
    }

    Coroutine moveRoutine;
    Vector3 CoinPos;

    void DestroyCoin(bool _isCritic)
    {
        Transform CoinTf = PlayerPrefsManager.GetInstance().CoinPos;
        /// 스크린 좌표를 월드 좌표로 변화 한다.
        CoinPos = Camera.main.ScreenToWorldPoint(CoinTf.position);

        moveRoutine = StartCoroutine(Progress(_isCritic));
    }

    DoubleToStringNum dts = new DoubleToStringNum();

    /// <summary>
    /// 코인 흡수 메소드
    /// </summary>
    /// <returns></returns>
    private IEnumerator Progress(bool _isCritic)
    {
        float rate = 1f;
        float progress = 0.0f;
        float RanRotat = Random.Range(-2f, -3f);

        if (transform.childCount == 1)
        {
            RanRotat = Random.Range(-1f, -2f);
        }

        while (transform.position.y > RanRotat)
        {
            yield return null;
        }

        // 힘 초기화.
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().useGravity = false;

        yield return new WaitForSeconds(1);

        //골드 획득
        var tmpGold = PlayerPrefsManager.GetInstance().gold;
        var tmpDps = PlayerPrefsManager.GetInstance().PlayerDPS;
        var goldPer = PlayerPrefsManager.GetInstance().BG_CoinStat;
        float artiGoldPer = (PlayerPrefsManager.GetInstance().Arti_GoldPer * 1.0f) +
            PlayerPrefsManager.GetInstance().uniformInfo[1].Uniform_LV * 1.0f +
            PlayerPrefsManager.GetInstance().uniformInfo[2].Uniform_LV * 1.0f +
            (PlayerPrefsManager.GetInstance().uniformInfo[3].Skill_LV * 0.5f);

        /// 크리티컬 이라면?
        if (_isCritic) tmpDps = PlayerPrefsManager.GetInstance().CriticalDPS;

        //Debug.LogError("PunchDPS : " + tmpDps);

        var result = "0";
        /// VIP 선물 상자라면? 10배
        if (transform.childCount == 1)
        {
            result = dts.multipleStringDouble(tmpDps, 10d *  2d * (goldPer + (artiGoldPer * 0.01d)));
        }
        else
        {
            /// 동영상 보고 [코인 3배 버프] 적용 받으면? -> 2배로 수정
            if (PlayerPrefsManager.GetInstance().isGoldTriple)
            {
                result = dts.multipleStringDouble(tmpDps, 2d * 2d * (goldPer + (artiGoldPer * 0.01d)));
                //Debug.Log("동영상 세배: " + result);
            }
            else // 아무 버프 없을 때.
            {
                /// 기본 배수 2배
                result = dts.multipleStringDouble(tmpDps, 2d * (goldPer + (artiGoldPer * 0.01d)));
                //Debug.Log("아무 버프 없 : "+result);
            }
        }


        /// 보험용 골드 증가. 스택 500번 쌓으면 증가.
        string SameSame = dts.AddStringDouble(double.Parse(tmpGold), double.Parse(result));
        string stackGold = PlayerPrefsManager.GetInstance().gold;

        if (SameSame == stackGold)
        {
            PlayerPrefsManager.GetInstance().GoldStack++;

            //Debug.LogWarning(" 골드 스택 : " + PlayerPrefsManager.GetInstance().GoldStack);

            if (PlayerPrefsManager.GetInstance().GoldStack > 5000)
            {
                PlayerPrefsManager.GetInstance().GoldStack = 0;

                // 실제 골드 창에 표기 되는거.
                string panda = dts.fDoubleToGoldOutPut(stackGold);
                string[] sNumberList = panda.Split('.');
                // sNumberList[0] 길이에 따라.
                int pandaZzang = sNumberList[0].Length;
                double rawGold = 1;
                if (pandaZzang == 1)
                {
                    // 골드값 더블로 받음
                    rawGold = (double.Parse(stackGold) * 0.001d);
                }
                else if (pandaZzang == 2)
                {
                    // 골드값 더블로 받음
                    rawGold = (double.Parse(stackGold) * 0.0001d);
                }
                else
                {
                    // 골드값 더블로 받음
                    rawGold = (double.Parse(stackGold) * 0.00001d);
                }

                //Debug.LogWarning(" 골드 rawGold : " + rawGold);

                result = rawGold.ToString("f0");
                var dak = stackGold.Substring(0,1);

                //Debug.LogWarning(" 골드 dak : " + dak);

                rawGold = rawGold / double.Parse(dak);
                result = rawGold.ToString("f0");

                //Debug.LogWarning(" rawGold rawGold : " + rawGold);


                //Debug.LogWarning("rawGold : " + rawGold);
                //
            }
        }

        //Debug.LogWarning("result " + result);
        PlayerPrefsManager.GetInstance().gold = dts.AddStringDouble(double.Parse(tmpGold), double.Parse(result));

        //Debug.Log("기존 골드 : " + double.Parse(tmpGold));
        //Debug.Log("골드 더해줌 : " + double.Parse(result));

        PopUpObjectManager.GetInstance().GettingGoldMessage(double.Parse(result));

        UserWallet.GetInstance().ShowUserGold();

        while (progress <= 1f)
        {
            yield return new WaitForFixedUpdate();

            var movingy = Mathf.Lerp(transform.position.y, CoinPos.y, progress);
            var movingx = Mathf.Lerp(transform.position.x, CoinPos.x, progress);


            transform.position = new Vector3(movingx, movingy, 7);

            progress += rate * Time.deltaTime;
        }


        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
            moveRoutine = null;


            Lean.Pool.LeanPool.Despawn(gameObject);
        }

    }

}
