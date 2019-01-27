using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace GGJ2019.Akihabara.Team5
{
    public class Home : MonoBehaviour
    {

        public int age = 50;

        public GameObject m_particalObj;
        public AudioClip clip;
        // Start is called before the first frame update
        void Start()
        {

        }

        public void OnCollideLocal(PlayerController2D p)
        {
            Debug.Log("OnCollideLocal");
            if (clip)
            {
                GameObject go = new GameObject();
                go.transform.position = transform.position;
                
                AudioSource a = go.AddComponent<AudioSource>();
                a.PlayOneShot(clip, 0.2f);
                Destroy(go, 5f);
            }

            p.age -= 50;
            p.age = Mathf.Max(0, p.age);

        }

        public void OnCollide()
        {
            if (m_particalObj) {
                Instantiate(m_particalObj, transform.parent.localPosition, Quaternion.identity);
            }

            if (transform.parent.GetComponent<PhotonView>().IsMine)
            {
                PhotonNetwork.Destroy(transform.parent.gameObject);
            }
        }
    }
}
