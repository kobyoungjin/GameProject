using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    Vector3 delta;
    [SerializeField]
    Define.CameraMode cameraMode = Define.CameraMode.Quarterview;
    [SerializeField]
    GameObject player;

    GameObject transparentObj;
    Renderer ObstacleRenderer;  // ������Ʈ�� �������ϰ� ������ִ� ������
    List<GameObject> Obstacles;

    public float distance = 7.0f;   // currentZoom���� ��Ȯ�� �̸����� ����
    float minZoom = 2.0f;
    float maxZoom = 6.0f;
    public float yPos;
    public float zPos;
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
        if(cameraMode == Define.CameraMode.Quarterview)
        {
            RaycastHit hit;

            if (Physics.Raycast(player.transform.position, delta, out hit, delta.magnitude, LayerMask.GetMask("Wall")))
            {
                float dist = (hit.point - player.transform.position).magnitude * 0.8f;
                transform.position = player.transform.position + delta.normalized * dist;
            }
            else
            {
                //transform.position = new Vector3(delta.x, 3.0f + delta.y * distance, delta.z * distance) + player.transform.position + Vector3.zero;
                //transform.rotation = Quaternion.Euler(transform.rotation.x + distance, transform.rotation.y, transform.rotation.z);
                transform.position = new Vector3(0, yPos, -zPos) + player.transform.position + Vector3.zero;
                transform.LookAt(player.transform);
            }
        }
        

        //CalculateZoom();
    }

    void CalculateZoom()
    {
        // ���콺 �� ��/�ƿ�
        distance -= Input.GetAxis("Mouse ScrollWheel");

        // �� �ּ�/�ִ� ����
        // Clamp�Լ� : �ִ�/�ּҰ��� �������ְ� ����
        distance = Mathf.Clamp(distance, minZoom, maxZoom);
    }

    public void SetQuarterView(Vector3 delta)
    {
        cameraMode = Define.CameraMode.Quarterview;
        this.delta = delta;
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
