using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Utility;

public class Waterblight : MonoBehaviour, ISpeedBonus, IDisposable
{
    private GameObject trailObj;
    private float width = 2.0f;
    private float trialDuration = 4.0f;
    private float speedBonus = 2.5f;

    public List<Vector3> GetTrail()
    {
        TrailRenderer trailRenderer = trailObj.GetComponent<TrailRenderer>();
        Vector3[] positions = new Vector3[trailRenderer.positionCount];
        trailRenderer.GetVisiblePositions(positions);
        List<Vector3> positionList = new();
        positionList.AddRange(positions);
        return positionList;
    }

    private Color getColor()
    {
        return GetComponent<ActiveItem_1>() ? Color.cyan : Color.blue;
    }

    private void OnEnable()
    {
        trailObj = new GameObject("WaterTrail");
        trailObj.transform.position = transform.position + Vector3.forward * MapManager.MAP_DEPTH / 2.0f;
        KeepRelativelyStatic constarint = trailObj.AddComponent<KeepRelativelyStatic>();
        constarint.SetReference(transform);
        constarint.SetBias();
        TrailRenderer trailRenderer = trailObj.AddComponent<TrailRenderer>();
        trailRenderer.time = trialDuration;
        trailRenderer.numCapVertices = 8;
        trailRenderer.material = new Material(Shader.Find(DEAFULT_LINE_SHADER_PATH));
        trailRenderer.startColor = getColor();
        trailRenderer.startWidth = width;
        trailRenderer.endColor = new Color(trailRenderer.startColor.r, trailRenderer.startColor.g, trailRenderer.startColor.b, 0.5f);
        trailRenderer.endWidth = width;
        ColliderManager.AddSharedCollider<WaterLayer>(trailRenderer);
    }
    private void OnDisable()
    {
        if (trailObj != null)
        {
            Destroy(trailObj.GetComponent<KeepRelativelyStatic>());
            trailObj.GetComponent<TrailRenderer>().autodestruct = true;
        }
    }

    public float GetValue()
    {
        return speedBonus;
    }

    public class WaterLayer : MonoBehaviour
    {
        private float lingerDuation = 3.0f;
        private float blightInterval = 5.0f;
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<WaterblightImmune>()) return;
            if (collision.GetComponent<Character>() != null ||
                collision.GetComponent<Vehicle>() != null ||
                collision.GetComponent<Enemy>() != null
                )
            {
                StartCoroutine(AddAndRemoveComponent<Waterblight>(collision.gameObject, lingerDuation));
                StartCoroutine(AddAndRemoveComponent<WaterblightImmune>(collision.gameObject, blightInterval));
            }
        }
    }

    private class WaterblightImmune : MonoBehaviour { }
}
