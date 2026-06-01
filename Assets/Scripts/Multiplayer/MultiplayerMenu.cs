using System;
using Fusion;
using TMPro;
using UnityEngine;

namespace Multiplayer
{
    public class MultiplayerMenu : MonoBehaviour
    {
        public NetworkRunner runnerPrefab;
        public TMP_InputField roomInput;
        public int gameSceneIndex = 1;
        private NetworkRunner _runner;

        private void Start()
        {
            var oldRunners = FindObjectsByType<NetworkRunner>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var oldRunner in oldRunners)
            {
                Debug.Log("<color=yellow>[Network] Deleting old session </color>");
                oldRunner.Shutdown(); 
                Destroy(oldRunner.gameObject); 
            }
        }
        
        public async void ConnectToRoom()
        {
            if (string.IsNullOrEmpty(roomInput.text))
                return;
            
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
                SceneManager = sceneManager,
                
                Scene = SceneRef.FromIndex(gameSceneIndex)
            });

            if (result.Ok)
            {
                Debug.Log("<color=green> Access!</color>");
            }
            else
            {
                Debug.LogError($"Connection error: {result.ErrorMessage}");
            }
        }
    }
}
