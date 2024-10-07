using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 5;
    public int damageTouch = 3;

    public Transform player;
    public Transform enemy;
    public float distantion;

    public float speed;

    private PlayerController playerController;


    public void Distantion()
    {
        distantion =  Vector2.Distance(player.position, enemy.position);
        Debug.Log("Дистанция между игроком и врагом" + distantion);
        if(distantion < 5) 
        {
            speed = 2;
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else 
        {
            Debug.Log("Cтоп игрок далеко");
            speed = 0;
            //Destroy(gameObject, 5f);
        }
    }

   
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            playerController.Hp -= damageTouch;
            Destroy(gameObject);
        }
    }
    public void Update()
    {
        Distantion();

    }
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController не найден!");
        }
    }

}
