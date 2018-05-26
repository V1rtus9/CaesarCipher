using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CaesarCipher
{
	interface ICipher
	{
		void Encode(string input, ref StringBuilder output);
		void Decode(string input, ref StringBuilder output);
		void Crack(string input, ref StringBuilder output);
	}
}
