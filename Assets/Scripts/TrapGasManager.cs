using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapGasManager : MonoBehaviour
{
    public GameObject gasPrefab;

    GameObject player;
    Enemies enemy;
    float health;
    float nextTimeToAttack = 0;
    bool isTriggered = false;

    void Start()
    {
        enemy = Persistence.persistence.FindEnemy("GasTrap");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        health = GetComponent<DestroyableObject>().health;
        if (health <= 0)
            Destroy(gameObject);
        //play explosion and sound

        if (isTriggered)
        {
            GameObject go = GameObject.Instantiate(gasPrefab, transform);
            Destroy(go, 10f);
            isTriggered = false;
            if (Vector3.Distance(player.transform.position, transform.position) <= 3.5f)
                StartCoroutine(player.GetComponent<PlayerController>().TakePoisonDamage(enemy.GetDamage()));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == player.GetComponent<Collider>() && Time.time >= nextTimeToAttack && !isTriggered)
        {
            isTriggered = true;
            nextTimeToAttack = Time.time + 40f / enemy.GetAttackSpeed();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other == player.GetComponent<Collider>())
        {
            if (Time.time >= nextTimeToAttack && !isTriggered)
            {
                isTriggered = true;
                nextTimeToAttack = Time.time + 40f / enemy.GetAttackSpeed();
            }
        }
    }
}
