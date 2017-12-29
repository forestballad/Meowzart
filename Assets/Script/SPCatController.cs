using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPCatController : MonoBehaviour {
    public bool m_BeenTouched;
    GameObject m_LocationMarker;

    public AudioClip CorrectSound;
    public AudioClip WrongSound;

    public GameObject HandAnimPrefab;

    // Use this for initialization
    void Start()
    {
        m_BeenTouched = false;
    }

    // Update is called once per frame
    void Update()
    {
        Animator m_Animator;
        string m_ClipName;
        AnimatorClipInfo[] m_CurrentClipInfo;
        m_Animator = gameObject.GetComponent<Animator>();
        //Fetch the current Animation clip information for the base layer
        m_CurrentClipInfo = m_Animator.GetCurrentAnimatorClipInfo(0);
        //Access the current length of the clip
        //Access the Animation clip name
        m_ClipName = m_CurrentClipInfo[0].clip.name;
        if (m_BeenTouched && (m_ClipName != "Cat_D_Idle") && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            DestroyCat();
        }
    }

    public void InitWithData(GameObject AssignedLoc, int LocNum, float LifeSpan)
    {
        m_LocationMarker = AssignedLoc;
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
        GameObject.Find("GameLogic").GetComponent<LevelController>().SPCatTouch();
        GameObject hand = Instantiate(HandAnimPrefab, this.transform);
        hand.transform.localPosition = new Vector2(0, 1.4f);
        GetComponent<Animator>().SetBool("Touch", true);
        GetComponent<AudioSource>().clip = CorrectSound;
        GetComponent<AudioSource>().Play();
    }

    void DestroyAsMiss()
    {
        if (m_BeenTouched)
        {
            return;
        }
        DestroyCat();
    }

    void DestroyCat()
    {
        Destroy(gameObject);
    }
}
