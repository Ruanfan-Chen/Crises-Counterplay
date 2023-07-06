using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellCooldown : MonoBehaviour
{
    [Header("UI items for Spell Cooldown")]
    [SerializeField] private Image imageCooldown;
    [SerializeField] private TMP_Text textCooldown;
    [SerializeField] private Image imageEdge;

    [Space(10)]
    [Header("Player and Character")]
    [SerializeField] private ActiveItem m_activeItem;

    //variable for looking after the cooldown
    private bool isCoolDown = false;
    private float cooldownTime;
    private float cooldownTimer = 0.0f;

    public void SetActiveItem(ActiveItem activeItem) { m_activeItem = activeItem; }
    // Start is called before the first frame update
    void Start()
    {
        textCooldown.gameObject.SetActive(false);
        imageEdge.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0.0f;

        cooldownTime = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        float progress = m_activeItem.GetChargeProgress();
        if (m_activeItem.IsUsable())
        {
            imageCooldown.fillAmount = 0.0f;
        }
        else
        {
            if(progress == 1.0f)
            {
                imageCooldown.fillAmount = 1.0f;
            }
            else
            {
                imageCooldown.fillAmount = 1.0f - progress;
            }
        }

        if(imageCooldown.fillAmount == 0.0f)
        {
            textCooldown.gameObject.SetActive(false);
            imageEdge.gameObject.SetActive(false);
        }
        else
        {
            textCooldown.gameObject.SetActive(true);
            imageEdge.gameObject.SetActive(true);
            textCooldown.text = Mathf.Round(progress * 100.0f).ToString() + "%";
            imageEdge.transform.localEulerAngles = new Vector3(0, 0, 360.0f * (1.0f - progress));
        }

        //if (Input.GetKeyDown(KeyCode.K) && m_activeItem.IsUsable())
        //{
        //    UseSpell();
        //}

        //if (isCoolDown)
        //{
        //    ApplyCooldown();
        //}

        /*        if(m_activeItem.GetChargeProgress() < 1.0f)
                {
                    UseSpell();
                }
                if (isCoolDown)
                {
                    ApplyCooldown();
                }
                if (!m_activeItem.IsUsable() && m_activeItem.GetChargeProgress() == 1.0f)
                {
                    imageCooldown.fillAmount = 1.0f;
                    textCooldown.gameObject.SetActive(false);
                    imageEdge.gameObject.SetActive(false);
                }*/

    }

    void Ready()
    {

    }

    void InProgress()
    {

    }

    void NotReady()
    {

    }

    void ApplyCooldown()
    {
        float progress = m_activeItem.GetChargeProgress();
        if (progress >= 1.0f)
        {
            isCoolDown = false;
            textCooldown.gameObject.SetActive(false);
            imageEdge.gameObject.SetActive(false);
            imageCooldown.fillAmount = 0.0f;
        }
        else
        {
            textCooldown.text = Mathf.Round(progress * 100.0f).ToString() + "%";
            imageCooldown.fillAmount = 1.0f - progress;

            imageEdge.transform.localEulerAngles = new Vector3(0, 0, 360.0f * (1.0f - progress));
        }

    }

    public bool UseSpell()
    {
        if(isCoolDown)
        {
            return false;
        }
        else
        {
            isCoolDown = true;
            textCooldown.gameObject.SetActive(true);
            textCooldown.text = Mathf.Round(0.0f).ToString() + "%";
            imageCooldown.fillAmount = 1.0f;

            imageEdge.gameObject.SetActive(true);
            return true; 
        }
    }
}
