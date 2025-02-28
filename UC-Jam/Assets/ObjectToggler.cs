using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ObjectToggler : MonoBehaviour
{
    [System.Serializable]
    public class ToggleElement
    {
        public GameObject inactiveObject;
        public GameObject activeObject;
    }

    public List<ToggleElement> elements; // Assign elements in the Inspector
    public GameManager gameManager; // Reference GameManager
    public string variableName; // Enter the GameManager variable name in the Inspector

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = GameManager.Instance; // Auto-assign GameManager
        }

        if (string.IsNullOrEmpty(variableName))
        {
            Debug.LogError("Variable name is not set in ObjectToggler!");
        }

        UpdateObjects();
    }

    private void Update()
    {
        UpdateObjects();
    }

    private void UpdateObjects()
    {
        if (gameManager == null || string.IsNullOrEmpty(variableName))
            return;

        bool state = GetBooleanVariable(gameManager, variableName);

        foreach (ToggleElement element in elements)
        {
            if (element.inactiveObject != null)
                element.inactiveObject.SetActive(!state);

            if (element.activeObject != null)
                element.activeObject.SetActive(state);
        }
    }

    private bool GetBooleanVariable(GameManager manager, string varName)
    {
        Type type = manager.GetType();
        FieldInfo field = type.GetField(varName);
        PropertyInfo property = type.GetProperty(varName);

        if (field != null && field.FieldType == typeof(bool))
        {
            return (bool)field.GetValue(manager);
        }
        else if (property != null && property.PropertyType == typeof(bool))
        {
            return (bool)property.GetValue(manager);
        }

        Debug.LogError($"Variable '{varName}' not found or not a boolean in GameManager!");
        return false;
    }
}
