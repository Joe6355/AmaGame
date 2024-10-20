using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossbowController : MonoBehaviour
{
    [SerializeField] private GameObject[] arrowPrefabs; // ������ �������� �����
    [SerializeField] private Transform firePoint; // ����� ��������
    [SerializeField] private int[] arrowCounts; // ���-�� ����� ������� ����
    [SerializeField] private float fireForce = 20f; // ���� ��������

    [SerializeField] private Text[] arrowCountsText; // ���������� ����������� ���-�� �����

    private int selectedArrowIndex = 0; // ������ ���������� ���� �����

    private void Start()
    {
        LoadArrowCounts(); // �������� ���������� ������ ��� ������ ����
        UpdateArrowCountsUI(); // ��������� UI ����� ��������
    }

    private void UpdateArrowCountsUI()
    {
        // �������� �� ���� ����� ����� � ��������� ��������� �����������
        for (int i = 0; i < arrowCounts.Length; i++)
        {
            arrowCountsText[i].text = arrowCounts[i].ToString(); // ��������� ����� � UI
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

            Debug.Log($"��������� ������� ���� {selectedArrowIndex}. ��������: {arrowCounts[selectedArrowIndex]}");

            UpdateArrowCountsUI(); // ��������� UI ����� ��������
        }
        else
        {
            Debug.Log("��� ����� ����� ����");
        }
    }

    public void SwitchArrowType()
    {
        selectedArrowIndex++;
        if (selectedArrowIndex >= arrowPrefabs.Length)
        {
            selectedArrowIndex = 0; // ������������ � ������� ���� �����
        }

        Debug.Log($"���������� ��� ������ ��: {selectedArrowIndex}");
    }

    public void AddArrows(int typeIndex, int amount)
    {
        if (typeIndex >= 0 && typeIndex < arrowCounts.Length)
        {
            arrowCounts[typeIndex] += amount;
            Debug.Log($"��������� {amount} ����� ���� {typeIndex}. ������: {arrowCounts[typeIndex]}");

            UpdateArrowCountsUI(); // ��������� UI ����� ���������� �����
        }
    }

    // ����� ��� ���������� ���������� ����� � PlayerPrefs
    public void SaveArrowCounts()
    {
        // ����������� ������ � ������ ��� ����������
        string arrowCountsString = string.Join(",", arrowCounts);
        PlayerPrefs.SetString("ArrowCounts", arrowCountsString);
        PlayerPrefs.Save();
        Debug.Log("���������� ����� ���������.");
    }

    // ����� ��� �������� ���������� ����� �� PlayerPrefs
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

            Debug.Log("���������� ����� ���������.");
        }
        else
        {
            Debug.Log("���������� ����� �� �������, ��������� �������� �� ���������.");
        }
    }

    private void OnApplicationQuit()
    {
        SaveArrowCounts(); // ��������� ������ ��� ������ �� ����
    }
}
