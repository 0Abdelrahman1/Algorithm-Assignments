using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using static Problem.PROBLEM_CLASS;

namespace Problem
{


	public class Problem : ProblemBase, IProblem
	{
		#region ProblemBase Methods
		public override string ProblemName { get { return "NeverRetrun"; } }

		public override void TryMyCode()
		{
			int n = 5;
			// Initialize landmarks
			List<Landmark> landmarks1 = new List<Landmark>
		{
			new Landmark(0, -54, 4, true),
			new Landmark(1, 1885, -2334, true),
			new Landmark(2, -524, 3047, true),
			new Landmark(3, -1179, 6405, false),
			new Landmark(4, -31, 4127, false)
		};

			// Initialize edges
			List<Tuple<int, int, int>> edges1 = new List<Tuple<int, int, int>>
		{
			Tuple.Create(0, 1, 3245),
			Tuple.Create(1, 2, 5614),
			Tuple.Create(0, 2, 2367),
			Tuple.Create(2, 4, 9259),
			Tuple.Create(2, 3, 8561),
			Tuple.Create(1, 3, 1792),
			Tuple.Create(3, 4, 4241)
		};
			int expected1 = 5037;
			int output1 = PROBLEM_CLASS.RequiredFunction(landmarks1, edges1, n);
			PrintCase(landmarks1, edges1, output1, expected1);


			int n2 = 6;
			// Initialize landmarks with the new data set
			List<Landmark> landmarks2 = new List<Landmark>
		{
			new Landmark(0, 0, 97, true),
			new Landmark(1, 1738, 8252, true),
			new Landmark(2, -6434, -2962, true),
			new Landmark(3, -1033, 3411, true),
			new Landmark(4, 15110, 12691, false), // First outside
            new Landmark(5, -14106, -16055, false) // Second outside
        };

			// Initialize edges based on the provided data
			List<Tuple<int, int, int>> edges2 = new List<Tuple<int, int, int>>
		{
			Tuple.Create(0, 3, 233),
			Tuple.Create(0, 2, 185),
			Tuple.Create(2, 3, 8838),
			Tuple.Create(1, 3, 1648),
			Tuple.Create(1, 2, 6402),
			Tuple.Create(0, 1, 843),
			Tuple.Create(2, 4, 913),
			Tuple.Create(3, 5, 9041),
			Tuple.Create(4, 5, 1370)
		};
			int expected2 = 1098;
			int output2 = PROBLEM_CLASS.RequiredFunction(landmarks2, edges2, n2);
			PrintCase(landmarks1, edges1, output2, expected2);

			int n3 = 4;
			List<Landmark> landmarks3 = new List<Landmark>
		{
			new Landmark(0, -60, -1, true), // Inside
            new Landmark(1, 3499, 5087, true), // Inside
            new Landmark(2, -1112, -4096, true), // Inside
            new Landmark(3, 8898, 4355, false) // Outside
        };

			// Initializing edges based on the new provided data
			List<Tuple<int, int, int>> edges3 = new List<Tuple<int, int, int>>
		{
			Tuple.Create(0, 2, 9536),
			Tuple.Create(1, 2, 1526),
			Tuple.Create(0, 1, 2557),
			Tuple.Create(0, 3, 9877),
			Tuple.Create(1, 3, 3187),
			Tuple.Create(2, 3, 5762)
		};

			int expected3 = 5744;
			int output3 = PROBLEM_CLASS.RequiredFunction(landmarks3, edges3, n3);
			PrintCase(landmarks3, edges3, output3, expected3);
		}



		Thread tstCaseThr;
		bool caseTimedOut;
		bool caseException;

		protected override void RunOnSpecificFile(string fileName, HardniessLevel level, int timeOutInMillisec)
		{
			int testCases;
			int actualResult = -1;
			int output = -1;

			FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);

			StreamReader sr = new StreamReader(file);
			string line = sr.ReadLine();
			testCases = int.Parse(line);

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
				line = sr.ReadLine();
				int v = int.Parse(line);
				line = sr.ReadLine();
				int e = int.Parse(line);
				var landmarks = new List<Landmark>();
				var edges = new List<Tuple<int, int, int>>();
				for (int j = 0; j < v; j++)
				{
					line = sr.ReadLine();
					var lineParts = line.Split(',');
					landmarks.Add(new Landmark(int.Parse(lineParts[0]), int.Parse(lineParts[1]), int.Parse(lineParts[2]), bool.Parse(lineParts[3])));
				}				
				for (int j = 0; j < e; j++)
				{
					line = sr.ReadLine();
					var lineParts = line.Split(',');
				
					edges.Add(new Tuple<int, int, int>(int.Parse(lineParts[0]), int.Parse(lineParts[1]), int.Parse(lineParts[2])));
				}
				line = sr.ReadLine();
				actualResult = int.Parse(line);
				caseTimedOut = true;
				caseException = false;
				{
					tstCaseThr = new Thread(() =>
					{
						try
						{
							Stopwatch sw = Stopwatch.StartNew();
							output = PROBLEM_CLASS.RequiredFunction(landmarks,edges,v);
							sw.Stop();
							//PrintCase(vertices,edges, output, actualResult);
							Console.WriteLine("|V| = {0}, |E| = {1}, time in ms = {2}", v, edges.Count, sw.ElapsedMilliseconds);
							Console.WriteLine("{0}", output);

						}
						catch (Exception ex)
						{
							Console.WriteLine(ex.Message);
							caseException = true;
							output = -1;
						}
						caseTimedOut = false;
					});

					//StartTimer(timeOutInMillisec);
					if (readTimeFromFile)
					{
						timeOutInMillisec = int.Parse(sr.ReadLine().Split(':')[1]);
					}
                    /*LARGE TIMEOUT FOR SAMPLE CASES TO ENSURE CORRECTNESS ONLY*/
                    if (level == HardniessLevel.Easy)
                    {
                        timeOutInMillisec = 100; //Large Value 
                    }
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
				else if (output == actualResult)    //Passed
				{
					Console.WriteLine("Test Case {0} Passed!", i);
					correctCases++;
				}
				else                    //WrongAnswer
				{
					Console.WriteLine("Wrong Answer in Case {0}.", i);
					Console.WriteLine(" your answer = {0}, correct answer = {1}", output, actualResult);
					wrongCases++;
				}

				i++;

				GC.Collect();
			}
			file.Close();
			sr.Close();
			Console.WriteLine();
			Console.WriteLine("# correct = {0}", correctCases);
			Console.WriteLine("# time limit = {0}", timeLimitCases);
			Console.WriteLine("# wrong = {0}", wrongCases);
			Console.WriteLine("\nFINAL EVALUATION (%) = {0}", Math.Round((float)correctCases / totalCases * 100, 0));
		}

		protected override void OnTimeOut(DateTime signalTime)
		{
		}

		public override void GenerateTestCases(HardniessLevel level, int numOfCases, bool includeTimeInFile = false, float timeFactor = 1)
		{
            throw new NotImplementedException();

		}

		#endregion

		#region Helper Methods
		private static void PrintCase(List<Landmark> land, List<Tuple<int, int, int>> edges, int output, int expected)
		{
			Console.WriteLine("Landmarkers: ");

			for (int i = 0; i < land.Count; i++)
			{
				Console.WriteLine("ID= {0}, X= {1}, Y= {2}, Inside= {3}", land[i].Id, land[i].X, land[i].Y, land[i].IsInside);
			}
			Console.WriteLine("Edges: ");
			for (int i = 0; i < edges.Count; i++)
			{
				Console.WriteLine("{0}, {1}, {2}", edges[i].Item1, edges[i].Item2, edges[i].Item3);
			}
			Console.WriteLine("Output: {0}", output);
			Console.WriteLine("Expected: {0}", expected);
			if (output == expected)    //Passed
			{
				Console.WriteLine("CORRECT");
			}
			else                    //WrongAnswer
			{
				Console.WriteLine("WRONG");
			}
			Console.WriteLine();
		}

		#endregion

	}

}