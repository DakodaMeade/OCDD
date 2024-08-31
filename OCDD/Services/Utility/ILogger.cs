/*
 * Dakoda Meade
 * Logger interface class
 * not currently being used
 * 
 */
namespace OCDD.Services.Utility
{
	public interface ILogger
	{
		void Debug(string message);
		void Error(string message);
		void Warning(string message);
		void Info(string message);
	}
}
