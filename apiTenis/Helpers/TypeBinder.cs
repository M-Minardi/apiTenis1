using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace apiTenis.Helpers
{
    //model binder custom, para que en productoDTO podamos recibir array de int. (por un FormFrom)
    public class TypeBinder<T>: IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var nombrePropiedad = bindingContext.ModelName;
            var proveedorDelValores = bindingContext.ValueProvider.GetValue(nombrePropiedad);
            if (proveedorDelValores == ValueProviderResult.None || string.IsNullOrWhiteSpace(proveedorDelValores.FirstValue))
                return Task.CompletedTask;
            try
            {
                var valorDeserializado = JsonConvert.DeserializeObject<T>(proveedorDelValores.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(valorDeserializado);
            }catch 
            {
                bindingContext.ModelState.TryAddModelError(nombrePropiedad, "Valor inválido para tipo List<int>");
            }
            return Task.CompletedTask;
        }
    }
}
