﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostCart : MonoBehaviour
{

    string tmpHead = "";
    string tmpTail = "";

    /// <summary>
    /// 포스트 매니저에서 불러올것
    /// </summary>
    public void SetPostContent(string _code, string _amount, string _uid, string _message)
    {
        /// 메세지 비어 있으면 본래대로 표기
        if (_message == "null" || _message == null || _message == string.Empty)
        {
            /// 번역
            if (Lean.Localization.LeanLocalization.CurrentLanguage == "Korean")
            {
                switch (_code)
                {
                    case "diamond":
                        tmpHead = "다이아몬드 ";
                        tmpTail = " 개.";
                        break;
                    case "gold":
                        tmpHead = "";
                        tmpTail = " 골드.";
                        break;
                    case "key":
                        tmpHead = "열쇠 ";
                        tmpTail = " 개.";
                        break;
                    case "gupbap":
                        tmpHead = "국밥 ";
                        tmpTail = " 개.";
                        break;
                    case "ssal":
                        tmpHead = "쌀밥 ";
                        tmpTail = " 개.";
                        break;
                    case "ticket":
                        tmpHead = "입장권 ";
                        tmpTail = " 장.";
                        break;

                    //

                    case "kimchi":
                        tmpHead = "깍두기 ";
                        tmpTail = " 개.";
                        break;
                    case "shield":
                        tmpHead = "방패 뽑기권 ";
                        tmpTail = " 장.";
                        break;
                }
            }
            else
            {
                switch (_code)
                {
                    case "diamond":
                        tmpHead = "Diamonds ";
                        tmpTail = " .";
                        break;
                    case "gold":
                        tmpHead = "Golds";
                        tmpTail = " .";
                        break;
                    case "key":
                        tmpHead = "Keys ";
                        tmpTail = " .";
                        break;
                    case "gupbap":
                        tmpHead = "Korean soups ";
                        tmpTail = " .";
                        break;
                    case "ssal":
                        tmpHead = "Rices ";
                        tmpTail = " .";
                        break;
                    case "ticket":
                        tmpHead = "Tikets ";
                        tmpTail = " .";
                        break;

                    //

                    case "kimchi":
                        tmpHead = "Radish kimchi ";
                        tmpTail = " .";
                        break;
                    case "shield":
                        tmpHead = "Shield tikets ";
                        tmpTail = " .";
                        break;
                }
            }

            transform.GetChild(1).GetComponent<Text>().text = tmpHead + _amount + tmpTail;
        }
        else  // 메세지 내용이 있으면 여기
        {
            transform.GetChild(1).GetComponent<Text>().text = _message;
        }


        transform.GetChild(3).GetComponent<Text>().text = _uid;
    }

    public void Cart_Clicked()
    {
        var playNANOO = GameObject.Find("PlayNanoo").GetComponent<PlayNANOOExample>();
        playNANOO.PostboxItemUse(transform.GetChild(3).GetComponent<Text>().text);

        Lean.Pool.LeanPool.Despawn(transform);
    }
}
