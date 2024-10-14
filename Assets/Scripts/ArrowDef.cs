using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ArrowDef : MonoBehaviour
{
    public float lifeTime = 2f;
    public int damage = 1;

    // Списки тегов для врагов
    protected string[] enemyTeg = { "Slime", "Skeleton" };
    protected string[] enemyTegHoly = { "HolyEnemy" };

    // Публичные свойства для доступа
    public string[] EnemyTags => enemyTeg;
    public string[] HolyEnemyTags => enemyTegHoly;

    protected virtual void Update()
    {
        Destroy(gameObject, lifeTime); // Уничтожаем стрелу через указанное время
    }

    protected virtual void OnCollisionEnter2D(Collision2D coll)
    {
        if (enemyTeg.Contains(coll.gameObject.tag))
        {
            Enemy enemy = coll.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, false); // Обычная стрела (false)
                Debug.Log($"Обычная стрела попала в {coll.gameObject.name} и нанесла урон: {damage}");
            }
        }
        else
        {
            Debug.Log($"Обычная стрела попала в {coll.gameObject.name}, но это не враг.");
        }

        Destroy(gameObject); // Уничтожаем стрелу после попадания
    }
}
