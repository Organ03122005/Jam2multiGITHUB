using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner _runner;

    void Start()
    {
        _runner = FindObjectOfType<NetworkRunner>();
    }

    // 🧑‍🤝‍🧑 คนเข้า
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Player Joined: " + player);

        if (runner.IsServer)
        {
            // Spawn Player Object
            var playerPrefab = Resources.Load<NetworkObject>("Player");
            runner.Spawn(playerPrefab, Vector3.zero, Quaternion.identity, player);
        }
    }

    // ❌ คนออก
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Player Left: " + player);
    }

    // ▶️ ปุ่ม Start (Host เท่านั้น)
    public void StartGame()
    {
        if (_runner.IsServer)
        {
            _runner.LoadScene("GameScene");
        }
    }

    // (ปล่อยว่างได้)
    public void OnInput(NetworkRunner r, NetworkInput i) { }
    public void OnInputMissing(NetworkRunner r, PlayerRef p, NetworkInput i) { }
    public void OnConnectedToServer(NetworkRunner r) { }
    public void OnDisconnectedFromServer(NetworkRunner r, NetDisconnectReason reason) { }
    public void OnConnectFailed(NetworkRunner r, NetAddress addr, NetConnectFailedReason reason) { }
    public void OnConnectRequest(NetworkRunner r, NetworkRunnerCallbackArgs.ConnectRequest req, byte[] token) { }
    public void OnUserSimulationMessage(NetworkRunner r, SimulationMessagePtr msg) { }
    public void OnSessionListUpdated(NetworkRunner r, System.Collections.Generic.List<SessionInfo> list) { }
    public void OnCustomAuthenticationResponse(NetworkRunner r, System.Collections.Generic.Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner r, HostMigrationToken token) { }
    public void OnReliableDataReceived(NetworkRunner r, PlayerRef p, ReliableKey key, System.ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner r, PlayerRef p, ReliableKey key, float progress) { }
    public void OnSceneLoadDone(NetworkRunner r) { }
    public void OnSceneLoadStart(NetworkRunner r) { }
    public void OnObjectEnterAOI(NetworkRunner r, NetworkObject obj, PlayerRef p) { }
    public void OnObjectExitAOI(NetworkRunner r, NetworkObject obj, PlayerRef p) { }
    public void OnShutdown(NetworkRunner r, ShutdownReason reason) { }
}