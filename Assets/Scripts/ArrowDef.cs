using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ArrowDef : MonoBehaviour
{
    public float lifeTime = 2f;
    public int damage = 1;

    // ������ ����� ��� ������
    protected string[] enemyTeg = { "Slime", "Skeleton" };
    protected string[] enemyTegHoly = { "HolyEnemy" };

    // ��������� �������� ��� �������
    public string[] EnemyTags => enemyTeg;
    public string[] HolyEnemyTags => enemyTegHoly;

    protected virtual void Update()
    {
        Destroy(gameObject, lifeTime); // ���������� ������ ����� ��������� �����
    }

    protected virtual void OnCollisionEnter2D(Collision2D coll)
    {
        if (enemyTeg.Contains(coll.gameObject.tag))
        {
            Enemy enemy = coll.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, false); // ������� ������ (false)
                Debug.Log($"������� ������ ������ � {coll.gameObject.name} � ������� ����: {damage}");
            }
        }
        else
        {
            Debug.Log($"������� ������ ������ � {coll.gameObject.name}, �� ��� �� ����.");
        }

        Destroy(gameObject); // ���������� ������ ����� ���������
    }
}
