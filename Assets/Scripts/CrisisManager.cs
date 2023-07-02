using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrisisManager : MonoBehaviour
{
    [SerializeField] private float m_startDelay;
    // event will occur within [m_event_interval_min, m_event_interval_max] after last occurrence
    [SerializeField] private float m_event_interval_min; // min time (in seconds) after occurrence event occurred
    [SerializeField] private float m_event_interval_max; // max time (in seconds) after last event occurred
    [SerializeField] private GameObject m_player;
    private float eventTimer;

    void Start()
    {
        // Update event interval; m_level >= 1
        //m_event_interval_max = Mathf.Max(1.0f, m_event_interval_max - (m_level - 1.0f));
        //m_event_interval_min = Mathf.Max(1.0f, m_event_interval_min - (m_level - 1.0f));
        //Debug.Log("m_event_interval_min = " + m_event_interval_min + ", m_event_interval_max = " + m_event_interval_max);
        eventTimer = Random.Range(m_event_interval_min, m_event_interval_max) + m_startDelay;
    }

    // Update is called once per frame
    void Update()
    {
        eventTimer -= Time.deltaTime;
        if (eventTimer <= 0)
        {
            SpawnVehicle();
            eventTimer = Random.Range(m_event_interval_min, m_event_interval_max);
        }
    }

    void SpawnVehicle() {
        float length = 2.24f * 30.0f;
        Vector3 startPos = GetComponent<MapManager>().GetRandomPointOnEdge();
        Vector3 targetPos = startPos + (m_player.transform.position - startPos).normalized * length;

        Vehicle.Instantiate(startPos, targetPos);
    }

    void SpawnElectricField() {

    }

    void SpawnTidalWave()
    {

    }
}
