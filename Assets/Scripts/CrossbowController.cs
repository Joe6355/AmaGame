using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossbowController : MonoBehaviour
{
    [SerializeField] private GameObject[] arrowPrefabs; // массив префабов стрел
    [SerializeField] private Transform firePoint; // точка выстрела
    [SerializeField] private int[] arrowCounts; // кол-во стрел каждого типа
    [SerializeField] private float fireForce = 20f; // сила стрельбы

    [SerializeField] private Text[] arrowCountsText; // визуальное отображение кол-во стрел

    private int selectedArrowIndex = 0; // индекс выбранного типа стрел

    private void Start()
    {
        LoadArrowCounts(); // Загрузка сохранённых данных при старте игры
        UpdateArrowCountsUI(); // обновляем UI после загрузки
    }

    private void UpdateArrowCountsUI()
    {
        // Проходим по всем типам стрел и обновляем текстовое отображение
        for (int i = 0; i < arrowCounts.Length; i++)
        {
            arrowCountsText[i].text = arrowCounts[i].ToString(); // Обновляем текст в UI
        }
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

            UpdateArrowCountsUI(); // обновляем UI после стрельбы
        }
        else
        {
            Debug.Log("Нет стрел этого типа");
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

            UpdateArrowCountsUI(); // обновляем UI после добавления стрел
        }
    }

    // Метод для сохранения количества стрел в PlayerPrefs
    public void SaveArrowCounts()
    {
        // Преобразуем массив в строку для сохранения
        string arrowCountsString = string.Join(",", arrowCounts);
        PlayerPrefs.SetString("ArrowCounts", arrowCountsString);
        PlayerPrefs.Save();
        Debug.Log("Количество стрел сохранено.");
    }

    // Метод для загрузки количества стрел из PlayerPrefs
    public void LoadArrowCounts()
    {
        if (PlayerPrefs.HasKey("ArrowCounts"))
        {
            string arrowCountsString = PlayerPrefs.GetString("ArrowCounts");
            string[] counts = arrowCountsString.Split(',');

            for (int i = 0; i < counts.Length; i++)
            {
                int.TryParse(counts[i], out arrowCounts[i]);
            }

            Debug.Log("Количество стрел загружено.");
        }
        else
        {
            Debug.Log("Сохранённых стрел не найдено, использую значения по умолчанию.");
        }
    }

    private void OnApplicationQuit()
    {
        SaveArrowCounts(); // Сохраняем данные при выходе из игры
    }
}
