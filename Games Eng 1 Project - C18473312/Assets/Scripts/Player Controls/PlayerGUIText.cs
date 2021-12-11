using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


public class PlayerGUIText : MonoBehaviour {
    
    public static PlayerGUIText instance; // Singleton instance
    private static StringBuilder message = new StringBuilder(); // Stringbuilder to keep output in.

    // Used to style the gui
    private GUIStyle guiStyle = new GUIStyle();

    // Appends a string to GUI
    public static void AddString(string text)
    {
        message.Append(text + "\n");
    }

    // ===================================== LifeCycle methods =============================
    public void OnGUI() {

        // Label styling.
        guiStyle.fontSize = 70; 
        guiStyle.normal.textColor = Color.white;

        // Creates a new label, full screen size, stringbuilder string and fontsize passed in.
        GUI.Label(new Rect(20, 20, Screen.width, Screen.height), "" + message, guiStyle);


        // GUI clears itself after display
        if (Event.current.type == EventType.Repaint) {
            message.Length = 0;
        }
    }
    // Singleton pattern
    void Awake() {
        if(instance == null){
            instance = this;
        }
        else if(instance != this){
            Debug.Log("Sea manager instance already running, destroying duplicate instance.");
            Destroy(this);
        }
    }




}
