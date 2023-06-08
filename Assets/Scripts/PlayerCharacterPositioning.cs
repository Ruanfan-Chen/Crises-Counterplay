using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterPositioning : MonoBehaviour
{
    public GameObject characterL;
    public GameObject characterU;
    public GameObject characterR;
    private float triRadius = 1.0f;
    private Vector3 positionBias = Vector3.zero;
    Quaternion rotationBias = Quaternion.Euler(0, 0, 0);
    private Vector3 basePositionL;
    private Vector3 basePositionU;
    private Vector3 basePositionR;
    private Quaternion baseRotationL = Quaternion.Euler(0, 0, -120);
    private Quaternion baseRotationU = Quaternion.Euler(0, 0, 0);
    private Quaternion baseRotationR = Quaternion.Euler(0, 0, 120);
    // Start is called before the first frame update
    void Start()
    {
        basePositionL = baseRotationL * Vector3.up * triRadius;
        basePositionU = baseRotationU * Vector3.up * triRadius;
        basePositionR = baseRotationR * Vector3.up * triRadius;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 activeCharacterPositionSum = Vector3.zero;
        int activeCharacterCount = 0;
        if (characterL.activeSelf)
        {
            activeCharacterPositionSum += basePositionL;
            activeCharacterCount++;
        }
        if (characterU.activeSelf)
        {
            activeCharacterPositionSum += basePositionU;
            activeCharacterCount++;
        }
        if (characterR.activeSelf)
        {
            activeCharacterPositionSum += basePositionR;
            activeCharacterCount++;
        }
        if (activeCharacterCount > 0)
            positionBias = -activeCharacterPositionSum / activeCharacterCount;
        else
            positionBias = Vector3.zero;
        characterL.transform.position = basePositionL + positionBias;
        characterL.transform.rotation = baseRotationL * rotationBias;
        characterU.transform.position = basePositionU + positionBias;
        characterU.transform.rotation = baseRotationU * rotationBias;
        characterR.transform.position = basePositionR + positionBias;
        characterR.transform.rotation = baseRotationR * rotationBias;
    }

    public GameObject GetClosestCharacter(Quaternion rotation)
    {
        float angleL = Quaternion.Angle(characterL.transform.rotation, rotation);
        float angleU = Quaternion.Angle(characterU.transform.rotation, rotation);
        float angleR = Quaternion.Angle(characterR.transform.rotation, rotation);
        float minAngle = Mathf.Min(angleL, angleU, angleR);
        if (minAngle == angleL) return characterL;
        if (minAngle == angleU) return characterU;
        if (minAngle == angleR) return characterR;
        return null;
    }

    public GameObject GetClosestCharacter(float bias)
    {
        return GetClosestCharacter(Quaternion.Euler(0, 0, 120 * bias));
    }
}
