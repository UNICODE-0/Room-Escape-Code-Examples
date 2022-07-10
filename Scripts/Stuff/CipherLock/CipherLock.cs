using System.Collections.Generic;
using UnityEngine;

public enum CombinationSymbol
{
    Polygon,
    HollowPolygon,
    Circle,
    HollowCircle,
    Square,
    HollowSquare

}
[RequireComponent(typeof(AudioSource))]
public class CipherLock : MonoBehaviour
{
    [SerializeField] private CipherLockButton[] _Buttons;
    [SerializeField] private LockPart[] _LockParts;
    [SerializeField] private CombinationSymbol[] _LockCipher;
    private int _SymbolsForCombination;
    private Stack<CombinationSymbol> _Combination = new Stack<CombinationSymbol>();
    private AudioSource _AudioSource;
    private bool _IsOpen;
    public bool isOpen
    {
        get { return _IsOpen; }
    }
    
    public delegate void Unclocked(int LockID);
    public static event Unclocked unclocked;

    private void OnEnable() 
    {
        CipherLockButton.buttonDown += CheckCombination;
    } 
    private void OnDisable() 
    {
        CipherLockButton.buttonDown -= CheckCombination;
    } 
    private void Start() 
    {
        _AudioSource = GetComponent<AudioSource>();
        _IsOpen = false;
        _SymbolsForCombination = _LockCipher.Length;
    }

    private void CheckCombination(CombinationSymbol Symbol, int ButtonId)
    {
        bool ButtonExist = false;
        foreach (var ButtonCol in _Buttons)
        {
            if(ButtonCol.gameObject.GetInstanceID() == ButtonId) ButtonExist = true; 
        }
        if(ButtonExist == false) return;

        _Combination.Push(Symbol);
        
        if(_Combination.Count == _SymbolsForCombination)
        {
            int MatchCount = 0;
            for (int i = _SymbolsForCombination - 1; i >= 0; i--)
            {
                if(_LockCipher[i] == _Combination.Pop()) MatchCount++;
            }

            if(MatchCount == _SymbolsForCombination)
            {
                Open();
            } 
            else
            {
                ResetCombination();
            }
            
        } 
    }
    private void Open()
    {
        _AudioSource.Play();
        foreach (var lockpart in _LockParts)
        {
            lockpart.EnablePhysic();
        }
        foreach (var butcol in _Buttons)
        {
            butcol.GetComponent<Collider>().enabled = false;
        }
        unclocked(gameObject.GetInstanceID());
    }
    private void ResetCombination()
    {
        _Combination.Clear();
        foreach (var button in _Buttons)
        {
            if(button.isDown) button.Use();
        }

    }
}
