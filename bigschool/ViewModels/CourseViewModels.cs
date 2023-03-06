using bigschool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bigschool.ViewModels
{
    public class CourseViewModels
    {
        public String Place { get; set; }
        public String Date { get; set; }
        public String Time { get; set; }
        public byte Category { get; set; }
        public IEnumerable<Category> categories { get; set; }
        public DateTime GetDateTime()
        {
            return DateTime.Parse(String.Format("{0}{1}",Date,Time));
        }
    }
}