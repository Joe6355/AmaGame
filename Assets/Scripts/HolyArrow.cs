using UnityEngine;

public class HolyArrow : ArrowDef
{
    public int healingAmount = 2; // ���������� ��������, ������� �����������������

    protected override void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            // �������� true ��� ������ ������
            enemy.TakeDamage(damage, true);
            Debug.Log($"������ ������ ������ � {coll.gameObject.name}.");
        }
        else
        {
            Debug.Log($"������ ������ ������ � {coll.gameObject.name}, �� ��� �� ����.");
        }

        Destroy(gameObject); // ���������� ������ ����� ���������
    }
}
