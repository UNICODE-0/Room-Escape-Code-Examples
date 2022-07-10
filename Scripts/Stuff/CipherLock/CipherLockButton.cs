using UnityEngine;

public class CipherLockButton : Button
{
    [SerializeField] private CombinationSymbol _CombinationSymbol;
    private bool _IsDown = false;
    public bool isDown
    {
        get { return _IsDown; }
    }

    public delegate void ButtonDown(CombinationSymbol Symbol, int ButtonID);
    public static event ButtonDown buttonDown;
    
    public override void Use()
    {
        base.Use();
        if(_IsDown)
        {
            _IsDown = false;
            _Animator.SetTrigger("Up");
            _Collider.enabled = true;
        } else
        {
            _IsDown = true;
            _Animator.SetTrigger("Down");
            _Collider.enabled = false;
            buttonDown(_CombinationSymbol, gameObject.GetInstanceID());
        }

    }
}
