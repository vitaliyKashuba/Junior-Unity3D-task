using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    string name = "Player1";
    const int BUTTON_LENGTH = 100;
    const int BUTTON_HEIGTH = 25;
    readonly int B_POSITION_X = Screen.width / 2;
    readonly int B_POSITION_Y = Screen.height / 2;

    void OnGUI()
    {
        if (GUI.Button(new Rect(B_POSITION_X, B_POSITION_Y - 50, BUTTON_LENGTH, BUTTON_HEIGTH), "New Game"))
        {
            Static.setName(name);
            Application.LoadLevel(1);
        }

        name = GUI.TextField(new Rect(B_POSITION_X, B_POSITION_Y, BUTTON_LENGTH, BUTTON_HEIGTH), name);

        if (GUI.Button(new Rect(B_POSITION_X, B_POSITION_Y + 50, BUTTON_LENGTH, BUTTON_HEIGTH), "Scores"))
        {

        }

    }
}
