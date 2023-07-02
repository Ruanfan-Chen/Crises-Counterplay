using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawn : MonoBehaviour
{
    [SerializeField] private float m_startDelay;
    // event will occur within [m_event_intervl_min, m_event_intervl_max] after last occurance
    [SerializeField] private float m_event_intervl_min; // min time (in seconds) after last event occurred
    [SerializeField] private float m_event_intervl_max; // max time (in seconds) after last event occurred
    [SerializeField] private GameObject m_player;
    private float vehicleTimer;

    void Start()
    {
        // Update event interval; m_level >= 1
        //m_event_intervl_max = Mathf.Max(1.0f, m_event_intervl_max - (m_level - 1.0f));
        //m_event_intervl_min = Mathf.Max(1.0f, m_event_intervl_min - (m_level - 1.0f));
        //Debug.Log("m_event_intervl_min = " + m_event_intervl_min + ", m_event_intervl_max = " + m_event_intervl_max);
        vehicleTimer = Random.Range(m_event_intervl_min, m_event_intervl_max) + m_startDelay;
    }

    // Update is called once per frame
    void Update()
    {
        vehicleTimer -= Time.deltaTime;
        if (vehicleTimer <= 0)
        {
            float length = 2.24f * 30.0f;
            Vector3 startPos = GetComponent<MapManager>().GetRandomPointOnEdge();
            Vector3 targetPos = startPos + (m_player.transform.position - startPos).normalized * length;

            Vehicle.Instantiate(startPos, targetPos);
            vehicleTimer = Random.Range(m_event_intervl_min, m_event_intervl_max);
        }
    }
}
