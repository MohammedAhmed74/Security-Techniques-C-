using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {
            string temp = "";
            int c = 0;
            int num_rows;
            int num_columns;
            int counter = 0;
            bool found = false;
            int Ekey = 0;
            plainText = plainText.ToUpper();

            for (int key = 2; key < 10; key++)
            {
                num_rows = key;
                temp = "";
                c = 0;
                num_columns = plainText.Length / key;
                if (num_rows * num_columns < plainText.Length)
                    num_columns++;
                for (int i = 0; i < num_columns; i++)
                {
                    temp += plainText[c];
                    c += key;
                }
                counter = 0;
                for (int i = 0; i < num_columns + 1; i++)
                {
                    if (temp[0] == cipherText[i])
                    {
                        for (int j = 0; j < num_columns; j++)
                        {
                            if (temp[j] == cipherText[i + j])
                            {
                                counter++;
                                if (counter == num_columns)
                                {
                                    found = true;
                                    Ekey = key;
                                    break;
                                }
                            }
                        }
                    }
                    if (found)
                        break;
                }
                if (found)
                    break;
            }
            return Ekey;
        }

        public string Decrypt(string cipherText, int key)
        {
            int num_rows = key;
            int num_columns = cipherText.Length / key;
            if (num_columns * num_rows < cipherText.Length)
                num_columns++;
            char[,] cipher = new char[num_rows, num_columns];
            int c = 0;
            for (int i = 0; i < num_rows; i++)
            {
                for (int j = 0; j < num_columns; j++)
                {
                    if (c >= cipherText.Length)
                        break;
                    cipher[i, j] = cipherText[c];
                    c++;
                }
            }

            for (int i = 0; i < num_columns; i++)
            {
                for (int j = 0; j < num_rows; j++)
                {
                    Console.Write(cipher[j, i]);
                }
                Console.WriteLine(" ");
            }

            //       *************************************************
            string plain_Text = "";
            int counter = cipherText.Length;
            for (int i = 0; i < num_columns; i++)
            {
                for (int j = 0; j < num_rows; j++)
                {
                    if (counter == 0)
                        break;
                    if (cipher[j, i] == '\0')
                        continue;
                    plain_Text += cipher[j, i];
                    counter--;
                }
            }
            plain_Text = plain_Text.ToUpper();
            Console.WriteLine(plain_Text);
            return plain_Text;
        }

        public string Encrypt(string plainText, int key)
        {
            int num_rows = key;
            int num_columns = plainText.Length / key;
            if (num_columns * num_rows < plainText.Length)
                num_columns++;
            char[,] plain = new char[num_rows, num_columns];
            int c = 0;
            for (int i = 0; i < num_columns; i++)
            {
                for (int j = 0; j < num_rows; j++)
                {
                    if (c >= plainText.Length)
                        break;
                    plain[j, i] = plainText[c];
                    c++;
                }
            }

            string cipherText = "";
            int counter = plainText.Length;
            for (int i = 0; i < num_rows; i++)
            {
                for (int j = 0; j < num_columns; j++)
                {
                    if (counter == 0)
                        break;
                    if (plain[i, j] == '\0')
                        continue;
                    cipherText += plain[i, j];
                    counter--;
                }
            }
            cipherText = cipherText.ToUpper();
            return cipherText;
        }
    }
}
