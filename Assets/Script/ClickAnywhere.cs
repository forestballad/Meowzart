using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickAnywhere : MonoBehaviour {
    bool IsIncreasing;

	// Use this for initialization
	void Start () {
        IsIncreasing = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (IsIncreasing)
        {
            GetComponent<Image>().color = new Vector4(1, 1, 1, GetComponent<Image>().color.a + 0.025f);
        }
        else
        {
            GetComponent<Image>().color = new Vector4(1, 1, 1, GetComponent<Image>().color.a - 0.025f);
        }
        if (GetComponent<Image>().color.a >= 1)
        {
            IsIncreasing = false;
        }
        else if (GetComponent<Image>().color.a <= 0)
        {
            IsIncreasing = true;
        }
    }
}
