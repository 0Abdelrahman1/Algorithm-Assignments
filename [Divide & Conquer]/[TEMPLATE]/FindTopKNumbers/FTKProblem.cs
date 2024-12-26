using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Problem
{

    public class Problem : ProblemBase, IProblem
    {
        #region ProblemBase Methods
        public override string ProblemName { get { return "FindTopKNumbers"; } }

        public override void TryMyCode()
        {
            /* WRITE 4~6 DIFFERENT CASES FOR TRACE, EACH WITH
             * 1) SMALL INPUT SIZE
             * 2) EXPECTED OUTPUT
             * 3) RETURNED OUTPUT FROM THE FUNCTION
             * 4) PRINT THE CASE 
             */
            int k = 0;
            int[] output ,expected;

            //
            k = 4;
            int[] arr1 = { -1, -3, 12, - 4,   1, 8, 10, 6, 3, 4, };
            expected =new int[] {12,10,8,6};
            output = global::Problem.PROBLEM_CLASS.RequiredFunction(arr1, k);
            PrintCase(k, arr1, output, expected);

            //
            k = 5;
            int[] arr2 = { 50,-10,6,95,1,4,3,0,8,55,53 };
            expected = new int[] { 95,55,53,50,8};
            output = global::Problem.PROBLEM_CLASS.RequiredFunction(arr2, k);
            PrintCase(k, arr2, output, expected);

            //
            k = 2;
            int[] arr3 = { -4, -3, -1, 3, 4, 5, 6, 8, 10, 12 };
            expected = new int[] {12,10};
            output = global::Problem.PROBLEM_CLASS.RequiredFunction(arr3, k);
            PrintCase(k, arr3, output, expected);

            //
            k = 3;
            int[] arr4 = { -9, -5, 2, 7, 1, 3, 6, -8, 7 };
            expected = new int[] {7,7,6};
            output = global::Problem.PROBLEM_CLASS.RequiredFunction(arr4, k);
            PrintCase(k, arr4, output, expected);
        }

        Thread tstCaseThr;
        bool caseTimedOut ;
        bool caseException;

        protected override void RunOnSpecificFile(string fileName, HardniessLevel level, int timeOutInMillisec)
        {
            /* READ THE TEST CASES FROM THE SPECIFIED FILE, FOR EACH CASE DO:
             * 1) READ ITS INPUT & EXPECTED OUTPUT
             * 2) READ ITS EXPECTED TIMEOUT LIMIT (IF ANY)
             * 3) CALL THE FUNCTION ON THE GIVEN INPUT USING THREAD WITH THE GIVEN TIMEOUT 
             * 4) CHECK THE OUTPUT WITH THE EXPECTED ONE
             */

            int testCases;
            int N = 0;
            int k = 0;
            int[] arr = null;
            int []output, actualResult;
            output =new int[] {0};

            Stream s = new FileStream(fileName, FileMode.Open);
            BinaryReader br = new BinaryReader(s);

            testCases = br.ReadInt32();

            int totalCases = testCases;
            int correctCases = 0;
            int wrongCases = 0;
            int timeLimitCases = 0;
            bool readTimeFromFile = false;
            if (timeOutInMillisec == -1)
            {
                readTimeFromFile = true;
            }
            int i = 1;
            while (testCases-- > 0)
            {
                N = br.ReadInt32();
                k = br.ReadInt32();
                actualResult = new int[k];
                arr = new int[N];
                for (int j = 0; j < N; j++)
                {
                    arr[j] = br.ReadInt32();
                }
                for (int j = 0; j < k; j++)
                {
                    actualResult[j] = br.ReadInt32();
                }
                //Console.WriteLine("N = {0}, Res = {1}", N, actualResult);
                Stopwatch sw = null;
                caseTimedOut = true;
                caseException = false;
                {
                    if (readTimeFromFile)
                    {
                        timeOutInMillisec = br.ReadInt32();
                    }
                    /*LARGE TIMEOUT FOR SAMPLE CASES TO ENSURE CORRECTNESS ONLY*/
                    if (level == HardniessLevel.Easy)
                    {
                        timeOutInMillisec = 100; //Large Value 
                    }
                    tstCaseThr = new Thread(() =>
                    {
                        try
                        {
                            int sum = 0;
                            int numOfRep = 1;
                            sw = Stopwatch.StartNew();
                            for (int x = 0; x < numOfRep; x++)
                            {
                                output= global::Problem.PROBLEM_CLASS.RequiredFunction(arr, k);
                            }
                            sw.Stop();
                            Console.WriteLine("N = {0}, time in ms = {1}, timeout = {2}", arr.Length, sw.ElapsedMilliseconds, timeOutInMillisec);
                        }
                        catch
                        {
                            caseException = true;
                            output = new int[] {0};
                        }
                        caseTimedOut = false;
                    });

                   
                    /*=========================================================*/
                    tstCaseThr.Start();
                    tstCaseThr.Join(timeOutInMillisec);
                }
                if (caseTimedOut)       //Timedout
                {
                    Console.WriteLine("Time Limit Exceeded in Case {0}.", i);
                    tstCaseThr.Abort();
                    timeLimitCases++;
                }
                else if (caseException) //Exception 
                {
                    Console.WriteLine("Exception in Case {0}.", i);
                    wrongCases++;
                }
                else if (output.SequenceEqual(actualResult))    //Passed
                {
                    Console.WriteLine("Test Case {0} Passed!", i);
                    correctCases++;
                }
                else                    //WrongAnswer
                {
                    Console.WriteLine("Wrong Answer in Case {0}.", i);
                    Console.WriteLine(" your answer = " + output + ", correct answer = " + actualResult);
                    wrongCases++;
                }

                i++;
            }
            s.Close();
            br.Close();
            Console.WriteLine();
            Console.WriteLine("# correct = {0}", correctCases);
            Console.WriteLine("# time limit = {0}", timeLimitCases);
            Console.WriteLine("# wrong = {0}", wrongCases);
            Console.WriteLine("\nFINAL EVALUATION (%) = {0}", Math.Round((float)correctCases / totalCases * 100, 0));
        }
        
        protected override void OnTimeOut(DateTime signalTime)
        {
        }

        /// <summary>
        /// Generate a file of test cases according to the specified params
        /// </summary>
        /// <param name="level">Easy or Hard</param>
        /// <param name="numOfCases">Required number of cases</param>
        /// <param name="includeTimeInFile">specify whether to include the expected time for each case in the file or not</param>
        /// <param name="timeFactor">factor to be multiplied by the actual time</param>
        public override void GenerateTestCases(HardniessLevel level, int numOfCases, bool includeTimeInFile = false, float timeFactor = 1)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Helper Methods
        private static void PrintCase(int k, int[] arr, int []output, int []expected)
        {
            /* PRINT THE FOLLOWING
             * 1) INPUT
             * 2) EXPECTED OUTPUT
             * 3) RETURNED OUTPUT
             * 4) WHETHER IT'S CORRECT OR WRONG
             * */
            Console.WriteLine("K: {0}", k);

            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write(arr[i] + " ");
            }
            Console.WriteLine();
            Console.Write("Output = ");
            for (int i=0;i<k;i++)
                Console.Write(output[i]+ " ");
            Console.WriteLine(" ");
            Console.Write("Expected = ");
            for (int i = 0; i < k; i++)
                Console.Write(expected[i]+" ");
            Console.WriteLine();

            if (output.SequenceEqual(expected))
            {
                Console.WriteLine("CORRECT");
            }
            else
            {
                Console.WriteLine("WRONG");
            }
            Console.WriteLine("-----------------------------");
        }

        #endregion

    }
}
