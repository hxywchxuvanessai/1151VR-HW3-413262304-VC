
using UnityEngine;
using UnityEngine.UI;

public class RouletteController : MonoBehaviour
{
    // 輪盤旋轉速度
    float rotSpeed = 0f;

    // 蓄力時間
    float charge = 0f;
    float maxCharge = 3f;

    // 是否正在蓄力
    bool isCharging = false;

    // 蓄力進度條
    public Slider chargeBar;

    // 輪盤原本的大小
    Vector3 originalScale;

    // 輪盤圖片
    SpriteRenderer spriteRenderer;

    void Start()
    {
        Application.targetFrameRate = 60;

        originalScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();

        chargeBar.value = 0f;
    }

    void Update()
    {
        // 按下滑鼠開始蓄力
        if (Input.GetMouseButtonDown(0) && rotSpeed <= 0f)
        {
            isCharging = true;
            charge = 0f;
        }

        // 持續按住滑鼠
        if (Input.GetMouseButton(0) && isCharging)
        {
            charge += Time.deltaTime;
            charge = Mathf.Clamp(charge, 0f, maxCharge);

            float power = charge / maxCharge;

            // 更新進度條
            chargeBar.value = power;

            // 輪盤蓄力時微微放大、縮小
            float pulse = 1f + power * 0.1f
                + Mathf.Sin(Time.time * 12f) * 0.02f;

            transform.localScale = originalScale * pulse;

            // 蓄力時輪盤顏色逐漸變暖
            spriteRenderer.color = Color.Lerp(
                Color.white,
                new Color(1f, 0.75f, 0.75f),
                power
            );
        }

        // 放開滑鼠開始旋轉
        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            isCharging = false;

            rotSpeed = 10f + charge * 15f;
            // 恢復輪盤大小及顏色
            transform.localScale = originalScale;
            spriteRenderer.color = Color.white;
        }

        // 讓輪盤旋轉
        transform.Rotate(0, 0, rotSpeed);

        // 逐漸減速
        rotSpeed *= Mathf.Pow(0.96f, Time.deltaTime * 60f);

        if (rotSpeed < 0.1f)
        {
            rotSpeed = 0f;
        }
    }
}
