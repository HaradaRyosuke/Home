using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GGJ2019.Akihabara.Team5
{
    public class Home : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        //public void OnCollideLocal(PlayerController2D p)
        //{
        //    p.point++;
        //}

        public void OnCollide()
        {
            Debug.Log("you came back home");
        }
    }
}
