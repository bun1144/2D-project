// CharacterSfx.cs
using UnityEngine;

public class CharacterSfx : MonoBehaviour
{
    [Header("Clips")]
    public AudioClip jumpClip;
    public AudioClip attackClip;

    [Header("Cooldown กันสแปมเสียง")]
    public float minInterval = 0.03f; // ลดให้สั้นลงหน่อย

    private float lastJumpTime = -999f;
    private float lastAttackTime = -999f;

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
            PlayJump();

        if (Input.GetButtonDown("Fire1"))
            PlayAttack();
    }

    // ถ้าใช้ Animation Event ให้ผูก Event มาที่สองเมธอดนี้แทนก็ได้
    public void OnAttackSwingEvent() => PlayAttack();
    public void OnJumpEvent()        => PlayJump();

    public void PlayJump()
    {
        if (Time.time - lastJumpTime < minInterval) return;
        lastJumpTime = Time.time;

        if (jumpClip != null)
            AudioManager.Instance.PlaySfx(jumpClip); // 2D ชัวร์
        // หรือแบบกำหนดเอง: AudioManager.Instance.PlaySfxAt(jumpClip, transform.position, false);
    }

    public void PlayAttack()
    {
        if (Time.time - lastAttackTime < minInterval) return;
        lastAttackTime = Time.time;

        if (attackClip != null)
            AudioManager.Instance.PlaySfx(attackClip); // 2D ชัวร์
        // หรือแบบกำหนดเอง: AudioManager.Instance.PlaySfxAt(attackClip, transform.position, false);
    }
}
