using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public enum AbnormalStatus
    {
        둔화
    };

    // for test 1,2 : 재화 획득 / 7,8 : 체력 변동

    public static GameManager instance;
    // GameManager.instance.~

    public GameObject Player;

    // 깨달음의 조각
    public float pieceOfEnlightenment;

    // 지식의 불꽃
    public float sparkOfKnowledge;


    public Image hpBar;

    public Text pieceOfEnlightenmentText;
    public Text sparkOfKnowledgeText;

    public GameObject warrantOfSpentaPanel;
    public GameObject warrantOfAmeshaPanel;

    

    // 상태이상 유지 시간
    public float[] abnormalTime; // 0 : Slow

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;    // 인스턴스에 나 할당

            DontDestroyOnLoad(gameObject);
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
    }

    void Update()
    {
        Test();
       
        CntlAbnormal();

        if (Input.GetKeyDown(KeyCode.F4))
        {
            Debug.Log(Player.transform.position);
        }

        else if (Input.GetKeyDown(KeyCode.F11))
        {
            GetComponent<SaveLoadMng>().JsonSave();
        }
        else if (Input.GetKeyDown(KeyCode.F12))
        {
            GetComponent<SaveLoadMng>().JsonLoad();
        }
        else if (Input.GetKeyDown(KeyCode.F10))
        {
            GetComponent<SaveLoadMng>().Reset();
        }
        else if (Input.GetKeyDown(KeyCode.F9))
        {
            Vector3 nowLoc = GameManager.instance.Player.transform.position;

            Debug.Log(nowLoc.x);
            Debug.Log(nowLoc.y);
        }
    }

    public void GetItem()
    {
        pieceOfEnlightenmentText.text = pieceOfEnlightenment.ToString();
        sparkOfKnowledgeText.text = sparkOfKnowledge.ToString();
    }

    public void ChangeHp()
    {
        hpBar.fillAmount = Player.GetComponent<PlayerStatus>().hp / Player.GetComponent<PlayerStatus>().maxHp;
    }

    public void Test()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))  
        {
            pieceOfEnlightenment++;
            GetItem();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            sparkOfKnowledge++;
            GetItem();
        }
       
    }

    
    public void NextStage()
    {
        GetComponent<SceneLoad>().LoadScene();
    }

    public void ChangeSparkOfKnowledge(int num)
    {
        sparkOfKnowledge += num;
        GetItem();
    }

    public void ChangePieceOfEnlightenment(int num)
    {
        pieceOfEnlightenment += num;
        GetItem();
    }

    public void GetAbnormal(AbnormalStatus abnormalStatus, float holdingTime)
    {
        switch (abnormalStatus)
        {
            case AbnormalStatus.둔화:
                if (abnormalTime[0] == 0)
                {
                    Player.GetComponent<PlayerMove>().speed -= 3;
                    abnormalTime[0] = holdingTime;
                }
                else
                {
                    abnormalTime[0] = holdingTime;
                }
                break;
        }
        
    }

    public void CntlAbnormal()
    {
        for (int i = 0; i < abnormalTime.Length; i++)
        {
            if (abnormalTime[i] > 0)
            {
                abnormalTime[i] -= Time.deltaTime;
                if (abnormalTime[0] <= 0)
                {
                    abnormalTime[0] = 0;
                    Player.GetComponent<PlayerMove>().speed += 3;
                }
            }
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); 
#endif
    }
}
