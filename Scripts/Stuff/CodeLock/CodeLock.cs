using UnityEngine;
using TMPro;

[System.Serializable]
class LockCode
{
    [Range(0,9)]
    [SerializeField] private int _FirstNum = 0;
    public int firstNum
    {
        get { return _FirstNum; }
        set { _FirstNum = value; }
    }
    [Range(0,9)]
    [SerializeField] private int _SecondNum = 0;
    public int secondNum
    {
        get { return _SecondNum; }
        set { _SecondNum = value; }
    }
    [Range(0,9)]
    [SerializeField] private int _ThirdNum = 0;
    public int thirdNum
    {
        get { return _ThirdNum; }
        set { _ThirdNum = value; }
    }
    [Range(0,9)]
    [SerializeField] private int _FourthNum = 0;
    public int fourthNum
    {
        get { return _FourthNum; }
        set { _FourthNum = value; }
    }
    
}
[RequireComponent(typeof(AudioSource))]
public class CodeLock : MonoBehaviour
{
    [SerializeField] private TextMeshPro[] _NumbersText;
    [SerializeField] private Collider[] _ButtonsCol;
    [SerializeField] private LockPart[] _LockParts;
    [Space]
    [SerializeField] private LockCode _Code;

    private bool _IsOpen;
    public bool isOpen
    {
        get { return _IsOpen; }
    }

    private const int NUMBER_COUNT = 4, MAX_NUMBER = 10;
    private int[] _Numbers = new int[NUMBER_COUNT];
    private AudioSource _AudioSource;
    
    public delegate void Unclocked(int LockID);
    public static event Unclocked unclocked;

    private void OnEnable() => CodeLockButton.buttonDown += ChangeNumber;
    private void OnDisable() => CodeLockButton.buttonDown -= ChangeNumber;
    private void Start() 
    {
        _AudioSource = GetComponent<AudioSource>();
        _IsOpen = false;
        ClearInput();
    }
    private void ClearInput()
    {
        for (int i = 0; i < NUMBER_COUNT; i++)
        {
            _Numbers[i] = 0;
            _NumbersText[i].text =  _Numbers[i].ToString();
        }
    }

    private void IncreaseNumber(int Index)
    {
        _Numbers[Index] = (_Numbers[Index] + 1) % MAX_NUMBER;
        _NumbersText[Index].text = _Numbers[Index].ToString();
    }
    private void DecreaseNumber(int Index)
    {
        _Numbers[Index] -= 1;
        if(_Numbers[Index] < 0) _Numbers[Index] += MAX_NUMBER;
        _NumbersText[Index].text = _Numbers[Index].ToString();
    }
    private bool CheckCode(LockCode code)
    {
        if( _Numbers[0] == code.firstNum   &&
            _Numbers[1] == code.secondNum  &&
            _Numbers[2] == code.thirdNum   &&
            _Numbers[3] == code.fourthNum  ) return true;
        return false;
    }

    private void ChangeNumber(string ButtonName, int ButtonId)
    {
        bool ButtonExist = false;
        foreach (var ButtonCol in _ButtonsCol)
        {
            if(ButtonCol.gameObject.GetInstanceID() == ButtonId) ButtonExist = true; 
        }
        if(ButtonExist == false) return;

        switch (ButtonName)
        {
            case "Button_up1":
            IncreaseNumber(0);
                break;
            case "Button_up2":
            IncreaseNumber(1);
                break;
            case "Button_up3":
            IncreaseNumber(2);
                break;
            case "Button_up4":
            IncreaseNumber(3);
                break;
            case "Button_down1":
            DecreaseNumber(0);
                break;
            case "Button_down2":
            DecreaseNumber(1);
                break;
            case "Button_down3":
            DecreaseNumber(2);
                break;
            case "Button_down4":
            DecreaseNumber(3);      
                break;
            default:
            Debug.LogWarning($"{ButtonName} button doesn't exist");
                break;
        }
        if(CheckCode(_Code))
        {
            Open();
        }
    }
    private void Open()
    {
        _AudioSource.Play();
        _IsOpen = true;
        foreach (var lockpart in _LockParts)
        {
            lockpart.EnablePhysic();
        }
        foreach (var butcol in _ButtonsCol)
        {
            butcol.enabled = false;
        }
        unclocked(gameObject.GetInstanceID());
    }
}
