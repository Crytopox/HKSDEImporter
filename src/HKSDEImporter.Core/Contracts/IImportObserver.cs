using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface IImportObserver
{
    void OnStarted(ImportContext context);
    void OnStepStarted(string stepName, long totalCount);
    void OnStepProgress(string stepName, long processedCount);
    void OnWarning(string warning);
    void OnError(string error);
    void OnCompleted(ImportContext context);
}
