namespace InTechWebApp.Hubs.Models
{
	public class HubResponseModel
	{
		public HubResponseStatus Statuscode { get; set; }
		public string Message { get; set; }
	}

	public enum HubResponseStatus
	{
		UnknownError = 0,
		SessionExpired = 100
	}
}