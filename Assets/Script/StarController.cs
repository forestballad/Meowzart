using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarController : MonoBehaviour {
    public Sprite StarDim;
    public int DimTime;
    bool Dim;

	// Use this for initialization
	void Start () {
        Dim = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!Dim && GameObject.Find("GameLogic").GetComponent<AudioSource>().time > DimTime)
        {
            Dim = true;
            GetComponent<Image>().sprite = StarDim;
        }
	}
}
