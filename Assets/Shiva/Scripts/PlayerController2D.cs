using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;

namespace GGJ2019.Akihabara.Team5
{
    public class PlayerController2D : MonoBehaviourPunCallbacks, IPunObservable
    {

        public static GameObject LocalPlayerInstance;

        public float speed = 10.0f;

        public int point = 0;
        public int age = 0;

        public Text pointText;

        private Rigidbody2D rigidbody2D;

        private void Awake()
        {
            if (photonView.IsMine)
            {
                PlayerController2D.LocalPlayerInstance = gameObject;
                gameObject.name = "Mine";
                FindObjectOfType<FollowTarget>().target = transform;
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
            pointText.text = "" + point + "," + age;

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
                    PhotonView ni = hit.gameObject.GetComponent<PhotonView>();
                    if (ni && ni.gameObject.GetComponent<NetworkedItem>()){
                        ni.RPC("OnCollide", RpcTarget.All);

                        Debug.Log(hit.gameObject);
                        hit.gameObject.SendMessage(
                            "OnCollideLocal",
                            this,
                            SendMessageOptions.DontRequireReceiver);
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
                stream.SendNext(point);
            }
            else
            {
                // Network player, receive data
                point = (int)stream.ReceiveNext();
            }
        }
        #endregion

        [PunRPC]
        //private void OnCollide(Player player)
        private void OnTimeTick()
        {
            point -= Mathf.Max(0, Mathf.RoundToInt(Mathf.Log(age, 2)));
            age++;
        }
    }
}