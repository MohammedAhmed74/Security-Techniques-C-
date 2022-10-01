using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        public string Decrypt(string cipherText, string key)
        {
            string alphabets = "abcdefghiklmnopqrstuvwxyz".ToUpper();
            // String key = "playfairexample";
            cipherText = cipherText.ToUpper();
            string pt = "";
            string tmpkey = key.ToUpper();
            tmpkey.Replace("J", "I");
            char[] tmpk = tmpkey.ToCharArray();
            char[] tmpa = alphabets.ToCharArray();
            bool dup = false;
            // tmpkey += alphabets;
            List<char> list = new List<char>();
            list.AddRange(tmpk);
            list.AddRange(tmpa);
            char[] arr = list.ToArray();
            int m = 0;
           







            string res = "";


            for (int i = 0; i < arr.Length; i++)
            {
                dup = false;

                if (i == 0)
                {
                    res += arr[i].ToString();

                    continue;
                }

                for (int j = i - 1; j >= 0; j--)
                {


                    if (arr[i] == arr[j])
                    {
                        //Console.WriteLine(tmpkey[i]);

                        dup = true;



                    }
                }
                if (!dup)
                    res += arr[i].ToString();
               

            }

            char[,] arrr = new char[5, 5];

            for (int i = 0; i < 5; i++)
            {



                for (int j = 0; j < 5; j++)
                {
                    arrr[i, j] = res[m];
                    m++;




                }
            }
            for (int i = 0; i < 5; i++)
            {



                for (int j = 0; j < 5; j++)
                {

                    Console.Write(arrr[i, j]);

                }
                Console.WriteLine();
            }


            for (int p = 0; p < cipherText.Length; p += 2)
            {

                int row1 = 0;
                int col1 = 0;
                int row2 = 0;
                int col2 = 0;





                GetPosition(ref arrr, cipherText[p], ref row1, ref col1);
                GetPosition(ref arrr, cipherText[p + 1], ref row2, ref col2);



               
                if (row1 == row2)
                {

                    pt += new string(SameRow(ref arrr, row1, col1, col2, -1));

                }
                else if (col1 == col2)
                {
                    pt += new string(SameColumn(ref arrr, col1, row1, row2, -1));

                }
                else
                {
                    pt += new string(Different(ref arrr, row1, col1, row2, col2));


                }




            }







            int tmp = pt.Length;
             for (int i = 0; i < tmp; i++)
             {
                if (i != 0 && i + 1 < tmp)
                {
                    if (pt[i] == 'X' && (i) % 2 != 0 && pt[i - 1] == pt[i + 1])
                    {
                        pt = pt.Remove(i, 1);
                        tmp -= 1;
                       
                    }
                }


            }
            if (pt[tmp-1] == 'X') {
                pt = pt.Remove(tmp-1, 1);
            }
            

            //  Console.WriteLine("$$$$$$$$$  " + ct);

            return pt;
        }

        public string Encrypt(string plainText, string key)
        {
            string alphabets = "abcdefghiklmnopqrstuvwxyz".ToUpper();
            // String key = "playfairexample";
            plainText = plainText.ToUpper();
            string ct = "";
            string tmpkey = key.ToUpper();
            tmpkey.Replace("J", "I");
            char[] tmpk = tmpkey.ToCharArray();
            char[] tmpa = alphabets.ToCharArray();
            bool dup = false;
            // tmpkey += alphabets;
            List<char> list = new List<char>();
            list.AddRange(tmpk);
            list.AddRange(tmpa);
            char[] arr = list.ToArray();
            int m = 0;

            for (int p = 0; p < plainText.Length; p += 2)
            {


                if (p + 1 < plainText.Length)
                {
                    if (plainText[p] == plainText[p + 1])
                    {
                        plainText = plainText.Insert(p + 1, "X");


                    }
                }

                
            }



            if ((plainText.Length % 2) != 0)
                plainText += "X";


            string res = "";


            for (int i = 0; i < arr.Length; i++)
            {
                dup = false;

                if (i == 0)
                {
                    res += arr[i].ToString();

                    continue;
                }

                for (int j = i - 1; j >= 0; j--)
                {


                    if (arr[i] == arr[j])
                    {
                        //Console.WriteLine(tmpkey[i]);

                        dup = true;



                    }
                }
                if (!dup)
                    res += arr[i].ToString();


            }

            char[,] arrr = new char[5, 5];

            for (int i = 0; i < 5; i++)
            {



                for (int j = 0; j < 5; j++)
                {
                    arrr[i, j] = res[m];
                    m++;




                }
            }

            bool d = false;
            for (int p = 0; p < plainText.Length; p += 2)
            {

                int row1 = 0;
                int col1 = 0;
                int row2 = 0;
                int col2 = 0;


                GetPosition(ref arrr, plainText[p], ref row1, ref col1);
                GetPosition(ref arrr, plainText[p + 1], ref row2, ref col2);



               
                if (row1 == row2)
                {

                    ct += new string(SameRow(ref arrr, row1, col1, col2, 1));

                }
                else if (col1 == col2)
                {
                    ct += new string(SameColumn(ref arrr, col1, row1, row2, 1));

                }
                else
                {
                    ct += new string(Different(ref arrr, row1, col1, row2, col2));


                }




            }






            return ct;




        }
        static void GetPosition(ref char[,] arrr, char ch, ref int row, ref int col)
        {
       
            for (int i = 0; i < 5; ++i)
            {
                for (int j = 0; j < 5; ++j)
                {
                    if (arrr[i, j] == ch)
                    {
                        row = i;
                        col = j;
                    }
                }
            }
        }

        static int Mod(int a, int b)
        {
            b = Math.Abs(b);
            return (a % b + b) % b;
        }
        static char[] SameRow(ref char[,] keySquare, int row, int col1, int col2, int encipher)
        {
            return new char[] { keySquare[row, Mod((col1 + encipher), 5)], keySquare[row, Mod((col2 + encipher), 5)] };
        }
        static char[] SameColumn(ref char[,] keySquare, int col, int row1, int row2, int encipher)
        {
            return new char[] { keySquare[Mod((row1 + encipher), 5), col], keySquare[Mod((row2 + encipher), 5), col] };
        }
        static char[] Different(ref char[,] keySquare, int row1, int col1, int row2, int col2)
        {
            return new char[] { keySquare[row1, col2], keySquare[row2, col1] };
        }
    }
}
