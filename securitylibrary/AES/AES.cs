using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class AES : CryptographicTechnique
    {
        public override string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }

        public override string Encrypt(string plainText, string key)
        {
            int plainSize = (plainText.Length - 2) / 2, keySize = (key.Length - 2) / 2;
            double rows = Math.Sqrt(keySize);
            int plainBlock = 1, roundNum = 0;
            char row, column;
            long dec1, dec2;
            int counter1 = 2;
            string[,] stateArr = new string[(int)rows, (int)rows];
            string[,] shiftedArr = new string[(int)rows, (int)rows];
            string[,] mixedColumns = new string[(int)rows, (int)rows];
            string[,] RKey = new string[(int)rows, (int)rows];
            string[,] finalRes = new string[(int)rows, (int)rows];
            string[,] mainKey = new string[(int)rows, (int)rows];
            string plain2 = "";
            string temp = "";
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    mainKey[j, i] += key[counter1];
                    mainKey[j, i] += key[counter1 + 1];
                    counter1 += 2;
                }
            }
            counter1 = 2;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    temp = plainText[counter1].ToString();
                    temp += plainText[counter1 + 1].ToString();
                    dec1 = Convert.ToInt32(temp, 16);
                    temp = key[counter1].ToString();
                    temp += key[counter1 + 1].ToString();
                    dec2 = Convert.ToInt32(temp, 16);
                    plain2 += (dec1 ^ dec2).ToString("X2");
                    counter1 += 2;
                }
            }
            plainText = "0x";
            plainText += plain2;
            while (plainBlock * keySize <= plainSize)
            {
                /*string[,] Tablero =  {
                   { "a0", "88", "23", "2a"},
{ "fa", "54", "a3", "6c" },
{ "fe", "2c", "39", "76"},
{ "17", "b1", "39", "05" } };
               string[,] Tablero2 =  {
                   { "04", "e0", "48", "28"},
{ "66", "cb", "f8", "06" },
{ "81", "19", "d3", "26"},
{ "e5", "9a", "7a", "4c" } };*/
                for (int k = 0; k < 9; k++)
                {
                    stateArr = subBytes(plainText, plainBlock, (int)rows, keySize);
                    shiftedArr = shiftRows(stateArr, (int)rows);
                    mixedColumns = MixColumns(shiftedArr, (int)rows);
                    RKey = keySchedule(mainKey, (int)rows, roundNum);
                    roundNum++;
                    finalRes = AddRoundKey(mixedColumns, RKey, (int)rows);
                    plainText = "0x";
                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < rows; j++)
                        {
                            plainText += finalRes[j, i];
                        }
                    }
                    mainKey = RKey;
                }

                stateArr = subBytes(plainText, plainBlock, (int)rows, keySize);
                shiftedArr = shiftRows(stateArr, (int)rows);
                RKey = keySchedule(mainKey, (int)rows, roundNum);
                roundNum++;
                finalRes = AddRoundKey(shiftedArr, RKey, (int)rows);


                plainBlock++;
            }

            string cypherText = "0x";
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    cypherText += finalRes[j, i];
                }
            }
            return cypherText;
        }


        string[,] AddRoundKey(string[,] mixedArr, string[,] RKey, int rows)
        {
            long decRKey = 0;
            long decColumn = 0;
            long res = 0;
            string[,] finalRes = new string[rows, rows];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    decRKey = Convert.ToInt64(RKey[j, i], 16);
                    decColumn = Convert.ToInt64(mixedArr[j, i], 16);
                    res = decRKey ^ decColumn;
                    finalRes[j, i] = res.ToString("X2");
                }
            }
            return finalRes;
        }

        string[,] keySchedule(string[,] key, int rows, int Rnum)
        {
            string[,] RKey = new string[rows, rows];
            string[] rotatedColumn = new string[rows];
            string[] searchValues = new string[rows];
            string[] RconArr = new string[4];
            int columnNum = 0;

            long decValue = 0;
            for (int i = 1; i < rows; i++)
            {
                rotatedColumn[i - 1] += key[i, rows - 1];
            }
            rotatedColumn[rows - 1] += key[0, rows - 1];
            for (int i = 0; i < rows; i++)
            {
                //decValue = Convert.ToInt64(rotatedColumn[i], 16);
                searchValues[i] = sBox[Convert.ToInt32(rotatedColumn[i], 16)].ToString("x2");
            }
            RconArr = Rcon(Rnum);
            long decRcon = 0;
            long decSVal = 0;
            long decColumn = 0;
            long res = 0;
            //Convert.ToInt64(allhex, 16);
            for (int i = 0; i < rows; i++)
            {
                decRcon = Convert.ToInt64(RconArr[i], 16);
                decSVal = Convert.ToInt64(searchValues[i], 16);
                decColumn = Convert.ToInt64(key[i, columnNum], 16);
                res = decSVal ^ decRcon;
                res = res ^ decColumn;
                RKey[i, 0] = res.ToString("X2");
            }
            for (int i = 1; i < rows; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    decSVal = Convert.ToInt64(RKey[j, i - 1], 16);
                    decColumn = Convert.ToInt64(key[j, i], 16);
                    res = decSVal ^ decColumn;
                    RKey[j, i] = res.ToString("X2");
                }
            }
            return RKey;
        }
        string[] Rcon(int Rnum)
        {
            string[] ret = new string[4];
            string[,] Rcon1 =  {
                    { "01", "02", "04", "08", "10", "20", "40", "80", "1b", "36"},
 {"00", "00", "00", "00", "00", "00", "00", "00", "00", "00"},
 {"00", "00", "00", "00", "00", "00", "00", "00", "00", "00"},
 {"00", "00", "00", "00", "00", "00", "00", "00", "00", "00"}, };
            for (int i = 0; i < 4; i++)
            {
                ret[i] = Rcon1[i, Rnum];
            }
            return ret;
        }
        string[,] subBytes(string plainText, int plainBlock, int rows, int keySize)
        {
            int plainStop = 0;
            int counter = 0;
            string plainState;
            plainState = "";
            string input = "";
            string[,] stateArr = new string[(int)rows, (int)rows];
            // cutting keysize from plainText
            for (int i = plainStop; i < (plainBlock * keySize * 2) + 2; i++)
            {
                if (i < 2)
                    continue;
                plainState += plainText[i];
                plainStop++;
            }
            // now we got plainState
            // putting each 2 char in cell in 2d array

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    stateArr[i, j] = "";
                }
            }
            counter = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    input += plainState[counter];
                    input += plainState[counter + 1];
                    int in1 = Convert.ToInt32(input, 16);
                    //stateArr[j, i] = sBox[Convert.ToInt32(input, 16)].ToString();
                    stateArr[j, i] = sBox[in1].ToString("X2");
                    if (stateArr[j, i].Length < 2)
                    {
                        stateArr[j, i] = "0";
                        stateArr[j, i] += sBox[in1].ToString("X2");
                    }
                    input = "";
                    counter += 2;
                }
            }
            return stateArr;
        }

        string[,] shiftRows(string[,] stateArr, long rows)
        {

            string[,] shiftedArr = new string[(int)rows, (int)rows];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    shiftedArr[i, j] = "";
                }
            }
            int shiftTimes;
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < (int)rows; j++)
                {
                    shiftedArr[i, j] = stateArr[i, j];
                }
            }
            for (int i = 1; i < (int)rows; i++)
            {
                shiftTimes = i;
                for (int k = 0; k < rows; k++)
                {
                    shiftedArr[i, k] = stateArr[i, shiftTimes];
                    shiftTimes++;
                    if (shiftTimes >= (int)rows)
                        break;
                }
                shiftTimes = i;
                for (int k = 0; k < rows; k++)
                {
                    shiftedArr[i, (int)rows - shiftTimes] = stateArr[i, k];
                    shiftTimes--;
                    if (shiftTimes <= 0)
                        break;
                }
            }
            return shiftedArr;
        }
        string[,] MixColumns(string[,] shiftedArr, int rows)
        {
            string[,] fixedArr = {
            {"02", "03", "01", "01" },
            {"01", "02", "03", "01" },
            {"01", "01", "02", "03" },
            {"03", "01", "01", "02" },
            };

            int size = fixedArr.Length;
            string[,] result = new string[rows, rows];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    result[i, j] = "";
                }
            }
            long[] result1 = new long[rows];
            long final;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    final = 0;
                    for (int k = 0; k < rows; k++)
                    {
                        result1[k] = 0;

                        result1[k] = getValue(fixedArr[i, k], shiftedArr[k, j]);
                        //result1[k] = getValue("01","70");// decimal

                    }
                    for (int k = 0; k < rows - 1; k++)
                    {
                        result1[k + 1] = result1[k] ^ result1[k + 1];
                        final = result1[k + 1];
                    }
                    result[i, j] = final.ToString("X2");
                }
            }
            return result;
        }

        long getValue(string fixed1, string temp)
        {
            if (fixed1 == "01")
                return Convert.ToInt32(temp, 16);
            else
            {
                string first = "";
                string second = "";
                first += temp[0];
                second += temp[1];
                bool is0 = false;
                long fDic = Int64.Parse(first, System.Globalization.NumberStyles.HexNumber);
                long sDic = Int64.Parse(second, System.Globalization.NumberStyles.HexNumber);
                string fBinary = Convert.ToString(fDic, 2);
                string sBinary = Convert.ToString(sDic, 2);
                if (sBinary.Length < 4)
                {
                    string temp1;
                    temp1 = sBinary;
                    sBinary = "";
                    for (int i = 0; i < 4 - temp1.Length; i++)
                    {
                        sBinary += '0';
                    }
                    sBinary += temp1;
                }
                if (fBinary.Length < 4)
                {
                    string temp2;
                    temp2 = fBinary;
                    fBinary = "";
                    for (int i = 0; i < 4 - temp2.Length; i++)
                    {
                        fBinary += '0';
                    }
                    fBinary += temp2;
                    is0 = true;
                }               //                             Shiftting
                if (!is0)
                {
                    string allBin = "";
                    for (int i = 1; i < 4; i++)
                    {
                        allBin += fBinary[i];
                    }
                    allBin += sBinary;
                    allBin += '0';
                    string allhex = Convert.ToInt32(allBin, 2).ToString("X2");
                    long dec1 = Convert.ToInt64(allhex, 16);
                    long dec2 = Convert.ToInt64("1B", 16);
                    long last1Ret = dec1 ^ dec2;
                    if (fixed1 == "03")
                    {
                        last1Ret = last1Ret ^ Convert.ToInt32(temp, 16);
                    }
                    //hexResult = result.ToString("X");
                    return last1Ret;
                }
                else
                {
                    long last0Ret;
                    string allBin = "";
                    string allhex;
                    for (int i = 1; i < 4; i++)
                    {
                        allBin += fBinary[i];
                    }
                    allBin += sBinary;
                    allBin += '0';
                    if (fixed1 == "03")
                    {
                        allhex = Convert.ToInt32(allBin, 2).ToString("X2");
                        long dec1 = Convert.ToInt64(allhex, 16);
                        long dec2 = Convert.ToInt64(temp, 16);
                        last0Ret = dec1 ^ dec2;
                        //allhex = dec2.ToString("X2");
                    }
                    else
                    {
                        //allhex = Convert.ToInt32(allBin, 2).ToString("X2");
                        last0Ret = Convert.ToInt32(allBin, 2);
                    }

                    return last0Ret;
                };

            }
        }


        private readonly byte[] sBox = new byte[256] {
    //0     1    2      3     4    5     6     7      8    9     A      B    C     D     E     F
    0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 0x01, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76, //0
    //16    17    18    19    20   21    22    23    24    25    26    27    28    29    30    31
    0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0, 0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0, //1
    0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15, //2
    0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a, 0x07, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75, //3
    0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0, 0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84, //4
    0x53, 0xd1, 0x00, 0xed, 0x20, 0xfc, 0xb1, 0x5b, 0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf, //5
    0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 0x02, 0x7f, 0x50, 0x3c, 0x9f, 0xa8, //6
    0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5, 0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2, //7
    0xcd, 0x0c, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73, //8
    0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88, 0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0x0b, 0xdb, //9
    0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c, 0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79, //A
    0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08, //B
    0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6, 0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a, //C
    0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e, 0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e, //D
    0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94, 0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf, //E
    0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68, 0x41, 0x99, 0x2d, 0x0f, 0xb0, 0x54, 0xbb, 0x16 }; //F
    }
}
