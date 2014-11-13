using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BugSpawner : MonoBehaviour
{
    //public GUIStyle styleDeadline, styleOver, styleCount;
    public Text text, overText, countText;
    public float GameTime;
    public bool isPlaying = true;
    public float minTime = 0.1f;
    public float maxTime = 2;
    float timeTillSpawn;
    public GameObject BugPrefab;
    public Transform left, right, top, bottom;

    // Use this for initialization
    void Start()
    {
        GameEventManager.TimeLeft = GameTime;
        GameEventManager.BugsKilled = GameEventManager.TotalBugs = 0;
        overText.enabled = countText.enabled = text.enabled = false;
        SetTime();
    }

    void SetTime()
    {
        timeTillSpawn = Random.Range(minTime, maxTime);
    }

    void SpawnBug()
    {
        Vector3 pos = new Vector3(
            Random.Range(left.position.x, right.position.x),
            Random.Range(bottom.position.y, top.position.y),
            0);
        Instantiate(BugPrefab, pos, Quaternion.identity);
        GameEventManager.TotalBugs++;
    }

    // Update is called once per frame
    void Update()
    {
        GameEventManager.TimeLeft -= Time.deltaTime;
        if (GameEventManager.TimeLeft <= 0)
        {
            //overText.enabled = countText.enabled = true;
            GameEventManager.TimeLeft = 0;
            countText.text = "Bugs found: " + GameEventManager.TotalBugs +
                "\nBugs fixed: " + GameEventManager.BugsKilled + " (" + 100 * GameEventManager.BugsKilled / GameEventManager.TotalBugs + "%)";
        }
        else
        {
            timeTillSpawn -= Time.deltaTime;
            if (timeTillSpawn <= 0)
            {
                SetTime();
                SpawnBug();
            }
        }
        text.text = "DEADLINE: " + string.Format("{0:0.00}", GameEventManager.TimeLeft);
    }

    void OnGUI()
    {
        GUIStyle style = GUI.skin.GetStyle("label");
        style.alignment = TextAnchor.MiddleCenter;

        style.normal.textColor = Color.red;
        style.fontSize = 30;
        style.fontStyle = FontStyle.Bold;
        GUI.Label(new Rect(0, 0, 1024, 100), "DEADLINE: " + string.Format("{0:0.00}", GameEventManager.TimeLeft));

        if (GameEventManager.TimeLeft <= 0)
        {
            style.normal.textColor = Color.white;
            style.fontSize = 50;
            GUI.Label(new Rect(0, 200, 1024, 100), "Game Over!");

            style.normal.textColor = Color.grey;
            style.fontSize = 24;
            GUI.Label(new Rect(0, 250, 1024, 100), "Bugs found: " + GameEventManager.TotalBugs +
                "\nBugs fixed: " + GameEventManager.BugsKilled +
                " (" + 100 * GameEventManager.BugsKilled / GameEventManager.TotalBugs + "%)");
        }
    }
}
