using UnityEngine;

public class HitBoxController : MonoBehaviour
{
    public Enemy Enemy;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        Enemy = GetComponentInParent<Enemy>();
    }




   

}
