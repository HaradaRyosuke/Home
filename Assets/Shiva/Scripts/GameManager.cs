using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


using Photon.Pun;
using Photon.Realtime;
using System.Linq;


namespace GGJ2019.Akihabara.Team5
{
    public class GameManager : MonoBehaviourPunCallbacks
    {

        public Vector2 range;
        public Text statusText;
        public Text scoreText;

        public GameObject playerPrefab;
        public GameObject homePrefab;
        public GameObject lifeItemPrefab;

        public float spawnTime = 1f;
        public float tickTime = 1f;

        private Vector3 homePosition;

        public IEnumerator TimeTickRoutine() {

            statusText.text = "" + PhotonNetwork.IsMasterClient;

            while (true)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    PlayerController2D[] ps = FindObjectsOfType<PlayerController2D>();
                    foreach(PlayerController2D p in ps) {
                        p.GetComponent<PhotonView>().RPC("OnTimeTick", RpcTarget.All);
                    }
                    yield return new WaitForSeconds(tickTime);
                }
                yield return new WaitForEndOfFrame();
            }
        }

        public IEnumerator GameMasterRoutine()
        {

            statusText.text = "" + PhotonNetwork.IsMasterClient;

            while (true)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    Spawn(lifeItemPrefab);

                    yield return new WaitForSeconds(spawnTime);
                }
                yield return new WaitForEndOfFrame();

                UpdateScore();

            }
        }

        public void UpdateScore () {
            PlayerController2D[] ps = FindObjectsOfType<PlayerController2D>();
            var query = ps.OrderBy(x => x.age);
            string text = "";
            foreach(var (item, index) in query.Select((item, index) => (item, index))) {
                if (item.GetComponent<PhotonView>().IsMine){
                    text += "<b>#" + (index + 1) + " " + item.userName + " " + item.realAge + "</b>\n";
                }else {
                    text += "#" + (index + 1) + " " + item.userName + " " + item.realAge + "\n";
                }

            }
            scoreText.text = text;
        }


        private void Start()
        {
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                if (PlayerController2D.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    Vector2 pos = new Vector2(
                        UnityEngine.Random.Range(-range.x, range.x),
                        UnityEngine.Random.Range(-range.y, range.y));

                    Debug.Log(PhotonNetwork.NickName);

                    GameObject player = PhotonNetwork.Instantiate(this.playerPrefab.name, pos, Quaternion.identity, 0);
                    player.GetComponent<PhotonView>().RPC("SetName", RpcTarget.All, PhotonNetwork.NickName);

                    homePosition = pos + Vector2.left * 5;
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }

#if UNITY_5_4_OR_NEWER
            // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) =>
            {
                this.CalledOnLevelWasLoaded(scene.buildIndex);
            };
#endif

            StartCoroutine(GameMasterRoutine());
            StartCoroutine(TimeTickRoutine());
        }


        #if !UNITY_5_4_OR_NEWER
        /// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
        void OnLevelWasLoaded(int level)
        {
            this.CalledOnLevelWasLoaded(level);
        }
        #endif

        void CalledOnLevelWasLoaded(int level)
        {
            // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
            //if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            //{
                //transform.position = new Vector2(0f, 5f);
            //}
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }


        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
                //LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
                //LoadArena();
            }
        }

        public void Spawn(GameObject prefab) {

            PhotonNetwork.InstantiateSceneObject(
                prefab.gameObject.name, 
                new Vector2(UnityEngine.Random.Range(-range.x, range.x), 
                            UnityEngine.Random.Range(-range.y, range.y)), 
                Quaternion.identity, 0);
        }

        public void SpawnHome (){
            PhotonNetwork.Instantiate(this.homePrefab.name, homePosition, Quaternion.identity, 0);
        }


    }
}