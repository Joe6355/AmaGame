using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject wellcomeText;
    [SerializeField] private GameObject interactivButton;
    [SerializeField] private CircleCollider2D circleCollider;

    [SerializeField] private GameObject panelShop;//������ ��������

    [SerializeField] private PlayerController player;

    private bool isPlayerInRange = false;

    [System.Serializable]
    public class ShopItem
    {
        public string itemName;
        public int itemPrice;
    }

    public ShopItem[] shopItems; // ������ ������� � ������


    private void Start()
    {
        //���� ���������� ����� ��� ������
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
        if (coll.CompareTag("Player"))//���� � ������� ���� ����� �������� ���
        {
            isPlayerInRange = true;
            wellcomeText.SetActive(true);
            interactivButton.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))//���� � ������� ���� ����� �������� ���
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
            Debug.Log("������� ������");
        }
    }


    public void BuyItem(int itemIndex)
    {
        if (itemIndex >= 0 && itemIndex < shopItems.Length)
        {
            ShopItem item = shopItems[itemIndex];

            // ���������, ������� �� � ������ �����
            if (player.totalCoins >= item.itemPrice)
            {
                player.AddCoin(-item.itemPrice); // �������� ��������� ��������
                Debug.Log($"������ {item.itemName} �� {item.itemPrice} �����!");

                // ����� �� ������ �������� ������ ������ �������� ������
            }
            else
            {
                Debug.Log("������������ ����� ��� ������� " + item.itemName);
            }
        }
    }


    //��������� ������ �������
    /*private void OnDrawGizmosSelected()
    {
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();

        // ���� ��������� ����������, ������ ���
        if (circleCollider != null)
        {
            // ������������� ���� Gizmo
            Gizmos.color = Color.red;

            // ������ ����������, ������������ ������ ����������
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
        }
    }*/
}