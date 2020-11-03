namespace EMS.FrontEnd.Services {
    using System.Collections;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ICsvExportService {
        Task ExportAsync(string exportPath, IEnumerable records, string delimiter = null);
    }
}