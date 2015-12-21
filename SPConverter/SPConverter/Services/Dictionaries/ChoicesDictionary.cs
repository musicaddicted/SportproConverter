using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SPConverter.Model;

namespace SPConverter.Services.Dictionaries
{
    public class ChoicesDictionary
    {
        public static string DictionaryPath = Path.Combine(Application.StartupPath, "Dictionaries", "choices.csv");

        private static ChoicesDictionary _instance;

        public static ChoicesDictionary Instance => _instance ?? (_instance = new ChoicesDictionary());

        public List<Choice> Choices { get; private set; }

        public ChoicesDictionary()
        {
            Init();
        }

        private void Init()
        {
            Choices = new List<Choice>();

            string[] allStrings = File.ReadAllLines(DictionaryPath, Encoding.UTF8);

            for (int i = 1; i < allStrings.Length; i++)
            {
                string choiceLine = allStrings[i];
                if (string.IsNullOrEmpty(choiceLine) || choiceLine.StartsWith("//"))
                    continue;

                string[] splitStrings = choiceLine.Split(new[] { ';' }, StringSplitOptions.None);

                Choices.Add(new Choice
                {
                    OriginalCategory = splitStrings[0],
                    ResultCategory = splitStrings[1]
                });
            }
        }

        public void Refresh()
        {
            Init();
        }

        public void Add(Choice choise)
        {
            File.AppendAllText(DictionaryPath, $"{choise.OriginalCategory};{choise.ResultCategory}\r\n",Encoding.UTF8);
            Choices.Add(choise);
        }
    }

    public class Choice
    {
        public string OriginalCategory;

        public string ResultCategory;

        public override string ToString()
        {
            return $"{OriginalCategory};{ResultCategory}";
        }
    }
}
