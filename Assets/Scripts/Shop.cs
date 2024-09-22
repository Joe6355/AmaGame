using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject wellcomeText;
    [SerializeField] private GameObject interactivButton;
    [SerializeField] private CircleCollider2D circleCollider;

    [SerializeField] private GameObject panelShop;//панель магазина

    [SerializeField] private PlayerController player;

    private bool isPlayerInRange = false;

    [System.Serializable]
    public class ShopItem
    {
        public string itemName;
        public int itemPrice;
    }

    public ShopItem[] shopItems; // Массив товаров с ценами


    private void Start()
    {
        //откл визуальную часть при старте
        wellcomeText.SetActive(false);
        interactivButton.SetActive(false);
        panelShop.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKey(KeyCode.F))
        {
            OpenShop();
        }

        if (Input.GetKeyUp(KeyCode.F) && !isPlayerInRange)
        {
            panelShop.SetActive(false);
        }

    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))//если в обслати есть плеер включаем все
        {
            isPlayerInRange = true;
            wellcomeText.SetActive(true);
            interactivButton.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))//если в обслати есть плеер включаем все
        {
            isPlayerInRange = false;
            wellcomeText.SetActive(false);
            interactivButton.SetActive(false);
            panelShop.SetActive(false);
        }
    }

    public void OpenShop()
    {
        //Debug.Log(isPlayerInRange);
        if (isPlayerInRange)
        {
            panelShop.SetActive(true);
            Debug.Log("Магазин открыт");
        }
    }


    public void BuyItem(int itemIndex)
    {
        if (itemIndex >= 0 && itemIndex < shopItems.Length)
        {
            ShopItem item = shopItems[itemIndex];

            // Проверяем, хватает ли у игрока монет
            if (player.totalCoins >= item.itemPrice)
            {
                player.AddCoin(-item.itemPrice); // Вычитаем стоимость предмета
                Debug.Log($"Куплен {item.itemName} за {item.itemPrice} монет!");

                // Здесь ты можешь добавить логику выдачи предмета игроку
            }
            else
            {
                Debug.Log("Недостаточно монет для покупки " + item.itemName);
            }
        }
    }


    //отрисовка радуса покупки
    /*private void OnDrawGizmosSelected()
    {
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();

        // Если коллайдер существует, рисуем его
        if (circleCollider != null)
        {
            // Устанавливаем цвет Gizmo
            Gizmos.color = Color.red;

            // Рисуем окружность, обозначающую радиус коллайдера
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
        }
    }*/
}