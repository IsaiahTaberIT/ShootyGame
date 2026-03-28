using System.Reflection;
using TMPro;
using UnityEngine;
using static Logic;

public class UiController : MonoBehaviour
{
    public string BaseText = "TestInsert:<i>aftertext";
    public string ReplaceCode = "<i>";
    public MonoBehaviour TargetClass;
    public string TargetVariableName;
    public UnityEngine.Object UIComponent;





    public void UpdateUiValue()
    {
        System.Type type = TargetClass.GetType();
        FieldInfo info = type.GetField(TargetVariableName);

        if (UIComponent is TextMeshProUGUI t)
        {
            string value;

            value = BaseText.InsertOnCode(ReplaceCode, info.GetValue(TargetClass).ToString());

            t.text = value;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {

        if (TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI t))
        {
            UIComponent = t;
        }

        UpdateUiValue();
    }
}
