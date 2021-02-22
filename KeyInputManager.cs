using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputManager : MonoBehaviour
{
    public MINIgameManager MINIgameManager;
    public MuganMode MuganMode;


#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {
        // 좌 화살표
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //MINIgameManager.LeftClicked();
            MuganMode.LeftBtn_Down();
        }
        

        // 우 화살표
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //MINIgameManager.RightClicked();
            MuganMode.RightBtn_Down();
        }


    }

#endif


}
