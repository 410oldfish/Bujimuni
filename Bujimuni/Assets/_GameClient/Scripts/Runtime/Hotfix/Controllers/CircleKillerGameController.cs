using System;
using CircleKiller;
using Lighten;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class CircleKillerGameController : XEntityController
{
    public GameObject EnemyPrefab;
    public float SpawnTime = 1;

    private float m_spawnTimeCounter;

    protected override void OnEntityAwake()
    {
        base.OnEntityAwake();
    }

    protected override void OnEntityDestroy()
    {
        base.OnEntityDestroy();
    }

    private void Update()
    {
        if (!this.GetModel<CircleKillerModel>().IsRunning)
            return;
        m_spawnTimeCounter += Time.deltaTime;
        if (m_spawnTimeCounter >= this.SpawnTime)
        {
            var go = Instantiate(this.EnemyPrefab, Random.insideUnitCircle * 5f, Quaternion.identity);
            //var enemyController = go.GetComponent<SampleEnemyController>();
            m_spawnTimeCounter = 0f;
        }
    }
}