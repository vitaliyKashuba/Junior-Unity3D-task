using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    string playerName = Static.getName();
    const int BUTTON_LENGTH = 100;
    const int BUTTON_HEIGTH = 25;
    readonly int B_POSITION_X = Screen.width / 2;
    readonly int B_POSITION_Y = Screen.height / 2;

    MainMenu()
    {
        Static.setScoresheet(XMLUtil.readData());
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(B_POSITION_X, B_POSITION_Y - 50, BUTTON_LENGTH, BUTTON_HEIGTH), "New Game"))
        {
            Static.setName(playerName);
            SceneManager.LoadScene("maze");
        }

        playerName = GUI.TextField(new Rect(B_POSITION_X, B_POSITION_Y, BUTTON_LENGTH, BUTTON_HEIGTH), playerName);

        if (GUI.Button(new Rect(B_POSITION_X, B_POSITION_Y + 50, BUTTON_LENGTH, BUTTON_HEIGTH), "Scores"))
        {

        }

    }
}
