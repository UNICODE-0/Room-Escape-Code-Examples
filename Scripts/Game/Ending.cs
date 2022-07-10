using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    [SerializeField] private GameObject _FakeDoor;
    [SerializeField] private GameObject _BlockWall;
    [SerializeField] private AudioSource _DoorSlap;

    private bool _IsTriggered = false;
    private void OnTriggerEnter(Collider other) 
    {
        if(other.TryGetComponent(out PlayerMovement playerMovement) && _IsTriggered == false)
        {
            _FakeDoor.SetActive(true);
            _BlockWall.SetActive(true);
            _DoorSlap.Play();
            PlayerManager.UI.FadeToBlack();
            _IsTriggered = true;
            StartCoroutine(ChangeScene());
        }
    }
    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(6.5f);
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(1);
    }
}
