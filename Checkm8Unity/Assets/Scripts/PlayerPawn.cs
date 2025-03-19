using UnityEngine;

public class PlayerPawn : PlayerBase
{
    protected override void Shoot()
    {
        Instantiate(projectilePrefab, transform.position, transform.rotation);
        //Debug.Log("shot");
    } 
}
