using UnityEngine;
using UnityEditor;

namespace Tara
{
[CustomEditor(typeof(AsteroidManager))]
public class AsteriodSpawnerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AsteroidManager spawner = (AsteroidManager)target;

        if (spawner.MinSpawnRange - 1f > spawner.MaxSpawnRange)
        {
            spawner.MaxSpawnRange += spawner.MinSpawnRange - 1f - spawner.MaxSpawnRange;
        } 
        else if (spawner.MaxSpawnRange -1f < spawner.MinSpawnRange)
        {
            spawner.MinSpawnRange -= (spawner.MinSpawnRange) - spawner.MaxSpawnRange;
        }
        if (spawner.MinSpawnRange == spawner.MaxSpawnRange)
        {
            spawner.MinSpawnRange -= 1f;
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