using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace NekAverages
{
    /// <summary>
    /// This class is for making objects which will be added to the dictionary
    /// </summary>
    public class Grade
    {
        private int _weight;
        private float _value;
        public int Weight
        {
            get => _weight;
            set => _weight = value;
        }
        public float Value
        {
            get => _value;
            set => _value = value;
        }
        public Grade(int W, float V)
        {
            _weight = W;
            _value = V;
        }
    }
    public class Averages
    {
        /// <summary>
        /// Object to be returned by subject queries, contains Final and Average
        /// </summary>
        public class SubjResult
        {
            private float _average;
            private int _final;
            public int Final
            {
                get => _final;
            }
            public float Average
            {
                get => _average;
            }
            public SubjResult(int F, float A)
            {
                _average = A;
                _final = F;
            }
        }
        private float _threshold = 0.50F;
        /// <summary>
        /// Rounding threshold of the grades, threshold of 0.00 causes the grades to never be rounded 
        /// </summary>
        public float Threshold
        {
            get => _threshold;
            set => _threshold = (value < 1.00 ? value : throw new ArgumentException());
        }
        /// <summary>
        /// TODO: Add something here
        /// </summary>
        public Averages()
        {
            subjquery = from s in StandardSubjectNames.Concat(CustomSubjectNames) select s;
        }
        List<string> StandardSubjectNames = new List<string>()
        {
            "English",
            "Biology",
            "Chemistry",
            "EDB",
            "Physics",
            "Geography",
            "History",
            "Computer_Sciences",
            "Mathematics",
            "Music",
            "German",
            "Art",
            "Polish",
            "Natural_Sciences",
            "RE",
            "Technology",
            "PE",
            "Citizenship"
        };
        List<string> CustomSubjectNames = new List<String>();
        private IEnumerable<string> subjquery;
        /// <summary>
        /// This method creates base files for around 20 base supported subjects, as the files will be overwritten the client should include a warning before performing this action. Just for the safety if they exists they will be copied to the \Backups folder.
        /// </summary>
        public void Init()
        {
            string curdir = Directory.GetCurrentDirectory();
            if(!Directory.Exists(curdir + @"\Grades\Backup"))
            {
                Directory.CreateDirectory(curdir + @"\Grades\Backup");
            }
            foreach(var file in Directory.EnumerateFiles(curdir + @"\Grades"))
            {
                string filename = Regex.Match(file, @"\\[0-9a-zA-Z_\-. ]+.txt$").Value.Substring(1);
                File.Move(file, curdir + @"\Backup\Grades\" + filename);
                File.Delete(file);
            }
            foreach(var item in subjquery)
            {
                using(File.CreateText(curdir + @"\Grades\" + item + ".txt"));
            }
        }
        public Dictionary<string,int> Final;
        public Dictionary<string,List<Grade>> Subjects;
        public Dictionary<string,List<Grade>> MidYearSubjects;
        private int CustomRound(float value, float threshold)
        {
            int valfirst = (int)Char.GetNumericValue(value.ToString().First());
            if(Threshold == 0) return valfirst;
            if(value >= (float)valfirst + threshold)
            {
                return valfirst + 1;
            }
            else
            {
                return valfirst;
            }
        }
        public int GetSubjectFinal(string subj)
        {
            float A = GetSubjectAverage(subj);
            return CustomRound(A, Threshold);
        }
        public int GetSubjectFinal(string subj, ref float A)
        {
            A = GetSubjectAverage(subj);
            return CustomRound(A, Threshold);
        }
        public float GetSubjectAverage(string subj)
        {
            try
            {
                int weights = 0;
                float total = 0;
                foreach(var item in Subjects[subj])
                {
                    total += item.Value;
                    weights += item.Weight;
                }
                return total / weights;
            }
            catch
            {
                return -1.0F;
            }
        }
        public SubjResult GetSubjectResults()
        {

        }
        public Task<int> ReadFiles()
        {
            int returnstatus = 1;
            foreach(var file in Directory.EnumerateFiles(Directory.GetCurrentDirectory() + @"\Grades"))
            {
                string subjname = Regex.Match(file, @"\\[0-9a-zA-Z_\-. ]+.txt$").Value.Substring(1);
                if(subjquery.Contains(subjname))
                {
                    foreach(var line in File.ReadAllLines(file))
                    {
                        string[] ints = line.Split(';');
                        Grade grade = new Grade(int.Parse(ints[0]), float.Parse(ints[1]));
                        Subjects[subjname].Add(grade);
                    }
                }
            }
            return Task.FromResult(returnstatus);
        }
    }
}
