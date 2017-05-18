using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Policy;
using UnityEditorInternal;
using UnityEngine;
using Debug = UnityEngine.Debug;

/// <summary>
/// used to keep player name and stats between scenes
/// </summary>
public static class Static
{
    private const String DEFAULT_NAME = "Player1";
    private static String playerName;// { get; set; } TODO: find out why error here
    private static Scoresheet scoresheet;

    public static void setName(String name)
    {
        playerName = name;
    }

    public static String getName()
    {
        return playerName ?? DEFAULT_NAME; // nlv(playerName, DEFAULT_NAME)
    }

    public static Scoresheet getScoresheet()
    {
        return scoresheet;
    }

    public static void setScoresheet(Scoresheet sheet)
    {
        scoresheet = sheet;
    }

    public static void appendResult(Scoresheet.Node node)
    {
        scoresheet.addGame(node);
        try
        {
            XMLUtil.writeData(scoresheet);
        }
        catch (IOException e)
        {
            Debug.Log("Error during saving results, resuls will not be saved");
            Debug.Log(e.Message);
        }
    }
}
