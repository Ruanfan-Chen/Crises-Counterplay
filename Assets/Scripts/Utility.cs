using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utility
{
    static string DEAFULT_LINE_SHADER_PATH = "Legacy Shaders/Particles/Alpha Blended Premultiply";
    public static T WeightedRandom<T>(Dictionary<T, float> weightDict)
    {
        float r = UnityEngine.Random.Range(0.0f, weightDict.Sum(kvp => kvp.Value));
        foreach (KeyValuePair<T, float> kvp in weightDict)
        {
            if (r <= kvp.Value)
                return kvp.Key;
            else
                r -= kvp.Value;
        }
        return default;
    }

    public static IEnumerator ForcedMovement(Transform transform, Vector3 displacement, float initialSpeed, float duration)
    {
        float current = 0.0f;
        float currentVelocity = initialSpeed / displacement.magnitude;
        while (current < 1.0f)
        {
            float next = Mathf.SmoothDamp(current, 1.0f, ref currentVelocity, duration);
            transform.Translate((next - current) * displacement);
            current = next;
            yield return null;
        }
    }

    public static IEnumerator AddAndRemoveComponent<T>(GameObject gameObject, float duration) where T : Component
    {
        T component = gameObject.AddComponent<T>();
        yield return new WaitForSeconds(duration);
        UnityEngine.Object.Destroy(component);
    }

    public class BiDictionary<T, U>
    {
        private readonly Dictionary<T, U> TUDict = new();
        private readonly Dictionary<U, T> UTDict = new();

        public IReadOnlyDictionary<T, U> GetTUDict() { return TUDict; }
        public IReadOnlyDictionary<U, T> GetUTDict() { return UTDict; }

        public U this[T t] { get => TUDict[t]; set => Add(t, value); }
        public T this[U u] { get => UTDict[u]; set => Add(value, u); }

        public bool Remove(T t)
        {
            if (TUDict.TryGetValue(t, out U u))
            {
                TUDict.Remove(t);
                UTDict.Remove(u);
                return true;
            }
            return false;
        }

        public bool Remove(U u)
        {
            if (UTDict.TryGetValue(u, out T t))
            {
                UTDict.Remove(u);
                TUDict.Remove(t);
                return true;
            }
            return false;
        }

        public bool TryGetValue(T t, out U u)
        {
            return TUDict.TryGetValue(t, out u);
        }

        public bool TryGetValue(U u, out T t)
        {
            return UTDict.TryGetValue(u, out t);
        }

        public void Add(T t, U u)
        {
            if (TUDict.ContainsKey(t))
                Remove(t);
            if (UTDict.ContainsKey(u))
                Remove(u);
            TUDict[t] = u;
            UTDict[u] = t;
        }

        public void Clear()
        {
            TUDict.Clear();
            UTDict.Clear();
        }
    }

    public static GameObject DrawLine(string name, Vector3 start, Vector3 end, Color color, float width = 0.2f)
    {
        GameObject lineObj = new GameObject(name);
        lineObj.tag = "Disposable";
        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

        //Use Dedault-Line Material
        lineRenderer.material = new Material(Shader.Find(DEAFULT_LINE_SHADER_PATH));

        //Set color
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        //Set width
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        //Set line count which is 2
        lineRenderer.positionCount = 2;

        //Set the position of both two lines
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        return lineObj;
    }

    public static GameObject DrawCircle(string name, Vector3 center, float radius, Color color, int erpDensity = 24, float width = 0.2f)
    {
        GameObject lineObj = new GameObject(name);
        lineObj.tag = "Disposable";
        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

        //Use Dedault-Line Material
        lineRenderer.material = new Material(Shader.Find(DEAFULT_LINE_SHADER_PATH));

        //Set color
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        //Set width
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        //Set line count which is erpDensity
        lineRenderer.positionCount = erpDensity;

        //Connect loop
        lineRenderer.loop = true;

        //Set the positions
        for (int i = 0; i < erpDensity; i++)
        {
            lineRenderer.SetPosition(i, center + Quaternion.Euler(0.0f, 0.0f, 360 * i / erpDensity) * Vector3.up * radius);
        }
        return lineObj;
    }

    public static IEnumerable<GameObject> OverlapGameObject(GameObject gameObject, Func<Collider2D, bool> predicate)
    {
        List<Collider2D> colliders = new();
        gameObject.GetComponent<Collider2D>().OverlapCollider(new ContactFilter2D().NoFilter(), colliders);
        return colliders.Where(predicate).Select(collider => collider.gameObject);
    }

    public static IEnumerable<T> BFTraversal<T>(T root, System.Func<T, IEnumerable<T>> GetAdjacent)
    {
        Queue<T> queue = new();
        queue.Enqueue(root);
        HashSet<T> visited = new() { root };
        while (queue.Count > 0)
        {
            T current = queue.Dequeue();
            foreach (T t in GetAdjacent(current))
            {
                if (visited.Contains(t)) continue;
                visited.Add(t);
                queue.Enqueue(t);
            }
        }
        return visited;
    }
}
