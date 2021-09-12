using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using Isu.ModuleFolder;

namespace Isu.ModuleFolder
{
    public class Group
    {
        private Group(uint number, CourseNumber course)
        {
            this.Number = number;
            this.Course = course;

            StudentsList = new List<Student>();
        }

        public uint Number { get; private set; }
        public CourseNumber Course { get; private set; }

        public List<Student> StudentsList { get; set; }

        public static Group CreateInstance(uint name, CourseNumber course)
        {
            return new Group(name, course);
        }
    }
}