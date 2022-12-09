using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        ShootBullet();
    }
    public void ShootBullet()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        }
    }
}
