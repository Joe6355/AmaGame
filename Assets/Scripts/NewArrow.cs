using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewArrow : MonoBehaviour
{

    public float lifeTime = 2f;

    private void Update()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
