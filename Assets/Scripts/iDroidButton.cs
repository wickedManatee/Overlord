using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class iDroidButton : MonoBehaviour {
    private bool isMonsterBtn;
    public bool wasWatchClicked;
    public bool wasEscClicked;
    public bool wasInfoClicked;
    public bool isEnemySelectButton;
    public bool isSpawnConfirmedButton;
    public bool isSpawnCanceledButton;

    GameObject[] panels;
    public GameObject panelToHide;
    public GameObject panelToShow;

    iDroidManager droidMgr;

    private void Start()
    {
        droidMgr = GameObject.FindObjectOfType<iDroidManager>();
        droidMgr.justSwitched = false;
        panels = droidMgr.panels;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (droidMgr == null)
        {
            Debug.Log("droidMgr is null");
            return;
        }
        Debug.Log("Touched by " + other.name);

        if (other.name.StartsWith("Pointer")
            && !droidMgr.justSwitched)
        {
            droidMgr.justSwitched = true;
            string enemyName = "";

            if (wasWatchClicked)
            {
                btnHomeClick();
                return;
            }

            if (wasEscClicked)
                btnEscClick();

            if (wasInfoClicked)
            {
                enemyName = this.gameObject.GetComponentInChildren<Text>().text;

                Enemies enemyFound = Persistence.persistence.FindEnemy(enemyName);
                if (enemyFound == null)
                    Debug.Log("Enemy not found");
                else
                {
                    if (enemyFound.enemyType.ToString() == Enemies.eType.monster.ToString())
                        isMonsterBtn = true;
                    else
                        isMonsterBtn = false;

                    droidMgr.enemyToSpawn = enemyFound;
                    btnEnemyInfo(droidMgr.enemyToSpawn);
                }
            }

            if (isEnemySelectButton)
                btnSelectEnemy();

            if (isSpawnConfirmedButton)
                btnChooseSpawnLocation();

            if (isSpawnCanceledButton)
                btnCancelSpawn();

            if (panelToHide != null)
                panelToHide.SetActive(false);
            if (panelToShow != null)
                panelToShow.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name.StartsWith("Pointer"))
        {
            StartCoroutine(WaitForX(2));
            droidMgr.justSwitched = false;
        }
    }

    void btnHomeClick()
    {
        foreach (GameObject go in panels)
        {
            if (go.name != "Panel0" && go.name != "PanelHome")
                go.SetActive(false);
            else
                go.SetActive(true);
        }
    }

    void btnEscClick()
    {
        foreach (GameObject go in panels)
            go.SetActive(false);
    }

    void btnEnemyInfo(Enemies chosenEnemy)
    {
        string panelName;
        if (isMonsterBtn)
            panelName = "PanelMInfo";
        else
            panelName = "PanelTInfo";

        GameObject panel = panels.Where(x => x.name == panelName).FirstOrDefault();
        panel.SetActive(true);

        Text[] data = panel.GetComponentsInChildren<Text>();
        foreach (Text field in data)
        {
            switch (field.name)
            {
                case ("name"):
                    field.text = chosenEnemy.enemyName;
                    break;
                case ("health"):
                    field.text = chosenEnemy.GetHealth().ToString();
                    break;
                case ("damage"):
                    field.text = chosenEnemy.GetDamage().ToString();
                    break;
                case ("speed"):
                    field.text = chosenEnemy.GetSpeed().ToString();
                    break;
                case ("range"):
                    field.text = chosenEnemy.GetRange().ToString();
                    break;
                case ("attack"):
                    field.text = chosenEnemy.attackType.ToString();
                    break;
                case ("cost"):
                    field.text = chosenEnemy.GetCost().ToString();
                    break;
            }
        }
    }

    ///<summary>green button on Enemy Info panel was clicked</summary>
    void btnSelectEnemy()
    {
        droidMgr.laser.GetComponent<SteamVR_LaserPointer>().laserToggle = true;
    }

    void btnChooseSpawnLocation()
    {
        if (droidMgr.laser.GetComponent<SteamVR_LaserPointer>().laserHit)
        {
            Vector3 hit = droidMgr.laser.GetComponent<SteamVR_LaserPointer>().laserPoint.point;
            droidMgr.laser.GetComponent<SteamVR_LaserPointer>().laserToggle = false;
            droidMgr.Spawn(hit);
        }
    }

    void btnCancelSpawn()
    {        
        droidMgr.enemyToSpawn = null;
        droidMgr.laser.GetComponent<SteamVR_LaserPointer>().laserToggle = false;
    }

    IEnumerator WaitForX(float sec)
    {
        yield return new WaitForSeconds(sec);
    }
}
