using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // UI canvas references
    public Canvas default_canvas;
    public Canvas dead_canvas;

    // Dead canvas animator reference
    private Animator dead_canvas_anim;

    // Start is called before the first frame update
    void Start()
    {
        // Init UI canvas animator reference
        if (dead_canvas)
            dead_canvas_anim = dead_canvas.GetComponent<Animator>();
    }

    public void UpdateScore(int _new_score)
    {
        if (default_canvas)
        {
            Text score_text = default_canvas.GetComponentInChildren<Text>();
            if (score_text) { score_text.text = "Score: " + _new_score.ToString(); }
        }
    }

    public void DisplayDeadUI(Vector3 position)
    {
        if (dead_canvas)
        {
            // Translate dead canvas to spawning position
            RectTransform rect = dead_canvas.GetComponent<RectTransform>();
            if (rect) { rect.localPosition = new Vector3(position.x, 1, 1); }
        }

        // Trigger display dead canvas animation
        if (dead_canvas_anim)
        {
            dead_canvas_anim.SetBool("isDead", true);
        }
    }
    
    public void RestartScene()
    {
        // Reload scene
        SceneManager.LoadScene("Cardboard", LoadSceneMode.Single);
    }

    public void CloseGame()
    {
        // Close app
        Application.Quit();
    }
}
