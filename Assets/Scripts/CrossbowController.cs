using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowController : MonoBehaviour
{
    [SerializeField] private GameObject[] arrowPrefabs;//������ �������� �����
    [SerializeField] private Transform firePoint;//����� ��������
    [SerializeField] private int[] arrowCounts;//���-�� ����� ������� ����
    [SerializeField] private float fireForce = 20f;//���� ��������

    private int selectedArrowIndex = 0;//������ ���������� ���� �����



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

            Debug.Log($"��������� ������� ���� {selectedArrowIndex}. ��������: {arrowCounts[selectedArrowIndex]}");

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
        }
    }
}
