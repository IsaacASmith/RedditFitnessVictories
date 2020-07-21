using CsvHelper;
using FitnessVictoryCollector.DataModels;
using FitnessVictoryCollector.ExportModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace FitnessVictoryCollector
{
    public class PostWriter
    {
        private string _outputPath;
        public PostWriter(string outputPath)
        {
            _outputPath = outputPath;
        }

        public void SaveAggregatePosts(Dictionary<int, int> currYearVals, Dictionary<int, int> pastVals)
        {
            var exportModels = new List<VictoryExport>();

            for (int i = 1; i < 53; i++)
            {
                exportModels.Add(new VictoryExport
                {
                    Week = i,
                    PastVictoryCount = pastVals.ContainsKey(i) ? pastVals[i] : 0,
                    CurrYearVictoryCount = currYearVals.ContainsKey(i) ? currYearVals[i] : 0
                });
            }

            using var writer = new StreamWriter($"{_outputPath}/aggregate-victories.csv");
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(exportModels);
        }

        public void SaveAllPosts(IEnumerable<VictoryPost> allPosts)
        {
            using var writer = new StreamWriter($"{_outputPath}/all-victories.csv");
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(allPosts);
        }
    }
}
