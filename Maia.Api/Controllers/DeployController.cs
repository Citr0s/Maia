using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Maia.Api.Controllers
{
    [Route("api/[controller]")]
    public class DeployController : Controller
    {
        [HttpGet("{repositoryName}")]
        public ActionResult Post(string repositoryName)
        {
            var allowedApps = new List<string>
            {
                "maia"
            };

            Task.Run(() =>
                ShellHelper.Bash($"cd /var/www/{repositoryName} && sudo bash deploy.sh 2>&1")
            );
            return Ok();
        }

        private static class ShellHelper
        {
            public static string Bash(string cmd)
            {
                var escapedArgs = cmd.Replace("\"", "\\\"");

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        Arguments = $"-c \"{escapedArgs}\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                var result = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return result;
            }
        }
    }
}