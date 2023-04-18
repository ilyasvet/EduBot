﻿using System.Collections.Generic;

namespace Simulator.Models
{
    internal class CaseStageEndModule : CaseStage
    {
        public bool IsEndOfCase { get; set; }
        public List<double> Rates { get; set; }
        public List<string> Texts { get; set; }
        public int ModuleNumber { get; set; }
        public CaseStageEndModule(
            int number,
            string textBefore
            )
            : base(number, textBefore)
        {
            Rates = new List<double>();
            Texts = new List<string>();
        } 
    }
}