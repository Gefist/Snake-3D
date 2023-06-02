using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tail : MonoBehaviour
{
    // Поля для опреденения заполнености игрового поля.
    [SerializeField] private GameObject movement_space;                     // Поле с объектом(камера) на котором лежит скрипт с массивом с данными позиции головы, хвоста и яблока. Это сделано для синхронизации данных позиции яблока, головы и хвоста.  
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
    };                           // Поле для расшифровки массива (с данными позиции головы, хвоста и яблока) для понимания какая ячейка чем заполнена.

    // Поле для определения позиции хвоста на игровом поле.
    private Vector3 position;                                               // Поле определяющее положение хвоста относительно глобальных координат.
    public Vector3 cell;                                                    // Поле определяющее положение ячейки относительно глобальных координат для сравнения с положением головы относительно глобальных координат для определения положения хвоста на игровом поле.
    public byte old_position;                                              // Поле определяющее предыдущий номер ячейки в котором находился хвост для замены данных этой ячейки на новые.

    // Поля для поворота и движения хвоста.
    [SerializeField] private GameObject settings;                           // Поле с объектом задающим уровень сложности.
    private float speed;                                                    // Поле задающее скорость движения и поворота хвоста.

    // Поля коректной для работы хвоста.
    [SerializeField] private GameObject previous_tail;                      // Поле с объектом(хвостом/головой) движущемся перед нынешним.
    [SerializeField] private GameObject next_tail;                          // Поле с объектом со следующим хвостом.
    [SerializeField] private Hear hear;                                     // Поле с обектом голова.
    [SerializeField] private byte tail_number;                              // Поле с номером хвоста.
    
    public bool resurrection = false;
    public Apple apple;

    public void Changes_speed()
    {
        // Метод для изменения скорости движения и поворота хвоста вследствии изменения уровня сложности.

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
        // Метод задающий движение хвосту.

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
        // Метод для визуального поворота хвоста. лучше не включать.

        transform.DORotate(previous_tail.transform.rotation.eulerAngles, speed, RotateMode.Fast);
    }

    public void Definition_position()
    {
        // Метод для определения позиции головы на игровом поле.

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
        // Метод для включения следующего хвоста если количество яблок совпадает с номером хвоста.
        
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
