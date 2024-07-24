/*
 * Dakoda Meade
 * CST-350
 * Activity 6-2
 * Logger interface class
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
