using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public static class SaveSystem
{
    [HideInInspector]
    public static bool hasData;

    public static void SaveDeck(Deck d)
    {
        hasData = true;

        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/savedDeck";

        //string path = Path.Combine(Application.persistentDataPath, "/savedDeck");

        Debug.Log(path);

        FileStream stream = new FileStream(path, FileMode.Create);

        DeckData deck = new DeckData(d);

        formatter.Serialize(stream, deck);

        stream.Close();
    }

    public static DeckData LoadDeck()
    {
        if (hasData || !hasData) //CHANGE
        {
            string path = Application.persistentDataPath + "/savedDeck";

            //string path = Path.Combine(Application.persistentDataPath, "/savedDeck");

            FileStream stream = new FileStream(path, FileMode.Open);

            if (File.Exists(path) && stream.Length > 0)
            {
                BinaryFormatter formatter = new BinaryFormatter();

                DeckData loadedDeck = formatter.Deserialize(stream) as DeckData;

                stream.Close();

                return loadedDeck;
            }
            else
            {
                Debug.Log("No Saved Data");
                stream.Close();
                return null;
            }
        }

        return null;
    }

}
