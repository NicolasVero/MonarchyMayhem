using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DanceChoiceController : MonoBehaviour
{

    [SerializeField] private DanceWheelController DanceWheelController;
    [SerializeField] private GameObject danceText;
    [SerializeField] private int ID;
    [SerializeField] private string itemName;
    [SerializeField] private TextMeshProUGUI itemText;
    private Animator animator;
    private bool selected = false;

    void Awake() {
        this.animator = GetComponent<Animator>();
    }

    public void Selected() {
        this.DanceWheelController.DanceAnimations(this.ID);
        Invoke("DeselectClickedButton", 1f);
    }
    public void Deselected() {
        Debug.Log("Deselected");
    }
    
    public void HoverEnter() {
        this.animator.SetBool("Hover", true);
        GameController.SetPanelVisibility(this.danceText, true);
        this.itemText.text = this.itemName;
    }

    public void HoverExit() {
        this.animator.SetBool("Hover", false);
        GameController.SetPanelVisibility(this.danceText, false);
        this.itemText.text = "";
    }

    private void DeselectClickedButton() { UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null); }

}
