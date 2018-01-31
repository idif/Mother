using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : SingletonBehaviour<PauseMenu> {
	
    private bool isPaused = false;
    public GameObject pausepanel;

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1;
        pausepanel.SetActive(false);
    }
    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
        pausepanel.SetActive(true);
    }
    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }

	public void CallPauseMenu(){
		if (!isPaused)
		{
			Pause();
		}
		else
		{
			Resume();
		}
	}
		
}