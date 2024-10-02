using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDef : MonoBehaviour
{

    public float lifeTime = 2f;

    private void Update()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log($"Стрела попала в {coll.gameObject.name}");
        Destroy(gameObject);
    }
}
