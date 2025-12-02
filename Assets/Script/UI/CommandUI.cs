using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CommandUI : MonoBehaviour
{
    public InputField inputField;
    public PlayerController player;

    private void Start()
    {
        // 最初から InputField にフォーカス
        inputField.ActivateInputField();
    }

    private void Update()
    {
        // Enter または Shift でコマンド実行
        if (Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.LeftShift) ||
            Input.GetKeyDown(KeyCode.RightShift))
        {
            OnExecute();
        }
    }

    private void OnExecute()
    {
        string text = inputField.text;
        if (string.IsNullOrWhiteSpace(text)) return;

        List<CommandInterpreter.Command> commands = CommandInterpreter.Interpret(text);
        player.SetCommands(commands);

        // 実行後もすぐ入力できるようにフォーカスを戻す
        inputField.text = "";
        inputField.ActivateInputField();
    }
}
