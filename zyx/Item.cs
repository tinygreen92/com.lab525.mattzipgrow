using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public void UpdateItem(int count)
    {
        if (count == -100)
        {
            gameObject.SetActive(false);
        }
        else
        {
            /// 이 오브젝트의 이름을 숫자로 바꿔준다.
            gameObject.name = string.Format("{0}", count);
            
            switch (transform.parent.name)
            {
                case "InfinityContent":
                    /// 해당 아이템 새로고침
                    GetComponent<PunchItem>().BoxInfoUpdate(count);
                    gameObject.SetActive(true);
                    break;

                case "InfinityQuest":
                    /// 해당 아이템 새로고침
                    GetComponent<QuestItem>().BoxInfoUpdate(count);
                    gameObject.SetActive(true);
                    break;
                
                case "InfinityShield":
                    /// 해당 아이템 새로고침
                    GetComponent<ShieldItem>().BoxInfoUpdate(count);
                    gameObject.SetActive(true);
                    break;
            }



        }
    }
}