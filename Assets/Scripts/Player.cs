using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject[] characters;
    private float moveSpeed = 5.0f;
    private float triRadius = 0.7f;
    private float angularVelocity = 180.0f;
    Quaternion rotationBias = Quaternion.Euler(0, 0, 0);

    public float GetMoveSpeed() { return moveSpeed; }

    public void SetMoveSpeed(float value) { moveSpeed = value; }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ConstraintInsideOfMap>().SetOffset(1.5f);
    }
    // Update is called once per frame
    void Update()
    {
        // Move Player
        transform.Translate(GetMoveSpeed() * Time.deltaTime * new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized);
        // Calculate Characters transform
        Vector3 positionBias = Vector3.zero;
        Vector3 activeCharacterPositionSum = Vector3.zero;
        int activeCharacterCount = 0;
        for (int i = 0; i < 3; i++)
        {
            if (characters[i].activeSelf)
            {
                activeCharacterPositionSum += GetBasePosition(i);
                activeCharacterCount++;
            }
        }
        if (activeCharacterCount > 0)
            positionBias = -activeCharacterPositionSum / activeCharacterCount;
        // Move Characters
        for (int i = 0; i < 3; i++)
        {
            characters[i].transform.position = transform.position + rotationBias * (GetBasePosition(i) + positionBias);
            characters[i].transform.rotation = rotationBias * GetBaseRotation(i);
        }
        if (Input.GetKeyDown(KeyCode.J))
            GetClosestCharacter(1).GetComponent<Character>().ActivateItem();
        if (Input.GetKeyDown(KeyCode.K))
            GetClosestCharacter(0).GetComponent<Character>().ActivateItem();
        if (Input.GetKeyDown(KeyCode.L))
            GetClosestCharacter(2).GetComponent<Character>().ActivateItem();
    }
    Quaternion GetBaseRotation(float i) { return Quaternion.Euler(0, 0, i * 120); }

    Vector3 GetBasePosition(float i) { return GetBaseRotation(i) * Vector3.up * triRadius; }

    public GameObject GetClosestCharacter(Quaternion rotation)
    {
        float[] angles = new float[3];
        angles[0] = Quaternion.Angle(characters[0].transform.rotation, rotation);
        angles[1] = Quaternion.Angle(characters[1].transform.rotation, rotation);
        angles[2] = Quaternion.Angle(characters[2].transform.rotation, rotation);
        float minAngle = Mathf.Min(angles);
        if (minAngle == angles[0]) return characters[0];
        if (minAngle == angles[1]) return characters[1];
        if (minAngle == angles[2]) return characters[2];
        return null;
    }

    public GameObject GetClosestCharacter(float bias)
    {
        return GetClosestCharacter(GetBaseRotation(bias));
    }
}
