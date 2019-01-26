using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;

namespace GGJ2019.Akihabara.Team5
{
    public class PlayerController2D : MonoBehaviourPunCallbacks, IPunObservable
    {

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        public float speed = 10.0f;

        private Rigidbody2D rigidbody2D;

        private void Awake()
        {
            if (photonView.IsMine)
            {
                PlayerController2D.LocalPlayerInstance = this.gameObject;
                gameObject.name = "Mine";   
            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!photonView.IsMine && PhotonNetwork.IsConnected)
            {
                return;
            }

            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            rigidbody2D.velocity = Vector2.Lerp(rigidbody2D.velocity, (new Vector2(x, y) * speed), 0.5f);

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f);
            foreach(Collider2D hit in hits) {
                if (hit.gameObject != gameObject) {
                    Debug.Log(hit.gameObject);
                    PhotonView ni = hit.gameObject.GetComponent<PhotonView>();
                    if (ni && ni.gameObject.GetComponent<NetworkedItem>()){
                        ni.RPC("OnCollide", RpcTarget.All);
                    }
                }
            }

        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}"); // not seen if you're the player connecting


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                //LoadArena();
            }
        }

        #region IPunObservable implementation
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                Debug.Log("sending");
                stream.SendNext(true);
            }
            else
            {
                // Network player, receive data
                bool b = (bool)stream.ReceiveNext();
                Debug.Log(b);
            }
        }
        #endregion
    }
}