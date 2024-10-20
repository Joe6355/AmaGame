using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int moveSpeed; // скорость персонажа
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Camera cam;
    [SerializeField] private Animator anim;

    Vector2 movement; // вектор движения
    Vector2 mousePos; // позиция мышки

    [SerializeField] private bool cursorchik;
    [SerializeField] public int totalCoins; // общее кол-во монет
    [SerializeField] private Text coinValueText;

    public CrossbowController crossbowController;

    // все для возвращения на точку
    [SerializeField] private Transform mirrorHome;
    private KeyCode keyToHold = KeyCode.E; // кнопка, которую нужно удерживать
    public float holdDuration = 3f; // время удержания кнопки
    private bool isHolding = false; // флаг состояния
    private float holdTimer = 0f; // таймер для отслеживания
    [SerializeField] private int mirrorRemainder = 1; // кол-во зарядов зеркала
    [SerializeField] private Text mirrorCountText;
    private int originalMoveSpeed; // для восстановления скорости после телепортации
    // Полоска прогресса телепортации
    [SerializeField] private Image progressBarImage; // Изображение прогресс-бара
    [SerializeField] private GameObject progressBarContainer; // Контейнер прогресс-бара

    // общая логика хп
    [SerializeField] private Image hpBar;
    public float hp = 100;
    public float maxHp = 100;

    private void Start()
    {
        LoadPlayerData(); // Загружаем сохранённые данные для HP и зеркал

        hpBar.fillAmount = hp / maxHp;
        mirrorCountText.text = mirrorRemainder.ToString();

        totalCoins = PlayerPrefs.GetInt("Coins", 0);
        Debug.Log("Монеты игрока" + totalCoins);
        UpdateCoinText(); // обновляем ui с кол-вом монет

        crossbowController = FindObjectOfType<CrossbowController>(); // Найдет объект с этим компонентом
        if (crossbowController == null)
        {
            Debug.LogError("CrossbowController не привязан!");
        }

        originalMoveSpeed = moveSpeed; // сохраняем исходную скорость

        progressBarContainer.SetActive(false);
    }

    private void HpBar()
    {
        hpBar.fillAmount = hp / maxHp;
    }

    private void MirrorHome()
    {
        // Если заряд зеркала закончился, ничего не делаем
        if (mirrorRemainder <= 0)
        {
            return;
        }

        // Проверяем удержание кнопки
        if (Input.GetKey(keyToHold))
        {
            moveSpeed = originalMoveSpeed / 2;

            if (!isHolding)
            {
                isHolding = true;
                holdTimer = 0f; // сбрасываем таймер
                progressBarContainer.SetActive(true);
            }

            holdTimer += Time.deltaTime;

            // Обновляем заполнение полоски прогресса
            float fillAmount = holdTimer / holdDuration;
            progressBarImage.fillAmount = fillAmount;

            // Если время удержания прошло — телепортируем
            if (holdTimer >= holdDuration)
            {
                TeleportPlayerHome();
                mirrorRemainder--; // уменьшаем заряд только после успешной телепортации
                mirrorCountText.text = mirrorRemainder.ToString();
                Debug.Log($"Заряд зеркала: {mirrorRemainder}");
                ResetMirrorState(); // сбрасываем состояние удержания
            }
        }
        else
        {
            ResetMirrorState();
        }
    }

    private void ResetMirrorState()
    {
        isHolding = false;
        holdTimer = 0f;
        moveSpeed = originalMoveSpeed; // возвращаем скорость

        progressBarImage.fillAmount = 0f;
        progressBarContainer.SetActive(false);
    }

    private void TeleportPlayerHome()
    {
        transform.position = mirrorHome.position;
        Debug.Log("Вы дома");
    }

    private void CrossBowController()
    {
        if (Input.GetMouseButtonDown(0))
        {
            crossbowController.Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            crossbowController.SwitchArrowType();
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("ArrowDef"))
        {
            crossbowController.AddArrows(0, 3);
        }
        if (coll.CompareTag("ArrowFire"))
        {
            crossbowController.AddArrows(0, 1);
        }
    }

    private void Update()
    {
        CursorController();
        CrossBowController();
        MirrorHome();
    }

    private void FixedUpdate()
    {
        Movement();
        UpdateCoinText();
        MousPosition();
        HpBar();
    }

    void MousPosition()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void Movement()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    void CursorController()
    {
        Cursor.visible = cursorchik;
    }

    public void AddCoin(int amount)
    {
        totalCoins += amount; // увеличиваем кол-во монеток
        PlayerPrefs.SetInt("Coins", totalCoins); // сохраняем в PlayerPrefs
        Debug.Log("Собрано монеток: " + totalCoins);
    }

    public void ResetCoins()
    {
        totalCoins = 0; // сбрасываем количество монет
        PlayerPrefs.SetInt("Coins", totalCoins); // обновляем в PlayerPrefs
        UpdateCoinText(); // обновляем UI текст
    }

    private void UpdateCoinText()
    {
        coinValueText.text = totalCoins.ToString();
    }

    // Сохранение HP и количества зеркал
    public void SavePlayerData()
    {
        PlayerPrefs.SetFloat("PlayerHP", hp); // сохраняем здоровье
        PlayerPrefs.SetInt("MirrorRemainder", mirrorRemainder); // сохраняем количество зеркал
        PlayerPrefs.Save();
        Debug.Log("Данные игрока сохранены.");
    }

    // Загрузка сохранённых данных
    public void LoadPlayerData()
    {
        if (PlayerPrefs.HasKey("PlayerHP"))
        {
            hp = PlayerPrefs.GetFloat("PlayerHP");
        }

        if (PlayerPrefs.HasKey("MirrorRemainder"))
        {
            mirrorRemainder = PlayerPrefs.GetInt("MirrorRemainder");
        }

        Debug.Log("Данные игрока загружены.");
    }

    private void OnApplicationQuit()
    {
        SavePlayerData(); // Сохранение данных при завершении игры
    }
}
