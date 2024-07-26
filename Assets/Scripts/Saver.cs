using UnityEngine;

public class Saver : MonoBehaviour
{
    private Vector2 SavedPoint;

    public static Saver Instance;

    private void Awake() => Instance = this;

    private void Start()
    { 
        SavedPoint = Movement.Instance.transform.position;
    }
    public void Save(Vector2 SavePoint)
    {
        SavedPoint = SavePoint;
    }
    public void Load()
    {
        Movement.Instance.transform.position = SavedPoint;
        Movement.Instance.Heal(Movement.Instance.GetMaxHealth());
    }
}
