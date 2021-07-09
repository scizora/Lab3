using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerControllerEV : MonoBehaviour
{
    private float force;
    public IntVariable marioUpSpeed;
    public IntVariable marioMaxSpeed;
    public GameConstants gameConstants;

    private Rigidbody2D marioBody;
    private Animator marioAnimator;
    private SpriteRenderer marioSprite;

    public ParticleSystem sparkle;

    private bool isDead = false;
    private bool isAKeyUp = true;
    private bool isDKeyUp = true;
    private bool isSpacebarUp = true;
    private bool onGroundState = true;
    private bool faceRightState = true;

    public UnityEvent onPlayerDeath;
    public CustomCastEvent onPlayerCastPowerup;

    private AudioSource[] marioAudio;

    void PlayJumpSound() {
        marioAudio[0].PlayOneShot(marioAudio[0].clip);
    }

    void Start() {
        marioUpSpeed.SetValue(gameConstants.playerMaxJumpSpeed);
        marioMaxSpeed.SetValue(gameConstants.playerMaxSpeed);
        force = gameConstants.playerDefaultForce;

        marioBody = GetComponent<Rigidbody2D>();
        marioAnimator = GetComponent<Animator>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioAnimator.SetBool("onGround", onGroundState);
        marioAudio = GetComponents<AudioSource>();
    }
	
    void FixedUpdate()
    {
        if (!isDead)
        {
            if (onGroundState && marioBody.velocity.y != 0) {
                onGroundState = false;
                marioAnimator.SetBool("onGround", onGroundState);
            }
            //check if a or d is pressed currently
            if (!isAKeyUp || !isDKeyUp)
            {
                float direction = faceRightState ? 1.0f : -1.0f;
                marioSprite.flipX = faceRightState ? false : true;
                Vector2 movement = new Vector2(force * direction, 0);
                if (marioBody.velocity.magnitude < marioMaxSpeed.Value) {
                    marioBody.AddForce(movement);
                }
            }
            if (!isSpacebarUp && onGroundState)
            {
                PlayJumpSound();
                marioBody.AddForce(Vector2.up * marioUpSpeed.Value, ForceMode2D.Impulse);
                onGroundState = false;
                marioAnimator.SetBool("onGround", onGroundState);
            }
        }
    }

    void Update() {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
        if (Input.GetKeyDown("a")) {
            isAKeyUp = false;
            isDKeyUp = true; // force release
            faceRightState = false;
            if (onGroundState && marioBody.velocity.x > 1.0) {
                if (!marioAnimator.GetBool("onSkid")) {
                    marioAnimator.SetTrigger("onSkid");
                }
            }
        }
        else if (Input.GetKeyUp("a")) {
            isAKeyUp = true;
        }

        if (Input.GetKeyDown("d")) {
            isAKeyUp = true; // force release
            isDKeyUp = false;
            faceRightState = true;
            if (onGroundState && marioBody.velocity.x < -1.0) {
                if (!marioAnimator.GetBool("onSkid")) {
                    marioAnimator.SetTrigger("onSkid");
                }
            }
        }
        else if (Input.GetKeyUp("d")) {
            isDKeyUp = true;
        }

        if (Input.GetKeyDown("space")) {
            isSpacebarUp = false;
        }
        else if (Input.GetKeyUp("space")) {
            isSpacebarUp = true;
        }

        if (Input.GetKeyDown("z")) {
            onPlayerCastPowerup.Invoke(KeyCode.Z);
        }
        if (Input.GetKeyDown("x")) {
            onPlayerCastPowerup.Invoke(KeyCode.X);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") && !onGroundState) {
            onGroundState = true; // back on ground
            marioAnimator.SetBool("onGround", onGroundState);
            sparkle.Play();
        }
    }

    IEnumerator AnimateDeath() {
        marioBody.bodyType = RigidbodyType2D.Kinematic;
        float steps = 20.0f;
        float gravity = -9.8f;
        float initialVelocityY = 1.0f;
        marioBody.velocity = new Vector2(0.0f, 0.0f);
        gameObject.GetComponent<Collider2D>().enabled = false;
		for (int i = 0; i < steps; i++){
			// make sure enemy is still above ground
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + initialVelocityY * i/steps + 0.5f * gravity * i/steps * i/steps, this.transform.position.z);
			yield return null;
		}
        yield return new WaitForSeconds(2);
    }

    public void PlayerDiesSequence(){
        // Mario dies
        isDead = true;
        // marioAnimator.SetBool("isDead", isDead);
        Debug.Log("Mario dies");
        StartCoroutine(AnimateDeath());
        marioAudio[1].PlayOneShot(marioAudio[1].clip);
    }
}