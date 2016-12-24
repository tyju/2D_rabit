using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    // Reset
    if (Input.GetKeyDown(KeyCode.A)) {
      Reset();
    }
  }

  public void Reset() {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
}
