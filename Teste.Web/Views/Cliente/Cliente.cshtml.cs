using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Teste.Web.Views.Cliente
{
    public class Cliente : PageModel
    {
        private readonly ILogger<Cliente> _logger;

        public Cliente(ILogger<Cliente> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}