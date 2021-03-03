using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendManager : MonoBehaviour
{
    public SpriteRenderer friend_FirstBody; 
    public SpriteRenderer friend_SecondBody;

    public Text TestText;


    private void Start()
    {
        TestText.text = "동료1_LV." + PlayerPrefsManager.GetInstance().Friend_01_MattzipPer_Lv + "/ 동료2_LV." + PlayerPrefsManager.GetInstance().Friend_02_OffTimeUp_Lv;
    }

    /// <summary>
    /// TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST 
    /// </summary>
    /// <param name="_index"></param>
    public void TEST_Friend_UPDOWN(int _index)
    {
        if (_index == 1)
        {
            PlayerPrefsManager.GetInstance().Friend_01_MattzipPer_Lv++;

            friend_FirstBody.gameObject.SetActive(true);
        }
        else
        {
            PlayerPrefsManager.GetInstance().Friend_02_OffTimeUp_Lv++;

            friend_SecondBody.gameObject.SetActive(true);
        }
        PlayerPrefs.Save();

        TestText.text = "동료1_LV." + PlayerPrefsManager.GetInstance().Friend_01_MattzipPer_Lv + "/ 동료2_LV." + PlayerPrefsManager.GetInstance().Friend_02_OffTimeUp_Lv;
    }







}
