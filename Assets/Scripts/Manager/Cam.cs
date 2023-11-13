using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    Vector3 delta;
    [SerializeField]
    Define.CameraMode cameraMode = Define.CameraMode.Quarterview;
    Define.CameraMode preCameraMode = Define.CameraMode.Backview;
    [SerializeField]
    GameObject player;

    GameObject transparentObj;
    Renderer ObstacleRenderer;  // ������Ʈ�� �������ϰ� ������ִ� ������
    List<GameObject> Obstacles;

    public GameObject target;

    public float distance = 7.0f;   // currentZoom���� ��Ȯ�� �̸����� ����
    float dampTrace = 0.5f;
    float minZoom = 2.0f;
    float maxZoom = 6.0f;

    public float yPos;
    public float zPos;

    bool changed = false;

    float QuterDis;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Obstacles = new List<GameObject>(); // �� ����Ʈ ����
        delta = new Vector3(0, 0.6f, -1.5f);
        yPos = 7.26f;  // 8
        zPos = 5.65f;
    }

    private void Update()
    {
        //FadeOut();
        
    }
    void LateUpdate()
    {
        CalculateZoom();
    }

    private void FixedUpdate()
    {
        //Debug.Log(Mathf.Floor(Vector3.Distance(transform.position, player.transform.position) *1000f) /1000f);
        //Debug.Log(Mathf.Floor(QuterDis *1000f) /1000f);

        RaycastHit hit;
        if (cameraMode == Define.CameraMode.Quarterview)
        {
            if (Physics.Raycast(player.transform.position, delta, out hit, delta.magnitude, LayerMask.GetMask("Wall")))
            {
                float dist = (hit.point - player.transform.position).magnitude * 0.8f;
                transform.position = player.transform.position + delta.normalized * dist;
            }
            else
            {
                if(Mathf.Floor(Vector3.Distance(transform.position, player.transform.position) * 1000f) / 1000f == Mathf.Floor(QuterDis * 1000f) / 1000f)
                {
                    changed = false;
                }
                else
                {
                    changed = true;
                }

                if (preCameraMode == Define.CameraMode.Backview && changed)
                {
                    Debug.Log("s");
                    transform.position = Vector3.Lerp(transform.position, new Vector3(0, yPos, -zPos) + player.transform.position + Vector3.zero, dampTrace);
                    transform.LookAt(player.transform);
                    return;
                }

                transform.position = new Vector3(0, yPos, -zPos) + player.transform.position + Vector3.zero;
                QuterDis = Vector3.Distance(transform.position, player.transform.position);
                transform.LookAt(player.transform);
                return;
            }
        }
        else if (cameraMode == Define.CameraMode.Backview)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position - (player.transform.forward * distance)
                                                        + (Vector3.up * yPos), Time.deltaTime * dampTrace);
            transform.LookAt(player.transform);
        }
    }

    void CalculateZoom()
    {
        // ���콺 �� ��/�ƿ�
        distance -= Input.GetAxis("Mouse ScrollWheel");

        // �� �ּ�/�ִ� ����
        // Clamp�Լ� : �ִ�/�ּҰ��� �������ְ� ����
        distance = Mathf.Clamp(distance, minZoom, maxZoom);
    }

    public void SetViewMode(Define.CameraMode mode)
    {
        preCameraMode = cameraMode;

        cameraMode = mode;        
        //target = obj;
    }

    public void SetTarget(GameObject obj)
    {
        target = obj;
    }

    private void FadeOut()
    {
        // Raycast�� �̿��Ͽ� �÷��̾�� ī�޶� ���̿� �ִ� ������Ʈ ����
        // ������Ʈ�� �������� �������� Layer�� Ignor Raycast�� �ٲ���ƾ� ��
        // Ignore Raycast: Player, Terrain, Particles(Steam, DustStorm)
        float distance = Vector3.Distance(transform.position, player.transform.position) - 1;
        Vector3 direction = (player.transform.position - transform.position).normalized;
        RaycastHit[] hits;

        // ī�޶󿡼� �÷��̾ ���� �������� ����� �� ���� ������Ʈ�� �ִٸ�
        hits = Physics.RaycastAll(transform.position, direction, distance);
       

        bool remove = true;
        if (Obstacles.Count != 0 && hits != null)
        {
            Debug.Log(Obstacles.Count);
            for (int i = 0; i < Obstacles.Count; i++)
            {
                foreach (var hit in hits)
                {
                    Debug.Log(hit);
                    // hit�� ������Ʈ�� ����Ʈ�� ������� �ʾ��� ���̸� ��� Ž��
                    if (Obstacles[i] != hit.collider.gameObject)
                        continue;
                    // ����� ������Ʈ�� ����
                    else
                    {
                        remove = false;
                        break;
                    }
                }

                // ���� ����̸�
                if (remove == true)
                {
                    ObstacleRenderer = Obstacles[i].GetComponent<MeshRenderer>();
                    RestoreMaterial();

                    Obstacles.Remove(Obstacles[i]);
                }
            }
        }

        if (hits.Length > 0)
        {
            // �̹� ����� ������Ʈ���� Ȯ��
            for (int i = 0; i < hits.Length; i++)
            {
                Debug.DrawRay(transform.position, direction * distance, Color.red);

                transparentObj = hits[i].collider.gameObject;

                // �̹� ����� ������Ʈ�̸� ���� ������Ʈ �˻�
                if (Obstacles != null && Obstacles.Contains(transparentObj))
                    continue;

                // ������� ���� ������Ʈ�� ����ȭ �� ����Ʈ�� �߰�
                if (transparentObj.layer == 9)
                    ObstacleRenderer = transparentObj.GetComponent<Renderer>();
                if (ObstacleRenderer != null && transparentObj != null)
                {
                    // ������Ʈ�� �������ϰ� �������Ѵ�
                    Material material = ObstacleRenderer.material;
                    Color matColor = material.color;
                    matColor.a = 0.5f;
                    material.color = matColor;

                    // ����Ʈ�� �߰�
                    Obstacles.Add(transparentObj);
                    ObstacleRenderer = null;
                    transparentObj = null;
                }
            }
        }
    }

    // ���� ����ȭ�� ������Ʈ�� ���󺹱� �ϴ� �޼ҵ�
    void RestoreMaterial()
    {
        Material material = ObstacleRenderer.material;
        Color matColor = material.color;
        matColor.a = 1f;    // ���İ� 1:������(���󺹱�)
        material.color = matColor;

        ObstacleRenderer = null;
    }
}
