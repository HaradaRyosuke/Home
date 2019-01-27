using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ2019.Akihabara.Team5
{
    public class SoldierScript : MonoBehaviour
    {
        private Animator controller;

        public float speed;

        // Start is called before the first frame update
        void Start()
        {
            controller = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            controller.SetFloat("Speed", speed);
        }

        public void SetDirection(Vector2 dir)
        {
            if (dir.x < 0) {
                transform.localScale = new Vector3(-1, 1, 1);
            } else if (dir.x == 0){
                //nop
            } else
            {
                transform.localScale = Vector3.one;
            }
            this.speed = dir.magnitude;
        }

        public void Attack () {
            controller.SetTrigger("Attack");
        }

        public void Dead () {
            controller.SetBool("Dead", true);
        }
    }
}
