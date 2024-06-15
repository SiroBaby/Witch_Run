using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float depth = 1;
    public float start = 1;
    public float end = 1;

    Player player;
    // Start is called before the first frame update

    void Awake(){
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float realVelocity = player.velocity.x / depth;
        Vector2 pos = transform.position;

        pos.x -= realVelocity * Time.fixedDeltaTime;
        
        if (pos.x <= end)
        {
            pos.x = start;
        }

        transform.position = pos;
    }
}
