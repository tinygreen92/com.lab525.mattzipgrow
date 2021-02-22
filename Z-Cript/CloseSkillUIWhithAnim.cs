/**
 * @author : tinygreen92 
 * @date   : 2019-06-11
 * @desc   : 스킬 설명창 X 누르면 애니메이션 재생해주고 꺼줘라
 *           
 */
using UnityEngine;

public class CloseSkillUIWhithAnim : MonoBehaviour
{
    public void ShowUIAnim()
    {
        // 애니메이션 재생중일때 예외처리
        if (PlayerPrefsManager.GetInstance().isUIAinmPlay) return;
        //
        gameObject.SetActive(true);
        gameObject.GetComponent<Animator>().Play("OpenSkillBack", -1, 0f);
    }

    public void HideUIAnim()
    {
        // 애니메이션 재생중일때 예외처리
        if (PlayerPrefsManager.GetInstance().isUIAinmPlay) return;
        //
        gameObject.GetComponent<Animator>().Play("CloseSkillBack", -1, 0f);
        Invoke("HideInvokeSkill", 0.2f);
    }

    private void HideInvokeSkill()
    {
        gameObject.SetActive(false);
    }
}
