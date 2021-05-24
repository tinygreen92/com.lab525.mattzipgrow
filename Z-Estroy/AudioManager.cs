using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 오디오정보 클래스
[System.Serializable] // 이걸 적어줘야 인스펙터창에 나온다
public class AudioInfomation
{
    public string name; // 오디오이름
    public float volume; // 오디오볼륨
    public bool isLoop; // 반복재생(배경음악의 경우)
    public AudioClip clip; // 오디오파일
}

// 오디오를 관리하는 클래스(싱글톤) - 배경음악,효과음,음성을 따로관리
public class AudioManager : MonoBehaviour
{
    // 인스턴스를 정적으로 선언
    public static AudioManager instance;
    // private가 생략되어있음 - [SerializeField]를 써줘야 인스펙터창에 뜬다
    [SerializeField] AudioInfomation[] bgmInfo = null; // 배경음악 오디오 정보
    //[SerializeField] AudioSource bgmPlayer = null; // 배경음악 오디오 플레이어
    private AudioSource bgmPlayer;
    private int currentBGMIndex; // 현재 플레이중인 배경음악의 인덱스번호

    [SerializeField] AudioInfomation[] seInfo = null; // 효과음 오디오 정보
    private List<AudioSource> sePlayer; // 효과음 오디오 플레이어

    //[SerializeField] AudioSource voicePlayer = null; // 음성 오디오 플레이어
    private AudioSource voicePlayer;
    [SerializeField] float defualtVolume = 1f; // 음성 볼륨값


    /// <summary>
    /// 주먹질 효과음 껏다키기
    /// true - 소리 0
    /// false - 1. 소리 켜기 2.뮤트라면 안켜기
    /// </summary>
    /// <param name="isMute"></param>
    public void HitVolumeMute(bool isMute)
    {
        if (isMute)
        {
            isHitMute = true;
        }
        else
        {
            if (PlayerPrefsManager.GetInstance().isAllmute)
            {
                isHitMute = true;
            }
            else
            {
                isHitMute = false;
            }
        }
    }

    void Mumumute() => HitVolumeMute(false);

    /// <summary>
    /// 코드 내부에서만 사용하는거 지연시간 0.6
    /// </summary>
    public void InCodeHitVolMute()
    {
        Invoke(nameof(Mumumute), 0.6f);
    }

    public void AllMute()
    {
        bgmInfo[0].volume = 0;
        bgmInfo[1].volume = 0;
        bgmInfo[2].volume = 0;
        bgmInfo[3].volume = 0;
        bgmInfo[4].volume = 0;
        //
        seInfo[0].volume = 0;
        seInfo[1].volume = 0;
        seInfo[2].volume = 0;
        seInfo[3].volume = 0;
        seInfo[4].volume = 0;
        //
        seInfo[5].volume = 0;
        seInfo[6].volume = 0;
        seInfo[7].volume = 0;

        seInfo[8].volume = 0;
        seInfo[9].volume = 0;

        PlayerPrefsManager.GetInstance().isAllmute = true;
        StopAllAudio();
    }

    public void AllUnMute()
    {
        bgmInfo[0].volume = 0.8f;
        bgmInfo[1].volume = 0.8f;
        bgmInfo[2].volume = 0.8f; 
        bgmInfo[3].volume = 2.0f;
        bgmInfo[4].volume = 2.5f;
        //
        seInfo[0].volume = 0.3f;
        seInfo[1].volume = 0.3f;
        seInfo[2].volume = 0.15f; /// 주먹소리
        seInfo[3].volume = 0.3f;
        seInfo[4].volume = 0.3f;
        //
        seInfo[5].volume = 0.3f;
        seInfo[6].volume = 0.3f;
        seInfo[7].volume = 0.25f;

        seInfo[8].volume = 0.25f;
        seInfo[9].volume = 0.25f;

        PlayerPrefsManager.GetInstance().isAllmute = false;

        PlayMainBGM();
    }

    [Header("-사운드 On 버튼 달아두기")]
    public GameObject soundIcon;

    public void AudioInit()
    {
        instance = this; // 인스턴스는 자기자신

        // 오디오재생 플레이어에 오디오 소스컴퍼넌트를 붙인다
        bgmPlayer = this.gameObject.AddComponent<AudioSource>();
        voicePlayer = this.gameObject.AddComponent<AudioSource>();
        sePlayer = new List<AudioSource>();
        for (int i = 0; i < seInfo.Length; i++)
        {
            sePlayer.Add(this.gameObject.AddComponent<AudioSource>());
        }


    }

    /// <summary>
    /// 오디오 버튼 세팅 기능 세팅
    /// </summary>
    public void AudioSetting()
    {
        if (PlayerPrefsManager.GetInstance().isAllmute)
        {
            soundIcon.SetActive(false);
            AllMute();
        }
        else
        {
            AllUnMute();
            /// 처음에 시작할때 튜토리얼 전에 끝내줌.
            StopBGM();
        }
    }

    //--------- 외부에서 호출하는 메소드 --------------
    // 파일이름과 어떤타입의 오디오인지를 인수로 넘겨받는다
    public void PlayMainBGM()
    {
        PlayBGM("main");
    }
    public void BnextBGM()
    {
        PlayBGM("mini");
    }
    public void MuganBGM()
    {
        PlayBGM("MUGAN");
    }
    public void MiniGameBGM()
    {
        PlayBGM("MiniGame");
    }
    public void PVP_BGM()
    {
        PlayBGM("PvPGame");
    }

    public void MiniGameYAM()
    {
        PlaySE("YAM");
    }

    public void Btn_healing()
    {
        PlaySE("healing");
    }
    /// <summary>
    /// 메뉴 화면 진입하면 소리 안나게.
    /// </summary>
    bool isHitMute;
    public void Btn_hit()
    {
        if (isHitMute) return;
        PlaySE("hit");
    }


    public void Mugan_Block()
    {
        PlaySE("muganBlock");
    }

    public void Mugan_Hit()
    {
        PlaySE("muganHit");
    }
    //

    public void Btn_button()
    {
        PlaySE("button");
    }
    public void Btn_warnnig()
    {
        PlaySE("warnnig");
    }
    public void Btn_popup()
    {
        PlaySE("popup");
    }

    public void Btn_Gatcha()
    {
        PlaySE("Gatcha");
    }
    public void Btn_Mooyaho()
    {
        PlaySE("Mooyaho");
    }









    // BGM재생 메소드 파라미터로 이름을 받는다
    private void PlayBGM(string p_name)
    {
        // bgmInfo의 배열만큼 반복실행
        for (int i = 0; i < bgmInfo.Length; i++)
        {
            // 파라미터로 넘어온 이름과 bgmInfo의 이름과 비교
            if (p_name == bgmInfo[i].name)
            {
                // bgmInfo에 담겨있는 오디오클립을 재생하고 반복문을 빠져나간다
                bgmPlayer.clip = bgmInfo[i].clip;
                bgmPlayer.volume = bgmInfo[i].volume;
                bgmPlayer.loop = bgmInfo[i].isLoop; // 배경음악 반복재생
                bgmPlayer.Play();
                currentBGMIndex = i; // 현재 재생중인 BGM의 인덱스번호를 저장
                return;
            }
        }
        // 파라미터로 넘어온 이름이 bgmInfo에 없을때 에러로그를 띄운다
        Debug.LogError(p_name + "에 해당하는 배경음악이 없습니다");
    }

    // 배경음악을 멈춘다
    private void StopBGM()
    {
        bgmPlayer.Stop();
    }

    // 배경음악을 일시정지한다
    private void PauseBGM()
    {
        bgmPlayer.Pause();
    }
    // 배경음악의 일시정지를 푼다
    private void UnPauseBGM()
    {
        bgmPlayer.UnPause();
    }

    // 효과음을 재생 파라미터로 이름을 받는다
    private void PlaySE(string p_name)
    {
        // seInfo의 배열만큼 반복실행
        for (int i = 0; i < seInfo.Length; i++)
        {
            // 파라미터로 넘어온 이름과 bgmInfo의 이름과 비교
            if (p_name == seInfo[i].name)
            {
                // 효과음 플레이어의 갯수만큼 반복실행
                for (int j = 0; j < sePlayer.Count; j++)
                {
                    // seInfo에 담겨있는 오디오클립을 재생하고 반복문을 빠져나간다
                    sePlayer[j].clip = seInfo[i].clip;
                    sePlayer[j].volume = seInfo[i].volume;
                    sePlayer[j].PlayOneShot(sePlayer[j].clip);
                }
            }
        }
    }

    // 재생중인 모든 효과음을 멈춘다
    private void StopAllSE()
    {
        // 효과음 플레이어의 갯수만큼 반복실행
        for (int i = 0; i < sePlayer.Count; i++)
        {
            sePlayer[i].Stop(); // 효과음 재생 정지
        }
    }

    // 음성 재생 파라미터로 이름과 볼륨을 받는다(리소스 폴더안에서 로드)
    private void PlayVoice(string p_name, float volume)
    {
        // Assets폴더밑에 Resources 폴더안에있는 파일을 로드한다
        //!!!! 이 부분은 폴더구성이 각기 다르기때문에 자신의 폴더구성에 맞게 변경해야함!!!!!
        AudioClip _clip = Resources.Load<AudioClip>("Audio/Voice/" + p_name) as AudioClip;
        if (_clip != null)
        {
            voicePlayer.clip = _clip;
            voicePlayer.volume = volume;
            voicePlayer.Play();
        }
        else
        {
            Debug.LogError(p_name + "에 해당하는 음성파일이 없습니다");
        }
    }
    // 음성재생을 멈춘다
    private void StopVoice()
    {
        voicePlayer.Stop();
    }




    ///
    ///p_Type : BGM -> BGM 배경음악 재생
    ///p_Type : SE -> SE 효과음 재생
    ///p_Type : VOICE -> Voice 음성 재생
    ///
    public void PlayAudio(string p_name, string p_type)
    {
        PlayAudio(p_name, p_type, defualtVolume); // 볼륨값 지정을 안하는경우는 디폴트값으로 지정
    }
    // 볼륨값을 지정한 경우는 해당 볼륨으로 재생
    public void PlayAudio(string p_name, string p_type, float volume)
    {
        // 넘겨받은 타입변수값에 따라서 해당 플레이어를 재생시킨다
        if (p_type == "BGM") PlayBGM(p_name);
        else if (p_type == "SE") PlaySE(p_name);
        else if (p_type == "VOICE") PlayVoice(p_name, volume);
        else Debug.LogError("해당하는 타입의 오디오플레이어가 없습니다");
    }
    // 오디오재생을 정지하는 메소드
    public void StopAudio(string p_type)
    {
        if (p_type == "BGM") StopBGM();
        else if (p_type == "SE") StopAllSE();
        else if (p_type == "VOICE") StopVoice();
        else Debug.LogError("해당하는 타입의 오디오플레이어가 없습니다");
    }
    // 모든 오디오재생을 정지하는 메소드
    public void StopAllAudio()
    {
        StopBGM();
        StopAllSE();
        StopVoice();
    }
    // 다음 배경음악을 재생시키는 메소드
    public void PlayNextBGM()
    {
        // 현재 재생중인 배경음악이 마지막 배경음악인 경우
        if (currentBGMIndex + 1 == bgmInfo.Length)
        {
            PlayBGM(bgmInfo[0].name);
        }
        else
        {
            // 현재 재생중인 배경음악의 다음곡을 재생한다
            PlayBGM(bgmInfo[currentBGMIndex + 1].name);
        }
    }

    // 배경음악을 랜덤으로 재생시키는 메소드
    public void PlayRandomBGM()
    {
        // 배경음악이 한곡이면 의미가 없음
        if (bgmInfo.Length > 1)
        {
            int randomIndex;
            // 현재 재생중인 배경음악과 다른게 뽑힐때까지 반복한다
            while (true)
            {
                randomIndex = Random.Range(0, bgmInfo.Length);
                if (currentBGMIndex != randomIndex) break;
            }
            currentBGMIndex = randomIndex;
            PlayBGM(bgmInfo[currentBGMIndex].name);
        }
    }
}