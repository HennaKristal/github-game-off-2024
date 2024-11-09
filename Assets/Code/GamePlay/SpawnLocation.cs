using UnityEngine;

public class SpawnLocation : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    private void Start()
    {
        Instantiate(playerStats.playerPrefab);
    }
}
