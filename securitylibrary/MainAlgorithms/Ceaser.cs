using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        public string Encrypt(string plainText, int key)
        {
            int ascii;
            char temp;
            plainText = plainText.ToUpper();
            string Encrypted = "";
            char[] alphs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            for (int i = 0; i < plainText.ToCharArray().Length; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    if (plainText[i] == alphs[j])
                    {
                        ascii = 65 + ((key + j) % 26);
                        Encrypted += (char)ascii;
                        break;
                    }
                }
            }
            return Encrypted;
        }

        public string Decrypt(string cipherText, int key)
        {
            int ascii;
            char temp;
            cipherText = cipherText.ToUpper();
            string Decrypted = "";
            char[] alphs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            for (int i = 0; i < cipherText.ToCharArray().Length; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    if (cipherText[i] == alphs[j])
                    {
                        if (j < key)
                        {
                            ascii = 65 + 26 - key + j;
                            Decrypted += (char)ascii;
                            break;
                        }
                        ascii = 65 - key + j;
                        if (ascii < 0)
                            ascii += 26;
                        Decrypted += (char)ascii;
                        break;
                    }
                }
            }
            return Decrypted;
        }

        public int Analyse(string plainText, string cipherText)
        {
            int ascii;
            int key = -2;
            plainText = plainText.ToUpper();
            cipherText = cipherText.ToUpper();
            int map1 = 0;
            int map2 = 0;
            char[] alphs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            int diff;

            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    if (plainText[i] == alphs[j])
                    {
                        map1 = j;
                    }
                    if (cipherText[i] == alphs[j])
                    {
                        map2 = j;
                    }
                }
            }

            diff = map2 - map1;
            if (diff < 0)
            {
                key = (26 - map1) + map2;
            }
            else
                key = diff;
            return key;
        }
    }
}