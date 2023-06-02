using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// ������ ��� ������

public class Buttons : MonoBehaviour
{
    [SerializeField] private GameObject play;               // ������ ������� ����.
    [SerializeField] private GameObject restart;            // ���� � �������� (������ ������������ ����).
    [SerializeField] private GameObject �heck;              // ����� ��������� ����.
    [SerializeField] private GameObject logo;               // ����������� ��������.
    [SerializeField] private GameObject settings;           // ������ ��������.
    [SerializeField] private GameObject sound;              // ������ �����.
    [SerializeField] private GameObject no_sound;           // ������ ���������� �����.
    [SerializeField] private GameObject easy_level;         // ������ ������ ������ ���������.
    [SerializeField] private GameObject medium_level;       // ������ �������� ������ ���������.
    [SerializeField] private GameObject hard_level;         // ������ �������� ������ ���������.
    [SerializeField] private GameObject revival;            // ������ �����������.
    [SerializeField] private Button revival_button;         // ������ �����������.

    [SerializeField] private GameObject Background_music;   // ������ ��������������� ����.

    [SerializeField] private Hear hear;                     // ����� �������� ������.

    private bool settings_open = false;                     // ���� ������������ ������� ��������� ��� ���.

    public string difficulty_level = "Medium";              // ���� � ����������� � �������� ���������.
    
    public void Settings()
    {
        // ����� ����������� ���������.

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
        // ����� ��� ���������� ������.

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
        // ����� ��� ������� ����.

        hear.enabled = true;
        
        play.SetActive(false);
        �heck.SetActive(true);
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
        // ����� ��� ���������� ����.

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
        // ����� ��� �����������.

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
