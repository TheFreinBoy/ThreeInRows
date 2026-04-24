using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Match3/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Board Settings")]
    public int boardWidth = 10;
    public int boardHeight = 9;
    
    public ArrayLayout boardLayout; 
    
    [Header("Level Rules")]
    public float initialTime = 60f;
    public int targetScore = 1000;
    

    [Range(3, 6)]
    public int availableColors = 5; 
}