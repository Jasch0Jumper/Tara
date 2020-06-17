using UnityEngine;
using UnityEditor;

namespace Tara
{
[CustomEditor(typeof(AsteriodManager))]
public class AsteriodSpawnerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AsteriodManager spawner = (AsteriodManager)target;

        if (spawner.minSpawnRange - 1f > spawner.maxSpawnRange)
        {
            spawner.maxSpawnRange += spawner.minSpawnRange - 1f - spawner.maxSpawnRange;
        } 
        else if (spawner.maxSpawnRange -1f < spawner.minSpawnRange)
        {
            spawner.minSpawnRange -= (spawner.minSpawnRange) - spawner.maxSpawnRange;
        }
        if (spawner.minSpawnRange == spawner.maxSpawnRange)
        {
            spawner.minSpawnRange -= 1f;
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Summon Asteriods"))
        {
            spawner.SummonAsteriods();
        }
        if (GUILayout.Button("Destroy Asteriods"))
        {
            spawner.DestroyAsteriods();
        }
        if (GUILayout.Button("Reset Asteriods"))
        {
            spawner.ResetAsteriods();
        }
    }
}
}