using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BreakBrickCoin : MonoBehaviour
{
    public bool broken;
    public GameObject prefab;
    public GameConstants gameConstants;

    public UnityEvent onBrickCoinBreak;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player") && !broken) {
            AudioSource breakBrickCoinAudio;
            onBrickCoinBreak.Invoke();
            breakBrickCoinAudio = gameObject.transform.parent.GetComponent<AudioSource>();
            breakBrickCoinAudio.PlayOneShot(breakBrickCoinAudio.clip);
            broken = true;
            // assume we have 5 debris per box
            for (int x = 0; x < gameConstants.debrisCount; x++) {
                Instantiate(prefab, transform.position, Quaternion.identity);
            }
            gameObject.transform.parent.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.transform.parent.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.transform.parent.Find("TopEdge").GetComponent<EdgeCollider2D>().enabled = false;
            GetComponent<EdgeCollider2D>().enabled = false;
        }
    }
}
