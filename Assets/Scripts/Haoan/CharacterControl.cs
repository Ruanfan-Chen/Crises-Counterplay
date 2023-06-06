using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    [SerializeField] GameObject m_projectile;
    [SerializeField] float m_range;
    [SerializeField] bool m_drawRange;
    [SerializeField] float m_atkInterval;
    [SerializeField] float m_angle;
    [SerializeField] Color m_projectileColor;
    // Start is called before the first frame update
    void Start()
    {
        transform.forward = new Vector3(0.0f, 1.0f, 0.0f);
        InvokeRepeating("LaunchProjectile", 0.0f, m_atkInterval);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_drawRange)
        {
            DrawTool.DrawSector(transform, transform.localPosition, 120.0f, m_range);
        }
    }

    void LaunchProjectile()
    {
        GameObject projectile = Instantiate(m_projectile, transform.position, transform.rotation * Quaternion.Euler(0, 0, Random.Range(-m_angle / 2, m_angle / 2)));
        projectile.GetComponent<SpriteRenderer>().color = Random.ColorHSV(0.0f, 1.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 1.0f);
    }
}
