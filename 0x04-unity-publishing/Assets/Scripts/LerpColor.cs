using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpColor : MonoBehaviour
{
    public Color[] colors;
    private MeshRenderer meshRenderer;
    // Start is called before the first frame update
    private int playerHealth;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = colors[0];
    }

    // Update is called once per frame
    void Update()
    {
        int playerHealth = GetComponent<PlayerController>().health;

        if (playerHealth > 0)
        {
            var newColor = Color.Lerp(
                    meshRenderer.material.color, colors[playerHealth - 1], 0.5f);
        
            meshRenderer.material.color = newColor;
            GetComponentInChildren<Light>().color = newColor;
        }
    }
}
