using System.Linq;
using UnityEngine;

public class PoisonArrow : ArrowDef
{
    [SerializeField] protected int poisonDamagePerTick = 1; // Урон яда за тик
    [SerializeField] protected int poisonTicks = 3; // Количество тиков
    [SerializeField] protected float tickInterval = 1f; // Интервал между тиками

    protected override void OnCollisionEnter2D(Collision2D coll)
    {
        base.OnCollisionEnter2D(coll); // Используем базовый функционал

        if (coll.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            // Проверяем, если враг не святой
            if (!enemyTegHoly.Contains(coll.gameObject.tag))
            {
                enemy.ApplyPoison(poisonDamagePerTick, poisonTicks, tickInterval); // Передаем параметры яда врагу
            }
            else
            {
                Debug.Log($"Ядовитая стрела попала в {coll.gameObject.name}, но это святой враг. Урон не наносится.");
            }
        }

        Destroy(gameObject); // Уничтожаем стрелу после попадания
    }
}