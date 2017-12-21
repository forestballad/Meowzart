using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Screen.SetResolution(640 / 2, 1136 / 2, false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }
}
