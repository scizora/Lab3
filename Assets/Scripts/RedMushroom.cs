using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RedMushroom : MonoBehaviour
{
	public Texture t;
	public GameConstants gameConstants;

	IEnumerator minimize() {
		int steps = 5;
		float stepper = 1.0f/(float) steps;

		for (int i = 0; i < steps; i ++){
			gameObject.transform.parent.transform.localScale = new Vector3(gameObject.transform.parent.transform.localScale.x + stepper, gameObject.transform.parent.transform.localScale.y + stepper, gameObject.transform.parent.transform.localScale.z);
			yield return null;
		}

		for (int i = 0; i < steps*2; i ++){
			gameObject.transform.parent.transform.localScale = new Vector3(gameObject.transform.parent.transform.localScale.x - stepper, gameObject.transform.parent.transform.localScale.y - stepper, gameObject.transform.parent.transform.localScale.z);
			yield return null;
		}
		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.CompareTag("Player")){
			// update UI
			BoxCollider2D parentColl = gameObject.transform.parent.GetComponent<BoxCollider2D>();
            parentColl.sharedMaterial.friction = 1;
            parentColl.enabled = false;
            parentColl.enabled = true;
			StartCoroutine(minimize());
		}
	}
}