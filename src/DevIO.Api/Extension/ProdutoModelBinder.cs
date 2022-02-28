using DevIO.Api.Interop.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace DevIO.Api.Extension
{
    // utilizado para a deserialização de um IFormFile
    public class ProdutoModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) throw new ArgumentException(nameof(bindingContext));

            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                PropertyNameCaseInsensitive = true
            };

            var produtoImagemViewModel = JsonSerializer.Deserialize<ProdutoImagemViewModel>(bindingContext.ValueProvider.GetValue("produto").FirstOrDefault(), serializeOptions);
            produtoImagemViewModel.ImagemUpload = bindingContext.ActionContext.HttpContext.Request.Form.Files.FirstOrDefault();

            bindingContext.Result = ModelBindingResult.Success(produtoImagemViewModel);
            return Task.CompletedTask;
        }
    }
}
