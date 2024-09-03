using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private new AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }


    public void ClickButtonPlay(int NumberButton)
    {
        audio.Play();
        SceneManager.LoadScene(NumberButton);
    }

    public void ClickButtonSettings()
    {
        audio.Play();

    }

    public void ClickButtonExit()
    {
        audio.Play();
        Application.Quit();
    }
}
