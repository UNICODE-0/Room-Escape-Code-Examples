using UnityEngine;

public class CodeLockButton : Button
{
    [SerializeField] protected string _Name;
    public delegate void ButtonDown(string ButtonName, int ButtonID);
    public static event ButtonDown buttonDown;
    public override void Use()
    {
        base.Use();
        _Animator.SetTrigger("Press");
        buttonDown(_Name, gameObject.GetInstanceID());
    }
}
