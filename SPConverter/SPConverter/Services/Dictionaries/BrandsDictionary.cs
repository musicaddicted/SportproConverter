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
    public class BrandsDictionary
    {
        public static string DictionaryPath = Path.Combine(Application.StartupPath, "Dictionaries", "brands.csv");

        private static BrandsDictionary _instance;
        public List<Brand> Brands { get; private set; }

        public BrandsDictionary()
        {
            Init();
        }

        private void Init()
        {
            Brands = new List<Brand>();

            string[] allBrandsStrings = File.ReadAllLines(DictionaryPath, Encoding.UTF8);

            for (int i = 1; i < allBrandsStrings.Length; i++)
            {
                string brandLine = allBrandsStrings[i];
                if (string.IsNullOrEmpty(brandLine))
                    continue;

                string[] splitStrings = brandLine.Split(new[] { ';' }, StringSplitOptions.None);

                Brands.Add(new Brand
                {
                    Name = splitStrings[0],
                    BlocksCountInArticul = int.Parse(splitStrings[1])
                });
            }
        }

        public static BrandsDictionary Instance => _instance ?? (_instance = new BrandsDictionary());
    }
}
