using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour
{
    [SerializeField]
    private RectTransform m_ItemBase;
    [SerializeField]
    int m_instantiateItemCount = 9;
    float m_itemWidth = 400;
    float m_itemHeight = 710;
    [SerializeField]
    RectTransform m_ParntVeiport;
    [SerializeField]
    float gap = 12.5f;
    public Direction direction;
    [System.NonSerialized]
    public List<RectTransform> m_itemList = new List<RectTransform>();
    protected float m_diffPreFramePosition = 0;
    [SerializeField]
    private int m_currentItemNo = 0;
    public ScrollRect scrollRect;
    public int listData;
    public enum Direction
    {
        Vertical,
        Horizontal,
    }

    // cache component
    public RectTransform m_Content;
    /// <summary>
    /// 100개 짜리 컨텐츠 가로 사이즈
    /// </summary>
    private float SOY_CHIKEN = 20631.25f;

    private float AnchoredPosition
    {
        get
        {
            return (direction == Direction.Vertical) ?
                -m_Content.anchoredPosition.y:
                m_Content.anchoredPosition.x - SOY_CHIKEN;
        }
    }
    private float ItemScale
    {
        get
        {
            return (direction == Direction.Vertical) ?
                m_itemHeight :
                m_itemWidth;
        }
    }

    void Start()
    {
        m_itemWidth = m_ItemBase.rect.width;
        m_itemHeight = m_ItemBase.rect.height;
    }

    public void ListStartShield()
    {
        SOY_CHIKEN = 6189.375f;
        listData = PlayerPrefsManager.GetInstance().shieldInfo.Count;

        Debug.LogError("listData :: " + listData);

        if (listData < m_instantiateItemCount)
        {
            m_instantiateItemCount = listData;
        }
        else
        {
            m_instantiateItemCount = (direction == Direction.Vertical) ?
                Mathf.RoundToInt(m_ParntVeiport.rect.height / ItemScale) + 3 :
                Mathf.RoundToInt(m_ParntVeiport.rect.width / ItemScale) + 3;
        }

        // create items
        scrollRect.horizontal = direction == Direction.Horizontal;
        scrollRect.vertical = direction == Direction.Vertical;

        if (direction == Direction.Vertical)
        {
            m_Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (ItemScale + gap) * (listData - 1) + gap);
            m_Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_itemWidth + gap * 2);
        }
        else
        {
            m_Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (ItemScale + gap) * listData + gap);
            m_Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (m_itemHeight + gap * 2) - 25f);
            //m_Content.sizeDelta = new Vector2(0, (ItemScale + gap) * (listData) + gap);
        }

        scrollRect.onValueChanged.AddListener(valueChange);
        Debug.Log(listData);
        m_ItemBase.gameObject.SetActive(false);
        for (int i = 0; i < m_instantiateItemCount; i++)
        {
            var item = GameObject.Instantiate(m_ItemBase) as RectTransform;
            item.SetParent(transform, false);
            item.name = i.ToString();

            if (direction == Direction.Vertical)
            {
                item.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_itemWidth - gap * 2);
                item.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_itemHeight);
            }
            else
            {
                item.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_itemWidth);
                item.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_itemHeight);
            }

            item.anchoredPosition =
                (direction == Direction.Vertical) ?
                new Vector2(gap, -gap - (ItemScale + gap) * i) :
                new Vector2((ItemScale + gap) * i + (ItemScale / 2), 0);

            m_itemList.Add(item);

            item.gameObject.SetActive(true);
            item.GetComponent<Item>().UpdateItem(i);
        }
    }


    public void ListStart()
    {
        listData = PlayerPrefsManager.GetInstance().weaponInfo.Count;

        if (listData < m_instantiateItemCount)
        {
            m_instantiateItemCount = listData;
        }
        else
        {
            m_instantiateItemCount = (direction == Direction.Vertical) ?
                Mathf.RoundToInt(m_ParntVeiport.rect.height / ItemScale) + 3 :
                Mathf.RoundToInt(m_ParntVeiport.rect.width / ItemScale) + 3;
        }

        // create items
        scrollRect.horizontal = direction == Direction.Horizontal;
        scrollRect.vertical = direction == Direction.Vertical;

        if (direction == Direction.Vertical)
        {
            m_Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (ItemScale + gap) * (listData - 1) + gap);
            m_Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_itemWidth + gap * 2);
        }
        else
        {
            m_Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (ItemScale + gap) * listData + gap);
            m_Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (m_itemHeight + gap * 2) - 25f);
            //m_Content.sizeDelta = new Vector2(0, (ItemScale + gap) * (listData) + gap);
        }

        scrollRect.onValueChanged.AddListener(valueChange);
        Debug.Log(listData);
        m_ItemBase.gameObject.SetActive(false);
        for (int i = 0; i < m_instantiateItemCount; i++)
        {
            var item = GameObject.Instantiate(m_ItemBase) as RectTransform;
            item.SetParent(transform, false);
            item.name = i.ToString();

            if (direction == Direction.Vertical)
            {
                item.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_itemWidth - gap * 2);
                item.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_itemHeight);
            }
            else
            {
                item.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_itemWidth);
                item.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_itemHeight);
            }

            item.anchoredPosition =
                (direction == Direction.Vertical) ?
                new Vector2(gap, -gap - (ItemScale + gap) * i) :
                new Vector2((ItemScale + gap) * i + (ItemScale / 2), 0);

            m_itemList.Add(item);

            item.gameObject.SetActive(true);
            item.GetComponent<Item>().UpdateItem(i);
        }
    }

    private void valueChange(Vector2 _pos)
    {
        // scroll up, item attach bottom  or  right
        while (AnchoredPosition - m_diffPreFramePosition < -(ItemScale + gap) * 2)
        {
            m_diffPreFramePosition -= (ItemScale + gap);

            var item = m_itemList[0];
            m_itemList.RemoveAt(0);
            m_itemList.Add(item);

            var pos = (ItemScale + gap) * m_instantiateItemCount + (ItemScale + gap) * m_currentItemNo;
            item.anchoredPosition = (direction == Direction.Vertical) ? 
                new Vector2(gap, -pos - gap) : 
                new Vector2(pos + (ItemScale / 2) + gap, 0);

            m_currentItemNo++;

            if (m_currentItemNo + m_instantiateItemCount < listData + 1)
            {
                item.GetComponent<Item>().UpdateItem(m_currentItemNo + m_instantiateItemCount - 1);
            }
            else
            {
                item.GetComponent<Item>().UpdateItem(-100);
            }
        }

        // scroll down, item attach top  or  left
        while (AnchoredPosition - m_diffPreFramePosition > 0)
        {
            m_diffPreFramePosition += (ItemScale + gap);

            var itemListLastCount = m_instantiateItemCount - 1;
            var item = m_itemList[itemListLastCount];
            m_itemList.RemoveAt(itemListLastCount);
            m_itemList.Insert(0, item);

            m_currentItemNo--;

            var pos = (ItemScale + gap) * m_currentItemNo + gap;
            item.anchoredPosition = (direction == Direction.Vertical) ? 
                new Vector2(gap, -pos) : 
                new Vector2(pos + (ItemScale / 2) - gap, 0);

            if (m_currentItemNo > -1)
            {
                item.GetComponent<Item>().UpdateItem(m_currentItemNo);
            }
            else
            {
                item.GetComponent<Item>().UpdateItem(-100);
            }
        }
    }
}