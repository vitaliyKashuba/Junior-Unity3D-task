using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    string playerName = Static.getName();
    public const int BUTTON_LENGTH = 100;
    public const int BUTTON_HEIGTH = 25;
    readonly int B_POSITION_X = Screen.width / 2;
    readonly int B_POSITION_Y = Screen.height / 2;
    public Vector2 scrollPosition = Vector2.zero;

    MainMenu()
    {
        Static.setScoresheet(XMLUtil.readData());
    }

    void OnGUI()    //TODO: add better GUI
    {
        if (GUI.Button(new Rect(B_POSITION_X, B_POSITION_Y - 50, BUTTON_LENGTH, BUTTON_HEIGTH), "New Game"))
        {
            Static.setName(playerName);
            SceneManager.LoadScene("maze");
        }

        playerName = GUI.TextField(new Rect(B_POSITION_X, B_POSITION_Y, BUTTON_LENGTH, BUTTON_HEIGTH), playerName);


        scrollPosition = GUI.BeginScrollView(new Rect(B_POSITION_X/2, B_POSITION_Y + 50, 500, 200), scrollPosition, new Rect(0, 0, 480, Static.getScoresheet().nodes.Count*20));
        int p = 0;
        foreach (Scoresheet.Node node in Static.getScoresheet().nodes)
        {
            GUI.TextField(new Rect(0, p, 480, 20), node.ToString());
            p = p + 20;
        }
        GUI.EndScrollView();

    }
}
