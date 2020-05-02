using musicDriverInterface;
using System.Collections.Generic;
using System.Linq;

namespace mucomDotNET.Compiler {
    public class MucomSetting {
        public List<string> Files;

        public int Channels = 44;

        public bool ShowUsage = false;

        public MucomSetting() {
            Files = new List<string>();
        }

        public void Parse(string[] args) {
            if (args == null) {
                ShowUsage = true;
                return;
            }

            for(var i = 0; i < args.Length; i++) {
                var t = args[i];

                // ファイルなど
                if (t[0] != '-') {
                    Files.Add(t); continue;
                }

                if (t[1] == '-') i = LongOptParse(t, i, args);
                else i = ShortOptParse(t, i, args);
            }

            if (!Validate()) ShowUsage = true;

            if (Files.Count == 0) ShowUsage = true;
        }

        public void ShowOptionHelp() {
            Log.WriteLine(LogLevel.INFO, "オプション");
            Log.WriteLine(LogLevel.INFO, Formatter("-?, -h, --help", "この文章"));
            Log.WriteLine(LogLevel.INFO, Formatter("-c <n>, --channels <n>", "チャンネル数の指定(11,22,44のいずれか)"));
        }

        private string Formatter(string Text, string Detail) {
            return $"{Text,-32} : {Detail}";
        }

        private bool Validate() {
            var Result = true;
            int[] ChannelOption = { 11, 22, 44 };
            if (!ChannelOption.Contains(Channels)) {
                Log.WriteLine(LogLevel.INFO, "チャンネル数は11, 22, 44のいずれかで指定できます");
                Result = false;
            }

            return Result;
        }

        private int LongOptParse(string t, int i, string[] args) {
            var OptionName = t.Substring(2);
            switch(OptionName) {
                case "channel":
                    i++;
                    Channels = int.Parse(args[i]);
                    break;
                case "help":
                    ShowUsage = true;
                    break;
            }
            return i;
        }

        private int ShortOptParse(string t, int i, string[] args) {
            var OptionName = t.Substring(1);
            switch (OptionName) {
                case "c":
                    i++;
                    Channels = int.Parse(args[i]);
                    break;
                case "?":
                case "h":
                    ShowUsage = true;
                    break;
            }
            return i;
        }

    }

}
