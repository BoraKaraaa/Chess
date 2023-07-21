using System.Collections.Generic;
using UnityEngine;

public class ManagerSceneAdder : MonoBehaviour
{
    [SerializeField] private List<Transform> _managerScenePrefabs;
    
    private void Awake()
    {
        if (FindObjectOfType<ManagerSceneController>() == null)
        {
            foreach (var managerScenePrefab in _managerScenePrefabs)
            {
                Instantiate(managerScenePrefab);
            }
        }
    }
}
