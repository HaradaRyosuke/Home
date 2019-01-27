using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace GGJ2019.Akihabara.Team5
{
    public class Bullet : MonoBehaviour
    {
        public PlayerController2D my;
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f);
            foreach (Collider2D hit in hits)
            {
                if (hit.gameObject != gameObject)
                {
                    PlayerController2D player = hit.gameObject.GetComponent<PlayerController2D>();
                    if (player && player != my){
                        PhotonView ni = player.GetComponent<PhotonView>();

                        ni.RPC("BulletHit", RpcTarget.All, hit.transform.position);
                        Destroy(gameObject);
                        break;
                    }
                }
            }
        }
    }
}
