using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
   [SerializeField] private int moveSpeed;//�������� ���������
   [SerializeField] private Rigidbody2D rb;
   [SerializeField] private Camera cam;
   [SerializeField] private Animator anim;

    Vector2 movement;//������ ��������
    Vector2 mousePos;// ������� �����



    [SerializeField] private bool cursorchik;

    [SerializeField] public int totalCoins;//����� ���-�� �����
    [SerializeField] private Text coinValueText;

    public CrossbowController crossbowController;

    public int Hp = 5;
   
    private void Start()
    {
        totalCoins = PlayerPrefs.GetInt("Coins", 0);
        Debug.Log("������ ������" + totalCoins);
        UpdateCoinText();//��������� ui � ���-��� �����

        crossbowController = FindObjectOfType<CrossbowController>(); // ������ ������ � ���� �����������
        if (crossbowController == null)
        {
            Debug.LogError("CrossbowController �� ��������!");
        }
    }
    private void CrossBowController()
    {
        if (Input.GetMouseButtonDown(0))
        {
            crossbowController.Shoot();
        }

        // ������������ �����
        if (Input.GetKeyDown(KeyCode.Q))
        {
            crossbowController.SwitchArrowType();
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("ArrowDef"))
        {
            crossbowController.AddArrows(0,3);
            
        }
        if (coll.CompareTag("ArrowFire"))
        {
            crossbowController.AddArrows(0, 1);
        }
    }

    private void Update()
    {
        MousPosition();
        CursorController();
        CrossBowController();
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
        rb.rotation = angle;//������������ ������ � ������� �������
    }

    void CursorController()
    {
        Cursor.visible = cursorchik;
    }

    public void AddCoin(int amount)
    {
        totalCoins += amount;//����������� ���-�� �������
        PlayerPrefs.SetInt("Coins", totalCoins);//��������� � PlayerPrefs
        Debug.Log("������� �������" + totalCoins);
        
    }
    public void ResetCoins()
    {
        totalCoins = 0; // ���������� ���������� �����
        PlayerPrefs.SetInt("Coins", totalCoins); // ��������� � PlayerPrefs
        UpdateCoinText(); // ��������� UI �����
    }

    private void UpdateCoinText()
    {
        coinValueText.text = totalCoins.ToString();
    }

}
