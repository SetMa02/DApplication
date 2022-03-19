using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddEmployee : MonoBehaviour
{
    [SerializeField] private CanvasGroup _addEmployeeCanvas;

    public void OpenCreateEmployeeCanvas()
    {
        _addEmployeeCanvas.alpha = 1;
        _addEmployeeCanvas.interactable = true;
        _addEmployeeCanvas.blocksRaycasts = true;
    }
}
