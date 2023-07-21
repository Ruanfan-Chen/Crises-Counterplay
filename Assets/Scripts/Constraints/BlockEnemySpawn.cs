using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEnemySpawn : MonoBehaviour
{
    private void OnEnable()
    {
        EnemySpawn.AddBlocker(this);
    }

    private void OnDisable()
    {
        EnemySpawn.RemoveBlocker(this);
    }
}
