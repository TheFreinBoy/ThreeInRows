using Fusion;
using System.Linq;
using UnityEngine;

namespace Multiplayer
{
    public class ArenaManager : MonoBehaviour
    {
        [SerializeField] private BoardService _boardService;
        [SerializeField] private NetworkObject _playerPrefab;

        private bool _isGameStarted;
        private bool _isLocalPlayerSpawned;
        private NetworkRunner _runner;

        private void Update()
        {
            if (_runner == null)
            {
                _runner = FindFirstObjectByType<NetworkRunner>();
                if (_runner == null) return;
            }

            if (!_isLocalPlayerSpawned && _runner.IsRunning)
            {
                _runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, _runner.LocalPlayer);
                _isLocalPlayerSpawned = true;
            }

            if (!_isGameStarted && _runner.ActivePlayers.Count() == 2)
            {
                _isGameStarted = true;

                if (_boardService != null)
                {
                    _boardService.StartMatch();
                }
            }
        }
    }
}