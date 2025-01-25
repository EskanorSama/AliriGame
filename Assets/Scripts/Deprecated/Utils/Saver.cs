using UnityEngine;

public class Saver : MonoBehaviour
{
    private Vector2 SavedPoint;

    public static Saver Instance;

    private void Awake() => Instance = this;

    private void Start()
    { 
        SavedPoint = Player.Instance.transform.position;
    }
    public void Save(Vector2 SavePoint)
    {
        SavedPoint = SavePoint;
    }
    public void Load()
    {
        Player.Instance.transform.position = SavedPoint;
        Player.Instance.Heal(Player.Instance.GetMaxHealth());
    }
}
