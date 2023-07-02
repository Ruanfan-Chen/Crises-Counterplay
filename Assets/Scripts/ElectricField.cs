using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ElectricField : MonoBehaviour
{
    private static string prefabPath = "Prefabs/ElectricField";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static GameObject Instantiate(Vector3 position) {
        GameObject electricField = Instantiate(Resources.Load<GameObject>(prefabPath), position, Quaternion.identity);
        electricField.AddComponent<ElectricField>();
        return electricField;
    }
}
