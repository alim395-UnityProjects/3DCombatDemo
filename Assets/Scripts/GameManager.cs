using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    AudioManager audioManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        };
        audioManager = GetComponent<AudioManager>();
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            NextScene();
        }
        if(Input.GetKeyDown(KeyCode.LeftBracket))
        {
            PreviousScene();
        }
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PreviousScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
