using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    public int minAngularSpeed = 50;
    public int maxAngularSpeed = 200;

   
    public int speed;

    public void Start()
    {
        int sign = 0;
        do
        {
            sign = Random.Range(-1, 2);
        } while (sign == 0);

        sign /= Mathf.Abs(sign);

        int randomNumber = Random.Range(Mathf.Abs(minAngularSpeed), Mathf.Abs(maxAngularSpeed));
       
        speed = sign * randomNumber;
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1), speed * Time.deltaTime);
    }
}
