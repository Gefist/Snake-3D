using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Логика для кнопок

public class Buttons : MonoBehaviour
{
    [SerializeField] private GameObject play;               // Кнопка запуска игры.
    [SerializeField] private GameObject restart;            // Поле с объектом (кнопка перезагрузки игры).
    [SerializeField] private GameObject сheck;              // Текст выводящий счёт.
    [SerializeField] private GameObject logo;               // Изображение логотипа.
    [SerializeField] private GameObject settings;           // Кнопка настроек.
    [SerializeField] private GameObject sound;              // Кнопка звука.
    [SerializeField] private GameObject no_sound;           // Кнопка отсутствия звука.
    [SerializeField] private GameObject easy_level;         // Кнопка лёгкого уровня сложности.
    [SerializeField] private GameObject medium_level;       // Кнопка среднего уровня сложности.
    [SerializeField] private GameObject hard_level;         // Кнопка сложного уровня сложности.
    [SerializeField] private GameObject revival;            // Кнопка возраждения.
    [SerializeField] private Button revival_button;         // Кнопка возраждения.

    [SerializeField] private GameObject Background_music;   // Объект воспроизводящий звук.

    [SerializeField] private Hear hear;                     // Класс движущий змейку.

    private bool settings_open = false;                     // Поле определяющее открыты настройки или нет.

    public string difficulty_level = "Medium";              // Поле с информацией о нынешней сложности.
    
    public void Settings()
    {
        // Метод открывающий настройки.

        if (!settings_open)
        {
            if (Background_music.activeSelf)
            {
                sound.SetActive(true);
            }
            else
            {
                no_sound.SetActive(true);
            }

            switch (difficulty_level)
            {
                case "Easy":
                    easy_level.SetActive(true);
                    break;

                case "Medium":
                    medium_level.SetActive(true);
                    break;

                case "Hard":
                    hard_level.SetActive(true);
                    break;
            }

        }
        else
        {
            sound.SetActive(false);
            no_sound.SetActive(false);

            switch (difficulty_level)
            {
                case "Easy":
                    easy_level.SetActive(false);
                    break;

                case "Medium":
                    medium_level.SetActive(false);
                    break;

                case "Hard":
                    hard_level.SetActive(false);
                    break;
            }
        }
        settings_open = !(settings_open);

        
    }

    public void Sound()
    {
        // Метод для включающий музыку.

        if (Background_music.activeSelf)
        {
            sound.SetActive(false);
            no_sound.SetActive(true);
            Background_music.SetActive(false);
        }
        else if (!(Background_music.activeSelf))
        {
            sound.SetActive(true);
            no_sound.SetActive(false);
            Background_music.SetActive(true);
        }
    }

    public void Difficulty_level()
    {
        switch (difficulty_level)
        {
            case "Easy":
                difficulty_level = "Medium";
                easy_level.SetActive(false);
                medium_level.SetActive(true);

                break;

            case "Medium":
                difficulty_level = "Hard";
                medium_level.SetActive(false);
                hard_level.SetActive(true);
                break;

            case "Hard":
                difficulty_level = "Easy";
                hard_level.SetActive(false);
                easy_level.SetActive(true);
                break;
        }
    }

    public void Play()
    {
        // Метод для запуска игры.

        hear.enabled = true;
        
        play.SetActive(false);
        сheck.SetActive(true);
        logo.SetActive(false);

        settings_open = false;
        settings.SetActive(false);

        sound.SetActive(false);
        no_sound.SetActive(false);
        easy_level.SetActive(false);
        medium_level.SetActive(false);
        hard_level.SetActive(false);
        revival_button.interactable = true;
    }
   
    public void Restart_game()
    {
        // Метод для перезауска игры.

        hear.Restart_level();

        play.SetActive(true);
        restart.SetActive(false);
        logo.SetActive(true);
        revival.SetActive(false);
        revival_button.interactable = false;

        settings_open = false;
        settings.SetActive(false);

        sound.SetActive(false);
        no_sound.SetActive(false);
        easy_level.SetActive(false);
        medium_level.SetActive(false);
        hard_level.SetActive(false);
        revival_button.interactable = true;
    }

    public void Resume_level()
    {
        // Метод для возрождения.

        hear.Resume_level();
        
        play.SetActive(true);
        restart.SetActive(false);
        revival.SetActive(false);

        settings_open = false;
        settings.SetActive(false);

        logo.SetActive(false);

        sound.SetActive(false);
        no_sound.SetActive(false);
        easy_level.SetActive(false);
        medium_level.SetActive(false);
        hard_level.SetActive(false);
        revival_button.interactable = false;
    }
}
