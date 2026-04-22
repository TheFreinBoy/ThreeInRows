using Match3;
using Unity.VisualScripting;
using StaticData;


[System.Serializable]
public class ArrayLayout
{
    [System.Serializable]
    public struct RowData
    {
       public bool[] row;
    }
    public RowData[] rows = new RowData[Config.BoardWidth];
}