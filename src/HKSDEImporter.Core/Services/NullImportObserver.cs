using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Services;

public sealed class NullImportObserver : IImportObserver
{
    public void OnStarted(ImportContext context)
    {
    }

    public void OnStepStarted(string stepName, long totalCount)
    {
    }

    public void OnStepProgress(string stepName, long processedCount)
    {
    }

    public void OnWarning(string warning)
    {
    }

    public void OnError(string error)
    {
    }

    public void OnCompleted(ImportContext context)
    {
    }
}
