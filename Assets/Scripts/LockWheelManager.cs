using UnityEngine;

public class LockWheelManager : MonoBehaviour
{
    public Transform[] wheels;
    public ChestController chest;

    public int[] correctCombination = { 4, 0, 3, 0, 0 };
    int[] currentValues;

    int selectedWheelIndex = 0;
    public bool isLocked = false;

    public Color selectedColor = Color.magenta;
    public Color normalColor = Color.white;

    ChestInteraction chestInteraction;

    void Start()
    {
        chestInteraction = GetComponent<ChestInteraction>();

        currentValues = new int[wheels.Length];
        for (int i = 0; i < currentValues.Length; i++) 
        { 
            currentValues[i] = 0;
            SetWheelColor(i, normalColor);
        }
    }

    void Update()
    {
        if (chestInteraction == null || !chestInteraction.isUsingLock) return;

        HandleSelection();
        HandleRotation();
    }
    void HandleSelection()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeSelection(1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeSelection(-1);
        }
    }

    void ChangeSelection(int direction)
    {
        SetWheelColor(selectedWheelIndex, normalColor);

        selectedWheelIndex += direction;
        if (selectedWheelIndex >= wheels.Length) selectedWheelIndex = 0;
        if (selectedWheelIndex < 0) selectedWheelIndex = wheels.Length - 1;

        UpdateVisual();
    }

    void HandleRotation()
    {
        if (Input.GetKeyDown(KeyCode.W))
            RotateWheel(1);
        if (Input.GetKeyDown(KeyCode.S))
            RotateWheel(-1);
    }

    void RotateWheel(int direction)
    {
        currentValues[selectedWheelIndex] = (currentValues[selectedWheelIndex] + direction + 10) % 10;

        float startOffset = 36f;
        float targetRotation = (currentValues[selectedWheelIndex] * -36f) + startOffset;
        wheels[selectedWheelIndex].localRotation = Quaternion.Euler(targetRotation, 0, 0);

        CheckCombination();
    }

    void CheckCombination()
    {
        for (int i = 0; i < wheels.Length; i++)
            if (currentValues[i] != correctCombination[i])
                return;
        if (chest != null) chest.OpenChest();

        if(chestInteraction != null)
            chestInteraction.Invoke("ExitLockPad", 0.5f);
    }

    public void UpdateVisual()
    {
        SetWheelColor(selectedWheelIndex, selectedColor);
    }

    void SetWheelColor(int index, Color color)
    {
        if (wheels[index].TryGetComponent<Renderer>(out var r))
            r.material.color = color;
    }
}
