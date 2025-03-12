using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Only add game objects that has a Tool component with a unique ToolType to this.
/// A pawn can ask AllTools for a prefab reference by providing a ToolType, but is responsible for the instantiation itself.
/// </summary>
[CreateAssetMenu(fileName = "AllTools", menuName = "Scriptable Objects/AllTools")]
public class AllTools : ScriptableObject
{
    public GameObject[] allTools;
    Dictionary<ToolType, GameObject> toolDictionary = new Dictionary<ToolType, GameObject>();
    
    // Might want to find a better way to handle initialization...
    bool isInitialized = false;
    
    // This needs to be called before anyone attempts get a tool prefab.
    public void Initialize()
    {
        toolDictionary.Clear();
        foreach(GameObject tool in allTools)
        {
            Tool toolComponent = tool.GetComponent<Tool>();
            if (toolComponent != null)
            {
                toolDictionary.Add(toolComponent.GetToolType(), tool);
            }
            else Debug.LogError("Something that is not a tool has been added to AllTools!", tool);
        }
    }

    /// <summary>
    /// Returns a tool prefab reference. Instatiate it yourself!
    /// </summary>
    public GameObject GetTool(ToolType toolType)
    {
        if (!isInitialized) Initialize();
        return toolDictionary[toolType];
    }
}
