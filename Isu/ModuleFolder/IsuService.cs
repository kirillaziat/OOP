using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using Isu.ModuleFolder;
using Isu.Services;
using Isu.Tools;

namespace Isu.ModuleFolder
{
    public class IsuService : IIsuService
    {
        private readonly List<Group> groupBase;
        private readonly List<Student> globalStudentsList;

        private int id = 0;

        public IsuService()
        {
            globalStudentsList = new List<Student>();
            groupBase = new List<Group>();
        }

        public Group CreateGroupAndCheckCorrectness(string name)
        {
            if (name == null || (name.Length != 5))
            {
                throw new IsuException("Invalid group name");
            }

            string courseForGroup = name[2].ToString();
            string numberForGroup = name.Substring(3, 2);

            if (!uint.TryParse(courseForGroup, out uint course))
            {
                throw new IsuException("Invalid Group course");
            }

            if (!uint.TryParse(numberForGroup, out uint number))
            {
                throw new IsuException("Invalid Group name");
            }

            var group = Group.CreateInstance(number, CourseNumber.CreateInstance(course));

            return group;
        }

        public Group AddGroup(string name)
        {
            Group group = CreateGroupAndCheckCorrectness(name);

            if (groupBase.All(groupTemp => groupTemp != group))
            {
                groupBase.Add(group);
            }

            return group;
        }

        public Student AddStudent(Group group, string name)
        {
            if (name == null)
            {
                throw new IsuException("Invalid student name");
            }

            if (group.Number >= 100 || group.Course.Course == 0 || group.Course.Course > 4
                || groupBase.All(groupTemp => groupTemp.Number != group.Number) || group.StudentsList.Count > 30
                || (groupBase.Any() && groupBase.All(groupTemp => groupTemp != group)))
            {
                throw new IsuException("Invalid group");
            }

            var student = Student.CreateInstance(id, name, group);
            id += 1;

            if (group.StudentsList.All(studentTemp => studentTemp != student))
            {
                group.StudentsList.Add(student);
                globalStudentsList.Add(student);
            }

            return student;
        }

        public Student GetStudent(int id)
        {
            if (id < 0 || id > this.id)
            {
                throw new IsuException("Invalid ID");
            }

            foreach (Student student in globalStudentsList)
            {
                if (student.Id == id)
                {
                    return student;
                }
            }

            return null;
        }

        public Student FindStudent(string name)
        {
            foreach (Student student in globalStudentsList)
            {
                if (student.Name == name)
                {
                    return student;
                }
            }

            return null;
        }

        public List<Student> FindStudents(string groupName)
        {
            Group group = CreateGroupAndCheckCorrectness(groupName);

            foreach (Group i in groupBase)
            {
                if (i.Course.Course == group.Course.Course && i.Number == group.Number)
                {
                    return i.StudentsList;
                }
            }

            return null;
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            if (courseNumber.Course <= 0 || courseNumber.Course > 4)
            {
                throw new IsuException("Invalid course");
            }

            foreach (Group i in groupBase)
            {
                if (i.Course.Course == courseNumber.Course)
                {
                    return i.StudentsList;
                }
            }

            return null;
        }

        public Group FindGroup(string groupName)
        {
            Group group = CreateGroupAndCheckCorrectness(groupName);

            foreach (Group i in groupBase)
            {
                if (i.Number == group.Number && i.Course.Course == group.Course.Course)
                {
                    return i;
                }
            }

            return null;
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            if (courseNumber.Course <= 0 || courseNumber.Course > 4 || courseNumber == null)
            {
                throw new IsuException("Invalid Course");
            }

            var groupsByCourse = new List<Group>();

            foreach (Group group in groupBase)
            {
                if (group.Course.Course == courseNumber.Course)
                {
                    groupsByCourse.Add(group);
                }
            }

            return groupsByCourse;
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            if (student.Id < 0 || student.Id > id)
            {
                throw new IsuException("Invalid Student");
            }

            if (globalStudentsList.All(studentTemp => studentTemp != student))
            {
                throw new IsuException("Invalid student is not in the ITMO base");
            }

            student.Group.StudentsList.Remove(student);

            if (groupBase.Any(groupTemp => groupTemp == newGroup))
            {
                newGroup.StudentsList.Add(student);
            }
            else
            {
                groupBase.Add(newGroup);
                newGroup.StudentsList.Add(student);
            }
        }
    }
}