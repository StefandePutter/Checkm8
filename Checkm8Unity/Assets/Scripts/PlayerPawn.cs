using UnityEngine;

public class PlayerPawn : PlayerBase
{
    protected override void Shoot()
    {
        GameObject bullet = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Destroy(bullet,0.3f);
    } 
}
