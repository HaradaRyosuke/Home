using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//プレイヤーの移動・攻撃・アイテム回収？の制御
//移動：”WASD”　攻撃：スペース(仮)　アイテム回収：R(仮)
//子のModelを取得して向きの制御をする
//
//



public class PlayerControl : MonoBehaviour
{

    // 移動スピード
    [Header("移動速度")]
    public float speed = 5;



    [Header("生存時間")]
    public float m_maxTime = 120;
    [System.NonSerialized]
    public float timeElapsed;//残り時間
    private float timeOut = 0;//終了時間

    [Header("ライフバー")]
    public Transform m_lifeImage;//ライフバーのイメージ

    void Start()
    {
        timeElapsed = m_maxTime;//時間の代入
    }


    void Update()
    {

        // 右・左
        float x = Input.GetAxisRaw("Horizontal");

        // 上・下
        float y = Input.GetAxisRaw("Vertical");

        // 移動する向きを求める
        Vector2 direction = new Vector2(x, y).normalized;

        // 移動する向きとスピードを代入する
        GetComponent<Rigidbody2D>().velocity = direction * speed;



        timeElapsed -= Time.deltaTime;

        //ライフゲージを減らす
        float xx = (timeElapsed / m_maxTime) * 1.25f;
        m_lifeImage.localPosition = new Vector3(xx, 0, 0);

        if (timeElapsed <= timeOut)//残り時間が０になったか
        {
            // Do anything
            Debug.Log("時間経過");
            //timeElapsed = 0.0f;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("HITss");
    }
}
