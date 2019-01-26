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

    private ItemManager m_itemManager;
    private float m_myScore;
    public Text m_scoreText;

    void Start()
    {

    }


    private void FsixedUpdate()
    {
        
    }

    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
       if(true)
        {
            m_itemManager = collision.gameObject.GetComponent<ItemManager>();
            m_myScore += m_itemManager.m_value;
            Debug.Log("SCORE;" + m_myScore);
            Destroy(collision.gameObject);

            m_scoreText.text = (m_myScore).ToString();
        }
    }

}

