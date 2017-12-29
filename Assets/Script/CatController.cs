using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour {
    public int m_CatType;
    public int m_CatLocNum;
    public bool m_BeenTouched;
    GameObject m_LocationMarker;


    public AudioClip CorrectSound;
    public AudioClip WrongSound;

    public List<GameObject> HandAnimPrefab;

	// Use this for initialization
	void Start () {
        m_BeenTouched = false;
	}
	
	// Update is called once per frame
	void Update () {
        Animator m_Animator;
        string m_ClipName;
        AnimatorClipInfo[] m_CurrentClipInfo;
        m_Animator = gameObject.GetComponent<Animator>();
        //Fetch the current Animation clip information for the base layer
        m_CurrentClipInfo = m_Animator.GetCurrentAnimatorClipInfo(0);
        //Access the current length of the clip
        //Access the Animation clip name
        m_ClipName = m_CurrentClipInfo[0].clip.name;
        if (m_BeenTouched && (m_ClipName != "Cat_M_Idle" && m_ClipName != "Cat_O_Idle") && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            GameObject.Find("GameLogic").GetComponent<LevelController>().CatTouch(m_CatType);
            DestroyCat();
        }
    }

    public void InitWithData(GameObject AssignedLoc, int LocNum, float LifeSpan)
    {
        m_LocationMarker = AssignedLoc;
        m_CatLocNum = LocNum;
        transform.position = m_LocationMarker.GetComponent<CatLocData>().transform.position;
        transform.localScale = new Vector3(m_LocationMarker.GetComponent<CatLocData>().Scale, m_LocationMarker.GetComponent<CatLocData>().Scale, 1);
        GetComponent<SpriteRenderer>().sortingOrder = m_LocationMarker.GetComponent<CatLocData>().Layer;
        Invoke("DestroyAsMiss", LifeSpan);
    }

    void OnMouseDown()
    {
        if (m_BeenTouched)
        {
            return;
        }
        m_BeenTouched = true;
        GameObject hand = Instantiate(HandAnimPrefab[GameObject.Find("GameLogic").GetComponent<LevelController>().m_hand],this.transform);
        hand.transform.localPosition = new Vector2(0, 1.4f);
       
        if (m_CatType == GameObject.Find("GameLogic").GetComponent<LevelController>().m_hand)
        {
            GetComponent<Animator>().SetInteger("Touch", 0);
            GetComponent<AudioSource>().clip = CorrectSound;
            GetComponent<AudioSource>().Play();
        }
        else
        {
            GetComponent<Animator>().SetInteger("Touch", 1);
            GetComponent<AudioSource>().clip = WrongSound;
            GetComponent<AudioSource>().Play();
        }
    }

    void DestroyAsMiss()
    {
        if (m_BeenTouched)
        {
            return;
        }
        GameObject.Find("GameLogic").GetComponent<LevelController>().CatMiss(m_CatType);
        DestroyCat();
    }

    void DestroyCat()
    {
        GameObject.Find("GameLogic").GetComponent<LevelController>().ReleaseCatLoc(m_CatLocNum);
        Destroy(gameObject);
    }
}
