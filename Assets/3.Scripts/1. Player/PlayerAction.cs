using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public enum SkillName
    {
        Bohuman, Cassatra, Asha, Armaity
    }


    public enum NonSkillName
    {
         Dash, Block, Attack
    }

    void Start()
    {
        
    }

    void Update()
    {
        SkillMng();
    }

    void SkillMng()
    {
        // �ٸ� �ൿ ���� ��� ó�� �ʿ�



        if (Input.GetKeyDown(KeyCode.E))
        {
            // ���ĸ� ���
            SKillUse(SkillName.Bohuman);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // ī��Ʈ�� ���
            SKillUse(SkillName.Cassatra);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            // �ƻ� ���
            SKillUse(SkillName.Asha);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            // �Ƹ�����Ƽ ���
            SKillUse(SkillName.Armaity);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            // ���� ���
            NonSkillUse(NonSkillName.Block);
        }

    }

    void SKillUse(SkillName skillName)
    {
        if (gameObject.GetComponent<PlayerStatus>().skillCanUse[(int)skillName])
        {
            gameObject.GetComponent<PlayerStatus>().Action(skillName);
            gameObject.GetComponentInChildren<PlayerSpine>().Action(skillName);
        }

    }
    void NonSkillUse(NonSkillName nonSkillName)
    {
        if (gameObject.GetComponent<PlayerStatus>().nonSkillCanUse[(int)nonSkillName])
        {
            gameObject.GetComponent<PlayerStatus>().Action(nonSkillName);
            gameObject.GetComponentInChildren<PlayerSpine>().Action(nonSkillName);
        }

    }
}