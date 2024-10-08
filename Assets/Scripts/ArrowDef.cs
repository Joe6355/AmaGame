using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDef : MonoBehaviour
{

    public float lifeTime = 2f;
    public Enemy enemy;
    public int damage = 1;
    private void Start()
    {
        enemy = FindObjectOfType<Enemy>();
        if (enemy == null)
        {
            Debug.LogError("enemy не найден!");
        }
    }
    private void Update()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        enemy.health -= damage;
        Debug.Log($"Стрела попала в {coll.gameObject.name}");
        Destroy(gameObject);
    }
}
