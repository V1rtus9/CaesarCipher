using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaesarCipher
{
	public sealed class CaesarShiftCipher : ICipher
	{
		/// <summary>
		/// 
		/// </summary>
		private static CaesarShiftCipher instance;
		/// <summary>
		/// 
		/// </summary>
		private string alphabet = "A B C D E F G H I J K L M N O P Q R S T U V W X Y Z"; // 26	
		/// <summary>
		/// 
		/// </summary>
		private List<char> alphabetSymbols = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
		/// <summary>
		/// 
		/// </summary>
		private List<char> shiftedSymbols = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
		//{ 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
		/// <summary>
		///  letterFrequency are the relative letter frequencies in the English language. The values are taken from <http://en.wikipedia.org/wiki/Letter_frequency>. They are expressed as decimals and sum to 1.0.
		/// </summary>
		private List<double> letterFrequency = new List<double>() { 0.08167, 0.01492, 0.02782, 0.04253, 0.12702, 0.02228, 0.02015, 0.06094, 0.06966, 0.00153, 0.00772, 0.04025, 0.02406, 0.06749, 0.07507, 0.01929, 0.00095, 0.05987, 0.06327, 0.09056, 0.02758, 0.00978, 0.02360, 0.00150, 0.01974, 0.00074 };
		/// <summary>
		/// 
		/// </summary>
		private int previousShiftValue = 0;

		/// <summary>
		/// Private constructor
		/// </summary>
		//private CaesarShiftCipher() { }
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static CaesarShiftCipher Instance()
		{
			if (instance == null)
			{
				return instance = new CaesarShiftCipher();
			}

			return instance;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		/// <param name="output"></param>
		/// <param name="shift"></param>
		public void Decode(string input, ref StringBuilder output, int shift)
		{
			if (!CheckShift(shift) || shiftedSymbols.Count == 0)
				ShiftList(shift);

			foreach (Char item in input)
			{
				if (Char.IsWhiteSpace(item) || Char.IsPunctuation(item))
				{
					output.Append(item);
					continue;
				}

				try
				{
					output.Append(shiftedSymbols[alphabetSymbols.IndexOf(item)]);
				}
				catch (ArgumentOutOfRangeException)
				{
					output.Append("[$]");
				}

			}

		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		/// <param name="output"></param>
		public void Decode(string input, ref StringBuilder output) { }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		/// <param name="output"></param>
		/// <param name="shift"></param>
		public void Encode(string input, ref StringBuilder output, int shift)
		{
			if (!CheckShift(shift) || shiftedSymbols.Count == 0)
				ShiftList(shift);

			foreach (Char item in input)
			{
				if (Char.IsWhiteSpace(item) || Char.IsPunctuation(item))
				{
					output.Append(item);
					continue;
				}

				try
				{
					output.Append(alphabetSymbols[shiftedSymbols.IndexOf(item)]);
				}
				catch (ArgumentOutOfRangeException)
				{
					output.Append("$");
				}

			}

		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		/// <param name="output"></param>
		public void Encode(string input, ref StringBuilder output) { }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		/// <param name="output"></param>
		public void Crack(string input, ref StringBuilder output, ref int shift)
		{

			List<double> values = new List<double>();
			StringBuilder inputValue = new StringBuilder();

			//shiftedSymbols = alphabetSymbols;

			for (int i = 0; i < 26; i++)
			{
				ShiftList(i);
				inputValue.Clear();
				Decode(input, ref inputValue, i);

				values.Add(ChiSqr(RelativeFrequencies(inputValue.ToString()), i));
			}

			shift = values.IndexOf(values.Min());
			Decode(input, ref output, shift);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		/// <param name="output"></param>
		public void Crack(string input, ref StringBuilder output) { }
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private string[] GetAlphabetArray()
		{
			return alphabet.Split(' ');
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="shift"></param>
		private void ShiftList(int shift)
		{
			shiftedSymbols = alphabetSymbols.ShiftRight(shift);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="shift"></param>
		/// <returns></returns>
		private bool CheckShift(int shift)
		{
			if (previousShiftValue == shift)
				return true;

			previousShiftValue = shift;

			return false;
		}

		private double[] RelativeFrequencies(string input)
		{
			double[] charsArray = new double[Char.MaxValue];

			foreach (Char item in input)
			{
				charsArray[(int)item]++;
			}

			double lenght = charsArray.Count(s => s != 0);

			for (int i = 0; i < charsArray.Length; i++)
			{
				if (charsArray[i] == 0 || Char.IsWhiteSpace((Char)i) || Char.IsPunctuation((Char)i))
					continue;

				charsArray[i] = Math.Round(charsArray[i] / lenght, 2);
			}

			return charsArray;
		}

		public double ChiSqr(double[] relativeFreq, int rot)
		{
			double sum = 0;

			for (int i = 0; i < relativeFreq.Length; i++)
			{
				if (relativeFreq[i] == 0 || Char.IsWhiteSpace((Char)i) || Char.IsPunctuation((Char)i))
					continue;

				double expected = letterFrequency[alphabetSymbols.IndexOf(((Char)i))];

				sum += Math.Pow((relativeFreq[i] - expected), 2) / expected;
			}

			return Math.Sqrt(sum);
		}

		//JGNNQ FWFG, JQY CTG AQW?
	}
}
