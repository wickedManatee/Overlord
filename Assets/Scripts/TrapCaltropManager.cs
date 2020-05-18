 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapCaltropManager : MonoBehaviour {
    GameObject player;
    public AudioClip triggerSound;

    Enemies enemy;
    
    void Start () {
        enemy = Persistence.persistence.FindEnemy("Caltrop");
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	private void OnTriggerEnter(Collider other)
    {
        if (other == player.GetComponent<Collider>())
        {
            player.GetComponent<PlayerController>().TakeDamage(enemy.GetDamage());
            player.GetComponent<PlayerController>().SlowPlayerStart(enemy.GetAttackSpeed());
            GetComponent<AudioSource>().PlayOneShot(triggerSound);
            //Spawn blood
            Destroy(gameObject);
        }
    }
}
