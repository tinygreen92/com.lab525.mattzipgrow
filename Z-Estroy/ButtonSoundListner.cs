using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonSoundListner : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    PunchManager punchManager;
    QuestManager questManager;
    ShieldManager shieldManager;
    FriendManager friendManager;

    private void Awake()
    {
        punchManager = GameObject.Find("PunchManager").GetComponent<PunchManager>();
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        shieldManager = GameObject.Find("ShieldManager").GetComponent<ShieldManager>();
        friendManager = GameObject.Find("FriendManager").GetComponent<FriendManager>();
    }


    // 버튼 클릭 감지
    bool isBtnDown;
    int p_index = 0;
    public void OnPointerDown(PointerEventData eventData)
    {

        /// 퀘스트 롱 클릭
        if(transform.parent.parent.tag == "GRID_QUEST")
        {
            Invoke(nameof(InvoDownQuset), 0.3f);
        }

        if (transform.parent == null) return;
        else if (transform.parent.parent == null) return;
        else if (transform.parent.parent.parent == null) return;
        
        /// 훈련도구 업글 롱 클릭
        if (transform.parent.parent.parent.tag == "UPGRADE" && transform.GetSiblingIndex() == 1)
        {
            Invoke(nameof(InvoDown), 0.3f);
        }
        /// 방패 업글 롱 클릭
        else if (transform.parent.tag == "Shield" && transform.GetSiblingIndex() == 1)
        {
            Invoke(nameof(InvoDown), 0.3f);
        }
        /// 동료 업글 롱 클릭
        else if (transform.parent.parent.tag == "Friend" && transform.GetSiblingIndex() == 0)
        {
            Invoke(nameof(InvoDown), 0.3f);
        }
    }

    bool isBtnDownQuest;
    int q_index;
    void InvoDownQuset()
    {
        // 퀘스트 버튼 일때 클릭한 지점 이름 얻어오기
        string nameIndex = EventSystem.current.currentSelectedGameObject.transform.parent.name;
        string strTmp = Regex.Replace(nameIndex, @"\D", "");
        q_index = int.Parse(strTmp) - 1;

        isBtnDownQuest = true;
    }

    void InvoDown()
    {
        // 클릭한 지점 이름 얻어오기
        p_index = int.Parse(transform.parent.parent.name);
        isBtnDown = true;
    }

    /// <summary>
    /// 이 스크립트가 붙어 있는 오브젝트에서 마우스 떨어질때
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (transform.tag == "popup") AudioManager.instance.Btn_popup();
        if (transform.tag == "button") AudioManager.instance.Btn_button();
        if (transform.tag == "warnnig") AudioManager.instance.Btn_warnnig();
        /// 훈련강화 강화 할때 쓴다.
        /// 

        if (transform.parent.parent.tag == "GRID_QUEST")
        {
            CancelInvoke(nameof(InvoDownQuset));
            isBtnDownQuest = false;
        }

        if (transform.parent == null) return;
        else if (transform.parent.parent == null) return;
        else if (transform.parent.parent.parent == null) return;

        if (transform.parent.parent.parent.transform.tag == "UPGRADE" || transform.parent.tag == "Shield" || transform.parent.parent.tag == "Friend")
        {
            CancelInvoke(nameof(InvoDown));
            isBtnDown = false;
        }

    }


    private void FixedUpdate()
    {
        if (isBtnDown)
        {
            if (transform.parent.tag == "Shield")
                shieldManager.LongClicedUpgradeBtn(p_index);
            else if(transform.parent.parent.tag == "Friend")
                friendManager.ClickedUpgradeBtn(p_index);
            else
                punchManager.LongClicedUpgradeBtn(p_index);

        }

        if (isBtnDownQuest)
        {
            if (transform.parent.parent.name == "GRID_QUEST_1")
            {
                questManager.DailyMission(q_index);
            }
            else if (transform.parent.parent.name == "InfinityQuest")
            {
                questManager.GetRewordSpecialMission(int.Parse(transform.parent.name));
            }
            else if (transform.parent.parent.name == "GRID_QUEST_3")
            {
                questManager.GetRewordAllMission(q_index);
            }
        }

    }



}
