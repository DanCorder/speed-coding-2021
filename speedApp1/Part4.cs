using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace speedApp1
{
    class Part4
    {
        private const string Week0Path = @"d:\work\speed-coding-2021\input\part4\Week0.json";
        private const string Week1Path = @"d:\work\speed-coding-2021\input\part4\Week1.json";
        private const string Week2Path = @"d:\work\speed-coding-2021\input\part4\Week2.json";
        private const string Week3Path = @"d:\work\speed-coding-2021\input\part4\Week3.json";
        public static void Run()
        {
            var Week0 = JsonConvert.DeserializeObject<Requests>(
                File.ReadAllText(Week0Path));
            var Week1 = JsonConvert.DeserializeObject<Requests>(
                File.ReadAllText(Week1Path));
            var Week2 = JsonConvert.DeserializeObject<Requests>(
                File.ReadAllText(Week2Path));
            var Week3 = JsonConvert.DeserializeObject<Requests>(
                File.ReadAllText(Week3Path));

            Console.WriteLine(GetDetails(0, Week0));
            Console.WriteLine(GetDetails(1, Week1));
            Console.WriteLine(GetDetails(2, Week2));
            Console.WriteLine(GetDetails(3, Week3));
        }

        private static string GetDetails(int week, Requests weekRequests)
        {
            var conflictsByDay = new Dictionary<int, Dictionary<MeetingRequest, MeetingRequest>>();
            int lunchesMissed = 0;
            var requestsByDay = weekRequests.MeetingRequests.GroupBy(r => r.Day).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var requests in requestsByDay)
            {
                conflictsByDay[requests.Key] = new Dictionary<MeetingRequest, MeetingRequest>();
                for (int i = 0; i < requests.Value.Count - 1; i++)
                {
                    for (int j = i + 1; j < requests.Value.Count; j++)
                    {
                        if (Overlap(requests.Value[i].StartTime, requests.Value[i].LengthInHalfHours, requests.Value[j].StartTime, requests.Value[j].LengthInHalfHours))
                        {
                            conflictsByDay[requests.Key][requests.Value[i]] = requests.Value[j];
                            conflictsByDay[requests.Key][requests.Value[j]] = requests.Value[i];
                        }
                    }
                }

                var overlappingLunch1 = requests.Value.Where(r => Overlap(r.StartTime, r.LengthInHalfHours, 7, 2)).ToList();
                var overlappingLunch2 = requests.Value.Where(r => Overlap(r.StartTime, r.LengthInHalfHours, 8, 2)).ToList();
                var overlappingLunch3 = requests.Value.Where(r => Overlap(r.StartTime, r.LengthInHalfHours, 9, 2)).ToList();

                if (overlappingLunch1.Count != 0 && overlappingLunch2.Count != 0 && overlappingLunch3.Count != 0)
                {
                    var missedLunch = true;
                    if (overlappingLunch1.All(m => conflictsByDay[requests.Key].ContainsKey(m) && !Overlap(conflictsByDay[requests.Key][m].StartTime, conflictsByDay[requests.Key][m].LengthInHalfHours, 7, 2)))
                    {
                        missedLunch = false;
                    }
                    if (overlappingLunch2.All(m => conflictsByDay[requests.Key].ContainsKey(m) && !Overlap(conflictsByDay[requests.Key][m].StartTime, conflictsByDay[requests.Key][m].LengthInHalfHours, 8, 2)))
                    {
                        missedLunch = false;
                    }
                    if (overlappingLunch3.All(m => conflictsByDay[requests.Key].ContainsKey(m) && !Overlap(conflictsByDay[requests.Key][m].StartTime, conflictsByDay[requests.Key][m].LengthInHalfHours, 9, 2)))
                    {
                        missedLunch = false;
                    }

                    if (missedLunch)
                    {
                        lunchesMissed++;
                    }
                }
            }

            
            return $"Week {week}: Conflicts: {conflictsByDay.Sum(kvp => kvp.Value.Count/2)}, Lunches missed: {lunchesMissed}";
        }

        private static bool Overlap(int start1, int length1, int start2, int length2)
        {
            return (start1 < (start2 + length2))
                   && (start2 < (start1 + length1));
        }

        public class MeetingRequest
        {
            [JsonProperty("day")]
            public int Day { get; set; }

            [JsonProperty("startTime")]
            public int StartTime { get; set; }

            [JsonProperty("lengthInHalfHours")]
            public int LengthInHalfHours { get; set; }
        }

        public class Requests
        {
            [JsonProperty("meetingRequests")]
            public List<MeetingRequest> MeetingRequests { get; set; }
        }
    }
    
    
}