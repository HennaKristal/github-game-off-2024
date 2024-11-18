using UnityEngine;

public class SpawnLocation : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    private void Awake()
    {
        Instantiate(playerStats.playerPrefab, transform.position, transform.rotation);
    }
}
