using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerLogic : MonoBehaviour
{
    [SerializeField] private GameObject pausePannel;
    [SerializeField] private bool inGame = true;

    private void Update()
    {
        if(inGame) Pause();
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1f)
            {
                Time.timeScale = 0f;
                pausePannel.SetActive(true);
                PlayerMovement.Instance.canMove = false;
            }
            else if (Time.timeScale == 0f)
            {
                Time.timeScale = 1f;
                pausePannel.SetActive(false);
                PlayerMovement.Instance.canMove = true;
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pausePannel.SetActive(false);
        PlayerMovement.Instance.canMove = true;
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }
}
