namespace EMS.FrontEnd.Services {
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;
    using CsvHelper;

    public class ExportService : ICsvExportService {
        public Task ExportAsync(string exportPath, IEnumerable records, string delimiter = null) {
            using var writer = new StreamWriter(exportPath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            if (!string.IsNullOrWhiteSpace(delimiter))
                csv.Configuration.Delimiter = delimiter;
            return csv.WriteRecordsAsync(records);
        }
    }
}