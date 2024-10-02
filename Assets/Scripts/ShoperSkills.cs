using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoperSkills : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    [System.Serializable]
    public class Skill
    {
        public string skillName;
        public int skillPrice;
        public bool isUnloked;
    }

    public Skill[] skill; //массив наших навыков

    private bool isPlayerInRange = false;

    [SerializeField] private GameObject buttonOnSkills;
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject panelSkills;

    private void Start()
    {
        buttonOnSkills.SetActive(false);
        text.SetActive(false);
        panelSkills.SetActive(false);

    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKey(KeyCode.F))
        {
            OpenSkillTree();
        }

        if (!isPlayerInRange && Input.GetKey(KeyCode.F))
        {
            panelSkills.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            buttonOnSkills.SetActive(true);
            text.SetActive(true);
            

            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            buttonOnSkills.SetActive(false);
            text.SetActive(false);
            panelSkills.SetActive(false);

            isPlayerInRange = false;
        }
    }
    
    private void OpenSkillTree()
    {
        if(isPlayerInRange)
        {
            panelSkills.SetActive(true);
        }
    }



}
