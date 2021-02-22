using UnityEngine;
using System.Collections;
using EasyMobile; // include the Easy Mobile namespace to use its scripting API

public class EasyMobileInitializer : MonoBehaviour
{

    // Checks if EM has been initialized and initialize it if not.
    // This must be done once before other EM APIs can be used.
    void Awake()
    {
        if (!RuntimeManager.IsInitialized())
            RuntimeManager.Init();
    }

    public void AdmobInit()
    {
        // Grants the vendor-level consent for AdMob.
        Advertising.GrantDataPrivacyConsent(AdNetwork.AdMob);
        Advertising.GrantDataPrivacyConsent(AdNetwork.AudienceNetwork);

        // Revokes the vendor-level consent of AdMob.
        Advertising.RevokeDataPrivacyConsent(AdNetwork.AdMob);
        Advertising.RevokeDataPrivacyConsent(AdNetwork.AudienceNetwork);

        // Reads the current vendor-level consent of AdMob.
        //ConsentStatus admobConsent = Advertising.GetDataPrivacyConsent(AdNetwork.AdMob);
    }


    public void ShowBannerAd()
    {
        // Show banner ad
        Advertising.ShowBannerAd(BannerAdPosition.Top, BannerAdSize.SmartBanner);
        //Time.timeScale = 1.1f;
    }

    public void HideBannerAd()
    {
        // Hide banner ad
        Advertising.HideBannerAd();
        //Time.timeScale = 1;
    }

    


}