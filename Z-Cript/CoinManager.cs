using System.Collections;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    bool isBanjun;
    public void CoinInit()
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
            DestroyCoin();
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
        Invoke(nameof(DestroyBAP), 1.5f);
    }

    void DestroyBAP()
    {
        // 힘 초기화.
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().useGravity = false;

        Lean.Pool.LeanPool.Despawn(gameObject);
    }


    public void KimchiInit()
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

        Transform CoinTf = PlayerPrefsManager.GetInstance().KimchiPos;
        /// 스크린 좌표를 월드 좌표로 변화 한다.
        CoinPos = Camera.main.ScreenToWorldPoint(CoinTf.position);
        moveRoutine = StartCoroutine(KimchiProgress());
    }


    /// <summary>
    /// 코인 흡수 메소드
    /// </summary>
    /// <returns></returns>
    private IEnumerator KimchiProgress()
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
        /// 김치 재화 획득

        var tmpGold = PlayerPrefsManager.GetInstance().Kimchi;
        ///
        /// 1d 대신 김치 유물 추가 할 것
        ///
        var result = dts.multipleStringDouble(PlayerPrefsManager.GetInstance().PlayerDPS, 1d);
        PlayerPrefsManager.GetInstance().Kimchi = dts.AddStringDouble(double.Parse(tmpGold), double.Parse(result));

        UserWallet.GetInstance().ShowUserKimchi();

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

    void DestroyCoin()
    {
        Transform CoinTf = PlayerPrefsManager.GetInstance().CoinPos;
        /// 스크린 좌표를 월드 좌표로 변화 한다.
        CoinPos = Camera.main.ScreenToWorldPoint(CoinTf.position);

        moveRoutine = StartCoroutine(Progress());
    }

    readonly DoubleToStringNum dts = new DoubleToStringNum();

    /// <summary>
    /// 코인 흡수 메소드
    /// </summary>
    /// <returns></returns>
    private IEnumerator Progress()
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
        ///골드 획득  계산식 수정 > 골드 획득량 = 맷집 * 1 * (유니폼 + 스킬 + 유물 + 훈련장)
        var tmpDps = PlayerPrefsManager.GetInstance().Mat_Mattzip;
        /// 훈련장 골드 버프
        double goldPer = PlayerPrefsManager.GetInstance().BG_CoinStat;
        double artiGoldPer = 1d * (
            // 골드 증가 유물
            PlayerPrefsManager.GetInstance().Arti_GoldPer +
            //유니폼 골드증가
            PlayerPrefsManager.GetInstance().uniformInfo[1].Uniform_LV +
            PlayerPrefsManager.GetInstance().uniformInfo[2].Uniform_LV +
            // 캐릭터 스킬 골드 증가
            PlayerPrefsManager.GetInstance().uniformInfo[3].Skill_LV);

        // 크리티컬 이라면?
        //if (_isCritic) tmpDps = PlayerPrefsManager.GetInstance().CriticalDPS;

        var result = "0";
        /// VIP 선물 상자라면?
        if (transform.childCount == 1)
        {
            ///골드 획득  계산식 수정 > 골드 획득량 = 맷집 X (유니폼 + 스킬 + 유물 + 훈련장)
            //result = dts.multipleStringDouble(tmpDps, 10d *  2d * (goldPer + (artiGoldPer * 0.01d)));
            result = dts.multipleStringDouble(tmpDps, 5d * (goldPer + (artiGoldPer * 0.01d)));
        }
        else
        {
            /// 동영상 보고 [코인 3배 버프] 적용 받으면? -> 2배로 수정
            if (PlayerPrefsManager.GetInstance().isGoldTriple)
            {
                result = dts.multipleStringDouble(tmpDps, 2d * (goldPer + (artiGoldPer * 0.01d)));
                //Debug.Log("동영상 세배: " + result);
            }
            else // 아무 버프 없을 때.
            {
                result = dts.multipleStringDouble(tmpDps, (goldPer + (artiGoldPer * 0.01d)));
                //Debug.Log("아무 버프 없 : "+result);
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
