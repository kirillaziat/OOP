using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;

namespace Isu.ModuleFolder
{
    public class Student
    {
        public Student(int id, string name, Group group)
        {
            Id = id;
            Name = name;
            Group = group;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }

        public Group Group { get; set; }

        public static Student CreateInstance(int id, string name, Group group)
        {
            return new Student(id, name, group);
        }
    }
}