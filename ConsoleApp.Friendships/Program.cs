﻿using System;
using System.IO;
using ConsoleApp.Friendships.Friendships;
using CSharpToSqlMapper;
using static DataGenerator.DataGeneration;

namespace ConsoleApp.Friendships
{
    class Program
    {
        public const string FriendshipTag = "Friendships";
        static void Main(string[] args)
        {
            var mapper = new Mapper();
            mapper.Schema.Name = "if150185";
            mapper.AddTable<Person>(FriendshipTag);
            mapper.AddTable<Friendship>(FriendshipTag);

            mapper.Initialize();

            var persons = mapper.InsertsFor<Person>(105, FriendshipTag)
                .Set(e => e.Name, Join(
                    RepeatEach(SequentialFrom("Twilight", "Dawn", "Strawberry", "Apple", "Orange", "Starlight", "Sunburst", "Trixie", "Cadence", "Discord", "Flutter", "Luna", "Celestia", "Tempest", "Fizzlepop"), 7),
                    Value(" "),
                    SequentialFrom("Sparkle", "Dash", "Glimmer", "Wings", "McHorseface", "Shadow", "Berrytwist")
                    ))
                .Set(e => e.Age, Random(5, 5000));

            var friendships = mapper.InsertsFor<Friendship>(300, FriendshipTag)
                .Set(e => e.PersonA, RandomFrom(persons))
                .Set(e => e.PersonB, RandomFrom(persons))
                .Set(e => e.FromDate, Random(DateTime.Now.AddDays(-60), DateTime.Now))
                .Set(e => e.ToDate, Random(DateTime.Now, DateTime.Now.AddDays(60)));

            string path = Path.GetFullPath("D:/Stefan/HTL/5BHIF/DBI/2020-03-05 Friendships/Generated");
            if (!Directory.Exists(path))
            {
                Console.Error.WriteLine("Please create the following directory: " + path);
                return;
            }

            OutputSqlFiles(path, mapper, 1, FriendshipTag);
        }

        private static void OutputSqlFiles(string path, Mapper mapper, int outputIndex, string tag)
        {
            OutputText(path, $"{outputIndex}_1_Tables_{tag}.sql", mapper.ToStringCreateOrReplace(tag));
            OutputText(path, $"{outputIndex}_2_Constraints_{tag}.sql", mapper.ToStringAlter(tag));
            OutputText(path, $"{outputIndex}_3_Triggers_{tag}.sql", mapper.ToStringTriggersAndSequences(tag));
            OutputText(path, $"{outputIndex}_4_Inserts_{tag}.sql", mapper.ToStringInserts(tag));
        }

        public static void OutputText(string path, string fileName, string content)
        {
            bool outputToFile = true;
            string filePath = Path.Combine(path, fileName);
            if (outputToFile)
            {
                File.WriteAllText(filePath, "--Autogenerated, do not edit\n");
                File.AppendAllText(filePath, content);
            }
            else
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"-- {filePath}");
                Console.ForegroundColor = color;
                Console.WriteLine(content);
                Console.WriteLine();
            }
        }
    }
}