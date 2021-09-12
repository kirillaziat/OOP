using System;
using System.Collections.Generic;
using System.Linq;
using Isu.ModuleFolder;
using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    public class Tests
    {
        private IsuService isuService = new IsuService();

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            Group group = isuService.AddGroup("M3106");
            Student kirill = isuService.AddStudent(group, "Misha");
            
            Assert.True(kirill != null);
            Assert.True(kirill == isuService.FindStudent(kirill.Name));
            Assert.True(isuService.FindStudents("M3106").All(student => student == kirill));
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                Group group = isuService.AddGroup("M3107");

                for (int i = 0; i < 35; i++)
                {
                    isuService.AddStudent(group, "name");
                }
            });
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                Group invalidGroup = isuService.AddGroup("00000000000");
            });
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            Group group8 = isuService.AddGroup("M3208");
            Group group9 = isuService.AddGroup("M3209");

            Student kirill = isuService.AddStudent(group8, "Kirill");
            Student dmitriy = isuService.AddStudent(group9, "Dmitriy");

            isuService.ChangeStudentGroup(dmitriy, group8);
                
            Assert.True(isuService.FindStudents("M3208").Contains(kirill));
            Assert.True(!isuService.FindStudents("M3209").Contains(dmitriy));
        }
    }
}