using UnityEngine;
using System.Collections.Generic;

public class SwordSlash : MonoBehaviour
{
    public float killDistance = 1.2f; 
    private Camera cam;
    private Vector3 lastPos;
    private Vector2 slashDir;

    private HashSet<Enemy> hitEnemies = new HashSet<Enemy>();

    void Start()
    {
        cam = Camera.main;
        lastPos = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hitEnemies.Clear();
        }

        if (Input.GetMouseButton(0))
        {
            MoveWithMouse();
            CalculateSlashDirection();

            if (Vector2.Distance(transform.position, lastPos) > 0.05f)
            {
                KillEnemiesByDistance();
            }
        }

        lastPos = transform.position;
    }

    void MoveWithMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(cam.transform.position.z);
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
        worldPos.z = 0;
        transform.position = worldPos;
    }

    void CalculateSlashDirection()
    {
        Vector2 delta = transform.position - lastPos;
        if (delta.magnitude > 0.01f)
            slashDir = delta.normalized;
    }

    void KillEnemiesByDistance()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, killDistance);

        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag("Enemy")) continue;

            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy == null) continue;

            if (hitEnemies.Contains(enemy)) continue;

            enemy.OnSlashed(slashDir);

            hitEnemies.Add(enemy);
        }
    }

}
