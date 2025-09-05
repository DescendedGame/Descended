using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerFunctionality : MonoBehaviour
{

    [SerializeField] NetworkManager networkManager;
    [SerializeField] Button clientButton;
    [SerializeField] Button hostButton;
    [SerializeField] GameObject networkUI;

    private void Awake()
    {
        clientButton.onClick.AddListener(JoinServer);
        hostButton.onClick.AddListener(HostServer);
    }

    public void HostServer()
    {
        networkUI.SetActive(false);
        networkManager.StartHost();
    }

    public void JoinServer()
    {
        networkUI.SetActive(false);
        networkManager.StartClient();
    }
}
