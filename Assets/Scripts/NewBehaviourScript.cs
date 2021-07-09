using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public float speed, upSpeed;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    public float maxSpeed;
    private bool onGroundState = true;
    private bool jumpState = false;

    private Animator marioAnimator;
    private AudioSource[] marioAudio;

    public ParticleSystem sparkle;

    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();

        marioAnimator = GetComponent<Animator>();
        marioAnimator.SetBool("onGround", onGroundState);

        marioAudio = GetComponents<AudioSource>();

        // subscribe to player event
        GameManager.OnPlayerDeath += PlayerDiesSequence;
    }

    // FixedUpdate may be called once per frame. See documentation for details.
    void FixedUpdate()
    {
        if (Input.GetKeyDown("space") && onGroundState)
        {
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            marioAnimator.SetBool("onGround", onGroundState);
            jumpState = true;
        }

        // dynamic rigidbody
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(moveHorizontal) > 0)
        {
            Vector2 movement = new Vector2(moveHorizontal, 0);
            if (marioBody.velocity.magnitude < maxSpeed)
                marioBody.AddForce(movement * speed);
        }

        if (onGroundState)
        {
            if (marioBody.velocity.y != 0)
            {
                onGroundState = false;
                marioAnimator.SetBool("onGround", onGroundState);
            }
            if ((!Input.GetKey("a") && !Input.GetKey("d")))
            {
                // stop
                marioBody.velocity = Vector2.zero;
            }
        }
        else if (!onGroundState)
        {
            if (marioBody.velocity.y == 0)
            {
                if (jumpState)
                {
                    jumpState = false;
                }
                onGroundState = true;
                marioAnimator.SetBool("onGround", onGroundState);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));

        // toggle state
        if (Input.GetKeyDown("a") && faceRightState)
        {
            if (onGroundState && Mathf.Abs(marioBody.velocity.x) > 1.0)
            {
                marioAnimator.SetTrigger("onSkid");
            }
            faceRightState = false;
            marioSprite.flipX = true;
        }

        if (Input.GetKeyDown("d") && !faceRightState)
        {
            if (onGroundState && Mathf.Abs(marioBody.velocity.x) > 1.0)
            {
                marioAnimator.SetTrigger("onSkid");
            }
            faceRightState = true;
            marioSprite.flipX = false;
        }

        if (Input.GetKeyDown("z"))
        {
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.Z, this.gameObject);
        }

        if (Input.GetKeyDown("x"))
        {
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.X, this.gameObject);
        }
    }

    void PlayJumpSound()
    {
        if (jumpState)
        {
            marioAudio[0].PlayOneShot(marioAudio[0].clip);
        }
    }

    // called when the cube hits the floor
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") && !onGroundState)
        {
            onGroundState = true; // back on ground
            marioAnimator.SetBool("onGround", onGroundState);
            if (jumpState)
            {
                jumpState = false;
            }
            sparkle.Play();
        }
        // else if (col.gameObject.CompareTag("Obstacles") && Mathf.Abs(marioBody.velocity.y) < 0.01f) {
        //     Debug.Log("Collided");
        //     sparkle.Play();
        //     onGroundState = true; // back on ground
        //     marioAnimator.SetBool("onGround", onGroundState);
        //     if (jumpState) {
        //         jumpState = false;
        //     }
        // }
    }

    // void ShowRestart() {
    //     Camera camera = Camera.main;
    //     camera.backgroundColor = Color.black;
    //     camera.clearFlags = CameraClearFlags.SolidColor;
    //     GameObject cameraObject = GameObject.Find("Main Camera");
    //     cameraObject.GetComponent<CameraController>().enabled = false;
    //     GameObject mainGameObject = GameObject.Find("UI").GetComponent<MenuController>().mainGameObject;
    //     Destroy(mainGameObject);
    //     Transform transformUI = GameObject.Find("UI").transform;
    //     foreach (Transform eachChild in transformUI) {
    //         if (eachChild.name != "Score" && eachChild.name != "PowerupSlot1" && eachChild.name != "PowerupSlot2") {
    //             Debug.Log("Child found. Name: " + eachChild.name);
    //             // enable them
    //             eachChild.gameObject.SetActive(true);
    //         }
    //     }
    // }

    IEnumerator AnimateDeath()
    {
        marioBody.bodyType = RigidbodyType2D.Kinematic;
        float steps = 20.0f;
        float gravity = -9.8f;
        float initialVelocityY = 1.0f;
        marioBody.velocity = new Vector2(0.0f, 0.0f);
        gameObject.GetComponent<Collider2D>().enabled = false;
        for (int i = 0; i < steps; i++)
        {
            // make sure enemy is still above ground
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + initialVelocityY * i / steps + 0.5f * gravity * i / steps * i / steps, this.transform.position.z);
            yield return null;
        }
        yield return new WaitForSeconds(2);
    }

    void PlayerDiesSequence()
    {
        // Mario dies
        Debug.Log("Mario dies");
        StartCoroutine(AnimateDeath());
        marioAudio[1].PlayOneShot(marioAudio[1].clip);
    }
}
