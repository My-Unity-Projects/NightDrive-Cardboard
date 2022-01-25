//Attach this script to your Canvas GameObject.
//Also attach a GraphicsRaycaster component to your canvas by clicking the Add Component button in the Inspector window.
//Also make sure you have an EventSystem in your hierarchy.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIRaycast : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    // UI interaction variables
    private Button target;
    private float sec_count = 0.0f;
    private float sec_threshold = 3.0f;

    void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
    }

    void Update()
    {
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position
        m_PointerEventData.position = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster
        m_Raycaster.Raycast(m_PointerEventData, results);


        //For every result returned, check if the player has raycasted for more than 5 seconds
        foreach (RaycastResult result in results)
        {
            Button button = result.gameObject.GetComponent<Button>();
            if (button)
            {
                if (!target)
                {
                    target = button;
                }
                else if (!target.Equals(button))
                {
                    target = button;
                    sec_count = 0;
                }
                else if (target.Equals(button))
                {
                    sec_count += Time.deltaTime;
                }

                break;
            }
        }

        // Execute action linked to the target button
        if (sec_count >= sec_threshold)
            target.onClick.Invoke();
    }
}
