using System.Collections.Generic;
using UnityEngine;

public class Garage : MonoBehaviour
{
    public List<EngineStats> engines; // List of all possible engines
    public List<CoolerStats> coolers; // List of all possible coolers
    public List<GeneratorStats> generators; // List of all possible generators
    public List<PlaneStats> planes; // List of all possible planes
    public List<WeaponStats> weapons; // List of all possible main weapons

    private void Start()
    {
        // TODO: on start load prefab or serialized data about every single scriptable object part and load their values
        // Also set this object to do not destory on load
    }

    public void AddItem(string category, string name)
    {
        // TODO find the correct part's scrptable object and set it's stats correctly
        //part.isOwned = true;
        //part.isPurchasable = true;
    }

    private void EquipPart(string category, string name)
    {
        // Get currently equipped item and unequip it or unequip just every item in this category
        // currentpart.isequipped = false;

        // TODO: equip the new item
        // part.isEquipped = true;
    }

    // Helper function to find a part by name in any category list
    //private PlanePart FindPartByName(string name)
    //{
    //    foreach (var list in new List<List<PlanePart>> { engines, guns, cores })
    //    {
    //        PlanePart part = list.Find(p => p.partName == name);
    //        if (part != null) return part;
    //    }
    //    return null;
    //}
}
