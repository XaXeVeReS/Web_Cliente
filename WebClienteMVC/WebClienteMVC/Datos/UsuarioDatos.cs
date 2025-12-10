using System;
using System.ServiceModel;
using System.Threading.Tasks;
using WebClienteMVC.WCF_Apl_Dis;

public class UsuarioDatos
{
    private readonly Service1Client servicio = new Service1Client();

    // LOGIN
    public async Task<int> LoginAsync(string user, string pass)
    {
        try
        {
            return await servicio.Login_UserAsync(user, pass);
        }
        catch (FaultException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    // REGISTRO
    public async Task<string> RegistrarAsync(Cls_Usuarios u)
    {
        try
        {
            await servicio.Insert_UserAsync(u);
            return null;
        }
        catch (FaultException ex)
        {
            return ex.Message;
        }
    }

    // OLVIDÉ MI CONTRASEÑA
    public async Task<string> EnviarTokenAsync(string email, string usuario)
    {
        try
        {
            await servicio.Send_TokenAsync(email, usuario);
            return null;
        }
        catch (FaultException ex)
        {
            return ex.Message;
        }
    }

    // CAMBIAR CONTRASEÑA
    public async Task<string> CambiarPasswordAsync(string email, string token, string pass)
    {
        try
        {
            await servicio.Update_PasswordAsync(email, token, pass);
            return null;
        }
        catch (FaultException ex)
        {
            return ex.Message;
        }
    }
}
