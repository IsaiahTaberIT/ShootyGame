using UnityEngine;
using static Logic;
public class Laser : Weapon
{
    public Timer ChargeUpTime = new(0.5f, 0, true);
    public Timer UpTime = new(10f, 0, true);
    public Vector3 TargetDirection;
    public float Z;
    public float OffsetMult;
    private void Update()
    {
        Vector3 self = transform.position;


     
        Vector3 mouse = GameController.WorldMousePos;
        mouse.z = Z;
        Vector3 player = GameController.Controller.Player_Ref.transform.position;
        player.z = Z;





        TargetDirection = mouse - player;
        self = TargetDirection.normalized * transform.lossyScale.z * OffsetMult + player;
        self.z = Z;
        transform.position = self;

        transform.rotation = Quaternion.LookRotation(TargetDirection, Vector3.forward);


    }
    /*
transform.position = TargetDirection.normalized* transform.lossyScale.z;

TargetDirection = GameController.WorldMousePos - GameController.Controller.Player_Ref.transform.position;

    float targetangle = Vector2.SignedAngle((Vector2)TargetDirection, Vector2.up);

Vector3 euler = new Vector3(0, 0, targetangle);


transform.eulerAngles = euler;
*/
}
