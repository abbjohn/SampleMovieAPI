using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using SampleMovieApi.Models;

namespace SampleMovieApi.Services
{
    public class Database
    {
        private const string METADATA_FILEPATH = "./Data/metadata.csv";

        public Database()
        {
            MetaData = new SynchronizedCollection<MetaDataModel>();

            using var reader = new StreamReader(METADATA_FILEPATH);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<MetaDataModel>();
            foreach (var record in records)
            {
                MetaData.Add(record);
            }
        }

        public SynchronizedCollection<MetaDataModel> MetaData { get; set; }
    }
}
