using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class TeleportController : MonoBehaviour
{
    
    [SerializeField] private NetworkManager _networkManager;
    [SerializeField] private NetworkPlayer _networkPlayer;


    public GameObject baseControllerGameObject;
    public GameObject teleportationGameobject;

    public InputActionReference teleportActivationReference;

    [Space]
    public UnityEvent onTeleportActivate;
    public UnityEvent onTeleportCancel;

    private void Start()
    {
        _networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();
        _networkManager.OnPlayerFinishedConnect += InitializeTeleport;

    }


    void InitializeTeleport()
    {
        if (_networkPlayer.IsMinePhoton)
        {
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
