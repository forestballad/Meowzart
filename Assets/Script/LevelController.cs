using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {
    // 0 for HC hand, 1 for AS hand
    public int m_hand;
    public List<Sprite> HandSelectionSprites;
    public List<Texture2D> HandCursorSprites;

    public GameObject OCatPrefab;
    public GameObject MCatPrefab;

    public List<GameObject> CatLocs;
    List<bool> CatLocsAvailability;

    public AudioSource BGMPlayer;

    public Sprite RankM;
    public Sprite RankA_O;
    public Sprite RankA_M;
    public Sprite RankB_O;
    public Sprite RankB_M;
    public Sprite RankC;
    public Sprite RankD;

    public GameObject EndGameDisplay;
    public GameObject EndGameCG;
    public GameObject EndGameCGArrowLeft;
    public GameObject EndGameCGArrowRight;

    List<Sprite> ActiveCGs;
    int CGIndex;

    float m_timer;
    bool m_endgameFlag;

    int m_spawnLevel;



    int MiCorrect;
    int MiMiss;
    int MiWrong;
    int OeCorrect;
    int OeMiss;
    int OeWrong;

	// Use this for initialization
	void Start () {

        InitGameData();

        RefreshScore();
        InvokeRepeating("SpawnCat", 0, 2);
    }

    void InitGameData()
    {
        m_timer = 0;
        m_endgameFlag = false;

        ChangeHand(0);

        CatLocsAvailability = new List<bool>(new bool[10]);
        ActiveCGs = new List<Sprite>();
        for (int i = 0; i < 10; i++)
        {
            CatLocsAvailability[i] = true;
        }

        MiCorrect = 0;
        MiMiss = 0;
        MiWrong = 0;
        OeCorrect = 0;
        OeMiss = 0;
        OeWrong = 0;

        m_spawnLevel = 0;
    }

    // Update is called once per frame
    void Update () {
        m_timer += Time.deltaTime;

        if (m_spawnLevel == 0 && m_timer >= 10)
        {
            CancelInvoke();
            InvokeRepeating("SpawnCat", 0, 1.5f);
            m_spawnLevel = 1;
        }

        else if (m_spawnLevel == 1 && m_timer >= 20)
        {
            CancelInvoke();
            InvokeRepeating("SpawnCat", 0, 1f);
            m_spawnLevel = 2;
        }
        else if (m_spawnLevel == 2 && m_timer >= 30)
        {
            CancelInvoke();
            InvokeRepeating("SpawnCat", 0, 0.75f);
            m_spawnLevel = 3;
        }

        if (m_timer >= BGMPlayer.clip.length)
        {
            CancelInvoke();
        }
        else if (!m_endgameFlag && m_timer >= BGMPlayer.clip.length+2)
        {
            EndGame();
        }

        if (Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.C))
        {
            ChangeHand(0);
        }
        else if (Input.GetKey(KeyCode.A)|| Input.GetKeyDown(KeyCode.S))
        {
            ChangeHand(1);
        }
    }

    void RefreshScore()
    {
        GameObject.Find("OCat_CorrectText").GetComponent<Text>().text = "Correct: " + OeCorrect;
        GameObject.Find("OCat_MissText").GetComponent<Text>().text = "Miss: " + OeMiss;
        GameObject.Find("OCat_WrongText").GetComponent<Text>().text = "Wrong: " + OeWrong;
        GameObject.Find("MCat_CorrectText").GetComponent<Text>().text = "Correct: " + MiCorrect;
        GameObject.Find("MCat_MissText").GetComponent<Text>().text = "Miss: " + MiMiss;
        GameObject.Find("MCat_WrongText").GetComponent<Text>().text = "Wrong: " + MiWrong;
    }

    public void ChangeHand(int hand)
    {
        if (hand == -1)
        {
            m_hand = 1-m_hand;
        }
        else
        {
            m_hand = hand;
        }
        Cursor.SetCursor(HandCursorSprites[m_hand],new Vector2(14,2),CursorMode.ForceSoftware);
        GameObject.Find("HandSwitchButton").GetComponent<Image>().sprite = HandSelectionSprites[m_hand];
    }

    void SpawnCat()
    {
       SpawnSingleCat();
    }

    void SpawnSingleCat()
    {
        int catType = Random.Range(0, 2);
        GameObject newCat;
        if (catType == 0)
        {
            newCat = Instantiate(OCatPrefab);
            newCat.GetComponent<CatController>().m_CatType = 0;
        }
        else
        {
            newCat = Instantiate(MCatPrefab);
            newCat.GetComponent<CatController>().m_CatType = 1;
        }

        int NewCatLoc = Random.Range(0, 10);
        while (CatLocsAvailability[NewCatLoc] == false)
        {
            NewCatLoc = Random.Range(0, 10);
        }

        newCat.GetComponent<CatController>().InitWithData(CatLocs[NewCatLoc], NewCatLoc, 2f);
        CatLocsAvailability[NewCatLoc] = false;
        if (GameObject.Find("Main Camera").GetComponent<Camera>().WorldToViewportPoint(newCat.transform.position).x < 0.5)
        {
            newCat.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void CatTouch(int type)
    {
        if (type == 0 && m_hand == 0)
        {
            OeCorrect++;
        }
        else if (type == 1 && m_hand == 1)
        {
            MiCorrect++;
        }
        else if (type == 0 && m_hand == 1)
        {
            OeWrong++;
        }
        else if (type == 1 && m_hand == 0)
        {
            MiWrong++;
        }
        RefreshScore();
    }

    public void CatMiss(int type)
    {
        if (type == 0)
        {
            OeMiss++;
        }
        else
        {
            MiMiss++;
        }
    }

    public void ReleaseCatLoc(int LocNum)
    {
        CatLocsAvailability[LocNum] = true;
    }

    public void EndGame()
    {
        m_endgameFlag = true;
        CancelInvoke();

        BGMPlayer.Stop();

        GenerateActiveCGs();
        CGIndex = 0;


        EndGameCG.GetComponent<Image>().sprite = ActiveCGs[0];
        EndGameDisplay.SetActive(true);

        if (ActiveCGs.Count == 1)
        {
            EndGameCGArrowLeft.SetActive(false);
            EndGameCGArrowRight.SetActive(false);
        }
    }

    void GenerateActiveCGs()
    {
        if (MiMiss + OeMiss + MiWrong + OeWrong == 0)
        {
            ActiveCGs.Add(RankM);

        }
        else if (MiWrong >= (MiMiss + MiCorrect) && OeWrong >= (OeMiss + OeCorrect))
        {
            ActiveCGs.Add(RankC);
        }
        else if (MiCorrect / 3 >= (MiMiss + MiWrong) && OeCorrect / 3 >= (OeMiss + OeWrong))
        {
            ActiveCGs.Add(RankA_O);
            ActiveCGs.Add(RankA_M);
        }
        else if (MiCorrect / 3 < (MiMiss + MiWrong) && OeCorrect / 3 < (OeMiss + OeWrong))
        {
            ActiveCGs.Add(RankB_O);
            ActiveCGs.Add(RankB_M);
        }
        else if (MiCorrect / 3 >= (MiMiss + MiWrong) && OeCorrect / 3 < (OeMiss + OeWrong))
        {
            ActiveCGs.Add(RankA_M);
            ActiveCGs.Add(RankB_O);
        }
        else
        {
            ActiveCGs.Add(RankA_O);
            ActiveCGs.Add(RankB_M);
        }
    }

    public void SwitchCG()
    {
        CGIndex += 1;
        if (CGIndex == ActiveCGs.Count)
        {
            CGIndex = 0;
        }
        EndGameCG.GetComponent<Image>().sprite = ActiveCGs[CGIndex];
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene(0);
    }
}
