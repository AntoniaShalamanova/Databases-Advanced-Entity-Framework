using System;
using System.Collections.Generic;
using P01_StudentSystem.Data;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            string[] courseNames =
            {
                "Advanced",
                "Fundamentals",
                "Databases",
                "Basic"
            };

            string[] languageNames =
            {
                "C#",
                "Java",
                "JS",
                "Python",
                "C++",
                "GO",
                "Ruby",
                "PHP"
            };

            var courses = GenerateCourses(courseNames, languageNames);

            SeedCourses(courses);
        }

        private static List<Course> GenerateCourses(string[] courseNames, string[] languageNames)
        {
            var courses = new List<Course>();

            foreach (var course in courseNames)
            {
                foreach (var language in languageNames)
                {
                    string name = $"{language} {course}";

                    courses.Add(new Course
                    {
                        Name = name,
                        Description = $"Course for {language}",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(60),
                        Price = 500
                    });
                }
            }

            return courses;
        }

        private static void SeedCourses(List<Course> coursesToSeed)
        {
            using (var db = new StudentSystemContext())
            {
                db.Courses.AddRange(coursesToSeed);

                db.SaveChanges();
            }
        }
    }
}
