using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    [HideInInspector]
    public Animator animSrc;
    public AudioClip[] sounds;
    
    Enemies enemy;
    int health;

    PlayerController player;
    iDroidManager droidMgr;

    float nextTimeToAttack = 0;
    float nextTimeToMoveSound = 0;
    int lineOfSightDistance = 10;

    public bool justSpawned;

    float wanderRadius = 10f;
    float wanderTimer = 5f;
    NavMeshAgent agent;
    float timer;

    public void Awake()
    {
        droidMgr = GameObject.FindObjectOfType<iDroidManager>();
        enemy = Persistence.persistence.FindEnemy(this.name);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        animSrc = gameObject.GetComponentInChildren<Animator>();

        health = enemy.GetHealth();
    }

    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }

    private void FixedUpdate()
    {
        if (isGrabbed())
        {
            this.gameObject.GetComponent<MagnetChild>().DetachEnemy();
            justSpawned = false;
            return;
        }
        else if (justSpawned || health <= 0)
            return;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (HasLineOfSight())
        {
            transform.LookAt(player.transform);
            if (WithinAttackRange(distance) && Time.time >= nextTimeToAttack)
                Attack();
            else
                Move(player.transform.position);
        } else
        {
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                Move(newPos);
                timer = 0;
            } else
            {
                animSrc.SetBool("IsMoving", false);
            }
        }
    }

    public void TakeDamage(float amount)
	{
        Debug.Log(this.name + " health from " + health + " minus " + amount);
        health -= Mathf.FloorToInt(amount);	
        
		if (health <= 0) {
			Die ();
		}
        else
        {
            AudioSource.PlayClipAtPoint(FindSound("TakeDamage"), transform.position);
            animSrc.SetTrigger("TakeDamage");
        }
	}

    bool HasLineOfSight()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, lineOfSightDistance) && hit.transform.tag == "Player")
            return true;
        return false;
    }

    bool WithinAttackRange(float distance)
    {
        if (distance <= enemy.GetRange())
            return true;
        return false;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }

    void Move(Vector3 spot)
    {
        agent.SetDestination(spot);
        if (Time.time >= nextTimeToMoveSound)
        {
            AudioSource.PlayClipAtPoint(FindSound("Move"), transform.position);
            nextTimeToMoveSound = Time.time + FindSound("Move").length + 2f;
        }
        animSrc.SetBool("IsMoving", true);
    }

    void Die(){
        AudioSource.PlayClipAtPoint(FindSound("Death1"), transform.position);
        animSrc.SetTrigger("Death1");
        droidMgr.UpdateBankLabel(enemy.GetCost() / 2);
        Destroy(gameObject, GetAnimationLength("Death1"));
    }

    void Attack()
    {
        AudioSource.PlayClipAtPoint(FindSound("Attack1"), transform.position);
        animSrc.SetTrigger("Attack1");
        player.TakeDamage(enemy.GetDamage());
        nextTimeToAttack = Time.time + 40f / enemy.GetAttackSpeed();
    }

    bool isGrabbed()
    {
        if (Persistence.persistence.gm.hand1.currentAttachedObject == transform.gameObject ||
            Persistence.persistence.gm.hand2.currentAttachedObject == transform.gameObject)
        {
            //play pause animation
            //stop attacking
            //stop moving
            return true;
        }
        
        return false;
    }

    AudioClip FindSound(string clipName)
    {
        AudioClip clip = null;
        foreach (AudioClip audio in sounds)
        {
            if (audio.name == clipName)
                clip = audio;
        }
        return clip;
    }

    float GetAnimationLength(string animName)
    {
        float time = 0f;
        RuntimeAnimatorController ac = animSrc.runtimeAnimatorController;    //Get Animator controller
        for (int i = 0; i < ac.animationClips.Length; i++)                 //For all animations
        {
            if (ac.animationClips[i].name == animName)        //If it has the same name as your clip
            {
                time = ac.animationClips[i].length;
            }
        }
        return time;
    }
}
