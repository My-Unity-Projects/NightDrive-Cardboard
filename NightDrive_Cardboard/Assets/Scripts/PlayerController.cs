using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player motorcycle reference
    public Transform motorcycle;

    // Player animator reference
    private Animator _anim;

    // Player state variables
    private bool isDead = false;

    // Velocity variable
    private float velocity = 20.0f;

    // Player current score
    private float score = 0.0f;

    // References to terrain and UI manager
    private TerrainManager terrain_manager;
    private UIManager ui_manager;

    void Start()
    {
        // Init animator reference
        _anim = GetComponent<Animator>();

        // Init terrain and UI manager references
        terrain_manager = FindObjectOfType<TerrainManager>();
        ui_manager = FindObjectOfType<UIManager>();
    }

    void Update()
    {
        if (motorcycle && !isDead)
        {
            // Compute wheel rotation in degrees
            float wheel_z_rot = (360 - motorcycle.localRotation.eulerAngles.z) * (Mathf.PI / 180.0f);

            // Compute axis motorbike speed
            float x_speed = velocity * Mathf.Sin(wheel_z_rot) * Time.deltaTime;
            float z_speed = velocity * Mathf.Cos(wheel_z_rot) * Time.deltaTime;

            // Update X position of the motorbike
            transform.position += new Vector3(x_speed, 0, 0);

            // Clamp motorbike position so it doesn't gets out of the road
            if (transform.position.x < -3.5f)
                transform.position = new Vector3(-3.5f, transform.position.y, transform.position.z);
            else if (transform.position.x > 3.5f)
                transform.position = new Vector3(3.5f, transform.position.y, transform.position.z);

            // Update current player score
            score += z_speed;

            // Update player score in the UI
            if (ui_manager)
                ui_manager.UpdateScore((int) score);

            // Set terrain speed to computed z_speed
            if (terrain_manager)
                terrain_manager.SetTerrainSpeed(z_speed);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            // Play screen shake animation
            if (_anim)
                _anim.SetBool("shake_screen", true);

            // Decrease obstacle sound volume
            other.GetComponent<AudioSource>().volume = 0.2f;

            // Set motorcycle as dead
            isDead = true;

            // Disable obstacle
            other.gameObject.SetActive(false);

            // Stop terrain movement
            if (terrain_manager)
                terrain_manager.SetTerrainSpeed(0);

            // Display dead menu
            if (ui_manager)
                ui_manager.DisplayDeadUI(transform.position);
        }
    }
}
