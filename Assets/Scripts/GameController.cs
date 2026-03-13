using UnityEngine;
using UnityEngine.InputSystem.XR;

public class GameController : MonoBehaviour
{
  public PlayerController Player_Ref;
  public static GameController Controller;
  public Spawner Spawner_Ref;
  public WorldBounds Bounds;
    private void Awake()
    {
        Controller = this;
    }
    private void OnValidate()
    {
        Controller = this;
    }
    private void OnEnable()
    {
        Controller = this;
    }

}
