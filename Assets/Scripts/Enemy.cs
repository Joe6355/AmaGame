using System.Linq;
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int health = 5;// �������� �����
    
    public int damageTouch = 3;  // ����, ��������� ��� ������������

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
            Debug.LogError("PlayerController �� ������!");
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
            Debug.LogError("����� �� ������! ���������, ��� � ���� ���������� ��� 'Player'.");
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

    // ����� ��� ���������� ��������� �����
    public void ApplyPoison(int poisonDamagePerTick, int poisonTicks, float tickInterval)
    {
        StartCoroutine(ApplyPoisonDamage(poisonDamagePerTick, poisonTicks, tickInterval));
    }

    // ������� ��� ��������� ��������� �����
    private IEnumerator ApplyPoisonDamage(int poisonDamagePerTick, int poisonTicks, float tickInterval)
    {
        for (int i = 0; i < poisonTicks; i++)
        {
            if (health > 0) // ���������, ��� �� ����
            {
                TakeDamage(poisonDamagePerTick, false); // ������� �������� ����
                Debug.Log($"�� ��� {i + 1}: ���� = {poisonDamagePerTick}");
                yield return new WaitForSeconds(tickInterval); // �������� ����� �������
            }
            else
            {
                break; // ��������� ����, ���� ���� ����
            }
        }
    
}

    // ����� ��� ��������� ����� ��� �������
    public void TakeDamage(int amount, bool isHolyArrow)
    {
        // �������� ������ �� ������
        ArrowDef arrowDef = FindObjectOfType<ArrowDef>();

        

        // ���� ������ ������
        if (isHolyArrow)
        {
            // ���� � ����� ��� �� ������ Holy, ������� ����
            if (arrowDef.HolyEnemyTags.Contains(gameObject.tag))
            {
                health -= amount;
                Debug.Log($"���� ������� ���� �� ������ ������: {amount}. ������� ��������: {health}");
            }
            // ���� ���� �� ������, ��������������� ��������
            else if (arrowDef.EnemyTags.Contains(gameObject.tag))
            {
                Heal(amount);
                Debug.Log($"���� ��� ������� ������ ������� �� {amount}. ������� ��������: {health}");
            }
        }
        else
        {
            // ���� ������ �� ������, ������� ������� ����
            health -= amount;
            Debug.Log($"���� ������� ������� ����: {amount}. ������� ��������: {health}");
        }

        SetTransparence(0.5f);

        if (health <= 0)
        {
            Die();
        }

        Invoke("ResetTransparency", 0.1f);
    }

    // ����� ��� �������
    public void Heal(int amount)
    {
        health += amount;
        Debug.Log($"���� ��� ������� �� {amount}. ������� ��������: {health}");

    }

    private void Die()
    {
        Debug.Log("���� ����.");
        DropLoot();
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            playerController.hp -= damageTouch;
            Debug.Log("����� ������� ����: " + damageTouch);
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
                Debug.Log("��� �����: " + loot.name);
            }
            else
            {
                Debug.Log("��� �� �����.");
            }
        }
    }
}
