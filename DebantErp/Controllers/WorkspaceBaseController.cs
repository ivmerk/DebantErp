using Microsoft.AspNetCore.Mvc;

namespace DebantErp.Controllers;

// Общий предок для разделов рабочей области (/workspace/...).
// Держит то, что одинаково у всех справочников: размер страницы пагинации.
public abstract class WorkspaceBaseController : Controller
{
    protected const int PageSize = 20;
}
