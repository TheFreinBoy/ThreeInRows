using Fusion;

namespace Multiplayer
{
    public class NetworkBoardState : NetworkBehaviour
    {
        [Networked, Capacity(100)] 
        public NetworkArray<int> BoardData { get; }
        
        [Networked] 
        public int ActualWidth { get; set; }
    
        [Networked] 
        public int ActualHeight { get; set; }

        public void UpdateCell(int x, int y, int boardWidth, int boardHeight, int cellTypeValue)
        {
            if (HasStateAuthority)
            {
                ActualWidth = boardWidth;
                ActualHeight = boardHeight;
                
                int index = y * boardWidth + x;
                BoardData.Set(index, cellTypeValue);
            }
        }
    }
}