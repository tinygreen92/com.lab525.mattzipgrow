using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedSun : MonoBehaviour
{
    public Image HpBar;
    public GameObject HP_UNDER_IMG;

    Coroutine pikapika;
    private void FixedUpdate()
    {
        // 체력이 10% 이하로 떨어지면 번쩍
        if (HpBar.fillAmount <= 0.1f)
        {
            if (isCoRunning) return;
            pikapika = StartCoroutine(Pikachu());
            isCoRunning = true;
        }
        else
        {
            if (!isCoRunning) return;
            if (pikapika != null) StopCoroutine(pikapika);
            HP_UNDER_IMG.SetActive(false);
            isCoRunning = false;

        }

    }
    bool isCoRunning;
    IEnumerator Pikachu()
    {
        AudioManager.instance.Btn_warnnig();

        isCoRunning = true;
        yield return null;

        for (; ; )
        {
            yield return new WaitForSeconds(0.2f);
            HP_UNDER_IMG.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            HP_UNDER_IMG.SetActive(false);
        }

    }
}
