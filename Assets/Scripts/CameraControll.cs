using UnityEngine;


public class CameraControl : MonoBehaviour
{
    public float speed = 2.0f; // �������� �������� ������
    public Transform target; // ����, �� ������� ����� ��������� ������

    private void Awake()
    {
        // ���� ���� �� ������, ���� ������ ������ ���� Character � �����
        if (!target)
        {
            target = FindFirstObjectByType<CharacterControll>().transform;
        }
    }

    private void LateUpdate()
    {
        // ���������� ����� ������� ������
        Vector3 position = target.position;
        position.z = -10.0f; // ������������� z-���������� ������

        // ������ ���������� ������ � ����� �������
        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
    }
}