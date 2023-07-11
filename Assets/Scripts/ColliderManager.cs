using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColliderManager
{
    private static readonly Dictionary<Type, GameObject> colliderObjects = new();
    public static void AddSharedCollider<T>(TrailRenderer trailRenderer) where T : MonoBehaviour
    {
        ClearNull();
        if (!colliderObjects.ContainsKey(typeof(T)))
        {
            colliderObjects.Add(typeof(T), new(typeof(T) + "Layer", new[] { typeof(CustomCollider2D), typeof(Rigidbody2D), typeof(T), typeof(SharedCollider) }));
            colliderObjects[typeof(T)].GetComponent<CustomCollider2D>().isTrigger = true;
            colliderObjects[typeof(T)].GetComponent<Rigidbody2D>().isKinematic = true;
        }
        colliderObjects[typeof(T)].GetComponent<SharedCollider>().Subscribe(trailRenderer);
    }

    public static void RemoveSharedCollider<T>(TrailRenderer trailRenderer) where T : MonoBehaviour
    {
        ClearNull();
        if (colliderObjects.ContainsKey(typeof(T)))
        {
            colliderObjects[typeof(T)].GetComponent<SharedCollider>().Unsubscribe(trailRenderer);
        }
    }

    private static void ClearNull()
    {
        foreach (KeyValuePair<Type, GameObject> kvp in colliderObjects.Where(kvp => kvp.Value == null).ToList())
            colliderObjects.Remove(kvp.Key);
    }

    private class SharedCollider : MonoBehaviour
    {
        private List<TrailRenderer> trailRenderers = new();

        public void Subscribe(TrailRenderer trailRenderer)
        {
            trailRenderers.Add(trailRenderer);
        }

        public void Unsubscribe(TrailRenderer trailRenderer)
        {
            trailRenderers.Remove(trailRenderer);
        }

        void Update()
        {
            PhysicsShapeGroup2D shapeGroup = new();
            foreach (TrailRenderer trailRenderer in trailRenderers)
            {
                Vector3[] positions = new Vector3[trailRenderer.positionCount];
                int visiblePositionCount = trailRenderer.GetVisiblePositions(positions);
                if (visiblePositionCount < 2)
                    continue;
                List<Vector2> vertices = new();
                for (int i = 0; i < visiblePositionCount; i++)
                    vertices.Add(positions[i]);
                shapeGroup.AddEdges(vertices);
            }
            GetComponent<CustomCollider2D>().SetCustomShapes(shapeGroup);
        }
    }
}
