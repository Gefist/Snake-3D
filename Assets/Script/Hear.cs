using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// Основной скрипт для движения змеи, детекта свайпа и вывода съеденых яблок.

public class Hear : MonoBehaviour
{
    // Поле для свайпа.
    private bool directionChosen;                                           // Поле определяющее закончен ли свайп.
    private byte turn;                                                      // Поле определяющее направление свайпа.
    private Vector2 start_position;                                         // Поле определяющее точку косания.      
    private Vector2 direction_turns;                                        // Поле определяющее разницу между точкой касания и конечной точкой свайпа.

    // Поля для опреденения заполнености игрового поля.
    [SerializeField] private GameObject movement_space;                     // Поле с объектом(камера) на котором лежит скрипт с массивом с данными позиции головы, хвоста и яблока. Это сделано для синхронизации данных позиции яблока, головы и хвоста.  
    public Vector3[] field = new Vector3[64]
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

    // Поле для определения позиции головы на игровом поле.
    private Vector3 position;                                               // Поле определяющее положение головы относительно глобальных координат.
    public Vector3 cell;                                                    // Поле определяющее положение ячейки относительно глобальных координат для сравнения с положением головы относительно глобальных координат для определения положения головы на игровом поле.
    public byte old_position = 0;                                           // Поле определяющее предыдущий номер ячейки в котором находилась голова для замены данных этой ячейки на новые.

    // Поля для телепортации яблока и изменения счёта и изменение длинны хвоста.
    [SerializeField] private GameObject apple;                              // Поле с объектом (яблоко).
    [SerializeField] private Text check_text;                               // Поле с текстом количества седеных яблок.
    public byte tail = 0;                                                   // Поле с данными о количестве съеденых яблок.

    // Поля для поворота и движения головы.
    [SerializeField] private GameObject settings;                           // Поле с объектом задающим уровень сложности.
    private float speed;                                                    // Поле задающее скорость движения и поворота головы.
    private string direction = "x";                                         // Поле определяющее нынешнее направление движения головы относительно глобальной системы координат.
    private static Vector3 direction_x = new Vector3(0, 180, 0);            // Поле определяющее направление в сторону x.
    private static Vector3 direction_negative_x = new Vector3(0, 0, 0);     // Поле определяющее направление в сторону -x.
    private static Vector3 direction_y = new Vector3(0, 0, 270);            // Поле определяющее направление в сторону y.
    private static Vector3 direction_negative_y = new Vector3(0, 0, 90);    // Поле определяющее направление в сторону -y.
    private static Vector3 direction_z = new Vector3(0, 90, 0);             // Поле определяющее направление в сторону z.
    private static Vector3 direction_negative_z = new Vector3(0, 270, 0);   // Поле определяющее направление в сторону -z.

    // Поля для включения выключения некотрых кнопок для перезакрузки игры.
    [SerializeField] private GameObject logo;                               // Поле с объектом (изображение логотипа).
    [SerializeField] private GameObject restart;                            // Поле с объектом (кнопка перезагрузки игры).
    [SerializeField] private Hear field_script;                             // Класс движущий голову.
    [SerializeField] private GameObject revival_button;                     // Кнопка возраждения.

    // Поля для работы хвоста.
    [SerializeField] private GameObject next_tail;                          // Поле с объектом со следующим хвостом.

    // Поля для возраждения змейки.
    public bool revival = false;
    [SerializeField] private byte old_tail;

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
                    case "Hear":
                        old_position = i;
                        break;

                    case "None":
                        movement_space.GetComponent<Turns>().busy[old_position] = "None";
                        movement_space.GetComponent<Turns>().busy[i] = "Hear";
                        old_position = i;
                        break;

                    case "Apple":
                        movement_space.GetComponent<Turns>().busy[old_position] = "None";
                        movement_space.GetComponent<Turns>().busy[i] = "Hear";
                        old_position = i;

                        tail++;
                        check_text.text = "Съедено яблок: " + tail;
                        
                        apple.GetComponent<Apple>().teleportation_apple = true;
                        break;

                    case "Tail":
                        Death();
                        break;
                }
                if (!revival)
                    Movement();
            }
            else if ((position[0] >= 4) || (position[0] <= -1) ||
                     (position[1] >= 4) || (position[1] <= -1) ||
                     (position[2] >= 4) || (position[2] <= -1))
            {
                Death();
            }
        }
    }

    public void Determining_swipe_direction()
    {
        // Метод для определения направления свайпа.

        if (directionChosen)
        {
            if (direction_turns[0] >= 50 || direction_turns[1] >= 50 || direction_turns[0] <= -50 || direction_turns[1] <= -50)
            {
                if (Mathf.Abs(direction_turns[0]) > Mathf.Abs(direction_turns[1]))
                {
                    if (direction_turns[0] < 0)
                    {
                        turn = 1;               // Влево.
                    }
                    else if (direction_turns[0] > 0)
                    {
                        turn = 3;               // Вправо.
                    }
                }
                else if (Mathf.Abs(direction_turns[0]) < Mathf.Abs(direction_turns[1]))
                {
                    if (direction_turns[1] < 0)
                    {
                        turn = 4;               // Вниз.
                    }
                    else if (direction_turns[1] > 0)
                    {
                        turn = 2;               // Вверх.
                    }
                }

                direction_turns[0] = 0;
                direction_turns[1] = 0;
            }
        }
    }

    public void Turns()
    {
        // Метод для поворота головы.

        if (turn == 1)
        {
            switch (direction)
            {
                case "x":
                    direction = "z";
                    transform.DORotate(direction_z, speed-0.1f, RotateMode.Fast);
                    break;

                case "-x":
                    direction = "-z";
                    transform.DORotate(direction_negative_z, speed - 0.1f, RotateMode.Fast);
                    break;

                case "y":
                    direction = "-z";
                    transform.DORotate(direction_negative_z, speed - 0.1f, RotateMode.Fast);
                    break;

                case "-y":
                    direction = "-z";
                    transform.DORotate(direction_negative_z, speed - 0.1f, RotateMode.Fast);
                    break;

                case "z":
                    direction = "-x";
                    transform.DORotate(direction_negative_x, speed - 0.1f, RotateMode.Fast);
                    break;

                case "-z":
                    direction = "x";
                    transform.DORotate(direction_x, speed - 0.1f, RotateMode.Fast);
                    break;
            }
        }
        else if (turn == 2)
        {
            switch (direction)
            {
                case "x":
                    direction = "y";
                    transform.DORotate(direction_y, speed - 0.1f, RotateMode.Fast);
                    break;

                case "-x":
                    direction = "y";
                    transform.DORotate(direction_y, speed - 0.1f, RotateMode.Fast);
                    break;

                case "y":
                    direction = "x";
                    transform.DORotate(direction_x, speed - 0.1f, RotateMode.Fast);
                    break;

                case "-y":
                    direction = "-x";
                    transform.DORotate(direction_negative_x, speed - 0.1f, RotateMode.Fast);
                    break;

                case "z":
                    direction = "y";
                    transform.DORotate(direction_y, speed - 0.1f, RotateMode.Fast);
                    break;

                case "-z":
                    direction = "y";
                    transform.DORotate(direction_y, speed - 0.1f, RotateMode.Fast);
                    break;
            }
        }
        else if (turn == 3)
        {
            switch (direction)
            {
                case "x":
                    direction = "-z";
                    transform.DORotate(direction_negative_z, speed - 0.1f, RotateMode.Fast);
                    break;

                case "-x":
                    direction = "z";
                    transform.DORotate(direction_z, speed - 0.1f, RotateMode.Fast);
                    break;

                case "y":
                    direction = "z";
                    transform.DORotate(direction_z, speed - 0.1f, RotateMode.Fast);
                    break;

                case "-y":
                    direction = "z";
                    transform.DORotate(direction_z, speed - 0.1f, RotateMode.Fast);
                    break;

                case "z":
                    direction = "x";
                    transform.DORotate(direction_x, speed - 0.1f, RotateMode.Fast);
                    break;

                case "-z":
                    direction = "-x";
                    transform.DORotate(direction_negative_x, speed - 0.1f, RotateMode.Fast);
                    break;
            }
        }
        else if (turn == 4)
        {
            switch (direction)
            {
                case "x":
                    direction = "-y";
                    transform.DORotate(direction_negative_y, speed - 0.1f, RotateMode.Fast);
                    break;

                case "-x":
                    direction = "-y";
                    transform.DORotate(direction_negative_y, speed - 0.1f, RotateMode.Fast);
                    break;

                case "y":
                    direction = "-x";
                    transform.DORotate(direction_negative_x, speed - 0.1f, RotateMode.Fast);
                    break;

                case "-y":
                    direction = "x";
                    transform.DORotate(direction_x, speed - 0.1f, RotateMode.Fast);
                    break;

                case "z":
                    direction = "-y";
                    transform.DORotate(direction_negative_y, speed - 0.1f, RotateMode.Fast);
                    break;

                case "-z":
                    direction = "-y";
                    transform.DORotate(direction_negative_y, speed - 0.1f, RotateMode.Fast);
                    break;
            }
        }

        turn = 0;
    }

    public void Definition_direction_movements()
    {
        // Метод для определения нынешнего направления движения головы.

        if (this.transform.rotation.eulerAngles.x == 0 &&
            this.transform.rotation.eulerAngles.y == 180 &&
            this.transform.rotation.eulerAngles.z == 0)
        {
            direction = "x";
        }
        else if (this.transform.rotation.eulerAngles.x == 0 &&
            this.transform.rotation.eulerAngles.y == 0 &&
            this.transform.rotation.eulerAngles.z == 0)
        {
            direction = "-x";
        }
        else if (this.transform.rotation.eulerAngles.x == 0 &&
            this.transform.rotation.eulerAngles.y == 90 &&
            this.transform.rotation.eulerAngles.z == 0)
        {
            direction = "z";
        }
        else if (this.transform.rotation.eulerAngles.x == 0 &&
            this.transform.rotation.eulerAngles.y == 270 &&
            this.transform.rotation.eulerAngles.z == 0)
        {
            direction = "-z";
        }
        else if (this.transform.rotation.eulerAngles.z == 270)
        {
            direction = "y";
        }
        else if (this.transform.rotation.eulerAngles.z == 90)
        {
            direction = "-y";
        }

    }

    public void Movement()
    {
        // Метод задающий движение голове.

        Definition_direction_movements();

        switch (direction)
        {
            case "x":
                transform.DOLocalMoveX(this.transform.position.x + 1, speed).SetEase(Ease.Linear);
                break;

            case "-x":
                transform.DOLocalMoveX(this.transform.position.x - 1, speed).SetEase(Ease.Linear);
                break;

            case "y":
                transform.DOLocalMoveY(this.transform.position.y + 1, speed).SetEase(Ease.Linear);
                break;

            case "-y":
                transform.DOLocalMoveY(this.transform.position.y - 1, speed).SetEase(Ease.Linear);
                break;

            case "z":
                transform.DOLocalMoveZ(this.transform.position.z + 1, speed).SetEase(Ease.Linear);
                break;

            case "-z":
                transform.DOLocalMoveZ(this.transform.position.z - 1, speed).SetEase(Ease.Linear);
                break;
        }

    }

    public void Swipe_detect()
    {
        // Метод для распознования свайпа.

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    start_position = touch.position;
                    directionChosen = false;
                    break;

                case TouchPhase.Moved:
                    direction_turns = touch.position - start_position;
                    break;

                case TouchPhase.Ended:
                    directionChosen = true;
                    break;
            }
        }
    }

    public void Compass()
    {
        //Метод для поворота головы в нужную сторону во время смерти.

        transform.DORotate(direction_x, 0, RotateMode.Fast);
    }

    public void Updating_cells()
    {
        // Метод для обнуления значений всех ячеек.

        for (byte i = 0; i < field.Length; i++)
        {
            movement_space.GetComponent<Turns>().busy[i] = "None";
        }
    }

    public void Tail_in_first()
    {
        // Метод для перемещение всех хвостов в начальную ячейку для возрождения.

        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("tail");
        foreach (GameObject obj in objectsWithTag)
        {
            obj.transform.position = field[0];
            movement_space.GetComponent<Turns>().busy[0] = "Tail";
        }
    }

    public void Resume_level()
    {
        // Метод для возрождения змейки.

        Compass();

        Tail_in_first();

        Updating_cells();

        Tail_in_first();

        movement_space.GetComponent<Turns>().busy[1] = "Hear";

        apple.GetComponent<Apple>().teleportation_apple = true;

        revival = false;
        this.enabled = false;
    }

    public void Restart_level()
    {
        // Метод для перезапуска игры.

        tail = 0;
        check_text.text = "Съедено яблок: " + tail;

        Compass();

        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("tail");
        foreach (GameObject obj in objectsWithTag)
        {
            obj.SetActive(false);
        }

        Updating_cells();

        this.transform.position = field[0];
        movement_space.GetComponent<Turns>().busy[0] = "Hear";

        apple.GetComponent<Apple>().teleportation_apple = true;
  
        revival = false;
        this.enabled = false;
    }

    public void Death()
    {
        // Метод для смерти.
        revival = true;

        Compass();
        Compass();

        Updating_cells();

        this.transform.position = field[1];
        movement_space.GetComponent<Turns>().busy[1] = "Hear";

        revival = true;

        Tail_in_first();

        Tail_in_first();

        logo.SetActive(true);
        restart.SetActive(true);
        revival_button.SetActive(true);
    }

    public void Visibility_tail()
    {
        // Метод для включения компонента рендеринга в хвосте если количество яблок совпадает с номером хвоста.

        if (1 == tail)
        {
            next_tail.SetActive(true);
        }
    }

    private void Update()
    {
        Changes_speed();

        Determining_swipe_direction();

        Swipe_detect();

        Turns();
        
        Definition_position();

        Visibility_tail();
    }
}