using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyControllerEV : MonoBehaviour
{
	public GameConstants gameConstants;
	private int moveRight;
	private float originalX;
	private Vector2 velocity;
	private Rigidbody2D enemyBody;
    private Rigidbody2D marioBody;

	private Animator enemyAnimator;

	private bool move;

    public UnityEvent onPlayerDeath;
    public UnityEvent onEnemyDeath;
	
	void Start()
	{
	}

    void OnEnable() {
        marioBody = GameObject.Find("Game/MarioEV").GetComponent<Rigidbody2D>();
        enemyBody = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();

        // get the starting position
		originalX = transform.position.x;
	
		move = true;
		// randomise initial direction
		moveRight = Random.Range(0, 2) == 0 ? -1 : 1;
		
		// compute initial velocity
		ComputeVelocity();

        enemyAnimator.SetInteger("moveRight", moveRight);
		enemyAnimator.SetBool("marioDead", false);
    }
	
	void ComputeVelocity()
	{
	    velocity = new Vector2((moveRight) * gameConstants.maxOffset / gameConstants.enemyPatroltime, 0);
	}
  
	void MoveEnemy()
	{
		enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
	}

	void Update()
	{
		if (move) {
			if (Mathf.Abs(enemyBody.position.x - originalX) < gameConstants.maxOffset)
			{   
				// move goomba
				MoveEnemy();
			}
			else
			{
				// change direction
				moveRight *= -1;
				enemyAnimator.SetInteger("moveRight", moveRight);
				ComputeVelocity();
				MoveEnemy();
			}
		}
	}

    void OnCollisionEnter2D(Collision2D other) {
		// check if it collides with Mario
		if (other.gameObject.tag == "Player"){
			// check if collides on top
			float yoffset = (other.transform.position.y - this.transform.position.y);
			if (yoffset > 0.5f) {
				marioBody.velocity = new Vector2(marioBody.velocity.x, 0.0f);
				if (gameObject.tag == "Goomba") {
					marioBody.AddForce(Vector2.up * gameConstants.bounce, ForceMode2D.Impulse);
				}
				else if (gameObject.tag == "Koopa") {
					marioBody.AddForce(Vector2.up * gameConstants.bounce * 1.5f, ForceMode2D.Impulse);
				}
				KillSelf();
			}
			else {
				// hurt player
		        onPlayerDeath.Invoke();
			}
		}
	}

    public void PlayerDeathResponse()
    {
        velocity = Vector3.zero;
        enemyAnimator.SetBool("marioDead", true);
    }

    void KillSelf() {
		StartCoroutine(flatten());
		Debug.Log("Kill sequence ends");
	}

    IEnumerator flatten() {
		Debug.Log("Flatten starts");
		int steps = 5;
		float stepper = 1.0f/(float) steps;

		for (int i = 0; i < steps; i ++){
			this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y - stepper, this.transform.localScale.z);

			// make sure enemy is still above ground
			this.transform.position = new Vector3(this.transform.position.x, gameConstants.groundSurface + GetComponent<SpriteRenderer>().bounds.extents.y, this.transform.position.z);
			yield return null;
		}
		Debug.Log("Flatten ends");
		this.gameObject.SetActive(false);
		Debug.Log("Enemy returned to pool");
        onEnemyDeath.Invoke();
		yield break;
	}
}