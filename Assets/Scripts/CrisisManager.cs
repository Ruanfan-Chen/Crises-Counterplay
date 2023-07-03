using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CrisisManager : MonoBehaviour
{
    [SerializeField] private GameObject m_player;
    [SerializeField] private float m_startDelay;
    // event will occur within [m_event_interval_min, m_event_interval_max] after last occurrence
    [SerializeField] private float m_eventIntervalMin; // min time (in seconds) after occurrence event occurred
    [SerializeField] private float m_eventIntervalMax; // max time (in seconds) after last event occurred    
    [SerializeField] private float m_vehicleTraceDuration;
    [SerializeField] private float m_vehicleStartDelay;
    [SerializeField] private float m_vehicleSpeed;
    [SerializeField] private float m_vehicleContactDamage;
    [SerializeField] private float m_electricFieldTraceDuration;
    [SerializeField] private float m_electricFieldStartDelay;
    [SerializeField] private float m_electricFieldRadius;
    [SerializeField] private float m_electricFieldDuration;
    [SerializeField] private float m_electricFieldDamage;
    private float eventTimer;

    void Start()
    {
        // Update event interval; m_level >= 1
        //m_event_interval_max = Mathf.Max(1.0f, m_event_interval_max - (m_level - 1.0f));
        //m_event_interval_min = Mathf.Max(1.0f, m_event_interval_min - (m_level - 1.0f));
        //Debug.Log("m_event_interval_min = " + m_event_interval_min + ", m_event_interval_max = " + m_event_interval_max);
        eventTimer = Random.Range(m_eventIntervalMin, m_eventIntervalMax) + m_startDelay;
    }

    // Update is called once per frame
    void Update()
    {
        eventTimer -= Time.deltaTime;
        if (eventTimer <= 0)
        {
            int r = Utility.WeightedRandom(new Dictionary<int, float>() { [0] = 1.0f, [1] = 1.0f });
            switch (r)
            {
                case 0:
                    SpawnVehicle();
                    break;
                case 1:
                    SpawnElectricField();
                    break;
            }
            eventTimer = Random.Range(m_eventIntervalMin, m_eventIntervalMax);
        }
    }

    void SpawnVehicle()
    {
        float length = 2.24f * 30.0f;
        Vector3 startPos = GetComponent<MapManager>().GetRandomPointOnEdge();
        Vector3 targetPos = startPos + (m_player.transform.position - startPos).normalized * length;

        StartCoroutine(Vehicle.Instantiate(startPos, targetPos, m_vehicleTraceDuration, m_vehicleStartDelay, m_vehicleSpeed, m_vehicleContactDamage, true));
    }

    void SpawnElectricField()
    {
        Vector2 mapScale = GetComponent<MapManager>().GetMapScale();
        Vector3 position = new Vector3(Random.Range(-mapScale.x / 2, mapScale.x / 2), Random.Range(-mapScale.y / 2, mapScale.y / 2), 0);
        StartCoroutine(ElectricField.Instantiate(m_player.GetComponent<Player>().GetCharacter(), position, m_electricFieldTraceDuration, m_electricFieldStartDelay, m_electricFieldRadius, m_electricFieldDuration, m_electricFieldDamage));
    }

    void SpawnTidalWave()
    {

    }
}
