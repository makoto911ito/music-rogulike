using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoteSceneScriot : MonoBehaviour
{
    public void NoteSceneLoad()
    {
        SceneManager.LoadScene("NoteScene", LoadSceneMode.Additive);
    }
}
