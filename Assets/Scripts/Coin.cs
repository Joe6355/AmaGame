using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin: MonoBehaviour
{
    [SerializeField] private int coinValue = 1; //сколько дает монетка при сборе
    //[SerializeField] private int lifeTime = 5;//время жизни монетки

    private void Start()
    {
        //Destroy(gameObject, lifeTime);    
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            PlayerController player = coll.GetComponent<PlayerController>();

            if (player != null)
            {
                player.AddCoin(coinValue);           
            }

            Destroy(gameObject);
        }
    }

}
