using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScaler : MonoBehaviour
{
    public float factor = 1.0f;

    void Start()
    {
        SkinnedMeshRenderer rend = GetComponent<SkinnedMeshRenderer>();
        Mesh mesh = rend.sharedMesh;
        if (mesh != null)
        {
            Bounds bounds = mesh.bounds;

            Vector3 size = Vector3.Scale(bounds.size, transform.localScale) * factor;

            if (size.y < .001)
                size.y = size.z;

            rend.material.mainTextureScale = size;
        }
        else
        {
            Debug.Log("Mesh not found");
        }
    }
}