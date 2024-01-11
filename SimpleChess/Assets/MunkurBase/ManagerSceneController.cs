using UnityEngine;

public class ManagerSceneController : SingletonPersistent<ManagerSceneController>
{
    [SerializeField] private bool editMode;

    void Start()
    {
        if (!editMode)
        {
            LevelController.Instance.LoadNextLevel();
        }
    }
}
