using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMaterial : MonoBehaviour
{

    Renderer r;
    Material oldMaterial;

    Material[] map;
    float timeToChange = 3f;
    float nextChange;
    int currentSlide;

    public bool play;
    
    void Awake()
    {
        r = GetComponent<Renderer>();
        oldMaterial = r.material;
        play = false;
        nextChange = 0f;
        currentSlide = 0;
    }

    private void Update()
    {
        if (play && map != null)
        {
            r.material = map[currentSlide];
            if (Time.time >= nextChange)
            {
                currentSlide++;
                if (currentSlide >= map.Length)
                    currentSlide = 0;

                nextChange = Time.time + timeToChange;
            }
        }
    }

    public void PlayMap(Material[] mat)
    {
        map = mat;
        currentSlide = 0;
        play = true;
        nextChange = Time.time + timeToChange;
    }

    public void StopMap()
    {
        play = false;
        map = null;
        r.material = oldMaterial;
    }
}
