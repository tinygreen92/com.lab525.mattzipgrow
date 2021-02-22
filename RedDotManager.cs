using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDotManager : MonoBehaviour
{
    static RedDotManager instance;

    [Header("- 궤스트 알람")]
    public GameObject QuestDot0;
    public GameObject QuestDot1;
    public GameObject QuestDot2;
    [Header("- 우편함 알람")]
    public GameObject PostDot0;
    public GameObject PostDot1;


    public static RedDotManager GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// 우편함 - 설정 알람 온
    /// </summary>
    public void PostDotOn0()
    {
        PostDot0.SetActive(true);
    }
    /// <summary>
    /// 우현함 설정 알람 오프
    /// </summary>
    public void PostDotOff0()
    {
        PostDot0.SetActive(false);
    }


    /// <summary>
    /// 우편함 - 설정 알람 온
    /// </summary>
    public void PostDotOn1()
    {
        PostDot1.SetActive(true);
    }
    /// <summary>
    /// 우현함 설정 알람 오프
    /// </summary>
    public void PostDotOff1()
    {
        PostDot1.SetActive(false);
    }




    ////////////////////////////////////////////////////////////////////////////////////////////


    /// <summary>
    /// 퀘스트 알람 온
    /// </summary>
    public void QuestDotOn0()
    {
        QuestDot0.SetActive(true);
    }
    /// <summary>
    /// 퀘스트 알람 오프
    /// </summary>
    public void QuestDotOff0()
    {
        QuestDot0.SetActive(false);
    }

    /// <summary>
    /// 퀘스트 알람 온
    /// </summary>
    public void QuestDotOn1()
    {
        QuestDot1.SetActive(true);
    }
    /// <summary>
    /// 퀘스트 알람 오프
    /// </summary>
    public void QuestDotOff1()
    {
        QuestDot1.SetActive(false);
    }

    /// <summary>
    /// 퀘스트 알람 온
    /// </summary>
    public void QuestDotOn2()
    {
        QuestDot2.SetActive(true);
    }
    /// <summary>
    /// 퀘스트 알람 오프
    /// </summary>
    public void QuestDotOff2()
    {
        QuestDot2.SetActive(false);
    }

}
