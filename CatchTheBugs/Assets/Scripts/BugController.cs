using UnityEngine;
using System.Collections;

public class BugController : MonoBehaviour
{
    public float angleChange;
    public float angleRange;
    float angleD = 0;

    public float velocity;
    public float angle;

    Vector3 prevPosition;

    public Transform left, right, top, bottom;

    // Use this for initialization
    void Start()
    {
        angle = Random.Range(0, 360);
        prevPosition = transform.position;

        left = GameObject.Find("Left").transform;
        right = GameObject.Find("Right").transform;
        top = GameObject.Find("Top").transform;
        bottom = GameObject.Find("Bottom").transform;
    }

    void updateAngle()
    {
        angleD = Mathf.Clamp(angleD + Random.Range(-angleChange, angleChange), -angleRange, angleRange);
        angle = angle + angleD;
        while (angle > 360)
            angle -= 360;
        while (angle < 0)
            angle += 360;

        if (transform.position.x <= left.transform.position.x && angle > 180)
            angle = 360 - angle;
        if (transform.position.x >= right.transform.position.x && angle < 180)
            angle = 360 - angle;

        if (transform.position.y >= top.transform.position.y && (angle < 90 || angle > 270))
            angle = 180 - angle;
        if (transform.position.y <= bottom.transform.position.y && (angle > 90 && angle < 270))
            angle = 180 - angle;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float rad = Mathf.Deg2Rad * angle;
        float speed = velocity * Time.deltaTime;
        Vector3 move = new Vector3(speed * Mathf.Sin(rad), speed * Mathf.Cos(rad), 0);
        transform.position += move;

        prevPosition = transform.position;
        if (move != Vector3.zero)
        {
            float ang = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(ang, Vector3.forward);
        }

        updateAngle();
    }

    void OnMouseDown()
    {
        if (GameEventManager.TimeLeft > 0)
        {
            GameEventManager.BugsKilled++;
            renderer.enabled = false;
            Destroy(gameObject);
        }
    }
}
