using UnityEngine;

public enum FormationLayoutType { Grid, Manual }

[CreateAssetMenu(fileName = "FormationLayoutSO", menuName = "Scriptable Objects/FormationLayoutSO")]
public class FormationLayoutSO : ScriptableObject
{
    public FormationLayoutType layoutType;

    [Header("Grid settings")] 
    public int rows;
    public int columns;
    public float spacing;

    [Header("Manual settings")] 
    public Transform formationRoot;

    public Vector3[] GetSlotPositions() //Método que determina qual layout será usado a partir do layoutType
    {
        if (layoutType == FormationLayoutType.Grid)
            return GetGridPositions();

        return GetManualPositions();
    }

    private Vector3[] GetGridPositions() // Método que calcula o grid
    {
        Vector3[] positions = new Vector3[rows * columns];
        int index = 0;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                float x = (col - (columns - 1) / 2.0f) * spacing;
                float z = (row - (rows - 1) / 2.0f) * spacing;
                positions[index] = new Vector3(x, 0, z);
                index++;
            }
        }

        return positions;
    }

    private Vector3[] GetManualPositions() // Método que lê a posição local dos filhos
    {
        if (formationRoot == null) return new Vector3[0];

        Vector3[] positions = new Vector3[formationRoot.childCount];

        for (int i = 0; i < formationRoot.childCount; i++)
            positions[i] = formationRoot.GetChild(i).localPosition;

        return positions;
    }
}
