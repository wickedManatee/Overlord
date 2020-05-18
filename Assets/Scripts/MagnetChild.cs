using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetChild : MonoBehaviour {

	public MagnetParent startingMagnetParent;
    public GameObject currentMagnetParent;
    /* Is attached to belt*/
    public bool isAttachedToBeltOnStart;
	public float maxDistanceFromParent = 0f;
	public bool isMagnetized;
	public bool isRespawnable;


	private GameObject vrPlayer;

	// Use this for initialization
	void Start () {
		vrPlayer = GameObject.FindGameObjectWithTag ("VRPlayer");
		if (isAttachedToBeltOnStart)
			startingMagnetParent.AttachObjectToMagnet (this.gameObject);
	}

	void LateUpdate() 
	{
		if (this.transform.position.y < (vrPlayer.transform.position.y - 10)
			|| (maxDistanceFromParent > 0.25f && Vector3.Distance(this.transform.position, startingMagnetParent.transform.position) > maxDistanceFromParent))
		{
			if (isRespawnable) {
				startingMagnetParent.AttachObjectToMagnet(this.gameObject);
			} else {
				Destroy (this.gameObject);
			}
		}
	}

    public void DetachFromParent()
    {
        if (currentMagnetParent != null)
            currentMagnetParent.GetComponent<MagnetParent>().DetachObjectFromMagnet(transform.gameObject);
    }

    public void DetachEnemy()
    {
        DetachFromParent();
        transform.parent = null;
        transform.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
    }
}
