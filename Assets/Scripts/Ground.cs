using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    Player player; // Tham chiếu đến đối tượng Player

    public float groundHeight; // Chiều cao của mặt đất
    public float groundRight; // Vị trí bên phải của mặt đất
    public float screenRight; // Vị trí bên phải của màn hình
    BoxCollider2D groundcollider2D; // Collider của mặt đất

    bool didGenerateGround = false; // Cờ để kiểm tra mặt đất đã được tạo mới chưa

    public ObstacleWater obstacleWater;
    public ObstacleEnemy obstacleEnemy;

    // Phương thức Awake được gọi khi đối tượng này được khởi tạo
    private void Awake()
    {
        // Tìm đối tượng Player và lấy component Player từ nó
        player = GameObject.Find("Player").GetComponent<Player>();

        // Lấy component BoxCollider2D của mặt đất
        groundcollider2D = GetComponent<BoxCollider2D>();
        // Tính toán chiều cao của mặt đất dựa trên vị trí và kích thước của collider
        groundHeight = transform.position.y + (groundcollider2D.size.y / 2);
        // Tính toán vị trí bên phải của màn hình
        screenRight = Camera.main.transform.position.x * 2;
    }

    void Start()
    {
        // Mã khởi tạo có thể đặt ở đây
    }

    void Update()
    {
        // Mã cập nhật có thể đặt ở đây
    }

    // FixedUpdate được gọi ở khoảng thời gian cố định và được dùng cho tính toán vật lý
    private void FixedUpdate()
    {
        // Lấy vị trí hiện tại của mặt đất
        Vector2 pos = transform.position;
        // Di chuyển mặt đất sang trái dựa trên vận tốc của người chơi
        pos.x -= player.velocity.x * Time.fixedDeltaTime;

        // Tính toán vị trí bên phải của mặt đất
        groundRight = transform.position.x + (groundcollider2D.size.x / 2);

        // Nếu mặt đất đã ra khỏi màn hình, phá hủy nó
        if (groundRight < 0)
        {
            Destroy(gameObject);
            return;
        }

        // Nếu mặt đất chưa được tạo mới và vị trí bên phải của mặt đất nhỏ hơn vị trí bên phải của màn hình
        if (!didGenerateGround)
        {
            if (groundRight < screenRight)
            {
                didGenerateGround = true; // Đặt cờ mặt đất đã được tạo mới
                generateGround(); // Gọi phương thức để tạo mới mặt đất
            }
        }
        // Cập nhật vị trí của mặt đất
        transform.position = pos;
    }

    // Phương thức để tạo mới mặt đất
    void generateGround()
    {
        // Tạo bản sao của đối tượng mặt đất hiện tại
        GameObject go = Instantiate(gameObject);
        // Lấy component BoxCollider2D của đối tượng mới
        BoxCollider2D goCollider = go.GetComponent<BoxCollider2D>();
        Vector2 pos;

        // Tính toán các giá trị liên quan đến chiều cao nhảy tối đa của người chơi
        float h1 = player.jumpVelocity * player.maxHoldJumpTime;
        float t = player.jumpVelocity / -player.gravity;
        float h2 = player.jumpVelocity * t + (0.5f * (player.gravity * (t * t)));
        float maxJumpHeight = h1 + h2;
        float maxY = maxJumpHeight * 0.7f;
        maxY += groundHeight;
        float minY = 1;
        float actualY = UnityEngine.Random.Range(minY, maxY);

        // Đặt vị trí y của mặt đất mới
        pos.y = actualY - goCollider.size.y / 2;
        if (pos.y > 2.7f)
            pos.y = 2.7f;

        // Tính toán các giá trị liên quan đến thời gian và khoảng cách
        float t1 = t + player.maxHoldJumpTime;
        float t2 = Mathf.Sqrt((2.0f * (maxY - actualY)) / -player.gravity);
        float totalTime = t1 + t2;
        float maxX = totalTime * player.velocity.x;
        maxX *= 0.7f;
        maxX += groundRight;
        float minX = screenRight + 5;
        float actualX = UnityEngine.Random.Range(minX, maxX);

        // Đặt vị trí x của mặt đất mới
        pos.x = actualX + goCollider.size.x / 2; 
        go.transform.position = pos;

        // Cập nhật chiều cao của mặt đất mới
        Ground goGround = go.GetComponent<Ground>();
        goGround.groundHeight = go.transform.position.y + (goCollider.size.y / 2);

        int obstacleWaterNum = UnityEngine.Random.Range(0, 2);
        for (int i=0; i<obstacleWaterNum; i++)
        {
            float y = goGround.groundHeight - 0.8f;
            float width = goCollider.size.x / 2 - 1;
            float left = go.transform.position.x - width + 8f;
            float right = go.transform.position.x + width - 8f;
            float x = UnityEngine.Random.Range(left, right);
            GameObject water = Instantiate(obstacleWater.gameObject);
            water.transform.position = new Vector2(x, y);
        }

        int obstacleEnemyNum = UnityEngine.Random.Range(0, 2);
        for (int i=0; i<obstacleEnemyNum; i++)
        {
            float y = goGround.groundHeight + 1;
            float width = goCollider.size.x / 2 - 1;
            float left = go.transform.position.x - width;
            float right = go.transform.position.x + width;
            float x = UnityEngine.Random.Range(left, right);
            GameObject tegiac = Instantiate(obstacleEnemy.gameObject);
            tegiac.transform.position = new Vector2(x, y);
        }
    }
}
