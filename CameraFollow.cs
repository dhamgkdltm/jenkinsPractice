using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject followObject; // 카메라가 따라다니는 대상
    public Vector2 followOffset; // 대상과의 거리 오프셋
    public float speed = 3f; // 카메라 이동 속도
    private Vector2 threshold;
    private Rigidbody2D rb; // 대상의 Rigidbody2D

    // Start is called before the first frame update
    void Start()
    {
        // threshold와 rb 초기화
        threshold = calculateThreshold();
        rb = followObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // followObject의 위치 가져오기
        Vector2 follow = followObject.transform.position;

        // x축, y축 거리 차이 계산
        float xDifference = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * follow.x);
        float yDifference = Vector2.Distance(Vector2.up * transform.position.y, Vector2.up * follow.y);

        Vector3 newPosition = transform.position; // 새로운 카메라 위치 초기화

        // x축 거리 차이가 일정 값 이상일 때 카메라 이동
        if (Mathf.Abs(xDifference) >= threshold.x)
        {
            newPosition.x = follow.x;
        }

        // y축 거리 차이가 일정 값 이상일 때 카메라 이동
        if (Mathf.Abs(yDifference) >= threshold.y)
        {
            newPosition.y = follow.y;
        }
        
        float moveSpeed = rb.velocity.magnitude > speed ? rb.velocity.magnitude : speed; // 카메라 이동 속도 계산
        
        transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime); // 카메라 위치 이동

        /* 플레이어의 높이가 3 이상일 때 카메라가 플레이어를 따라가는 Offsety를 조정 start */
        if (followObject.transform.position.y > 3)
        {
            followOffset.y = 4;
        }
        else
        {
            followOffset.y = 0;
        }

        threshold = calculateThreshold();
        /* 플레이어의 높이가 3 이상일 때 카메라가 플레이어를 따라가는 Offsety를 조정 end */
    }

    // 카메라와 대상 간의 거리를 계산하여 threshold 값 반환
    private Vector3 calculateThreshold()
    {
        Rect aspect = Camera.main.pixelRect;
        Vector2 t = new Vector2(Camera.main.orthographicSize * aspect.width / aspect.height, Camera.main.orthographicSize);
        t.x -= followOffset.x;
        t.y -= followOffset.y + 0.2f;
        return t;
    }

    // threshold 값에 맞게 사각형을 그리는 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 border = calculateThreshold();
        Gizmos.DrawWireCube(transform.position + Vector3.down * 4f, new Vector3(border.x * 2, border.y * 2, 1));
    }
}