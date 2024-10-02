using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowController : MonoBehaviour
{
    [SerializeField] private GameObject[] arrowPrefabs;//массив префабов стрел
    [SerializeField] private Transform firePoint;//точка выстрела
    [SerializeField] private int[] arrowCounts;//кол-во стрел каждого типа
    [SerializeField] private float fireForce = 20f;//сила стрельбы

    private int selectedArrowIndex = 0;//индекс выбранного типа стрел



    private void Update()
    {
    }

    public void Shoot()
    {
        if (arrowCounts[selectedArrowIndex] > 0)
        {
            GameObject arrow = Instantiate(arrowPrefabs[selectedArrowIndex], firePoint.position, firePoint.rotation);
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);

            arrowCounts[selectedArrowIndex]--;

            Debug.Log($"Выстрелил стрелой типа {selectedArrowIndex}. Осталось: {arrowCounts[selectedArrowIndex]}");

        }
        else
        {
            Debug.Log("Нет стерл этого типа");
        }
    }

    public void SwitchArrowType()
    {
        selectedArrowIndex++;
            if (selectedArrowIndex >= arrowPrefabs.Length)
            {
                selectedArrowIndex = 0; // Возвращаемся к первому типу стрел
            }

        Debug.Log($"Переключен тип стрелы на: {selectedArrowIndex}");
    }

    public void AddArrows(int typeIndex, int amount)
    {
        if (typeIndex >= 0 && typeIndex < arrowCounts.Length)
        {
            arrowCounts[typeIndex] += amount;
            Debug.Log($"Добавлено {amount} стрел типа {typeIndex}. Теперь: {arrowCounts[typeIndex]}");
        }
    }
}
