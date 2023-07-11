using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColliderManager
{
    private static readonly Dictionary<Type, GameObject> colliderObjects = new();
    public static PhysicsShapeGroup2D GetShapeGroup<T>()
    {
        ConstructIfNew<T>();
        return colliderObjects[typeof(T)].GetComponent<SharedCollider>().GetShapeGroup(); 
    }
    public static void AddSharedCollider<T>(TrailRenderer trailRenderer) where T : MonoBehaviour
    {
        ClearNull();
        ConstructIfNew<T>();
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

    private static void ConstructIfNew<T>()
    {
        if (!colliderObjects.ContainsKey(typeof(T)))
        {
            colliderObjects.Add(typeof(T), new(typeof(T) + "Layer", new[] { typeof(CustomCollider2D), typeof(Rigidbody2D), typeof(T), typeof(SharedCollider) }));
            colliderObjects[typeof(T)].GetComponent<CustomCollider2D>().isTrigger = true;
            colliderObjects[typeof(T)].GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }

    private static void ClearNull()
    {
        foreach (KeyValuePair<Type, GameObject> kvp in colliderObjects.Where(kvp => kvp.Value == null).ToList())
            colliderObjects.Remove(kvp.Key);
    }

    private class SharedCollider : MonoBehaviour
    {
        private readonly List<TrailRenderer> trailRenderers = new();
        private readonly PhysicsShapeGroup2D shapeGroup = new();

        public PhysicsShapeGroup2D GetShapeGroup() { return shapeGroup; }

        public void Subscribe(TrailRenderer trailRenderer)
        {
            trailRenderers.Add(trailRenderer);
        }

        public void Unsubscribe(TrailRenderer trailRenderer)
        {
            trailRenderers.Remove(trailRenderer);
        }
        private void ClearNull()
        {
            foreach (TrailRenderer trailRenderer in trailRenderers.Where(trailRenderer => trailRenderer == null).ToList())
                trailRenderers.Remove(trailRenderer);
        }

        void Update()
        {
            ClearNull();
            shapeGroup.Clear();
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
