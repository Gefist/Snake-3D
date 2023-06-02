using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tail : MonoBehaviour
{
    // ���� ��� ����������� ������������ �������� ����.
    [SerializeField] private GameObject movement_space;                     // ���� � ��������(������) �� ������� ����� ������ � �������� � ������� ������� ������, ������ � ������. ��� ������� ��� ������������� ������ ������� ������, ������ � ������.  
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

    // ���� ��� ����������� ������� ������ �� ������� ����.
    private Vector3 position;                                               // ���� ������������ ��������� ������ ������������ ���������� ���������.
    public Vector3 cell;                                                    // ���� ������������ ��������� ������ ������������ ���������� ��������� ��� ��������� � ���������� ������ ������������ ���������� ��������� ��� ����������� ��������� ������ �� ������� ����.
    public byte old_position;                                              // ���� ������������ ���������� ����� ������ � ������� ��������� ����� ��� ������ ������ ���� ������ �� �����.

    // ���� ��� �������� � �������� ������.
    [SerializeField] private GameObject settings;                           // ���� � �������� �������� ������� ���������.
    private float speed;                                                    // ���� �������� �������� �������� � �������� ������.

    // ���� ��������� ��� ������ ������.
    [SerializeField] private GameObject previous_tail;                      // ���� � ��������(�������/�������) ���������� ����� ��������.
    [SerializeField] private GameObject next_tail;                          // ���� � �������� �� ��������� �������.
    [SerializeField] private Hear hear;                                     // ���� � ������� ������.
    [SerializeField] private byte tail_number;                              // ���� � ������� ������.
    
    public bool resurrection = false;
    public Apple apple;

    public void Changes_speed()
    {
        // ����� ��� ��������� �������� �������� � �������� ������ ���������� ��������� ������ ���������.

        switch (settings.GetComponent<Buttons>().difficulty_level)
        {
            case "Easy":
                speed = 3.0f;
                break;

            case "Medium":
                speed = 2.0f;
                break;

            case "Hard":
                speed = 1.0f;
                break;
        }
    }

    public void Movement()
    {
        // ����� �������� �������� ������.

        if (this.name == "1")
        {
            transform.DOLocalMove(field[previous_tail.GetComponent<Hear>().old_position], speed).SetEase(Ease.Linear);        
        }
        else
        {
            transform.DOLocalMove(field[previous_tail.GetComponent<Tail>().old_position], speed).SetEase(Ease.Linear);
        }
    }

    public void Turns()
    {
        // ����� ��� ����������� �������� ������. ����� �� ��������.

        transform.DORotate(previous_tail.transform.rotation.eulerAngles, speed, RotateMode.Fast);
    }

    public void Definition_position()
    {
        // ����� ��� ����������� ������� ������ �� ������� ����.

        for (byte i = 0; i < field.Length; i++)
        {
            position = this.transform.position;
            cell = field[i];

            if (position == cell)
            {
                
                switch (movement_space.GetComponent<Turns>().busy[i])
                {
                    case "Tail":
                        old_position = i;
                        
                        break;

                    case "None":
                        movement_space.GetComponent<Turns>().busy[old_position] = "None";
                        movement_space.GetComponent<Turns>().busy[i] = "Tail";
                        
                        old_position = i;
                        
                        break;
                }
                if (!hear.revival)
                    Movement();               
            }
        }
    }
    
    public void Visibility_tail()
    {
        // ����� ��� ��������� ���������� ������ ���� ���������� ����� ��������� � ������� ������.
        
        if (tail_number + 1 <= hear.tail)
        {
            next_tail.SetActive(true);
        }
        else
        {
            next_tail.SetActive(false); 
        }
    }

    void OnEnable()
    {
        if (this.name == "1")
        {
            this.transform.position = field[previous_tail.GetComponent<Hear>().old_position];
        }
        else
        {
            this.transform.position = field[previous_tail.GetComponent<Tail>().old_position];
        }
    }

    void Start()
    {
        tail_number = byte.Parse(this.name);
        
        if (this.name == "1")
            this.transform.position = field[previous_tail.GetComponent<Hear>().old_position];
        else
            this.transform.position = field[previous_tail.GetComponent<Tail>().old_position];

        Definition_position();
        movement_space.GetComponent<Turns>().busy[old_position] = "None";
    }

    void Update()
    {
        Definition_position();

        Changes_speed();

        Visibility_tail();
    }
}
