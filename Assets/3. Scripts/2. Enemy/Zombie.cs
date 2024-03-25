using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    Animator anim;

    // 0 : ���� ��� / 1 : ��ų ���
    public float[] dmg;
    public float attackPower;

    public AnimatorStateInfo animState;

    public bool isPlayer;

    public float attackCoolTime;
    public float attackCurTime;

    public float skillCoolTime;
    public float skillCurTime;

    public int moveDir;
    public float moveSpeed;

    Rigidbody2D rigid;
    public bool isDead;

    public float startX, startY, lenX, lenY;

    public float distance;
    void Start()
    {
        rigid = GetComponentInParent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        SelectDir();
    }

    private void OnDrawGizmos()
    {
        // ���� ��Ÿ� 9
        // ��ų ��Ÿ� 24
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(new Vector3(startX * transform.parent.localScale.x, startY, 0) + transform.position, new Vector3(lenX, lenY, 0));
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

        skillCurTime -= Time.deltaTime;
        attackCurTime -= Time.deltaTime;

        animState = anim.GetCurrentAnimatorStateInfo(0);

        distance = Mathf.Abs(GameManager.instance.Player.transform.position.x - transform.position.x);

        Move();
        CheckPlayer();

        if (isPlayer && animState.IsName("idle"))
        {
            if (skillCurTime < 0 && distance < 23)
            {
                Skill();
                skillCurTime = skillCoolTime;
            }

            else if (attackCurTime < 0 && distance < 9f)
            {
                Attack();
                attackCurTime = attackCoolTime;
            }
        }
    }


    public void Dead()
    {
        anim.SetTrigger("dead");
        rigid.velocity = Vector3.zero;
        transform.parent.GetChild(1).GetChild(0).gameObject.SetActive(false);
        isDead = true;
    }
    public void CheckPlayer()
    {
        if (isPlayer)
        {
            return;
        }

        Collider2D[] hits = Physics2D.OverlapAreaAll(new Vector2(transform.position.x - 20f, transform.position.y - 12f), new Vector2(transform.position.x + 20f, transform.position.y));

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.layer == 3)
            {
                isPlayer = true;
                skillCurTime = skillCoolTime;
                CancelInvoke();
            }
        }
    }

    public void Move()
    {
        if (!animState.IsName("idle"))
        {
            rigid.velocity = Vector2.zero;
        }

        else if (!isPlayer)
        {
            rigid.velocity = new Vector2(moveDir * moveSpeed, rigid.velocity.y);
        }

        else if (distance > 9f)
        {
            if (GameManager.instance.Player.transform.position.x < transform.position.x)
            {
                transform.parent.localScale = new Vector2(Mathf.Abs(transform.parent.localScale.x) * -1, transform.parent.localScale.y);
                rigid.velocity = new Vector2(-1 * moveSpeed, rigid.velocity.y);
            }
            else
            {
                transform.parent.localScale = new Vector2(Mathf.Abs(transform.parent.localScale.x), transform.parent.localScale.y);
                rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);
            }
        }
        else
        {
            rigid.velocity = Vector2.zero;
        }
    }

    public void SelectDir()
    {
        if (moveDir != 0)
        {
            moveDir = 0;
        }
        else
        {
            int randInt = Random.Range(0, 2);
            if (randInt == 0)
            {
                moveDir--;

            }
            else
            {
                moveDir++;
            }
            transform.parent.localScale = new Vector2(Mathf.Abs(transform.parent.localScale.x) * moveDir, transform.parent.localScale.y);
        }

        Invoke("SelectDir", 2f);
    }
    public void Skill()
    {
        if (GameManager.instance.Player.transform.position.x < transform.position.x)
        {
            transform.parent.localScale = new Vector2(Mathf.Abs(transform.parent.localScale.x) * -1, transform.parent.localScale.y);
        }
        else
        {
            transform.parent.localScale = new Vector2(Mathf.Abs(transform.parent.localScale.x), transform.parent.localScale.y);
        }
        anim.SetTrigger("skill");
    }
    public void SkillDmg()
    {
        Vector2 v2 = new Vector2(transform.position.x + transform.parent.localScale.x * 13, transform.position.y + 3f);
        Collider2D[] hits = Physics2D.OverlapAreaAll(v2, new Vector2(24, 7));

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.tag == "Player")
            {
                hit.GetComponent<PlayerStatus>().Damaged(false, attackPower, dmg[1]);
            }
        }
    }
    public void Attack()
    {
        if (GameManager.instance.Player.transform.position.x < transform.position.x)
        {
            transform.parent.localScale = new Vector2(Mathf.Abs(transform.parent.localScale.x) * -1, transform.parent.localScale.y);
        }
        else
        {
            transform.parent.localScale = new Vector2(Mathf.Abs(transform.parent.localScale.x), transform.parent.localScale.y);
        }
        anim.SetTrigger("attack");
    }

    public void AttackDmg()
    {
        Vector2 v2 = new Vector2(6 * transform.parent.localScale.x + transform.position.x, transform.position.y + 5);
        Collider2D[] hits = Physics2D.OverlapBoxAll(v2, new Vector2(6, 12), 0);

        foreach (Collider2D hit in hits)
        {
            if (hit.tag == "Player")
            {
                hit.GetComponent<PlayerStatus>().Damaged(false, attackPower, dmg[0]);
            }
        }
    }
    public int GetNowAnim()
    {
        if (animState.IsName("attack"))
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
}