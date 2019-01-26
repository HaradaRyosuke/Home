using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//得点の取得
//アイテム
//残り生存時間
//

public class PlayerStatus : MonoBehaviour
{
    private PlayerControl m_playerControl;
    private ItemManager m_itemManager;//物が当たった瞬間に取得するもの
    private float m_myScore;//自分のスコアの格納

    [Header("モデルを取得")]
    public GameObject m_model;
    private GameObject[] m_PlayerState = new GameObject[5];//プレイヤーの状態モデル
    private int m_playerCase;
    [Header("モデル内のライフゲージ取得")]
    public GameObject m_lifeGaje;

    [Header("スコアボード用textの取得")]
    public Text m_scoreText;//スコアを書く場所

    [Header("帰宅時用Text")]
    public GameObject m_homeText;

    private int m_versionNam = 0;

    void Start()
    {
        m_myScore = 0;
        m_playerControl = this.gameObject.GetComponent<PlayerControl>();

        int i = 0;//カウント用
        foreach(Transform child in m_model.transform)
        {
            m_PlayerState[i] = child.gameObject;
            i++;
        }
    }


    void Update()
    {
        if(m_myScore >= 80 && m_versionNam == 0)
        {

            m_versionNam = 1;
            m_PlayerState[0].SetActive(false);
            m_PlayerState[1].SetActive(true);
        }
        else if(m_myScore >= 150 && m_versionNam == 1)
        {
            m_versionNam = 2;
            m_PlayerState[1].SetActive(false);
            m_PlayerState[2].SetActive(true);
        }
        else if(m_myScore >= 200 && m_versionNam == 2)
        {
            m_versionNam = 3;
            m_PlayerState[2].SetActive(false);
            m_PlayerState[3].SetActive(true);
        }
        else if (m_myScore >= 300 && m_versionNam == 4)
        {
            m_versionNam = 4;
            m_PlayerState[3].SetActive(false);
            m_PlayerState[4].SetActive(true);
        }

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.gameObject.tag == "Item")
        {
            m_itemManager = collision.gameObject.GetComponent<ItemManager>();

            //スコアアイテムの場合
            if(m_itemManager.m_itemType == ItemManager.ItemType.ScoreItem)
            {
                //スコアの加算
                m_myScore += m_itemManager.m_value;
            }

            if(m_itemManager.m_itemType == ItemManager.ItemType.SpeedItem)
            {
                m_playerControl.speed += m_itemManager.m_value;
            }
            if(m_itemManager.m_itemType == ItemManager.ItemType.TimeItem)
            {
                if((m_playerControl.timeElapsed + m_itemManager.m_value) >= 120)
                {
                    float overflowValue = 0;
                    overflowValue = (m_playerControl.timeElapsed + m_itemManager.m_value) - 120;
                    m_playerControl.timeElapsed = 120;
                    //スコアの加算
                    m_myScore = Mathf.FloorToInt(overflowValue * 2);
                }
            }

            //アイテムの消去
            Destroy(collision.gameObject);
            //スコアの適応
            m_scoreText.text = (m_myScore).ToString();
        }

        if (collision.gameObject.tag == "Home")
        {
            m_homeText.SetActive(true);
            m_model.SetActive(false);
            m_lifeGaje.SetActive(false);
        }

    }


    //プレイヤーのオブジェクトの変更
    public void PChangePlayerObject ()
    {
        switch(m_playerCase)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;

        }
    }

}

