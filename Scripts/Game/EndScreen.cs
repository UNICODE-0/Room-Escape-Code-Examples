using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private Image _Image;
    [SerializeField] private Text _Text;
    [SerializeField] private float _AttenuationRate = 0.1f;
    private void Start() 
    {
        _Image.color = new Color(_Image.color.r,_Image.color.g,_Image.color.b, 0f);
        _Text.color = new Color(_Text.color.r,_Text.color.g,_Text.color.b,0f);
    }
    private void FixedUpdate() 
    {
        _Image.color = new Color(_Image.color.r,_Image.color.g,_Image.color.b, _Image.color.a + _AttenuationRate);
        _Text.color = new Color(_Text.color.r,_Text.color.g,_Text.color.b, _Text.color.a + _AttenuationRate);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
