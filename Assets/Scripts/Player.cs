using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private static string characterPrefabPath = "Prefabs/Character";
    private GameObject character;

    public GameObject GetCharacter() { return character; }

    public void SetCharacter(GameObject value) { character = value; }

    void Start()
    {
        GetComponent<ConstraintInsideOfMap>().SetOffset(1.5f);
        character = Instantiate(Resources.Load<GameObject>(characterPrefabPath), gameObject.transform);
        character.GetComponent<SpriteRenderer>().color = Color.red;
    }
    // Update is called once per frame
    void Update()
    {
        // Move Player
        transform.Translate(Character.GetMoveSpeed() * Time.deltaTime * new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized);

        if (character != null)
        {
            transform.position += character.transform.localPosition;
            transform.rotation *= character.transform.localRotation;
            character.transform.localPosition = Vector3.zero;
            character.transform.localRotation = Quaternion.identity;
        }
    }
}
