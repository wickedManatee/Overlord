using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpikeManager : MonoBehaviour {
    
    public AudioClip triggerSound;
    //public GameObject disabledPrefab;
    
    // Trigger is true, spike is false
    public bool triggerOrSpike;

    GameObject player;
    Enemies enemy;
    float health;
    float nextTimeToAttack = 0;

    // Use this for initialization
    void Start () {
        enemy = Persistence.persistence.FindEnemy("SpikeTrap");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerStay(Collider other)
    {
        if (triggerOrSpike)
        {
            if (other == player.GetComponent<Collider>() && Time.time >= nextTimeToAttack)
            {
                nextTimeToAttack = Time.time + 40f / enemy.GetAttackSpeed();
                transform.parent.gameObject.GetComponent<AudioSource>().PlayOneShot(triggerSound);
                transform.parent.gameObject.GetComponent<Animation>().Play();
            }
        }     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggerOrSpike)
        {
            if (other == player.GetComponent<Collider>())
            {
                player.GetComponent<PlayerController>().TakeDamage(enemy.GetDamage());
                //Spawn blood
            }
        }
    }

    // Update is called once per frame
    void Update () {
        
        if (!triggerOrSpike)
        {
            health = GetComponent<DestroyableObject>().health;
            if (health <= 0)
                Destroy(gameObject);
        }        
    }
}
