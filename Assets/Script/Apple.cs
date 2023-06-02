using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    // ���� ��� ����������� ������������ �������� ����.
    [SerializeField] private GameObject movement_space;                     // ������ (�� ������ ������ ������) �� ������� ����� ������ � �������� � ������� ������� ������, ������ � ������. ��� ������� ��� ������������� ������ ������� ������, ������ � ������.  
    private Vector3[] field = new Vector3[64]
    {
        new Vector3(0,0,0),
        new Vector3(1,0,0),
        new Vector3(2,0,0),
        new Vector3(3,0,0),
        new Vector3(0,1,0),
        new Vector3(1,1,0),
        new Vector3(2,1,0),
        new Vector3(3,1,0),
        new Vector3(0,2,0),
        new Vector3(1,2,0),
        new Vector3(2,2,0),
        new Vector3(3,2,0),
        new Vector3(0,3,0),
        new Vector3(1,3,0),
        new Vector3(2,3,0),
        new Vector3(3,3,0),
        new Vector3(0,0,1),
        new Vector3(1,0,1),
        new Vector3(2,0,1),
        new Vector3(3,0,1),
        new Vector3(0,1,1),
        new Vector3(1,1,1),
        new Vector3(2,1,1),
        new Vector3(3,1,1),
        new Vector3(0,2,1),
        new Vector3(1,2,1),
        new Vector3(2,2,1),
        new Vector3(3,2,1),
        new Vector3(0,3,1),
        new Vector3(1,3,1),
        new Vector3(2,3,1),
        new Vector3(3,3,1),
        new Vector3(0,0,2),
        new Vector3(1,0,2),
        new Vector3(2,0,2),
        new Vector3(3,0,2),
        new Vector3(0,1,2),
        new Vector3(1,1,2),
        new Vector3(2,1,2),
        new Vector3(3,1,2),
        new Vector3(0,2,2),
        new Vector3(1,2,2),
        new Vector3(2,2,2),
        new Vector3(3,2,2),
        new Vector3(0,3,2),
        new Vector3(1,3,2),
        new Vector3(2,3,2),
        new Vector3(3,3,2),
        new Vector3(0,0,3),
        new Vector3(1,0,3),
        new Vector3(2,0,3),
        new Vector3(3,0,3),
        new Vector3(0,1,3),
        new Vector3(1,1,3),
        new Vector3(2,1,3),
        new Vector3(3,1,3),
        new Vector3(0,2,3),
        new Vector3(1,2,3),
        new Vector3(2,2,3),
        new Vector3(3,2,3),
        new Vector3(0,3,3),
        new Vector3(1,3,3),
        new Vector3(2,3,3),
        new Vector3(3,3,3)
    };                           // ���� ��� ����������� ������� (� ������� ������� ������, ������ � ������) ��� ��������� ����� ������ ��� ���������.

    // ���� ��� ������������ ������.
    public bool teleportation_apple = true;                                 // ���� ������ ���������� �� ������������ � ��������� ����� ������.
    private int time = 0;                                                   // ���� ������������ ��������� �� ��� ������ �������� ���� ��� ���.
    private int coordinate;                                                 // ���� ���������� �������� ������ ��� ������������.               

    // ���� ��� ���������� ������(��������� ����).
    [SerializeField] private GameObject apple;                              // ���� � �������� ������.

    public void Teleportation_apple()
    {
        // ����� ��� ������������ ������.

        coordinate = Random.Range(1, 64);

        if (teleportation_apple)
        {
            if (movement_space.GetComponent<Turns>().busy[coordinate] == "None")
            {
                transform.position = field[coordinate];
                movement_space.GetComponent<Turns>().busy[coordinate] = "Apple";

                teleportation_apple = false;
                time = 0;
            }
            else
            {
                time++;
                if (time == 64*64)
                    apple.SetActive(false);
            }
        }
    }

    void Update()
    {
        Teleportation_apple();
    }
}
