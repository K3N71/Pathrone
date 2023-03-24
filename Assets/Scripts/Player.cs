using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField, Description("Reference to the Globals Object.")]
    private Globals globals;
    [SerializeField, Description("How fast the player bounces.")]
    private float bounceRate = 0.7f;
    private float bounceTimer;
    private bool bounceDir = true;
    [SerializeField, Description("How high the player bounces.")]
    private float bounceHeight = 1.0f;

    [Header("Stats")]
    [SerializeField]
    private int strength = 5;
    [SerializeField]
    private int knowledge = 5;
    [SerializeField]
    private int intuition = 5;
    [SerializeField]
    private int luck = 5;
    [SerializeField]
    private int longevity = 5;

    // Start is called before the first frame update
    void Start()
    {
        bounceTimer = bounceRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (globals.CurrentState == GameState.Game)
        {
            // Bounce code
            bounceTimer -= Time.deltaTime;
            if (bounceTimer <= 0)
            {
                bounceTimer = bounceRate;
                Vector3 bounceTransform = this.transform.position;
                if (bounceDir) bounceTransform.y += bounceHeight / 10;
                else bounceTransform.y -= bounceHeight / 10;
                bounceDir = !bounceDir;
                this.transform.position = bounceTransform;
            }
        }
    }
}
