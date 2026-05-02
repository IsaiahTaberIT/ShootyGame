using System.Net;
using UnityEngine;
using static UnityEngine.ParticleSystem;
[RequireComponent (typeof(ParticleSystem))]
[ExecuteAlways]
public class ParticleSystemDriver : MonoBehaviour
{
    public float ElapsedTime;
    public ParticleSystem Target;
    public Shader MaterialShader;
    public Material GeneratedMaterial;
    public Gradient g;
    public Color color;    [Min(0.01f)] public float MaxLifetime = 10f;
    public float CurrentTime;

    ParticleSystemRenderer pRender;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        

        Target = GetComponent<ParticleSystem>();

        GeneratedMaterial = new(MaterialShader);


        pRender = Target.GetComponent<ParticleSystemRenderer>();

        pRender.material = GeneratedMaterial;

    }



    private void Update()
    {
        MainModule main = Target.main;

        if (Target.isPlaying)
        {
            ElapsedTime += Time.deltaTime;
        }
        else if (Target.isStopped) 
        {
            ElapsedTime = 0;
        }



        ColorOverLifetimeModule c = Target.colorOverLifetime;

        g = c.color.gradient;
        CurrentTime = Target.time;

        color = g.Evaluate(Mathf.InverseLerp(0, MaxLifetime, ElapsedTime));

        pRender.sharedMaterial.SetColor("_BaseColor", color);


    }

    private void OnValidate()
    {

        ElapsedTime = 0;
        Target = GetComponent<ParticleSystem>();


    }


}
