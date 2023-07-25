using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteWhenAnimDone : MonoBehaviour
{
    Animator m_anim;
    // Start is called before the first frame update
    void Start()
    {
        m_anim = GetComponent<Animator>();
        Destroy(gameObject, m_anim.GetCurrentAnimatorStateInfo(0).length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
