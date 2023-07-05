using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    private GameObject NNeighbour;
    private GameObject SNeighbour;
    private GameObject NENeighbour;
    private GameObject NWNeighbour;
    private GameObject SENeighbour;
    private GameObject SWNeighbour;

    public GameObject GetNNeighbour() { return GetNNeighbour(); }

    public void SetNNeighbour(GameObject value) { SetNNeighbour(value); }

    public GameObject GetSNeighbour() { return GetSNeighbour(); }

    public void SetSNeighbour(GameObject value) { SetSNeighbour(value); }

    public GameObject GetNENeighbour() { return GetNENeighbour(); }

    public void SetNENeighbour(GameObject value) { SetNENeighbour(value); }

    public GameObject GetNWNeighbour() { return GetNWNeighbour(); }

    public void SetNWNeighbour(GameObject value) { SetNWNeighbour(value); }

    public GameObject GetSENeighbour() { return GetSENeighbour(); }

    public void SetSENeighbour(GameObject value) { SetSENeighbour(value); }

    public GameObject GetSWNeighbour() { return GetSWNeighbour(); }

    public void SetSWNeighbour(GameObject value) { SetSWNeighbour(value); }

    public IEnumerable<GameObject> GetNeighbour()
    {
        return new HashSet<GameObject>() { NNeighbour, SNeighbour, NENeighbour, NWNeighbour, SENeighbour, SWNeighbour }.Where(gameObject => gameObject);
    }

    //public GameObject BFS()
    //{
    //    Queue<GameObject> queue = new();
    //    queue.Enqueue(gameObject);
    //    HashSet<GameObject> explored = new() { gameObject };
    //    while (queue.Count > 0)
    //    {
    //        GameObject current = queue.Dequeue();
    //        if (false)
    //            return current;
    //        foreach (GameObject neighbour in GetNeighbour())
    //        {
    //            if (!explored.Contains(neighbour))
    //            {
    //                explored.Add(neighbour);
    //                queue.Enqueue(neighbour);
    //            }
    //        }
    //    }
    //    return null;
    //}
}
