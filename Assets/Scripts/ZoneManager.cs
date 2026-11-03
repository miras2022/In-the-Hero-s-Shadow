using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ZoneManager : MonoBehaviour
{
    public List<Zone> zones = new List<Zone>();
    public GameObject zonePrefab; // необязательно, мы создаём вручную, но можно использовать

#if UNITY_EDITOR
    [ContextMenu("Generate Zones (2D Map)")]
    public void GenerateZones2D()
    {
        ClearZones();

        // Создание зон с позициями в 2D (x, y)
        CreateZone("Castle", new Vector2(0, 0));                 // центр
        CreateZone("Market", new Vector2(2, 1));                 // справа-сверху
        CreateZone("Tavern", new Vector2(3, -1));                // справа-внизу
        CreateZone("Farm", new Vector2(-2, -1));                 // слева-внизу
        CreateZone("Church", new Vector2(-2, 1));                // слева-сверху
        CreateZone("ArtisanQuarter", new Vector2(0, 2));         // сверху по центру

        // Связи
        Connect("Castle", new string[] { "Farm", "Church", "Market" });
        Connect("Market", new string[] { "Castle", "Tavern", "ArtisanQuarter" });
        Connect("ArtisanQuarter", new string[] { "Market", "Church" });
        Connect("Church", new string[] { "Castle", "Farm", "ArtisanQuarter" });
        Connect("Farm", new string[] { "Castle", "Church", "Tavern" });
        Connect("Tavern", new string[] { "Farm", "Market" });

        // После создания автоматически поставим Player и Knight на нужные зоны, если они есть в сцене
        SetInitialCharacters();

        Debug.Log("✅ 2D-зоны успешно созданы и размещены!");
    }

    private void ClearZones()
    {
        foreach (var zone in zones)
        {
            if (zone != null)
                DestroyImmediate(zone.gameObject);
        }
        zones.Clear();
    }

    private void CreateZone(string name, Vector2 position)
    {
        GameObject obj;
        if (zonePrefab != null)
        {
            obj = (GameObject)PrefabUtility.InstantiatePrefab(zonePrefab, this.transform);
            obj.transform.position = new Vector3(position.x, position.y, 0);
            obj.name = name;
        }
        else
        {
            obj = new GameObject(name);
            obj.transform.parent = transform;
            obj.transform.position = new Vector3(position.x, position.y, 0);
            // добавить визуал (SpriteRenderer)
            SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
            sr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
            sr.color = Random.ColorHSV(0f, 1f, 0.6f, 1f, 0.6f, 1f);
            sr.sortingOrder = 0;
        }

        // Добавляем 2D Collider, чтобы можно было кликать
        var box = obj.GetComponent<BoxCollider2D>();
        if (box == null) box = obj.AddComponent<BoxCollider2D>();
        // немного размера по спрайту
        box.isTrigger = true;

        Zone zone = obj.GetComponent<Zone>();
        if (zone == null) zone = obj.AddComponent<Zone>();
        zone.zoneName = name;
        zones.Add(zone);
    }

    private void Connect(string from, string[] toList)
    {
        Zone fromZone = zones.Find(z => z.zoneName == from);
        if (fromZone == null) return;
        foreach (string to in toList)
        {
            Zone toZone = zones.Find(z => z.zoneName == to);
            if (toZone != null && !fromZone.connectedZones.Contains(toZone))
                fromZone.connectedZones.Add(toZone);
        }
    }

    private void SetInitialCharacters()
    {
        // Найдём объекты в сцене по имени (Player и Knight)
        var playerObj = GameObject.Find("Player");
        var knightObj = GameObject.Find("Knight");

        if (playerObj != null)
        {
            Zone market = zones.Find(z => z.zoneName == "Market");
            if (market != null)
            {
                playerObj.transform.position = market.transform.position;
                var pc = playerObj.GetComponent<PlayerController>();
                if (pc != null) pc.currentZone = market;
            }
        }

        if (knightObj != null)
        {
            Zone castle = zones.Find(z => z.zoneName == "Castle");
            if (castle != null)
            {
                knightObj.transform.position = castle.transform.position;
                var ka = knightObj.GetComponent<KnightAI>();
                if (ka != null) ka.currentZone = castle;
            }
        }
    }
#endif
}

