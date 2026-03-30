using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;
using TMPro;
public class FusionConnectionHandler : MonoBehaviour
{
    private NetworkRunner _runner;
    [Header("UI References")]
    public TMP_InputField roomCodeInput;

    private NetworkSceneManagerDefault _sceneManager;
    public GameObject connectionUiRoot;
    public GameObject leaveRoomButton;

    private bool _callbacksRegistered;
    private void Awake()
    {
        SetConnectedUi(false);
    }
    private void EnsureRunner()
    {
        if (_runner != null && _sceneManager != null)
        {
            return;
        }
        var runnerGo = new GameObject("FusionRunner");
        DontDestroyOnLoad(runnerGo);
        _runner = runnerGo.AddComponent<NetworkRunner>();
        _sceneManager = runnerGo.AddComponent<NetworkSceneManagerDefault>();
        _callbacksRegistered = false;
    }

    private void SetConnectedUi(bool isConnected)
    {
        if (connectionUiRoot != null)
        {
            connectionUiRoot.SetActive(!isConnected);
        }
        if (leaveRoomButton != null)
        {
            leaveRoomButton.SetActive(isConnected);
        }
    }


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void JoinAsHost()
    {
        string roomName = roomCodeInput != null ? roomCodeInput.text : "AutoRoom";
        StartGame(GameMode.Host, roomName);
    }
    public void JoinAsClient()
    {
        string roomName = roomCodeInput != null ? roomCodeInput.text : "AutoRoom";
        StartGame(GameMode.Client, roomName);
    }
    public void JoinAsAuto()
    {
        string roomName = roomCodeInput != null ? roomCodeInput.text : "AutoRoom";
        StartGame(GameMode.AutoHostOrClient, roomName);
    }
    public async void StartGame(GameMode mode, string roomName)
    {
        if (_runner == null)
        {
            _runner = gameObject.AddComponent<NetworkRunner>();
        }
        EnsureRunner();
        RegisterRunnerCallbacks();
        _runner.ProvideInput = true;

        var sceneInfo = new NetworkSceneInfo();
        sceneInfo.AddSceneRef(SceneRef.FromIndex(SceneManager.GetActiveScene().
        buildIndex), LoadSceneMode.Additive);
        var result = await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomName,
            Scene = sceneInfo,
            SceneManager =

        gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
        if (result.Ok)
        {
            Debug.Log($"Joined as {mode} in room: {roomName}");
            SetConnectedUi(true);
        }
        else
        {
            Debug.LogError($"Error: {result.ShutdownReason} |{result.ErrorMessage}");
            SetConnectedUi(false);
        }
    }

    public async void LeaveRoom()
    {
        if (_runner == null)
        {
            SetConnectedUi(false);
            return;
        }
        await _runner.Shutdown();
        if (_runner != null && _runner.gameObject != null)
        {
            Destroy(_runner.gameObject);
        }
        _runner = null;
        _sceneManager = null;
        _callbacksRegistered = false;
        SetConnectedUi(false);
    }
    private void RegisterRunnerCallbacks()
    {
        if (_runner == null || _callbacksRegistered)
        {
            return;
        }
        var allBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (var behaviour in allBehaviours)
        {
            if (behaviour is INetworkRunnerCallbacks callbacks)
            {
                _runner.AddCallbacks(callbacks);
            }
        }
        _callbacksRegistered = true;
    }

}
