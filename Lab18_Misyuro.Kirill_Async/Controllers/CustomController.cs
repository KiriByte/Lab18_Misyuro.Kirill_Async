using System.Net;
using Lab18_Misyuro.Kirill_Async.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab18_Misyuro.Kirill_Async.Controllers;

public class CustomController : Controller
{
    private CancellationTokenSource _cancelTokenSource;
    private CancellationToken _token;
    Task task1 = null;
    Task task2 = null;
    private CustomModel _customModel = new CustomModel();

    // GET
    public IActionResult Index()
    {
        return View(_customModel);
    }

    public async Task<RedirectToActionResult> Start()
    {
        _cancelTokenSource = new CancellationTokenSource();
        _token = _cancelTokenSource.Token;
        task1 = Task.Run(() => FirstTask(), _token);
        task2 = Task.Run(() => SecondTask(), _token);


        return RedirectToAction("Index");
    }

    public StatusCodeResult Stop()
    {
        _cancelTokenSource.Cancel();
        Task.WhenAll(task1, task2).Wait();
        return StatusCode(299);
    }

    public async Task FirstTask()
    {
        while (!_token.IsCancellationRequested)
        {
            _customModel.i++;
            await Task.Delay(1000);
        }
    }


    public async Task SecondTask()
    {
        while (!_token.IsCancellationRequested)
        {
            _customModel.Counter++;
            await Task.Delay(1);
        }
    }
}