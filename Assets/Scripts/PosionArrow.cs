using System.Linq;
using UnityEngine;

public class PoisonArrow : ArrowDef
{
    [SerializeField] protected int poisonDamagePerTick = 1; // ���� ��� �� ���
    [SerializeField] protected int poisonTicks = 3; // ���������� �����
    [SerializeField] protected float tickInterval = 1f; // �������� ����� ������

    protected override void OnCollisionEnter2D(Collision2D coll)
    {
        base.OnCollisionEnter2D(coll); // ���������� ������� ����������

        if (coll.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            // ���������, ���� ���� �� ������
            if (!enemyTegHoly.Contains(coll.gameObject.tag))
            {
                enemy.ApplyPoison(poisonDamagePerTick, poisonTicks, tickInterval); // �������� ��������� ��� �����
            }
            else
            {
                Debug.Log($"�������� ������ ������ � {coll.gameObject.name}, �� ��� ������ ����. ���� �� ���������.");
            }
        }

        Destroy(gameObject); // ���������� ������ ����� ���������
    }
}