using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Các biến công khai cho thuộc tính và trạng thái của người chơi
    public float gravity; // Lực hấp dẫn tác động lên người chơi
    public Vector2 velocity; // Vận tốc hiện tại của người chơi
    public float maxXVelocity = 100; // Vận tốc ngang tối đa
    public float maxAcceleration = 10; // Gia tốc tối đa
    public float acceleration = 10; // Gia tốc hiện tại
    public float distance = 0; // Khoảng cách đã di chuyển
    public float jumpVelocity = 20; // Vận tốc khi nhảy
    public float groundHeight = 10; // Chiều cao của mặt đất
    public bool isGrounded = false; // Người chơi có đang trên mặt đất không
    public bool isHoldingJump = false; // Người chơi có đang giữ phím nhảy không
    public float maxHoldJumpTime = 0.4f; // Thời gian tối đa có thể giữ phím nhảy
    public float maxMaxHoldJumpTime = 0.4f; // Thời gian tối đa có thể giữ phím nhảy (giá trị tối đa)
    public float holdJumpTimer = 0.0f; // Bộ đếm thời gian giữ phím nhảy
    public float jumpGroundThreshold = 1; // Ngưỡng để coi người chơi là đang trên mặt đất

    // Start được gọi trước khi khung hình đầu tiên được cập nhật
    void Start()
    {
        // Mã khởi tạo có thể đặt ở đây
    }

    // Update được gọi một lần mỗi khung hình
    void Update()
    {
        // Lấy vị trí hiện tại của người chơi
        Vector2 pos = transform.position;
        // Tính khoảng cách từ người chơi tới mặt đất
        float groundDistance = Mathf.Abs(pos.y - groundHeight);

        // Kiểm tra nếu người chơi đang trên mặt đất hoặc trong ngưỡng nhảy
        if (isGrounded || groundDistance <= jumpGroundThreshold)
        {
            // Nếu phím Space được nhấn, bắt đầu nhảy
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isGrounded = false; // Người chơi không còn trên mặt đất
                velocity.y = jumpVelocity; // Đặt vận tốc dọc cho nhảy
                isHoldingJump = true; // Bắt đầu giữ phím nhảy
                holdJumpTimer = 0; // Đặt lại bộ đếm thời gian giữ phím nhảy
            }
        }

        // Nếu phím Space được thả ra, dừng giữ phím nhảy
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isHoldingJump = false;
        }
    }

    // FixedUpdate được gọi ở khoảng thời gian cố định và được dùng cho tính toán vật lý
    private void FixedUpdate()
    {
        // Lấy vị trí hiện tại của người chơi
        Vector2 pos = transform.position;

        // Nếu người chơi không đang trên mặt đất
        if (!isGrounded)
        {
            // Nếu đang giữ phím nhảy, tăng bộ đếm thời gian giữ phím nhảy
            if (isHoldingJump)
            {
                holdJumpTimer += Time.fixedDeltaTime;
                // Nếu bộ đếm thời gian giữ phím nhảy vượt quá thời gian tối đa, dừng giữ phím nhảy
                if (holdJumpTimer >= maxHoldJumpTime)
                {
                    isHoldingJump = false;
                }
            }

            // Cập nhật vị trí dọc của người chơi dựa trên vận tốc
            pos.y += velocity.y * Time.fixedDeltaTime;
            // Nếu không giữ phím nhảy, áp dụng lực hấp dẫn
            if (!isHoldingJump)
            {
                velocity.y += gravity * Time.fixedDeltaTime;
            }

            // Thực hiện raycast để phát hiện va chạm với mặt đất
            Vector2 rayOrigin = new Vector2(pos.x + 0.7f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
            // Nếu raycast chạm vào một collider
            if (hit2D.collider != null)
            {
                // Kiểm tra nếu collider là đối tượng mặt đất
                Ground ground = hit2D.collider.GetComponent<Ground>();
                if (ground != null)
                {
                    // Đặt chiều cao mặt đất, vị trí của người chơi, và đặt lại vận tốc dọc
                    groundHeight = ground.groundHeight;
                    pos.y = groundHeight;
                    velocity.y = 0;
                    isGrounded = true; // Người chơi bây giờ đang trên mặt đất
                }
            }
            // Vẽ tia ray debug màu xanh lá cây
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.green);
        }

        // Cập nhật khoảng cách đã di chuyển của người chơi
        distance += velocity.x * Time.fixedDeltaTime;

        // Nếu người chơi đang trên mặt đất
        if (isGrounded)
        {
            // Tính tỷ lệ của vận tốc hiện tại với vận tốc tối đa
            float velocityRatio = velocity.x / maxXVelocity;
            // Điều chỉnh gia tốc dựa trên tỷ lệ vận tốc
            acceleration = maxAcceleration * (1 - velocityRatio);
            // Điều chỉnh thời gian giữ phím nhảy tối đa dựa trên tỷ lệ vận tốc
            maxHoldJumpTime = maxMaxHoldJumpTime * velocityRatio;

            // Cập nhật vận tốc ngang của người chơi
            velocity.x += acceleration * Time.fixedDeltaTime;
            // Giới hạn vận tốc đến vận tốc tối đa
            if (velocity.x >= maxXVelocity)
            {
                velocity.x = maxXVelocity;
            }

            // Thực hiện raycast để kiểm tra nếu người chơi vẫn còn trên mặt đất
            Vector2 rayOrigin = new Vector2(pos.x - 0.7f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
            // Nếu raycast không chạm vào bất kỳ collider nào, người chơi không còn trên mặt đất
            if (hit2D.collider == null)
            {
                isGrounded = false;
            }
            // Vẽ tia ray debug màu vàng
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.yellow);
        }

        // Cập nhật vị trí của người chơi
        transform.position = pos;
    }
}
