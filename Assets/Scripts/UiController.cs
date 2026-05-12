using System;
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
    private FieldInfo Info;

    public void UpdateUiValue()
    {

        System.Type type = TargetClass.GetType();

        if (Info == null)
        {
            Info = type.GetField(TargetVariableName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        if (UIComponent is TextMeshProUGUI t)
        {
            string value;

            if (!TargetClass)
            {
                Debug.Log("No Target Class Reference");
                return;

            }

            if (Info == null)
            {
                Debug.Log("Target Class Does Not Have Requested Field");
                return;
            }


            value = BaseText.InsertOnCode(ReplaceCode, Info.GetValue(TargetClass).ToString());

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
