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

    public int Hp = 5;

    // все для возвращения на точку
    [SerializeField] private Transform mirrorHome;
    private KeyCode keyToHold = KeyCode.E; // кнопка, которую нужно удерживать
    public float holdDuration = 3f; // время удержания кнопки
    private bool isHolding = false; // флаг состояния
    private float holdTimer = 0f; // таймер для отслеживания
    [SerializeField] private int mirrorRemainder = 1; // кол-во зарядов зеркала
    private int originalMoveSpeed; // для восстановления скорости после телепортации
    // Полоска прогресса телепортации
    [SerializeField] private Image progressBarImage; // Изображение прогресс-бара
    [SerializeField] private GameObject progressBarContainer; // Контейнер прогресс-бара
    

    private void Start()
    {
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

    private void MirrorHome()
    {
        // Если заряд зеркала закончился, ничего не делаем
        if (mirrorRemainder <= 0)
        {
            //Debug.Log("Зарядов зеркала больше нет.");
            return;
        }

        // Проверяем удержание кнопки
        if (Input.GetKey(keyToHold))
        {
            // Начинаем уменьшать скорость
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
                Debug.Log($"Заряд зеркала: {mirrorRemainder}");
                ResetMirrorState(); // сбрасываем состояние удержания
            }
        }
        else
        {
            // Если кнопку отпустили, возвращаем скорость и сбрасываем состояние
            ResetMirrorState();
        }
    }

    // Метод для сброса состояния удержания и восстановления скорости
    private void ResetMirrorState()
    {
        isHolding = false;
        holdTimer = 0f;
        moveSpeed = originalMoveSpeed; // возвращаем скорость

        // Сбрасываем прогресс и прячем полоску
        progressBarImage.fillAmount = 0f;
        progressBarContainer.SetActive(false);
    }

   
    private void TeleportPlayerHome() // вернуть игрока домой
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

        // Переключение стрел
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
        rb.rotation = angle; // поворачиваем игрока в сторону курсора
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
}
