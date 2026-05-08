using UnityEngine;

public class DecayScript : MonoBehaviour
{
    public Logic.Timer DecayTimer = new(10, 0);

    private void OnEnable()
    {
        DecayTimer.OnLoop += () => Destroy(this.gameObject);
    }

    private void Update()
    {
        DecayTimer.Step();
    }


}
