using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPGS_Linker : MonoBehaviour
{
    public GameObject gPGSManager;

    /// <summary>
    /// GameStartManager에서 호출 할 것
    /// </summary>
    public void Init()
    {
        gPGSManager = GameObject.Find("GPGSManager");
    }

}
