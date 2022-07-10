using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image _Crosshair1, _Crosshair2;

    [Header("Black screen settings")]
    [Space]

    [SerializeField] private float _DisappearanceRate = 11f;
    [SerializeField] private float _DisappearanceTreshold = 0.02f;
    [SerializeField] private float _AppearanceRate = 7f;
    [SerializeField] private float _AppearanceTreshold = 0.98f;
    [SerializeField] private Image _BlackScreen;
    [SerializeField] private float AttenuationRate = 0.01f;

    private readonly Color _CrosshairTransparentColor = new Color(1,1,1,0f),
    _CrosshairSolidColor = new Color(1,1,1,1f),
    _BlackScreenTransparentColor = new Color(0,0,0,1f);
    private void OnEnable() 
    {
        PlayerManager.UI = this;
    }
    private void OnDisable() 
    {
        PlayerManager.UI = null;
    }
    private void Awake() 
    {
        _Crosshair1.color = _CrosshairTransparentColor;
        _Crosshair2.color = _CrosshairTransparentColor;
        _BlackScreen.color = _BlackScreenTransparentColor;
        
        FadeToTransparent();
    }
    private void FixedUpdate() 
    {
        if(RuntimeInfo.playerCanInteract 
        || PlayerManager.dragObjects.onDrag
        || PlayerManager.interactionObjects.onIinteract
        || PlayerManager.slidingObjects.onSlide)
        {
            if(RuntimeInfo.interactionObjects.interactive != null )
            {
                _Crosshair2.color = _CrosshairTransparentColor;
                ShowImage(_Crosshair1, _AppearanceRate, _AppearanceTreshold);
            } else
            {
                _Crosshair1.color = _CrosshairTransparentColor;
                ShowImage(_Crosshair2, _AppearanceRate, _AppearanceTreshold);
            }
        } 
        else if(_Crosshair1.color.a > 0 || _Crosshair2.color.a > 0)
        {
            HideImage(_Crosshair1.color.a > 0 ? _Crosshair1 : _Crosshair2, _DisappearanceRate, _DisappearanceTreshold);
        } 
    }
    private void ShowImage(Image image,float AppearanceRate,float AppearanceTreshold) // call in update
    {
        if(image.color.a < AppearanceTreshold) image.color = Color.Lerp(image.color,_CrosshairSolidColor, AppearanceRate * Time.deltaTime);
        else image.color = _CrosshairSolidColor;
    }
    private void HideImage(Image image,float DisappearanceRate ,float DisappearanceTreshold) // call in update
    {
        if(image.color.a > DisappearanceTreshold) image.color = Color.Lerp(image.color,_CrosshairTransparentColor, DisappearanceRate * Time.deltaTime);
        else image.color = _CrosshairTransparentColor;
    }
    public void FadeToTransparent()
    {
        StartCoroutine(BlackScreenFadeToTransparent());
    }
    public void FadeToBlack()
    {
        StartCoroutine(BlackScreenFadeToBlack());
    }
    private IEnumerator BlackScreenFadeToTransparent()
    {
        while (_BlackScreen.color.a > 0)
        {
            yield return new WaitForFixedUpdate();
            _BlackScreen.color = new Color(0f, 0f, 0f, _BlackScreen.color.a - AttenuationRate * Time.deltaTime);
        }
    }
    private IEnumerator BlackScreenFadeToBlack()
    {
        while (_BlackScreen.color.a < 1)
        {
            yield return new WaitForFixedUpdate();
            _BlackScreen.color = new Color(0f, 0f, 0f, _BlackScreen.color.a + AttenuationRate * Time.deltaTime);
        }
    }
}
