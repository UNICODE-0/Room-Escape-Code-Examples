using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    static public PlayerMovement movement { get; set; }
    static public PlayerDragObjects dragObjects { get; set; }
    static public PlayerInteractionObjects interactionObjects { get; set; }
    static public PlayerSlidingObject slidingObjects { get; set; }
    static public CharacterController characterController { get; set; }
    static public PlayerCamera playerCamera { get; set; }
    static public PlayerRaycast viewRaycast { get; set; }
    static public PlayerAnimator animator { get; set; }
    static public PlayerUI UI { get; set; }
    static public bool playerExist { 
        get
        {
            return movement && dragObjects;
        }
    }
}
