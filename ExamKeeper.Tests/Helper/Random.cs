using System;
using System.Collections.Generic;
using System.Linq;

namespace ExamKeeper.Tests {
    public static class RandomHelper {
        private static Random random = new Random();

        public static string RandomString(int length, bool includeNum = true) {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            if (includeNum) {
                chars += "0123456789";
            }
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RandomString(int minLength, int maxLength) {
            int length = random.Next(minLength, maxLength + 1);
            return RandomString(length);
        }
        public static int RandomNumber(int digit) {
            string chars = "0123456789";
            string numberString = new string(Enumerable.Repeat(chars, digit)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return Convert.ToInt32(numberString);
        }
        public static T RandomPick<T>(List<T> list) {
            var random = new Random();
            int index = random.Next(list.Count);
            return list[index];
        }
        public static List<T> RandomPick<T>(List<T> list, int amount) {
            if (list.Count == amount) {
                return list;
            }
            if (list.Count < amount) {
                amount = list.Count;
            }
            List<T> result = new List<T>();
            for (int i = 0; i < amount; i++) {
                int index = random.Next(list.Count);
                result.Add(list[index]);
                list.RemoveAt(index);
            }
            return list;
        }
    }
}