using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapOrb : MonoBehaviour {
    public Transform attachPoint;
    public int mapIndex;
    public Material[] slideshow;

    public void Respawn()
    {
        transform.position = attachPoint.position;
        transform.parent = attachPoint.transform;
    }
}
