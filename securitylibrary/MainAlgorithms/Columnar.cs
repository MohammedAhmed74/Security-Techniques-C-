using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public List<int> Analyse(string plainText, string cipherText)
        {
            List<int> result = new List<int>() { };
            int num_rows = 0;
            int num_columns = 0;
            int counterDown;
            cipherText = cipherText.ToUpper();
            plainText = plainText.ToUpper();
            int saveK = 0;
            bool done = false;
            for (int i = 2; i < plainText.Length / 2; i++)
            {
                int k = 0;
                while (k < plainText.Length)
                {
                    if (plainText[0] == cipherText[k])
                    {
                        saveK = k;
                        num_rows = plainText.Length / i;
                        if ((num_rows * i) < plainText.Length)
                        {
                            num_rows += 1;
                        }
                        counterDown = num_rows;
                        for (int j = 0; j < plainText.Length; j += i)
                        {
                            if (plainText[j] == cipherText[k])
                            {
                                counterDown--;
                                if (counterDown == 0)
                                {
                                    done = true;
                                    num_columns = plainText.Length / num_rows;
                                    if (num_rows * num_columns < plainText.Length)
                                        num_columns++;
                                    break;
                                }
                                k++;
                                if (k > cipherText.Length - 1)
                                    break;
                            }
                            else
                                break;

                        }
                        if (done == true)
                            break;

                        k = saveK;
                    }
                    k++;
                    if (done == true)
                        break;

                }
                if (done == true)
                    break;
            }
            done = false;
            char[,] plain = new char[num_rows, num_columns];
            char[,] cipher = new char[num_rows, num_columns];
            int u = 0;
            for (int i = 0; i < num_rows; i++)
            {
                for (int j = 0; j < num_columns; j++)
                {
                    if (u > plainText.Length - 1)
                    {
                        done = true;
                        break;
                    }
                    plain[i, j] = plainText[u];
                    u++;
                }
                if (done == true)
                    break;
            }
            u = 0;
            done = false;
            counterDown = num_rows;
            int temp = num_rows;
            int progress = 0;
            string tempstr = "";
            int rowLength = 0;
            int lastColumns = (num_columns * num_rows) - plainText.Length;
            for (int o = 0; o < num_columns; o++)
            {
                for (int i = 0; i < num_columns; i++)
                {
                    rowLength = 0;
                    tempstr = "";
                    u = progress;
                    counterDown = num_rows;
                    while (counterDown > 0)
                    {
                        if (u > cipherText.Length - 1)
                            break;
                        tempstr += cipherText[u];
                        u++;
                        counterDown--;
                    }

                    if (tempstr[0] == plain[0, i])
                    {
                        for (int j = 0; j < num_rows; j++)
                        {
                            if (tempstr.Length == num_rows - 1 && j == num_rows - 1)
                                break;
                            if (tempstr[j] == plain[j, i])
                            {
                                rowLength++;
                            }
                        }

                        if (rowLength == num_rows)
                        {
                            for (int k = 0; k < num_rows; k++)
                            {
                                cipher[k, o] = plain[k, i];
                                u++;
                                progress++;
                            }
                            break;
                        }
                        else if (rowLength == num_rows - 1)
                        {
                            for (int k = 0; k < num_rows - 1; k++)
                            {
                                cipher[k, o] = plain[k, i];
                                u++;
                                progress++;
                            }
                            break;
                        }
                    }

                }
            }
            //0000000000000000000000000000000000000000000000000000000

            Console.WriteLine("0000000000000000000000000000000");
            for (int i = 0; i < num_columns; i++)
            {
                for (int j = 0; j < num_rows; j++)
                {
                    Console.Write(plain[j, i]);
                }
                Console.WriteLine("");
            }

            Console.WriteLine("0000000000000000000000000000000");
            for (int i = 0; i < num_columns; i++)
            {
                for (int j = 0; j < num_rows; j++)
                {
                    Console.Write(cipher[j, i]);
                }
                Console.WriteLine("");
            }

            //0000000000000000000000000000000000000000000000000000000
            int rowEle = num_rows;
            int[] sortResult = new int[num_columns];
            done = false;
            for (int o = 0; o < num_columns; o++)
            {
                done = false;
                for (int i = 0; i < num_columns; i++)
                {
                    rowEle = num_rows;
                    if (plain[0, o] == cipher[0, i])
                    {
                        for (int j = 0; j < num_rows; j++)
                        {
                            if (plain[j, o] == cipher[j, i])
                            {
                                rowEle--;
                                if (rowEle < 2)
                                {
                                    done = true;
                                    result.Add(i + 1);
                                    break;
                                }
                            }
                        }
                    }
                }
                if (done != true)
                {
                    for (int i = 0; i < num_columns; i++)
                    {
                        rowEle = num_rows;
                        if (plain[1, o] == cipher[0, i])
                        {
                            for (int j = 0; j < num_rows - 1; j++)
                            {
                                if (plain[j + 1, o] == cipher[j, i])
                                {
                                    rowEle--;
                                    if (rowEle < 2)
                                    {
                                        result.Add(i + 1);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        public string Decrypt(string cipherText, List<int> key)
        {
            int num_columns = key.Max();
            int num_rows = cipherText.Length / num_columns;
            if (cipherText.Length > num_columns * num_rows)
                num_rows++;
            int saveRows = num_rows;
            int spaces = (num_columns * num_rows) - cipherText.Length;
            int lastRow = num_columns - spaces;
            bool once = false;
            int k = 0;

            char[,] ciphText = new char[num_rows, num_columns];
            char[,] plainText = new char[num_rows, num_columns];
            for (int i = 0; i < num_columns; i++)
            {
                if (lastRow == 0 && once != true)
                {
                    num_rows -= 1;
                    once = true;
                }

                for (int j = 0; j < num_rows; j++)
                {
                    if (k > cipherText.Length - 1)
                        break;
                    ciphText[j, i] = cipherText[k];
                    k++;
                }
                lastRow--;
            }
            k = 0;

            num_rows = saveRows;
            lastRow = num_columns - spaces;
            once = false;
            for (int i = 0; i < num_columns; i++)
            {
                if (k > cipherText.Length)
                    break;

                if (lastRow == 0 && once != true)
                {
                    num_rows -= 1;
                    once = true;
                }

                for (int j = 0; j < num_rows; j++)
                {
                    plainText[j, i] = ciphText[j, key[i] - 1];
                    k++;
                }

            }
            k = 0;
            string result = "";
            bool done = false;
            for (int i = 0; i < num_rows; i++)
            {
                for (int j = 0; j < num_columns; j++)
                {

                    if (k > cipherText.Length - 1 && done == false)
                    {
                        break;
                        done = true;
                    }

                    result += (char)plainText[i, j];
                    k++;
                }
                if (done == true)
                    break;
            }

            result = result.ToLower();
            return result;
        }

        public string Encrypt(string plainText, List<int> key)
        {
            int num_columns = key.Max();
            int num_rows = plainText.Length / num_columns;
            if (plainText.Length > num_columns * num_rows)
                num_rows++;
            int saveRows = num_rows;
            int spaces = (num_columns * num_rows) - plainText.Length;
            int lastRow = num_columns - spaces;
            bool once = false;
            int k = 0;

            char[,] plaText = new char[num_rows, num_columns];
            char[,] cipher_Text = new char[num_rows, num_columns];
            for (int i = 0; i < num_rows; i++)
            {
                if (lastRow == 0 && once != true)
                {
                    num_rows -= 1;
                    once = true;
                }

                for (int j = 0; j < num_columns; j++)
                {
                    if (k > plainText.Length - 1)
                        break;
                    plaText[i, j] = plainText[k];
                    Console.Write(plaText[i, j]);
                    k++;
                }
                lastRow--;
                Console.WriteLine("---");
            }
            k = 0;

            num_rows = saveRows;
            lastRow = num_columns - spaces;
            once = false;
            Console.WriteLine("***********************************");
            for (int i = 0; i < num_columns; i++)
            {
                if (k > plainText.Length)
                    break;

                if (lastRow == 0 && once != true)
                {
                    num_rows -= 1;
                    once = true;
                }

                for (int j = 0; j < num_rows; j++)
                {
                    cipher_Text[j, key[i] - 1] = plaText[j, i];
                    Console.Write(cipher_Text[j, key[i] - 1]);
                    k++;
                }

                Console.WriteLine("-----");
            }
            string result = "";
            for (int i = 0; i < num_columns; i++)
            {
                for (int j = 0; j < num_rows; j++)
                {
                    result += (char)cipher_Text[j, i];

                }
            }
            string final_result = "";
            for (int j = 0; j < result.Length; j++)
            {
                if (result[j] != '\0')
                    final_result += result[j];

            }

            return final_result;
        }
    }
}
