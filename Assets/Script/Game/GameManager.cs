using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public InputField commandInput; // キーボード入力用
    public Text logText;
    public PlayerController player;

    private bool isDead = false;

    private void Start()
    {
        // 最初から InputField にフォーカス
        commandInput.ActivateInputField();
    }

    private void Update()
    {
        if (!isDead && player.transform.position.y < -10f)
            PlayerDie();

        // Enter または Shift でコマンド実行
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            ExecuteCommand();
        }
    }

    public void ExecuteCommand()
    {
        if (isDead) return;

        string input = commandInput.text;
        if (string.IsNullOrWhiteSpace(input))
        {
            Log("コマンドが空です");
            return;
        }

        Log("▶ " + input);

        List<CommandInterpreter.Command> commands = CommandInterpreter.Interpret(input);
        player.SetCommands(commands);

        commandInput.text = "";
        commandInput.ActivateInputField(); // 実行後もすぐ入力可能に
    }

    private void Log(string msg)
    {
        if (logText != null)
            logText.text += msg + "\n";
    }

    public void PlayerDie()
    {
        if (isDead) return;
        isDead = true;
        Log("プレイヤー死亡…");
        Invoke("Restart", 1.5f);
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
