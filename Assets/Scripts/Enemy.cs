using System.Drawing;

using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 5;  // Здоровье врага
    public int damageTouch = 3;  // Урон, наносимый при столкновении

    public Transform player;
    public float distantion;
    private float speed;
    public float defSpeed = 5;

    private PlayerController playerController;


    
    public SpriteRenderer sprite;


    // Лут (префабы), которые могут выпасть
    [SerializeField] private GameObject[] lootPrefabs; // Массив префабов лута
    [SerializeField] private float dropChanceMin = 0.25f; // Минимальный шанс выпадения лута (25%)
    [SerializeField] private float dropChanceMax = 0.50f; // Максимальный шанс выпадения лута (50%)
    [SerializeField] private int lootDropCount = 3; // Количество выпадающих лутов

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

    // Функция для определения дистанции между игроком и врагом и его перемещения
    public void Distantion()
    {
        distantion = Vector2.Distance(player.position, transform.position);
        Debug.Log("Дистанция между игроком и врагом: " + distantion);

        if (distantion < 5)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            speed = defSpeed;
        }
        else
        {
            speed = 0;
            Debug.Log("Игрок далеко, враг остановился");
        }
    }

    // Когда враг умирает, проверяется вероятность выпадения лута
    private void Die()
    {
        DropLoot();  // Выпадает лут при смерти
        Destroy(gameObject);  // Уничтожаем врага
    }

    // Функция для столкновений с игроком
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            playerController.Hp -= damageTouch;
            Debug.Log("Игрок получил урон: " + damageTouch);
            Die(); // Вызов смерти врага
        }
    }

    private void SetTransparence(float alpha)
    {
        sprite.color = new UnityEngine.Color(1f, 0f, 0f, alpha); // Устанавливаем цвет с заданной прозрачностью
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        SetTransparence(0.5f);
        if (health <= 0)
        {
            Die();
        }
        Invoke("ResetTransparency", 0.1f); // Используем Invoke для сброса прозрачности через 0.2 секунды
    }
    private void ResetTransparency()
    {
        SetTransparence(1f); // Возвращаем цвет к непрозрачному
    }
    // Функция для выпадения лута
    private void DropLoot()
    {
        for (int i = 0; i < lootDropCount; i++)
        {
            float dropChance = Random.Range(dropChanceMin, dropChanceMax);

            if (Random.value <= dropChance)
            {
                // Выбираем случайный лут из массива префабов
                GameObject loot = lootPrefabs[Random.Range(0, lootPrefabs.Length)];

                // Создаем лут на позиции врага
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
