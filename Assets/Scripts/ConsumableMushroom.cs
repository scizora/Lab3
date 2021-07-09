using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableMushroom : MonoBehaviour
{
    public Vector2 velocityBefore;
    public Rigidbody2D rigidBody;
    public BoxCollider2D coll;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        // rigidBody.AddForce(Vector2.up * 8, ForceMode2D.Impulse);
        coll = GetComponent<BoxCollider2D>();
        coll.sharedMaterial.friction = 0;
        coll.enabled = false;
        coll.enabled = true;

        int randomInt = Random.Range(0, 2);
        rigidBody.AddForce((randomInt == 0 ? Vector2.left : Vector2.right) * 3, ForceMode2D.Impulse);
    }
    
    void FixedUpdate() {
        velocityBefore = rigidBody.velocity;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.gameObject.tag);
        if (col.gameObject.CompareTag("Pipe")) {
            rigidBody.velocity = new Vector2(velocityBefore.x * -1, velocityBefore.y);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Goomba")) {
            Destroy(gameObject);
        }    
    }
}
