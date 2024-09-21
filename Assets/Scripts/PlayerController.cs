using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   [SerializeField] private int moveSpeed;
   [SerializeField] private Rigidbody2D rb;
   [SerializeField] private Camera cam;
   [SerializeField] private Animator anim;

    Vector2 movement;//веткор движения
    Vector2 mousePos;// позиция мышки

    [SerializeField] private bool cursorchik;

    private void Start()
    {
        
    }

    private void Update()
    {
        MousPosition();
        CursorController();
    }
    private void FixedUpdate()
    {
        Movement();
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
        if (cursorchik)
            Cursor.visible = true;
        if (!cursorchik)
            Cursor.visible = false;
    }

}
