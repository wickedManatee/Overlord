using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLevelSelector : MonoBehaviour {
    [HideInInspector]
    public MapOrb proposedMap;
    public MapTeleporters acceptTP;
    public MapTeleporters rejectTP;
    public GameObject movieScreen;
    public GameObject explosionPrefab;

    ParticleSystem particles;

    void Start () {
        particles = GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.parent.name.StartsWith("Ball"))
            return;

        if (proposedMap != null)
            DeleteOldOption();

        proposedMap = other.transform.parent.GetComponent<MapOrb>();
        if (proposedMap != null)
        {
            acceptTP.SetMap(proposedMap);
            rejectTP.SetMap(proposedMap);
            ShowPreview(proposedMap.slideshow);
            particles.Stop();
        }
    }

    private void OnTriggerExit (Collider other)
    {
        if (!other.transform.parent.name.StartsWith("Ball"))
            return;

        if (proposedMap == other.transform.parent.GetComponent<MapOrb>())
            DeleteOldOption();
    }

    private void ShowPreview(Material[] map)
    {
        movieScreen.GetComponent<PlayMaterial>().PlayMap(map);
    }

    private void StopPreview()
    {
        movieScreen.GetComponent<PlayMaterial>().StopMap();
    }

    public void DeleteOldOption()
    {
        acceptTP.SetMap(null);
        rejectTP.SetMap(null);
        StopPreview();
        particles.Play();
        GameObject.Instantiate(explosionPrefab,  transform);
        if (proposedMap != null)
            proposedMap.Respawn();
        proposedMap = null;
    }
}
