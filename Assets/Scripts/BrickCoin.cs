using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickCoin : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    public GameConstants gameConstants;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.AddForce(Vector2.up * 8, ForceMode2D.Impulse);
        Destroy(gameObject, 0.5f);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
