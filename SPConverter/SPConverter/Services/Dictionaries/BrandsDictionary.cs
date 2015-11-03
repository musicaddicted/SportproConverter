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
        private static BrandsDictionary _instance;

        public static string DictionaryPath = Path.Combine(Application.StartupPath, "Dictionaries", "brands.csv");

        public List<Brand> Brands { get; }

        public BrandsDictionary()
        {
            Brands = new List<Brand>();

            string[] allBrandsStrings = File.ReadAllLines(DictionaryPath);


            for (int i = 1; i < allBrandsStrings.Length; i++)
            {
                string brandLine = allBrandsStrings[i];
                if (string.IsNullOrEmpty(brandLine))
                    continue;

                string[] splitStrings = brandLine.Split(new[] {';'}, StringSplitOptions.None);

                Brands.Add(new Brand
                {
                    Name = splitStrings[0],
                    BlocksCountInArticul = int.Parse(splitStrings[1])
                });
            }
        }

        // TODO вернуть!
        public static BrandsDictionary Instance => new BrandsDictionary(); //_instance ?? (_instance = new BrandsDictionary());
    }
}
