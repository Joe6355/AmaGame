using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
   [SerializeField] private int moveSpeed;//скорость персонажа
   [SerializeField] private Rigidbody2D rb;
   [SerializeField] private Camera cam;
   [SerializeField] private Animator anim;

    Vector2 movement;//веткор движения
    Vector2 mousePos;// позиция мышки

    [SerializeField] private bool cursorchik;

    [SerializeField] private int totalCoins;//общее кол-во монет
    [SerializeField] private Text coinValueText;

    private void Start()
    {
        totalCoins = PlayerPrefs.GetInt("Coins", 0);
        Debug.Log("Монеты игрока" + totalCoins);
        UpdateCoinText();//обновляем ui с кол-вом монет
    }

    private void Update()
    {
        MousPosition();
        CursorController();
    }
    private void FixedUpdate()
    {
        Movement();
        UpdateCoinText();
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
        rb.rotation = angle;//поворачиваем игрока в сторону курсора
    }

    void CursorController()
    {
        Cursor.visible = cursorchik;
    }

    public void AddCoin(int amount)
    {
        totalCoins += amount;//увеличиваем кол-во монеток
        PlayerPrefs.SetInt("Coins", totalCoins);//сохраняем в PlayerPrefs
        Debug.Log("Собрано монеток" + totalCoins);
        
    }
    public void ResetCoins()
    {
        totalCoins = 0; // Сбрасываем количество монет
        PlayerPrefs.SetInt("Coins", totalCoins); // Обновляем в PlayerPrefs
        UpdateCoinText(); // Обновляем UI текст
    }

    private void UpdateCoinText()
    {
        coinValueText.text = totalCoins.ToString();
    }

}
