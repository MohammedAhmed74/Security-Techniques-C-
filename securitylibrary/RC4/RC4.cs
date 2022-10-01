using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RC4
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class RC4 : CryptographicTechnique
    {
        public override string Decrypt(string cipherText, string key)
        {
            string plainText = RC4_Algo(cipherText, key);
            return plainText;
        }

        public override string Encrypt(string plainText, string key)
        {

            string cipherText = RC4_Algo(plainText, key);
            return cipherText;
        }

        public string RC4_Algo(string plainText, string key)
        {
            bool hexa = false;
            string plain = "";
            string tempS = "";
            int dec;
            if (plainText[0] == '0' && plainText[1] == 'x')
            {
                for (int i = 2; i < plainText.Length; i += 2)
                {
                    tempS = "";
                    tempS += plainText[i];
                    tempS += plainText[i + 1];
                    dec = Convert.ToInt32(tempS, 16);
                    tempS = "";
                    tempS += (char)dec;
                    plain += tempS;
                }
                hexa = true;
                plainText = plain;
            }
            string keyy = "";
            if (key[0] == '0' && key[1] == 'x')
            {
                for (int i = 2; i < key.Length; i += 2)
                {
                    tempS = "";
                    tempS += key[i];
                    tempS += key[i + 1];
                    dec = Convert.ToInt32(tempS, 16);
                    tempS = "";
                    tempS += (char)dec;
                    keyy += tempS;
                }
                key = keyy;
            }
            string cipherText = "";
            if (hexa)
                cipherText += "0x";
            int[] s = new int[256];
            for (int i = 0; i < 256; i++)
            {
                s[i] = i;
            }
            char[] t = new char[256];
            int k_lenth = key.Length;
            int counter = 0;
            for (int i = 0; i < 256; i++)
            {
                t[i] = key[counter];
                counter++;
                if (counter == k_lenth)
                    counter = 0;
            }
            //                                         A __ Done
            int temp = 0;
            int j = 0;
            for (int i = 0; i < 256; i++)
            {
                //if ((j + s[j] + t[j]) < 256)
                j = (j + s[i] + t[i]) % 256; /*
                else if((j + s[j] + t[j]) > 256*2)
                    j = (j + s[j] + t[j]) - 256*2;
                else
                    j = (j + s[j] + t[j]) - 256; */
                if (j == 0)
                    j = 0;
                temp = s[i];
                s[i] = s[j];
                s[j] = temp;
            }
            //                                         B __ Done
            counter = 0;
            int o = 0; j = 0;
            int k = 0;
            int T = 0;
            char c;
            while (counter < plainText.Length)
            {

                o = (o + 1) % 256;

                j = (j + s[o]) % 256;

                temp = s[o];
                s[o] = s[j];
                s[j] = temp;

                T = (s[j] + s[o]) % 256;
                k = s[T];
                temp = plainText[counter];
                int l = 'í';
                char p = (char)210;
                int oo = temp ^ k;
                if (hexa)
                {
                    tempS = "";
                    tempS += oo;
                    cipherText += int.Parse(tempS).ToString("X");
                    counter++;
                    continue;
                }
                c = (char)(oo);
                cipherText += c;
                counter++;
            }
            dec = 0;

            return cipherText;
        }
    }
}