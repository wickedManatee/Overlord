using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTeleporters : MonoBehaviour
{
    public bool passFail;
    public GameObject player;
    public MapLevelSelector lvlSelector;

    ParticleSystem particles;
    MapOrb map;

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        particles.Stop();
    }

    void Update()
    {
        if (map == null && particles.isPlaying)
        {
            particles.Stop();
        }
        else if (map != null && particles.isStopped)
        {
            particles.Play();
        }
    }

    public void UpdateTeleporterStatus(bool state)
    {
        if (state)
            particles.Play();
        else
            particles.Stop();
    }

    public void SetMap(MapOrb chosen)
    {
        map = chosen;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == player.GetComponent<Collider>()
            && particles.isPlaying
            && passFail)
        {
            if (map.mapIndex == -1 )
                UnityEngine.SceneManagement.SceneManager.LoadScene(Random.Range(1, UnityEngine.SceneManagement.SceneManager.sceneCount-1));
            else
               UnityEngine.SceneManagement.SceneManager.LoadScene(map.mapIndex);
        }
        else if (other == player.GetComponent<Collider>()
          && particles.isPlaying
          && !passFail)
        {
            lvlSelector.DeleteOldOption();
        }
    }
}