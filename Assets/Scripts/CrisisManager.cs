using UnityEngine;

public class CrisisManager : MonoBehaviour
{
    [SerializeField] private float m_startDelay;

    [SerializeField] private float m_vehicleIntervalMin;
    [SerializeField] private float m_vehicleIntervalMax;
    [SerializeField] private float m_vehicleTraceDuration;
    [SerializeField] private float m_vehicleStartDelay;
    [SerializeField] private float m_vehicleSpeedMin;
    [SerializeField] private float m_vehicleSpeedMax;
    [SerializeField] private float m_vehicleContactDamage;

    [SerializeField] private float m_electricFieldIntervalMin;
    [SerializeField] private float m_electricFieldIntervalMax;
    [SerializeField] private float m_electricFieldTraceDuration;
    [SerializeField] private float m_electricFieldStartDelay;
    [SerializeField] private float m_electricFieldRadius;
    [SerializeField] private float m_electricFieldDuration;
    [SerializeField] private float m_electricFieldDamage;

    private float vehicleTimer;
    private float electricField;

    private static bool activated;

    void Start()
    {
        vehicleTimer = m_startDelay;
        electricField = m_startDelay;
    }

    void Update()
    {
        if (activated)
        {
            vehicleTimer -= Time.deltaTime;
            if (vehicleTimer <= 0.0f && LevelManager.GetSpawnVehicle())
            {
                SpawnVehicle();
                vehicleTimer = Random.Range(m_vehicleIntervalMin, m_vehicleIntervalMax);
            }
            electricField -= Time.deltaTime;
            if (electricField <= 0.0f && LevelManager.GetSpawnElectricityField())
            {
                SpawnElectricField();
                electricField = Random.Range(m_electricFieldIntervalMin, m_electricFieldIntervalMax);
            }
        }

    }

    void SpawnVehicle()
    {
        float length = MapManager.GetBounds().size.magnitude;
        Vector3 startPos = MapManager.GetRandomPointOnEdge();
        Vector3 targetPos = startPos + (GameplayManager.getCharacter().transform.position - startPos).normalized * length;

        StartCoroutine(Vehicle.Instantiate(startPos, targetPos, m_vehicleTraceDuration, m_vehicleStartDelay, Random.Range(m_vehicleSpeedMin, m_vehicleSpeedMax), m_vehicleContactDamage, true));
    }

    void SpawnElectricField()
    {
        Vector3 position = GameplayManager.getCharacter().transform.position + (Vector3)Random.insideUnitCircle * m_electricFieldRadius * 1.5f;
        StartCoroutine(ElectricField.Instantiate(position, m_electricFieldTraceDuration, m_electricFieldStartDelay, m_electricFieldRadius, m_electricFieldDuration, m_electricFieldDamage));
    }

    public static void Activate()
    {
        activated = true;
    }

    public static void Deactivate()
    {
        activated = false;
    }
}
