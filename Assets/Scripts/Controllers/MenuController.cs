using UnityEngine;
using MLAPI;
// Important!
using MLAPI.Transports.UNET;
using MLAPI.Spawning;

public class MenuController : MonoBehaviour
{
    // Get the IP from the inspector or the InputField from the start UI
    [SerializeField]
    private string currentIP = "127.0.0.1";
    // Get the Port from the inspector or the InputField from the start UI
    [SerializeField]
    private int currentPort = 7777;
    // パネルメニュー
    [SerializeField]
    private GameObject menuPanel;
    [SerializeField]
    private Vector3 spawnInitPosition;
    private void ApprovalHostCheck(byte[] connectionData, ulong clientId, MLAPI.NetworkingManager.ConnectionApprovedDelegate callback)
    {
        //Your logic here
        bool approve = true;
        bool createPlayerObject = true;

        // The prefab hash. Use null to use the default player prefab
        // If using this hash, replace "MyPrefabHashGenerator" with the name of a prefab added to the NetworkedPrefabs field of your NetworkingManager object in the scene
       
       ulong? prefabHash = SpawnManager.GetPrefabHashFromGenerator("Player2");

        //If approve is true, the connection gets added. If it's false. The client gets disconnected
        callback(createPlayerObject, prefabHash, approve, spawnInitPosition, Quaternion.identity);
    }


    public void StartHosting()
    {
        Debug.Log("Current IP adress: " + currentIP);
        Debug.Log("Current Port: " + currentPort);
        NetworkingManager.Singleton.ConnectionApprovalCallback += ApprovalHostCheck;

        NetworkingManager.Singleton.GetComponent<UnetTransport>().ConnectAddress = currentIP;
        NetworkingManager.Singleton.GetComponent<UnetTransport>().ConnectPort = currentPort;


        NetworkingManager.Singleton.StartHost(spawnInitPosition, Quaternion.identity, true, null, null);
       

        menuPanel.SetActive(false);
    }

    public void StartClient()
    {
        Debug.Log("Current IP adress: " + currentIP);
        Debug.Log("Current Port: " + currentPort);

        NetworkingManager.Singleton.GetComponent<UnetTransport>().ConnectAddress = currentIP;
        NetworkingManager.Singleton.GetComponent<UnetTransport>().ConnectPort = currentPort;
        



        NetworkingManager.Singleton.StartClient();
        // TESTING STUFF:
        //NetworkingManager.Singleton.ConnectedClients[0].PlayerObject = NetworkingManager.Singleton.NetworkConfig.NetworkedPrefabs[1];

        //GameObject go = Instantiate(NetworkingManager.Singleton.NetworkConfig.NetworkedPrefabs[1].Prefab, Vector3.zero, Quaternion.identity);
        //go.GetComponent<NetworkedObject>().SpawnAsPlayerObject(NetworkingManager.Singleton.LocalClientId);

        menuPanel.SetActive(false);

    }

    public void OnCurrentIPChange(string currentIP_inc)
    {
        this.currentIP = currentIP_inc;
    }
    public void OnCurrentPortChange(string currentPort_inc)
    {
        int.TryParse(currentPort_inc, out this.currentPort);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
