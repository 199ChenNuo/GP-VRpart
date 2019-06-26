using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Image image;
    public Text imageText;
    public Button left;
    public Button right;
    public Button start;
    public Button export;

    public EventController eventController;
    public MeshController meshController;

    private string baseDir = "Assets/Resources/FOLD/";
    private string format = ".fold";

    private Dictionary<int, List<string>> folds;
    private string[] foldname = new string[4] { "Origami", "Popups", "CurvedCreases", "Tessellations" };

    private int typeID = 0;
    private int imageID = 0;
    
    // Start is called before the first frame update
    private void Awake()
    {
        folds = new Dictionary<int, List<string>>();

        List<string> origami = new List<string>();
        origami.Add("flappingBird");
        origami.Add("simpleVertex");
        origami.Add("traditionalCrane");

        List<string> popups = new List<string>();
        popups.Add("castlePopup");

        List<string> curved = new List<string>();
        curved.Add("huffmanTower");

        List<string> tessellation = new List<string>();
        tessellation.Add("huffmanStarsTriangles");
        tessellation.Add("miura-ori");

        folds.Add(0, origami);
        folds.Add(1, popups);
        folds.Add(2, curved);
        folds.Add(3, tessellation);

    }
    void Start()
    {
        left.onClick.AddListener(OnClickLeft);
        right.onClick.AddListener(OnClickRight);
        start.onClick.AddListener(OnStart);
        export.onClick.AddListener(OnExport);

        ChangeImage();
        CheckButton();
    }

    private void CheckButton()
    {
        if (imageID == 0)
        {
            left.gameObject.SetActive(false);
        }
        else
        {
            left.gameObject.SetActive(true);
        }

        if(imageID == folds[typeID].Count-1)
        {
            right.gameObject.SetActive(false);
        }
        else
        {
            right.gameObject.SetActive(true);
        }
    }

    public void OnClickRight()
    {
        imageID += 1;
        ChangeImage();
        CheckButton();
    }
    public void OnClickLeft()
    {
        imageID -= 1;
        ChangeImage();
        CheckButton();
    }
    public void OnStart()
    {
        string file = baseDir + foldname[typeID] +"/"+ folds[typeID][imageID] + format;
        Debug.Log(file);
        eventController.Load(file);
    }
    public void OnExport()
    {
        eventController.Export();
    }
    private void ChangeImage()
    {
        string fold = foldname[typeID];
        List<string> li = folds[typeID];
        string path = "FoldImage/" + fold + "/" + imageID.ToString();
        Debug.Log(path);
        image.sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
        imageText.text = li[imageID];
    }
    
    public void OnTypeChange(int idx)
    {
        imageID = 0;
        typeID = idx;
        ChangeImage();
        CheckButton();
    }

    public void OnViewModeChange(int idx)
    {
        meshController.displayType = (MeshController.Type)idx;
    }
}
