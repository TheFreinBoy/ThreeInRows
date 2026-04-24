using StaticData;

[System.Serializable]
public class ArrayLayout
{
    [System.Serializable]
    public struct RowData
    {
       public bool[] row;
    }
    
    public RowData[] rows;
    
    public ArrayLayout(int width, int height)
    {
        rows = new RowData[height];
        for (int i = 0; i < height; i++)
        {
            rows[i].row = new bool[width];
        }
    }
}