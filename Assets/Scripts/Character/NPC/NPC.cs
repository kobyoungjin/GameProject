using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    Renderer renderer;
    Shader shader;
    GameManager gameManager;

    void Start()
    {
        renderer = GetComponentInChildren<Renderer>();
        renderer.sharedMaterial.SetFloat("_OutLineWidth", 0.001f);
        gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();

        gameManager.SetText(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseExit()
    {
        renderer.sharedMaterial.SetFloat("_OutLineWidth", 0.001f);
    }

}