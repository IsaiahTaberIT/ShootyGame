using NUnit;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static GameController;

public class GameController : MonoBehaviour
{
    public InputActionReference LookAction;

    public bool IsGameRunning => !(GameOver || Paused);

    public bool GameOver = false;
    public bool Paused;
    public static Action OnFixedUpdateUnPaused = () => { };
    public static Action OnUpdateUnPaused = () => { };

  public bool EnemyArrayFull => (AvailablePtr < 0);

  public PlayerController Player_Ref;
  public static GameController Controller;
  public Spawner Spawner_Ref;
  public WorldBounds Bounds;
  public Camera MainCamera_Ref;
  public Vector3[] PreviousMousePositions = new Vector3[120];
  public static Vector3 WorldMousePos;
  Vector3 ViewMousePos;


    public List<int> AvailableIndicies = new(100);
    public int AvailablePtr = 0;

    [SerializeField] private int _MaxEnemyCount = 1;
    public List<Enemy> Enemies = new List<Enemy>();

    [SerializeField] private int CurrentMousePositionUpdateIndex;
    public bool Godmode;

    public int MaxEnemyCount
    {
        get
        {
            return _MaxEnemyCount;
        }
        set
        {
            _MaxEnemyCount = value;
        }
    }

    
    



    public void TryAddEnemy(Enemy e)
    {
        if (!EnemyArrayFull)
        {
            if (Logic.IsValueInRange(AvailableIndicies, AvailablePtr))
            {
                Enemies[AvailableIndicies[AvailablePtr]] = e;
                Enemies[AvailableIndicies[AvailablePtr]].Index = AvailableIndicies[AvailablePtr];
            }

            AvailablePtr--;
        
        }
    }


    public bool CanHandleMoreEmemies(int Count)
    {
        if (AvailablePtr - Count < 0)
        {
            return false;
        }

        return true;
    }


    void ResetEnemyList()
    {
        Enemies.Clear();

        Enemy[] tempArray = FindObjectsByType<Enemy>(FindObjectsSortMode.None);


        Enemies.Clear();
        Enemies.AddRange(new Enemy[MaxEnemyCount]);
        AvailableIndicies.Clear();
        AvailableIndicies.Capacity = MaxEnemyCount;

        AvailableIndicies.AddRange(new int[MaxEnemyCount]);


        for (int i = 0; i < AvailableIndicies.Count; i++)
        {
            AvailableIndicies[i] = i;
        }

        AvailablePtr = MaxEnemyCount - 1;


        for (int i = 0; i < tempArray.Length; i++)
        {
            if (i < MaxEnemyCount)
            {
                Enemies[AvailableIndicies[AvailablePtr]] = tempArray[i];
                Enemies[AvailableIndicies[AvailablePtr]].Index = AvailableIndicies[AvailablePtr];
                AvailablePtr--;
            }
            else if (i > MaxEnemyCount)
            {
                tempArray[i].Die();
            }
            else
            {
                
            }



        }

    }

    public void RemoveEnemy(int index)
    {
        Enemies[index] = null;


        if (Logic.IsValueInRange(AvailableIndicies, AvailablePtr + 1))
        {
            AvailablePtr++;

            AvailableIndicies[AvailablePtr] = index;

        }


    }

    private void OnValidate()
    {

        Init();
        ResetEnemyList();

    }


  

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

    public void GameEnd()
    {

        if (!Godmode) 
        {
            GameOver = true;
        }



    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    private void Update()
    {
        if (IsGameRunning)
        {
            OnUpdateUnPaused.Invoke();
        }



    }
    private void FixedUpdate()
    {
        if (IsGameRunning)
        {
            Physics.Simulate(Time.fixedDeltaTime);
            OnFixedUpdateUnPaused.Invoke();


            Vector2 mousePosition = LookAction.action.ReadValue<Vector2>();

          //  Debug.Log(mousePosition);

            ViewMousePos = (mousePosition / new Vector2(Screen.width, Screen.height));
            ViewMousePos.y = 1f - ViewMousePos.y;
            ViewMousePos = mousePosition;
            ViewMousePos.z = 30;

            Camera c = Controller.MainCamera_Ref;

            Vector3 o = c.transform.position;

            RaycastHit hitinfo;

            if (Physics.Raycast(o, c.ScreenToWorldPoint(ViewMousePos) - o, out hitinfo, 10000f, Bounds.PlaySurfaceLayer))
            {
                WorldMousePos = hitinfo.point;
            }

            UpdateMousePositionArray(WorldMousePos);

        }




   
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
    private void OnEnable()
    {

        Init();

        Enemies.AddRange(new Enemy[MaxEnemyCount]);
        AvailableIndicies.AddRange(new int[MaxEnemyCount]);
        ResetEnemyList();

    }

}
