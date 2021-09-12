using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;

namespace Isu.ModuleFolder
{
    public class CourseNumber
    {
        private CourseNumber(uint course) => this.Course = course;

        public uint Course { get; private set; }

        public static CourseNumber CreateInstance(uint course)
        {
            return new CourseNumber(course);
        }
    }
}