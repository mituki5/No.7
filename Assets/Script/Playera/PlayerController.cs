using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移動")]
    public float moveSpeed = 2f;
    public float runMultiplier = 1.5f;
    public float slowMultiplier = 0.5f;

    [Header("ジャンプ")]
    public float smallJumpPower = 5f;
    public float bigJumpPower = 8f;
    public float doubleJumpPower = 6f;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool canDoubleJump = false;
    private bool isExecuting = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(float step, bool right = true)
    {
        float dir = right ? 1f : -1f;
        rb.linearVelocity = new Vector2(step * dir, rb.linearVelocity.y);
    }

    public void Jump(string type)
    {
        if (!isGrounded) return;

        switch (type)
        {
            case "small":
            case "normal":
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, smallJumpPower); break;
            case "big":
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, bigJumpPower); break;
            case "double":
                if (canDoubleJump)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpPower);
                    canDoubleJump = false;
                }
                break;
        }
        canDoubleJump = true;
    }

    public void Run() { moveSpeed *= runMultiplier; }
    public void Slow() { moveSpeed *= slowMultiplier; }
    public void Crouch() { transform.localScale = new Vector3(1, 0.5f, 1); }

    public void Die()
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = new Vector3(0, 1, 0);
        Debug.Log("プレイヤー死亡");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
        if (collision.gameObject.CompareTag("Enemy"))
            Die();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }

    // -----------------------------
    // コマンド実行用
    // -----------------------------
    public void SetCommands(List<CommandInterpreter.Command> commands)
    {
        StartCoroutine(ExecuteCommands(commands));
    }

    private IEnumerator ExecuteCommands(List<CommandInterpreter.Command> commands)
    {
        if (isExecuting) yield break;
        isExecuting = true;

        foreach (var cmd in commands)
        {
            switch (cmd.type)
            {
                case CommandInterpreter.CommandType.MoveRight:
                    Move(moveSpeed, true);
                    yield return new WaitForSeconds(cmd.value * 0.5f);
                    rb.linearVelocity = Vector2.zero;
                    break;
                case CommandInterpreter.CommandType.MoveLeft:
                    Move(moveSpeed, false);
                    yield return new WaitForSeconds(cmd.value * 0.5f);
                    rb.linearVelocity = Vector2.zero;
                    break;
                case CommandInterpreter.CommandType.JumpSmall:
                    Jump("small"); yield return new WaitForSeconds(0.5f); break;
                case CommandInterpreter.CommandType.JumpNormal:
                    Jump("normal"); yield return new WaitForSeconds(0.5f); break;
                case CommandInterpreter.CommandType.JumpBig:
                    Jump("big"); yield return new WaitForSeconds(0.5f); break;
                case CommandInterpreter.CommandType.JumpDouble:
                    Jump("double"); yield return new WaitForSeconds(0.5f); break;
                case CommandInterpreter.CommandType.Run:
                    Run(); break;
                case CommandInterpreter.CommandType.Slow:
                    Slow(); break;
                case CommandInterpreter.CommandType.Crouch:
                    Crouch(); break;
            }
        }

        isExecuting = false;
    }
}
