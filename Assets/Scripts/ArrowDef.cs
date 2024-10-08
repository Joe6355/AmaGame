using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ArrowDef : MonoBehaviour
{

    public float lifeTime = 2f;
    public int damage = 1;

    private string[] enemyTeg = {"Slime","Skeleton"};
    private void Start()
    {
        
    }
    private void Update()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        // Проверяем, является ли объект врагом по тегу
        if (enemyTeg.Contains(coll.gameObject.tag))
        {
            // Получаем компонент Enemy из объекта столкновения
            Enemy enemy = coll.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Наносим урон
                Debug.Log($"Стрела попала в {coll.gameObject.name} и нанесла урон: {damage}");
            }
        }
        else
        {
            Debug.Log($"Стрела попала в {coll.gameObject.name}, но это не враг.");
        }

        Destroy(gameObject); // Уничтожаем стрелу после попадания
    }
}

