using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            int[] indexes = new int[plainText.Length];
            cipherText = cipherText.ToUpper();
            char[] key = new char[26];
            int ascii;
            char temp;
            plainText = plainText.ToUpper();
            char[] alphs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            for (int i = 0; i < 26; i++)
            {

                key[i] = '@';

            }
            for (int i = 0; i < plainText.ToCharArray().Length; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    if (plainText[i] == alphs[j])
                    {
                        key[j] = cipherText[i];
                        indexes[i] = j;
                    }

                }
            }
            int @in = 0;
            for (int j = 0; j < 26; j++)
            {
                if (@in > 25)
                    break;

                if (!cipherText.Contains(alphs[j]) && key[@in].CompareTo('@') == 0)
                {
                    key[@in] = alphs[j];
                }
                else if (!cipherText.Contains(alphs[j]) && key[@in].CompareTo('@') != 0)
                {
                    j--;

                }

                @in++;
            }
            return new string(key);
        }

        public string Decrypt(string cipherText, string key)
        {
            string upcypher = cipherText.ToUpper();
            string upkey = key.ToUpper();
            char[] arr2 = new char[cipherText.Length];
            for (int i = 0; i < cipherText.Length; i++)
            {

                int j = upkey.IndexOf(upcypher[i]) + 97;
                arr2[i] = (char)j;


            }

            return new string(arr2);

            // throw new NotImplementedException();

        }

        public string Encrypt(string plainText, string key)
        {
            // throw new NotImplementedException();
            char[] arr = new char[plainText.Length];
            for (int i = 0; i < plainText.Length; i++)
            {


                int j = plainText[i] - 97;
                arr[i] = key[j];

            }

            return new string(arr);
        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        public string AnalyseUsingCharFrequency(string cipher)
        {
            throw new NotImplementedException();
        }
    }
}