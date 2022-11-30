using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CursorSelect : MonoBehaviour, IEndGameObserver
{
    public CursorDefinition[] cursors;
    public CanvasGroup[] canvasGroups;

    public Image enemyCursorUI;
    public Sprite enemyCursorSprite;

    public GameObject[] pointsToUnlockUI;

    private CanvasGroup selectedsGroup;

    private readonly float lockedAlpha = 0.05f;
    private readonly float unselectedAlpha = 0.4f;
    private readonly float selectedAlpha = 1f;

    void Start()
    {
        // setting cursor
        if (PlayerPrefs.HasKey("selected mouse"))
        {
            selectedsGroup = canvasGroups[PlayerPrefs.GetInt("selected mouse")];
            SetCursor(PlayerPrefs.GetInt("selected mouse"));
        }
        // if they've never played/chosen one before just use default
        else
        {
            selectedsGroup = canvasGroups[0];
            SetCursor(0);
        }

        GameManager.instance.AddEndGameObserver(this);
    }

    // make button opaque to show it's selected & equip that cursor
    public void SetCursor(int cursorIndex)
    {
        selectedsGroup.alpha = unselectedAlpha;
        Cursor.SetCursor(cursors[cursorIndex].texture, cursors[cursorIndex].hotSpot, CursorMode.ForceSoftware);
        selectedsGroup = canvasGroups[cursorIndex];
        selectedsGroup.alpha = selectedAlpha;

        PlayerPrefs.SetInt("selected mouse", cursorIndex);
    }

    // make buttons interactable and all that jazz when player loses
    public void EndGameNotify()
    {
        int amountUnlocked = Mathf.FloorToInt(PlayerPrefs.GetInt("highscore") / 50);

        for (int i = 0; i < cursors.Length; i++)
        {
            if (amountUnlocked >= i)
            {
                canvasGroups[i].alpha = unselectedAlpha;
                pointsToUnlockUI[i].SetActive(false);
                canvasGroups[i].GetComponentInChildren<Button>().interactable = true;
            }
            else
            {
                canvasGroups[i].alpha = lockedAlpha;
                canvasGroups[i].GetComponentInChildren<Button>().interactable = false;
            }
        }
        if (amountUnlocked >= 4)
        {
            enemyCursorUI.sprite = enemyCursorSprite;
        }

        SetCursor(PlayerPrefs.GetInt("selected mouse"));
    }
}