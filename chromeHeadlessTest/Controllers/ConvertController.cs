using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace chromeHeadlessTest.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ConvertController : ControllerBase {
        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get(string url) {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions {
                Headless = true,
            });
            var page = await browser.NewPageAsync();
            await page.GoToAsync(url);

            await page.EvaluateExpressionAsync(@"
                for(var ele of document.querySelectorAll('.pagination')){
                    ele.classList.remove('pagination');
                }");

            return File(await page.PdfStreamAsync(new PdfOptions() {
                Format = PaperFormat.A4
            }), "application/pdf");
        }

    }
}
