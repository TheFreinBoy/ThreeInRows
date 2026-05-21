using Fusion;
using TMPro;
using UnityEngine;

namespace Multiplayer
{
    public class MultiplayerMenu : MonoBehaviour
    {
        public NetworkObject playerPrefab;
        public NetworkRunner runnerPrefab;
        public TMP_InputField roomInput;
        
        private NetworkRunner _runner;

        public async void ConnectToRoom()
        {
            if (string.IsNullOrEmpty(roomInput.text))
            {
                return;
            }

            if (_runner == null)
            {
                _runner = Instantiate(runnerPrefab);
            }
            
            var sceneManager = _runner.gameObject.GetComponent<NetworkSceneManagerDefault>();
            if (sceneManager == null)
                sceneManager = _runner.gameObject.AddComponent<NetworkSceneManagerDefault>();

            Debug.Log($"Connecting to Photon servers. Room: {roomInput.text}...");
            
            var result = await _runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Shared,
                SessionName = roomInput.text,
                SceneManager = sceneManager
            });

            if (result.Ok)
            {
                Debug.Log("<color=green> Access!</color>");
                _runner.Spawn(playerPrefab, Vector3.zero, Quaternion.identity, _runner.LocalPlayer);
            }
            else
            {
                Debug.LogError($"Connection error: {result.ErrorMessage}");
            }
        }
    }
}
