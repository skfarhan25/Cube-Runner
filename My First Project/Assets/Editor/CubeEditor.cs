using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyMovement))]
public class NewBehaviourScript : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EnemyMovement enemyMovement = (EnemyMovement)target;

        GUILayout.BeginHorizontal();

            if (GUILayout.Button("Rotate to 0"))
            {
                enemyMovement.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            if (GUILayout.Button("Rotate to 90"))
            {
                enemyMovement.transform.rotation = Quaternion.Euler(0, 90, 0);
            }

        GUILayout.EndHorizontal();

    }
}
