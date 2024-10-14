using UnityEngine;

public class HolyArrow : ArrowDef
{
    public int healingAmount = 2; // Количество здоровья, которое восстанавливается

    protected override void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            // Передаем true для святой стрелы
            enemy.TakeDamage(damage, true);
            Debug.Log($"Святая стрела попала в {coll.gameObject.name}.");
        }
        else
        {
            Debug.Log($"Святая стрела попала в {coll.gameObject.name}, но это не враг.");
        }

        Destroy(gameObject); // Уничтожаем стрелу после попадания
    }
}
