using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Vector2 jumpVelocity = new Vector2 (0, 20);

    Rigidbody2D rb;
    SpriteRenderer sr;
    bool notInit = true;
    Vector2 startPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start () {
        startPos = rb.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
                if ( (Input.touches.Length != 0 && Input.touches[0].phase == TouchPhase.Began) && !InGameManager.gameManager.paused)
                {
                    rb.velocity = jumpVelocity;
                    notInit = false;
                }
        #else
            if ((Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0)) && !InGameManager.gameManager.paused)
            {
                rb.velocity = jumpVelocity;
                notInit = false;
            }
        #endif
    }

    void FixedUpdate()
    {
        if (notInit)
        {
            rb.MovePosition(startPos);
            
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "4Colored")
        {
            SpriteRenderer other = collision.GetComponent<SpriteRenderer>();
            if(other.color != sr.color)
            {
                InGameManager.gameManager.EndGame();
            }

        }
         
    }
}
