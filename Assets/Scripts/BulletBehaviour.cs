using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    float bulletSpeed = 10f;

    void bulletMovement()
    {
        if (this.gameObject.transform.position.y > 6)
        {
            Destroy(this.gameObject);
        }
        else
        {
            this.gameObject.transform.Translate(Vector2.up * bulletSpeed * Time.deltaTime);
        }
    }

    void Update()
    {
        bulletMovement();
    }
}
