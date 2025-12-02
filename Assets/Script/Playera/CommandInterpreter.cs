using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class CommandInterpreter
{
    public struct Command
    {
        public CommandType type;
        public float value;
    }

    public enum CommandType
    {
        MoveRight,
        MoveLeft,
        JumpSmall,
        JumpBig,
        JumpDouble,
        JumpNormal,
        Run,
        Crouch,
        Slow
    }

    public static List<Command> Parse(string input)
    {
        return Interpret(input);
    }

    public static List<Command> Interpret(string input)
    {
        input = input.ToLower();
        List<Command> result = new List<Command>();

        // 移動
        if (input.Contains("右"))
            result.Add(new Command { type = CommandType.MoveRight, value = ExtractNumber(input) });
        if (input.Contains("左"))
            result.Add(new Command { type = CommandType.MoveLeft, value = ExtractNumber(input) });

        // ジャンプ
        if (input.Contains("二段"))
            result.Add(new Command { type = CommandType.JumpDouble });
        else if (input.Contains("大ジャンプ"))
            result.Add(new Command { type = CommandType.JumpBig });
        else if (input.Contains("小ジャンプ"))
            result.Add(new Command { type = CommandType.JumpSmall });
        else if (input.Contains("ジャンプ") || input.Contains("上に") || input.Contains("上へ"))
            result.Add(new Command { type = CommandType.JumpNormal });

        // その他
        if (input.Contains("走") || input.Contains("ダッシュ"))
            result.Add(new Command { type = CommandType.Run });
        if (input.Contains("しゃが"))
            result.Add(new Command { type = CommandType.Crouch });
        if (input.Contains("ゆっくり"))
            result.Add(new Command { type = CommandType.Slow });

        return result;
    }

    private static int ExtractNumber(string input)
    {
        Match m = Regex.Match(input, @"\d+");
        if (m.Success) return int.Parse(m.Value);
        return 1;
    }
}
