using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaesarCipher
{
	public static class ListShifter
	{
		public static List<T> ShiftLeft<T>(this List<T> list, int shiftBy)
		{
			if (list.Count <= shiftBy)
			{
				return list;
			}

			var result = list.GetRange(shiftBy, list.Count - shiftBy);
			result.AddRange(list.GetRange(0, shiftBy));
			return result;
		}

		public static List<T> ShiftRight<T>(this List<T> list, int shiftBy)
		{
			if (list.Count <= shiftBy)
			{
				return list;
			}

			var result = list.GetRange(list.Count - shiftBy, shiftBy);
			result.AddRange(list.GetRange(0, list.Count - shiftBy));
			return result;
		}
	}
}
