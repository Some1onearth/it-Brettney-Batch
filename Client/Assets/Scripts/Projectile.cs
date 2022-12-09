using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move
        transform.Translate(Vector3.forward * 10f * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        GameManager.gameManager.AddScore();
    }
}
