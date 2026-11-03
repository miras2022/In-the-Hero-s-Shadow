using UnityEngine;
using System.Collections.Generic;

public class ArtisanQuarter : MonoBehaviour
{
    public string zoneName;
    public List<Zone> connectedZones = new List<Zone>();

    public void Highlight(bool on)
    {
        GetComponent<Renderer>().material.color = on ? Color.yellow : Color.white;
    }
}
