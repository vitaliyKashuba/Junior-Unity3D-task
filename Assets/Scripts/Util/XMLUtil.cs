using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class XMLUtil
{
    private const String FILENAME = "Highscores.xml";
    static Type[] extraTypes = { typeof(Scoresheet.Node) };
    static XmlSerializer formatter = new XmlSerializer(typeof(Scoresheet), extraTypes);

    public static void writeData(Scoresheet scoresheet) //TODO: replace with serialization?
    {
        //Type[] extraTypes = { typeof(Scoresheet.Node) };
        //XmlSerializer formatter = new XmlSerializer(typeof(Scoresheet), extraTypes);
        using (FileStream fs = new FileStream(FILENAME, FileMode.OpenOrCreate))
        {
            formatter.Serialize(fs, scoresheet);
        }
    }

    public static Scoresheet readData()
    {
        Scoresheet scoresheet;
        Debug.Log("read data");
        using (FileStream fs = new FileStream(FILENAME, FileMode.OpenOrCreate))
        {
            scoresheet = (Scoresheet)formatter.Deserialize(fs);
            Debug.Log(scoresheet.ToString());
        }
        return scoresheet;
    }
}
