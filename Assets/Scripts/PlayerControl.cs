using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject gameplayManager;
    private float offset = 2.0f;
    private float speed = 5.0f;
    private float dashSpeed = 50.0f;
    private float dashDistance = 10.0f;
    private float horizontalInput;
    private float verticalInput;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 displacement = speed * Time.deltaTime * new Vector3(horizontalInput, verticalInput).normalized;
        Vector3 newPos = transform.position + displacement;
        transform.position = gameplayManager.GetComponent<MapManager>().PosInMap(newPos, offset);
        if (Input.GetKeyDown(KeyCode.J))
            GetComponent<PlayerCharacterPositioning>().GetClosestCharacter(1).GetComponent<CharacterInventory>().ActivateItem();
        if (Input.GetKeyDown(KeyCode.K))
            GetComponent<PlayerCharacterPositioning>().GetClosestCharacter(0).GetComponent<CharacterInventory>().ActivateItem();
        if (Input.GetKeyDown(KeyCode.L))
            GetComponent<PlayerCharacterPositioning>().GetClosestCharacter(-1).GetComponent<CharacterInventory>().ActivateItem();
    }

    public IEnumerator Dash()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        float displacement = 0;
        while (displacement + dashSpeed * Time.deltaTime <= dashDistance)
        {
            transform.position += dashSpeed * Time.deltaTime * new Vector3(horizontalInput, verticalInput).normalized;
            displacement += dashSpeed * Time.deltaTime;
            yield return null;
        }
        transform.position += (dashDistance - displacement) * new Vector3(horizontalInput, verticalInput).normalized;
    }
}
