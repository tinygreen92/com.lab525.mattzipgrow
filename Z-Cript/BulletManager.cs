using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BulletManager : MonoBehaviour
{
    //public Sprite NormalAtk;
    //public Sprite CriticalAtk;

    float bulletSpeed = 30;

    public Rigidbody thisRigidbody;
    public CapsuleCollider thisCollider;

    bool isBap;
    public void BapInit()
    {
        isBap = true;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        if (transform.childCount == 3) transform.GetChild(2).gameObject.SetActive(false);
        //transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = NormalAtk;

        thisRigidbody.isKinematic = false;
        // 힘 초기화.
        thisRigidbody.velocity = Vector3.zero;
        thisRigidbody.angularVelocity = Vector3.zero;
        // 각도 크기 설정
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        float PunScale = Random.Range(0.8f, 1.3f);

        Vector3 bull_Scale = new Vector3(PunScale, PunScale, 1f);

        transform.localScale = bull_Scale;

        thisRigidbody.AddForce(new Vector3(-10, 25, 0), ForceMode.Impulse);
    }

    public void BulletInit()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        if (transform.childCount == 3) transform.GetChild(2).gameObject.SetActive(false);

        thisRigidbody.isKinematic = false;
        thisCollider.enabled = true;
        thisCollider.isTrigger = true;
        // 힘 초기화.
        thisRigidbody.velocity = Vector3.zero;
        thisRigidbody.angularVelocity = Vector3.zero;
        // 각도 크기 설정
        Init_RotationScale();


    }

    Coroutine spinning;


    /// <summary>
    /// 젠되는 파이어볼 타입
    /// </summary>
    enum FireType
    {
        Normal,
        Strong,
        Heal
    }
    /// <summary>
    /// 평타 파이어볼 / 스트롱 볼  대미지 계산해서 가져옴
    /// 스트롱 볼은 평타의 1.5배 대미지
    /// 힐링볼은 평타의 0.5배 힐링
    /// </summary>
    string GetFireBall(int stage, FireType type)
    {
        double tmp = 0;

        switch (type)
        {
            case FireType.Normal:
                if (stage == 1)
                {
                    tmp = 100;
                }
                else
                {
                    tmp = 100d * System.Math.Pow(1.3d, stage);
                }
                break;
            case FireType.Strong:
                if (stage == 1)
                {
                    tmp = 100;
                }
                else
                {
                    tmp = 100d * System.Math.Pow(1.3d, stage);
                }
                break;
            case FireType.Heal:
                if (stage == 1)
                {
                    tmp = 100;
                }
                else
                {
                    tmp = 100d * System.Math.Pow(1.3d, stage);
                }
                break;
            default:
                break;
        }

        return tmp.ToString("F0");
    }

    /// <summary>
    /// 호출할때 회전각 / 스케일 배정
    /// </summary>
    void Init_RotationScale()
    {
        // 로테이션 각도
        float RanRotat = UnityEngine.Random.Range(-8.0f, 8.0f); // 리스트에 올려둔 위치값.
        //각도 설정
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, RanRotat));


        float PunScale = Random.Range(0.7f, 1.5f);

        Vector3 bull_Scale = new Vector3(PunScale, PunScale, 1f);

        transform.localScale = bull_Scale;

        spinning = StartCoroutine(SpinPunch());

        if (name == "ArmBall" || name == "ArStrongBall" || name == "AsHealBall")
        {
            float mutop = PlayerPrefsManager.GetInstance().MaxGet_MuganTop;
            bulletSpeed = 15f + (mutop * 0.1f);
        }
        else
        {
            bulletSpeed = 30f;
        }

        thisRigidbody.AddForce(transform.right * bulletSpeed, ForceMode.Impulse);
    }

    /// <summary>
    /// 펀치 4번 이상 부터 회전.
    /// </summary>
    /// <returns></returns>
    IEnumerator SpinPunch()
    {
        if (name == "PunchPrefab 0" || name == "PunchPrefab 1" || name == "PunchPrefab 2" || name == "PunchPrefab 3" 
            || name == "ArmBall" || name == "ArStrongBall" || name == "AsHealBall" )
        {
            goto HELL;
        }

        float rotation = 0f;
        while (true)
        {
            //계속 회전 되라
            transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
            rotation -= 20f;
            yield return new WaitForFixedUpdate();
        }

    HELL:
        yield return null;
    }

    /// <summary>
    /// 펀치 4번 이상 부터 회전.
    /// </summary>
    /// <returns></returns>
    IEnumerator SpinPunch0()
    {
        if (name == "PunchPrefab 0" || name == "PunchPrefab 1" || name == "PunchPrefab 2" || name == "PunchPrefab 3"
            || name == "ArmBall" || name == "ArStrongBall" || name == "AsHealBall")
        {
            goto HELL;
        }

        float rotation = 0f;
        while (true)
        {
            //계속 회전 되라
            transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
            rotation += 20f;
            yield return new WaitForFixedUpdate();
        }

    HELL:
        yield return null;
    }


    public void BulletInit0()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);

        if (transform.childCount == 3) transform.GetChild(2).gameObject.SetActive(false);

        thisRigidbody.isKinematic = false;
        thisCollider.enabled = true;
        thisCollider.isTrigger = true;

        // 힘 초기화.
        thisRigidbody.velocity = Vector3.zero;
        thisRigidbody.angularVelocity = Vector3.zero;

        // 각도 크기 설정
        Init_RotationScale0();

    }

    /// <summary>
    /// 호출할때 회전각 / 스케일 배정
    /// </summary>
    void Init_RotationScale0()
    {
        // 랜덤 초기화
        float temp = Time.time * 100f;
        Random.InitState((int)temp);
        // 로테이션 각도
        float RanRotat = Random.Range(-8.0f, 8.0f); // 리스트에 올려둔 위치값.
        //각도 설정
        transform.rotation = Quaternion.Euler(new Vector3(0, 180, RanRotat));

        float PunScale = Random.Range(0.7f, 1.5f);

        Vector3 bull_Scale = new Vector3(PunScale, PunScale, 1f);

        //Debug.LogWarning(bull_Scale.ToString());

        transform.localScale = bull_Scale;

        spinning = StartCoroutine(SpinPunch0());

        if (name == "ArmBall" || name == "ArStrongBall" || name == "AsHealBall")
        {
            float mutop = PlayerPrefsManager.GetInstance().MaxGet_MuganTop;
            bulletSpeed = 15f + (mutop * 0.1f);
        }
        else
        {
            bulletSpeed = 30f;
        }

        thisRigidbody.AddForce(transform.right * bulletSpeed, ForceMode.Impulse);
    }


    /// <summary>
    /// 콜라이더 충돌 처리
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !PlayerPrefsManager.GetInstance().isInfinity)
        {

            AudioManager.instance.Btn_hit();

            if (isBap)
            {
                Lean.Pool.LeanPool.Despawn(gameObject, 0.1f);
                isBap = false;
                return;
            }

            if (PlayerPrefsManager.GetInstance().isGroggy)
            {
                Lean.Pool.LeanPool.Despawn(gameObject, 0.1f);
                return;
            }

            /// TODO : 타격 처리.
            thisRigidbody.isKinematic = true;
            thisCollider.isTrigger = false;

            // 바디 쉐이크
            other.GetComponentInParent<CameraShaker>().Shake();
            // 배경 쉐이크
            other.transform.parent.parent.GetChild(0).GetComponent<CameraShaker>().Shake();

            transform.GetChild(0).gameObject.SetActive(false);
            other.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 128 / 255f);

            float RanDam = Random.Range(0f, 100f);
            bool isCriiiiiiiii = false;
            /// 크리티컬 발동
            if (RanDam < float.Parse(PlayerPrefsManager.GetInstance().Critical_Per))
            {
                //transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = CriticalAtk;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                if (transform.childCount == 3) transform.GetChild(2).gameObject.SetActive(false);
                isCriiiiiiiii = true;
            }
            /// 일반 타격
            else
            {
                transform.GetChild(1).gameObject.SetActive(true);
            }
            /// 몸통 번쩍거리기
            StartCoroutine(WitheMan(other, isCriiiiiiiii));
            /// 총알 돌아가는 코루틴 정지.
            StopCoroutine(spinning);
        }
        else if (other.gameObject.tag == "Defence")
        {
            AudioManager.instance.Btn_hit();

            /// TODO : 타격 처리.
            thisRigidbody.isKinematic = true;
            thisCollider.isTrigger = false;

            // 바디 쉐이크
            other.GetComponentInParent<CameraShaker>().Shake();
            // 배경 쉐이크
            other.transform.parent.parent.GetChild(0).GetComponent<CameraShaker>().Shake();

            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);

            other.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 128 / 255f);

            StartCoroutine(DefenMan(other));

            StopCoroutine(spinning);

        }
        else if (other.gameObject.tag == "infiniti")
        {
            AudioManager.instance.Btn_hit();

            if (PlayerPrefsManager.GetInstance().isGroggy)
            {
                Lean.Pool.LeanPool.Despawn(gameObject, 0.1f);
                return;
            }

            /// TODO : 타격 처리.
            thisRigidbody.isKinematic = true;
            thisCollider.isTrigger = false;

            // 바디 쉐이크
            other.GetComponentInParent<CameraShaker>().Shake();
            // 배경 쉐이크
            other.transform.parent.parent.GetChild(0).GetComponent<CameraShaker>().Shake();

            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);

            other.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 128 / 255f);

            StopCoroutine(spinning);

            //코인말고 국밥  생성
            coin = Lean.Pool.LeanPool.Spawn(PlayerPrefsManager.GetInstance().BapOB, transform.position, transform.rotation);
            coin.GetComponent<CoinManager>().BAPInit();

            other.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0);

            transform.GetChild(1).gameObject.SetActive(false);
            if (transform.childCount == 3) transform.GetChild(2).gameObject.SetActive(false);

            Lean.Pool.LeanPool.Despawn(gameObject, 0.01f);

            if (PlayerPrefsManager.GetInstance().isInfinityEnd) return;

            // 체력 게이지 DOWN  (피버랑 맷집 안오름)
            Infini_HP_Calculate(other);
        }
        else if (other.gameObject.tag == "MuganShiled")
        {
            if (PlayerPrefsManager.GetInstance().isMuGanTopEnd)
            {
                Lean.Pool.LeanPool.Despawn(gameObject, 0.05f);
                return;
            }
            /// 사운드
            AudioManager.instance.Mugan_Block();
            /// TODO : 그로기 말고 종료 플래그로
            if (PlayerPrefsManager.GetInstance().isGroggy)
            {
                Lean.Pool.LeanPool.Despawn(gameObject, 0.1f);
                return;
            }
            // 배경 쉐이크
            other.GetComponent<CameraShaker>().Shake();
            /// TODO : 타격 처리.
            //thisRigidbody.isKinematic = true;
            thisCollider.enabled = false;
            /// 실드  몸 번쩍
            other.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 128 / 255f);
            /// 파이어볼 로테이션
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            Vector3 direction = transform.position - other.gameObject.transform.position;
            direction = direction.normalized * 1000;
            // 팅겨 내봐.
            thisRigidbody.AddForce(direction);

            StartCoroutine(MuGanBossMan(other));

        }
        else if (other.gameObject.tag == "Mugan_Body")
        {
            if (PlayerPrefsManager.GetInstance().isMuGanTopEnd)
            {
                Lean.Pool.LeanPool.Despawn(gameObject, 0.05f);
                return;
            }

            AudioManager.instance.Mugan_Hit();

            /// TODO : 타격 처리.
            thisRigidbody.isKinematic = true;
            thisCollider.isTrigger = false;
            thisCollider.enabled = false;

            // 바디 쉐이크
            other.GetComponentInParent<CameraShaker>().Shake();
            // 배경 쉐이크
            other.transform.parent.parent.GetChild(0).GetComponent<CameraShaker>().Shake();

            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);

            // 번쩍
            other.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 128 / 255f);

            StartCoroutine(MuGanMan(other));
        }
        else if (other.gameObject.tag == "PVP_Body")
        {
            if (PlayerPrefsManager.GetInstance().isPVPtoEnd)
            {
                Lean.Pool.LeanPool.Despawn(gameObject, 0.05f);
                return;
            }

            AudioManager.instance.Btn_hit();

            /// TODO : 타격 처리.
            thisRigidbody.isKinematic = true;
            thisCollider.isTrigger = false;
            thisCollider.enabled = false;

            // 바디 쉐이크
            other.GetComponentInParent<CameraShaker>().Shake();
            // 배경 쉐이크
            other.transform.parent.parent.GetChild(0).GetComponent<CameraShaker>().Shake();

            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);

            // 번쩍
            other.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 128 / 255f);

            StartCoroutine(PVPPVP(other));
        }
    }



    GameObject coin;
    GameObject kimchi;
    GameObject LucBox;

    /// <summary>
    /// 일반 게임 모드에서 맞음
    /// </summary>
    /// <param name="tmpC"></param>
    /// <returns></returns>
    IEnumerator WitheMan(Collider tmpC, bool _isCritic)
    {
        ///코인 프리팹
        coin = Lean.Pool.LeanPool.Spawn(PlayerPrefsManager.GetInstance().CoinOB, transform.position, Quaternion.identity);
        coin.GetComponent<CoinManager>().CoinInit();
        /// 김치 프리팹
        kimchi = Lean.Pool.LeanPool.Spawn(PlayerPrefsManager.GetInstance().kimchiCoin, transform.position, Quaternion.identity);
        kimchi.GetComponent<CoinManager>().KimchiInit();

        /// 체력 게이지 DOWN / 피버 게이지 UP // 주사위 굴림 - 선물상자 드랍
        HP_Calculate(tmpC, _isCritic);
        /// 맷집 게이지 증가
        Mat_100_Count(_isCritic);
        /// 스킬 게이지 증가
        Skill_500_Count(tmpC);

        yield return new WaitForSeconds(0.05f);

        // 선물상자 드랍
        DropTheLuckyBox();

        tmpC.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0);

        transform.GetChild(1).gameObject.SetActive(false);

        yield return new WaitForSeconds(0.15f);

        if (transform.childCount == 3) transform.GetChild(2).gameObject.SetActive(false);

        Lean.Pool.LeanPool.Despawn(gameObject, 1.5f);
    }

    /// <summary>
    /// 방어전 모드에서 맞음
    /// </summary>
    /// <param name="tmpC"></param>
    /// <returns></returns>
    IEnumerator DefenMan(Collider tmpC)
    {
        Defence_HP_Calculate(tmpC);

        yield return new WaitForSeconds(0.05f);

        tmpC.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        transform.GetChild(1).gameObject.SetActive(false);
        if (transform.childCount == 3) transform.GetChild(2).gameObject.SetActive(false);

        Lean.Pool.LeanPool.Despawn(gameObject, 2.0f);
    }



    /// <summary>
    /// 무한의 탑 모드에서 본체가 맞음
    /// </summary>
    /// <param name="tmpC"></param>
    /// <returns></returns>
    IEnumerator MuGanMan(Collider tmpC)
    {
        yield return null;

        /// 몸체맞고 체력 감소
        Mugan_HP_Calculate(tmpC);

        yield return new WaitForSeconds(0.05f);

        tmpC.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        transform.GetChild(1).gameObject.SetActive(false);
        //

        Lean.Pool.LeanPool.Despawn(gameObject, 1.0f);
    }
    /// <summary>
    /// 무한의 탑 모드에서 실드로 막음
    /// </summary>
    /// <param name="tmpC"></param>
    /// <returns></returns>
    IEnumerator MuGanBossMan(Collider tmpC)
    {
        yield return new WaitForSeconds(0.05f);

        tmpC.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        transform.GetChild(1).gameObject.SetActive(false);
        if (transform.childCount == 3) transform.GetChild(2).gameObject.SetActive(false);

        /// 보스 체력
        string maxBossHp = PlayerPrefsManager.GetInstance().MAX_boss_HP;
        string bossHp = PlayerPrefsManager.GetInstance().bossHP;
        /// 플레이어 체력
        string Mat_currentHP = PlayerPrefsManager.GetInstance().Mat_currentHP;
        string Mat_MaxHP = PlayerPrefsManager.GetInstance().Mat_MaxHP;
        /// 맷집력
        string Mattzip = UserWallet.GetInstance().GetMattzipForCul(PlayerPrefsManager.GetInstance().Mat_Mattzip);
        /// 순수 파이어볼 공격력
        string FireBall = GetFireBall(PlayerPrefsManager.GetInstance().MaxGet_MuganTop, FireType.Normal);
        /// 강력 파이어볼이라면 대미지 1.5배
        if (name == "ArStrongBall")
            FireBall = dts.multipleStringDouble(FireBall, 1.5d);
        else if (name == "AsHealBall")
        {
            FireBall = dts.multipleStringDouble(FireBall, 0.5d);

            Mat_currentHP = dts.AddStringDouble(Mat_currentHP, FireBall);
            Mugan_HP.transform.GetChild(0).GetComponent<Text>().text =
                UserWallet.GetInstance().SeetheNatural(double.Parse(Mat_currentHP)) + "/" +
                UserWallet.GetInstance().SeetheNatural(double.Parse(Mat_MaxHP));
            /// 최근 체력 갱신
            PlayerPrefsManager.GetInstance().Mat_currentHP = Mat_currentHP;
            /// 이미지 깔아줌
            Mugan_HP.fillAmount = (float)dts.DevideStringDouble(Mat_currentHP, Mat_MaxHP);
            goto HELL;
        }

        /// 맷집 뺀 공격력
        string PunchDPS = dts.SubStringDouble(FireBall, Mattzip);

        Debug.LogError("무한의 탑 파이어볼 공격력 " + PunchDPS);

        /// 만약 맷집이 공격력보다 더 높으면 반사댐 없다.
        if (PunchDPS == "-1")
        {
            bossHp = dts.SubStringDouble(bossHp, FireBall);
            Boss_HP.fillAmount = (float)dts.DevideStringDouble(bossHp, maxBossHp);
        }
        /// 반사 대미지 까지 받는다.
        else // 아니면 반사 댐 얼마.
        {
            //
            Mat_currentHP = dts.SubStringDouble(Mat_currentHP, PunchDPS);

            Mugan_HP.fillAmount = (float)dts.DevideStringDouble(Mat_currentHP, Mat_MaxHP);

            Mugan_HP.transform.GetChild(0).GetComponent<Text>().text =
                UserWallet.GetInstance().SeetheNatural(double.Parse(Mat_currentHP)) + "/" +
                UserWallet.GetInstance().SeetheNatural(double.Parse(Mat_MaxHP));

            /// 맷집만큼 보스 체력 깎음
            bossHp = dts.SubStringDouble(bossHp, Mattzip);
            Boss_HP.fillAmount = (float)dts.DevideStringDouble(bossHp, maxBossHp);
        }

        /// 현재 보스 체력 최신화
        PlayerPrefsManager.GetInstance().bossHP = bossHp;
        /// 플레이어 체력 최신화
        PlayerPrefsManager.GetInstance().Mat_currentHP = Mat_currentHP;

        /// 1순위 계산 플레이어 체력 0이 되었다!!
        if (Mat_currentHP == "0" || Mat_currentHP == "-1")
        {
            Mugan_HP.fillAmount = 0;
            Mugan_HP.transform.GetChild(0).GetComponent<Text>().text = "0/" + UserWallet.GetInstance().SeetheNatural(double.Parse(Mat_MaxHP));

            PlayerPrefsManager.GetInstance().isMuGanTopEnd = true;
            // 이미 이어하기를 썼다면 바로 종료
            if (PlayerPrefsManager.GetInstance().isSecondChan)
            {
                // 그만두기 버튼 눌러
                GameObject.Find("MUGANNOTOPManager").GetComponent<MuganMode>().EndBtnClicked();
            }
            else
            {
                /// TODO : 이어하기 팝업 호출.
                PopUpObjectManager.GetInstance().ShowMuganCountinue();
            }
        }
        /// 2순위 계산 보스 체력 0이 되었다!!
        else if (bossHp == "0" || bossHp == "-1")
        {
            Boss_HP.fillAmount = 0;
            // 펀치 그만 나오게.
            PlayerPrefsManager.GetInstance().isMuGanTopEnd = true;

            // 클리어!
            GameObject.Find("MUGANNOTOPManager").GetComponent<MuganMode>().ClearMuGanTop();
        }

        HELL:
        /// 오브젝트 풀링 해제
        Lean.Pool.LeanPool.Despawn(gameObject, 1.0f);
    }





    /// <summary>
    /// 기본 확률 1% 로 럭키박스 드랍. 
    /// </summary>
    private void DropTheLuckyBox()
    {
        // min 값 = 나올 확률
        float dropTable = Random.Range(0f, 100.0f);
        /// 황금 상자 확률 업
        if(PlayerPrefsManager.GetInstance().isEmptyLuckBox)
        {
            dropTable *= 0.1f;
        }
        //
        if (dropTable < PlayerPrefsManager.GetInstance().LuckyProb)
        {
            /// TODO : 박스 프리팹으로 교체좀
            LucBox = Lean.Pool.LeanPool.Spawn(PlayerPrefsManager.GetInstance().LucBox, transform.position, transform.rotation);
            LucBox.GetComponent<CoinManager>().CoinInit();
            /// 오토켰을 때?
            if (PlayerPrefsManager.GetInstance().isAutoAtk)
            {
                PlayerPrefsManager.GetInstance().tmm.ExUpdateMission(23); /// 미션 업데이트
            }

        }

    }


    /// <summary>
    /// 그로기 모드 돌입
    /// </summary>
    void GroggyModeOn(Collider tmpC)
    {
        GameObject.Find("GroggyManager").GetComponent<GroggyManager>().GroggyModeImageLock(true);

        tmpC.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0);

        PlayerPrefsManager.GetInstance().isGroggy = true;

        Transform hitBody = tmpC.transform.parent;
        // 타격 스프라이트 비활성화
        transform.GetChild(1).gameObject.SetActive(false);
        if (transform.childCount == 3) transform.GetChild(2).gameObject.SetActive(false);

        // 서있는 스프라이트 안보이게 꺼줌
        hitBody.GetComponent<SpriteRenderer>().enabled = false;
        // 그로기 애니메이션 재생
        hitBody.GetComponent<Animation>().Play("Groggy");





        //// 맷집력 000.00 숫자 감춰줌
        //BreadBtn.transform.parent.GetChild(1).gameObject.SetActive(false);
        //// 클릭 가능한 빵버튼 보여줌
        //if (PlayerPrefsManager.GetInstance().isFristGameStart)
        //{
        //    BreadBtn.gameObject.SetActive(true);
        //}
        //// 1번 클릭해준다
        //PlayerPrefsManager.GetInstance().TurtorialCount = 1000;
        //PopUpObjectManager.GetInstance().ShowWarnnigProcess("국밥을 먹여서 정신 차리게 하자!");

        GameObject.Find("GroggyManager").GetComponent<GroggyManager>().BreadBtnTouch();
    }





    DoubleToStringNum dts = new DoubleToStringNum();
    Image HP_Bar;               // 체력 바 filled
    Image MattzipGauge;         // 맷집게이지 이미지
    Image SkillGauge;           // 꾸짖을 갈 게이지
    Button BreadBtn;            // 빵 먹이는 버튼

    public void SetFillHP(Image hp, Image matt, Image skill, Button bbang)
    {
        HP_Bar = hp;
        MattzipGauge = matt;
        SkillGauge = skill;
        //
        BreadBtn = bbang;
    }


    /// <summary>
    /// 체력 감소 메소드
    /// </summary>
    void HP_Calculate(Collider tmpC, bool _isCritic)
    {
        if (PlayerPrefsManager.GetInstance().isHPsubing) return;

        PlayerPrefsManager.GetInstance().isHPsubing = true;
        /// 방패 방어력 계산

        /// 방어력 
        double vallu = PlayerPrefsManager.GetInstance().GetPlayerDefence();

        /// 공격력 - 방어력
        string PunchDPS = dts.SubStringDouble(PlayerPrefsManager.GetInstance().PlayerDPS, vallu);
        Debug.LogWarning("공격력 : " + PlayerPrefsManager.GetInstance().PlayerDPS);
        Debug.LogWarning("방어력 : " + vallu);
        Debug.LogWarning("받는 대미지 : " + PunchDPS);
        //var PunchDPS = PlayerPrefsManager.GetInstance().PlayerDPS;

        ///true 라면 크리티컬 터진거임.
        if (_isCritic)
        {
            PunchDPS = dts.SubStringDouble(PlayerPrefsManager.GetInstance().CriticalDPS, vallu);
            //PunchDPS = PlayerPrefsManager.GetInstance().CriticalDPS;
        }


        if (PunchDPS == "-1") PunchDPS = "0";

        PlayerPrefsManager.GetInstance().Mat_currentHP = dts.SubStringDouble(PlayerPrefsManager.GetInstance().Mat_currentHP, PunchDPS);


        /// 체력 0이 되었다!!
        if (PlayerPrefsManager.GetInstance().Mat_currentHP == "0" || PlayerPrefsManager.GetInstance().Mat_currentHP == "-1")
        {
            string tmps = PlayerPrefsManager.GetInstance().Mat_MaxHP;
            PlayerPrefsManager.GetInstance().Mat_currentHP = "0";

            HP_Bar.GetComponentInChildren<Text>().text = "0/" + UserWallet.GetInstance().SeetheNatural(double.Parse(tmps));

            // 그로기 상태 돌입
            GroggyModeOn(tmpC);


        }
        else /// 아직 안죽었다!
        {
            string tmp1 = PlayerPrefsManager.GetInstance().Mat_currentHP;
            string tmp2 = PlayerPrefsManager.GetInstance().Mat_MaxHP;

            HP_Bar.GetComponentInChildren<Text>().text = UserWallet.GetInstance().SeetheNatural(double.Parse(tmp1)) + "/" + UserWallet.GetInstance().SeetheNatural(double.Parse(tmp2));
        }
        //

        HP_Bar.fillAmount = (float)dts.DevideStringDouble(PlayerPrefsManager.GetInstance().Mat_currentHP, PlayerPrefsManager.GetInstance().Mat_MaxHP);

        PlayerPrefsManager.GetInstance().isHPsubing = false;
    }



    /// <summary>
    /// 무한 버티기 모드에서 체력 감소 메소드
    /// </summary>
    void Infini_HP_Calculate(Collider tmpC)
    {
        if (PlayerPrefsManager.GetInstance().isHPsubing || PlayerPrefsManager.GetInstance().isInfinityEnd) return;

        PlayerPrefsManager.GetInstance().isHPsubing = true;

        //인피니티 모드 dps 할당
        string InfiPunchDPS = dts.fDoubleToStringNumber(PlayerPrefsManager.GetInstance().InfiPunchDPS);

        string vallu = UserWallet.GetInstance().GetMattzipForCul(PlayerPrefsManager.GetInstance().Mat_Mattzip);

        string PunchDPS = dts.SubStringDouble(InfiPunchDPS, vallu);
        if (PunchDPS == "-1") PunchDPS = "0";

        PlayerPrefsManager.GetInstance().Mat_currentHP = dts.SubStringDouble(PlayerPrefsManager.GetInstance().Mat_currentHP, PunchDPS);


        Debug.LogWarning("최근 체력 : " + PlayerPrefsManager.GetInstance().Mat_currentHP);

    HELL:

        /// 체력 0이 되었다!!
        if (PlayerPrefsManager.GetInstance().Mat_currentHP == "0" || PlayerPrefsManager.GetInstance().Mat_currentHP == "-1")
        {
            PopUpObjectManager.GetInstance().ComboCnt++;

            string tmps = PlayerPrefsManager.GetInstance().Mat_MaxHP;
            PlayerPrefsManager.GetInstance().Mat_currentHP = "0";

            HP_Bar.GetComponentInChildren<Text>().text = "0/" + UserWallet.GetInstance().SeetheNatural(double.Parse(tmps));


            /// 무한 견디기 모드 일때만 체력 0
            if (PlayerPrefsManager.GetInstance().isInfinity && !PlayerPrefsManager.GetInstance().isEndGame)
            {
                // 버티기 모드 끝.
                PlayerPrefsManager.GetInstance().isEndGame = true;
                PlayerPrefsManager.GetInstance().isInfinityEnd = true;

                tmpC.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0);

                Transform hitBody = tmpC.transform.parent;
                // 스프라이트 비활성화
                transform.GetChild(1).gameObject.SetActive(false);
                if (transform.childCount == 3) transform.GetChild(2).gameObject.SetActive(false);

                /// 무한 버티기 각 단계 개인 최고 기록
                string infiPR = PlayerPrefsManager.GetInstance().InfiPersonalRecord;
                string[] sDataList = (infiPR).Split('*');

                //국밥 획득 갯수 = 맞은 횟수 곱하기 N
                int gettingGoop = PopUpObjectManager.GetInstance().ComboCnt;
                string ttmmpp = PlayerPrefsManager.GetInstance().Infi_Index.ToString("f0");
                switch (ttmmpp)
                {
                    case "100":
                        if (sDataList[0] != "600") sDataList[0] = gettingGoop.ToString();
                        gettingGoop *= 5;
                        break;

                    case "1050":
                        if (sDataList[1] != "600") sDataList[1] = gettingGoop.ToString();
                        gettingGoop *= 11;
                        break;

                    case "11025":
                        if (sDataList[2] != "600") sDataList[2] = gettingGoop.ToString();
                        gettingGoop *= 24;
                        break;

                    case "115763":
                        if (sDataList[3] != "600") sDataList[3] = gettingGoop.ToString();
                        gettingGoop *= 51;
                        break;

                    case "1215506":
                        if (sDataList[4] != "600") sDataList[4] = gettingGoop.ToString();
                        gettingGoop *= 106;
                        break;

                    case "12762816":
                        if (sDataList[5] != "600") sDataList[5] = gettingGoop.ToString();
                        gettingGoop *= 217;
                        break;

                    case "134009564":
                        if (sDataList[6] != "600") sDataList[6] = gettingGoop.ToString();
                        gettingGoop *= 440;
                        break;

                    case "1407100423":
                        if (sDataList[7] != "600") sDataList[7] = gettingGoop.ToString();
                        gettingGoop *= 887;
                        break;

                    case "14774554438":
                        if (sDataList[8] != "600") sDataList[8] = gettingGoop.ToString();
                        gettingGoop *= 1782;
                        break;

                    case "155132821598":
                        if (sDataList[9] != "600") sDataList[9] = gettingGoop.ToString();
                        gettingGoop *= 3573;
                        break;


                }

                //PlayerPrefsManager.GetInstance().MaxGet_GookBap = gettingGoop;
                infiPR = "";
                for (int i = 0; i < sDataList.Length - 1; i++)
                {
                    infiPR += sDataList[i] + "*";
                }
                PlayerPrefsManager.GetInstance().InfiPersonalRecord = infiPR;


                //Debug.LogWarning("콤보 횟수 : " + PopUpObjectManager.GetInstance().ComboCnt);
                //Debug.LogWarning(PlayerPrefsManager.GetInstance().Infi_Index + "배 곱하기 : " + gettingGoop);

                hitBody.GetComponent<SpriteRenderer>().enabled = true;
                hitBody.GetComponent<Animation>().Stop();
                //
                hitBody.transform.GetChild(1).gameObject.SetActive(false);
                hitBody.transform.GetChild(2).gameObject.SetActive(false);
                hitBody.transform.GetChild(3).gameObject.SetActive(false);
                // 그로기 풀어준다.
                PlayerPrefsManager.GetInstance().isGroggy = false;

                /// 퀘스트
                PlayerPrefsManager.GetInstance().questInfo[0].daily_MiniCombo++;

                if (PlayerPrefsManager.GetInstance().questInfo[0].All_MiniGame < 1000)
                {
                    PlayerPrefsManager.GetInstance().questInfo[0].All_MiniGame++;
                }

                hitBody.GetComponent<Animation>().Stop();

                Invoke("Healing_Infi", 0.5f);


                /// 국밥 획득량 % 증가
                double getSSalAmount = ((gettingGoop * 1d) * (1.0d + PlayerPrefsManager.GetInstance().Arti_InfiReword * 0.01d));

                getSSalAmount = getSSalAmount *
                    (1.0d + ((PlayerPrefsManager.GetInstance().uniformInfo[3].Uniform_LV +
                    (PlayerPrefsManager.GetInstance().uniformInfo[4].Uniform_LV * 0.05d) +
                    (PlayerPrefsManager.GetInstance().uniformInfo[5].Skill_LV * 0.5d))


                    * 0.01d));
                Debug.LogError("보통 국밥 : " + getSSalAmount);

                // 국밥 팝업 호출
                PopUpObjectManager.GetInstance().ShowInfinityPopUP(getSSalAmount);
                // 국밥 콤보 카운트 초기화.
                //PopUpObjectManager.GetInstance().ComboCnt = 0;     -> 국밥 받기 버튼 누를때 초기화.
                //
            }

        }
        else /// 아직 안죽었다!
        {
            string tmp1 = PlayerPrefsManager.GetInstance().Mat_currentHP;
            string tmp2 = PlayerPrefsManager.GetInstance().Mat_MaxHP;

            HP_Bar.GetComponentInChildren<Text>().text = UserWallet.GetInstance().SeetheNatural(double.Parse(tmp1)) + "/" + UserWallet.GetInstance().SeetheNatural(double.Parse(tmp2));
            HP_Bar.fillAmount = (float)dts.DevideStringDouble(PlayerPrefsManager.GetInstance().Mat_currentHP, PlayerPrefsManager.GetInstance().Mat_MaxHP);

            if (PlayerPrefsManager.GetInstance().isInfinity)
            {
                // 버티기 난이도 만큼 씩 증가
                PlayerPrefsManager.GetInstance().InfiPunchDPS += (double)PlayerPrefsManager.GetInstance().Infi_Index;
                //
                Debug.Log(PlayerPrefsManager.GetInstance().InfiPunchDPS);
                // 콤보 카운트
                PopUpObjectManager.GetInstance().ComboCnt++;
                /// 제한 사항 넣기. 최대 콤보 600 이다.
                if (PopUpObjectManager.GetInstance().ComboCnt > 599)
                {
                    PopUpObjectManager.GetInstance().ComboCnt = 599;
                    PlayerPrefsManager.GetInstance().Mat_currentHP = "0";

                    goto HELL;
                }

                PopUpObjectManager.GetInstance().SetComboText();

            }
        }
        //

        PlayerPrefsManager.GetInstance().isHPsubing = false;
    }

    void Healing_Infi()
    {
        string tmps = PlayerPrefsManager.GetInstance().Mat_MaxHP;
        PlayerPrefsManager.GetInstance().Mat_currentHP = tmps;
        HP_Bar.GetComponentInChildren<Text>().text = UserWallet.GetInstance().SeetheNatural(double.Parse(tmps)) + "/" + UserWallet.GetInstance().SeetheNatural(double.Parse(tmps));
        HP_Bar.fillAmount = (float)dts.DevideStringDouble(PlayerPrefsManager.GetInstance().Mat_currentHP, PlayerPrefsManager.GetInstance().Mat_MaxHP);
    }






    Image DefHP_Bar;
    public void SetDefenceBar(Image hp)
    {
        DefHP_Bar = hp;
    }
    /// <summary>
    /// 방어전 모드 체력 감소
    /// </summary>
    void Defence_HP_Calculate(Collider tmpC)
    {
        string mega = PlayerPrefsManager.GetInstance().megaDamColl[PlayerPrefsManager.GetInstance().PunchIndex];

        /// 방어전 모드 대미지 % 감소
        double doublePrice = double.Parse(mega);
        doublePrice = (doublePrice * (1.0d - PlayerPrefsManager.GetInstance().Arti_DefencePer * 0.001d));


        string vallu = UserWallet.GetInstance().GetMattzipForCul(PlayerPrefsManager.GetInstance().Mat_Mattzip);
        string PunchDPS = dts.SubStringDouble(doublePrice.ToString("f0"), vallu);

        if (PunchDPS == "-1") PunchDPS = "0";


        PlayerPrefsManager.GetInstance().Mat_currentHP = dts.SubStringDouble(PlayerPrefsManager.GetInstance().Mat_currentHP, PunchDPS);

        /// 체력 0이 되었다!!
        if (PlayerPrefsManager.GetInstance().Mat_currentHP == "0" || PlayerPrefsManager.GetInstance().Mat_currentHP == "-1")
        {
            string tmps = PlayerPrefsManager.GetInstance().Mat_MaxHP;
            PlayerPrefsManager.GetInstance().Mat_currentHP = "0";

            if(DefHP_Bar != null) DefHP_Bar.GetComponentInChildren<Text>().text = "0/" + UserWallet.GetInstance().SeetheNatural(double.Parse(tmps));

            // 그로기 상태 돌입
            tmpC.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            Transform hitBody = tmpC.transform.parent;
            // 스프라이트 비활성화
            transform.GetChild(1).gameObject.SetActive(false);
            if (transform.childCount == 3) transform.GetChild(2).gameObject.SetActive(false);

            hitBody.GetComponent<SpriteRenderer>().enabled = false;
            hitBody.GetComponent<Animation>().Play("Groggy");
            // 실패 트리거 올려줘서 디펜모드에서 처리.
            PlayerPrefsManager.GetInstance().DefendTrigger = 1;
        }
        else
        {
            string tmp1 = PlayerPrefsManager.GetInstance().Mat_currentHP;
            string tmp2 = PlayerPrefsManager.GetInstance().Mat_MaxHP;


            if (DefHP_Bar != null) DefHP_Bar.GetComponentInChildren<Text>().text = UserWallet.GetInstance().SeetheNatural(double.Parse(tmp1)) + "/" + UserWallet.GetInstance().SeetheNatural(double.Parse(tmp2));
        }
        //
        if (DefHP_Bar != null) DefHP_Bar.fillAmount = (float)dts.DevideStringDouble(PlayerPrefsManager.GetInstance().Mat_currentHP, PlayerPrefsManager.GetInstance().Mat_MaxHP);

    }



    Image Mugan_HP;
    Image Boss_HP;
    public void SetMuganBar(Image hp, Image boop)
    {
        Mugan_HP = hp;
        Boss_HP = boop;
    }
    /// <summary>
    /// 무한의 탑 모드에서 방어실패 - 몸체 맞고 체력 감소
    /// </summary>
    void Mugan_HP_Calculate(Collider tmpC)
    {
        string playerCurrentHP = PlayerPrefsManager.GetInstance().Mat_currentHP;
        string playerMaxHP = PlayerPrefsManager.GetInstance().Mat_MaxHP;

        /// 순수 파이어볼 공격력
        string FireBall = GetFireBall(PlayerPrefsManager.GetInstance().MaxGet_MuganTop, FireType.Normal);
        /// 강력 파이어볼이라면 대미지 1.5배
        if (name == "ArStrongBall")
            FireBall = dts.multipleStringDouble(FireBall, 1.5d);
        else if(name == "AsHealBall")
        {
            FireBall = dts.multipleStringDouble(FireBall, 0.5d);
            playerCurrentHP = dts.AddStringDouble(playerCurrentHP, FireBall);
            Mugan_HP.transform.GetChild(0).GetComponent<Text>().text =
                UserWallet.GetInstance().SeetheNatural(double.Parse(playerCurrentHP)) + "/" +
                UserWallet.GetInstance().SeetheNatural(double.Parse(playerMaxHP));
            PlayerPrefsManager.GetInstance().Mat_currentHP = playerCurrentHP;
            //
            Mugan_HP.fillAmount = (float)dts.DevideStringDouble(playerCurrentHP, playerMaxHP);
            /// 최근 체력 갱신
            return;
        }

        playerCurrentHP = dts.SubStringDouble(playerCurrentHP, FireBall);

        /// 체력 0이 되었다!!
        if (playerCurrentHP == "0" || playerCurrentHP == "-1")
        {
            playerCurrentHP = "0";
            Mugan_HP.transform.GetChild(0).GetComponent<Text>().text = "0/" + UserWallet.GetInstance().SeetheNatural(double.Parse(playerMaxHP));
            PlayerPrefsManager.GetInstance().isMuGanTopEnd = true;

            // 이미 이어하기를 썼다면 바로 종료
            if (PlayerPrefsManager.GetInstance().isSecondChan)
            {
                // 그만두기 버튼 눌러
                GameObject.Find("MUGANNOTOPManager").GetComponent<MuganMode>().EndBtnClicked();
            }
            else
            {
                /// TODO : 이어하기 팝업 호출.
                PopUpObjectManager.GetInstance().ShowMuganCountinue();
            }
        }

        Mugan_HP.transform.GetChild(0).GetComponent<Text>().text =
            UserWallet.GetInstance().SeetheNatural(double.Parse(playerCurrentHP)) + "/" +
            UserWallet.GetInstance().SeetheNatural(double.Parse(playerMaxHP));
        Mugan_HP.fillAmount = (float)dts.DevideStringDouble(playerCurrentHP, playerMaxHP);
        /// 최근 체력 갱신
        PlayerPrefsManager.GetInstance().Mat_currentHP = playerCurrentHP;

    }

    /// <summary>
    /// pvp 에서 주먹으로 맞음
    /// </summary>
    /// <param name="tmpC"></param>
    /// <returns></returns>
    IEnumerator PVPPVP(Collider tmpC)
    {


        yield return new WaitForSeconds(0.05f);

        tmpC.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        transform.GetChild(1).gameObject.SetActive(false);
        //
        Lean.Pool.LeanPool.Despawn(gameObject, 1.0f);
    }

    /// <summary>
    /// 맷집 증가 메소드
    /// </summary>
    void Mat_100_Count(bool _isCritic)
    {
        /// 인트로 시청 안했으면 리턴
        if (!PlayerPrefsManager.GetInstance().isFristGameStart) return;

        ///// 동료 한명씩 해제할 때 마다 게이지 올라가는거 올라감
        //if (PlayerPrefsManager.GetInstance().Friend_01_MattzipPer_Lv > 0 && PlayerPrefsManager.GetInstance().Friend_02_OffTimeUp_Lv > 0)
        //    PlayerPrefsManager.GetInstance().Mat_100 += 3; // 1씩 증가
        //else if (PlayerPrefsManager.GetInstance().Friend_01_MattzipPer_Lv > 0)
        //    PlayerPrefsManager.GetInstance().Mat_100 += 2; // 1씩 증가
        //else if (PlayerPrefsManager.GetInstance().Friend_02_OffTimeUp_Lv > 0)
        //    PlayerPrefsManager.GetInstance().Mat_100 += 2; // 1씩 증가
        //else PlayerPrefsManager.GetInstance().Mat_100++; // 1씩 증가

        /// 펀치 최종 공격력
        float playerDPS = float.Parse(PlayerPrefsManager.GetInstance().PlayerDPS);
        if (_isCritic) playerDPS = float.Parse(PlayerPrefsManager.GetInstance().CriticalDPS);
        /// 대미지 누적
        PlayerPrefsManager.GetInstance().Mat_100 += playerDPS;
        /// 맷집 증가에 필요한 대미지 게이지 (Max치)
        float needGaugeMat = PlayerPrefsManager.GetInstance().Cilcked_Cnt_MattZip;
        /// 누적 대미지 / 증가 필요 대미지
        MattzipGauge.fillAmount = PlayerPrefsManager.GetInstance().Mat_100 / needGaugeMat;
        /// 게이지 100% 채웠으면?
        if (PlayerPrefsManager.GetInstance().Mat_100 >= needGaugeMat)
        {
            /// 최종 공격력 0.1% + 1 증가
            PlayerPrefsManager.GetInstance().Mat_Mattzip_Hit += 1f + (playerDPS * 0.0001f);
            var mattLv = PlayerPrefsManager.GetInstance().Mat_Mattzip_Hit;
            ///맷집 증가에 필요한 대미지 증가 계산식 = 시작값 5, 시작값 * ( 1 + 0.03 ) ^ Lv
            //PlayerPrefsManager.GetInstance().Cilcked_Cnt_MattZip = (float)(5f * System.Math.Pow(1.03f, PlayerPrefsManager.GetInstance().Mat_Mattzip_Hit));
            PlayerPrefsManager.GetInstance().Cilcked_Cnt_MattZip = (0.5f * mattLv * mattLv) + mattLv * 0.5f + 4.5f;
            UserWallet.GetInstance().ShowUserMatZip();
            /// 초기화
            PlayerPrefsManager.GetInstance().Mat_100 -= needGaugeMat;
            MattzipGauge.fillAmount = 0;
        }

    }

    /// <summary>
    /// 꾸짖을 갈 기 게이지 증가 메소드
    /// </summary>
    void Skill_500_Count(Collider tmpC)
    {
        PlayerPrefsManager.GetInstance().Mat_Skill_300++;
        //
        SkillGauge.fillAmount = PlayerPrefsManager.GetInstance().Mat_Skill_300 / (1000f - PlayerPrefsManager.GetInstance().Pet_Buff_Lv);

        if (PlayerPrefsManager.GetInstance().Mat_Skill_300 >= (1000 - PlayerPrefsManager.GetInstance().Pet_Buff_Lv))
        {
            /// TODO : 꾸짖을 갈!
            tmpC.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            PlayerPrefsManager.GetInstance().isGroggy = true;

            Transform hitBody = tmpC.transform.parent;
            // 스프라이트 비활성화
            transform.GetChild(1).gameObject.SetActive(false);
            if (transform.childCount == 3) transform.GetChild(2).gameObject.SetActive(false);
            hitBody.GetComponent<SpriteRenderer>().enabled = false;
            hitBody.GetComponent<Animation>()["Healling"].speed = 1;
            hitBody.GetComponent<Animation>().Play("Healling");

            GameObject.Find("GroggyManager").GetComponent<GroggyManager>().HeallingOff();


            PlayerPrefsManager.GetInstance().Mat_Skill_300 = 0;
            SkillGauge.fillAmount = 0;

        }

    }





}
