using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class BulpCtrl {

    private static string BulpSuffix = "Bulp";

    private static Dictionary<Player, Bulp> bulps = new Dictionary<Player, Bulp>();

    public static void InitializeBulps(ArrayList players)
    {
        GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>() as GameObject[];

        foreach(GameObject gameObject in gameObjects)
            if (gameObject.name.Contains("Bulp"))
                gameObject.GetComponent<MeshRenderer>().enabled = false;

        foreach (Player player in players)
        {
            GameObject bulpObject = GameObject.Find(player.Color + BulpSuffix);
            bulpObject.AddComponent<Bulp>();
            bulpObject.GetComponent<MeshRenderer>().enabled = true;
            Bulp bulp = bulpObject.GetComponent<Bulp>();
            bulp.SetColor(PlayerCtrl.GetColor(player));
            bulps.Add(player, bulp);
        }
    }

    public static void Activate(Player player)
    {
        Activate(player, true);
    }

    public static void Deactivate(Player player)
    {
        Activate(player, false);
    }

    private static void Activate(Player player, bool value)
    {
        GetBulp(player).SetActive(value);
    }

    private static Bulp GetBulp(Player player)
    {
        Bulp bulp;
        bulps.TryGetValue(player, out bulp);
        if (bulp == null) throw new InvalidGameStateException();
        return bulp;
    }
}