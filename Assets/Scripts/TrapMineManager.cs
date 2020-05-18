using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapMineManager : MonoBehaviour
{
    public GameObject explosionPrefab;
    public AnimationClip triggerAnim;
    public GameObject triggerGO;

    GameObject player;
    Enemies enemy;
    float health;
    float nextTimeToAttack = 0;
    bool isTriggered = false;

    private void Start()
    {
        enemy = Persistence.persistence.FindEnemy("MineTrap");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == player.GetComponent<Collider>() && triggerGO != null)
        {
            isTriggered = true;
            nextTimeToAttack = Time.time + 40f / enemy.GetAttackSpeed();
            GetComponent<Animation>().clip = triggerAnim;
            GetComponent<Animation>().Play();
        }
    }

    private void Update()
    {
        health = GetComponent<DestroyableObject>().health;

        if ((isTriggered && Time.time >= nextTimeToAttack) 
            || health <= 0)
        {
            Destroy(triggerGO);
            FuckThePlayerUp();
        }
    }

    void FuckThePlayerUp()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        float ratio = Mathf.Clamp01(1 - dist / 5f / 2f);
        int dmg = 0;
        if (dist > 5f)
            dmg = 0;
        else if (dist <= 1f)
            dmg = enemy.GetDamage();
        else
            dmg = (int)(enemy.GetDamage() * ratio);
        
        GameObject.Instantiate(explosionPrefab, transform);
        if (dmg > 0f)
            player.GetComponent<PlayerController>().TakeDamage(dmg);
        isTriggered = false;
    }
}
