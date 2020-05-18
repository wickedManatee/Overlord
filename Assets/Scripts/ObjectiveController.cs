using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveController : MonoBehaviour {
	public int objNumber;
	public objectiveType objType;
	public string objText;
	public GameObject[] items;
    public Animator anim;
    public string[] anims;
	public float timer;
    public int objPoints;

	[HideInInspector]
	public enum objectiveType {item, location, timer, interaction}

	private GameManager gm;

	void Start() {
		gm = Persistence.persistence.gm;
	}

	void Update() {
		if (gm == null)
			gm = Persistence.persistence.gm;
	}

	void OnTriggerEnter(Collider other)
	{
		/*
		 * if item, add to inventory
		 * else if location, skip
		 * else if timer, start timer
		 * else if interaction, enable interable object
		 */

		if (other.name == gm.player.name && gm.currentObjective == objNumber) {
			if (objType == objectiveType.item) {
				foreach (GameObject item in items) {
					Destroy(item);
					//TODO add to player inventory
				}

				gm.FinishObjective ();
			}
			else if (objType == objectiveType.location) {

				foreach (GameObject item in items) {
					Destroy (item);
				}
				gm.FinishObjective ();
			}
			else if (objType == objectiveType.timer) {
				StartCoroutine (WaitForObjective ());
			}
			else if (objType == objectiveType.interaction) {
				StartCoroutine (WaitForInteraction ());
				//TODO only after player interacts
			}
		}
	}

	IEnumerator WaitForObjective() {
		yield return new WaitForSeconds (timer  );
		gm.FinishObjective ();
	}

	IEnumerator WaitForInteraction() {
		float maxLength = 0f;
		//Animation anim;
		foreach (string animation in anims) {
            anim.SetTrigger(animation);
            //anim = new Animation ();
            //anim.clip = animation;
            //if (animation.length > maxLength)
            //	maxLength = animation.length;
            //anim.Play ();
        }

		yield return new WaitForSeconds (maxLength);
		gm.FinishObjective ();
	}
}
