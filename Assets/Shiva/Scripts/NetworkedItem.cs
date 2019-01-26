﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace GGJ2019.Akihabara.Team5
{
    public class NetworkedItem : MonoBehaviourPunCallbacks, IPunObservable
    {
        public GameObject prefab;

        private GameObject instance;
        // Start is called before the first frame update
        // Start is called before the first frame update
        void Start()
        {
            //rigidbody2D = GetComponent<Rigidbody2D>();
            instance = Instantiate<GameObject>(prefab, transform.position, transform.rotation);
        }


        // Update is called once per frame
        void Update()
        {

        }

        #region IPunObservable implementation
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            //if (stream.IsWriting)
            //{
            //    // We own this player: send the others our data
            //    Debug.Log("sending");
            //    stream.SendNext(true);
            //}
            //else
            //{
            //    // Network player, receive data
            //    bool b = (bool)stream.ReceiveNext();
            //    Debug.Log(b);
            //}
        }
        #endregion

        [PunRPC]
        //private void OnCollide(Player player)
        private void OnCollide()
        {
            Debug.Log("OnCollide");
            Destroy(instance);
        }
    }

}