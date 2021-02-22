
/**
 * @author : https://cwkcw.tistory.com/74
 * @date   : 2019-06-13
 * @desc   : Double 형 자료 끊어서 표기하는 거 
 *           사용례 :  DoubleToStringNum dts = new DoubleToStringNum();
 *                                          dts.fDoubleToStringNumber(더블형);
 *           
 *           
 */

using System;
using System.Collections.Generic;
using UnityEngine;

public class DoubleToStringNum
{
    // 더블 받아서 스트링으로 
    /// <summary>
    /// string 값 받아서 A, B 포함인지 판별후 double로 뱉어냄
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public double PanByulGi(string str)
    {
        if (str == "0") return 0;

        string sResult = string.Empty;

        if (str.Contains("E+"))
        {
            //E+ 를 A, B로 바꿔줌
            str = fPanbyulgi(str);
        }


        if (str.Contains(".")) // 네자리수 이상?
        {
            string[] sNumberList = str.Split('.');

            sResult = sNumberList[1].Substring(3); // 000K 에서 K만 남기기.

            int digit = GetEnumNumber(sResult) - 4; // 0이 몇갠지

            string tmp = sNumberList[0] + sNumberList[1].Substring(0,3); // 소수점 빼버리고 문자열로

            for (int i = 0; i < digit; i++)
            {
                tmp += "0"; // 0의 자리 더해주면서 문자열로.
            }

            sResult = tmp;

        }
        else // 네자리수 미만.
        {
            sResult = str;
        }

        //if (add1.Contains("E+"))
        //{
        //    //E+ 를 A, B로 바꿔줌
        //    add1 = fPanbyulgi(add1);
        //}


        return double.Parse(sResult);
    }




    /// <summary>
    ///  string 받아서 알파벳으로 자리수 지정해서 더해서 출력
    /// </summary>
    /// <returns></returns>
    public string AddStringDouble(string add1, string add2)
    {
        double sResult;
        double stringAdd1 = 0;
        double stringAdd2 = 0;
        /// (string)알파벳 포함 숫자 -> (double)00000000000 숫자로 풀어헤쳐줌.
        if (add1 == "0")
        {
            stringAdd1 = 0;

            if (add2 == "0")
            {
                stringAdd2 = 0;
            }
            else
            {
                stringAdd2 = PanByulGi(add2);
            }
        }
        else
        {
            stringAdd1 = PanByulGi(add1);

            if (add2 == "0")
            {
                stringAdd2 = 0;
            }
            else
            {
                stringAdd2 = PanByulGi(add2);
            }
        }


        // 더해주고 결과
        sResult = (stringAdd1 + stringAdd2);

        return sResult.ToString("f0");
    }

    /// <summary>
    ///  double 받아서 절삭 안되고 
    /// </summary>
    /// <returns></returns>
    public string AddStringDouble(double add1, double add2)
    {
        // 더해주고 결과
        string sResult = (add1 + add2).ToString("f0");

        return sResult;
    }



    /// <summary>
    ///  테스트용 작은 숫자 안 씹히게
    /// </summary>
    /// <returns></returns>
    public double AddDouble(string add1, string add2)
    {
        double dResult = 0;

        double stringAdd1 = 0;
        double stringAdd2 = 0;
        /// (string)알파벳 포함 숫자 -> (double)00000000000 숫자로 풀어헤쳐줌.
        if (add1 == "0")
        {
            stringAdd1 = 0;

            if (add2 == "0")
            {
                stringAdd2 = 0;
            }
            else
            {
                stringAdd2 = PanByulGi(add2);
            }
        }
        else
        {
            stringAdd1 = PanByulGi(add1);

            if (add2 == "0")
            {
                stringAdd2 = 0;
            }
            else
            {
                stringAdd2 = PanByulGi(add2);
            }
        }

        // 더해주고 중간 결과
        dResult = (stringAdd1 + stringAdd2);




        return dResult;
    }



    /// <summary>
    ///  스트링 받아서 알파벳으로 자리수 지정해서 빼고 출력
    /// </summary>
    /// <param name="sub1">큰 숫자를 앞에</param>
    /// <param name="sub2">작은 숫자를 뒤에</param>
    /// <returns>앞자리가 작으면 결과값은 스트링 "0"</returns>
    public string SubStringDouble(string sub1, string sub2)
    {
        //sub1 에서 sub2 를 뺀다.
        //TODO : 마이너스가 될때 예외처리(OK)
        string sResult = string.Empty;

        double stringSub1 = 0;
        double stringSub2 = 0;
        /// (string)알파벳 포함 숫자 -> (double)00000000000 숫자로 풀어헤쳐줌.
        if (sub1 == "0")
        {
            stringSub1 = 0;

            if (sub2 == "0")
            {
                stringSub2 = 0;
            }
            else
            {
                stringSub2 = PanByulGi(sub2);
            }
        }
        else
        {
            stringSub1 = PanByulGi(sub1);

            if (sub2 == "0")
            {
                stringSub2 = 0;
            }
            else
            {
                stringSub2 = PanByulGi(sub2);
            }
        }

        if (stringSub1 < stringSub2)
        {
            //sResult = fDoubleToStringNumber(stringSub2 - stringSub1);
            sResult = "-1";
        }else if (stringSub1 == stringSub2)
        {
            sResult = "0";
        }
        else
        {
            if (stringSub1 - stringSub2 <= 0)
            {
                sResult = "0";
            }
            else
            {
                // 앞자리가 크면?
                sResult = (stringSub1 - stringSub2).ToString("f0");
            }


        }


        return sResult;
    }

    public string SubStringDouble(string sub1, double sub2)
    {
        //sub1 에서 sub2 를 뺀다.
        //TODO : 마이너스가 될때 예외처리(OK)
        string sResult = string.Empty;

        /// (string)알파벳 숫자 -> (double)00000000000 숫자로 풀어헤쳐줌.
        double stringSub1 = PanByulGi(sub1);

        if (stringSub1 < sub2)
        {
            //sResult = fDoubleToStringNumber(stringSub2 - stringSub1);
            sResult = "-1";
        }
        else if (stringSub1 == sub2)
        {
            sResult = "0";
        }
        else
        {
            // 앞자리가 크면?
            sResult = (stringSub1 - sub2).ToString("f0");

        }


        return sResult;
    }


    /// <summary>
    ///  나누기 앞 숫자 / 뒷 수자
    /// </summary>
    /// <returns></returns>
    public double DevideStringDouble(string add1, string add2)
    {
        double stringAdd1 = 0;
        double stringAdd2 = 0;
        /// (string)알파벳 포함 숫자 -> (double)00000000000 숫자로 풀어헤쳐줌.
        if (add1 == "0")
        {
            stringAdd1 = 0;

            if (add2 == "0")
            {
                stringAdd2 = 0;
            }
            else
            {
                stringAdd2 = PanByulGi(add2);
            }
        }
        else
        {
            stringAdd1 = PanByulGi(add1);

            if (add2 == "0")
            {
                stringAdd2 = 0;
            }
            else
            {
                stringAdd2 = PanByulGi(add2);
            }
        }

        double tResult = (stringAdd1 / stringAdd2);


        return tResult;
    }


    /// <summary>
    ///  string 받아서 배수(double)하고 알파벳 숫자 스트링으로 출력
    /// </summary>
    /// <returns></returns>
    public string multipleStringDouble(string mul, double times)
    {
        string sResult = string.Empty;

        /// (string)알파숫자 -> (double)00000000000 숫자로 풀어헤쳐줌.
        double stringMul1 = double.Parse(mul);
        // 배수로 곱해줌
        stringMul1 = (stringMul1 * times);

        /// 1 안되면 예외로 올려줌
        if (stringMul1 < 1.0d) stringMul1 = 1.0d;

        //스트링 출력
        sResult = (stringMul1).ToString("f0");

        return sResult;
    }



    /// <summary>
    /// E 표기로 된 double값 받아와서 알파벳 숫자 스트링으로 변환
    /// </summary>
    /// <param name="dNumber">입력값은 따블 </param>
    /// <returns></returns>
    public string fDoubleToStringNumber(double dNumber) // 881902114
    {
        string sResult = string.Empty;
        string sNumber = string.Empty;
        string sDigit = string.Empty;

        string[] sNumberList = (dNumber.ToString()).Split('+');

        double dKeepNumber = 0;

        // Split 안되었다 = 1.23E+3 형식이 아니다
        if (sNumberList.Length < 2)
        {
            double dRMV_Decimal = Math.Truncate(dNumber);

            //4자리 수 미만
            if (dRMV_Decimal.ToString().Length < 4)
            {
                dKeepNumber = dRMV_Decimal;
                sNumber = string.Format("{0}", dKeepNumber);
            }
            else //4자리 수 이상 100010
            {
                dKeepNumber = double.Parse(dRMV_Decimal.ToString().Substring(0, 4));
                sNumber = string.Format("{0}", dKeepNumber);
            }

            //Enum 에 정의되어있으면
            if (Enum.IsDefined(typeof(EnumNumber), dRMV_Decimal.ToString().Length))
            {
                // 숫자 길이 만큼 얻어와서 이넘
                sDigit = ((EnumNumber)dRMV_Decimal.ToString().Length).ToString().Replace("num_", "");
                sNumber = string.Format("{0:f3}", dKeepNumber * 0.001);

            }
            else // Enum 에 정의 되어있지 않으면 5 6 8 9 11 12 3으로 나눠서 나머지 2 나머지 0
            {
                // 자릿수가 3으로 나눠서 2남으면 2자리수
                if (dRMV_Decimal.ToString().Length % 3 == 2 && dRMV_Decimal.ToString().Length > 3)
                {
                    sDigit = ((EnumNumber)dRMV_Decimal.ToString().Length -1).ToString().Replace("num_", "");
                    sNumber = string.Format("{0:f3}", dKeepNumber * 0.01);
                }
                else if(dRMV_Decimal.ToString().Length % 3 == 0 && dRMV_Decimal.ToString().Length > 3)
                {
                    sDigit = ((EnumNumber)dRMV_Decimal.ToString().Length - 2).ToString().Replace("num_", "");
                    sNumber = string.Format("{0:f3}", dKeepNumber * 0.1);
                }

            }
        }
        else // 1.23E+3 형식일때 16자리 부터.
        {
            var tmp = int.Parse(sNumberList[1]) + 1;

            dKeepNumber = double.Parse(sNumberList[0].ToString().Replace("E", ""));

            if (tmp % 3 == 2 && tmp > 3) //16 = 1,  17 = 2,  18 = 0
            {
                tmp -= 1;
                sNumber = string.Format("{0:f3}", dKeepNumber * 10);
                sDigit = ((EnumNumber)tmp).ToString().Replace("num_", "");
            }
            else if (tmp % 3 == 0 && tmp > 3)
            {
                tmp -= 2;
                sNumber = string.Format("{0:f3}", dKeepNumber * 100);
                sDigit = ((EnumNumber)tmp).ToString().Replace("num_", "");
            }
            else
            {
                sDigit = ((EnumNumber)tmp).ToString().Replace("num_", "");
                sNumber = string.Format("{0:f3}", dKeepNumber);

            }

            if (tmp > 69)
            {
                sResult = "N/A";
            }

        }

        if (float.Parse(sNumber) >= 100f && sDigit == "CX")
        {
            sNumber = "100.000";
        }


        sResult = String.Format("{0}{1}", sNumber, sDigit);

        return sResult;

    }

    /// <summary>
    /// E + 스트링 ->  double -> Tostring
    /// </summary>
    /// <param name="dNumber">E + 가 포함된 스트링을 입력해서 변환 </param>
    /// <returns>E + 가 포함된 스트링을 입력해서 변환</returns>
    public string fDoubleToStringNumber(string dNumber)
    {
        if (dNumber == "0" || dNumber == null || dNumber == "") return "0";

        string sResult = string.Empty;
        string sNumber = string.Empty;
        string sDigit = string.Empty;

        if (!dNumber.Contains("+"))
        {
            double twalla = double.Parse(dNumber);
            dNumber = twalla.ToString();
        }

        string[] sNumberList = (dNumber.ToString()).Split('+');

        double dKeepNumber = 0;

        // Split 안되었다 = 1.23E+3 형식이 아니다
        if (sNumberList.Length < 2)
        {
            double dRMV_Decimal = double.Parse(dNumber);

            //4자리 수 미만
            if (dNumber.Length < 4)
            {
                dKeepNumber = dRMV_Decimal;
                sNumber = string.Format("{0}", dKeepNumber);
            }
            else //4자리 수 이상
            {
                dKeepNumber = double.Parse(dRMV_Decimal.ToString().Substring(0, 4));
                sNumber = string.Format("{0}", dKeepNumber);
            }

            //Enum 에 정의되어있으면
            if (Enum.IsDefined(typeof(EnumNumber), dRMV_Decimal.ToString().Length))
            {
                // 숫자 길이 만큼 얻어와서 이넘
                sDigit = ((EnumNumber)dRMV_Decimal.ToString().Length).ToString().Replace("num_", "");
                sNumber = string.Format("{0:f3}", dKeepNumber * 0.001);

            }
            else // Enum 에 정의 되어있지 않으면 5 6 8 9 11 12 3으로 나눠서 나머지 2 나머지 0
            {
                // 자릿수가 3으로 나눠서 2남으면 2자리수
                if (dRMV_Decimal.ToString().Length % 3 == 2 && dRMV_Decimal.ToString().Length > 3)
                {
                    sDigit = ((EnumNumber)dRMV_Decimal.ToString().Length - 1).ToString().Replace("num_", "");
                    sNumber = string.Format("{0:f3}", dKeepNumber * 0.01);
                }
                else if (dRMV_Decimal.ToString().Length % 3 == 0 && dRMV_Decimal.ToString().Length > 3)
                {
                    sDigit = ((EnumNumber)dRMV_Decimal.ToString().Length - 2).ToString().Replace("num_", "");
                    sNumber = string.Format("{0:f3}", dKeepNumber * 0.1);
                }

            }
        }
        else // 1.23E+3 형식일때 16자리 부터.
        {
            var tmp = int.Parse(sNumberList[1]) + 1;

            dKeepNumber = double.Parse(sNumberList[0].ToString().Replace("E", ""));

            if (tmp % 3 == 2 && tmp > 3) //16 = 1,  17 = 2,  18 = 0
            {
                tmp -= 1;
                sNumber = string.Format("{0:f3}", dKeepNumber * 10);
                sDigit = ((EnumNumber)tmp).ToString().Replace("num_", "");
            }
            else if (tmp % 3 == 0 && tmp > 3)
            {
                tmp -= 2;
                sNumber = string.Format("{0:f3}", dKeepNumber * 100);
                sDigit = ((EnumNumber)tmp).ToString().Replace("num_", "");
            }
            else
            {
                sDigit = ((EnumNumber)tmp).ToString().Replace("num_", "");
                sNumber = string.Format("{0:f3}", dKeepNumber);

            }

            if (tmp > 69)
            {
                sResult = "N/A";
            }

        }


        if (float.Parse(sNumber) >= 100f && sDigit == "CX")
        {
            sNumber = "100.000";
        }

        sResult = String.Format("{0}{1}", sNumber, sDigit);

        sResult = PanByulGi(sResult).ToString("f0");

        return sResult;

    }




    /// <summary>
    /// E + 스트링 ->  double -> Tostring
    /// </summary>
    /// <param name="dNumber">E + 가 포함된 스트링을 입력해서 변환 </param>
    /// <returns>E + 가 포함된 스트링을 입력해서 변환</returns>
    public string fDoubleToGoldOutPut(string dNumber)
    {
        if (dNumber == "0" || dNumber == null || dNumber == "") return "0";

        string sResult = string.Empty;
        string sNumber = string.Empty;
        string sDigit = string.Empty;

        if (!dNumber.Contains("+"))
        {
            double twalla = double.Parse(dNumber);
            dNumber = twalla.ToString();
        }

        string[] sNumberList = (dNumber.ToString()).Split('+');

        double dKeepNumber = 0;

        // Split 안되었다 = 1.23E+3 형식이 아니다
        if (sNumberList.Length < 2)
        {
            double dRMV_Decimal = double.Parse(dNumber);

            //4자리 수 미만
            if (dNumber.Length < 4)
            {
                dKeepNumber = dRMV_Decimal;
                sNumber = string.Format("{0}", dKeepNumber);
            }
            else //4자리 수 이상
            {
                dKeepNumber = double.Parse(dRMV_Decimal.ToString().Substring(0, 4));
                sNumber = string.Format("{0}", dKeepNumber);
            }

            //Enum 에 정의되어있으면
            if (Enum.IsDefined(typeof(EnumNumber), dRMV_Decimal.ToString().Length))
            {
                // 숫자 길이 만큼 얻어와서 이넘
                sDigit = ((EnumNumber)dRMV_Decimal.ToString().Length).ToString().Replace("num_", "");
                sNumber = string.Format("{0:f3}", dKeepNumber * 0.001);

            }
            else // Enum 에 정의 되어있지 않으면 5 6 8 9 11 12 3으로 나눠서 나머지 2 나머지 0
            {
                // 자릿수가 3으로 나눠서 2남으면 2자리수
                if (dRMV_Decimal.ToString().Length % 3 == 2 && dRMV_Decimal.ToString().Length > 3)
                {
                    sDigit = ((EnumNumber)dRMV_Decimal.ToString().Length - 1).ToString().Replace("num_", "");
                    sNumber = string.Format("{0:f3}", dKeepNumber * 0.01);
                }
                else if (dRMV_Decimal.ToString().Length % 3 == 0 && dRMV_Decimal.ToString().Length > 3)
                {
                    sDigit = ((EnumNumber)dRMV_Decimal.ToString().Length - 2).ToString().Replace("num_", "");
                    sNumber = string.Format("{0:f3}", dKeepNumber * 0.1);
                }

            }
        }
        else // 1.23E+3 형식일때 16자리 부터.
        {
            var tmp = int.Parse(sNumberList[1]) + 1;

            dKeepNumber = double.Parse(sNumberList[0].ToString().Replace("E", ""));

            if (tmp % 3 == 2 && tmp > 3) //16 = 1,  17 = 2,  18 = 0
            {
                tmp -= 1;
                sNumber = string.Format("{0:f3}", dKeepNumber * 10);
                sDigit = ((EnumNumber)tmp).ToString().Replace("num_", "");
            }
            else if (tmp % 3 == 0 && tmp > 3)
            {
                tmp -= 2;
                sNumber = string.Format("{0:f3}", dKeepNumber * 100);
                sDigit = ((EnumNumber)tmp).ToString().Replace("num_", "");
            }
            else
            {
                sDigit = ((EnumNumber)tmp).ToString().Replace("num_", "");
                sNumber = string.Format("{0:f3}", dKeepNumber);

            }

            if (tmp > 69)
            {
                sResult = "N/A";
            }

        }


        if (float.Parse(sNumber) >= 100f && sDigit == "CX")
        {
            sNumber = "100.000";
        }

        sResult = String.Format("{0}{1}", sNumber, sDigit);

        return sResult;

    }









    /// <summary>
    /// 입력값은 스트링
    /// E + 가 포함된 스트링을 입력해서 변환
    /// </summary>
    /// <param name="dNumber">E + 가 포함된 스트링을 입력해서 변환 </param>
    /// <returns>E + 가 포함된 스트링을 입력해서 변환</returns>
    public string fPanbyulgi(string dNumber)
    {
        string sResult = string.Empty;
        string sNumber = string.Empty;
        string sDigit = string.Empty;

        if (!dNumber.Contains("+"))
        {
            double twalla = double.Parse(dNumber);
            dNumber = twalla.ToString();
        }

        string[] sNumberList = (dNumber.ToString()).Split('+');

        double dKeepNumber = 0;

        // Split 안되었다 = 1.23E+3 형식이 아니다
        if (sNumberList.Length < 2)
        {
            double dRMV_Decimal = double.Parse(dNumber);

            //4자리 수 미만
            if (dNumber.Length < 4)
            {
                dKeepNumber = dRMV_Decimal;
                sNumber = string.Format("{0}", dKeepNumber);
            }
            else //4자리 수 이상
            {
                dKeepNumber = double.Parse(dRMV_Decimal.ToString().Substring(0, 4));
                sNumber = string.Format("{0}", dKeepNumber);
            }

            //Enum 에 정의되어있으면
            if (Enum.IsDefined(typeof(EnumNumber), dRMV_Decimal.ToString().Length))
            {
                // 숫자 길이 만큼 얻어와서 이넘
                sDigit = ((EnumNumber)dRMV_Decimal.ToString().Length).ToString().Replace("num_", "");
                sNumber = string.Format("{0:f3}", dKeepNumber * 0.001);

            }
            else // Enum 에 정의 되어있지 않으면 5 6 8 9 11 12 3으로 나눠서 나머지 2 나머지 0
            {
                // 자릿수가 3으로 나눠서 2남으면 2자리수
                if (dRMV_Decimal.ToString().Length % 3 == 2 && dRMV_Decimal.ToString().Length > 3)
                {
                    sDigit = ((EnumNumber)dRMV_Decimal.ToString().Length - 1).ToString().Replace("num_", "");
                    sNumber = string.Format("{0:f3}", dKeepNumber * 0.01);
                }
                else if (dRMV_Decimal.ToString().Length % 3 == 0 && dRMV_Decimal.ToString().Length > 3)
                {
                    sDigit = ((EnumNumber)dRMV_Decimal.ToString().Length - 2).ToString().Replace("num_", "");
                    sNumber = string.Format("{0:f3}", dKeepNumber * 0.1);
                }

            }
        }
        else // 1.23E+3 형식일때 16자리 부터.
        {
            var tmp = int.Parse(sNumberList[1]) + 1;

            dKeepNumber = double.Parse(sNumberList[0].ToString().Replace("E", ""));

            if (tmp % 3 == 2 && tmp > 3) //16 = 1,  17 = 2,  18 = 0
            {
                tmp -= 1;
                sNumber = string.Format("{0:f3}", dKeepNumber * 10);
                sDigit = ((EnumNumber)tmp).ToString().Replace("num_", "");
            }
            else if (tmp % 3 == 0 && tmp > 3)
            {
                tmp -= 2;
                sNumber = string.Format("{0:f3}", dKeepNumber * 100);
                sDigit = ((EnumNumber)tmp).ToString().Replace("num_", "");
            }
            else
            {
                sDigit = ((EnumNumber)tmp).ToString().Replace("num_", "");
                sNumber = string.Format("{0:f3}", dKeepNumber);

            }

            if (tmp > 69)
            {
                sResult = "N/A";
            }

        }

        if (float.Parse(sNumber) >= 100f && sDigit == "CX")
        {
            sNumber = "100.000";
        }

        sResult = String.Format("{0}{1}", sNumber, sDigit);

        return sResult;

    }


    private int GetEnumNumber(string tmp)
    {
        int iResult = 0;

        switch (tmp)
        {
            case "A":
                iResult = 4;
                break;
            case "B":
                iResult = 7;
                break;
            case "C":
                iResult = 10;
                break;
            case "D":
                iResult = 13;
                break;
            case "E":
                iResult = 16;
                break;
            case "F":
                iResult = 19;
                break;
            case "G":
                iResult = 22;
                break;
            case "H":
                iResult = 25;
                break;
            case "I":
                iResult = 28;
                break;
            case "J":
                iResult = 31;
                break;
            case "K":
                iResult = 34;
                break;
            case "L":
                iResult = 37;
                break;
            case "M":
                iResult = 40;
                break;
            case "N":
                iResult = 43;
                break;
            case "O":
                iResult = 46;
                break;
            case "P":
                iResult = 49;
                break;
            case "Q":
                iResult = 52;
                break;
            case "R":
                iResult = 55;
                break;
            case "S":
                iResult = 58;
                break;
            case "T":
                iResult = 61;
                break;
            case "U":
                iResult = 64;
                break;
            case "V":
                iResult = 67;
                break;
            case "W":
                iResult = 70;
                break;
            case "X":
                iResult = 73;
                break;
            case "Y":
                iResult = 76;
                break;
            case "Z":
                iResult = 79;
                break;
            case "AA":
                iResult = 82;
                break;
            case "AB":
                iResult = 85;
                break;
            case "AC":
                iResult = 88;
                break;
            case "AD":
                iResult = 91;
                break;
            case "AE":
                iResult = 94;
                break;
            case "AF":
                iResult = 97;
                break;
            case "AG":
                iResult = 100;
                break;
            case "AH":
                iResult = 103;
                break;
            case "AI":
                iResult = 106;
                break;
            case "AJ":
                iResult = 109;
                break;
            case "AK":
                iResult = 112;
                break;
            case "AL":
                iResult = 115;
                break;
            case "AM":
                iResult = 118;
                break;
            case "AN":
                iResult = 121;
                break;
            case "AO":
                iResult = 124;
                break;
            case "AP":
                iResult = 127;
                break;
            case "AQ":
                iResult = 130;
                break;
            case "AR":
                iResult = 133;
                break;
            case "AS":
                iResult = 136;
                break;
            case "AT":
                iResult = 139;
                break;
            case "AU":
                iResult = 142;
                break;
            case "AV":
                iResult = 145;
                break;
            case "AW":
                iResult = 148;
                break;
            case "AX":
                iResult = 151;
                break;
            case "AY":
                iResult = 154;
                break;
            case "AZ":
                iResult = 157;
                break;
            case "BA":
                iResult = 160;
                break;
            case "BB":
                iResult = 163;
                break;
            case "BC":
                iResult = 166;
                break;
            case "BD":
                iResult = 169;
                break;
            case "BE":
                iResult = 172;
                break;
            case "BF":
                iResult = 175;
                break;
            case "BG":
                iResult = 178;
                break;
            case "BH":
                iResult = 181;
                break;
            case "BI":
                iResult = 184;
                break;
            case "BJ":
                iResult = 187;
                break;
            case "BK":
                iResult = 190;
                break;
            case "BL":
                iResult = 193;
                break;
            case "BM":
                iResult = 196;
                break;
            case "BN":
                iResult = 199;
                break;
            case "BO":
                iResult = 202;
                break;
            case "BP":
                iResult = 205;
                break;
            case "BQ":
                iResult = 208;
                break;
            case "BR":
                iResult = 211;
                break;
            case "BS":
                iResult = 214;
                break;
            case "BT":
                iResult = 217;
                break;
            case "BU":
                iResult = 220;
                break;
            case "BV":
                iResult = 223;
                break;
            case "BW":
                iResult = 226;
                break;
            case "BX":
                iResult = 229;
                break;
            case "BY":
                iResult = 232;
                break;
            case "BZ":
                iResult = 235;
                break;
            case "CA":
                iResult = 238;
                break;
            case "CB":
                iResult = 241;
                break;
            case "CC":
                iResult = 244;
                break;
            case "CD":
                iResult = 247;
                break;
            case "CE":
                iResult = 250;
                break;
            case "CF":
                iResult = 253;
                break;
            case "CG":
                iResult = 256;
                break;
            case "CH":
                iResult = 259;
                break;
            case "CI":
                iResult = 262;
                break;
            case "CJ":
                iResult = 265;
                break;
            case "CK":
                iResult = 268;
                break;
            case "CL":
                iResult = 271;
                break;
            case "CM":
                iResult = 274;
                break;
            case "CN":
                iResult = 277;
                break;
            case "CO":
                iResult = 280;
                break;
            case "CP":
                iResult = 283;
                break;
            case "CQ":
                iResult = 286;
                break;
            case "CR":
                iResult = 289;
                break;
            case "CS":
                iResult = 292;
                break;
            case "CT":
                iResult = 295;
                break;
            case "CU":
                iResult = 298;
                break;
            case "CV":
                iResult = 301;
                break;
            case "CW":
                iResult = 304;
                break;
            case "CX":
                iResult = 307;
                break;
            case "CY":
                iResult = 310;
                break;
            case "CZ":
                iResult = 313;
                break;


        }



        return iResult;
    } 

}

enum EnumNumber
{
    num_A = 4, // 네자리 수 부터
    num_B = 7,
    num_C = 10,
    num_D = 13,
    num_E = 16,
    num_F = 19,
    num_G = 22,
    num_H = 25,
    num_I = 28,
    num_J = 31,
    num_K = 34,
    num_L = 37,
    num_M = 40,
    num_N = 43,
    num_O = 46,
    num_P = 49,
    num_Q = 52,
    num_R = 55,
    num_S = 58,
    num_T = 61,
    num_U = 64,
    num_V = 67,
    num_W = 70,
    num_X = 73,
    num_Y = 76,
    num_Z = 79,
    num_AA = 82,
    num_AB = 85,
    num_AC = 88,
    num_AD = 91,
    num_AE = 94,
    num_AF = 97,
    num_AG = 100,
    num_AH = 103,
    num_AI = 106,
    num_AJ = 109,
    num_AK = 112,
    num_AL = 115,
    num_AM = 118,
    num_AN = 121,
    num_AO = 124,
    num_AP = 127,
    num_AQ = 130,
    num_AR = 133,
    num_AS = 136,
    num_AT = 139,
    num_AU = 142,
    num_AV = 145,
    num_AW = 148,
    num_AX = 151,
    num_AY = 154,
    num_AZ = 157,
    num_BA = 160,
    num_BB = 163,
    num_BC = 166,
    num_BD = 169,
    num_BE = 172,
    num_BF = 175,
    num_BG = 178,
    num_BH = 181,
    num_BI = 184,
    num_BJ = 187,
    num_BK = 190,
    num_BL = 193,
    num_BM = 196,
    num_BN = 199,
    num_BO = 202,
    num_BP = 205,
    num_BQ = 208,
    num_BR = 211,
    num_BS = 214,
    num_BT = 217,
    num_BU = 220,
    num_BV = 223,
    num_BW = 226,
    num_BX = 229,
    num_BY = 232,
    num_BZ = 235,
    num_CA = 238,
    num_CB = 241,
    num_CC = 244,
    num_CD = 247,
    num_CE = 250,
    num_CF = 253,
    num_CG = 256,
    num_CH = 259,
    num_CI = 262,
    num_CJ = 265,
    num_CK = 268,
    num_CL = 271,
    num_CM = 274,
    num_CN = 277,
    num_CO = 280,
    num_CP = 283,
    num_CQ = 286,
    num_CR = 289,
    num_CS = 292,
    num_CT = 295,
    num_CU = 298,
    num_CV = 301,
    num_CW = 304,
    num_CX = 307,
    num_CY = 310,
    num_CZ = 313
}



