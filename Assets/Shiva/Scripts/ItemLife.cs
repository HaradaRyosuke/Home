using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GGJ2019.Akihabara.Team5
{
    public class ItemLife : MonoBehaviour
    {
        public int point = 5;
        public int timeToDie = 10;
        public GameObject m_particalObj;


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
                Destroy(transform.parent.gameObject);
            }
        }

        public void OnCollideLocal(PlayerController2D p)
        {
            p.point += point;
        }

        public void OnCollide()
        {
            Instantiate(m_particalObj, transform.parent.localPosition, Quaternion.identity);
            Destroy(transform.parent.gameObject);
        }
    }
}