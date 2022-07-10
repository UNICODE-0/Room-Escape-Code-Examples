using UnityEngine.SceneManagement;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject _PausePanel;
    private KeyCode _PauseKey;
    private void Start() 
    {
        _PauseKey = (KeyCode)Controls.PlayerControls["Pause"];
    }
    private void Update() 
    {
        if(Input.GetKeyDown(_PauseKey))
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            _PausePanel.SetActive(true);
        }
    }
    public void UnPause()
    {
        _PausePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }
    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
