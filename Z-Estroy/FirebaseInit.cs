using Firebase.Analytics;
using UnityEngine;

public class FirebaseInit : MonoBehaviour
{
    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Set a flag here to indicate whether Firebase is ready to use by your app.
                Login();
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

    }

    void Login()
    {
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        // Log an event with no parameters.
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
        //
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;

    }

    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {

        ////string notification = e.Message.Notification.Body;
        //string notification = e.Message.Notification.Body;
        ////var notification = e.Message.Data;
        ////var eData = e.Message.Data;

        //if (e.Message.Data.Count > 0)
        //{
        //    Debug.Log("data:");
        //    foreach (System.Collections.Generic.KeyValuePair<string, string> iter in e.Message.Data)
        //    {
        //        Debug.Log("  " + iter.Key + ": " + iter.Value);
        //        notification = iter.Value;
        //    }
        //}
        //else
        //{
        //    return;
        //}

        //// 예시) 펭수 기념 *국밥*100*그릇 드립니다.
        //Debug.LogWarning("파이어 베이스 몸통 메시지: " + notification);

        //string[] sDataArray = notification.Split('*');

        //// 아이템 + 수량 조합으로 끊어 먹을 것
        //int cutIndex = 0;
        //string itemCode = "";

        //// 출력할 스트링
        //string targetString = "";

        //for (int i = 0; i < sDataArray.Length; i++)
        //{
        //    if (sDataArray[i] == "국밥")
        //    {
        //        itemCode = "gupbap";
        //        cutIndex = i;
        //        break;
        //    }
        //    else if (sDataArray[i] == "열쇠")
        //    {
        //        itemCode = "key";
        //        cutIndex = i;
        //        break;
        //    }
        //    else if (sDataArray[i] == "쌀밥")
        //    {
        //        itemCode = "ssal";
        //        cutIndex = i;
        //        break;
        //    }
        //    else if (sDataArray[i] == "다이아")
        //    {
        //        itemCode = "diamond";
        //        cutIndex = i;
        //        break;
        //    }
        //}


        //for (int j = 0; j < cutIndex; j++)
        //{
        //    targetString += sDataArray[j];
        //}

        //targetString += sDataArray[cutIndex] + " ";
        //targetString += sDataArray[cutIndex + 1];

        //for (int j = cutIndex + 2; j < sDataArray.Length; j++)
        //{
        //    targetString += sDataArray[j];
        //}

        //Debug.LogWarning("분해후 재조합 문자 : " + targetString);
        //Debug.LogWarning("아이템 코드 : " + sDataArray[cutIndex]);
        //Debug.LogWarning("수량 : " + sDataArray[cutIndex + 1]);

        //// PostboxItemSend(string _code, int _amount, string _msg)

        //GameObject.Find("PlayNanoo").GetComponent<PlayNANOOExample>().PostboxItemSend(itemCode, int.Parse(sDataArray[cutIndex + 1]), targetString);

    }


}
