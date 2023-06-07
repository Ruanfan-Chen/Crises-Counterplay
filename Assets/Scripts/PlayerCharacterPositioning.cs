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
    private Quaternion rotationBias = Quaternion.Euler(0, 0, 0);
    private Vector3 defaultPositionL;
    private Vector3 defaultPositionU;
    private Vector3 defaultPositionR;
    private Quaternion defaultRotationL = Quaternion.Euler(0, 0, -120);
    private Quaternion defaultRotationU = Quaternion.Euler(0, 0, 0);
    private Quaternion defaultRotationR = Quaternion.Euler(0, 0, 120);
    // Start is called before the first frame update
    void Start()
    {
        defaultPositionL = defaultRotationL * Vector3.up * triRadius;
        defaultPositionU = defaultRotationU * Vector3.up * triRadius;
        defaultPositionR = defaultRotationR * Vector3.up * triRadius;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 activeCharacterPositionSum = Vector3.zero;
        int activeCharacterCount = 0;
        if (characterL.activeSelf)
        {
            activeCharacterPositionSum += defaultPositionL;
            activeCharacterCount++;
        }
        if (characterU.activeSelf)
        {
            activeCharacterPositionSum += defaultPositionU;
            activeCharacterCount++;
        }
        if (characterR.activeSelf)
        {
            activeCharacterPositionSum += defaultPositionR;
            activeCharacterCount++;
        }
        if (activeCharacterCount > 0)
            positionBias = -activeCharacterPositionSum / activeCharacterCount;
        else
            positionBias = Vector3.zero;
        characterL.transform.position = defaultPositionL + positionBias;
        characterL.transform.rotation = defaultRotationL * rotationBias;
        characterU.transform.position = defaultPositionU + positionBias;
        characterU.transform.rotation = defaultRotationU * rotationBias;
        characterR.transform.position = defaultPositionR + positionBias;
        characterR.transform.rotation = defaultRotationR * rotationBias;
    }
}
