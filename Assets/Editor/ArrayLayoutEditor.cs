using StaticData;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(ArrayLayout))]
    public class ArrayLayoutEditor : PropertyDrawer
    {
        private const float CellPadding = 24f;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PrefixLabel(position, label);
            var reposition = position;
            reposition.y += CellPadding;
            var data = property.FindPropertyRelative("rows");
            
            // Get the parent object to access LevelData
            if (property.serializedObject.targetObject is not LevelData levelData)
                return;

            int boardHeight = levelData.boardHeight;
            int boardWidth = levelData.boardWidth;
            
            if (data.arraySize != boardHeight)
                data.arraySize = boardHeight;

            for (var j = 0; j < boardHeight; j++)
            {
                var row = data.GetArrayElementAtIndex(j).FindPropertyRelative("row");
                reposition.height = CellPadding;
                if (row.arraySize != boardWidth)
                    row.arraySize = boardWidth;
                reposition.width = position.width / boardWidth;
                for (var i = 0; i < boardWidth; i++)
                {
                    EditorGUI.PropertyField(reposition, row.GetArrayElementAtIndex(i), GUIContent.none);
                    reposition.x += reposition.width;
                }
                reposition.x = position.x;
                reposition.y += CellPadding;
            }
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Get the parent object to access LevelData
            if (property.serializedObject.targetObject is not LevelData levelData)
                return CellPadding;
                
            return CellPadding * (levelData.boardHeight + 1);
        }
    }
}