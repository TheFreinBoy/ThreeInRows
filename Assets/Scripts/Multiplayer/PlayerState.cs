using Fusion;
using UnityEngine;

namespace Multiplayer
{
    public class PlayerState : NetworkBehaviour 
    {
        [Networked] public int Score { get; set; }
        
        public override void Spawned()
        {
            if (HasStateAuthority)
            {
                Debug.Log("<color=cyan>My avatar created!</color>");
                // "left UI"
            }
            else
            {
                Debug.Log("<color=yellow>Ого, enemy connected!</color>");
                //"right UI,"
            }
        }
    }
}