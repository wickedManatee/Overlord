using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour {
	public float health = 50f;
	public enum oType {enemy = 0, obj = 1, npc = 2}
	public oType objType;

    private void Start()
    {
        if (objType == oType.enemy)
            health = Persistence.persistence.FindEnemy(transform.name).GetHealth();
    }

    public virtual void TakeDamage(float amount)
	{
		health -= amount;
		if (health <= 0) {
			if (objType == oType.enemy)
				Die ();
			else
				Destroy ();
		}
	}

	protected virtual void Die(){
		Destroy (gameObject);
	}

	protected void Destroy(){
		Destroy (gameObject);
	}
}
