using System.Collections.Generic;
using System.Threading.Tasks;
using WebClienteMVC.WCF_Apl_Dis;

namespace WebClienteMVC.Datos
{

    public class PlatosDatos
    {
        private readonly Service1Client servicio = new Service1Client();

        public async Task<List<Cls_Platos>> ListarPlatosAsync()
        {
            var lista = await servicio.Get_PlatosAsync();
            return new List<Cls_Platos>(lista);
        }
    }
}
