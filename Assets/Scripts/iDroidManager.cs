using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using System;

public class iDroidManager : MonoBehaviour {
    public GameObject[] panels;
    public Text txtBank;
    public bool justSwitched;
    public Enemies enemyToSpawn;
    public GameObject laser;

    public void UpdateBankLabel(int amount)
    {
        txtBank.text = (GetBank() + amount).ToString();
    }

    public int GetBank()
    {
        string temp = txtBank.text;
        int total = Convert.ToInt32(temp);
        return total;
    }

    public void Spawn(Vector3 position)
    {
        if (enemyToSpawn.GetCost() > GetBank())
            return;

        UpdateBankLabel(enemyToSpawn.GetCost() * -1);

        GameObject go = (GameObject)Instantiate(Resources.Load(enemyToSpawn.enemyName));
        go.transform.position = position;
    }
}
