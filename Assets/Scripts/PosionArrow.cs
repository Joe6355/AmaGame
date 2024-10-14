
using System.Collections.Generic;
using UnityEngine;

public class PosionArrow : ArrowDef
{
    [SerializeField] protected int poisonDamagePerTick = 1;
    [SerializeField] protected int poisonTicks = 3;
    [SerializeField] protected float tickInterval = 1f;

    protected override void OnCollisionEnter2D(Collision2D coll)
    {
        base.OnCollisionEnter2D(coll); // Используем базовый функционал

        if (coll.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.ApplyPoison(poisonDamagePerTick, poisonTicks, tickInterval); // Передаем параметры яда врагу
        }

        Destroy(gameObject); // Уничтожаем стрелу после попадания
    }
}
