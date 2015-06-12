using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexColorConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter a color to convert: ");
            var colorText = Console.ReadLine();
            var converter = new ColorConverter();
            var type = converter.EvaluateInput(input: colorText);
            switch(type)
            {
                case ColorInputTypes.Hex:
                    var rgb = converter.ToRgb(colorText);
                    Console.WriteLine("RGB: {0}", rgb);
                    break;
                case ColorInputTypes.RGB:
                    var hex = converter.ToHex(colorText);
                    Console.WriteLine("Hex: {0}", hex);
                    break;
                default:
                    Console.WriteLine("Unknown input format");
                    break;
            }

            Console.Write("Again? ");
            var again = Console.ReadLine();

            if (again == "y")
            {
                Program.Main(new string[0]);
            } else
            {
                Console.WriteLine("All done!");
                var final = Console.ReadLine();
            }

            
        }
    }

    public class ColorConverter
    {
        public ColorInputTypes EvaluateInput(string input)
        {
            if (input == null) return ColorInputTypes.Unknown;
            if (input.StartsWith("#"))
            {
                return ColorInputTypes.Hex;
            }
            if (input.Contains(","))
            {
                return ColorInputTypes.RGB;
            }
            return ColorInputTypes.Unknown;
        }

        public string ToHex(string rgb)
        {
            var validateType = this.EvaluateInput(rgb);
            if (validateType != ColorInputTypes.RGB) throw new ArgumentException("RGB format color expected. i.e.: 128,128,128 -- Please try again.");

            var parts = rgb.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(t => Convert.ToInt32(t))
                            .Select(t => t.ToString("X"))
                            .Select(t => t.Length == 1 ? "0" + t : t);


            return "#" + String.Join("", parts);
        } 

        public string ToRgb(string hex)
        {
            var validateType = this.EvaluateInput(hex);
            if (validateType != ColorInputTypes.Hex) throw new ArgumentException("Hex format color expected. i.e.: #C0C0C0 -- Please try again.");

            var withoutHash = hex.Substring(1);
            string cleaned = string.Empty;
            switch(withoutHash.Length)
            {
                case 2:
                    cleaned = withoutHash + withoutHash + withoutHash;
                    break;
                case 3:
                    cleaned = withoutHash + withoutHash;
                    break;
                case 6:
                    cleaned = withoutHash;
                    break;
                default: 
                    throw new ArgumentException("Unknown Hex Format");
            }

            var r = int.Parse(cleaned.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            var g = int.Parse(cleaned.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            var b = int.Parse(cleaned.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            return string.Join(",", r,g,b);
        }
    }

    public enum ColorInputTypes
    {
        Hex, RGB, Unknown
    }
}
