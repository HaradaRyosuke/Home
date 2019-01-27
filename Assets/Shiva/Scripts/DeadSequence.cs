using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class DeadSequence : MonoBehaviour
{

    public Text text;
    public Button restart;

    public string userName;
    public string age;

    public AudioClip audio;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        restart.gameObject.SetActive(false);
        text.text = "";
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Sequence () {
        while(!audioSource) {
            yield return new WaitForEndOfFrame();
        }


        audioSource.PlayOneShot(audio, 0.5f);
        yield return new WaitForSeconds(1f);

        text.text = userName + "\n";
        audioSource.PlayOneShot(audio, 0.5f);

        yield return new WaitForSeconds(1f);

        text.text = userName + "\n" + "age "+ age;
        audioSource.PlayOneShot(audio, 0.5f);

        yield return new WaitForSeconds(1f);
        restart.gameObject.SetActive(true);

    }

    public void Run () {
        StartCoroutine(Sequence());
    }
}
