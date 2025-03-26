﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniCabinet.Core.Models.ViewModel.Exam
{
    public class ExamResultItemVM
    {
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }

        public decimal GradeAvarage { get; set; }
        public decimal FinalGrade { get; set; }
        public bool IsAutomatic { get; set; }
    }

}
