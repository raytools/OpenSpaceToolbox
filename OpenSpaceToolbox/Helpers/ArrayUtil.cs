using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSpaceToolbox.Helpers {
   public static class ArrayUtil {

      /// <summary>
      /// Sets bits in a byte array from index "from" to index "to" (inclusive)
      /// </summary>
      /// <param name="array">The array</param>
      /// <param name="from">Start index, inclusive</param>
      /// <param name="to">End index, inclusive</param>
      /// <param name="bit">Set/Unset</param>
      public static void SetBits(this byte[] array, int from, int to, bool bit)
      {
         for (int i = from; i <= to; i++) {
            int byteIndex = i / 8;
            array[byteIndex] = array[byteIndex].SetBit(i % 8, bit);
         }
      }


      /// <summary>
      /// Returns if the specified bit is set
      /// </summary>
      public static bool IsBitSet(this byte[] array, int index)
      {
         int byteIndex = index / 8;
         int bitIndex = index % 8;

         if (array == null || array.Length <= byteIndex) {
            return false;
         }

         int check = (array[byteIndex] & (1 << bitIndex));
         Debug.WriteLine($"array[byteIndex] = {array[byteIndex]}");
         return (array[byteIndex] & (1 << bitIndex)) != 0;
      }

      /// <summary>
      /// Sets the specified bit index in an integer
      /// </summary>
      /// <param name="value">The integer</param>
      /// <param name="bitIndex">The bit index</param>
      /// <param name="bitValue">True for set or false for unset</param>
      public static int SetBit(this int value, int bitIndex, bool bitValue)
      {
         if (bitIndex < 0 || bitIndex > 31) {
            throw new ArgumentOutOfRangeException($"BitIndex must be between 0 and 31!");
         }

         int mask = 1 << bitIndex;
         if (bitValue)
            value |= mask;
         else
            value &= ~mask;

         return value;
      }

      /// <summary>
      /// Sets the specified bit index in a byte
      /// </summary>
      /// <param name="value">The integer</param>
      /// <param name="bitIndex">The bit index</param>
      /// <param name="bitValue">True for set or false for unset</param>
      public static byte SetBit(this byte value, int bitIndex, bool bitValue)
      {
         if (bitIndex < 0 || bitIndex > 7) {
            throw new ArgumentOutOfRangeException($"BitIndex must be between 0 and 7!");
         }

         return (byte)((int)value).SetBit(bitIndex, bitValue);
      }

   }
}
