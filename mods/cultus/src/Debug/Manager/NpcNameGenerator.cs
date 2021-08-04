using System;

namespace cultus
{
    public class NpcNameGenerator
    {
        public string GetRandomName()
        {
            string[] nameParts = new string[] {
                "Ar",
                "Mok",
                "Ul",
                "Kar",
                "Grok",
                "Mor",
                "Ala",
                "Kon",
                "Uro"
            };

            int newNamePartCount = new Random().Next(1, 4);
            string newName = string.Empty;
            Random random = new Random();

            for (int i = 0; i <= newNamePartCount; i++)
            {
                newName += nameParts[random.Next(1, nameParts.Length)];
            }

            return CapitalizeFirstLetter(newName);
        }

        private static string CapitalizeFirstLetter(string str)
        {
            return char.ToUpper(str[0]) + str.Substring(1);
        }
    }
}