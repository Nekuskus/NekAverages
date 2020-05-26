using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;

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
    }
    public class Averages
    {
        /// <summary>
        /// This method creates base files for around 20 base supported subjects, as the files will be overwritten the client should include a warning before performing this action. Just for the safety if they exists they will be copied to the \Backups folder.
        /// </summary>
        public void Init()
        {

        }
        public Dictionary<string,int> Final;
        public Dictionary<string,Grade[]> Subjects;
        public Dictionary<string,Grade[]> MidYearSubjects;
        public void Main()
        {
            
        }
    }
}
