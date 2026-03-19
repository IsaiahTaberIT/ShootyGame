using NUnit;
using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class GameController : MonoBehaviour
{
  public PlayerController Player_Ref;
  public static GameController Controller;
  public Spawner Spawner_Ref;
  public WorldBounds Bounds;
  public Camera MainCamera_Ref;
  public Vector3[] PreviousMousePositions = new Vector3[120];
  public static Vector3 WorldMousePos;
    Vector3 ViewMousePos;
  [SerializeField ]private int CurrentMousePositionUpdateIndex;

    public Vector3 GetPastMousePosition(float seconds)
    {
        int framesInPast = Mathf.RoundToInt(1f / Time.smoothDeltaTime * seconds);

        framesInPast = Mathf.Clamp(framesInPast, 0, 120);

        //Debug.Log("frames " + framesInPast);

        int realIndex = CurrentMousePositionUpdateIndex - framesInPast;

        if (realIndex < 0)
        {
            realIndex = realIndex + PreviousMousePositions.Length;
        }

        return PreviousMousePositions[realIndex];

    }

    public void UpdateMousePositionArray(Vector3 pos)
    {
        PreviousMousePositions[CurrentMousePositionUpdateIndex] = pos;

        CurrentMousePositionUpdateIndex++;

        CurrentMousePositionUpdateIndex %= PreviousMousePositions.Length;
    }

    private void Update()
    {

        ViewMousePos = (Input.mousePosition / new Vector2(Screen.width, Screen.height));
        ViewMousePos.y = 1f - ViewMousePos.y;
        ViewMousePos = Input.mousePosition;
        ViewMousePos.z = 30;

        Camera c = Controller.MainCamera_Ref;

        Vector3 o = c.transform.position;

        RaycastHit hitinfo;

        if (Physics.Raycast(o, c.ScreenToWorldPoint(ViewMousePos) - o, out hitinfo, 10000f, Bounds.PlaySurfaceLayer))
        {
            WorldMousePos = hitinfo.point;
        }

        UpdateMousePositionArray(WorldMousePos);
        Player_Ref.WorldMousePos = WorldMousePos;
        
    }




    void Init()
    {
        Controller = this;
        MainCamera_Ref = Camera.main;
    }


    private void Awake()
    {
        Init();
    }
    private void OnValidate()
    {
        Init();
    }
    private void OnEnable()
    {
        Init();
    }

}
