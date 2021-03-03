using UnityEngine;
using System.Collections;
using EasyMobile; // include the Easy Mobile namespace to use its scripting API

public class EasyMobileInitializer : MonoBehaviour
{

    // Checks if EM has been initialized and initialize it if not.
    // This must be done once before other EM APIs can be used.
    void Start()
    {
        /// 이지모바일 로그인
        StartCoroutine(initEM());
    }

    IEnumerator initEM()
    {
        /// 이지모바일  init
        RuntimeManager.Init();
        while (!RuntimeManager.IsInitialized())
        {
            yield return null;
        }

        // Grants the vendor-level consent for AdMob.
        Advertising.GrantDataPrivacyConsent(AdNetwork.AdMob);
        // Revokes the vendor-level consent of AdMob.
        Advertising.RevokeDataPrivacyConsent(AdNetwork.AdMob);
    }


}