using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Sanctuary : MonoBehaviour, IUsable
{
    private bool Enter;

    private void Start()
    {
    }
    public void Use()
    {
        OpenSanctuary();
    }

    private void OpenSanctuary()
    {
        Invertory invertory = Invertory.Instance;
        invertory.ResetEquipItem();
        invertory.OnSanctuaryOpen();
        Enter = !Enter;
        if (Enter)
        {
            Saver.Instance.Save(transform.position);
            Player.Instance.Freeze();
            CameraMove.Instance.MoveRight();
            CameraMove.Instance.Lock = true;
            if (!invertory.Once) invertory.InventoryOpenAndClose();
            invertory.EquipButton.SetActive(true);
        }
        else
        {
            Player.Instance.UnFreeze();
            invertory.InventoryOpenAndClose();
            invertory.EquipButton.SetActive(false);
            CameraMove.Instance.ReturnRight();
            CameraMove.Instance.Lock = false;
        }
    }
}
