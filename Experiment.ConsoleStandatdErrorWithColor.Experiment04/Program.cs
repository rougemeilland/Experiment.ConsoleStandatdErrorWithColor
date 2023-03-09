using System;

namespace Experiment.ConsoleStandatdErrorWithColor.Experiment04
{
    // Qiita 投稿用 サンプルプログラム その2
    internal class Program
    {
        private static void Main(string[] args)
        {
            PrintInformationMessage("波〇砲 エネルギー充填率 0%...");
            Console.WriteLine("寿限無 寿限無 五劫の擦り切れ");
            PrintInformationMessage("充填率 20%...");
            Console.WriteLine("海砂利水魚の水行末");
            PrintInformationMessage("充填率 40%...");
            Console.WriteLine("雲来末 風来末");
            PrintInformationMessage("充填率 60%...");
            Console.WriteLine("食う寝るところに住むところ");
            PrintInformationMessage("充填率 80%...");
            Console.WriteLine("やぶら小路の藪柑子");
            PrintInformationMessage("充填率 100%...");
            Console.WriteLine("パイポ パイポ");
            PrintInformationMessage("充填率 120%...");
            PrintWarningMessage("艦長、もう危険です!");
            PrintInformationMessage("充填率 140%...");
            Console.WriteLine("パイポのシューリンガン");
            PrintInformationMessage("充填率 160%...");
            Console.WriteLine("シューリンガンのグーリンダイ");
            PrintInformationMessage("充填率 180%...");
            Console.WriteLine("グーリンダイのポンポコピーのポンポコナの");
            PrintInformationMessage("充填率 200%...");
            Console.WriteLine("長久命の長助");
            PrintErrorMessage("宇宙戦艦ヤ〇トは轟沈しました");
        }

        private static void PrintInformationMessage(string message)
        {
            Console.Error.WriteLine(message);
        }

        private static void PrintWarningMessage(string message)
        {
            // 前景色を黄色に変更するコードを標準エラー出力に出力
            Console.Error.Write(ConsoleColor.Yellow.ToForeGroundColorAnsiEscapeCode());

            Console.Error.WriteLine(message);

            // 前景色を初期状態に戻すコードを標準エラー出力に出力
            Console.Error.Write(((ConsoleColor)(-1)).ToForeGroundColorAnsiEscapeCode());
        }

        private static void PrintErrorMessage(string message)
        {
            // 前景色を赤に変更するコードを標準エラー出力に出力
            Console.Error.Write(ConsoleColor.Red.ToForeGroundColorAnsiEscapeCode());

            Console.Error.WriteLine(message);

            // 前景色を初期状態に戻すコードを標準エラー出力に出力
            Console.Error.Write(((ConsoleColor)(-1)).ToForeGroundColorAnsiEscapeCode());
            Console.Beep();
        }
    }
}
