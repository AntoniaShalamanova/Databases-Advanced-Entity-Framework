﻿using Newtonsoft.Json;
using System;

namespace CustomAutoMapper
{
    class StartUp
    {
        static void Main(string[] args)
        {
            var person = new Person()
            {
                FirstName = "Pesho",
                LastName = "Peshov",
                address = new Address()
                {
                    City = "Sofia",
                    Street = "Vitosha",
                    Number = 1
                }
            };

            var mapper = new Mapper();

            var student = mapper.Map<Student>(person);

            Console.WriteLine(JsonConvert.SerializeObject(student));
        }
    }
}
