using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class TeleportController : MonoBehaviour
{
    
    [SerializeField] private NetworkPlayer _networkPlayer;
    
    public GameObject baseControllerGameObject;
    public GameObject teleportationGameobject;

    public InputActionReference teleportActivationReference;

    [Space]
    public UnityEvent onTeleportActivate;
    public UnityEvent onTeleportCancel;

    private void Start()
    {
        InitializeTeleport();

    }


    void InitializeTeleport()
    {
        if (_networkPlayer.IsMinePhoton)
        {
            Debug.Log("<color=#FCB354>si IsMine true TELEPORT</color>" + name);
            teleportActivationReference.action.performed += TeleportModeActivate;
            teleportActivationReference.action.canceled += TeleportModeCancel;    
        }
        
    }

    private void TeleportModeCancel(InputAction.CallbackContext obj)
    {
        Invoke("DeactivateTeleporter", .1f);
    }

    void DeactivateTeleporter() => onTeleportCancel.Invoke();

    private void TeleportModeActivate(InputAction.CallbackContext obj) => onTeleportActivate.Invoke();
}
