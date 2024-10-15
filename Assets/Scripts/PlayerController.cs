using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int moveSpeed; // �������� ���������
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Camera cam;
    [SerializeField] private Animator anim;

    Vector2 movement; // ������ ��������
    Vector2 mousePos; // ������� �����

    [SerializeField] private bool cursorchik;
    [SerializeField] public int totalCoins; // ����� ���-�� �����
    [SerializeField] private Text coinValueText;

    public CrossbowController crossbowController;

    public int Hp = 5;

    // ��� ��� ����������� �� �����
    [SerializeField] private Transform mirrorHome;
    private KeyCode keyToHold = KeyCode.E; // ������, ������� ����� ����������
    public float holdDuration = 3f; // ����� ��������� ������
    private bool isHolding = false; // ���� ���������
    private float holdTimer = 0f; // ������ ��� ������������
    [SerializeField] private int mirrorRemainder = 1; // ���-�� ������� �������
    private int originalMoveSpeed; // ��� �������������� �������� ����� ������������
    // ������� ��������� ������������
    [SerializeField] private Image progressBarImage; // ����������� ��������-����
    [SerializeField] private GameObject progressBarContainer; // ��������� ��������-����
    

    private void Start()
    {
        totalCoins = PlayerPrefs.GetInt("Coins", 0);
        Debug.Log("������ ������" + totalCoins);
        UpdateCoinText(); // ��������� ui � ���-��� �����

        crossbowController = FindObjectOfType<CrossbowController>(); // ������ ������ � ���� �����������
        if (crossbowController == null)
        {
            Debug.LogError("CrossbowController �� ��������!");
        }

        originalMoveSpeed = moveSpeed; // ��������� �������� ��������

        progressBarContainer.SetActive(false);
        
    }

    private void MirrorHome()
    {
        // ���� ����� ������� ����������, ������ �� ������
        if (mirrorRemainder <= 0)
        {
            //Debug.Log("������� ������� ������ ���.");
            return;
        }

        // ��������� ��������� ������
        if (Input.GetKey(keyToHold))
        {
            // �������� ��������� ��������
            moveSpeed = originalMoveSpeed / 2;

            if (!isHolding)
            {
                isHolding = true;
                holdTimer = 0f; // ���������� ������
                progressBarContainer.SetActive(true);
            }

            holdTimer += Time.deltaTime;

            // ��������� ���������� ������� ���������
            float fillAmount = holdTimer / holdDuration;
            progressBarImage.fillAmount = fillAmount;

            // ���� ����� ��������� ������ � �������������
            if (holdTimer >= holdDuration)
            {
                TeleportPlayerHome();
                mirrorRemainder--; // ��������� ����� ������ ����� �������� ������������
                Debug.Log($"����� �������: {mirrorRemainder}");
                ResetMirrorState(); // ���������� ��������� ���������
            }
        }
        else
        {
            // ���� ������ ���������, ���������� �������� � ���������� ���������
            ResetMirrorState();
        }
    }

    // ����� ��� ������ ��������� ��������� � �������������� ��������
    private void ResetMirrorState()
    {
        isHolding = false;
        holdTimer = 0f;
        moveSpeed = originalMoveSpeed; // ���������� ��������

        // ���������� �������� � ������ �������
        progressBarImage.fillAmount = 0f;
        progressBarContainer.SetActive(false);
    }

   
    private void TeleportPlayerHome() // ������� ������ �����
    {
        transform.position = mirrorHome.position;
        Debug.Log("�� ����");
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
        rb.rotation = angle; // ������������ ������ � ������� �������
    }

    void CursorController()
    {
        Cursor.visible = cursorchik;
    }

    public void AddCoin(int amount)
    {
        totalCoins += amount; // ����������� ���-�� �������
        PlayerPrefs.SetInt("Coins", totalCoins); // ��������� � PlayerPrefs
        Debug.Log("������� �������: " + totalCoins);
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
