using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

    public float distanceToMove = 5f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            InGameManager.gameManager.GenerateRandomCircle();
            transform.position = new Vector3(transform.position.x, transform.position.y + distanceToMove, transform.position.z);
            Debug.Log("NICE! KEEP GOING!");

            InGameManager.gameManager.IncrementScore();
        }
    }

}
