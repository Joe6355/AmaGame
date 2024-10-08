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
        // ���������, �������� �� ������ ������ �� ����
        if (enemyTeg.Contains(coll.gameObject.tag))
        {
            // �������� ��������� Enemy �� ������� ������������
            Enemy enemy = coll.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // ������� ����
                Debug.Log($"������ ������ � {coll.gameObject.name} � ������� ����: {damage}");
            }
        }
        else
        {
            Debug.Log($"������ ������ � {coll.gameObject.name}, �� ��� �� ����.");
        }

        Destroy(gameObject); // ���������� ������ ����� ���������
    }
}

