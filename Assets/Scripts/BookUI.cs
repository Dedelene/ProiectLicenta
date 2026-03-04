using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BookUI : MonoBehaviour
{
    public static BookUI Instance { get; private set; }
    public static bool IsOpen { get; private set; }
    public GameObject bookUI;

    public Image pageImage;
    public Button prevButton;
    public Button nextButton;
    public Button closeButton;
    public GameObject dimBackground;
    public MonoBehaviour cameraController;
    public MonoBehaviour movementController;

    private Sprite[] pages;
    private int currentPage = 0;

    void Awake()
    {
        Instance = this;

        prevButton.onClick.AddListener(PrevPage);
        nextButton.onClick.AddListener(NextPage);
        closeButton.onClick.AddListener(CloseBook);

        if (dimBackground)
        {
            if (!dimBackground.TryGetComponent<Button>(out var bgBtn)) bgBtn = dimBackground.AddComponent<Button>();
            bgBtn.transition = Selectable.Transition.None;
            bgBtn.onClick.AddListener(CloseBook);
        }
    }
    void Start()
    {
        gameObject.SetActive(false);
    }


    void Update()
    {
        if (!IsOpen) return;
        if (Input.GetKeyDown(KeyCode.RightArrow)) NextPage();
        if (Input.GetKeyDown(KeyCode.LeftArrow)) PrevPage();
        if (Input.GetMouseButtonDown(1)) CloseBook();
    }
    
    public void OpenBook(Sprite[] contentPages)
    {

        bookUI.SetActive(true);
        if (cameraController) cameraController.enabled = false;
        if (movementController) movementController.enabled = false;

        if (contentPages == null || contentPages.Length == 0)
        {
            Debug.LogWarning("BookUI.OpenBook called with no pages.");
            return;
        }

        pages = contentPages;
        currentPage = 0;
        ShowPage();

        gameObject.SetActive(true);
        IsOpen = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseBook()
    {
        IsOpen = false;

        if (bookUI) bookUI.SetActive(false);
        gameObject.SetActive(false);

        if (cameraController) cameraController.enabled = true;
        if (movementController) movementController.enabled = true;
        bookUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ShowPage()
    {
        if (pages == null || pages.Length == 0) return;
        pageImage.sprite = pages[currentPage];
        prevButton.interactable = currentPage > 0;
        nextButton.interactable = currentPage < pages.Length - 1;
    }

    private void PrevPage()
    {
        if (currentPage <= 0) return;
        currentPage--;
        ShowPage();
    }

    private void NextPage()
    {
        if (currentPage >= pages.Length - 1) return;
        currentPage++;
        ShowPage();
    }
}
