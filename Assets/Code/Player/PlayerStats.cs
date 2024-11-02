using UnityEngine;


[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    public float horizontalSpeed = 5f;
    public float verticalSpeed = 5f;
    public float maxHealth = 100f;
    public float armor = 100f;


    /* other stats / ideas
    energyGeneration
    maxEnergy
    cooling
    maxHeatTolerance
    */

    #if UNITY_EDITOR
    private void OnValidate()
    {
        // if (moveSpeed < 0) Debug.LogWarning("Movement speed is set to 0, the player can not move", this);
    }
    #endif
}
