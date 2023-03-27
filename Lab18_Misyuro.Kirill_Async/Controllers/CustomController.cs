using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Lab18_Misyuro.Kirill_Async.Controllers;

public class CustomController : Controller
{
    private CancellationTokenSource _cancelTokenSource;
    private CancellationToken _token;
    private Task task;

    // GET
    public IActionResult Index()
    {
        return View();
    }

    public RedirectToActionResult Start()
    {
        _cancelTokenSource = new CancellationTokenSource();
        _token = _cancelTokenSource.Token;
        task = Task.Run((() => Loop()), _token);
        return RedirectToAction("Index");
    }

    public StatusCodeResult Stop()
    {
        _cancelTokenSource.Cancel();
        while (task.Status != TaskStatus.RanToCompletion){} ;
        _cancelTokenSource.Dispose();
        return StatusCode(299);
    }

    public async Task Loop()
    {
        var i = 0;
        while (!_token.IsCancellationRequested)
        {
            i++;
            await Task.Delay(100);
        }
    }
}