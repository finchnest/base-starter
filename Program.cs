using System;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis.Sarif;

namespace baselining_tool
{
    class Program
    {
        private const int V = 2;
        private Changes changes;

        internal Changes Changes { get => changes; set => changes = value; }

        //command-line structure: baseline-tool -baseline=baseline.sarif -output=identical
        static void Main(string[] args)
        {
            if (args.Length > V)
            {

                string mFilePath = @"" + args[1];
                string ext = Path.GetExtension(mFilePath);
                if (ext != ".sarif")
                {
                    baselineLogger("Expect a .sarif file but got " + ext);
                    //halt execution
                }
                SarifLog sarifLog = SarifLog.Load(args[1]);

                baselineLogger("Input recieved. checking format...");
                bool validFormat = checkFormat(sarifLog);
                if (validFormat == false)
                {

                    baselineLogger("The saif file is not a proper baseline file." +
                    " Use command Sarif -match-results-forward current.sarif --previous=baseline.sarif -output=output-file-path");
                    //halt execution
                }



            }
            else
            {
                baselineLogger("Parameters not entered correctly");
            }
        }

        private static void baselineLogger(string v)
        {
            string filePath = "\baselining-tool";
            StringBuilder sb = new StringBuilder();
            sb.Append(v);
            File.AppendAllText(filePath + "log.txt", sb.ToString());
            sb.Clear();
        }

        private static bool checkFormat(SarifLog sarifLog)
        {
            string count = sarifLog.Runs[0].Results[0].GetProperty("baselineState");
            if (count == null || count == "")
            {
                return false;
            }
            return true;

        }
    }

}
