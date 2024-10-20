using System.Linq;
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int health = 5;// Здоровье врага
    
    public int damageTouch = 3;  // Урон, наносимый при столкновении

    public Transform player;
    private float distantion;
    public float agrDistantion = 10;

    private float speed;
    public float defSpeed = 5;

    private PlayerController playerController;

    public SpriteRenderer sprite;

   

    [SerializeField] private GameObject[] lootPrefabs;
    [SerializeField] private float dropChanceMin = 0.25f;
    [SerializeField] private float dropChanceMax = 0.50f;
    [SerializeField] private int lootDropCount = 3;

    private void Start()
    {
       
        sprite = GetComponent<SpriteRenderer>();
        playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("PlayerController не найден!");
        }
    }

    private void Update()
    {
        Distantion();
    }

    private void FixedUpdate()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Игрок не найден! Убедитесь, что у него установлен тег 'Player'.");
        }
    }

    public void Distantion()
    {
        distantion = Vector2.Distance(player.position, transform.position);
        if (distantion < agrDistantion)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            speed = defSpeed;
        }
        else
        {
            speed = 0;
        }
    }

    private void SetTransparence(float alpha)
    {
        sprite.color = new UnityEngine.Color(1f, 0f, 0f, alpha);
    }

    private void ResetTransparency()
    {
        SetTransparence(1f);
    }

    // Метод для применения ядовитого урона
    public void ApplyPoison(int poisonDamagePerTick, int poisonTicks, float tickInterval)
    {
        StartCoroutine(ApplyPoisonDamage(poisonDamagePerTick, poisonTicks, tickInterval));
    }

    // Корутин для нанесения ядовитого урона
    private IEnumerator ApplyPoisonDamage(int poisonDamagePerTick, int poisonTicks, float tickInterval)
    {
        for (int i = 0; i < poisonTicks; i++)
        {
            if (health > 0) // Проверяем, жив ли враг
            {
                TakeDamage(poisonDamagePerTick, false); // Наносим ядовитый урон
                Debug.Log($"Яд тик {i + 1}: Урон = {poisonDamagePerTick}");
                yield return new WaitForSeconds(tickInterval); // Задержка между уронами
            }
            else
            {
                break; // Прерываем цикл, если враг умер
            }
        }
    
}

    // Метод для нанесения урона или лечения
    public void TakeDamage(int amount, bool isHolyArrow)
    {
        // Получаем ссылку на стрелу
        ArrowDef arrowDef = FindObjectOfType<ArrowDef>();

        

        // Если стрела святая
        if (isHolyArrow)
        {
            // Если у врага тег из списка Holy, наносим урон
            if (arrowDef.HolyEnemyTags.Contains(gameObject.tag))
            {
                health -= amount;
                Debug.Log($"Враг получил урон от святой стрелы: {amount}. Текущее здоровье: {health}");
            }
            // Если враг не святой, восстанавливаем здоровье
            else if (arrowDef.EnemyTags.Contains(gameObject.tag))
            {
                Heal(amount);
                Debug.Log($"Враг был вылечен святой стрелой на {amount}. Текущее здоровье: {health}");
            }
        }
        else
        {
            // Если стрела не святая, наносим обычный урон
            health -= amount;
            Debug.Log($"Враг получил обычный урон: {amount}. Текущее здоровье: {health}");
        }

        SetTransparence(0.5f);

        if (health <= 0)
        {
            Die();
        }

        Invoke("ResetTransparency", 0.1f);
    }

    // Метод для лечения
    public void Heal(int amount)
    {
        health += amount;
        Debug.Log($"Враг был вылечен на {amount}. Текущие здоровье: {health}");

    }

    private void Die()
    {
        Debug.Log("Враг умер.");
        DropLoot();
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            playerController.hp -= damageTouch;
            Debug.Log("Игрок получил урон: " + damageTouch);
            Die();
        }
    }

    private void DropLoot()
    {
        for (int i = 0; i < lootDropCount; i++)
        {
            float dropChance = Random.Range(dropChanceMin, dropChanceMax);
            if (Random.value <= dropChance)
            {
                GameObject loot = lootPrefabs[Random.Range(0, lootPrefabs.Length)];
                Instantiate(loot, transform.position, Quaternion.identity);
                Debug.Log("Лут выпал: " + loot.name);
            }
            else
            {
                Debug.Log("Лут не выпал.");
            }
        }
    }
}
