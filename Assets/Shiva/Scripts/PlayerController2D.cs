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

        public int point = 100;
        public int age = 0;
        public int realAge = 0;

        public Text pointText;

        private Rigidbody2D rigidbody2D;

        public Transform character;

        private Vector3 lastPosition;
        private Vector3 dir = Vector3.right;

        public string userName = "";

        public Rigidbody bullet;
        public float bulletSpeed;

        private bool isDead;
        private bool isDeadSequence;

        public DeadSequence deadSequencePrefab;
        private GameManager manager;

        public LifeBar lifeBar;

        private void Awake()
        {
            if (photonView.IsMine)
            {
                PlayerController2D.LocalPlayerInstance = gameObject;
                //gameObject.name = "Mine";
                FindObjectOfType<FollowTarget>().target = transform;
            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            isDead = false;
            rigidbody2D = GetComponent<Rigidbody2D>();
            lastPosition = transform.position;
            manager = FindObjectOfType<GameManager>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if(isDead) {
                if(!isDeadSequence){

                    DeadSequence go = Instantiate(deadSequencePrefab, Vector3.zero, Quaternion.identity);
                    go.userName = userName;
                    go.age = "" + realAge;
                    go.Run();
                    isDeadSequence = true;
                    Highscore.SaveHighScore(userName, realAge);
                }
                return;
            }
            Vector3 diff = transform.position - lastPosition;
            lastPosition = transform.position;
            diff = diff.normalized;
            if(diff.magnitude > 0){
                dir = diff;
            }
            character.gameObject.SendMessage("SetDirection", new Vector2(diff.x, diff.y), SendMessageOptions.DontRequireReceiver);

            pointText.text = "" + userName + "," + age;

            if (!photonView.IsMine && PhotonNetwork.IsConnected)
            {
                return;
            }

            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            Vector2 vec = new Vector2(x, y).normalized;
            rigidbody2D.velocity = Vector2.Lerp(rigidbody2D.velocity, ( vec * speed), 0.5f);

            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, -manager.range.x-5, manager.range.x+5),
                Mathf.Clamp(transform.position.y, -manager.range.y-5, manager.range.y+5),
                0
            );

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f);
            foreach(Collider2D hit in hits) {
                if (hit.gameObject != gameObject) {
                    PhotonView ni = hit.gameObject.GetComponent<PhotonView>();
                    if (ni && ni.gameObject.GetComponent<NetworkedItem>()){
                        hit.gameObject.SendMessage(
                            "OnCollideLocal",
                            this,
                            SendMessageOptions.DontRequireReceiver);
                        ni.RPC("OnCollide", RpcTarget.All);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                GetComponent<PhotonView>().RPC("SetAttack", RpcTarget.All, dir);
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

            if (realAge % 50 == 0)
            {
                if (!manager)
                {
                    manager = FindObjectOfType<GameManager>();
                }
                manager.SpawnHome();
            }

            point -= Mathf.Max(0, Mathf.RoundToInt(Mathf.Log(age, 2)));
            lifeBar.SetLife(point);
            age++;
            realAge++;

            if (point <= 0) {
                isDead = true;
            }
        }

        [PunRPC]
        //private void OnCollide(Player player)
        private void SetName(string name)
        {
            this.userName = name;
        }

        [PunRPC]
        private void SetAttack(Vector3 dir)
        {
            character.SendMessage("Attack", SendMessageOptions.DontRequireReceiver);

            Rigidbody b = Instantiate<Rigidbody>(bullet, transform.position, Quaternion.identity);
            b.GetComponent<Bullet>().my = this;
            b.AddRelativeForce(dir * bulletSpeed);
            Destroy(b.gameObject, 2f);
        }

        [PunRPC]
        private void BulletHit(Vector3 pos)
        {
            if(GetComponent<PhotonView>().IsMine) {
                isDead = true;
            }
            character.SendMessage("Dead");
        }
    }
}