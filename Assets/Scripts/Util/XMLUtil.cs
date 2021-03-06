﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class XMLUtil
{
    private const String FILENAME = "Highscores.xml";
    private static Type[] extraTypes = { typeof(Scoresheet.Node) };
    private static XmlSerializer formatter = new XmlSerializer(typeof(Scoresheet), extraTypes);

    public static void writeData(Scoresheet scoresheet)
    {
        using (FileStream fs = new FileStream(FILENAME, FileMode.OpenOrCreate))
        {
            formatter.Serialize(fs, scoresheet);
        }
    }

    public static Scoresheet readData()
    {
        Scoresheet scoresheet;
        try
        {
            using (FileStream fs = new FileStream(FILENAME, FileMode.OpenOrCreate))
            {
                scoresheet = (Scoresheet) formatter.Deserialize(fs);
            }
        }
        catch (XmlException e)
        {
            scoresheet = new Scoresheet();
        }
        return scoresheet;
    }
}
