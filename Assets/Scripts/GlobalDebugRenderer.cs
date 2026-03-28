using System.Collections.Generic;
using UnityEngine;
using static Logic;
public class GlobalDebugRenderer : MonoBehaviour
{
    static List<SphereRenderer> SphereRenderers = new();
    static List<SphereRenderer> Temps = new();

    public static void AddSphere(Vector3 position, float size, Color color, float duration)
    {
        SphereRenderers.Add(new(position, size, color, duration));
       // Debug.Log("added");

    }
    public static void AddSphere(Vector3 position, float size, Color color)
    {
        SphereRenderers.Add(new(position,size,color));
       // Debug.Log("added");

    }

    public static void AddSphere(Vector3 position, float size)
    {
        SphereRenderers.Add(new(position, size));
        //Debug.Log("added");


    }

    public static void AddSphere(Vector3 position)
    {
        SphereRenderers.Add(new(position));
       // Debug.Log("added");


    }



    [System.Serializable]
    public class SphereRenderer
    {
        public bool FinishedFlag;
        public Timer PersistenceTimer;
        private float Size;
        private Color RenderColor;
        private Vector3 Position;

        public void Draw()
        {
           // Debug.Log("drawn");
            PersistenceTimer.Step();
           // Debug.Log(PersistenceTimer.Time);
           // Debug.Log(PersistenceTimer.IsFinished);

            Gizmos.color = RenderColor;
            Gizmos.DrawSphere(Position, Size);

            if (PersistenceTimer.IsFinished)
            {
                FinishedFlag = true;
             //   Debug.Log("finished");
              //  Debug.Log("FinishedFlag: " + FinishedFlag);

            }


        }

        public SphereRenderer(Vector3 position, float size, Color color, float duration)
        {
            FinishedFlag = false;
            PersistenceTimer = new(duration, 0, true);
            Size = size;
            RenderColor = color;
            Position = position;
        }
        public SphereRenderer(Vector3 position, float size, Color color)
        {
            PersistenceTimer = new(1, 0, true);
            FinishedFlag = false;

            Size = size;
            RenderColor = color;
            Position = position;
        }
        public SphereRenderer(Vector3 position, float size)
        {
            PersistenceTimer = new(1, 0, true);
            RenderColor = Color.white;
            FinishedFlag = false;

            Size = size;
            Position = position;
        }
        public SphereRenderer(Vector3 position)
        {
            PersistenceTimer = new(1, 0, true);
            RenderColor = Color.white;
            FinishedFlag = false;

            Size = 1;
            Position = position;
        }

    }


    private void OnDrawGizmos()
    {
        Temps.Clear();

   

        for (int i = 0; i < SphereRenderers.Count; i++)
        {



            SphereRenderers[i].Draw();


          //  Debug.Log(SphereRenderers[i].FinishedFlag);



            if (!SphereRenderers[i].FinishedFlag)
            {
            //    Debug.Log("added");
                Temps.Add(SphereRenderers[i]);
            }




        }
        SphereRenderers.Clear();

        for (int i = 0; i < Temps.Count; i++)
        {
            SphereRenderers.Add(Temps[i]);
        }

    }


}
