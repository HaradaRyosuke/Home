using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace GGJ2019.Akihabara.Team5
{
    public class ItemLife : MonoBehaviour
    {
        public int point = 5;
        public int timeToDie = 10;
        public GameObject m_particalObj;
        public AudioClip clip;


        private float dyingTime = 0;
        // Start is called before the first frame update
        void Start()
        {
            dyingTime = Time.time + timeToDie;
        }


        // Update is called once per frame
        void Update()
        {
            if (Time.time > dyingTime) {
                if (transform.parent.GetComponent<PhotonView>().IsMine)
                {
                    PhotonNetwork.Destroy(transform.parent.gameObject);
                }
            }
        }

        public void OnCollideLocal(PlayerController2D p)
        {
            GameObject go = new GameObject();
            go.transform.position = transform.position;
            AudioSource a = go.AddComponent<AudioSource>();
            a.PlayOneShot(clip, 0.2f);
            Destroy(go, 5f);

            p.point += point;
            p.point = Mathf.Min(p.point, 100);
        }

        public void OnCollide()
        {
            Instantiate(m_particalObj, transform.parent.localPosition, Quaternion.identity);
            if (transform.parent.GetComponent<PhotonView>().IsMine)
            {
                PhotonNetwork.Destroy(transform.parent.gameObject);
            }
        }
    }
}